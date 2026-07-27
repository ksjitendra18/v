/*
    Central Maker-Checker Foundation
    =================================================================================
    Database : VigilanceMISDB

    Purpose
    -------
    Replaces the "6 workflow columns on every case table" pattern (used by COMPLAINT)
    with a single central approval registry that any module can be registered against.

    Rationale (see Docs/VMIS_MakerChecker_Rollout_Plan.md sec 5)
    -----------------------------------------------------------
    * No DDL on the case tables at all -- so the 21 procedures that copy history with
      INSERT INTO <T>_HISTORY SELECT * FROM <T> cannot break on ordinal drift. That
      trap is what makes the per-table pattern dangerous for VIGILANCE (171 cols) and
      RRB (159 cols).
    * One checker action procedure and one checker inbox instead of 14 of each.
    * "Everything pending for me" is a single query rather than a 14-way UNION.

    Objects created
    ---------------
      dbo.WORKFLOW_MODULE             registry: which modules participate + how to reach them
      dbo.CASE_APPROVAL               one row per case record under workflow
      dbo.CASE_APPROVAL_HISTORY       append-only audit trail
      dbo.vw_CASE_APPROVAL_ORPHANS    monitor: pending rows no checker can ever see
      dbo.spCase_CheckerAction        generic approve / reject / push-back
      dbo.spCase_CheckerQueue         generic checker inbox

    Also hardens dbo.MakerCheckerMapping (type mismatch, missing unique key, missing index).

    COMPLAINT is NOT migrated by this script. It keeps its existing inline columns and
    keeps working exactly as today. Migrating it is a separate, later step.

    Safe to re-run.
*/

SET NOCOUNT ON;
GO

-------------------------------------------------------------------------------------------------
-- 1. dbo.WORKFLOW_MODULE  -- registry of participating modules
-------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.WORKFLOW_MODULE') IS NULL
BEGIN
    CREATE TABLE dbo.WORKFLOW_MODULE
    (
        ModuleCode  varchar(20)  NOT NULL CONSTRAINT PK_WORKFLOW_MODULE PRIMARY KEY,
        ModuleName  varchar(100) NOT NULL,   -- label shown in the checker inbox
        TableName   sysname      NOT NULL,   -- 'IAC'
        KeyColumn   sysname      NOT NULL,   -- surrogate key       -> CASE_APPROVAL.RecordCode
        RefColumn   sysname      NOT NULL,   -- human-readable key  -> CASE_APPROVAL.RecordRef
        ZoneColumn  sysname      NOT NULL,   -- SOL-coded zone      -> CASE_APPROVAL.ZoneSolID
        ViewPage    varchar(200) NOT NULL,   -- checker detail page
        IsActive    bit          NOT NULL CONSTRAINT DF_WORKFLOW_MODULE_Active DEFAULT(1)
    );
END
GO

MERGE dbo.WORKFLOW_MODULE AS T
USING (VALUES
        ('IAC', 'IAC', 'IAC', 'SNO', 'IACNO', 'NEWZONE', '~/Mis/frmIACCheckerView.aspx', 1)
      ) AS S (ModuleCode, ModuleName, TableName, KeyColumn, RefColumn, ZoneColumn, ViewPage, IsActive)
   ON T.ModuleCode = S.ModuleCode
WHEN MATCHED THEN UPDATE SET
        T.ModuleName = S.ModuleName,
        T.TableName  = S.TableName,
        T.KeyColumn  = S.KeyColumn,
        T.RefColumn  = S.RefColumn,
        T.ZoneColumn = S.ZoneColumn,
        T.ViewPage   = S.ViewPage,
        T.IsActive   = S.IsActive
WHEN NOT MATCHED THEN
    INSERT (ModuleCode, ModuleName, TableName, KeyColumn, RefColumn, ZoneColumn, ViewPage, IsActive)
    VALUES (S.ModuleCode, S.ModuleName, S.TableName, S.KeyColumn, S.RefColumn, S.ZoneColumn, S.ViewPage, S.IsActive);
