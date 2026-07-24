using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using NLog;

namespace VMISP.Search
{
    public partial class frmVigilanceStatusSearch : System.Web.UI.Page
    {
        public static Logger _logger = LogManager.GetLogger("frmVigilanceStatusSearchLogger");
        CommonFunction objCommonFunction = new CommonFunction();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();

            }
        }

        private void funcSearch()
        {
            _logger.Info("Entered funcSearch");
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            ds.Clear();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spVigilanceStatus_View]";

                cmd.Parameters.AddWithValue("@p_USER", Convert.ToString(ViewState["userid"]));
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(ViewState["role"]));
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_PFNUMBER", txtPFNumber.Text.Trim());

                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables != null)
                {
                    txtLodiStatus.Text = Convert.ToString(ds.Tables[0].Rows[0]["DELETED_FROM_LODI"]);

                    ViewState["DETAILDATA"] = ds.Tables[1];
                    gvDetails.DataSource = ds.Tables[1];
                    gvDetails.DataBind();
                    btnExcelDownload.Visible = true;
                    lastUpdated.Visible = true;

                    
                }
                else
                {
                    ViewState["DETAILDATA"] = null;
                    lblMsg.Text = "Record not Found...";
                    gvDetails.DataSource = null;
                    gvDetails.DataBind();
                    btnExcelDownload.Visible = false;
                    lastUpdated.Visible = false;
                    txtLodiStatus.Text = "";
                    txtName.Text = "";
                }

                //Call HRMS Service
                funcHRMSEmployee();
            }

            catch (Exception es)
            {
                _logger.Info("excpetion in funcSearch: " + es.ToString());
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }

            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public void funcConvertToExcel(DataTable dt)
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "VigilanceStatus");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("", "");
                    Response.AddHeader("content-disposition", "attachment;filename=VigilanceStatus.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception eExcel)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(eExcel);
            }
        }

        protected void btnGetDetails_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            lblMsg.Text = "";

            if (string.IsNullOrEmpty(txtPFNumber.Text.Trim()))
            {
                lblMsg.Text = "Please enter PF Number";
                return;
            }
            else
            {
                funcSearch();
            }
        }

        protected void btnExcelDownload_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Convert.ToString(ViewState["DETAILDATA"])))
            {
                DataTable dtDetails = ((DataTable)ViewState["DETAILDATA"]);
                funcConvertToExcel(dtDetails);
            }
        }

        private void funcHRMSEmployee()
        {
            _logger.Info("Entered HRMS Function");
            try
            {
                if (!string.IsNullOrEmpty(txtPFNumber.Text.Trim()))
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11;

                    string UserID = ConfigurationManager.AppSettings["CBSServiceUserID"];
                    string Password = ConfigurationManager.AppSettings["CBSServicePassword"];
                    string CBSHRMSServiceAPIUrl = ConfigurationManager.AppSettings["CBS_HRMS_SERVICE_EMP"];

                    using (HttpClient clientCBSRequest = new HttpClient())
                    {
                        HttpRequestMessage RequestCBSMessage = new HttpRequestMessage();
                        RequestCBSMessage.RequestUri = new Uri(CBSHRMSServiceAPIUrl + txtPFNumber.Text.Trim());
                        RequestCBSMessage.Method = HttpMethod.Get;
                        RequestCBSMessage.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(UserID + ":" + Password)));
                        RequestCBSMessage.Headers.Add("Accept", "application/json");

                        //Send Request to CBS for Fetch Fastag Recharge Data
                        var resultCBS = clientCBSRequest.SendAsync(RequestCBSMessage).Result;
                        _logger.Info("Response from HRMS API: " + resultCBS.Content.ReadAsStringAsync().Result);
                        if (resultCBS.IsSuccessStatusCode)
                        {
                            var dataCBS = resultCBS.Content.ReadAsStringAsync().Result;
                            HRMS resultHRMS = JsonConvert.DeserializeObject<HRMS>(dataCBS);

                            txtName.Text = resultHRMS.EmployeeName;
                        }
                        else
                        {
                            txtName.Text = "Record not found.";
                        }
                    }
                }
            }
            catch (Exception exHRMS)
            {
                _logger.Info("Exception: " + exHRMS.ToString());
                txtName.Text = exHRMS.Message;
            }
        }

        private static bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}