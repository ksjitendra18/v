/*
    Fix: align Reject status code with the app's pre-existing convention + surface
    Checker Remarks in the maker's complaint grid.
    ---------------------------------------------------------------------------------
    Database : VigilanceMISDB

    Background
    ----------
    dbo.spComplaint_View already had a CASE mapping for COMPLAINT.APPROVALSTATUS:
        'P' -> Pending Approval
        'A' -> Approved
        'C' -> Changes Requested
        'X' -> Rejected
    dbo.spComplaint_CheckerAction (added 2026-07-25) used 'R' for Reject instead of 'X',
    so rejected complaints didn't match any case in spComplaint_View's APPROVALSTATUSTEXT
    and fell through to the default "still editable" bucket in the maker's grid
    (frmComplaint.aspx / gvMain).

    Changes
    -------
    1. spComplaint_CheckerAction: Reject now writes APPROVALSTATUS = 'X' (was 'R').
    2. COMPLAINT: one-time data fix, APPROVALSTATUS 'R' -> 'X' for any rows saved
       before this fix (test data from initial feature verification).
    3. spComplaint_View: LIST/SEARCH/single-record branches now also return
       CHECKERREMARKS, so it can be shown as a column in the maker's grid.
    4. frmComplaint.aspx / frmComplaint.aspx.cs: added a "Checker Remarks" grid column
       and locked the Edit button (disabled, labeled "Rejected") for APPROVALSTATUS='X'.
    5. frmComplaintChecker.aspx.cs / frmComplaintCheckerView.aspx.cs: GetStatusClass /
       GetStatusText switched from 'R' to 'X', and the "Changes Requested" wording for
       'C' now matches spComplaint_View instead of the "Pushed Back" label used earlier.

    Safe to re-run: both procedures use CREATE OR ALTER; the data fix is a no-op once applied.
*/

-------------------------------------------------------------------------------------------------
-- 1. spComplaint_CheckerAction: Reject code 'R' -> 'X'
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
-- 2. One-time data fix for rows saved before this change
-------------------------------------------------------------------------------------------------
UPDATE COMPLAINT SET APPROVALSTATUS = 'X' WHERE APPROVALSTATUS = 'R';
GO

