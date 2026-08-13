/*
    MISC Maker-Checker
    =================================================================================
    Database : VigilanceMISDB
    Requires : 2026-07-27_CaseApproval_Central_MakerChecker_Foundation.sql
               2026-08-13_MakerChecker_ModuleScope.sql   (registers group CMP)

    Puts the MISC module on the central CASE_APPROVAL registry, following the recipe
    in Docs/VMIS_IAC_MakerChecker_Implementation.md sec 9. No column is added to MISC
    or MISC_HISTORY, so the INSERT INTO MISC_HISTORY SELECT * FROM MISC statements in
    spMiscStructure_Update and spMISCExcel_Import stay ordinally safe.

    MISC belongs to checker group CMP (Complaint & MISC), so a checker granted CMP in
    a zone sees Complaint and MISC records for that zone, and no Vigilance or IAC.

    Changes
    -------
    1. Register MISC in WORKFLOW_MODULE (group CMP).
    2. Backfill : existing MISC rows are grandfathered as Approved (see note below).
    3. spMiscStructure_Update
         insert -> registers the record as Pending and logs SUBMITTED
         update -> blocked when Pending or Rejected; re-queued to Pending (+ RESUBMITTED)
                   when the prior status was Approved or Changes Requested
         both   -> Zone (New) is now mandatory for a maker, because the zone is what
                   routes the record to a checker.
    4. spMiscStructure_View  : LIST / SEARCH / GET now return APPROVALSTATUS,
                               APPROVALSTATUSTEXT and CHECKERREMARKS.
    5. spMISCExcel_Import    : registers imported rows, honouring
                               WORKFLOW_MODULE.ImportApprovalStatus.

    Backfill note
    -------------
    Step 2 marks every pre-existing MISC row Approved, on the basis that those records
    predate the control. Flipping them to 'P' instead would flood the checker inbox and
    lock every existing record from editing. Change the literal in step 2 if the
    business wants the opposite.

    Safe to re-run.
*/

SET NOCOUNT ON;
GO

-- Captured into the metadata of every procedure created below. Must be ON -- see the note in
-- 2026-08-13_MakerChecker_ModuleScope.sql. sqlcmd defaults QUOTED_IDENTIFIER to OFF.
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

-------------------------------------------------------------------------------------------------
-- 0. WORKFLOW_MODULE.ImportApprovalStatus -- present if the IAC Excel script has run
-------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('dbo.WORKFLOW_MODULE') AND name = 'ImportApprovalStatus')
    ALTER TABLE dbo.WORKFLOW_MODULE
        ADD ImportApprovalStatus char(1) NOT NULL
            CONSTRAINT DF_WORKFLOW_MODULE_ImportStatus DEFAULT('P');
GO

-------------------------------------------------------------------------------------------------
-- 1. Register MISC
--
--    ImportApprovalStatus = 'A' for MISC, unlike IAC.
--
--    The MISC upload sheet carries a free-text ZONE column but no SOL-coded zone -- the
--    import proc had no NEWZONESOLID parameter at all before this script. Under 'P' every
--    imported row would therefore land Pending with no zone: invisible to every inbox and
--    locked from editing. 'A' registers them Approved with CheckerUser = 'SYSTEM' and an
--    audit note, which is an explicit, audited exemption rather than a silent gap -- and
--    any later edit re-queues the record for verification like anything else.
--
--    To require checking on MISC uploads: add a NEWZONESOLID column to the sheet (step 5
--    already accepts it), then
--        UPDATE dbo.WORKFLOW_MODULE SET ImportApprovalStatus = 'P' WHERE ModuleCode = 'MISC';
-------------------------------------------------------------------------------------------------
MERGE dbo.WORKFLOW_MODULE AS T
USING (VALUES
        ('MISC', 'MISC', 'MISC', 'CODE', 'RNO', 'NEWZONE',
         '~/Mis/frmMiscCheckerView.aspx', 1, 'CMP', 'A')
      ) AS S (ModuleCode, ModuleName, TableName, KeyColumn, RefColumn, ZoneColumn,
              ViewPage, IsActive, GroupCode, ImportApprovalStatus)
   ON T.ModuleCode = S.ModuleCode
WHEN MATCHED THEN UPDATE SET
        T.ModuleName = S.ModuleName,
        T.TableName  = S.TableName,
        T.KeyColumn  = S.KeyColumn,
        T.RefColumn  = S.RefColumn,
        T.ZoneColumn = S.ZoneColumn,
        T.ViewPage   = S.ViewPage,
        T.IsActive   = S.IsActive,
        T.GroupCode  = S.GroupCode
        -- ImportApprovalStatus is deliberately NOT overwritten on re-run: it is an
        -- operational setting the business may have changed since first deployment.
WHEN NOT MATCHED THEN
    INSERT (ModuleCode, ModuleName, TableName, KeyColumn, RefColumn, ZoneColumn,
            ViewPage, IsActive, GroupCode, ImportApprovalStatus)
    VALUES (S.ModuleCode, S.ModuleName, S.TableName, S.KeyColumn, S.RefColumn, S.ZoneColumn,
            S.ViewPage, S.IsActive, S.GroupCode, S.ImportApprovalStatus);
GO

-------------------------------------------------------------------------------------------------
-- 2. Backfill: grandfather pre-existing MISC records
-------------------------------------------------------------------------------------------------
INSERT INTO dbo.CASE_APPROVAL
    (ModuleCode, RecordCode, RecordRef, ZoneSolID, ApprovalStatus, MakerUser, MakerDate, CheckerUser, CheckerDate, CheckerRemarks)
SELECT  'MISC',
        M.CODE,
        M.RNO,
        M.NEWZONE,
        'A',
        ISNULL(M.ADDUSER, 'SYSTEM'),
        ISNULL(M.ADDDATE, GETDATE()),
        'SYSTEM',
        GETDATE(),
        'Pre-existing record grandfathered at maker-checker rollout.'
