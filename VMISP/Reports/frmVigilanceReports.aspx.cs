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
    class MyConfigFileCredentials_VIG : IReportServerCredentials
    {
        #region IReportServerCredentials Members
        public MyConfigFileCredentials_VIG() { }

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

    public partial class frmVigilanceReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkVIGILANCEOUTSTANDING_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/VigilanceOutstanding";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkVIGILANCESTATUS_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/VigilanceStatus";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkFIRSTSTAGEPENDING_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/VigilanceFirstStagePending";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkSECONDSTAGEPENDING_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/VigilanceSecondStagePending";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkSECONDSTAGEPENDINGATDA_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/VigilanceSecondStagePendingAtDA";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkCHARGESHEETNOTSERVED_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/VigilanceChargeSheetNotServed";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkEOPONOTAPPOINTED_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/VigilanceEoPoNotAppointed";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkRECONSIDERVIEWAWIATEDFROMDA_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/VigilanceReconsiderViewAwiatedFromDA";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkENQUIRYISINPROGRESS_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/VigilanceEnquiryIsInProgress";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkVigilanceRetirement_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/VigilanceRetirement";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkFinalOrderAwaited_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/VigilanceFinalOrderAwaited";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkFIRSTSTAGEPENDINGATDESK_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/VigilanceFirstStagePendingatDesk";
                rvMain.ServerReport.ReportServerCredentials = new MyConfigFileCredentials2();
                rvMain.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void lnkMinorChargeSheet_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                Uri reportServerPath = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["report_ipaddress"] + "/reportserver" + System.Web.Configuration.WebConfigurationManager.AppSettings["report_serverstring"]);
                rvMain.ServerReport.ReportServerUrl = reportServerPath;
                rvMain.ServerReport.ReportPath = "/VMIS_Reports/VigilanceMinorChargeSheet";
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