-------------------------------------------------------------------------------------------------
-- 3. spComplaint_View: add CHECKERREMARKS to LIST / SEARCH / single-record result sets
--    (full body below is the complete, current definition -- not a diff)
-------------------------------------------------------------------------------------------------
CREATE OR ALTER PROC [dbo].[spComplaint_View]
(
	@p_VIEW			  VARCHAR(50)=NULL,
	@p_SEARCHNO		  VARCHAR(50)=NULL,
	@p_BRANCH		  VARCHAR(100)=NULL,
	@p_ACCUSED		  VARCHAR(100)=NULL,
	@p_ALLEGATIONS	  VARCHAR(100)=NULL,
	@p_STATUS		  VARCHAR(100)=NULL,
	@p_INTERNALREFNO  VARCHAR(100)=NULL,
	@p_ACCOUNTNAME    VARCHAR(100)=NULL,
	@p_EXTERNALSOURCE VARCHAR(100)=NULL,
	@p_CIRCLE		  VARCHAR(100)=NULL,
	@o_EERMSG		  VARCHAR(MAX) OUTPUT,
	@o_ERRCODE		  INT OUTPUT
)
AS
BEGIN
	DECLARE @SQL VARCHAR(MAX),@STRCOND VARCHAR(MAX);
	--------------------------------------------------------------------------------------
	SET @o_ERRCODE=0;
	SET @STRCOND = 'ACTIVE=''Y''';
	--------------------------------------------------------------------------------------
	IF(@p_SEARCHNO <> '' AND @p_SEARCHNO IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND RNO LIKE''%'+@p_SEARCHNO+'%''' + CHAR(13);
			END
	IF(@p_BRANCH <> '' AND @p_BRANCH IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND BRCOMPLAINT LIKE''%'+@p_BRANCH+'%''' + CHAR(13);
			END
	IF(@p_ACCUSED <> '' AND @p_ACCUSED IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND ACCUSED LIKE''%'+@p_ACCUSED+'%''' + CHAR(13);
			END
	IF(@p_ALLEGATIONS <> '' AND @p_ALLEGATIONS IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND ALLEGATIONS LIKE''%'+@p_ALLEGATIONS+'%''' + CHAR(13);
			END
	IF(@p_STATUS <> '' AND @p_STATUS IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND STATUS LIKE''%'+@p_STATUS+'%''' + CHAR(13);
			END

	IF(@p_INTERNALREFNO <> '' AND @p_INTERNALREFNO IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND COMPNO LIKE''%'+@p_INTERNALREFNO+'%''' + CHAR(13);
			END

	IF(@p_ACCOUNTNAME <> '' AND @p_ACCOUNTNAME IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND ACCOUNTNAME LIKE''%'+@p_ACCOUNTNAME+'%''' + CHAR(13);
			END

	IF(@p_EXTERNALSOURCE <> '' AND @p_EXTERNALSOURCE IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND SOURCE LIKE''%'+@p_EXTERNALSOURCE+'%''' + CHAR(13);
			END
	IF(@p_CIRCLE <> '' AND @p_CIRCLE IS NOT NULL)
			BEGIN
				SET @STRCOND=@STRCOND+ 'AND CIRCLEOFFICE LIKE''%'+@p_CIRCLE+'%''' + CHAR(13);
			END
	-----------------------------------------------------------------------------------------
	IF(UPPER(@p_VIEW) = 'LIST')
		BEGIN
			SELECT TOP 20 CODE,RNO,BRCOMPLAINT,CIRCLEOFFICE,COMPNO,ACCUSED,ALLEGATIONS,CASENO,
				   PRESENTPOSTING,ZONE,SOURCE,SOURCEREF,ACCOUNTNAME,STATUSCODE,
				   AMOUNT,SENTTO,REGION,CASECLOSE,DESIGNATION,NAMEOFINVOFFICIAL,
				   REASONSFORCLOSURE,STATUS,
				   APPROVALSTATUS,
CASE APPROVALSTATUS
    WHEN 'P' THEN 'Pending Approval'
    WHEN 'A' THEN 'Approved'
    WHEN 'C' THEN 'Changes Requested'
    WHEN 'X' THEN 'Rejected'
END AS APPROVALSTATUSTEXT,
				   ISNULL(CHECKERREMARKS,'') AS CHECKERREMARKS,
				   ADDUSER AS ENTRYBY,ISNULL(MODUSER,'') AS MODIFYBY,
				   ISNULL(CONVERT(VARCHAR(50),RECDATECOMP,103),'') AS COMPRECDATE,
				   ISNULL(CONVERT(VARCHAR(50),CLOSUREDT,103),'') AS CLOSUREDATE,
				   ISNULL(CONVERT(VARCHAR(50),DTIAC,103),'') AS IACDATE,
				   ISNULL(CONVERT(VARCHAR(50),SOURCEDATE,103),'') AS SOURCEDATE,
				   ISNULL(CONVERT(VARCHAR(50),SENTFORINVDATE,103),'') AS SENTFORINVDATE,
				   ISNULL(CONVERT(VARCHAR(50),DTOFINVREPORT,103),'') AS INVREPORTDATE,
				   ISNULL(CONVERT(VARCHAR(50),RYSENT,103),'') AS RYSENTDATE,
				   ISNULL(CONVERT(VARCHAR(50),ADDDATE,103),'') AS ENTRYDATE,
				   ISNULL(CONVERT(VARCHAR(50),MODDATE,103),'') AS MODIFYDATE,
				   (CASE WHEN CLOSUREDT IS NULL THEN 'N' ELSE 'Y' END) AS CLOSURE,
				   ([dbo].ReverseColumnValue_Function(REASONSFORCLOSURE,RNO,'COMPLAINT')) AS SHORTREASONSFORCLOSURE,
				   ([dbo].ReverseColumnValue_Function(STATUS,RNO,'COMPLAINT')) AS SHORTSTATUS
			FROM COMPLAINT WHERE ACTIVE='Y'
			ORDER BY ADDDATE DESC;
		END

	ELSE IF(UPPER(@p_VIEW) = 'SEARCH')
		BEGIN
			SET @SQL='SELECT TOP 20 CODE,RNO,BRCOMPLAINT,CIRCLEOFFICE,COMPNO,ACCUSED,ALLEGATIONS,CASENO,
							 PRESENTPOSTING,ZONE,SOURCE,SOURCEREF,ACCOUNTNAME,STATUSCODE,
							 AMOUNT,SENTTO,REGION,CASECLOSE,DESIGNATION,NAMEOFINVOFFICIAL,
							 REASONSFORCLOSURE,STATUS,APPROVALSTATUS,
CASE APPROVALSTATUS
    WHEN ''P'' THEN ''Pending Approval''
    WHEN ''A'' THEN ''Approved''
    WHEN ''C'' THEN ''Changes Requested''
    WHEN ''X'' THEN ''Rejected''
END AS APPROVALSTATUSTEXT,
							 ISNULL(CHECKERREMARKS,'''') AS CHECKERREMARKS,
							 ADDUSER AS ENTRYBY,ISNULL(MODUSER,'''') AS MODIFYBY,
							 ISNULL(CONVERT(VARCHAR(50),RECDATECOMP,103),'''') AS COMPRECDATE,
							 ISNULL(CONVERT(VARCHAR(50),CLOSUREDT,103),'''') AS CLOSUREDATE,
							 ISNULL(CONVERT(VARCHAR(50),DTIAC,103),'''') AS IACDATE,
							 ISNULL(CONVERT(VARCHAR(50),SOURCEDATE,103),'''') AS SOURCEDATE,
							 ISNULL(CONVERT(VARCHAR(50),SENTFORINVDATE,103),'''') AS SENTFORINVDATE,
							 ISNULL(CONVERT(VARCHAR(50),DTOFINVREPORT,103),'''') AS INVREPORTDATE,
							 ISNULL(CONVERT(VARCHAR(50),RYSENT,103),'''') AS RYSENTDATE,
							 ISNULL(CONVERT(VARCHAR(50),ADDDATE,103),'''') AS ENTRYDATE,
							 ISNULL(CONVERT(VARCHAR(50),MODDATE,103),'''') AS MODIFYDATE,
							 (CASE WHEN CLOSUREDT IS NULL THEN ''N'' ELSE ''Y'' END) AS CLOSURE,
							 ([dbo].ReverseColumnValue_Function(REASONSFORCLOSURE,RNO,''COMPLAINT'')) AS SHORTREASONSFORCLOSURE,
							 ([dbo].ReverseColumnValue_Function(STATUS,RNO,''COMPLAINT'')) AS SHORTSTATUS,PFNUMBER
					  FROM COMPLAINT WHERE '+@STRCOND+'
					  ORDER BY ADDDATE DESC'
		   EXEC(@SQL);
		   PRINT(@SQL);
		END

	ELSE
		BEGIN
			IF EXISTS (SELECT 1 FROM COMPLAINT WHERE (CASE WHEN @p_VIEW='GET' THEN RNO ELSE CAST(CODE AS VARCHAR) END)=@p_SEARCHNO AND ACTIVE='Y')
				BEGIN
					SELECT CODE,RNO,BRCOMPLAINT,CIRCLEOFFICE,COMPNO,ACCUSED,ALLEGATIONS,CASENO,
						   PRESENTPOSTING,ZONE,SOURCE,SOURCEREF,ACCOUNTNAME,STATUSCODE,
						   AMOUNT,SENTTO,REGION,CASECLOSE,DESIGNATION,NAMEOFINVOFFICIAL,
						   REASONSFORCLOSURE,STATUS,APPROVALSTATUS,
APPROVALSTATUS,
CASE APPROVALSTATUS
    WHEN 'P' THEN 'Pending Approval'
    WHEN 'A' THEN 'Approved'
    WHEN 'C' THEN 'Changes Requested'
    WHEN 'X' THEN 'Rejected'
END AS APPROVALSTATUSTEXT,
						   ISNULL(CHECKERREMARKS,'') AS CHECKERREMARKS,
						   ADDUSER AS ENTRYBY,ISNULL(MODUSER,'') AS MODIFYBY,
						   ISNULL(CONVERT(VARCHAR(50),RECDATECOMP,103),'') AS COMPRECDATE,
						   ISNULL(CONVERT(VARCHAR(50),CLOSUREDT,103),'') AS CLOSUREDATE,
						   ISNULL(CONVERT(VARCHAR(50),DTIAC,103),'') AS IACDATE,
						   ISNULL(CONVERT(VARCHAR(50),SOURCEDATE,103),'') AS SOURCEDATE,
						   ISNULL(CONVERT(VARCHAR(50),SENTFORINVDATE,103),'') AS SENTFORINVDATE,
						   ISNULL(CONVERT(VARCHAR(50),DTOFINVREPORT,103),'') AS INVREPORTDATE,
						   ISNULL(CONVERT(VARCHAR(50),RYSENT,103),'') AS RYSENTDATE,
						   ISNULL(CONVERT(VARCHAR(50),ADDDATE,103),'') AS ENTRYDATE,
						   ISNULL(CONVERT(VARCHAR(50),MODDATE,103),'') AS MODIFYDATE,
						   (CASE WHEN CLOSUREDT IS NULL THEN 'N' ELSE 'Y' END) AS CLOSURE,
						   ([dbo].ReverseColumnValue_Function(REASONSFORCLOSURE,RNO,'COMPLAINT')) AS SHORTREASONSFORCLOSURE,
						   ([dbo].ReverseColumnValue_Function(STATUS,RNO,'COMPLAINT')) AS SHORTSTATUS,DESK_USER_REMARKS,PFNUMBER,BANKNAME,
						   LETTERSENTTO,MARKEDFORINVESTIGATION,
						   ISNULL(CONVERT(VARCHAR(50),LETTERSENTDATE,103),'') AS LETTERSENTDATE,
						   ISNULL(CONVERT(VARCHAR(50),REMINDERDATE,103),'') AS REMINDERDATE,
						   ISNULL(CONVERT(VARCHAR(50),REPLYRECEIVEDDATE,103),'') AS REPLYRECEIVEDDATE,
						   NEWZONE AS NEWZONE,
						   NEWCIRCLE AS NEWCIRCLE
					FROM COMPLAINT WHERE ACTIVE='Y' AND (CASE WHEN @p_VIEW='GET' THEN RNO ELSE CAST(CODE AS VARCHAR) END)=@p_SEARCHNO
					ORDER BY RNO;
				END
			ELSE
				BEGIN
					SET @o_ERRCODE= -1;
					SET @o_EERMSG= @p_SEARCHNO + '- Complaint Number does not Exists......!';
				END
		END
END
GO
