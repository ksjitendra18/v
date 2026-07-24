using System;
using System.Collections.Generic;
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
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                string id = Request.QueryString["id"];

                if (string.IsNullOrWhiteSpace(id))
                {
                    Response.Redirect("frmComplaintChecker.aspx");
                    return;
                }

                LoadComplaint(id);
                //LoadEODetails();
            }
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
            lblComplaintNo.Text = GetString(dr, "RNO");
            lblStatus.Text = GetString(dr, "APPROVALSTATUS");

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
    }
}