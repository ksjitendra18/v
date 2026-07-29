using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace VMISP.Mis
{
    public partial class frmVigilanceChecker : System.Web.UI.Page
    {
        // Module code registered in dbo.WORKFLOW_MODULE.
        private const string ModuleCode = "VIGILANCE";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVigilanceRecords();
            }
        }

        private void BindVigilanceRecords()
        {
            string userPf = Convert.ToString(Session["userid"]);

            if (string.IsNullOrWhiteSpace(userPf))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                // spCase_CheckerQueue is the generic inbox query. It scopes the queue to the
                // zones this user is an active checker for, so no zone filtering is needed here.
                SqlCommand cmd = new SqlCommand("[dbo].[spCase_CheckerQueue]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@p_USER", userPf);
                cmd.Parameters.AddWithValue("@p_MODULE", ModuleCode);
                cmd.Parameters.AddWithValue("@p_STATUS", "P");

                DataTable dt = new DataTable();

                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
                    lblMsg.Text = "Could not load the checker queue. Please try again.";
                    return;
                }

                gvVigilance.DataSource = dt;
                gvVigilance.DataBind();

                lblTotal.Text = dt.Rows.Count.ToString();
            }
        }

        /// <summary>
        /// Bootstrap 5 badge modifiers. The inbox is a standalone Bootstrap 5 page, matching
        /// frmIACChecker.aspx -- unlike the verification page, which mirrors the Bootstrap 3
        /// entry form. See the layout note in the implementation doc.
        /// </summary>
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
