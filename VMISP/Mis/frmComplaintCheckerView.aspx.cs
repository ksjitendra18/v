using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class frmComplaintCheckerView : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];

            if (string.IsNullOrWhiteSpace(id))
            {
                Response.Redirect("frmComplaintChecker.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadComplaint(id);
                //LoadEODetails();
            }
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
            string id = Request.QueryString["id"];

            if (string.IsNullOrWhiteSpace(id))
            {
                Response.Redirect("frmComplaintChecker.aspx");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCheckerRemarks.Text))
            {
                lblMsg.CssClass = "d-block mb-3 fw-semibold text-danger";
                lblMsg.Text = "Checker Remarks are mandatory before taking any action.";
                LoadComplaint(id);
                return;
            }

            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[spComplaint_CheckerAction]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@p_RNO", id);
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
                    lblMsg.CssClass = "d-block mb-3 fw-semibold text-danger";
                    lblMsg.Text = "An error occurred while recording your action. Please try again.";
                    LoadComplaint(id);
                    return;
                }

                int errCode = Convert.ToInt32(outCode.Value);
                string message = Convert.ToString(outMsg.Value);

                if (errCode == 1)
                {
                    lblMsg.CssClass = "d-block mb-3 fw-semibold text-success";
                    lblMsg.Text = "Complaint " + actionDescription + " successfully.";
                }
                else
                {
                    lblMsg.CssClass = "d-block mb-3 fw-semibold text-danger";
                    lblMsg.Text = message;
                }
            }

            LoadComplaint(id);
        }

        private void LoadComplaint(string id)
        {
            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                string query = @"SELECT * FROM COMPLAINT WHERE RNO = @RNO";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RNO", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    PopulateControls(dr);
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
            string approvalStatus = GetString(dr, "APPROVALSTATUS");
            lblComplaintNo.Text = GetString(dr, "RNO");
            lblStatus.Text = GetStatusText(approvalStatus);
            spanStatus.Attributes["class"] = "badge badge-status " + GetStatusClass(approvalStatus);

            bool isPending = (approvalStatus ?? "").Trim().ToUpper() == "P";
            pnlActions.Visible = isPending;
            txtCheckerRemarks.ReadOnly = !isPending;

            // Complaint Summary
            txtComplaintDate.Text = GetDate(dr, "RECDATECOMP");
            txtCircleOffice.Text = GetString(dr, "CIRCLEOFFICE");
            txtInternalRef.Text = GetString(dr, "COMPNO");
            txtClosureDate.Text = GetDate(dr, "CLOSUREDT");

            // Complaint Details
            txtBranchComplaint.Text = GetString(dr, "BRCOMPLAINT");
            txtAccused.Text = GetString(dr, "ACCUSED");
            txtAllegations.Text = GetString(dr, "ALLEGATIONS");

            // Investigation Details
            txtCaseNo.Text = GetString(dr, "CASENO");
            txtIACDate.Text = GetDate(dr, "DTIAC");
            txtPresentPosting.Text = GetString(dr, "PRESENTPOSTING");

            //txtZone.Text = GetString(dr, "ZONE");

            txtSentTo.Text = GetString(dr, "SENTTO");
            txtSourceDate.Text = GetDate(dr, "SOURCEDATE");
            txtSourceReference.Text = GetString(dr, "SOURCEREF");
            txtAmount.Text = GetAmount(dr, "AMOUNT");
            txtAccountName.Text = GetString(dr, "ACCOUNTNAME");
            txtExternalSource.Text = GetString(dr, "SOURCE");
            txtRegion.Text = GetString(dr, "REGION");
            txtClose.Text = GetString(dr, "CASECLOSE");

            // Official Details
            txtINVReportDate.Text = GetDate(dr, "DTOFINVREPORT");
            txtDesignation.Text = GetString(dr, "DESIGNATION");
            txtINVOfficer.Text = GetString(dr, "NAMEOFINVOFFICIAL");
            txtRYSent.Text = GetDate(dr, "RYSENT");
            txtStatusCode.Text = GetString(dr, "STATUSCODE");
            txtPFNumber.Text = GetString(dr, "PFNUMBER");
            txtLetterSentDate.Text = GetDate(dr, "LETTERSENTDATE");
            txtClosureReason.Text = GetString(dr, "REASONSFORCLOSURE");

            // Communication Details
            txtLetterSentTo.Text = GetString(dr, "LETTERSENTTO");
            txtReminderDate.Text = GetDate(dr, "REMINDERDATE");
            txtReplyReceivedDate.Text = GetDate(dr, "REPLYRECEIVEDDATE");
            txtBank.Text = GetString(dr, "BANKNAME");
            txtNewZone.Text = GetString(dr, "NEWZONE");
            txtNewCircle.Text = GetString(dr, "NEWCIRCLE");

            // Complaint Status
            txtStatus.Text = GetString(dr, "STATUS");
            //txtDeskUserRemarks.Text = GetString(dr, "DESK_USER_REMARKS");

            // Checker Remarks (if previously entered)
            txtCheckerRemarks.Text = GetString(dr, "CHECKERREMARKS");
        }

        protected string GetStatusClass(string status)
        {
            switch ((status ?? "").Trim().ToUpper())
            {
                case "P":
                    return "badge-pending";

                case "A":
                    return "badge-closed";

                case "X":
                    return "bg-danger";

                case "C":
                    return "badge-progress";

                default:
                    return "bg-secondary";
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
                    return status;
            }
        }
    }
}