FROM    dbo.MISC M
WHERE   M.ACTIVE = 'Y'
  AND   NOT EXISTS (SELECT 1 FROM dbo.CASE_APPROVAL CA
                    WHERE CA.ModuleCode = 'MISC' AND CA.RecordCode = M.CODE);
GO

INSERT INTO dbo.CASE_APPROVAL_HISTORY (ModuleCode, RecordCode, ActionType, ActionBy, Remarks)
SELECT  'MISC', CA.RecordCode, 'GRANDFATHERED', 'SYSTEM',
        'Pre-existing record grandfathered at maker-checker rollout.'
FROM    dbo.CASE_APPROVAL CA
WHERE   CA.ModuleCode = 'MISC'
  AND   CA.CheckerUser = 'SYSTEM'
  AND   NOT EXISTS (SELECT 1 FROM dbo.CASE_APPROVAL_HISTORY H
                    WHERE H.ModuleCode = 'MISC' AND H.RecordCode = CA.RecordCode
                      AND H.ActionType = 'GRANDFATHERED');
GO

-------------------------------------------------------------------------------------------------
-- 3. dbo.spMiscStructure_Update
--    (full body below is the complete, current definition -- not a diff)
--    No parameter was added or removed, so the existing C# call site works unchanged.
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spMiscStructure_Update]
(
		@p_CODE BIGINT,
		@p_RNO VARCHAR(50),
		@p_COMPNO VARCHAR(50),
		@p_ACCUSED VARCHAR(100),
		@p_DESIGNATION VARCHAR(50),
		@p_FINALACTION VARCHAR(100),
		@p_BRCOMPLAINT VARCHAR(100),
		@p_ZONE VARCHAR(100),
		@p_CIRCLEOFFICE VARCHAR(100),
		@p_RECDATECOMP DATETIME=NULL,
		@p_SOURCE VARCHAR(100),
		@p_SOURCEREF VARCHAR(50),
		@p_SOURCEDATE DATETIME=NULL,
		@p_SENTTO VARCHAR(50),
		@p_SENTFORINVDATE DATETIME=NULL,
		@p_ACCOUNTNAME VARCHAR(100),
		@p_AMOUNT DECIMAL(19,4),
		@p_ALLEGATIONS VARCHAR(100),
		@p_DTIAC DATETIME=NULL,
		@p_STATUS VARCHAR(MAX),
		@p_HOSTATUS VARCHAR(250)=NULL,
		@p_STATUSCODE VARCHAR(50),
		@p_NATURE VARCHAR(100),
		@p_DTOFINVREPORT DATETIME=NULL,
		@p_CASECLOSE VARCHAR(5),
		@p_CLOSUREDT DATETIME=NULL,
		@p_RYSENT DATETIME=NULL,
		@p_REASONSFORCLOSURE VARCHAR(MAX),
		@p_NPADATE DATETIME=NULL,
		@p_NATURECOMP VARCHAR(250),
		@p_TYPE VARCHAR(100),
		@p_INVESTIGATIONDATE DATETIME=NULL,
		@p_MODE CHAR(1),
		@p_CLOSURE VARCHAR(1),
		@p_USER VARCHAR(50),
		@p_USERROLE VARCHAR(50),
		@p_DESK_USER_REMARKS VARCHAR(MAX),
		@p_USERIP VARCHAR(20),
		@o_EERMSG VARCHAR(MAX) OUTPUT,
		@o_ERRCODE INT OUTPUT,
		@p_BANKNAME VARCHAR(20),
		@p_LETTERSENTTO VARCHAR(10),
		@p_LETTERSENTDATE DATETIME=NULL,
		@p_REMINDERDATE DATETIME=NULL,
		@p_REPLYRECEIVEDDATE DATETIME=NULL,
		@p_PFNO VARCHAR(10),
		@p_ZONENEW VARCHAR(10)=NULL,
		@p_CIRCLENEW VARCHAR(10)=NULL,
		@p_ZONE_TYPE		VARCHAR(20),
		@p_ZONE_CM			VARCHAR(200)
)
AS
BEGIN
DECLARE @STATUS VARCHAR(MAX),@UPDATESTATUS VARCHAR(MAX);
DECLARE @OLDAPPROVALSTATUS CHAR(1), @NEWCODE BIGINT;
SET @o_ERRCODE=0;
----------------------------------------------------------------------------------------------------------------------
		IF(@p_USERROLE = 'VMIS_MISUSER')
			BEGIN
				IF(@p_RNO <> '')
					BEGIN
						IF EXISTS (SELECT 1 FROM MISC WHERE RNO=@p_RNO AND ACTIVE='Y' AND CODE <> @p_CODE)
							BEGIN
								SET @o_ERRCODE=3;
								SET @o_EERMSG=@p_RNO + '- R Number alredy Exists......!';
							END
					END

				--MAKER-CHECKER: THE ZONE IS WHAT ROUTES A RECORD TO A CHECKER. A RECORD SAVED
				--WITHOUT ONE WOULD SIT PENDING FOREVER, INVISIBLE TO EVERY INBOX AND LOCKED
				--FROM EDITING, SO REFUSE THE SAVE INSTEAD.
				IF(@o_ERRCODE = 0 AND ISNULL(LTRIM(RTRIM(@p_ZONENEW)),'') = '')
					BEGIN
						SET @o_ERRCODE=4;
						SET @o_EERMSG='Zone (New) is mandatory. It decides which checker verifies this record.';
					END

				--MAKER-CHECKER: A RECORD AWAITING VERIFICATION, OR ONE THE CHECKER HAS REJECTED,
				--IS NOT THE MAKER'S TO EDIT. THE GRID ALREADY DISABLES THE BUTTON; THIS BLOCKS
				--A DIRECT POSTBACK OR A SECOND ENTRY PAGE FROM GETTING PAST IT.
				IF(@o_ERRCODE = 0 AND @p_MODE = 'U')
					BEGIN
						SELECT @OLDAPPROVALSTATUS = ApprovalStatus
						FROM   CASE_APPROVAL
						WHERE  ModuleCode = 'MISC' AND RecordCode = @p_CODE;

						IF(@OLDAPPROVALSTATUS = 'X')
							BEGIN
								SET @o_ERRCODE=5;
								SET @o_EERMSG='This record has been rejected by the checker and cannot be edited.';
							END
						ELSE IF(@OLDAPPROVALSTATUS = 'P')
							BEGIN
								SET @o_ERRCODE=5;
								SET @o_EERMSG='This record is pending verification and cannot be edited until the checker acts on it.';
							END
					END

				IF(@o_ERRCODE = 0)
					BEGIN
						IF(@p_MODE='I')
							BEGIN
							  BEGIN TRY
								BEGIN TRAN;

								INSERT INTO MISC (RNO,COMPNO,
												  ACCUSED,DESIGNATION,
												  BRCOMPLAINT,
												  ZONE,CIRCLEOFFICE,
												  RECDATECOMP,
												  SOURCE,SOURCEREF,
												  SOURCEDATE,SENTTO,
												  SENTFORINVDATE,
												  ACCOUNTNAME,AMOUNT,
												  ALLEGATIONS,DTIAC,
												  STATUS,STATUSCODE,
												  NATURE,DTOFINVREPORT,
												  CASECLOSE,
												  CLOSUREDT,RYSENT,
												  REASONSFORCLOSURE,
												  NPADATE,DTINVESTIGATION,
												  TYPE,FINALACTION,NATURECOMP,
												  ADDUSER,ADDDATE,ADDUSERIP,BANKNAME,
												  LETTERSENTTO,LETTERSENTDATE,REMINDERDATE,REPLYRECEIVEDDATE,PFNO,
												  NEWZONE,NEWCIRCLE,ZONE_TYPE,ZONE_CM)
									   VALUES (@p_RNO,@p_COMPNO,
											   @p_ACCUSED,@p_DESIGNATION,
											   @p_BRCOMPLAINT,
											   @p_ZONE,@p_CIRCLEOFFICE,
											   @p_RECDATECOMP,
											   @p_SOURCE,@p_SOURCEREF,
											   @p_SOURCEDATE,@p_SENTTO,
											   @p_SENTFORINVDATE,
											   @p_ACCOUNTNAME,@p_AMOUNT,
											   @p_ALLEGATIONS,@p_DTIAC,
											   @p_STATUS,@p_STATUSCODE,
											   @p_NATURE,@p_DTOFINVREPORT,
											   @p_CASECLOSE,
											   (CASE WHEN(@p_CLOSURE = 'Y') THEN GETDATE() ELSE NULL END),@p_RYSENT,
											   @p_REASONSFORCLOSURE,
											   @p_NPADATE,@p_INVESTIGATIONDATE,
											   @p_TYPE,@p_FINALACTION,@p_NATURECOMP,
											   @p_USER,GETDATE(),@p_USERIP,@p_BANKNAME,
											   @p_LETTERSENTTO,@p_LETTERSENTDATE,@p_REMINDERDATE,@p_REPLYRECEIVEDDATE,@p_PFNO,
											   @p_ZONENEW,@p_CIRCLENEW,@p_ZONE_TYPE,@p_ZONE_CM)

								SET @NEWCODE = SCOPE_IDENTITY();

								--MAKER-CHECKER: REGISTER THE NEW RECORD AS PENDING VERIFICATION
								INSERT INTO CASE_APPROVAL
									(ModuleCode, RecordCode, RecordRef, ZoneSolID, ApprovalStatus, MakerUser, MakerDate)
								VALUES
									('MISC', @NEWCODE, @p_RNO, @p_ZONENEW, 'P', @p_USER, GETDATE());

								INSERT INTO CASE_APPROVAL_HISTORY
									(ModuleCode, RecordCode, ActionType, ActionBy, Remarks, UserRole, UserIP)
								VALUES
									('MISC', @NEWCODE, 'SUBMITTED', @p_USER, NULL, @p_USERROLE, @p_USERIP);

								COMMIT TRAN;

								SET @o_ERRCODE=1;
								SET @o_EERMSG='Record Saved Sucessfully......!';
							  END TRY
							  BEGIN CATCH
								IF (XACT_STATE() <> 0) ROLLBACK TRAN;
								SET @o_ERRCODE=0;
								SET @o_EERMSG='Could not save the record: ' + ERROR_MESSAGE();
							  END CATCH
							END

						ELSE IF(@p_MODE='U')
							BEGIN
							  BEGIN TRY
								BEGIN TRAN;

								--UPDATE TBALE MISC_HISTORY
								INSERT INTO MISC_HISTORY SELECT * FROM MISC WHERE CODE=@p_CODE;

								--UPDATE DATA OF MISC
								UPDATE MISC SET RNO=@p_RNO,
											    COMPNO=@p_COMPNO,
											    ACCUSED=@p_ACCUSED,
											    DESIGNATION=@p_DESIGNATION,
											    FINALACTION=@p_FINALACTION,
											    BRCOMPLAINT=@p_BRCOMPLAINT,
											    ZONE=@p_ZONE,
											    CIRCLEOFFICE=@p_CIRCLEOFFICE,
											    RECDATECOMP=@p_RECDATECOMP,
											    SOURCE=@p_SOURCE,
											    SOURCEREF=@p_SOURCEREF,
											    SOURCEDATE=@p_SOURCEDATE,
											    SENTTO=@p_SENTTO,
											    SENTFORINVDATE=@p_SENTFORINVDATE,
											    ACCOUNTNAME=@p_ACCOUNTNAME,
											    AMOUNT=@p_AMOUNT,
											    ALLEGATIONS=@p_ALLEGATIONS,
											    DTIAC=@p_DTIAC,
											    STATUS=@p_STATUS,
											    STATUSCODE=@p_STATUSCODE,
											    NATURE=@p_NATURE,
											    DTOFINVREPORT=@p_DTOFINVREPORT,
											    CASECLOSE=@p_CASECLOSE,
											    CLOSUREDT=(CASE WHEN(@p_CLOSURE = 'Y') THEN GETDATE() ELSE @p_CLOSUREDT END),
											    RYSENT=@p_RYSENT,
											    REASONSFORCLOSURE=@p_REASONSFORCLOSURE,
											    NPADATE=@p_NPADATE,
											    DTINVESTIGATION=@p_INVESTIGATIONDATE,
											    TYPE=@p_TYPE,
											    NATURECOMP=@p_NATURECOMP,
											    MODUSER=@p_USER,
												MODDATE=GETDATE(),
												MODUSERIP=@p_USERIP,
												BANKNAME=@p_BANKNAME,
												LETTERSENTTO=@p_LETTERSENTTO,
												LETTERSENTDATE=@p_LETTERSENTDATE,
												REMINDERDATE=@p_REMINDERDATE,
												REPLYRECEIVEDDATE=@p_REPLYRECEIVEDDATE,
												PFNO=@p_PFNO,
												NEWZONE=@p_ZONENEW,
												NEWCIRCLE=@p_CIRCLENEW,
												ZONE_TYPE=@p_ZONE_TYPE,
												ZONE_CM=@p_ZONE_CM
								WHERE CODE=@p_CODE;

								IF(@OLDAPPROVALSTATUS IS NULL)
									BEGIN
										--A RECORD THAT PREDATES THE ROLLOUT, OR ONE CREATED BY AN IMPORT.
										--ONBOARD IT NOW SO THE EDIT IS VERIFIED.
										INSERT INTO CASE_APPROVAL
											(ModuleCode, RecordCode, RecordRef, ZoneSolID, ApprovalStatus, MakerUser, MakerDate)
										VALUES
											('MISC', @p_CODE, @p_RNO, @p_ZONENEW, 'P', @p_USER, GETDATE());

										INSERT INTO CASE_APPROVAL_HISTORY
											(ModuleCode, RecordCode, ActionType, ActionBy, Remarks, UserRole, UserIP)
										VALUES
											('MISC', @p_CODE, 'SUBMITTED', @p_USER, NULL, @p_USERROLE, @p_USERIP);
									END
								ELSE
									BEGIN
										--AN EDIT INVALIDATES A PREVIOUS DECISION, SO SEND THE RECORD BACK
										--TO THE CHECKER'S QUEUE. KEEP THE REFERENCE AND ZONE IN STEP WITH
										--THE CASE ROW SO ROUTING FOLLOWS A ZONE CHANGE.
										UPDATE CASE_APPROVAL
										   SET RecordRef      = @p_RNO,
											   ZoneSolID      = @p_ZONENEW,
											   ApprovalStatus = 'P',
											   MakerUser      = @p_USER,
											   MakerDate      = GETDATE(),
											   CheckerUser    = NULL,
											   CheckerDate    = NULL,
											   CheckerRemarks = NULL
										 WHERE ModuleCode = 'MISC' AND RecordCode = @p_CODE;

										INSERT INTO CASE_APPROVAL_HISTORY
											(ModuleCode, RecordCode, ActionType, ActionBy, Remarks, UserRole, UserIP)
										VALUES
											('MISC', @p_CODE, 'RESUBMITTED', @p_USER, NULL, @p_USERROLE, @p_USERIP);
									END

								COMMIT TRAN;

								SET @o_ERRCODE= 2;
								SET @o_EERMSG='Record Updated Sucessfully......!'
							  END TRY
							  BEGIN CATCH
								IF (XACT_STATE() <> 0) ROLLBACK TRAN;
								SET @o_ERRCODE=0;
								SET @o_EERMSG='Could not update the record: ' + ERROR_MESSAGE();
							  END CATCH
							END
					END
			END

		ELSE IF(@p_USERROLE = 'VMIS_DESKUSER')
			BEGIN
				--UPDATE TBALE MISC_HISTORY
				INSERT INTO MISC_HISTORY SELECT * FROM MISC WHERE CODE=@p_CODE;
				--SELECT STATUS FROM MISC FOR APPEND STATUS IN CASE OF VMIS_DESKUSER USER
				SELECT @STATUS=ISNULL(STATUS,'') FROM MISC WHERE CODE=@p_CODE;
				SET @UPDATESTATUS = @p_HOSTATUS + ' | ' + @STATUS;
				UPDATE MISC SET STATUS=@UPDATESTATUS,
								DESK_USER_REMARKS=@p_DESK_USER_REMARKS,
								DESK_USER_ID=@p_USER,
								DESK_USER_IP=@p_USERIP,
								DESK_USER_ADDDATE=GETDATE(),
								DESK_USER_ROLE=@p_USERROLE
				WHERE CODE=@p_CODE;
				SET @o_ERRCODE= 1;
				SET @o_EERMSG='Dealing Officer Remarks Updated Sucessfully......!'
			END
