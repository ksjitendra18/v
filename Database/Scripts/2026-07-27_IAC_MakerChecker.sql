/*
    IAC Maker-Checker
    =================================================================================
    Database : VigilanceMISDB
    Requires : 2026-07-27_CaseApproval_Central_MakerChecker_Foundation.sql

    Puts the IAC module on the central CASE_APPROVAL registry. No column is added to
    IAC or IAC_HISTORY, so the INSERT INTO IAC_HISTORY SELECT * FROM IAC statements in
    spIACStructure_Update / spIACUser_Update / spIACExcel_Import stay ordinally safe.

    Changes
    -------
    1. Backfill  : existing IAC rows are grandfathered as Approved (see note below).
    2. spIACStructure_Update
         insert -> registers the record as Pending and logs SUBMITTED
         update -> blocked when Pending or Rejected; re-queued to Pending (+ RESUBMITTED)
                   when the prior status was Approved or Changes Requested
         both   -> Zone (New) is now mandatory for a maker, because the zone is what
                   routes the record to a checker. Without it the record would sit
                   Pending forever, invisible to every inbox and locked from editing.
    3. spIACStructure_View : LIST / SEARCH / GET now return APPROVALSTATUS,
                             APPROVALSTATUSTEXT and CHECKERREMARKS.
    4. spIACUser_Update    : closes the bypass -- changing the DA on an approved record
                             now sends it back to the checker instead of leaving it
                             approved and unverified.

    Backfill note
    -------------
    Step 1 marks every pre-existing IAC row Approved, on the basis that those records
    predate the control and were never anyone's to check. Flipping them to 'P' instead
    would flood the checker inbox and lock every existing record from editing. Change
    the literal in step 1 if the business wants the opposite.

    Safe to re-run.
*/

SET NOCOUNT ON;
GO

-------------------------------------------------------------------------------------------------
-- 1. Backfill: grandfather pre-existing IAC records
-------------------------------------------------------------------------------------------------
INSERT INTO dbo.CASE_APPROVAL
    (ModuleCode, RecordCode, RecordRef, ZoneSolID, ApprovalStatus, MakerUser, MakerDate, CheckerUser, CheckerDate, CheckerRemarks)
SELECT  'IAC',
        I.SNO,
        I.IACNO,
        I.NEWZONE,
        'A',
        ISNULL(I.ADDUSER, 'SYSTEM'),
        ISNULL(I.ADDDATE, GETDATE()),
        'SYSTEM',
        GETDATE(),
        'Pre-existing record grandfathered at maker-checker rollout.'
FROM    dbo.IAC I
WHERE   I.ACTIVE = 'Y'
  AND   NOT EXISTS (SELECT 1 FROM dbo.CASE_APPROVAL CA
                    WHERE CA.ModuleCode = 'IAC' AND CA.RecordCode = I.SNO);
GO

INSERT INTO dbo.CASE_APPROVAL_HISTORY (ModuleCode, RecordCode, ActionType, ActionBy, Remarks)
SELECT  'IAC', CA.RecordCode, 'GRANDFATHERED', 'SYSTEM',
        'Pre-existing record grandfathered at maker-checker rollout.'
FROM    dbo.CASE_APPROVAL CA
WHERE   CA.ModuleCode = 'IAC'
  AND   CA.CheckerUser = 'SYSTEM'
  AND   NOT EXISTS (SELECT 1 FROM dbo.CASE_APPROVAL_HISTORY H
                    WHERE H.ModuleCode = 'IAC' AND H.RecordCode = CA.RecordCode
                      AND H.ActionType = 'GRANDFATHERED');
GO

