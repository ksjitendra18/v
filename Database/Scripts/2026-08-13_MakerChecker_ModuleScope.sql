/*
    Module-Scoped Maker-Checker
    =================================================================================
    Database : VigilanceMISDB
    Requires : 2026-07-27_CaseApproval_Central_MakerChecker_Foundation.sql

    Problem
    -------
    Checker authorisation had no module dimension. MakerCheckerMapping is keyed on
    (UserPF, ZoneSolID) only, so a checker mapped to a zone automatically checked
    EVERY module in that zone -- IAC, Vigilance, Complaint and anything added later.
    There was no way to say "this person checks Vigilance and IAC but not Complaint".

    Design (see Docs/VMIS_ModuleScoped_MakerChecker_Plan.md sec 4)
    -------------------------------------------------------------
    Modules are assigned to a CHECKER GROUP; a checker is granted a group in a zone.

        WORKFLOW_MODULE_GROUP     VIG = Vigilance + IAC,  CMP = Complaint + MISC
        WORKFLOW_MODULE.GroupCode which group owns this module
        MakerCheckerMapping.GroupCode  which group this grant covers

    Grouping rather than a raw ModuleCode on the mapping because the requirement is
    about modules moving together. A module needing its own distinct checkers just
    gets a single-module group, so this expresses everything a per-module key would.

    Every authorisation and visibility check resolves through ONE inline function,
    dbo.fnCheckerScope, so a future re-grouping cannot be applied to five of six
    call sites.

    Objects created
    ---------------
      dbo.WORKFLOW_MODULE_GROUP   the groups
      dbo.fnCheckerScope          (ModuleCode, ZoneSolID) a user may check
      dbo.spCheckerGroup_Ddl      admin screen: bind the group list
      dbo.spCheckerScope_Get      admin screen: read a user's current grants
      dbo.spCheckerScope_Save     admin screen: save grants, race-free MERGE

    Objects repointed at fnCheckerScope
    -----------------------------------
      dbo.spCase_CheckerAction        guard 6 (authorisation)
      dbo.spCase_CheckerQueue         inbox
      dbo.vw_CASE_APPROVAL_ORPHANS    monitor -- now module-aware
      dbo.spComplaint_CheckerAction   Complaint's own proc (still on the old mechanism)

    Migration behaviour
    -------------------
    Existing active checker rows are FANNED OUT to one row per active group, so
    nobody's access changes at deployment. Narrowing a user is then a deliberate,
    audited admin action on Admin/UserCreation.aspx. After this script, an active
    checker row with a NULL GroupCode grants nothing -- the model fails closed.

    Safe to re-run.
*/

SET NOCOUNT ON;
GO

-- These two are captured into the metadata of every module created below and apply whenever it
-- runs, whatever the caller's session settings. They MUST be ON: SqlClient connects with
-- QUOTED_IDENTIFIER ON, and a module created with it OFF fails at runtime against indexed views,
-- filtered indexes, indexes on computed columns and XML data type methods.
-- sqlcmd defaults QUOTED_IDENTIFIER to OFF, so deploying this file without these lines (or
-- without sqlcmd's -I switch) creates objects that break when the application calls them.
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

-------------------------------------------------------------------------------------------------
-- 1. dbo.WORKFLOW_MODULE_GROUP  -- the checker groups
-------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.WORKFLOW_MODULE_GROUP') IS NULL
BEGIN
    CREATE TABLE dbo.WORKFLOW_MODULE_GROUP
    (
        GroupCode  varchar(20)  NOT NULL CONSTRAINT PK_WORKFLOW_MODULE_GROUP PRIMARY KEY,
        GroupName  varchar(100) NOT NULL,   -- the only label an admin ever sees
        IsActive   bit          NOT NULL CONSTRAINT DF_WORKFLOW_MODULE_GROUP_Active DEFAULT(1)
    );
END
GO

MERGE dbo.WORKFLOW_MODULE_GROUP AS T
USING (VALUES
        ('VIG', 'Vigilance & IAC',   1),
        ('CMP', 'Complaint & MISC',  1)
      ) AS S (GroupCode, GroupName, IsActive)
   ON T.GroupCode = S.GroupCode
WHEN MATCHED THEN UPDATE SET T.GroupName = S.GroupName
WHEN NOT MATCHED THEN
    INSERT (GroupCode, GroupName, IsActive) VALUES (S.GroupCode, S.GroupName, S.IsActive);
GO

-------------------------------------------------------------------------------------------------
-- 2. dbo.WORKFLOW_MODULE.GroupCode  -- which group owns each module
--
--    Nullable while the UPDATEs below run, then NOT NULL. A module with no group can
--    be checked by nobody, which is the silent-limbo failure this design exists to
--    prevent, so the constraint is what stops one being registered by accident.
-------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('dbo.WORKFLOW_MODULE') AND name = 'GroupCode')
    ALTER TABLE dbo.WORKFLOW_MODULE ADD GroupCode varchar(20) NULL;
