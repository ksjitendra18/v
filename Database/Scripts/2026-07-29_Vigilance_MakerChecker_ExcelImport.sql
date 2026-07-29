/*
    Vigilance Maker-Checker -- Excel Import
    =================================================================================
    Database : VigilanceMISDB
    Requires : 2026-07-27_CaseApproval_Central_MakerChecker_Foundation.sql
               2026-07-27_IAC_MakerChecker_ExcelImport.sql  (for WORKFLOW_MODULE.ImportApprovalStatus;
                                                             re-created here if absent)
               2026-07-29_Vigilance_MakerChecker.sql

    Why this exists
    ---------------
    2026-07-29_Vigilance_MakerChecker.sql puts the two entry forms under maker-checker but
    leaves Upload/frmExcelUpload.aspx alone. Leaving the import outside the workflow would
    give anyone a trivial way around the control -- upload a one-row sheet instead of typing
    the record in.

    A pre-existing defect this had to fix first
    -------------------------------------------
    spVigilanceExcel_Import could not be called at all. funcExcelImport_Vigilance passes
    @p_DESK_USER_REMARKS and @p_BANKNAME, which the procedure did not declare, and omits
    @p_TABLENAME and @p_USER, which had no defaults. Either mismatch fails before the body
    runs. The C# also reads TMSACREFNO / BANKNAME / DESK_USER_REMARKS columns that are not
    in VIGILANCE.xlsx, so the sheet threw first. Vigilance Excel upload has therefore been
    non-functional; it is repaired here rather than left broken under a new control.

    Changes
    -------
    1. WORKFLOW_MODULE.ImportApprovalStatus is created if the IAC script has not been run,
       and set for VIGILANCE. This is the business decision -- do bulk-imported records need
       checking? -- expressed as configuration rather than code.

    2. spVigilanceExcel_Import
         - @p_TABLENAME / @p_USER defaulted, so the existing call site binds
         - @p_BANKNAME, @p_DESK_USER_REMARKS, @p_NEWZONESOLID, @p_NEWCIRCLESOLID added,
           all defaulted, and now actually written to VIGILANCE. NEWZONE in particular was
           never populated by the import, which alone would have made every imported record
           unroutable to a checker
         - registers every imported row in CASE_APPROVAL at the configured status and logs
           an IMPORTED row in CASE_APPROVAL_HISTORY
         - rejects a row with no zone (@o_ERRCODE = -2) when imports require checking
         - opens its own transaction only when there is no ambient one, and never commits
           or rolls back the caller's

    Choosing ImportApprovalStatus
    -----------------------------
      'P' (the default set here)
          Imported records are Pending and appear in the checker inbox like any other, and
          rows with a blank NEWZONESOLID are rejected. A large sheet puts every one of its
          rows in front of a checker, and the maker cannot edit any of them until actioned.

      'A'
          Imported records are registered as Approved without a checker seeing them -- an
          explicit, audited exemption rather than a silent gap. They still carry a status,
          appear in reporting, and re-enter the workflow the moment anyone edits them. No
          zone is required, since nothing has to be routed.

    To switch:
        UPDATE dbo.WORKFLOW_MODULE SET ImportApprovalStatus = 'A' WHERE ModuleCode = 'VIGILANCE';

    Run the orphan monitor after every bulk import:
        SELECT * FROM dbo.vw_CASE_APPROVAL_ORPHANS WHERE ModuleCode = 'VIGILANCE';

    Safe to re-run.
*/

SET NOCOUNT ON;
GO

-------------------------------------------------------------------------------------------------
-- 1. WORKFLOW_MODULE.ImportApprovalStatus
-------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('dbo.WORKFLOW_MODULE') AND name = 'ImportApprovalStatus')
BEGIN
    ALTER TABLE dbo.WORKFLOW_MODULE
        ADD ImportApprovalStatus char(1) NOT NULL
            CONSTRAINT DF_WORKFLOW_MODULE_ImportStatus DEFAULT('P');
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_WORKFLOW_MODULE_ImportStatus')
    ALTER TABLE dbo.WORKFLOW_MODULE
        ADD CONSTRAINT CK_WORKFLOW_MODULE_ImportStatus CHECK (ImportApprovalStatus IN ('P','A'));
GO

