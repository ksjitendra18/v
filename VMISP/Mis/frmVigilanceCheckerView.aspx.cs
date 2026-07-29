using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace VMISP.Mis
{
    public partial class frmVigilanceCheckerView : System.Web.UI.Page
    {
        // Module code registered in dbo.WORKFLOW_MODULE.
        private const string ModuleCode = "VIGILANCE";

        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            long code = GetRecordCode();

            if (code <= 0)
            {
                Response.Redirect("frmVigilanceChecker.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadVigilance(code);
            }
        }

        /// <summary>
        /// The query string carries VIGILANCE.CODE, which is what CASE_APPROVAL.RecordCode holds.
        /// </summary>
        private long GetRecordCode()
        {
            long code;
            return long.TryParse(Convert.ToString(Request.QueryString["id"]), out code) ? code : 0;
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            TakeAction("A", "approved");
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            TakeAction("X", "rejected");
        }

        protected void btnPushBack_Click(object sender, EventArgs e)
        {
            TakeAction("C", "pushed back for correction");
        }

        private void TakeAction(string actionCode, string actionDescription)
        {
            long code = GetRecordCode();

            if (code <= 0)
            {
                Response.Redirect("frmVigilanceChecker.aspx");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCheckerRemarks.Text))
            {
                lblMsg.CssClass = "label label-danger";
                lblMsg.Text = "Checker Remarks are mandatory before taking any action.";
                LoadVigilance(code);
                return;
            }

            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                // spCase_CheckerAction is the generic action proc. It re-checks authorisation,
                // maker/checker separation and pending status server side, so nothing here is
                // load bearing for the control.
                SqlCommand cmd = new SqlCommand("[dbo].[spCase_CheckerAction]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@p_MODULE", ModuleCode);
                cmd.Parameters.AddWithValue("@p_CODE", code);
                cmd.Parameters.AddWithValue("@p_ACTION", actionCode);
                cmd.Parameters.AddWithValue("@p_REMARKS", txtCheckerRemarks.Text.Trim());
                cmd.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["userid"]));
                cmd.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));
                cmd.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());

                SqlParameter outMsg = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
                SqlParameter outCode = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outMsg);
                cmd.Parameters.Add(outCode);

                con.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
                    lblMsg.CssClass = "label label-danger";
                    lblMsg.Text = "An error occurred while recording your action. Please try again.";
                    LoadVigilance(code);
                    return;
                }

                int errCode = Convert.ToInt32(outCode.Value);
                string message = Convert.ToString(outMsg.Value);

                if (errCode == 1)
                {
                    lblMsg.CssClass = "label label-success";
                    lblMsg.Text = "Vigilance record " + actionDescription + " successfully.";
                }
                else
                {
                    lblMsg.CssClass = "label label-danger";
                    lblMsg.Text = message;
                }
            }

            LoadVigilance(code);
        }

        private void LoadVigilance(long code)
        {
            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                // The entry form shows several of these fields as dropdowns of descriptions while
                // VIGILANCE stores the code. Resolve them here so the checker reads what the maker
                // saw; ISNULL falls back to the raw code if the master row has gone, so nothing
                // silently disappears from the record.
                string query = @"
        SELECT  V.*,
                ISNULL(N.NATURECASE, V.NATURECASE)          AS NATURENAME,
                ISNULL(ST.STS_STATUS, V.STATUSCODE)         AS STATUSNAME,
                ISNULL(RG.REG_NAME, V.REGISTER)             AS REGISTERNAME,
                ISNULL(SC.SCALE, V.SCALE)                   AS SCALENAME,
                ISNULL(PP.PP_NAME, CONVERT(VARCHAR(20), V.PENALTYPROCEEDING)) AS PENALTYPROCEEDINGNAME,
                ISNULL(BD.Branch_name, V.DISAUTHORITYSCIRCLE)                 AS DISAUTHORITYCIRCLENAME,
                ISNULL(BL.BRN_SOLID + ' - ' + BL.BRN_NAME, V.LETTERSENTTO)    AS LETTERSENTTONAME,
                ISNULL(BZ.BRN_SOLID + ' - ' + BZ.BRN_NAME, V.NEWZONE)         AS NEWZONENAME,
                ISNULL(BC.BRN_SOLID + ' - ' + BC.BRN_NAME, V.NEWCIRCLE)       AS NEWCIRCLENAME,
                CA.ApprovalStatus,
                CA.MakerUser,
                CA.MakerDate,
                CA.CheckerRemarks
        FROM    VIGILANCE V
                LEFT JOIN NATURECASE N
                       ON N.CODE = V.NATURECASE AND N.FORTABLE = 'VIGILANCE'
                LEFT JOIN [STATUS] ST
                       ON ST.STS_CODE = V.STATUSCODE AND ST.STS_TABLE = 'VIGILANCE'
                LEFT JOIN REGISTER RG
                       ON RG.REG_CODE = TRY_CONVERT(BIGINT, V.REGISTER) AND RG.REG_TABLE = 'VIGILANCE'
                LEFT JOIN SCALE SC
                       ON SC.CODE = V.SCALE
                LEFT JOIN PENALTYPROCEEDING PP
                       ON PP.PP_CODE = V.PENALTYPROCEEDING AND PP.PP_TABLE = 'VIGILANCE'
                LEFT JOIN BRANCH_MASTER BD
                       ON BD.SOLID = V.DISAUTHORITYSCIRCLE
                LEFT JOIN BRANCH_MASTER_NEW BL
                       ON BL.BRN_SOLID = V.LETTERSENTTO
                LEFT JOIN BRANCH_MASTER_NEW BZ
                       ON BZ.BRN_SOLID = V.NEWZONE
                LEFT JOIN BRANCH_MASTER_NEW BC
                       ON BC.BRN_SOLID = V.NEWCIRCLE
                LEFT JOIN CASE_APPROVAL CA
                       ON CA.ModuleCode = @Module AND CA.RecordCode = V.CODE
        WHERE   V.CODE = @CODE AND V.ACTIVE = 'Y'";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CODE", code);
                cmd.Parameters.AddWithValue("@Module", ModuleCode);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    PopulateControls(dr);
                }
                else
                {
                    lblMsg.CssClass = "label label-danger";
                    lblMsg.Text = "Vigilance record not found.";
                    pnlDecision.Visible = false;
                    pnlVerifyNote.Visible = false;
                    pnlActions.Visible = false;
                }
            }
        }

        private string GetString(SqlDataReader dr, string columnName)
        {
            return dr[columnName] == DBNull.Value
                ? string.Empty
                : dr[columnName].ToString();
        }

        private string GetDate(SqlDataReader dr, string columnName)
        {
            if (dr[columnName] == DBNull.Value)
                return string.Empty;

            DateTime dt;

            if (DateTime.TryParse(dr[columnName].ToString(), out dt))
                return dt.ToString("dd/MM/yyyy");

            return string.Empty;
        }

        private string GetAmount(SqlDataReader dr, string columnName)
        {
            if (dr[columnName] == DBNull.Value)
                return string.Empty;

            decimal amount;

            if (decimal.TryParse(dr[columnName].ToString(), out amount))
                return amount.ToString("N2");

            return string.Empty;
        }

        private void PopulateControls(SqlDataReader dr)
        {
            // Header
            string approvalStatus = GetString(dr, "ApprovalStatus");

            lblRNo.Text = GetString(dr, "RNO");
            lblStatus.Text = GetStatusText(approvalStatus);
            spanStatus.Attributes["class"] = GetStatusClass(approvalStatus);
            lblMaker.Text = GetString(dr, "MakerUser");
            lblMakerDate.Text = GetDate(dr, "MakerDate");

            // Only a record still awaiting verification can be actioned. Anything else is
            // read only here -- the proc enforces the same rule server side.
            // pnlDecision stays visible either way, so the remarks recorded against an
            // already-actioned record can still be read.
            bool isPending = (approvalStatus ?? "").Trim().ToUpper() == "P";
            pnlActions.Visible = isPending;
            pnlVerifyNote.Visible = isPending;
            txtCheckerRemarks.ReadOnly = !isPending;

            // Record fields, in the entry form's order.
            txtRNo.Text = GetString(dr, "RNO");
            txtRNo1.Text = GetString(dr, "RNO1");
            txtNameOfParticulars.Text = GetString(dr, "NAMEOFPARTICULARS");
            txtName.Text = GetString(dr, "NAME");
            txtChargeDate.Text = GetDate(dr, "DTCHARGE");
            txtNatCHSheet.Text = GetString(dr, "NAT_CHSHEET");
            txtZone.Text = GetString(dr, "CBI_ZONE");
            txtStatusCode.Text = GetString(dr, "STATUSNAME");
            txtRegister.Text = GetString(dr, "REGISTERNAME");
            txtCircleOffice.Text = GetString(dr, "CIRCLEOFFICE");
            txtFinal.Text = GetString(dr, "FINAL");
            txtScale.Text = GetString(dr, "SCALENAME");
            txtRNoDate.Text = GetDate(dr, "DTRNO");
            txtPFNo.Text = GetString(dr, "PFNUMBER");
            txtRetirementDate.Text = GetDate(dr, "DTOFRETIREMENT");
            txtDAOrdDate.Text = GetDate(dr, "DT_ORD_DA");
            txtNAPUNDA.Text = GetString(dr, "NA_PUN_DA");
            txtPenaltyType.Text = GetString(dr, "PENALTYTYPE");
            txtDisAuthoritysCircle.Text = GetString(dr, "DISAUTHORITYCIRCLENAME");
            txtDispAuthority.Text = GetString(dr, "DISP_AUTHORITY");
            txtIstDaDate.Text = GetDate(dr, "DT_IST_DA");
            txtDAProposal.Text = GetString(dr, "DAPROPOSAL");
            txtFinalDate.Text = GetDate(dr, "DTFINAL");
            txtCVOAdvice.Text = GetString(dr, "ADVICECVOI");
            txtCVOAdviceDate.Text = GetDate(dr, "DT_CVO_ADVICE");
            txt2ndDADate.Text = GetDate(dr, "DT_2ND_DA");
            txt2DAProposal.Text = GetString(dr, "DAPROPOSAL_2");
            txtCVO2Advice.Text = GetString(dr, "ADVICECVO2");
            txtCVO2AdviceDate.Text = GetDate(dr, "DT_CVO_ADVICE_2");
            txtAccountName.Text = GetString(dr, "ACCTT_NAME");
            txtSource.Text = GetString(dr, "SOURCE");
            txtState.Text = GetString(dr, "STATE");
            txtPlaceinPresentScaleDate.Text = GetDate(dr, "DTOFPLACEMENTINPRESENTSCALE");
            txtSanctionRefusedDate.Text = GetDate(dr, "dtSANCTIONREFUSED");
            txtDesignation.Text = GetString(dr, "DESIGNATION");
            txtPunishmentProposedbyDA.Text = GetString(dr, "PUNISHMENTPROPOSEDBY");
            txtCompRecDate.Text = GetDate(dr, "DATEOFCOMPLAINT");
            txtStatusinBrief.Text = GetString(dr, "STATUS_INBRIEF");
            txtBRComplaint.Text = GetString(dr, "BRNAME");
            txtPenalty.Text = GetString(dr, "PENALTY");
            txtAmount.Text = GetAmount(dr, "AMOUNT");
            txtCSOREPDate.Text = GetDate(dr, "DT_SUB_REP");
            txtConEnqDate.Text = GetDate(dr, "DT_CON_ENQ");
            txtSuspensionDate.Text = GetDate(dr, "DTOFSUSPENSION");
            txtCbiRcNo1.Text = GetString(dr, "CBI_RC_NO1");
            txtRC1Date.Text = GetDate(dr, "DT_RC1");
            txtCBIRCNo2.Text = GetString(dr, "CBI_RC_NO2");
            txtRC2Date.Text = GetDate(dr, "DT_RC2");
            txtCVCOMNo.Text = GetString(dr, "CVC_OM_NO");
            txtOMCVCDate.Text = GetDate(dr, "DT_OM_CVC");
            txtRCSource.Text = GetString(dr, "RC_SOURCE");
            txtInvestig.Text = GetString(dr, "INVESTIG");
            txtAppEODate.Text = GetDate(dr, "DT_APP_EO");
            txtEOName.Text = GetString(dr, "NAME_EO");
            txtAppPODate.Text = GetDate(dr, "DT_APP_PO");
            txtPOName.Text = GetString(dr, "NAME_PO");
            txtCBIRecom.Text = GetString(dr, "RECOM_CBI");
            txtField1.Text = GetString(dr, "FEILD1");
            txtPrevCasePunishment.Text = GetString(dr, "PREVCASE_PUNISHMENTS");
            txtNatureofAccount.Text = GetString(dr, "NATUREOFACCOUNT");
            txtSanctionOrderDate.Text = GetDate(dr, "DTSANCTIONORDER");
            txtRecCVC2.Text = GetDate(dr, "REC_CVC_2");
            txtProposedActiontoCVC.Text = GetString(dr, "PROPOSEDACTIONTOCVC");
            txtCVC2Proposed.Text = GetString(dr, "CVC_2_PROPOSED");
            txtCVC2Ref.Text = GetDate(dr, "REF_CVC_2");
            txtReviewDate.Text = GetDate(dr, "REVIEWDATE");
            txtRegInvok.Text = GetString(dr, "REG_INVOK");
            txtNature.Text = GetString(dr, "NATURENAME");
            txtReferToCVCDate.Text = GetDate(dr, "DTREFERTOCVC");
            txtRecommofCVC.Text = GetString(dr, "RECOMMOFCVC");
            txtCVCAdbiceII.Text = GetString(dr, "CVCSADVICEII");
            txtBasicPay.Text = GetString(dr, "BASICPAY");
            txtLodiCase.Text = GetString(dr, "LODICASE");
            txtLodiNo.Text = GetString(dr, "LODINO");
            txtClosureDate.Text = GetDate(dr, "DATEOFCLOSURE");
            txtLapseNature.Text = GetString(dr, "LAPSENATURE");
            txtA1CSCVC.Text = GetDate(dr, "A1C_CVC");
            txtA1EOPOCVC.Text = GetDate(dr, "A1E_CVC");
            txtA2FOCVC.Text = GetDate(dr, "A2_CVC");
            txtCDIName.Text = GetString(dr, "NAME_CDI");
            txtAppCDIDate.Text = GetDate(dr, "DT_APP_CDI");
            txtPenaltyProceedings.Text = GetString(dr, "PENALTYPROCEEDINGNAME");
            txtLodiInclusionReason.Text = GetString(dr, "LODIINCLUSIONREASON");
            txtLodiDeletionReason.Text = GetString(dr, "LODIDELETIONREASON");
            txtLodiCode.Text = GetString(dr, "LODICODE");
            txtBankName.Text = GetString(dr, "BANKNAME");
            txtTMSACRefNo.Text = GetString(dr, "TMSAC_REF");
            txtLetterSentDate.Text = GetDate(dr, "LETTERSENTDATE");
            txtLetterSentTo.Text = GetString(dr, "LETTERSENTTONAME");
            txtReminderDate.Text = GetDate(dr, "REMINDERDATE");
            txtReplyReceivedDate.Text = GetDate(dr, "REPLYRECEIVEDDATE");
            txtPresentPosting.Text = GetString(dr, "PRESENTPOSTING");
            txtNewZone.Text = GetString(dr, "NEWZONENAME");
            txtNewCircle.Text = GetString(dr, "NEWCIRCLENAME");
            txtStatus.Text = GetString(dr, "STATUS");
            txtDealingOfficerRemarks.Text = GetString(dr, "DESK_USER_REMARKS");

            // Checker Remarks (if previously entered)
            txtCheckerRemarks.Text = GetString(dr, "CheckerRemarks");
        }

        /// <summary>
        /// Bootstrap 3 label classes, to match the styling the Vigilance entry form uses.
        /// </summary>
        protected string GetStatusClass(string status)
        {
            switch ((status ?? "").Trim().ToUpper())
            {
                case "P":
                    return "label label-warning";

                case "A":
                    return "label label-success";

                case "X":
                    return "label label-danger";

                case "C":
                    return "label label-info";

                default:
                    return "label label-default";
            }
        }

        protected string GetStatusText(string status)
        {
            switch ((status ?? "").Trim().ToUpper())
            {
                case "P":
                    return "Pending";

                case "A":
                    return "Approved";

                case "X":
                    return "Rejected";

                case "C":
                    return "Changes Requested";

                default:
                    return "Not Under Workflow";
            }
        }
    }
}