GO

UPDATE dbo.WORKFLOW_MODULE SET GroupCode = 'VIG' WHERE ModuleCode IN ('IAC','VIGILANCE');
UPDATE dbo.WORKFLOW_MODULE SET GroupCode = 'CMP' WHERE ModuleCode IN ('COMPLAINT','MISC');
GO

-- COMPLAINT is registered here as a REGISTRY ROW ONLY. Its approval data still lives in
-- the six inline columns on COMPLAINT, not in CASE_APPROVAL, and this script does not
-- migrate it. The row exists so that fnCheckerScope can return 'COMPLAINT' and the two
-- Complaint pages can be module-scoped like everything else. Because COMPLAINT has no
-- CASE_APPROVAL rows, registering it changes no count and no queue.
IF NOT EXISTS (SELECT 1 FROM dbo.WORKFLOW_MODULE WHERE ModuleCode = 'COMPLAINT')
    INSERT INTO dbo.WORKFLOW_MODULE
        (ModuleCode, ModuleName, TableName, KeyColumn, RefColumn, ZoneColumn, ViewPage, IsActive, GroupCode)
    VALUES
        ('COMPLAINT', 'Complaint', 'COMPLAINT', 'CODE', 'RNO', 'NEWZONE',
         '~/Mis/frmComplaintCheckerView.aspx', 1, 'CMP');
GO

-- Any module registered before this script that is still ungrouped gets its own
-- single-module group, so the NOT NULL below cannot fail and nobody silently loses a queue.
INSERT INTO dbo.WORKFLOW_MODULE_GROUP (GroupCode, GroupName, IsActive)
SELECT  LEFT(WM.ModuleCode, 20), WM.ModuleName, 1
FROM    dbo.WORKFLOW_MODULE WM
WHERE   WM.GroupCode IS NULL
  AND   NOT EXISTS (SELECT 1 FROM dbo.WORKFLOW_MODULE_GROUP G WHERE G.GroupCode = LEFT(WM.ModuleCode, 20));
GO

UPDATE dbo.WORKFLOW_MODULE SET GroupCode = LEFT(ModuleCode, 20) WHERE GroupCode IS NULL;
GO

IF EXISTS (SELECT 1 FROM sys.columns
           WHERE object_id = OBJECT_ID('dbo.WORKFLOW_MODULE') AND name = 'GroupCode' AND is_nullable = 1)
    ALTER TABLE dbo.WORKFLOW_MODULE ALTER COLUMN GroupCode varchar(20) NOT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_WORKFLOW_MODULE_GROUP')
    ALTER TABLE dbo.WORKFLOW_MODULE
        ADD CONSTRAINT FK_WORKFLOW_MODULE_GROUP
            FOREIGN KEY (GroupCode) REFERENCES dbo.WORKFLOW_MODULE_GROUP(GroupCode);
GO

-------------------------------------------------------------------------------------------------
-- 3. dbo.MakerCheckerMapping.GroupCode  -- which group a grant covers
-------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('dbo.MakerCheckerMapping') AND name = 'GroupCode')
    ALTER TABLE dbo.MakerCheckerMapping ADD GroupCode varchar(20) NULL;
GO

-- 3a. Retire the old unique key FIRST. It is (UserPF, ZoneSolID), which allows only one
--     grant per user per zone -- the fan-out below would violate it on its first row.
IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'UQ_MakerCheckerMapping')
    ALTER TABLE dbo.MakerCheckerMapping DROP CONSTRAINT UQ_MakerCheckerMapping;
GO