END
GO

-------------------------------------------------------------------------------------------------
-- 4. dbo.spMiscStructure_View
--    (full body below is the complete, current definition -- not a diff)
--    Adds APPROVALSTATUS / APPROVALSTATUSTEXT / CHECKERREMARKS from CASE_APPROVAL.
--    LEFT JOIN, so records that predate the workflow still list with NULL status.
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spMiscStructure_View]
(
	@p_SEARCHNO		VARCHAR(50)=NULL,
	@p_VIEW			VARCHAR(50)=NULL,
	@p_BRANCH		VARCHAR(50)=NULL,
	@p_STATUS		VARCHAR(50)=NULL,
	@p_CIRCLEOFFICE	VARCHAR(50)=NULL,
	@p_SOURCE		VARCHAR(100)=NULL,
	@p_SOURCEREF	VARCHAR(100)=NULL,
	@p_COMPNO		VARCHAR(50)=NULL,
	@p_ACCOUNTNAME	VARCHAR(50)=NULL,
	@o_EERMSG		VARCHAR(MAX) OUTPUT,
	@o_ERRCODE		INT OUTPUT
)
AS
BEGIN
	DECLARE @SQL VARCHAR(MAX),@STRCOND VARCHAR(MAX),@ERRORCODE VARCHAR(5);
	--------------------------------------------------------------------------------------
	SET @o_ERRCODE=0;
	SET @STRCOND = 'M.ACTIVE=''Y''';
	--------------------------------------------------------------------------------------
	IF(@p_SEARCHNO <> '' AND @p_SEARCHNO IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND RNO LIKE''%'+@p_SEARCHNO+'%''' + CHAR(13);
			END
	IF(@p_BRANCH <> '' AND @p_BRANCH IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND BRCOMPLAINT LIKE''%'+@p_BRANCH+'%''' + CHAR(13);
			END
	IF(@p_STATUS <> '' AND @p_STATUS IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND STATUS LIKE''%'+@p_STATUS+'%''' + CHAR(13);
			END
	IF(@p_CIRCLEOFFICE <> '' AND @p_CIRCLEOFFICE IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND CIRCLEOFFICE LIKE''%'+@p_CIRCLEOFFICE+'%''' + CHAR(13);
			END
	IF(@p_SOURCE <> '' AND @p_SOURCE IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND SOURCE LIKE''%'+@p_SOURCE+'%''' + CHAR(13);
			END
	IF(@p_SOURCEREF <> '' AND @p_SOURCEREF IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND SOURCEREF LIKE''%'+@p_SOURCEREF+'%''' + CHAR(13);
			END
	IF(@p_COMPNO <> '' AND @p_COMPNO IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND COMPNO LIKE''%'+@p_COMPNO+'%''' + CHAR(13);
			END
	IF(@p_ACCOUNTNAME <> '' AND @p_ACCOUNTNAME IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND ACCOUNTNAME LIKE''%'+@p_ACCOUNTNAME+'%''' + CHAR(13);
			END
	---------------------------------------------------------------------------------------------------------------------------------------
	IF(UPPER(@p_VIEW) = 'LIST')
		BEGIN
			SELECT TOP 20 M.CODE AS CODE,RNO,ISNULL(CONVERT(VARCHAR(50),RECDATECOMP,103),'') AS COMPRECDATE,BRCOMPLAINT,CIRCLEOFFICE,
				   COMPNO,ISNULL(CONVERT(VARCHAR(50),CLOSUREDT,103),'') AS CLOSUREDATE,ACCUSED,ALLEGATIONS,
				   CASENO,ISNULL(CONVERT(VARCHAR(50),DTIAC,103),'') AS IACDATE,PRESENTPOSTING,ZONE,
				   SOURCE,ISNULL(CONVERT(VARCHAR(50),SOURCEDATE,103),'') AS SOURCEDATE,SOURCEREF,ACCOUNTNAME,
				   STATUSCODE,ISNULL(CONVERT(VARCHAR(50),SENTFORINVDATE,103),'') AS SENTFORINVDATE,SENTTO,REGION,
				   AMOUNT,ISNULL(CONVERT(VARCHAR(50),DTOFINVREPORT,103),'') AS INVREPORTDATE,DESIGNATION,NAMEOFINVOFFICIAL,
				   CASECLOSE,ISNULL(CONVERT(VARCHAR(50),RYSENT,103),'') AS RYSENTDATE,REASONSFORCLOSURE,
				   ([dbo].ReverseColumnValue_Function(STATUS,RNO,'MISC')) AS SHORTSTATUS,
				   REASONSFORCLOSURE,STATUS,TYPE,FINALACTION,NATURECOMP,
				   ISNULL(CONVERT(VARCHAR(50),NPADATE,103),'') AS NPADATE,
				   ISNULL(CONVERT(VARCHAR(50),DTINVESTIGATION,103),'') AS INVESTIGATIONDATE,
				   M.ADDUSER AS ENTRYBY,ISNULL(CONVERT(VARCHAR(50),M.ADDDATE,103),'') AS ENTRYDATE,
				   ISNULL(M.MODUSER,'') AS MODIFYBY,ISNULL(CONVERT(VARCHAR(50),M.MODDATE,103),'') AS MODIFYDATE,
				   NATURE AS NATURECODE,NATURECASE AS NATURE,
				   (CASE WHEN CLOSUREDT IS NULL THEN 'N' ELSE 'Y' END) AS CLOSURE,PFNO,
				   NEWZONE AS NEWZONE,NEWCIRCLE AS NEWCIRCLE,ZONE_TYPE,ZONE_CM,
				   CA.ApprovalStatus AS APPROVALSTATUS,
				   CASE CA.ApprovalStatus
						WHEN 'P' THEN 'Pending Approval'
						WHEN 'A' THEN 'Approved'
						WHEN 'C' THEN 'Changes Requested'
						WHEN 'X' THEN 'Rejected'
				   END AS APPROVALSTATUSTEXT,
				   ISNULL(CA.CheckerRemarks,'') AS CHECKERREMARKS
			FROM (MISC M LEFT JOIN NATURECASE ON NATURE=NATURECASE.CODE AND FORTABLE='MISC')
				 LEFT JOIN CASE_APPROVAL CA ON CA.ModuleCode='MISC' AND CA.RecordCode=M.CODE
			WHERE M.ACTIVE='Y'
			ORDER BY M.ADDDATE DESC;
		END

	ELSE IF(UPPER(@p_VIEW) = 'SEARCH')
		BEGIN
			SET @SQL='SELECT TOP 20 M.CODE AS CODE,RNO,ISNULL(CONVERT(VARCHAR(50),RECDATECOMP,103),'''') AS COMPRECDATE,BRCOMPLAINT,CIRCLEOFFICE,
						     COMPNO,ISNULL(CONVERT(VARCHAR(50),CLOSUREDT,103),'''') AS CLOSUREDATE,ACCUSED,ALLEGATIONS,
						     CASENO,ISNULL(CONVERT(VARCHAR(50),DTIAC,103),'''') AS IACDATE,PRESENTPOSTING,ZONE,
						     SOURCE,ISNULL(CONVERT(VARCHAR(50),SOURCEDATE,103),'''') AS SOURCEDATE,SOURCEREF,ACCOUNTNAME,
						     STATUSCODE,ISNULL(CONVERT(VARCHAR(50),SENTFORINVDATE,103),'''') AS SENTFORINVDATE,SENTTO,REGION,
						     AMOUNT,ISNULL(CONVERT(VARCHAR(50),DTOFINVREPORT,103),'''') AS INVREPORTDATE,DESIGNATION,NAMEOFINVOFFICIAL,
						     CASECLOSE,ISNULL(CONVERT(VARCHAR(50),RYSENT,103),'''') AS RYSENTDATE,REASONSFORCLOSURE,
						    ([dbo].ReverseColumnValue_Function(STATUS,RNO,''MISC'')) AS SHORTSTATUS,
						     REASONSFORCLOSURE,STATUS,TYPE,FINALACTION,NATURECOMP,
						     ISNULL(CONVERT(VARCHAR(50),NPADATE,103),'''') AS NPADATE,
						     ISNULL(CONVERT(VARCHAR(50),DTINVESTIGATION,103),'''') AS INVESTIGATIONDATE,
						     M.ADDUSER AS ENTRYBY,ISNULL(CONVERT(VARCHAR(50),M.ADDDATE,103),'''') AS ENTRYDATE,
						     ISNULL(M.MODUSER,'''') AS MODIFYBY,ISNULL(CONVERT(VARCHAR(50),M.MODDATE,103),'''') AS MODIFYDATE,
						     NATURE AS NATURECODE,NATURECASE AS NATURE,
						    (CASE WHEN CLOSUREDT IS NULL THEN ''N'' ELSE ''Y'' END) AS CLOSURE,PFNO,
							 NEWZONE AS NEWZONE,NEWCIRCLE AS NEWCIRCLE,ZONE_TYPE, ZONE_CM,
							 CA.ApprovalStatus AS APPROVALSTATUS,
							 CASE CA.ApprovalStatus
								  WHEN ''P'' THEN ''Pending Approval''
								  WHEN ''A'' THEN ''Approved''
								  WHEN ''C'' THEN ''Changes Requested''
								  WHEN ''X'' THEN ''Rejected''
							 END AS APPROVALSTATUSTEXT,
							 ISNULL(CA.CheckerRemarks,'''') AS CHECKERREMARKS
					  FROM (MISC M LEFT JOIN NATURECASE ON NATURE=NATURECASE.CODE AND FORTABLE=''MISC'')
						   LEFT JOIN CASE_APPROVAL CA ON CA.ModuleCode=''MISC'' AND CA.RecordCode=M.CODE
					  WHERE '+@STRCOND+'
					  ORDER BY M.ADDDATE DESC'
			EXEC(@SQL);
			--PRINT(@SQL);
			--PRINT(@ERRORCODE);
		END

	ELSE
		BEGIN
			IF EXISTS (SELECT 1 FROM MISC WHERE (CASE WHEN @p_VIEW='GET' THEN RNO ELSE CAST(CODE AS VARCHAR) END)=@p_SEARCHNO AND ACTIVE='Y')
				BEGIN
					SELECT M.CODE AS CODE,RNO,ISNULL(CONVERT(VARCHAR(50),RECDATECOMP,103),'') AS COMPRECDATE,BRCOMPLAINT,CIRCLEOFFICE,
						   COMPNO,ISNULL(CONVERT(VARCHAR(50),CLOSUREDT,103),'') AS CLOSUREDATE,ACCUSED,ALLEGATIONS,
						   CASENO,ISNULL(CONVERT(VARCHAR(50),DTIAC,103),'') AS IACDATE,PRESENTPOSTING,ZONE,
						   SOURCE,ISNULL(CONVERT(VARCHAR(50),SOURCEDATE,103),'') AS SOURCEDATE,SOURCEREF,ACCOUNTNAME,
						   STATUSCODE,ISNULL(CONVERT(VARCHAR(50),SENTFORINVDATE,103),'') AS SENTFORINVDATE,SENTTO,REGION,
						   AMOUNT,ISNULL(CONVERT(VARCHAR(50),DTOFINVREPORT,103),'') AS INVREPORTDATE,DESIGNATION,NAMEOFINVOFFICIAL,
						   CASECLOSE,ISNULL(CONVERT(VARCHAR(50),RYSENT,103),'') AS RYSENTDATE,REASONSFORCLOSURE,
						   ([dbo].ReverseColumnValue_Function(STATUS,RNO,'MISC')) AS SHORTSTATUS,
						   REASONSFORCLOSURE,STATUS,TYPE,FINALACTION,NATURECOMP,NATURE,
						   ISNULL(CONVERT(VARCHAR(50),NPADATE,103),'') AS NPADATE,
						   ISNULL(CONVERT(VARCHAR(50),DTINVESTIGATION,103),'') AS INVESTIGATIONDATE,
						   M.ADDUSER AS ENTRYBY,ISNULL(CONVERT(VARCHAR(50),M.ADDDATE,103),'') AS ENTRYDATE,
						   ISNULL(M.MODUSER,'') AS MODIFYBY,ISNULL(CONVERT(VARCHAR(50),M.MODDATE,103),'') AS MODIFYDATE,
						   (CASE WHEN CLOSUREDT IS NULL THEN 'N' ELSE 'Y' END) AS CLOSURE,DESK_USER_REMARKS, BANKNAME,
						   LETTERSENTTO,
						   ISNULL(CONVERT(VARCHAR(50),LETTERSENTDATE,103),'') AS LETTERSENTDATE,
						   ISNULL(CONVERT(VARCHAR(50),REMINDERDATE,103),'') AS REMINDERDATE,
						   ISNULL(CONVERT(VARCHAR(50),REPLYRECEIVEDDATE,103),'') AS REPLYRECEIVEDDATE,PFNO,
						   NEWZONE AS NEWZONE,NEWCIRCLE AS NEWCIRCLE,ZONE_TYPE, ZONE_CM,
						   CA.ApprovalStatus AS APPROVALSTATUS,
						   CASE CA.ApprovalStatus
								WHEN 'P' THEN 'Pending Approval'
								WHEN 'A' THEN 'Approved'
								WHEN 'C' THEN 'Changes Requested'
								WHEN 'X' THEN 'Rejected'
						   END AS APPROVALSTATUSTEXT,
						   ISNULL(CA.CheckerRemarks,'') AS CHECKERREMARKS,
						   ISNULL(CA.MakerUser,'') AS MAKERUSER,
						   ISNULL(CONVERT(VARCHAR(50),CA.MakerDate,103),'') AS MAKERDATE
					FROM MISC M LEFT JOIN CASE_APPROVAL CA ON CA.ModuleCode='MISC' AND CA.RecordCode=M.CODE
					WHERE M.ACTIVE='Y' AND (CASE WHEN @p_VIEW='GET' THEN M.RNO ELSE CAST(M.CODE AS VARCHAR) END)=@p_SEARCHNO
					ORDER BY RNO;
				END
			ELSE
				BEGIN
					SET @o_ERRCODE= -1;
					SET @o_EERMSG= @p_SEARCHNO + ' - MISC Number does not Exists......!';
				END
		END
END
GO

-------------------------------------------------------------------------------------------------
-- 5. dbo.spMISCExcel_Import  -- the bulk path
--    (full body below is the complete, current definition -- not a diff)
--
--    Two optional parameters were APPENDED with defaults (@p_NEWZONESOLID,
--    @p_NEWCIRCLESOLID), so the existing call site keeps working untouched. The
--    upload sheet has no such column today; when one is added, the import can be
--    switched to require checking with a single UPDATE (see step 1).
--
--    Transaction handling: funcExcelImport_MISC does not open one today, but the IAC
--    caller does. The proc therefore only opens its own when @@TRANCOUNT = 0 and never
--    commits or rolls back a transaction it did not start -- doing so would silently
--    decide the fate of the caller's entire batch.
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spMISCExcel_Import]
(
	@p_RNO					varchar(50),
    @p_COMPNO				varchar(50),
    @p_ACCUSED				varchar(100),
    @p_DESIGNATION			varchar(50),
    @p_PRESENTPOSTING		varchar(100),
    @p_BRCOMPLAINT			varchar(100),
    @p_ZONE					varchar(100),
    @p_CIRCLEOFFICE			varchar(100),
    @p_REGION				varchar(50),
    @p_RECDATECOMP			DATETIME=NULL,
    @p_NPADATE				DATETIME=NULL,
    @p_SOURCE				varchar(50),
    @p_SOURCEREF			varchar(50),
    @p_SOURCEDATE			DATETIME=NULL,
    @p_DTINVESTIGATION		DATETIME=NULL,
    @p_SENTTO				varchar(50),
    @p_SENTFORINVDATE		DATETIME=NULL,
    @p_CATANO				bigint,
    @p_CATBNO				bigint,
    @p_ASNO					bigint,
    @p_NATURECOMP			varchar(250),
    @p_ACCOUNTNAME			varchar(100),
    @p_AMOUNT				decimal(19, 4),
    @p_ALLEGATIONS			varchar(250),
    @p_REMINDERS			varchar(250),
    @p_DTIAC				DATETIME=NULL,
    @p_STATUS				varchar(MAX),
    @p_STATUSCODE			varchar(50),
    @p_PENDINGWITH			varchar(50),
    @p_NAMEOFINVOFFICIAL	varchar(100),
    @p_DTOFINVREPORT		DATETIME=NULL,
    @p_DAYSTAKEN			bigint,
    @p_FINALACTION			varchar(200),
    @p_CASENO				varchar(50),
    @p_CASECLOSE			varchar(5),
    @p_CLOSUREDT			DATETIME=NULL,
    @p_TYPE					varchar(100),
    @p_RYSENT				DATETIME=NULL,
    @p_APLAN				varchar(50),
    @p_REGISTER				varchar(50),
    @p_NATURE				varchar(50),
    @p_REASONSFORCLOSURE	varchar(MAX),
    @p_BANKNAME				varchar(20),

    @p_ADDUSER				VARCHAR(10),
	@p_ADDUSERIP			VARCHAR(20),
	@o_EERMSG				VARCHAR(MAX) OUTPUT,
	@o_ERRCODE				INT OUTPUT,

	@p_NEWZONESOLID			VARCHAR(10)=NULL,
	@p_NEWCIRCLESOLID		VARCHAR(10)=NULL
)
AS
BEGIN
	DECLARE @NEWCODE BIGINT, @IMPORTSTATUS CHAR(1), @OWNTRAN BIT = 0;

	IF EXISTS (SELECT 1 FROM MISC WHERE RNO=@p_RNO AND ACTIVE<>'N')
		BEGIN
			SET @o_ERRCODE = '-1';
			SET @o_EERMSG=@p_RNO + ' - MISC Number does''t save because already Exists in Table, please change RNO......!';
			RETURN;
		END

	--MAKER-CHECKER: WHAT IMPORTED RECORDS LAND AS IS CONFIGURATION, NOT CODE.
	--NULL MEANS MISC IS NOT REGISTERED (OR IS INACTIVE), IN WHICH CASE THE IMPORT
	--BEHAVES EXACTLY AS IT DID BEFORE AND REGISTERS NOTHING.
	SELECT @IMPORTSTATUS = ImportApprovalStatus
	FROM   dbo.WORKFLOW_MODULE
	WHERE  ModuleCode = 'MISC' AND IsActive = 1;

	--A ROW IMPORTED PENDING WITH NO ZONE COULD NEVER REACH A CHECKER, AND THE MAKER
	--COULD NOT EDIT IT EITHER. REFUSING IS THE ONLY OUTCOME THAT IS NOT A SILENT TRAP.
	IF (@IMPORTSTATUS = 'P' AND ISNULL(LTRIM(RTRIM(@p_NEWZONESOLID)),'') = '')
		BEGIN
			SET @o_ERRCODE = -2;
			SET @o_EERMSG = @p_RNO + ' - Zone (SOL ID) is required, because imported MISC records need checker verification......!';
			RETURN;
		END

	BEGIN TRY
		--ONLY OPEN A TRANSACTION IF THE CALLER HAS NOT ALREADY DONE SO. NEVER COMMIT OR
		--ROLL BACK ONE WE DID NOT START -- THAT WOULD DECIDE THE FATE OF THE WHOLE SHEET.
		IF (@@TRANCOUNT = 0)
			BEGIN
				BEGIN TRAN;
				SET @OWNTRAN = 1;
			END

		INSERT INTO MISC(RNO,COMPNO,ACCUSED,DESIGNATION,PRESENTPOSTING,BRCOMPLAINT,ZONE,CIRCLEOFFICE,
					REGION,RECDATECOMP,NPADATE,SOURCE,SOURCEREF,SOURCEDATE,DTINVESTIGATION,SENTTO,SENTFORINVDATE,
					CATANO,CATBNO,ASNO,NATURECOMP,ACCOUNTNAME,AMOUNT,ALLEGATIONS,REMINDERS,DTIAC,STATUS,
					STATUSCODE,PENDINGWITH,NAMEOFINVOFFICIAL,DTOFINVREPORT,DAYSTAKEN,FINALACTION,CASENO,
					CASECLOSE,CLOSUREDT,TYPE,RYSENT,APLAN,REGISTER,NATURE,REASONSFORCLOSURE,BANKNAME,
					NEWZONE,NEWCIRCLE,
					ACTIVE,ADDUSER,ADDDATE,ADDUSERIP,CHANNEL)
				 VALUES (@p_RNO,@p_COMPNO,@p_ACCUSED,@p_DESIGNATION,@p_PRESENTPOSTING,@p_BRCOMPLAINT,@p_ZONE,@p_CIRCLEOFFICE,
						@p_REGION,@p_RECDATECOMP,@p_NPADATE,@p_SOURCE,@p_SOURCEREF,@p_SOURCEDATE,@p_DTINVESTIGATION,@p_SENTTO,@p_SENTFORINVDATE,
						@p_CATANO,@p_CATBNO,@p_ASNO,@p_NATURECOMP,@p_ACCOUNTNAME,@p_AMOUNT,@p_ALLEGATIONS,@p_REMINDERS,@p_DTIAC,@p_STATUS,
						@p_STATUSCODE,@p_PENDINGWITH,@p_NAMEOFINVOFFICIAL,@p_DTOFINVREPORT,@p_DAYSTAKEN,@p_FINALACTION,@p_CASENO,
						@p_CASECLOSE,@p_CLOSUREDT,@p_TYPE,@p_RYSENT,@p_APLAN,@p_REGISTER,@p_NATURE,@p_REASONSFORCLOSURE,@p_BANKNAME,
						NULLIF(LTRIM(RTRIM(@p_NEWZONESOLID)),''),NULLIF(LTRIM(RTRIM(@p_NEWCIRCLESOLID)),''),
						'Y',@p_ADDUSER,GETDATE(),@p_ADDUSERIP,'EXCEL UPLOAD')

		SET @NEWCODE = SCOPE_IDENTITY();

		--UPDATE TBALE MISC_HISTORY
		INSERT INTO MISC_HISTORY SELECT * FROM MISC WHERE CODE=@NEWCODE;

		--MAKER-CHECKER: REGISTER THE IMPORTED RECORD. 'IMPORTED' IS KEPT DISTINCT FROM
		--'SUBMITTED' SO BULK UPLOADS CAN BE TOLD APART FROM TYPED ENTRY IN REPORTING.
		IF (@IMPORTSTATUS IS NOT NULL)
			BEGIN
				INSERT INTO CASE_APPROVAL
					(ModuleCode, RecordCode, RecordRef, ZoneSolID, ApprovalStatus, MakerUser, MakerDate,
					 CheckerUser, CheckerDate, CheckerRemarks)
				VALUES
					('MISC', @NEWCODE, @p_RNO, NULLIF(LTRIM(RTRIM(@p_NEWZONESOLID)),''), @IMPORTSTATUS, @p_ADDUSER, GETDATE(),
					 CASE WHEN @IMPORTSTATUS = 'A' THEN 'SYSTEM' END,
					 CASE WHEN @IMPORTSTATUS = 'A' THEN GETDATE() END,
					 CASE WHEN @IMPORTSTATUS = 'A' THEN 'Bulk import exempted from checking by module configuration.' END);

				INSERT INTO CASE_APPROVAL_HISTORY
					(ModuleCode, RecordCode, ActionType, ActionBy, Remarks, UserIP)
				VALUES
					('MISC', @NEWCODE, 'IMPORTED', @p_ADDUSER,
					 CASE WHEN @IMPORTSTATUS = 'A'
						  THEN 'Registered Approved: bulk import exempted from checking by module configuration.'
					 END,
					 @p_ADDUSERIP);
			END

		IF (@OWNTRAN = 1) COMMIT TRAN;

		SET @o_ERRCODE=1;
		SET @o_EERMSG='Record Saved Sucessfully......!';
	END TRY
	BEGIN CATCH
		IF (@OWNTRAN = 1 AND XACT_STATE() <> 0) ROLLBACK TRAN;
		--RE-THROW, EXACTLY AS THE UNHANDLED VERSION DID, SO THE CALLER'S EXISTING CATCH
		--STILL SEES THE FAILURE AND CAN REPORT THE OFFENDING ROW.
		THROW;
	END CATCH
END
GO

-------------------------------------------------------------------------------------------------
-- 6. Post-deployment check
-------------------------------------------------------------------------------------------------
/*
SELECT ModuleCode, ModuleName, GroupCode, ImportApprovalStatus, IsActive FROM dbo.WORKFLOW_MODULE;
SELECT ApprovalStatus, COUNT(*) FROM dbo.CASE_APPROVAL WHERE ModuleCode='MISC' GROUP BY ApprovalStatus;
SELECT * FROM dbo.vw_CASE_APPROVAL_ORPHANS WHERE ModuleCode = 'MISC';
*/
