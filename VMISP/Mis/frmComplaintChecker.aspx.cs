using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace VMISP.Mis
{
    public partial class frmComplaintChecker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindComplaints();
            }
        }

        private void BindComplaints()
        {
            string userPf = Session["userid"].ToString();

            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                // Scoped by fnCheckerScope, which resolves a checker's grants to (module, zone)
                // pairs. Joining it on ModuleCode = 'COMPLAINT' means a checker granted only
                // Vigilance & IAC sees nothing here, even in a zone they check for those modules.
                // Complaint still keeps its approval state in its own columns rather than in
                // CASE_APPROVAL, which is why this is a query and not spCase_CheckerQueue.
                string query = @"
     SELECT
            C.RNO,
            C.COMPNO,
            C.BRCOMPLAINT,
            C.RECDATECOMP,
            C.APPROVALSTATUS
        FROM COMPLAINT C
        INNER JOIN dbo.fnCheckerScope(@UserPF) S
            ON S.ModuleCode = 'COMPLAINT'
           AND S.ZoneSolID  = C.NEWZONE
        WHERE C.APPROVALSTATUS = 'P'
        ORDER BY C.RECDATECOMP DESC;
";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserPF", userPf);

                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                gvComplaint.DataSource = dt;
                gvComplaint.DataBind();

                lblTotal.Text = dt.Rows.Count.ToString();
            }
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