-- 3b. Fan out every existing active checker grant to one row per active group.
--     Deployment therefore changes nobody's access: a checker who could see every
--     module yesterday still can, but the grant is now explicit and visible in the
--     admin screen instead of implied by the absence of a filter.
IF EXISTS (SELECT 1 FROM dbo.MakerCheckerMapping WHERE GroupCode IS NULL AND IsChecker = 1 AND IsActive = 1)
BEGIN
    INSERT INTO dbo.MakerCheckerMapping
        (UserId, UserPF, ZoneSolID, GroupCode, IsMaker, IsChecker, IsActive, CreatedBy)
    SELECT  M.UserId, M.UserPF, M.ZoneSolID, G.GroupCode, M.IsMaker, 1, 1, 'MODULE_SCOPE_MIGRATION'
    FROM    dbo.MakerCheckerMapping M
            CROSS JOIN dbo.WORKFLOW_MODULE_GROUP G
    WHERE   M.IsChecker = 1
      AND   M.IsActive  = 1
      AND   M.GroupCode IS NULL
      AND   G.IsActive  = 1
      AND   NOT EXISTS (SELECT 1 FROM dbo.MakerCheckerMapping X
                        WHERE X.UserPF = M.UserPF AND X.ZoneSolID = M.ZoneSolID
                          AND X.GroupCode = G.GroupCode);

    -- The old ungrouped rows are retired, not deleted -- they are the audit of what
    -- access looked like before this change.
    UPDATE dbo.MakerCheckerMapping
       SET IsActive = 0
     WHERE GroupCode IS NULL AND IsChecker = 1 AND IsActive = 1;
END
GO

-- 3c. The replacement unique key carries the group.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_MakerCheckerMapping_Scope'
                                           AND object_id = OBJECT_ID('dbo.MakerCheckerMapping'))
   AND NOT EXISTS (SELECT UserPF, ZoneSolID, GroupCode FROM dbo.MakerCheckerMapping
                   GROUP BY UserPF, ZoneSolID, GroupCode HAVING COUNT(*) > 1)
    -- A unique INDEX rather than a constraint: GroupCode is nullable on the retired
    -- pre-migration rows, and a unique index treats NULLs as comparable, which is
    -- what is wanted here (at most one retired ungrouped row per user+zone).
    CREATE UNIQUE INDEX UQ_MakerCheckerMapping_Scope
        ON dbo.MakerCheckerMapping (UserPF, ZoneSolID, GroupCode);
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_MakerCheckerMapping_Lookup'
                                       AND object_id = OBJECT_ID('dbo.MakerCheckerMapping'))
    DROP INDEX IX_MakerCheckerMapping_Lookup ON dbo.MakerCheckerMapping;
GO

CREATE INDEX IX_MakerCheckerMapping_Lookup
    ON dbo.MakerCheckerMapping (UserPF, IsChecker, IsActive) INCLUDE (ZoneSolID, GroupCode);
GO

-------------------------------------------------------------------------------------------------
-- 4. dbo.fnCheckerScope  -- the single resolution point
--
--    Returns every (module, zone) pair this user may check. Inline TVF, so it folds
--    into the caller's plan at no cost over the hand-written join it replaces.
--
--    Every authorisation and visibility check in the application resolves through this
--    function. That is the point of it: when a module moves group, or a group is
--    deactivated, there is one place for that to take effect rather than six.
-------------------------------------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fnCheckerScope (@p_USER varchar(50))
RETURNS TABLE
AS
RETURN
    SELECT  WM.ModuleCode,
            M.ZoneSolID,
            WM.GroupCode
    FROM    dbo.MakerCheckerMapping M
            INNER JOIN dbo.WORKFLOW_MODULE_GROUP G
                    ON G.GroupCode = M.GroupCode
                   AND G.IsActive  = 1
            INNER JOIN dbo.WORKFLOW_MODULE WM
                    ON WM.GroupCode = G.GroupCode
                   AND WM.IsActive  = 1
    WHERE   M.UserPF    = @p_USER
      AND   M.IsChecker = 1
      AND   M.IsActive  = 1;
GO

