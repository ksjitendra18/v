using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Security.Principal;
using System.Net;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VMISP.Reports
{
    [Serializable]
    class MyConfigFileCredentials1 : IReportServerCredentials
    {
        #region IReportServerCredentials Members
        public MyConfigFileCredentials1() { }

        public WindowsIdentity ImpersonationUser
        {
            get
            {
                //WindowsIdentity x = WindowsIdentity.GetCurrent();
                return null;
            }
        }

        public ICredentials NetworkCredentials
        {
            get
            {
                //  return null;
                return new NetworkCredential(WebConfigurationManager.AppSettings["report_uid"], WebConfigurationManager.AppSettings["report_pwd"], System.Web.Configuration.WebConfigurationManager.AppSettings["report_servername"]);
            }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;
            return false;
        }
        #endregion
    }

    public partial class frmComplaintRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkComplaintsReports_Click(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
            rvMain.ServerReport.ReportServerUrl = reportServerPath;
            rvMain.ServerReport.ReportPath = "/VMIS_Reports/rptComplaints";
            rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials1();
            rvMain.ServerReport.Refresh();
        }

        protected void lnkPenaltyProcedings_Click(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
            rvMain.ServerReport.ReportServerUrl = reportServerPath;
            rvMain.ServerReport.ReportPath = "/VMIS_Reports/PenaltyProcedings";
            rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials1();
            rvMain.ServerReport.Refresh();
        }
        private static bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
