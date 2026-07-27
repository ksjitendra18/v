/*
    IAC Maker-Checker -- Excel Import
    =================================================================================
    Database : VigilanceMISDB
    Requires : 2026-07-27_CaseApproval_Central_MakerChecker_Foundation.sql
               2026-07-27_IAC_MakerChecker.sql

    Why this exists
    ---------------
    The first two scripts put the manual entry form (Mis/frmIACStructure.aspx) under
    maker-checker, but left Upload/frmExcelUpload.aspx alone. For IAC that is the wrong
    way round: most IAC records arrive by Excel upload, so leaving the import outside
    the workflow left the control applying to the minority of records -- and gave anyone
    a trivial way around it, by uploading a one-row sheet instead of typing the record in.

    Changes
    -------
    1. WORKFLOW_MODULE gains ImportApprovalStatus. This is the business decision -- do
       bulk-imported records need checking? -- expressed as configuration rather than
       code, so it can be changed per module with one UPDATE. See "Choosing" below.

    2. spIACExcel_Import now registers every imported row in CASE_APPROVAL at that
       configured status, and logs an IMPORTED row in CASE_APPROVAL_HISTORY.

    3. When imports are configured to require checking, a row with no zone is rejected
       rather than imported. The zone is the only thing that routes a record to a
       checker; imported without one, the row would sit Pending forever -- invisible to
       every inbox and locked from editing. The manual form already refuses for the same
       reason, so this also closes the "upload instead of typing" bypass.

    No parameters were added or removed. The existing call site in
    Upload/frmExcelUpload.aspx.cs (funcExcelImport_IAC) works unchanged -- it already
    passes @p_USER, @p_ADDUSERIP and @p_NEWZONESOLID.

    Choosing ImportApprovalStatus
    -----------------------------
      'P' (the default set here)
          Imported records are Pending and appear in the checker inbox like any other.
          The control genuinely covers the dominant path -- but a 500-row upload puts
          500 records in front of a checker, and the maker cannot edit any of them until
          they are actioned. Practical only with a bulk action in the inbox.

      'A'
          Imported records are registered as Approved without a checker seeing them --
          an explicit, audited exemption rather than today's silent gap. They still get
          a CASE_APPROVAL row, so they show a status, appear in reporting, and re-enter
          the workflow the moment anyone edits them.

    To switch:
        UPDATE dbo.WORKFLOW_MODULE SET ImportApprovalStatus = 'A' WHERE ModuleCode = 'IAC';

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

-------------------------------------------------------------------------------------------------
-- 2. dbo.spIACExcel_Import
--    (full body below is the complete, current definition -- not a diff)
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spIACExcel_Import]
(
      @p_MEETNO               VARCHAR(100),
      @p_IACNO                VARCHAR(50),
      @p_IACNO_1              VARCHAR(50),
      @p_VIGNO                VARCHAR(50),
      @p_ACCUSED              VARCHAR(500),
      @p_PFNUMBER             VARCHAR(100),
      @p_SOURCE               VARCHAR(50),
      @p_PRESENTPOSTING		  VARCHAR(100),
      @p_NAMEOFTHEBRANCH      VARCHAR(100),
      @p_ZONE                 VARCHAR(100),
      @p_CIRCLEOFFICE         VARCHAR(100),
      @p_ACCOUNTNAME          VARCHAR(MAX),
      @p_NATURECASE           VARCHAR(100),
      @p_DA                   VARCHAR(50),
      @p_STATUS               VARCHAR(MAX),
      @p_DAVIEW               VARCHAR(50),
      @p_IACVIEW              VARCHAR(100),
      @p_CVOVIEW              VARCHAR(50),
      @p_STATUSCODE           VARCHAR(50),
      @p_AMOUNT               DECIMAL(19,3),
      @p_TABLENAME            VARCHAR(50),
      @p_DTRET                DATETIME=NULL,
      @p_DTIAC                DATETIME=NULL,
      @p_RECDT                DATETIME=NULL,
      @p_DATEIADNOTE          DATETIME=NULL,
      @p_CLOSUREDT            DATETIME=NULL,
      @p_USER                 VARCHAR(50),
	  @p_ADDUSERIP			  VARCHAR(20),
	  @p_NEWZONESOLID		  VARCHAR(10),
	  @p_NEWCIRCLESOLID		  VARCHAR(10),
	  @p_BANKNAME			  VARCHAR(10),
	  @p_DESIGNATION		  VARCHAR(100),
	  @p_SCALE				  VARCHAR(10),
	  @p_TMSACREFNO			  VARCHAR(50),

      @o_EERMSG               VARCHAR(MAX) OUTPUT,
      @o_ERRCODE              INT OUTPUT
)
AS
BEGIN
      DECLARE @NEWSNO BIGINT, @IMPORTSTATUS CHAR(1), @OWNTRAN BIT = 0;

      SELECT @IMPORTSTATUS = ImportApprovalStatus
      FROM   dbo.WORKFLOW_MODULE
      WHERE  ModuleCode = 'IAC' AND IsActive = 1;

      --IAC MAY NOT BE REGISTERED FOR WORKFLOW (OR MAY HAVE BEEN SWITCHED OFF). IN THAT CASE
      --IMPORT EXACTLY AS BEFORE AND REGISTER NOTHING.
      SET @IMPORTSTATUS = ISNULL(@IMPORTSTATUS, '');

      IF EXISTS (SELECT 1 FROM IAC WHERE IACNO=@p_IACNO AND ACTIVE<>'N')
            BEGIN
                  SET @o_ERRCODE = '-1';
                  SET @o_EERMSG=@p_IACNO + ' - IAC Number does''t save because already Exists in Table, please change IAC NO......!';
                  RETURN;
            END

      --WHEN IMPORTED RECORDS HAVE TO BE CHECKED, THE ZONE IS WHAT ROUTES THEM TO A CHECKER.
      --A ROW WITHOUT ONE WOULD BE IMPORTED AND THEN STUCK: PENDING FOREVER, IN NO INBOX AND
      --NOT EDITABLE. REJECT IT AT THE DOOR INSTEAD, THE SAME WAY THE MANUAL FORM DOES.
      IF (@IMPORTSTATUS = 'P' AND ISNULL(LTRIM(RTRIM(@p_NEWZONESOLID)),'') = '')
            BEGIN
                  SET @o_ERRCODE = -2;
                  SET @o_EERMSG = @p_IACNO + ' - not imported: NEWZONESOLID is blank. It decides which checker verifies this record......!';
                  RETURN;
            END

      BEGIN TRY
            --funcExcelImport_IAC wraps the whole sheet in one transaction and rolls it back on
            --error, so only open our own when there is no ambient one. Never COMMIT or ROLLBACK
            --a transaction this proc did not start -- that would silently decide the fate of the
            --caller's entire batch.
            IF (@@TRANCOUNT = 0)
                  BEGIN
                        BEGIN TRAN;
                        SET @OWNTRAN = 1;
                  END

            INSERT INTO IAC (MEETNO,
                             IACNO,IACNO_1,
                             VIGNO,ACCUSED,
                             PFNUMBER,SOURCE,
                             PRESENTPOSTING,NAMEOFTHEBRANCH,
                             ZONE,CIRCLEOFFICE,
                             ACNAME,NATURECASE,
                             AMOUNT,DA,
                             STATUS,DAVIEW,
                             IACVIEW,CVOVIEW,
                             STATUSCODE,
                             ADDUSER,
                             DTRET,DTIAC,
                             RECDT,DATEIADNOTE,
                             CLOSUREDT,ADDDATE,CHANNEL,ADDUSERIP,
							 NEWZONE,NEWCIRCLE,BANKNAME,
							 DESIGNATION,SCALE,TMSAC_REF)
					 VALUES (@p_MEETNO,
                             @p_IACNO,@p_IACNO_1,
                             @p_VIGNO,@p_ACCUSED,
                             @p_PFNUMBER,@p_SOURCE,
                             @p_PRESENTPOSTING,@p_NAMEOFTHEBRANCH,
                             @p_ZONE,@p_CIRCLEOFFICE,
                             @p_ACCOUNTNAME,@p_NATURECASE,
                             @p_AMOUNT,@p_DA,
                             @p_STATUS,@p_DAVIEW,
                             @p_IACVIEW,@p_CVOVIEW,
                             @p_STATUSCODE,
                             @p_USER,
                             @p_DTRET,@p_DTIAC,
                             @p_RECDT,@p_DATEIADNOTE,
                             @p_CLOSUREDT,GETDATE(),
                             'EXCEL UPLOAD',@p_ADDUSERIP,
							 @p_NEWZONESOLID,@p_NEWCIRCLESOLID,@p_BANKNAME,
							 @p_DESIGNATION,@p_SCALE,@p_TMSACREFNO)

            SET @NEWSNO = SCOPE_IDENTITY();

            --MAKER-CHECKER: REGISTER THE IMPORTED RECORD. EVEN WHEN IMPORTS ARE EXEMPT
            --(@IMPORTSTATUS='A') IT STILL GETS A ROW, SO IT CARRIES A VISIBLE STATUS, APPEARS
            --IN REPORTING, AND RE-ENTERS THE WORKFLOW THE MOMENT ANYONE EDITS IT.
            IF (@IMPORTSTATUS IN ('P','A'))
                  BEGIN
                        INSERT INTO CASE_APPROVAL
                              (ModuleCode, RecordCode, RecordRef, ZoneSolID, ApprovalStatus,
                               MakerUser, MakerDate, CheckerUser, CheckerDate, CheckerRemarks)
                        VALUES
                              ('IAC', @NEWSNO, @p_IACNO, @p_NEWZONESOLID, @IMPORTSTATUS,
                               @p_USER, GETDATE(),
                               CASE WHEN @IMPORTSTATUS = 'A' THEN 'SYSTEM' END,
                               CASE WHEN @IMPORTSTATUS = 'A' THEN GETDATE() END,
                               CASE WHEN @IMPORTSTATUS = 'A'
                                    THEN 'Bulk Excel upload - exempt from checker verification by configuration.' END);

                        INSERT INTO CASE_APPROVAL_HISTORY
                              (ModuleCode, RecordCode, ActionType, ActionBy, Remarks, UserIP)
                        VALUES
                              ('IAC', @NEWSNO, 'IMPORTED', @p_USER,
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
            SET @o_EERMSG  = @p_IACNO + ' - could not be imported: ' + ERROR_MESSAGE();

            --Propagate, exactly as before this change: funcExcelImport_IAC catches it, rolls the
            --sheet back and reports the offending row number.
            THROW;
      END CATCH
END
GO