-------------------------------------------------------------------------------------------------
-- 5. dbo.vw_CASE_APPROVAL_ORPHANS  -- monitor, now module-aware
--
--    Once grants are module-scoped a record can be pending in a zone that HAS checkers,
--    just not for its module. That record is invisible to every inbox and locked from
--    editing by the maker. The old zone-only view reported it as healthy.
-------------------------------------------------------------------------------------------------
CREATE OR ALTER VIEW dbo.vw_CASE_APPROVAL_ORPHANS
AS
    SELECT  CA.ApprovalId,
            CA.ModuleCode,
            CA.RecordCode,
            CA.RecordRef,
            CA.ZoneSolID,
            CA.MakerUser,
            CA.MakerDate,
            CASE WHEN CA.ZoneSolID IS NULL OR LTRIM(RTRIM(CA.ZoneSolID)) = ''
                 THEN 'No zone recorded on the case'
                 WHEN NOT EXISTS (SELECT 1 FROM dbo.MakerCheckerMapping M
                                  WHERE M.ZoneSolID = CA.ZoneSolID
                                    AND M.IsChecker = 1 AND M.IsActive = 1)
                 THEN 'No active checker mapped to this zone'
                 ELSE 'This zone has checkers, but none for this module''s group'
            END AS OrphanReason
    FROM    dbo.CASE_APPROVAL CA
    WHERE   CA.ApprovalStatus = 'P'
      AND   NOT EXISTS (SELECT 1
                        FROM   dbo.MakerCheckerMapping M
                               INNER JOIN dbo.WORKFLOW_MODULE_GROUP G
                                       ON G.GroupCode = M.GroupCode AND G.IsActive = 1
                               INNER JOIN dbo.WORKFLOW_MODULE WM
                                       ON WM.GroupCode = G.GroupCode AND WM.IsActive = 1
                        WHERE  M.ZoneSolID  = CA.ZoneSolID
                          AND  M.IsChecker  = 1
                          AND  M.IsActive   = 1
                          AND  WM.ModuleCode = CA.ModuleCode);
GO

