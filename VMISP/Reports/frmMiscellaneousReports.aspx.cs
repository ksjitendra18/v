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
    class MyConfigFileCredentials2 : IReportServerCredentials
    {
        #region IReportServerCredentials Members
        public MyConfigFileCredentials2() { }

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

    public partial class frmMiscellaneousReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void lnkDFSReports_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/DFSReport";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkDFSDetailsReports_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/DFSDetailsReport";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkProgressOfRRBReports_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/ProgressOfRRBReport";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkNatureProcedings_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/NatureProcedings";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkDepartmentalEnquiries_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/DepartmentalEnquiries";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnklnkInvestigation_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/Investigation";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkComplaints_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/Complaints";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }
        private static bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}