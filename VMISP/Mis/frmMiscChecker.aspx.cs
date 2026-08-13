using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace VMISP.Mis
{
    public partial class frmMiscChecker : System.Web.UI.Page
    {
        // Module code registered in dbo.WORKFLOW_MODULE. MISC belongs to checker group CMP
        // (Complaint & MISC), so only a checker granted that group sees anything here.
        private const string ModuleCode = "MISC";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMiscRecords();
            }
        }

        private void BindMiscRecords()
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
                // spCase_CheckerQueue is the generic inbox query. It scopes the queue through
                // fnCheckerScope to the modules AND zones this user is an active checker for,
                // so no zone or module filtering is needed here beyond naming the module.
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

                gvMisc.DataSource = dt;
                gvMisc.DataBind();

                lblTotal.Text = dt.Rows.Count.ToString();
            }
        }

        /// <summary>
        /// Bootstrap 5 badge modifiers. The inbox is a standalone Bootstrap 5 page, matching
        /// frmIACChecker.aspx and frmVigilanceChecker.aspx -- unlike the verification page,
        /// which mirrors the Bootstrap 3 entry form.
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