-------------------------------------------------------------------------------------------------
-- 2. dbo.spIACStructure_Update
--    (full body below is the complete, current definition -- not a diff)
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spIACStructure_Update]
(
	@p_SNO					VARCHAR(50),
	@p_IACNO				VARCHAR(50),
	@p_RECDATECOMP			DATETIME=NULL,
	@p_BRCOMPLAINT			VARCHAR(100),
	@p_CIRCLEOFFICE			VARCHAR(100)=NULL,
	@p_VIGNO				VARCHAR(50),
	@p_CLOSUREDT			DATETIME=NULL,
	@p_ACCUSED				VARCHAR(100),
	@p_DAVIEW				VARCHAR(50),
	@p_MEETNO				VARCHAR(100),
	@p_RETDATE				DATETIME=NULL,
	@p_IACVIEW				VARCHAR(100),
	@p_ZONE					VARCHAR(100)=NULL,
	@p_SOURCE				VARCHAR(50)=NULL,
	@p_DA					VARCHAR(50),
	@p_CVOVIEW				VARCHAR(50),
	@p_ACCOUNTNAME			VARCHAR(100),
	@p_AMOUNT				DECIMAL(19,4),
	@p_IACNO1				VARCHAR(50),
	@p_PFNUMBER				VARCHAR(100),
	@p_STATUSCODE			VARCHAR(50)=NULL,
	@p_NATURE				VARCHAR(100)=NULL,
	@p_STATUS				VARCHAR(MAX),
	@p_HOSTATUS				VARCHAR(250)=NULL,
	@p_CLOSURE				VARCHAR(1),
	@p_MODE					CHAR(1),
	@p_USER					VARCHAR(50),
	@p_USERROLE				VARCHAR(50),
	@p_DESK_USER_REMARKS	VARCHAR(MAX),
	@p_USERIP				VARCHAR(20),
	@o_EERMSG				VARCHAR(MAX) OUTPUT,
	@o_ERRCODE				INT OUTPUT,
	@p_BANKNAME				VARCHAR(20),
	@p_LETTERSENTTO			VARCHAR(10)=NULL,
	@p_LETTERSENTDATE		DATETIME=NULL,
	@p_REMINDERDATE			DATETIME=NULL,
	@p_REPLYRECEIVEDDATE	DATETIME=NULL,
	@p_LETTERSENTTODADATE	DATETIME=NULL,
	@p_ZONENEW				VARCHAR(10)=NULL,
	@p_CIRCLENEW			VARCHAR(10)=NULL,
	@p_TMSACREFNO			VARCHAR(50)=NULL,
	@p_DESIGNATION			VARCHAR(50)=NULL,
	@p_SCALE				VARCHAR(5)=NULL,
	@p_ABBFFCASE			VARCHAR(3),
	@p_ABBFFREFNO			VARCHAR(20),
	@p_ABBFFADVICEDETAILS	VARCHAR(MAX),
	@p_ABBFFCASESUBMISSIONDATE		DATETIME=NULL,
	@p_ABBFFREPLYDATE				DATETIME=NULL,
	@p_ABBFFADVICERECEIVEDATE		DATETIME=NULL
)
AS
BEGIN
	DECLARE @STATUS	VARCHAR(MAX),@UPDATESTATUS VARCHAR(MAX);
	DECLARE @OLDAPPROVALSTATUS CHAR(1), @SNO BIGINT, @NEWSNO BIGINT;
	------------------------------------------------------------------------------------------
	SET @o_ERRCODE=0;
	SET @SNO = TRY_CONVERT(BIGINT, @p_SNO);
	----------------------------------------------------------------------------------------------------------------------
		IF(@p_IACNO <> '')
			BEGIN
				IF EXISTS (SELECT 1 FROM IAC WHERE IACNO=@p_IACNO AND ACTIVE='Y' AND SNO <> @p_SNO)
					BEGIN
						SET @o_ERRCODE=3;
						SET @o_EERMSG='IAC Number alredy Exists......!';
					END
			END
		IF(@p_USERROLE = 'VMIS_MISUSER')
			BEGIN
				--MAKER-CHECKER: THE ZONE IS WHAT ROUTES A RECORD TO A CHECKER. A RECORD SAVED
				--WITHOUT ONE WOULD SIT PENDING FOREVER, INVISIBLE TO EVERY INBOX AND LOCKED
				--FROM EDITING, SO REFUSE THE SAVE INSTEAD.
				IF(@o_ERRCODE = 0 AND ISNULL(LTRIM(RTRIM(@p_ZONENEW)),'') = '')
					BEGIN
						SET @o_ERRCODE=4;
						SET @o_EERMSG='Zone (New) is mandatory. It decides which checker verifies this record.';
					END

				--MAKER-CHECKER: A RECORD AWAITING VERIFICATION, OR ONE THE CHECKER HAS REJECTED,
				--IS NOT THE MAKER''S TO EDIT. THE GRID ALREADY DISABLES THE BUTTON; THIS BLOCKS
				--A DIRECT POSTBACK OR A SECOND ENTRY PAGE FROM GETTING PAST IT.
				IF(@o_ERRCODE = 0 AND @p_MODE = 'U')
					BEGIN
						SELECT @OLDAPPROVALSTATUS = ApprovalStatus
						FROM   CASE_APPROVAL
						WHERE  ModuleCode = 'IAC' AND RecordCode = @SNO;

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

								INSERT INTO IAC (IACNO,RECDT,NAMEOFTHEBRANCH,CIRCLEOFFICE,
												 VIGNO,ACCUSED,DAVIEW,
												 MEETNO,DTRET,IACVIEW,ZONE,
												 SOURCE,DA,CVOVIEW,ACNAME,
												 AMOUNT,IACNO_1,PFNUMBER,STATUSCODE,
												 STATUS,NATURECASE,CLOSUREDT,
												 ADDUSER,ADDDATE,ADDUSERIP,BANKNAME,
												 LETTERSENTTO,LETTERSENTDATE,REMINDERDATE,REPLYRECEIVEDDATE,LETTERSENTTODADATE,
												 NEWZONE,NEWCIRCLE,TMSAC_REF,DESIGNATION,SCALE,
												 IAC_ABBFF_CASE,IAC_ABBFF_CASE_SUBMISSION_DATE,IAC_ABBFF_REPLY_DATE,
												 IAC_ABBFF_REFNO,IAC_ABBFF_ADVICE_RECEIVE_DATE,IAC_ABBFF_ADVICE_DETAIL)
									   VALUES (@p_IACNO,@p_RECDATECOMP,@p_BRCOMPLAINT,@p_CIRCLEOFFICE,
											   @p_VIGNO,@p_ACCUSED,@p_DAVIEW,
											   @p_MEETNO,@p_RETDATE,@p_IACVIEW,@p_ZONE,
											   @p_SOURCE,@p_DA,@p_CVOVIEW,@p_ACCOUNTNAME,
											   @p_AMOUNT,@p_IACNO1,@p_PFNUMBER,@p_STATUSCODE,
											   @p_STATUS,@p_NATURE,
											   (CASE WHEN(@p_CLOSURE = 'Y') THEN GETDATE() ELSE @p_CLOSUREDT END),
											   @p_USER,GETDATE(),@p_USERIP,@p_BANKNAME,
											   @p_LETTERSENTTO,@p_LETTERSENTDATE,@p_REMINDERDATE,@p_REPLYRECEIVEDDATE,@p_LETTERSENTTODADATE,
											   @p_ZONENEW,@p_CIRCLENEW,@p_TMSACREFNO,@p_DESIGNATION,@p_SCALE,
											   @p_ABBFFCASE,@p_ABBFFCASESUBMISSIONDATE,@p_ABBFFREPLYDATE,
											   @p_ABBFFREFNO,@p_ABBFFADVICERECEIVEDATE,@p_ABBFFADVICEDETAILS)

								SET @NEWSNO = SCOPE_IDENTITY();

								--MAKER-CHECKER: REGISTER THE NEW RECORD AS PENDING VERIFICATION
								INSERT INTO CASE_APPROVAL
									(ModuleCode, RecordCode, RecordRef, ZoneSolID, ApprovalStatus, MakerUser, MakerDate)
								VALUES
									('IAC', @NEWSNO, @p_IACNO, @p_ZONENEW, 'P', @p_USER, GETDATE());

								INSERT INTO CASE_APPROVAL_HISTORY
									(ModuleCode, RecordCode, ActionType, ActionBy, Remarks, UserRole, UserIP)
								VALUES
									('IAC', @NEWSNO, 'SUBMITTED', @p_USER, NULL, @p_USERROLE, @p_USERIP);

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

								--UPDATE TBALE IAC_HISTORY
								INSERT INTO IAC_HISTORY SELECT * FROM IAC WHERE SNO=@p_SNO;

								--UPDATE DATA OF IAC
								UPDATE IAC SET IACNO=@p_IACNO,
											   RECDT=@p_RECDATECOMP,
											   NAMEOFTHEBRANCH=@p_BRCOMPLAINT,
											   CIRCLEOFFICE=@p_CIRCLEOFFICE,
											   VIGNO=@p_VIGNO,
											   CLOSUREDT=(CASE WHEN(@p_CLOSURE = 'Y') THEN GETDATE() ELSE @p_CLOSUREDT END),
											   ACCUSED=@p_ACCUSED,
											   DAVIEW=@p_DAVIEW,
											   MEETNO=@p_MEETNO,
											   DTRET=@p_RETDATE,
											   IACVIEW=@p_IACVIEW,
											   ZONE=@p_ZONE,
											   SOURCE=@p_SOURCE,
											   DA=@p_DA,
											   CVOVIEW=@p_CVOVIEW,
											   ACNAME=@p_ACCOUNTNAME,
											   AMOUNT=@p_AMOUNT,
											   IACNO_1=@p_IACNO1,
											   PFNUMBER=@p_PFNUMBER,
											   STATUSCODE=@p_STATUSCODE,
											   STATUS=@p_STATUS,
											   NATURECASE=@p_NATURE,
											   MODUSER=@p_USER,
											   MODDATE=GETDATE(),
											   MODUSERIP=@p_USERIP,
											   BANKNAME=@p_BANKNAME,
											   LETTERSENTTO=@p_LETTERSENTTO,
											   LETTERSENTDATE=@p_LETTERSENTDATE,
											   REMINDERDATE=@p_REMINDERDATE,
											   REPLYRECEIVEDDATE=@p_REPLYRECEIVEDDATE,
											   LETTERSENTTODADATE=	@p_LETTERSENTTODADATE,
											   NEWZONE=@p_ZONENEW,
											   NEWCIRCLE=@p_CIRCLENEW,
											   TMSAC_REF = @p_TMSACREFNO,
											   DESIGNATION = @p_DESIGNATION,
											   SCALE = @p_SCALE,
											   IAC_ABBFF_CASE=@p_ABBFFCASE,
											   IAC_ABBFF_CASE_SUBMISSION_DATE=@p_ABBFFCASESUBMISSIONDATE,
											   IAC_ABBFF_REPLY_DATE=@p_ABBFFREPLYDATE,
											   IAC_ABBFF_REFNO=@p_ABBFFREFNO,
											   IAC_ABBFF_ADVICE_RECEIVE_DATE=@p_ABBFFADVICERECEIVEDATE,
											   IAC_ABBFF_ADVICE_DETAIL=@p_ABBFFADVICEDETAILS
								WHERE SNO=@p_SNO;

								IF(@OLDAPPROVALSTATUS IS NULL)
									BEGIN
										--A RECORD THAT PREDATES THE ROLLOUT, OR ONE CREATED BY AN IMPORT.
										--ONBOARD IT NOW SO THE EDIT IS VERIFIED.
										INSERT INTO CASE_APPROVAL
											(ModuleCode, RecordCode, RecordRef, ZoneSolID, ApprovalStatus, MakerUser, MakerDate)
										VALUES
											('IAC', @SNO, @p_IACNO, @p_ZONENEW, 'P', @p_USER, GETDATE());

										INSERT INTO CASE_APPROVAL_HISTORY
											(ModuleCode, RecordCode, ActionType, ActionBy, Remarks, UserRole, UserIP)
										VALUES
											('IAC', @SNO, 'SUBMITTED', @p_USER, NULL, @p_USERROLE, @p_USERIP);
									END
								ELSE
									BEGIN
										--AN EDIT INVALIDATES A PREVIOUS DECISION, SO SEND THE RECORD BACK
										--TO THE CHECKER'S QUEUE. KEEP THE REFERENCE AND ZONE IN STEP WITH
										--THE CASE ROW SO ROUTING FOLLOWS A ZONE CHANGE.
										UPDATE CASE_APPROVAL
										   SET RecordRef      = @p_IACNO,
											   ZoneSolID      = @p_ZONENEW,
											   ApprovalStatus = 'P',
											   MakerUser      = @p_USER,
											   MakerDate      = GETDATE(),
											   CheckerUser    = NULL,
											   CheckerDate    = NULL,
											   CheckerRemarks = NULL
										 WHERE ModuleCode = 'IAC' AND RecordCode = @SNO;

										INSERT INTO CASE_APPROVAL_HISTORY
											(ModuleCode, RecordCode, ActionType, ActionBy, Remarks, UserRole, UserIP)
										VALUES
											('IAC', @SNO, 'RESUBMITTED', @p_USER, NULL, @p_USERROLE, @p_USERIP);
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
				--UPDATE TBALE IAC_HISTORY
				INSERT INTO IAC_HISTORY SELECT * FROM IAC WHERE SNO=@p_SNO;

				--SELECT STATUS FROM IAC FOR APPEND STATUS IN CASE OF VMIS_DESKUSER USER
				SELECT @STATUS=ISNULL(STATUS,'') FROM IAC WHERE SNO=@p_SNO;

				SET @UPDATESTATUS = @p_HOSTATUS + ' | ' + @STATUS;

				UPDATE IAC SET STATUS=@UPDATESTATUS,
								DESK_USER_REMARKS=@p_DESK_USER_REMARKS,
								DESK_USER_ID=@p_USER,
								DESK_USER_IP=@p_USERIP,
								DESK_USER_ADDDATE=GETDATE(),
								DESK_USER_ROLE=@p_USERROLE
				WHERE SNO=@p_SNO;

				SET @o_ERRCODE= 1;
				SET @o_EERMSG='Dealing Officer Remarks Updated Sucessfully......!'
			END
