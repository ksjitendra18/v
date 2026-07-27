using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace VMISP.Mis
{
    public partial class frmIACCheckerView : System.Web.UI.Page
    {
        // Module code registered in dbo.WORKFLOW_MODULE.
        private const string ModuleCode = "IAC";

        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            long sno = GetRecordCode();

            if (sno <= 0)
            {
                Response.Redirect("frmIACChecker.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadIAC(sno);
            }
        }

        /// <summary>
        /// The query string carries IAC.SNO, which is what CASE_APPROVAL.RecordCode holds.
        /// </summary>
        private long GetRecordCode()
        {
            long sno;
            return long.TryParse(Convert.ToString(Request.QueryString["id"]), out sno) ? sno : 0;
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
            long sno = GetRecordCode();

            if (sno <= 0)
            {
                Response.Redirect("frmIACChecker.aspx");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCheckerRemarks.Text))
            {
                lblMsg.CssClass = "label label-danger";
                lblMsg.Text = "Checker Remarks are mandatory before taking any action.";
                LoadIAC(sno);
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
                cmd.Parameters.AddWithValue("@p_CODE", sno);
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
                    LoadIAC(sno);
                    return;
                }

                int errCode = Convert.ToInt32(outCode.Value);
                string message = Convert.ToString(outMsg.Value);

                if (errCode == 1)
                {
                    lblMsg.CssClass = "label label-success";
                    lblMsg.Text = "IAC record " + actionDescription + " successfully.";
                }
                else
                {
                    lblMsg.CssClass = "label label-danger";
                    lblMsg.Text = message;
                }
            }

            LoadIAC(sno);
        }

        private void LoadIAC(long sno)
        {
            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                string query = @"
        SELECT  I.*,
                N.NATURECASE AS NATURENAME,
                CA.ApprovalStatus,
                CA.MakerUser,
                CA.MakerDate,
                CA.CheckerRemarks
        FROM    IAC I
                LEFT JOIN NATURECASE N
                       ON N.CODE = I.NATURECASE AND N.FORTABLE = 'IAC'
                LEFT JOIN CASE_APPROVAL CA
                       ON CA.ModuleCode = @Module AND CA.RecordCode = I.SNO
        WHERE   I.SNO = @SNO AND I.ACTIVE = 'Y'";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SNO", sno);
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
                    lblMsg.Text = "IAC record not found.";
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

            lblIACNo.Text = GetString(dr, "IACNO");
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

            // IAC Summary
            txtIACNo.Text = GetString(dr, "IACNO");
            txtIACNo1.Text = GetString(dr, "IACNO_1");
            txtMeetNo.Text = GetString(dr, "MEETNO");
            txtRecDate.Text = GetDate(dr, "RECDT");
            txtVIGNo.Text = GetString(dr, "VIGNO");
            txtTMSACRefNo.Text = GetString(dr, "TMSAC_REF");
            txtStatusCode.Text = GetString(dr, "STATUSCODE");
            txtClosureDate.Text = GetDate(dr, "CLOSUREDT");

            // Officer and Branch Details
            txtAccused.Text = GetString(dr, "ACCUSED");
            txtPFNumber.Text = GetString(dr, "PFNUMBER");
            txtDesignation.Text = GetString(dr, "DESIGNATION");
            txtScale.Text = GetString(dr, "SCALE");
            txtRetDate.Text = GetDate(dr, "DTRET");
            txtBranch.Text = GetString(dr, "NAMEOFTHEBRANCH");
            txtCircleOffice.Text = GetString(dr, "CIRCLEOFFICE");
            txtZone.Text = GetString(dr, "ZONE");

            // Case Details
            txtAccountName.Text = GetString(dr, "ACNAME");
            txtAmount.Text = GetAmount(dr, "AMOUNT");
            txtNature.Text = GetString(dr, "NATURENAME");
            txtSource.Text = GetString(dr, "SOURCE");
            txtDA.Text = GetString(dr, "DA");
            txtDAView.Text = GetString(dr, "DAVIEW");
            txtIACView.Text = GetString(dr, "IACVIEW");
            txtCVOView.Text = GetString(dr, "CVOVIEW");

            // Communication Details
            txtLetterSentTo.Text = GetString(dr, "LETTERSENTTO");
            txtLetterSentDate.Text = GetDate(dr, "LETTERSENTDATE");
            txtReminderDate.Text = GetDate(dr, "REMINDERDATE");
            txtReplyReceivedDate.Text = GetDate(dr, "REPLYRECEIVEDDATE");
            txtLetterSentToDADate.Text = GetDate(dr, "LETTERSENTTODADATE");
            txtBank.Text = GetString(dr, "BANKNAME");
            txtNewZone.Text = GetString(dr, "NEWZONE");
            txtNewCircle.Text = GetString(dr, "NEWCIRCLE");

            // ABBFF Details
            txtABBFFCase.Text = GetString(dr, "IAC_ABBFF_CASE");
            txtABBFFRefNo.Text = GetString(dr, "IAC_ABBFF_REFNO");
            txtABBFFCaseSubmissionDate.Text = GetDate(dr, "IAC_ABBFF_CASE_SUBMISSION_DATE");
            txtABBFFReplyDate.Text = GetDate(dr, "IAC_ABBFF_REPLY_DATE");
            txtABBFFAdviceReceiveDate.Text = GetDate(dr, "IAC_ABBFF_ADVICE_RECEIVE_DATE");
            txtABBFFAdviceDetail.Text = GetString(dr, "IAC_ABBFF_ADVICE_DETAIL");

            // IAC Status
            txtStatus.Text = GetString(dr, "STATUS");
            txtDeskUserRemarks.Text = GetString(dr, "DESK_USER_REMARKS");

            // Checker Remarks (if previously entered)
            txtCheckerRemarks.Text = GetString(dr, "CheckerRemarks");
        }

        /// <summary>
        /// Bootstrap 3 label classes, to match the styling the IAC entry form uses.
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