-------------------------------------------------------------------------------------------------
-- 6. dbo.spCase_CheckerAction  -- guard 6 is now module-scoped
--    (full body below is the complete, current definition -- not a diff)
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spCase_CheckerAction]
(
    @p_MODULE   VARCHAR(20),
    @p_CODE     BIGINT,
    @p_ACTION   CHAR(1),          -- 'A' = Approve, 'X' = Reject, 'C' = Push Back for correction
    @p_REMARKS  VARCHAR(MAX),
    @p_USER     VARCHAR(50),
    @p_USERROLE VARCHAR(50),
    @p_USERIP   VARCHAR(30),
    @o_EERMSG   VARCHAR(MAX) OUTPUT,
    @o_ERRCODE  INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET @o_ERRCODE = 0;

    IF (@p_ACTION NOT IN ('A','X','C'))
    BEGIN
        SET @o_EERMSG = 'Invalid action.';
        RETURN;
    END

    IF (ISNULL(@p_REMARKS,'') = '')
    BEGIN
        SET @o_EERMSG = 'Checker remarks are mandatory.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.WORKFLOW_MODULE WHERE ModuleCode = @p_MODULE AND IsActive = 1)
    BEGIN
        SET @o_EERMSG = 'This module is not registered for maker-checker.';
        RETURN;
    END

    DECLARE @ZONE VARCHAR(10), @STATUS CHAR(1), @MAKER VARCHAR(50);

    SELECT  @ZONE   = ZoneSolID,
            @STATUS = ApprovalStatus,
            @MAKER  = MakerUser
    FROM    dbo.CASE_APPROVAL
    WHERE   ModuleCode = @p_MODULE AND RecordCode = @p_CODE;

    IF (@STATUS IS NULL)
    BEGIN
        SET @o_EERMSG = 'Record not found in the approval queue.';
        RETURN;
    END

    -- Maker and checker must be different people. VMIS_CHECKER is a secondary role, so a
    -- user can legitimately hold VMIS_MISUSER and VMIS_CHECKER at the same time -- without
    -- this check that user could approve their own entry, which defeats the whole control.
    -- (For single-account UAT, comment out this block only.)
    IF (@MAKER IS NOT NULL AND UPPER(LTRIM(RTRIM(@MAKER))) = UPPER(LTRIM(RTRIM(@p_USER))))
    BEGIN
        SET @o_EERMSG = 'Maker and checker cannot be the same user.';
        RETURN;
    END

    -- MODULE SCOPE: the caller must be an active checker for this record's zone AND for
    -- the group that owns this module. Resolved through fnCheckerScope so this rule has
    -- exactly one definition across the whole application.
    IF NOT EXISTS (SELECT 1 FROM dbo.fnCheckerScope(@p_USER) S
                   WHERE S.ModuleCode = @p_MODULE
                     AND S.ZoneSolID  = @ZONE)
    BEGIN
        SET @o_EERMSG = 'You are not authorized to act on this record.';
        RETURN;
    END

    IF (@STATUS <> 'P')
    BEGIN
        SET @o_EERMSG = 'This record has already been actioned and is no longer pending.';
        RETURN;
    END

    BEGIN TRY
        BEGIN TRAN;

            UPDATE dbo.CASE_APPROVAL
               SET ApprovalStatus = @p_ACTION,
                   CheckerUser    = @p_USER,
                   CheckerDate    = GETDATE(),
                   CheckerRemarks = @p_REMARKS
             WHERE ModuleCode = @p_MODULE
               AND RecordCode = @p_CODE
               AND ApprovalStatus = 'P';   -- re-check under the transaction

            IF (@@ROWCOUNT = 0)
            BEGIN
                ROLLBACK TRAN;
                SET @o_EERMSG = 'This record has already been actioned and is no longer pending.';
                RETURN;
            END

            INSERT INTO dbo.CASE_APPROVAL_HISTORY
                (ModuleCode, RecordCode, ActionType, ActionBy, Remarks, UserRole, UserIP)
            VALUES
                (@p_MODULE, @p_CODE,
                 CASE @p_ACTION WHEN 'A' THEN 'APPROVED' WHEN 'X' THEN 'REJECTED' ELSE 'PUSHED_BACK' END,
                 @p_USER, @p_REMARKS, @p_USERROLE, @p_USERIP);

        COMMIT TRAN;

        SET @o_ERRCODE = 1;
        SET @o_EERMSG  = 'Action recorded successfully.';
    END TRY
    BEGIN CATCH
        IF (XACT_STATE() <> 0) ROLLBACK TRAN;
        SET @o_ERRCODE = 0;
        SET @o_EERMSG  = 'Could not record the action: ' + ERROR_MESSAGE();
    END CATCH
END
GO

-------------------------------------------------------------------------------------------------
-- 7. dbo.spCase_CheckerQueue  -- inbox, now module-scoped
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spCase_CheckerQueue]
(
    @p_USER   VARCHAR(50),
    @p_MODULE VARCHAR(20) = NULL,   -- NULL = every module this checker is granted
    @p_STATUS CHAR(1)     = 'P'     -- NULL = every status
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  CA.ApprovalId,
            CA.ModuleCode,
            WM.ModuleName,
            WM.ViewPage,
            CA.RecordCode,
            CA.RecordRef,
            CA.ZoneSolID,
            CA.ApprovalStatus,
            CASE CA.ApprovalStatus
                WHEN 'P' THEN 'Pending Approval'
                WHEN 'A' THEN 'Approved'
                WHEN 'C' THEN 'Changes Requested'
                WHEN 'X' THEN 'Rejected'
            END AS ApprovalStatusText,
            CA.MakerUser,
            CA.MakerDate,
            CA.CheckerUser,
            CA.CheckerDate,
            CA.CheckerRemarks
    FROM    dbo.CASE_APPROVAL CA
            INNER JOIN dbo.WORKFLOW_MODULE WM
                    ON WM.ModuleCode = CA.ModuleCode
                   AND WM.IsActive   = 1
            -- Module scope: the grant must cover this record's module AND its zone.
            INNER JOIN dbo.fnCheckerScope(@p_USER) S
                    ON S.ModuleCode = CA.ModuleCode
                   AND S.ZoneSolID  = CA.ZoneSolID
    WHERE   (@p_MODULE IS NULL OR CA.ModuleCode     = @p_MODULE)
      AND   (@p_STATUS IS NULL OR CA.ApprovalStatus = @p_STATUS)
    ORDER BY CA.MakerDate DESC;
END
GO

-------------------------------------------------------------------------------------------------
-- 8. dbo.spComplaint_CheckerAction  -- Complaint is still on the inline-columns mechanism,
--    so its own action proc needs the same scoping. Only the authorisation check changed.
--    (full body below is the complete, current definition -- not a diff)
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spComplaint_CheckerAction]
(
    @p_RNO          VARCHAR(50),
    @p_ACTION       CHAR(1),        -- 'A' = Approve, 'X' = Reject, 'C' = Push Back (correction)
    @p_REMARKS      VARCHAR(MAX),
    @p_USER         VARCHAR(50),
    @p_USERROLE     VARCHAR(50),
    @p_USERIP       VARCHAR(20),
    @o_EERMSG       VARCHAR(MAX) OUTPUT,
    @o_ERRCODE      INT OUTPUT
)
AS
BEGIN
    SET @o_ERRCODE = 0;

    IF (@p_ACTION NOT IN ('A','X','C'))
    BEGIN
        SET @o_EERMSG = 'Invalid action.';
        RETURN;
    END

    IF (ISNULL(@p_REMARKS,'') = '')
    BEGIN
        SET @o_EERMSG = 'Checker remarks are mandatory.';
        RETURN;
    END

    DECLARE @CODE BIGINT, @NEWZONE VARCHAR(10);

    SELECT @CODE = CODE, @NEWZONE = NEWZONE
    FROM COMPLAINT
    WHERE RNO = @p_RNO AND ACTIVE = 'Y';

    IF (@CODE IS NULL)
    BEGIN
        SET @o_EERMSG = 'Complaint not found.';
        RETURN;
    END

    -- MODULE SCOPE: same rule and same resolution point as every other module.
    IF NOT EXISTS (SELECT 1 FROM dbo.fnCheckerScope(@p_USER) S
                   WHERE S.ModuleCode = 'COMPLAINT'
                     AND S.ZoneSolID  = @NEWZONE)
    BEGIN
        SET @o_EERMSG = 'You are not authorized to act on this complaint.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM COMPLAINT WHERE CODE = @CODE AND APPROVALSTATUS = 'P')
    BEGIN
        SET @o_EERMSG = 'This complaint has already been actioned and is no longer pending.';
        RETURN;
    END

    INSERT INTO COMPLAINT_HISTORY SELECT * FROM COMPLAINT WHERE CODE = @CODE;

    UPDATE COMPLAINT
    SET APPROVALSTATUS = @p_ACTION,
        CHECKERUSER = @p_USER,
        CHECKERDATE = GETDATE(),
        CHECKERREMARKS = @p_REMARKS
    WHERE CODE = @CODE;

    INSERT INTO COMPLAINT_APPROVAL_HISTORY
    (
        COMPLAINTCODE,
        ACTIONTYPE,
        ACTIONBY,
        REMARKS,
        USERROLE,
        USERIP
    )
    VALUES
    (
        @CODE,
        CASE @p_ACTION WHEN 'A' THEN 'APPROVED' WHEN 'X' THEN 'REJECTED' WHEN 'C' THEN 'PUSHED_BACK' END,
        @p_USER,
        @p_REMARKS,
        @p_USERROLE,
        @p_USERIP
    );

    SET @o_ERRCODE = 1;
    SET @o_EERMSG = 'Action recorded successfully.';
