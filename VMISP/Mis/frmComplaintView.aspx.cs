using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace VMISP.Mis
{
    public partial class frmComplaintView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.QueryString["id"];

                if (string.IsNullOrWhiteSpace(id))
                {
                    Response.Redirect("frmComplaint.aspx");
                    return;
                }

                LoadComplaint(id);
                LoadEODetails(id);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmComplaint.aspx");
        }

        private void LoadComplaint(string rno)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[spComplaint_View]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@p_VIEW", "GET");
                cmd.Parameters.AddWithValue("@p_SEARCHNO", rno);

                SqlParameter outMsg = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter outCode = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outMsg);
                cmd.Parameters.Add(outCode);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    lblMsg.Text = Convert.ToString(outMsg.Value);
                    return;
                }
            }

            DataRow row = dt.Rows[0];

            txtRNo.Text = GetString(row, "RNO");
            txtCompRecDate.Text = GetString(row, "COMPRECDATE");
            txtCircleOffice.Text = GetString(row, "CIRCLEOFFICE");
            txtBRComplaint.Text = GetString(row, "BRCOMPLAINT");
            txtCompNo.Text = GetString(row, "COMPNO");
            txtClosureDate.Text = GetString(row, "CLOSUREDATE");
            txtAccused.Text = GetString(row, "ACCUSED");
            txtAllegations.Text = GetString(row, "ALLEGATIONS");
            txtCaseNo.Text = GetString(row, "CASENO");
            txtIACDate.Text = GetString(row, "IACDATE");
            txtPresentPosting.Text = GetString(row, "PRESENTPOSTING");
            txtZone.Text = GetString(row, "ZONE");
            txtSentTo.Text = GetString(row, "SENTTO");
            txtSourceDate.Text = GetString(row, "SOURCEDATE");
            txtSourceRef.Text = GetString(row, "SOURCEREF");
            txtAmount.Text = GetString(row, "AMOUNT");
            txtAccountName.Text = GetString(row, "ACCOUNTNAME");
            txtSentForInvDate.Text = GetString(row, "SENTFORINVDATE");
            txtSource.Text = GetString(row, "SOURCE");
            txtRegion.Text = GetString(row, "REGION");
            txtClose.Text = GetString(row, "CASECLOSE");
            txtDateForINVReport.Text = GetString(row, "INVREPORTDATE");
            txtDesignation.Text = GetString(row, "DESIGNATION");
            txtNameINVOfficial.Text = GetString(row, "NAMEOFINVOFFICIAL");
            txtRYSent.Text = GetString(row, "RYSENTDATE");
            txtStatusCode.Text = GetString(row, "STATUSCODE");
            txtPFNumber.Text = GetString(row, "PFNUMBER");
            txtLetterSentDate.Text = GetString(row, "LETTERSENTDATE");
            txtRessonsForClosure.Text = GetString(row, "REASONSFORCLOSURE");
            txtLetterSentTo.Text = GetString(row, "LETTERSENTTO");
            txtReminderDate.Text = GetString(row, "REMINDERDATE");
            txtReplyReceivedDate.Text = GetString(row, "REPLYRECEIVEDDATE");
            txtBankName.Text = GetString(row, "BANKNAME");
            txtZoneNew.Text = GetString(row, "NEWZONE");
            txtCircleNew.Text = GetString(row, "NEWCIRCLE");
            txtStatus.Text = GetString(row, "STATUS");
            txtDealingOfficerRemarks.Text = GetString(row, "DESK_USER_REMARKS");
            txtCheckerStatus.Text = GetString(row, "APPROVALSTATUSTEXT");
            txtCheckerRemarks.Text = GetString(row, "CHECKERREMARKS");
        }

        private void LoadEODetails(string rno)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[spComplaintEO_View]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@p_UNIQUEID", rno);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }

            gvEODetails.DataSource = dt.Rows.Count > 0 ? dt : null;
            gvEODetails.DataBind();
        }

        private string GetString(DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return string.Empty;

            return row[columnName].ToString();
        }
    }
}
