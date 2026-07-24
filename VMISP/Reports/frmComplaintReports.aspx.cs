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
    class MyConfigFileCredentials3 : IReportServerCredentials
    {
        #region IReportServerCredentials Members
        public MyConfigFileCredentials3() { }

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

    public partial class frmComplaintReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void lnkCOMPLAINTOUTSTANDING_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/ComplaintOutstanding";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkCOMPLAINTSTATUS_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/ComplaintStatus";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkCOMPLAINTREPORTTOMD_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/ComplaintReportToMD";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkCOMPLAINTOUTSTANDINGCVC_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/ComplaintOutstandingCVC";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkCOMPLAINTOUTSTANDINGOTHERS_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/ComplaintOutstandingOthers";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkCOMPLAINTOUTSTANDINGASONDATE_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/ComplaintOutstandingAsOnDate";
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