-- Imported Vigilance records require checking. Change to 'A' to exempt bulk uploads.
UPDATE dbo.WORKFLOW_MODULE SET ImportApprovalStatus = 'P' WHERE ModuleCode = 'VIGILANCE';
GO

-------------------------------------------------------------------------------------------------
-- 2. dbo.spVigilanceExcel_Import
--    (full body below is the complete, current definition -- not a diff)
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spVigilanceExcel_Import]
(
	@p_RNO					VARCHAR(50),
    @p_RNO1					VARCHAR(50),
    @p_NAMEOFPARTICULARS	VARCHAR(100),
    @p_NAME					VARCHAR(100),
    @p_SCALE				VARCHAR(100),
    @p_DESIGNATION			VARCHAR(100),
    @p_BRNAME				VARCHAR(100),
    @p_CIRCLEOFFICE			VARCHAR(100),
    @p_STATE				VARCHAR(100),
    @p_LAPSENATURE			VARCHAR(100),
    @p_SOURCE				VARCHAR(100),
    @p_ACCTT_NAME			VARCHAR(100),
    @p_NATUREOFACCOUNT		VARCHAR(100),
    @p_INVESTIG				VARCHAR(100),
    @p_CBI_RC_NO1			VARCHAR(100),
    @p_CBI_RC_NO2			VARCHAR(100),
    @p_CBI_ZONE				VARCHAR(100),
    @p_RC_SOURCE			VARCHAR(100),
    @p_RECOM_CBI			VARCHAR(100),
    @p_PROPOSEDACTIONTOCVC  VARCHAR(100),
    @p_CVC_2_PROPOSED		VARCHAR(100),
    @p_CVC_OM_NO			VARCHAR(100),
    @p_RECOMMOFCVC			VARCHAR(100),
    @p_NAT_CHSHEET			VARCHAR(100),
    @p_REG_INVOK			VARCHAR(100),
    @p_NAME_PO				VARCHAR(100),
    @p_NAME_EO				VARCHAR(100),
    @p_NAME_CDI				VARCHAR(100),
    @p_PUNISHMENTPROPOSEDBY VARCHAR(100),
    @p_CVCSADVICEII			VARCHAR(100),
    @p_NA_PUN_DA			VARCHAR(100),
    @p_PENALTY				VARCHAR(100),
    @p_FINAL				VARCHAR(100),
    @p_DISP_AUTHORITY		VARCHAR(100),
    @p_DISAUTHORITYSCIRCLE  VARCHAR(100),
    @p_STATUS				VARCHAR(MAX),
    @p_STATUS_INBRIEF		VARCHAR(100),
    @p_STATUSCODE			VARCHAR(100),
    @p_BASICPAY				VARCHAR(100),
    @p_PREVCASE_PUNISHMENTS VARCHAR(100),
    @p_LODICASE				VARCHAR(100),
    @p_LODINO				VARCHAR(100),
    @p_NATURECASE			VARCHAR(100),
    @p_REGISTER				VARCHAR(100),
    @p_PFNUMBER				VARCHAR(100),
    @p_DAPROPOSAL			VARCHAR(100),
    @p_ADVICECVOI			VARCHAR(100),
    @p_DAPROPOSAL_2			VARCHAR(100),
    @p_ADVICECVO2			VARCHAR(100),
    @p_FEILD1				VARCHAR(100),
	@p_AMOUNT				DECIMAL(19,3),
	@p_ADDUSER				VARCHAR(50),
	@p_ADDUSERIP			VARCHAR(20),
	@p_TABLENAME			VARCHAR(50)=NULL,
	@p_USER					VARCHAR(50)=NULL,
	@p_DTCHARGE				DATETIME=NULL,
    @p_DTRNO				DATETIME=NULL,
    @p_DTOFRETIREMENT		DATETIME=NULL,
    @p_DTOFSUSPENSION		DATETIME=NULL,
    @p_DT_RC1				DATETIME=NULL,
    @p_DT_RC2				DATETIME=NULL,
    @p_DTSANCTIONORDER		DATETIME=NULL,
    @p_DTREFERTOCVC			DATETIME=NULL,
    @p_DT_OM_CVC			DATETIME=NULL,
    @p_DT_ERCO				DATETIME=NULL,
    @p_DTREPLYCO			DATETIME=NULL,
    @p_DT_APP_PO			DATETIME=NULL,
    @p_DT_APP_EO			DATETIME=NULL,
    @p_DT_APP_CDI			DATETIME=NULL,
    @p_REF_CVC_2			DATETIME=NULL,
    @p_REC_CVC_2			DATETIME=NULL,
    @p_DT_ORD_DA			DATETIME=NULL,
    @p_REVIEWDATE			DATETIME=NULL,
    @p_DTFINAL				DATETIME=NULL,
    @p_DATEOFCLOSURE		DATETIME=NULL,
    @p_DTOFPLACEMENTINPRESENTSCALE DATETIME=NULL,
    @p_DATEOFCOMPLAINT		DATETIME=NULL,
    @p_DT_IST_DA			DATETIME=NULL,
    @p_DT_CVO_ADVICE		DATETIME=NULL,
    @p_DT_2ND_DA			DATETIME=NULL,
    @p_DT_CVO_ADVICE_2		DATETIME=NULL,
    @p_A1C_CVC				DATETIME=NULL,
    @p_A1E_CVC				DATETIME=NULL,
    @p_A2_CVC				DATETIME=NULL,
    @p_ADDDATE				DATETIME=NULL,
	@p_TMSACREFNO			VARCHAR(50)=NULL,
	@p_BANKNAME				VARCHAR(20)=NULL,
	@p_DESK_USER_REMARKS	VARCHAR(MAX)=NULL,
	@p_NEWZONESOLID			VARCHAR(10)=NULL,
	@p_NEWCIRCLESOLID		VARCHAR(10)=NULL,
    
	@o_EERMSG				VARCHAR(MAX) OUTPUT,
	@o_ERRCODE				INT OUTPUT
)
AS
BEGIN
	DECLARE @NEWCODE BIGINT, @IMPORTSTATUS CHAR(1), @OWNTRAN BIT = 0;

	SELECT @IMPORTSTATUS = ImportApprovalStatus
	FROM   dbo.WORKFLOW_MODULE
	WHERE  ModuleCode = 'VIGILANCE' AND IsActive = 1;

	--VIGILANCE MAY NOT BE REGISTERED FOR WORKFLOW (OR MAY HAVE BEEN SWITCHED OFF). IN THAT
	--CASE IMPORT EXACTLY AS BEFORE AND REGISTER NOTHING.
	SET @IMPORTSTATUS = ISNULL(@IMPORTSTATUS, '');

	IF EXISTS (SELECT 1 FROM VIGILANCE WHERE RNO=@p_RNO AND ACTIVE<>'N')
		BEGIN
			SET @o_ERRCODE = '-1';
			SET @o_EERMSG=@p_RNO + ' - Vigilance Number does''t save because already Exists in Table, please change RNO......!';
			RETURN;
		END

	--WHEN IMPORTED RECORDS HAVE TO BE CHECKED, THE ZONE IS WHAT ROUTES THEM TO A CHECKER.
	--A ROW WITHOUT ONE WOULD BE IMPORTED AND THEN STUCK: PENDING FOREVER, IN NO INBOX AND
	--NOT EDITABLE. REJECT IT AT THE DOOR INSTEAD, THE SAME WAY THE MANUAL FORM DOES.
	IF (@IMPORTSTATUS = 'P' AND ISNULL(LTRIM(RTRIM(@p_NEWZONESOLID)),'') = '')
		BEGIN
			SET @o_ERRCODE = -2;
			SET @o_EERMSG = @p_RNO + ' - not imported: NEWZONESOLID is blank. It decides which checker verifies this record......!';
			RETURN;
		END

	BEGIN TRY
			--funcExcelImport_Vigilance wraps the whole sheet in one transaction and rolls it
			--back on error, so only open our own when there is no ambient one. Never COMMIT or
			--ROLLBACK a transaction this proc did not start -- that would silently decide the
			--fate of the caller's entire batch.
			IF (@@TRANCOUNT = 0)
				BEGIN
					BEGIN TRAN;
					SET @OWNTRAN = 1;
				END

			 INSERT INTO VIGILANCE (RNO,RNO1,
									NAMEOFPARTICULARS,
									NAME,SCALE,
									DESIGNATION,
									BRNAME,CIRCLEOFFICE,
									STATE,LAPSENATURE,
									SOURCE,
									ACCTT_NAME,
									NATUREOFACCOUNT,INVESTIG,
									CBI_RC_NO1,CBI_RC_NO2,
									CBI_ZONE,RC_SOURCE,
									RECOM_CBI		,
									PROPOSEDACTIONTOCVC,CVC_2_PROPOSED,
									CVC_OM_NO,
									RECOMMOFCVC,NAT_CHSHEET,
									REG_INVOK,
									NAME_PO,NAME_EO,
									NAME_CDI,
									PUNISHMENTPROPOSEDBY,
									CVCSADVICEII,
									NA_PUN_DA,PENALTY,
									FINAL,
									DISP_AUTHORITY,DISAUTHORITYSCIRCLE,
									STATUS,STATUS_INBRIEF,
									STATUSCODE,
									BASICPAY,PREVCASE_PUNISHMENTS,
									LODICASE,LODINO,
									NATURECASE,
									REGISTER,
									PFNUMBER,DAPROPOSAL,
									ADVICECVOI,
									DAPROPOSAL_2,
									ADVICECVO2,FEILD1,
									AMOUNT,ACTIVE,
									ADDUSER,
									DTCHARGE,DTRNO,
									DTOFRETIREMENT,DTOFSUSPENSION,
									DT_RC1,DT_RC2,
									DTSANCTIONORDER,
									DTREFERTOCVC,DT_OM_CVC,
									DT_ERCO,DTREPLYCO,
									DT_APP_PO,
									DT_APP_EO,DT_APP_CDI,
									REF_CVC_2,
									REC_CVC_2,DT_ORD_DA,
									REVIEWDATE,
									DTFINAL,
									DATEOFCLOSURE,
									DTOFPLACEMENTINPRESENTSCALE,
									DATEOFCOMPLAINT,DT_IST_DA,
									DT_CVO_ADVICE,DT_2ND_DA,
									DT_CVO_ADVICE_2,A1C_CVC,
									A1E_CVC,A2_CVC,
									ADDDATE,CHANNEL,ADDUSERIP,TMSAC_REF,
									BANKNAME,DESK_USER_REMARKS,NEWZONE,NEWCIRCLE) 		
						 VALUES (@p_RNO,@p_RNO1,
								 @p_NAMEOFPARTICULARS,
								 @p_NAME,@p_SCALE,
								 @p_DESIGNATION,
								 @p_BRNAME,@p_CIRCLEOFFICE,
								 @p_STATE,@p_LAPSENATURE,
								 @p_SOURCE,
								 @p_ACCTT_NAME,
								 @p_NATUREOFACCOUNT,@p_INVESTIG,
								 @p_CBI_RC_NO1,@p_CBI_RC_NO2,
								 @p_CBI_ZONE,@p_RC_SOURCE,
								 @p_RECOM_CBI,
								 @p_PROPOSEDACTIONTOCVC,@p_CVC_2_PROPOSED,
								 @p_CVC_OM_NO,
								 @p_RECOMMOFCVC,@p_NAT_CHSHEET,
								 @p_REG_INVOK,
								 @p_NAME_PO,@p_NAME_EO,
								 @p_NAME_CDI,
								 @p_PUNISHMENTPROPOSEDBY,
								 @p_CVCSADVICEII,
								 @p_NA_PUN_DA,@p_PENALTY,
								 @p_FINAL,
								 @p_DISP_AUTHORITY,@p_DISAUTHORITYSCIRCLE,
								 @p_STATUS,@p_STATUS_INBRIEF,
								 @p_STATUSCODE,
								 @p_BASICPAY,@p_PREVCASE_PUNISHMENTS,
								 @p_LODICASE,@p_LODINO,
								 @p_NATURECASE,
								 @p_REGISTER,
								 @p_PFNUMBER,@p_DAPROPOSAL,
								 @p_ADVICECVOI,
								 @p_DAPROPOSAL_2,
								 @p_ADVICECVO2,@p_FEILD1,
 								 @p_AMOUNT,
 								 'Y',
								 (CASE WHEN(@p_ADDUSER=NULL OR @p_ADDUSER='') THEN @p_USER ELSE @p_ADDUSER END),
								 @p_DTCHARGE,@p_DTRNO,
								 @p_DTOFRETIREMENT,@p_DTOFSUSPENSION,
								 @p_DT_RC1,@p_DT_RC2,
								 @p_DTSANCTIONORDER,
								 @p_DTREFERTOCVC,@p_DT_OM_CVC,
								 @p_DT_ERCO,@p_DTREPLYCO,
								 @p_DT_APP_PO,
								 @p_DT_APP_EO,@p_DT_APP_CDI,
								 @p_REF_CVC_2,
								 @p_REC_CVC_2,@p_DT_ORD_DA,
								 @p_REVIEWDATE,
								 @p_DTFINAL,
								 @p_DATEOFCLOSURE,
								 @p_DTOFPLACEMENTINPRESENTSCALE,
								 @p_DATEOFCOMPLAINT,@p_DT_IST_DA,
								 @p_DT_CVO_ADVICE,@p_DT_2ND_DA,
								 @p_DT_CVO_ADVICE_2,@p_A1C_CVC,
								 @p_A1E_CVC,@p_A2_CVC,
								 (CASE WHEN(@p_ADDDATE IS NULL OR @p_ADDDATE='') THEN GETDATE() ELSE @p_ADDDATE END),
								 'EXCEL UPLOAD',@p_ADDUSERIP,@p_TMSACREFNO,
								 @p_BANKNAME,@p_DESK_USER_REMARKS,@p_NEWZONESOLID,@p_NEWCIRCLESOLID)

			SET @NEWCODE = SCOPE_IDENTITY();

			--MAKER-CHECKER: REGISTER THE IMPORTED RECORD. EVEN WHEN IMPORTS ARE EXEMPT
			--(@IMPORTSTATUS='A') IT STILL GETS A ROW, SO IT CARRIES A VISIBLE STATUS, APPEARS
			--IN REPORTING, AND RE-ENTERS THE WORKFLOW THE MOMENT ANYONE EDITS IT.
			IF (@IMPORTSTATUS IN ('P','A'))
				BEGIN
					INSERT INTO CASE_APPROVAL
						(ModuleCode, RecordCode, RecordRef, ZoneSolID, ApprovalStatus,
						 MakerUser, MakerDate, CheckerUser, CheckerDate, CheckerRemarks)
					VALUES
						('VIGILANCE', @NEWCODE, @p_RNO, @p_NEWZONESOLID, @IMPORTSTATUS,
						 ISNULL(NULLIF(NULLIF(@p_ADDUSER,''), ''), ISNULL(@p_USER,'SYSTEM')), GETDATE(),
						 CASE WHEN @IMPORTSTATUS = 'A' THEN 'SYSTEM' END,
						 CASE WHEN @IMPORTSTATUS = 'A' THEN GETDATE() END,
						 CASE WHEN @IMPORTSTATUS = 'A'
							  THEN 'Bulk Excel upload - exempt from checker verification by configuration.' END);

					INSERT INTO CASE_APPROVAL_HISTORY
						(ModuleCode, RecordCode, ActionType, ActionBy, Remarks, UserIP)
					VALUES
						('VIGILANCE', @NEWCODE, 'IMPORTED', ISNULL(NULLIF(NULLIF(@p_ADDUSER,''), ''), ISNULL(@p_USER,'SYSTEM')),
						 CASE WHEN @IMPORTSTATUS = 'A'
							  THEN 'Created by Excel upload. Exempt from checker verification by configuration.'
							  ELSE 'Created by Excel upload. Queued for checker verification.' END,
						 @p_ADDUSERIP);
				END

			IF (@OWNTRAN = 1) COMMIT TRAN;

			SET @o_ERRCODE=1;
			SET @o_EERMSG='Record Saved Sucessfully......!';
	END TRY
	BEGIN CATCH
			IF (@OWNTRAN = 1 AND XACT_STATE() <> 0) ROLLBACK TRAN;

			SET @o_ERRCODE = 0;
			SET @o_EERMSG  = @p_RNO + ' - could not be imported: ' + ERROR_MESSAGE();

			--Propagate, exactly as before this change: funcExcelImport_Vigilance catches it,
			--rolls the sheet back and reports the offending row number.
			THROW;
	END CATCH
END
GO