GO

-------------------------------------------------------------------------------------------------
-- 2. dbo.CASE_APPROVAL  -- current approval state, one row per case record
-------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.CASE_APPROVAL') IS NULL
BEGIN
    CREATE TABLE dbo.CASE_APPROVAL
    (
        ApprovalId      bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_CASE_APPROVAL PRIMARY KEY,
        ModuleCode      varchar(20)  NOT NULL,
        RecordCode      bigint       NOT NULL,   -- case table surrogate key (IAC.SNO)
        RecordRef       varchar(50)  NULL,       -- what the user sees      (IAC.IACNO)
        ZoneSolID       varchar(10)  NULL,       -- snapshot at submit; drives checker routing
        ApprovalStatus  char(1)      NOT NULL CONSTRAINT DF_CASE_APPROVAL_Status DEFAULT('P'),
        MakerUser       varchar(50)  NULL,
        MakerDate       datetime     NULL CONSTRAINT DF_CASE_APPROVAL_MakerDate DEFAULT(GETDATE()),
        CheckerUser     varchar(50)  NULL,
        CheckerDate     datetime     NULL,
        CheckerRemarks  varchar(max) NULL,
        CONSTRAINT UQ_CASE_APPROVAL          UNIQUE (ModuleCode, RecordCode),
        CONSTRAINT CK_CASE_APPROVAL_STATUS   CHECK  (ApprovalStatus IN ('P','A','C','X')),
        CONSTRAINT FK_CASE_APPROVAL_MODULE   FOREIGN KEY (ModuleCode) REFERENCES dbo.WORKFLOW_MODULE(ModuleCode)
    );
END
GO

-- Covers the checker inbox: status + zone + module, with the display fields carried along.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_CASE_APPROVAL_Queue' AND object_id = OBJECT_ID('dbo.CASE_APPROVAL'))
    CREATE INDEX IX_CASE_APPROVAL_Queue
        ON dbo.CASE_APPROVAL (ApprovalStatus, ZoneSolID, ModuleCode)
        INCLUDE (RecordCode, RecordRef, MakerUser, MakerDate);
GO

-- Covers the list-screen join (module + record).
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_CASE_APPROVAL_Record' AND object_id = OBJECT_ID('dbo.CASE_APPROVAL'))
    CREATE INDEX IX_CASE_APPROVAL_Record
        ON dbo.CASE_APPROVAL (ModuleCode, RecordCode)
        INCLUDE (ApprovalStatus, CheckerRemarks);
GO

-------------------------------------------------------------------------------------------------
-- 3. dbo.CASE_APPROVAL_HISTORY  -- append-only audit trail
-------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.CASE_APPROVAL_HISTORY') IS NULL
BEGIN
    CREATE TABLE dbo.CASE_APPROVAL_HISTORY
    (
        Id          bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_CASE_APPROVAL_HISTORY PRIMARY KEY,
        ModuleCode  varchar(20)  NOT NULL,
        RecordCode  bigint       NOT NULL,
        ActionType  varchar(20)  NOT NULL,   -- SUBMITTED | RESUBMITTED | APPROVED | REJECTED | PUSHED_BACK | GRANDFATHERED
        ActionBy    varchar(50)  NOT NULL,
        ActionDate  datetime     NOT NULL CONSTRAINT DF_CASE_APPROVAL_HISTORY_Date DEFAULT(GETDATE()),
        Remarks     varchar(max) NULL,
        UserRole    varchar(50)  NULL,
        UserIP      varchar(30)  NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_CASE_APPROVAL_HISTORY_Record' AND object_id = OBJECT_ID('dbo.CASE_APPROVAL_HISTORY'))
    CREATE INDEX IX_CASE_APPROVAL_HISTORY_Record
        ON dbo.CASE_APPROVAL_HISTORY (ModuleCode, RecordCode, Id);
GO

-------------------------------------------------------------------------------------------------
-- 4. dbo.MakerCheckerMapping hardening
--    (needed regardless of which table strategy is used)
-------------------------------------------------------------------------------------------------

-- 4a. Join-type mismatch. ZoneSolID was varchar(6) while COMPLAINT.NEWZONE / IAC.NEWZONE are
--     varchar(10). Any SOL code longer than 6 characters could never match, so those zones
--     silently had no checker.
IF EXISTS (SELECT 1 FROM sys.columns
           WHERE object_id = OBJECT_ID('dbo.MakerCheckerMapping')
             AND name = 'ZoneSolID' AND max_length < 10)
    ALTER TABLE dbo.MakerCheckerMapping ALTER COLUMN ZoneSolID varchar(10) NOT NULL;
GO

-- 4b. Admin/UserCreation.aspx.cs does SELECT COUNT(*) then UPDATE-or-INSERT. That is a race:
--     two concurrent saves produce duplicate mappings, which duplicate every inbox row.
IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'UQ_MakerCheckerMapping')
   AND NOT EXISTS (SELECT UserPF, ZoneSolID FROM dbo.MakerCheckerMapping
                   GROUP BY UserPF, ZoneSolID HAVING COUNT(*) > 1)
    ALTER TABLE dbo.MakerCheckerMapping
        ADD CONSTRAINT UQ_MakerCheckerMapping UNIQUE (UserPF, ZoneSolID);
