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
                string query = @"
     SELECT
            C.RNO,
            C.COMPNO,
            C.BRCOMPLAINT,
            C.RECDATECOMP,
            C.APPROVALSTATUS
        FROM COMPLAINT C
        INNER JOIN MakerCheckerMapping UZM
            ON C.NEWZONE = UZM.ZoneSolID
        WHERE UZM.UserPF = '5224503'
            AND UZM.IsChecker = 1
            AND UZM.IsActive = 1
            AND C.APPROVALSTATUS = 'P'
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
                    return "badge-progress";

                case "R":
                    return "bg-danger";

                case "C":
                    return "badge-closed";

                default:
                    return "bg-secondary";
            }
        }
    }
}