END
GO

-------------------------------------------------------------------------------------------------
-- 9. Admin screen support -- Admin/UserCreation.aspx
-------------------------------------------------------------------------------------------------

-- 9a. Bind the group checkbox list.
CREATE OR ALTER PROC [dbo].[spCheckerGroup_Ddl]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  G.GroupCode,
            G.GroupName,
            -- Shown next to the group name so an admin can see what they are granting
            -- without having to know the module registry.
            -- STRING_AGG rather than the FOR XML PATH trick: the XML form uses an XML data type
            -- method, which requires QUOTED_IDENTIFIER ON at CREATE time and fails at runtime if
            -- the proc was deployed with it OFF. This has no such dependency.
            (SELECT STRING_AGG(WM.ModuleName, ', ') WITHIN GROUP (ORDER BY WM.ModuleName)
             FROM   dbo.WORKFLOW_MODULE WM
             WHERE  WM.GroupCode = G.GroupCode AND WM.IsActive = 1) AS Modules
    FROM    dbo.WORKFLOW_MODULE_GROUP G
    WHERE   G.IsActive = 1
    ORDER BY G.GroupName;
END
GO

-- 9b. Read a user's current grants, so the screen opens showing the truth.
CREATE OR ALTER PROC [dbo].[spCheckerScope_Get]
(
    @p_USERPF VARCHAR(10)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  M.GroupCode,
            M.ZoneSolID
    FROM    dbo.MakerCheckerMapping M
    WHERE   M.UserPF    = @p_USERPF
      AND   M.IsChecker = 1
      AND   M.IsActive  = 1
      AND   M.GroupCode IS NOT NULL;
END
GO

-- 9c. Save a user's grants as the group x zone cross product.
--
--     Replaces the read-then-write loop in Admin/UserCreation.aspx.cs, which fired
--     SELECT COUNT(*) then UPDATE-or-INSERT once per zone. That is a race -- two
--     concurrent saves produced duplicate mappings, which duplicated every inbox row.
--     One MERGE in one round trip, and the unique index makes the race impossible.
CREATE OR ALTER PROC [dbo].[spCheckerScope_Save]
(
    @p_USERPF    VARCHAR(10),
    @p_USERID    UNIQUEIDENTIFIER,
    @p_GROUPS    VARCHAR(MAX),      -- comma-separated GroupCodes
    @p_ZONES     VARCHAR(MAX),      -- comma-separated ZoneSolIDs
    @p_CREATEDBY VARCHAR(50),
    @o_EERMSG    VARCHAR(MAX) OUTPUT,
    @o_ERRCODE   INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET @o_ERRCODE = 0;

    BEGIN TRY
        BEGIN TRAN;

            ;WITH Scope AS
            (
                SELECT  LTRIM(RTRIM(G.value)) AS GroupCode,
                        LTRIM(RTRIM(Z.value)) AS ZoneSolID
                FROM    STRING_SPLIT(ISNULL(@p_GROUPS,''), ',') G
                        CROSS JOIN STRING_SPLIT(ISNULL(@p_ZONES,''), ',') Z
                WHERE   LTRIM(RTRIM(G.value)) <> ''
                  AND   LTRIM(RTRIM(Z.value)) <> ''
            )
            MERGE dbo.MakerCheckerMapping AS T
            USING Scope AS S
               ON T.UserPF = @p_USERPF
              AND T.ZoneSolID = S.ZoneSolID
              AND T.GroupCode = S.GroupCode
            WHEN MATCHED THEN UPDATE SET
                    T.IsChecker = 1,
                    T.IsMaker   = 0,
                    T.IsActive  = 1
            WHEN NOT MATCHED BY TARGET THEN
                INSERT (UserId, UserPF, ZoneSolID, GroupCode, IsMaker, IsChecker, IsActive, CreatedBy)
                VALUES (@p_USERID, @p_USERPF, S.ZoneSolID, S.GroupCode, 0, 1, 1, @p_CREATEDBY)
            -- Anything this user held that is not in the new selection is revoked.
            -- Deactivated rather than deleted: the row is the record of what was granted.
            WHEN NOT MATCHED BY SOURCE AND T.UserPF = @p_USERPF
                                      AND T.IsChecker = 1
                                      AND T.IsActive = 1
                THEN UPDATE SET T.IsActive = 0;

        COMMIT TRAN;

        SET @o_ERRCODE = 1;
        SET @o_EERMSG  = 'Checker scope saved.';
    END TRY
    BEGIN CATCH
        IF (XACT_STATE() <> 0) ROLLBACK TRAN;
        SET @o_ERRCODE = 0;
        SET @o_EERMSG  = 'Could not save the checker scope: ' + ERROR_MESSAGE();
    END CATCH
END
GO

-------------------------------------------------------------------------------------------------
-- 10. Post-deployment check -- both queries should look sane before you walk away
-------------------------------------------------------------------------------------------------
/*
-- Who checks what, after migration:
SELECT M.UserPF, M.ZoneSolID, M.GroupCode, G.GroupName
FROM   dbo.MakerCheckerMapping M
       LEFT JOIN dbo.WORKFLOW_MODULE_GROUP G ON G.GroupCode = M.GroupCode
WHERE  M.IsChecker = 1 AND M.IsActive = 1
ORDER  BY M.UserPF, M.GroupCode, M.ZoneSolID;

-- Nothing should be stranded:
SELECT * FROM dbo.vw_CASE_APPROVAL_ORPHANS;
*/