END
GO

-------------------------------------------------------------------------------------------------
-- 3. dbo.spIACStructure_View
--    (full body below is the complete, current definition -- not a diff)
--    Adds APPROVALSTATUS / APPROVALSTATUSTEXT / CHECKERREMARKS from CASE_APPROVAL.
--    LEFT JOIN, so records that predate the workflow still list with NULL status.
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spIACStructure_View]
(
	@p_VIEW			VARCHAR(50)=NULL,
	@p_SEARCHNO		VARCHAR(50)=NULL,
	@p_ACCOUNTNAME	VARCHAR(50)=NULL,
	@p_PFNUMBER		VARCHAR(50)=NULL,
	@p_ACCUSED		VARCHAR(50)=NULL,
	@p_STATUS		VARCHAR(100)=NULL,
	@p_BRANCH		VARCHAR(100)=NULL,
	@p_CIRCLE		VARCHAR(100)=NULL,
	@o_EERMSG		VARCHAR(MAX)=NULL OUTPUT,
	@o_ERRCODE		INT=NULL OUTPUT
)
AS
BEGIN
	DECLARE @SQL VARCHAR(MAX),@STRCOND VARCHAR(MAX),@ERRORCODE VARCHAR(5);
	--------------------------------------------------------------------------------------
	SET @o_ERRCODE=0;
	SET @STRCOND = 'I.ACTIVE=''Y''';
	--------------------------------------------------------------------------------------
	IF(@p_SEARCHNO <> '' AND @p_SEARCHNO IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND IACNO LIKE''%'+@p_SEARCHNO+'%''' + CHAR(13);
			END
	IF(@p_ACCOUNTNAME <> '' AND @p_ACCOUNTNAME IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND ACNAME LIKE''%'+@p_ACCOUNTNAME+'%''' + CHAR(13);
			END
	IF(@p_ACCUSED <> '' AND @p_ACCUSED IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND ACCUSED LIKE''%'+@p_ACCUSED+'%''' + CHAR(13);
			END
	IF(@p_PFNUMBER <> '' AND @p_PFNUMBER IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND (PFNUMBER LIKE''%'+@p_PFNUMBER+'%'' OR PFNUMBER_OLD LIKE''%'+@p_PFNUMBER+'%'')' + CHAR(13);
			END
	IF(@p_STATUS <> '' AND @p_STATUS IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND STATUS LIKE''%'+@p_STATUS+'%''' + CHAR(13);
			END
	IF(@p_BRANCH <> '' AND @p_BRANCH IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND NAMEOFTHEBRANCH LIKE''%'+@p_BRANCH+'%''' + CHAR(13);
			END
	IF(@p_CIRCLE <> '' AND @p_CIRCLE IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND CIRCLEOFFICE LIKE''%'+@p_CIRCLE+'%''' + CHAR(13);
			END
	---------------------------------------------------------------------------------------------------------------------------------------
	IF(UPPER(@p_VIEW) = 'LIST')
		BEGIN
			SELECT TOP 20 SNO,IACNO,VIGNO,NAMEOFTHEBRANCH AS BRCOMPLAINT,CIRCLEOFFICE,
				   ACCUSED,DAVIEW,IACVIEW,ZONE,IACNO_1 AS IACNO1,MEETNO,
				   SOURCE,DA,CVOVIEW,ACNAME AS ACCOUNTNAME,ISNULL(AMOUNT,0) AS AMOUNT,PFNUMBER,
				   STATUSCODE,N.CODE AS NATURECODE,N.NATURECASE AS NATURE,STATUS,
				   CA.ApprovalStatus AS APPROVALSTATUS,
				   CASE CA.ApprovalStatus
						WHEN 'P' THEN 'Pending Approval'
						WHEN 'A' THEN 'Approved'
						WHEN 'C' THEN 'Changes Requested'
						WHEN 'X' THEN 'Rejected'
				   END AS APPROVALSTATUSTEXT,
				   ISNULL(CA.CheckerRemarks,'') AS CHECKERREMARKS,
				   I.ADDUSER AS ENTRYBY,ISNULL(I.MODUSER,'') AS MODIFYBY,
				   ISNULL(CONVERT(VARCHAR(50),RECDT,103),'') AS RECDATE,
				   ISNULL(CONVERT(VARCHAR(50),CLOSUREDT,103),'') AS CLOSUREDATE,
				   ISNULL(CONVERT(VARCHAR(50),DTRET,103),'') AS RETDATE,
				   ISNULL(CONVERT(VARCHAR(50),I.ADDDATE,103),'') AS ENTRYDATE,
				   ISNULL(CONVERT(VARCHAR(50),I.MODDATE,103),'') AS MODIFYDATE,
				   ([dbo].ReverseColumnValue_Function(STATUS,SNO,'IAC')) AS SHORTSTATUS,
				   (CASE WHEN CLOSUREDT IS NULL THEN 'N' ELSE 'Y' END) AS CLOSURE
			FROM (IAC I LEFT JOIN NATURECASE N ON I.NATURECASE=N.CODE AND FORTABLE='IAC')
				 LEFT JOIN CASE_APPROVAL CA ON CA.ModuleCode='IAC' AND CA.RecordCode=I.SNO
			WHERE I.ACTIVE='Y'
			ORDER BY I.ADDDATE DESC;
		END

	ELSE IF(UPPER(@p_VIEW) = 'SEARCH')
		BEGIN
			SET @SQL='SELECT TOP 20 SNO,IACNO,VIGNO,NAMEOFTHEBRANCH AS BRCOMPLAINT,CIRCLEOFFICE,
							 ACCUSED,DAVIEW,IACVIEW,ZONE,IACNO_1 AS IACNO1,MEETNO,
							 SOURCE,DA,CVOVIEW,ACNAME AS ACCOUNTNAME,ISNULL(AMOUNT,0) AS AMOUNT,PFNUMBER,
							 STATUSCODE,N.CODE AS NATURECODE,N.NATURECASE AS NATURE,STATUS,
							 CA.ApprovalStatus AS APPROVALSTATUS,
							 CASE CA.ApprovalStatus
								  WHEN ''P'' THEN ''Pending Approval''
								  WHEN ''A'' THEN ''Approved''
								  WHEN ''C'' THEN ''Changes Requested''
								  WHEN ''X'' THEN ''Rejected''
							 END AS APPROVALSTATUSTEXT,
							 ISNULL(CA.CheckerRemarks,'''') AS CHECKERREMARKS,
							 I.ADDUSER AS ENTRYBY,ISNULL(I.MODUSER,'''') AS MODIFYBY,
							 ISNULL(CONVERT(VARCHAR(50),RECDT,103),'''') AS RECDATE,
							 ISNULL(CONVERT(VARCHAR(50),CLOSUREDT,103),'''') AS CLOSUREDATE,
							 ISNULL(CONVERT(VARCHAR(50),DTRET,103),'''') AS RETDATE,
							 ISNULL(CONVERT(VARCHAR(50),I.ADDDATE,103),'''') AS ENTRYDATE,
							 ISNULL(CONVERT(VARCHAR(50),I.MODDATE,103),'''') AS MODIFYDATE,
							 ([dbo].ReverseColumnValue_Function(STATUS,SNO,''IAC'')) AS SHORTSTATUS,
							 (CASE WHEN CLOSUREDT IS NULL THEN ''N'' ELSE ''Y'' END) AS CLOSURE
					  FROM (IAC I LEFT JOIN NATURECASE N ON I.NATURECASE=N.CODE AND FORTABLE=''IAC'')
						   LEFT JOIN CASE_APPROVAL CA ON CA.ModuleCode=''IAC'' AND CA.RecordCode=I.SNO
					  WHERE '+@STRCOND+'
					  ORDER BY I.ADDDATE DESC'
			EXEC(@SQL);
			PRINT(@SQL);
			PRINT(@ERRORCODE);
		END
	ELSE
		BEGIN
			IF EXISTS (SELECT 1 FROM IAC WHERE (CASE WHEN @p_VIEW='GET' THEN IACNO ELSE CAST(SNO AS VARCHAR) END)=@p_SEARCHNO AND ACTIVE='Y')
				BEGIN
					SELECT SNO,IACNO,VIGNO,NAMEOFTHEBRANCH AS BRCOMPLAINT,CIRCLEOFFICE,
						   ACCUSED,DAVIEW,IACVIEW,ZONE,IACNO_1 AS IACNO1,MEETNO,
						   SOURCE,DA,CVOVIEW,ACNAME AS ACCOUNTNAME,ISNULL(AMOUNT,0) AS AMOUNT,PFNUMBER,
						   STATUSCODE,N.CODE AS NATURECODE,N.NATURECASE AS NATURE,STATUS,
						   CA.ApprovalStatus AS APPROVALSTATUS,
						   CASE CA.ApprovalStatus
								WHEN 'P' THEN 'Pending Approval'
								WHEN 'A' THEN 'Approved'
								WHEN 'C' THEN 'Changes Requested'
								WHEN 'X' THEN 'Rejected'
						   END AS APPROVALSTATUSTEXT,
						   ISNULL(CA.CheckerRemarks,'') AS CHECKERREMARKS,
						   I.ADDUSER AS ENTRYBY,ISNULL(I.MODUSER,'') AS MODIFYBY,
						   ISNULL(CONVERT(VARCHAR(50),RECDT,103),'') AS RECDATE,
						   ISNULL(CONVERT(VARCHAR(50),CLOSUREDT,103),'') AS CLOSUREDATE,
						   ISNULL(CONVERT(VARCHAR(50),DTRET,103),'') AS RETDATE,
						   ISNULL(CONVERT(VARCHAR(50),I.ADDDATE,103),'') AS ENTRYDATE,
						   ISNULL(CONVERT(VARCHAR(50),I.MODDATE,103),'') AS MODIFYDATE,
						   ([dbo].ReverseColumnValue_Function(STATUS,SNO,'IAC')) AS SHORTSTATUS,
						   (CASE WHEN CLOSUREDT IS NULL THEN 'N' ELSE 'Y' END) AS CLOSURE,DESK_USER_REMARKS,
						   BANKNAME,LETTERSENTTO,
						   ISNULL(CONVERT(VARCHAR(50),LETTERSENTDATE,103),'') AS LETTERSENTDATE,
						   ISNULL(CONVERT(VARCHAR(50),LETTERSENTTODADATE,103),'') AS LETTERSENTTODADATE,
						   ISNULL(CONVERT(VARCHAR(50),REMINDERDATE,103),'') AS REMINDERDATE,
						   ISNULL(CONVERT(VARCHAR(50),REPLYRECEIVEDDATE,103),'') AS REPLYRECEIVEDDATE,
						   NEWZONE AS NEWZONE, NEWCIRCLE AS NEWCIRCLE,
						   TMSAC_REF AS TMSACREF, DESIGNATION AS DESIGNATION, SCALE AS SCALE,
						   IAC_ABBFF_CASE AS ABBFF_CASE,CONVERT(VARCHAR(50),IAC_ABBFF_CASE_SUBMISSION_DATE,103) AS ABBFF_CASE_SUBMISSION_DATE,
						   CONVERT(VARCHAR(50),IAC_ABBFF_REPLY_DATE,103) AS ABBFF_REPLY_DATE,
						   IAC_ABBFF_REFNO AS ABBFF_REFNO,IAC_ABBFF_ADVICE_DETAIL AS ABBFF_ADVICE_DETAIL,
						   CONVERT(VARCHAR(50),IAC_ABBFF_ADVICE_RECEIVE_DATE,103) AS ABBFF_ADVICE_RECEIVE_DATE
					FROM (IAC I LEFT JOIN NATURECASE N ON I.NATURECASE=N.CODE AND FORTABLE='IAC')
						 LEFT JOIN CASE_APPROVAL CA ON CA.ModuleCode='IAC' AND CA.RecordCode=I.SNO
					WHERE I.ACTIVE='Y' AND (CASE WHEN @p_VIEW='GET' THEN I.IACNO ELSE CAST(I.SNO AS VARCHAR) END)=@p_SEARCHNO
					ORDER BY SNO;
				END
			ELSE
				BEGIN
					SET @o_ERRCODE= -1;
					SET @o_EERMSG= @p_SEARCHNO + ' - IAC Number does not Exists......!';
				END
		END
END
GO

-------------------------------------------------------------------------------------------------
-- 4. dbo.spIACUser_Update  -- close the bulk-update bypass
--
--    This proc (Mis/frmIACUpdate.aspx, MISUSER-only) changes the DA on an existing IAC
--    record. It never touched the approval state, so a maker could alter an approved
--    record and it stayed approved and unverified. Now the change re-queues the record.
--    @p_USERROLE / @p_USERIP are optional so the existing three-parameter call still works.
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spIACUser_Update]
(
	@p_IACNO	VARCHAR(50),
	@p_DA		VARCHAR(100),
	@p_USER		VARCHAR(50),
	@o_EERMSG	VARCHAR(MAX) OUTPUT,
	@o_ERRCODE	INT OUTPUT,
	@p_USERROLE	VARCHAR(50)=NULL,
	@p_USERIP	VARCHAR(30)=NULL
)
AS
BEGIN
	DECLARE @SNO BIGINT, @OLDAPPROVALSTATUS CHAR(1);

	SET @o_ERRCODE=0;
	IF(@p_IACNO <> '')
		BEGIN
			SELECT @SNO = SNO FROM IAC WHERE IACNO=@p_IACNO AND ACTIVE='Y';

			IF(@SNO IS NOT NULL)
				BEGIN
					SELECT @OLDAPPROVALSTATUS = ApprovalStatus
					FROM   CASE_APPROVAL
					WHERE  ModuleCode='IAC' AND RecordCode=@SNO;

					--A REJECTED RECORD IS NOT EDITABLE THROUGH ANY PATH.
					IF(@OLDAPPROVALSTATUS = 'X')
						BEGIN
							SET @o_ERRCODE=3;
							SET @o_EERMSG= 'This record has been rejected by the checker and cannot be edited......!';
							RETURN;
						END

				  BEGIN TRY
					BEGIN TRAN;

					--UPDATE TBALE IAC_HISTORY
					INSERT INTO IAC_HISTORY SELECT * FROM IAC WHERE IACNO=@p_IACNO;
					UPDATE IAC SET DA=@p_DA,MODUSER=@p_USER,MODDATE=GETDATE() WHERE IACNO=@p_IACNO;

					--MAKER-CHECKER: THE EDIT INVALIDATES ANY EARLIER DECISION, SO SEND IT BACK
					--TO THE CHECKER RATHER THAN LEAVING IT APPROVED AND UNVERIFIED.
					IF(@OLDAPPROVALSTATUS IN ('A','C'))
						BEGIN
							UPDATE CASE_APPROVAL
							   SET ApprovalStatus = 'P',
								   MakerUser      = @p_USER,
								   MakerDate      = GETDATE(),
								   CheckerUser    = NULL,
								   CheckerDate    = NULL,
								   CheckerRemarks = NULL
							 WHERE ModuleCode='IAC' AND RecordCode=@SNO;

							INSERT INTO CASE_APPROVAL_HISTORY
								(ModuleCode, RecordCode, ActionType, ActionBy, Remarks, UserRole, UserIP)
							VALUES
								('IAC', @SNO, 'RESUBMITTED', @p_USER, 'DA changed via IAC Update screen.', @p_USERROLE, @p_USERIP);
						END

					COMMIT TRAN;

					SET @o_ERRCODE=1;
					SET @o_EERMSG= 'IAC DA Updated Sucessfully......!';
				  END TRY
				  BEGIN CATCH
					IF (XACT_STATE() <> 0) ROLLBACK TRAN;
					SET @o_ERRCODE=0;
					SET @o_EERMSG= 'Could not update the record: ' + ERROR_MESSAGE();
				  END CATCH
				END
			ELSE
				BEGIN
					SET @o_ERRCODE=2;
					SET @o_EERMSG= 'Invalid IAC Number......!';
				END
		END
END
GO