GO

-- 4c. Only a PK on Id existed, so every checker inbox query scanned the table.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_MakerCheckerMapping_Lookup' AND object_id = OBJECT_ID('dbo.MakerCheckerMapping'))
    CREATE INDEX IX_MakerCheckerMapping_Lookup
        ON dbo.MakerCheckerMapping (UserPF, IsChecker, IsActive) INCLUDE (ZoneSolID);
GO

-------------------------------------------------------------------------------------------------
-- 5. dbo.vw_CASE_APPROVAL_ORPHANS  -- pending records no checker can ever see
--
--    A pending row whose ZoneSolID is NULL, or whose zone has no active checker mapped,
--    is invisible to every inbox and locked from editing by the maker. Without a monitor
--    it sits there forever and nothing surfaces it. Run this after every bulk import.
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
                 ELSE 'No active checker mapped to this zone'
            END AS OrphanReason
    FROM    dbo.CASE_APPROVAL CA
    WHERE   CA.ApprovalStatus = 'P'
      AND   NOT EXISTS (SELECT 1
                        FROM   dbo.MakerCheckerMapping M
                        WHERE  M.ZoneSolID = CA.ZoneSolID
                          AND  M.IsChecker = 1
                          AND  M.IsActive  = 1);
GO

-------------------------------------------------------------------------------------------------
-- 6. dbo.spCase_CheckerAction  -- one procedure for every module
--
--    Guard order matters: cheap validation first, authorisation before state, and the
--    "still pending" check last so a double-submit cannot double-action a record.
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

    IF NOT EXISTS (SELECT 1 FROM dbo.MakerCheckerMapping
                   WHERE UserPF    = @p_USER
                     AND ZoneSolID = @ZONE
                     AND IsChecker = 1
                     AND IsActive  = 1)
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
-- 7. dbo.spCase_CheckerQueue  -- one inbox query for every module
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spCase_CheckerQueue]
(
    @p_USER   VARCHAR(50),
    @p_MODULE VARCHAR(20) = NULL,   -- NULL = every module this checker is mapped to
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
            INNER JOIN dbo.MakerCheckerMapping M
                    ON M.ZoneSolID = CA.ZoneSolID
                   AND M.IsChecker = 1
                   AND M.IsActive  = 1
    WHERE   M.UserPF = @p_USER
      AND   (@p_MODULE IS NULL OR CA.ModuleCode    = @p_MODULE)
      AND   (@p_STATUS IS NULL OR CA.ApprovalStatus = @p_STATUS)
    ORDER BY CA.MakerDate DESC;
END
GO
