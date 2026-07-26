/*
    Complaint Checker Workflow (Accept / Reject / Push Back)
    ---------------------------------------------------------
    Database : VigilanceMISDB

    1. NEW  : dbo.spComplaint_CheckerAction
              Called from frmComplaintCheckerView.aspx.cs (btnAccept_Click / btnReject_Click / btnPushBack_Click)
              to approve, reject, or push back a pending complaint (COMPLAINT.APPROVALSTATUS = 'P').

    2. CHANGE: dbo.spComplaint_Update
               When a maker resaves (@p_MODE='U') a complaint that was pushed back for correction
               (APPROVALSTATUS = 'C'), the status is now reset to 'P' so it re-enters the checker's
               pending queue, and a 'RESUBMITTED' row is logged in COMPLAINT_APPROVAL_HISTORY.
               All other update behaviour is unchanged.

    Safe to re-run: both procedures use CREATE OR ALTER.
*/

-------------------------------------------------------------------------------------------------
-- 1. NEW PROCEDURE: dbo.spComplaint_CheckerAction
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spComplaint_CheckerAction]
(
    @p_RNO          VARCHAR(50),
    @p_ACTION       CHAR(1),        -- 'A' = Approve, 'R' = Reject, 'C' = Push Back (correction)
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

    IF (@p_ACTION NOT IN ('A','R','C'))
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

    IF NOT EXISTS (
        SELECT 1 FROM MakerCheckerMapping
        WHERE UserPF = @p_USER
          AND ZoneSolID = @NEWZONE
          AND IsChecker = 1
          AND IsActive = 1
    )
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
        CASE @p_ACTION WHEN 'A' THEN 'APPROVED' WHEN 'R' THEN 'REJECTED' WHEN 'C' THEN 'PUSHED_BACK' END,
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
-- 2. CHANGED PROCEDURE: dbo.spComplaint_Update
--    (full body below is the complete, current definition -- not a diff)
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spComplaint_Update]
(
	@p_CODE					BIGINT,
	@p_RNO					VARCHAR(50),
	@p_COMPNO				VARCHAR(50),
	@p_ACCUSED				VARCHAR(100),
	@p_DESIGNATION			VARCHAR(50),
	@p_PRESENTPOSTING		VARCHAR(100),
	@p_BRCOMPLAINT			VARCHAR(100),
	@p_ZONE					VARCHAR(100)=NULL,
	@p_CIRCLEOFFICE			VARCHAR(100)=NULL,
	@p_REGION				VARCHAR(50),
	@p_RECDATECOMP			DATETIME=NULL,
	@p_SOURCE				VARCHAR(50),
	@p_SOURCEREF			VARCHAR(50)=NULL,
	@p_SOURCEDATE			DATETIME=NULL,
	@p_SENTTO				VARCHAR(50),
	@p_SENTFORINVDATE		DATETIME=NULL,
	@p_ACCOUNTNAME			VARCHAR(100),
	@p_AMOUNT				DECIMAL(19,4),
	@p_ALLEGATIONS			VARCHAR(100),
	@p_DTIAC				DATETIME=NULL,
	@p_STATUS				VARCHAR(MAX),
	@p_HOSTATUS				VARCHAR(250)=NULL,
	@p_STATUSCODE			VARCHAR(50)=NULL,
	@p_NAMEOFINVOFFICIAL	VARCHAR(100),
	@p_DTOFINVREPORT		DATETIME=NULL,
	@p_CASENO				VARCHAR(50),
	@p_CASECLOSE			VARCHAR(5),
	@p_CLOSUREDT			DATETIME=NULL,
	@p_RYSENT				DATETIME=NULL,
	@p_REASONSFORCLOSURE	VARCHAR(MAX),
	@p_MODE					CHAR(1),
	@p_USER					VARCHAR(50),
	@p_USERROLE				VARCHAR(50),
	@p_CLOSURE				VARCHAR(1),
	@p_PFNUMBER				VARCHAR(10),
    @p_USERIP				VARCHAR(20),
	@p_DESK_USER_REMARKS	VARCHAR(MAX),
	@p_BANKNAME				VARCHAR(20),
	@p_MARKEDFORINVESTIGATION	VARCHAR(5)=NULL,

	@p_LETTERSENTTO			VARCHAR(10)=NULL,
	@p_LETTERSENTDATE		DATETIME=NULL,
	@p_REMINDERDATE			DATETIME=NULL,
	@p_REPLYRECEIVEDDATE	DATETIME=NULL,

	@p_ZONENEW				VARCHAR(10)=NULL,
	@p_CIRCLENEW			VARCHAR(10)=NULL,

	@o_EERMSG				VARCHAR(MAX) OUTPUT,
	@o_ERRCODE				INT OUTPUT
)
AS
BEGIN
	DECLARE @STATUS	VARCHAR(MAX),@UPDATESTATUS VARCHAR(MAX),@ERRCODE BIGINT=0,@MONTH BIGINT,@YEAR BIGINT,@OLDAPPROVALSTATUS CHAR(1);
	----------------------------------------------------------------------------------------------------------------------
	SET @o_ERRCODE=0;
	SET @MONTH = MONTH(GETDATE());
	SET @YEAR =  YEAR(GETDATE());
	----------------------------------------------------------------------------------------------------------------------
	IF(UPPER(@p_USERROLE) = 'VMIS_MISUSER')
		BEGIN
			IF(@p_RNO <> '')
				BEGIN
					IF EXISTS (SELECT 1 FROM COMPLAINT WHERE RNO=@p_RNO AND ACTIVE='Y' AND CODE <> @p_CODE)
						BEGIN
							SET @o_ERRCODE=3;
							SET @o_EERMSG=@p_RNO + '- R Number alredy Exists......!';
						END
				END
			IF(@o_ERRCODE = 0)
			BEGIN
				IF(@p_MODE='I')
					BEGIN
					INSERT INTO COMPLAINT (RNO,COMPNO,
										   ACCUSED,DESIGNATION,
										   PRESENTPOSTING,BRCOMPLAINT,
										   ZONE,CIRCLEOFFICE,
										   REGION,RECDATECOMP,
										   SOURCE,SOURCEREF,
										   SOURCEDATE,SENTTO,
										   SENTFORINVDATE,
										   ACCOUNTNAME,AMOUNT,
										   ALLEGATIONS,DTIAC,
										   STATUS,STATUSCODE,
										   NAMEOFINVOFFICIAL,
										   DTOFINVREPORT,
										   CASENO,CASECLOSE,
										   CLOSUREDT,RYSENT,
										   REASONSFORCLOSURE,
										   ADDUSER,ADDDATE,
										   ADDUSERIP,PFNUMBER,BANKNAME, MARKEDFORINVESTIGATION,
										   LETTERSENTTO,LETTERSENTDATE,REMINDERDATE,REPLYRECEIVEDDATE,CHANNEL,
										   NEWZONE, NEWCIRCLE,
										   APPROVALSTATUS, MAKERUSER,MAKERDATE
										   )
								VALUES (@p_RNO,@p_COMPNO,
										@p_ACCUSED,@p_DESIGNATION,
										@p_PRESENTPOSTING,@p_BRCOMPLAINT,
										@p_ZONE,@p_CIRCLEOFFICE,
										@p_REGION,@p_RECDATECOMP,
										@p_SOURCE,@p_SOURCEREF,
										@p_SOURCEDATE,@p_SENTTO,
										@p_SENTFORINVDATE,
										@p_ACCOUNTNAME,@p_AMOUNT,
										@p_ALLEGATIONS,@p_DTIAC,
										@p_STATUS,@p_STATUSCODE,
										@p_NAMEOFINVOFFICIAL,
										@p_DTOFINVREPORT,@p_CASENO,
										@p_CASECLOSE,(CASE WHEN(@p_CLOSURE = 'Y') THEN GETDATE() ELSE NULL END),
										@p_RYSENT,
										@p_REASONSFORCLOSURE,
										@p_USER,GETDATE(),
										@p_USERIP,@p_PFNUMBER,@p_BANKNAME, @p_MARKEDFORINVESTIGATION,
										@p_LETTERSENTTO,@p_LETTERSENTDATE,@p_REMINDERDATE,@p_REPLYRECEIVEDDATE,'MANUL ENTRY',
										@p_ZONENEW, @p_CIRCLENEW, 'P',@p_USER,GETDATE())

							SET @o_ERRCODE=1;
							SET @ERRCODE=1;
							SET @o_EERMSG='Record Saved Sucessfully......!';

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
    SCOPE_IDENTITY(),
    'SUBMITTED',
    @p_USER,
    NULL,
    @p_USERROLE,
    @p_USERIP
);

				END

				ELSE IF(@p_MODE='U')
					BEGIN
					--UPDATE TBALE COMPLAINT_HISTORY
					INSERT INTO COMPLAINT_HISTORY SELECT * FROM COMPLAINT WHERE CODE=@p_CODE;

					--CAPTURE CURRENT APPROVAL STATUS SO A PUSHED-BACK COMPLAINT CAN BE RE-QUEUED FOR THE CHECKER ON RESUBMIT
					SELECT @OLDAPPROVALSTATUS = APPROVALSTATUS FROM COMPLAINT WHERE CODE=@p_CODE;

					--UPDATE DATA OF COMPLAINT
					UPDATE COMPLAINT SET RNO=@p_RNO,
										 COMPNO=@p_COMPNO,
										 ACCUSED=@p_ACCUSED,
										 DESIGNATION=@p_DESIGNATION,
										 PRESENTPOSTING=@p_PRESENTPOSTING,
										 BRCOMPLAINT=@p_BRCOMPLAINT,
										 ZONE=@p_ZONE,
										 CIRCLEOFFICE=@p_CIRCLEOFFICE,
										 REGION=@p_REGION,
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
										 NAMEOFINVOFFICIAL=@p_NAMEOFINVOFFICIAL,
										 DTOFINVREPORT=@p_DTOFINVREPORT,
										 CASENO=@p_CASENO,
										 CASECLOSE=@p_CASECLOSE,
										 CLOSUREDT=(CASE WHEN(@p_CLOSURE = 'Y') THEN GETDATE() ELSE @p_CLOSUREDT END),
										 RYSENT=@p_RYSENT,
										 REASONSFORCLOSURE=@p_REASONSFORCLOSURE,
										 MODUSER=@p_USER,
										 MODDATE=GETDATE(),
										 MODUSERIP=@p_USERIP,
										 PFNUMBER=@p_PFNUMBER,
										 BANKNAME=@p_BANKNAME,
										 LETTERSENTTO=@p_LETTERSENTTO,
										 LETTERSENTDATE=@p_LETTERSENTDATE,
										 REMINDERDATE=@p_REMINDERDATE,
										 REPLYRECEIVEDDATE=@p_REPLYRECEIVEDDATE,
										 MARKEDFORINVESTIGATION = @p_MARKEDFORINVESTIGATION,
										 NEWZONE=@p_ZONENEW,
										 NEWCIRCLE=@p_CIRCLENEW,
										 APPROVALSTATUS = (CASE WHEN @OLDAPPROVALSTATUS = 'C' THEN 'P' ELSE APPROVALSTATUS END)
					WHERE CODE=@p_CODE;

					--IF THE COMPLAINT WAS PUSHED BACK FOR CORRECTION, LOG THE RESUBMISSION SO IT RE-ENTERS THE CHECKER'S QUEUE
					IF (@OLDAPPROVALSTATUS = 'C')
					BEGIN
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
							@p_CODE,
							'RESUBMITTED',
							@p_USER,
							NULL,
							@p_USERROLE,
							@p_USERIP
						);
					END

					SET @o_ERRCODE= 2;
					SET @ERRCODE=1;
					SET @o_EERMSG='Record Updated Sucessfully......!'
				END

			END
		END

	ELSE IF(@p_USERROLE = 'VMIS_DESKUSER')
			BEGIN
					--UPDATE TBALE COMPLAINT_HISTORY
					INSERT INTO COMPLAINT_HISTORY SELECT * FROM COMPLAINT WHERE CODE=@p_CODE;

					--SELECT STATUS FROM COMPLAINT FOR APPEND STATUS IN CASE OF VMIS_DESKUSER USER
					SELECT @STATUS=ISNULL(STATUS,'') FROM COMPLAINT WHERE CODE=@p_CODE;

					SET @UPDATESTATUS = @p_HOSTATUS + ' | ' + @STATUS;


					UPDATE COMPLAINT SET STATUS=@UPDATESTATUS,
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
