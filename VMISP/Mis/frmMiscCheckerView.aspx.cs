using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace VMISP.Mis
{
    public partial class frmMiscCheckerView : System.Web.UI.Page
    {
        // Module code registered in dbo.WORKFLOW_MODULE. MISC belongs to checker group CMP.
        private const string ModuleCode = "MISC";

        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            long code = GetRecordCode();

            if (code <= 0)
            {
                Response.Redirect("frmMiscChecker.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadMisc(code);
            }
        }

        /// <summary>
        /// The query string carries MISC.CODE, which is what CASE_APPROVAL.RecordCode holds.
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
                Response.Redirect("frmMiscChecker.aspx");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCheckerRemarks.Text))
            {
                lblMsg.CssClass = "label label-danger";
                lblMsg.Text = "Checker Remarks are mandatory before taking any action.";
                LoadMisc(code);
                return;
            }

            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                // spCase_CheckerAction is the generic action proc. It re-checks module scope,
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
                    LoadMisc(code);
                    return;
                }

                int errCode = Convert.ToInt32(outCode.Value);
                string message = Convert.ToString(outMsg.Value);

                if (errCode == 1)
                {
                    lblMsg.CssClass = "label label-success";
                    lblMsg.Text = "MISC record " + actionDescription + " successfully.";
                }
                else
                {
                    lblMsg.CssClass = "label label-danger";
                    lblMsg.Text = message;
                }
            }

            LoadMisc(code);
        }

        private void LoadMisc(long code)
        {
            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                string query = @"
        SELECT  M.*,
                N.NATURECASE AS NATURENAME,
                CA.ApprovalStatus,
                CA.MakerUser,
                CA.MakerDate,
                CA.CheckerRemarks
        FROM    MISC M
                LEFT JOIN NATURECASE N
                       ON N.CODE = M.NATURE AND N.FORTABLE = 'MISC'
                LEFT JOIN CASE_APPROVAL CA
                       ON CA.ModuleCode = @Module AND CA.RecordCode = M.CODE
        WHERE   M.CODE = @CODE AND M.ACTIVE = 'Y'";

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
                    lblMsg.Text = "MISC record not found.";
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

            // Complaint summary
            txtRNo.Text = GetString(dr, "RNO");
            txtCompRecDate.Text = GetDate(dr, "RECDATECOMP");
            txtCircleOffice.Text = GetString(dr, "CIRCLEOFFICE");
            txtBRComplaint.Text = GetString(dr, "BRCOMPLAINT");
            txtCompNo.Text = GetString(dr, "COMPNO");
            txtClosureDate.Text = GetDate(dr, "CLOSUREDT");
            txtAccused.Text = GetString(dr, "ACCUSED");
            txtAllegations.Text = GetString(dr, "ALLEGATIONS");

            // Case details
            txtAmount.Text = GetAmount(dr, "AMOUNT");
            txtNPADate.Text = GetDate(dr, "NPADATE");
            txtFinalAction.Text = GetString(dr, "FINALACTION");
            txtSource.Text = GetString(dr, "SOURCE");
            txtZone.Text = GetString(dr, "ZONE");
            txtSourceDate.Text = GetDate(dr, "SOURCEDATE");
            txtSourceRef.Text = GetString(dr, "SOURCEREF");
            txtAccountName.Text = GetString(dr, "ACCOUNTNAME");
            txtClose.Text = GetString(dr, "CASECLOSE");
            txtDateForINVReport.Text = GetDate(dr, "DTOFINVREPORT");
            txtDesignation.Text = GetString(dr, "DESIGNATION");
            txtNatureComp.Text = GetString(dr, "NATURECOMP");
            txtInvestigationDate.Text = GetDate(dr, "DTINVESTIGATION");
            txtType.Text = GetString(dr, "TYPE");
            txtStatusCode.Text = GetString(dr, "STATUSCODE");

            // NATURE stores a code; the entry form's dropdown shows the description, so
            // resolve it here rather than showing the checker a number.
            txtNature.Text = GetString(dr, "NATURENAME");

            txtRessonsForClosure.Text = GetString(dr, "REASONSFORCLOSURE");

            // Communication details
            txtLetterSentDate.Text = GetDate(dr, "LETTERSENTDATE");
            txtLetterSentTo.Text = GetString(dr, "LETTERSENTTO");
            txtReminderDate.Text = GetDate(dr, "REMINDERDATE");
            txtReplyReceivedDate.Text = GetDate(dr, "REPLYRECEIVEDDATE");
            txtBankName.Text = GetString(dr, "BANKNAME");
            txtPFNumber.Text = GetString(dr, "PFNO");
            txtZoneNew.Text = GetString(dr, "NEWZONE");
            txtCircleNew.Text = GetString(dr, "NEWCIRCLE");
            txtZoneType.Text = GetString(dr, "ZONE_TYPE");
            txtZOCM.Text = GetString(dr, "ZONE_CM");

            // MISC status
            txtStatus.Text = GetString(dr, "STATUS");
            txtDeskUserRemarks.Text = GetString(dr, "DESK_USER_REMARKS");

            // Checker Remarks (if previously entered)
            txtCheckerRemarks.Text = GetString(dr, "CheckerRemarks");
        }

        /// <summary>
        /// Bootstrap 3 label classes, to match the styling the MISC entry form uses.
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
