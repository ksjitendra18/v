using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using NLog;
using System.Configuration;

namespace VMISP
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Cache-Control", "no-cache"); //HTTP 1.1            
            Response.AddHeader("Cache-Control", "no-store"); // HTTP 1.1
            Response.AddHeader("Cache-Control", "must-revalidate"); // HTTP 
            Response.AddHeader("Pragma", "no-cache"); // HTTP 1.1 
            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string returnUrl = Session["returnURL"].ToString();
            //logger.Info("Master page Loaded");
            if (Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null && funcCheckCurrentSession())
            {
                if (Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    logger.Info("Master page Loaded AuthToken");

                    Label1.Text = Convert.ToString(Session["solname"]);
                    Label2.Text = Convert.ToString(Session["nameofuser"]);
                    logger.Info(Session["solname"]);
                    DateTime date = DateTime.Now;
                    lblDateTime.Text = String.Format("{0:f}", date);

                    if (Convert.ToString(Session["role"]).ToUpper() == "VMIS_ADMIN")
                    {
                        Label3.Text = "Vigilance Admin User";
                    }
                    if (Convert.ToString(Session["role"]).ToUpper() == "VMIS_DESKUSER")
                    {
                        Label3.Text = "Vigilance Desk User";
                    }
                    if (Convert.ToString(Session["role"]).ToUpper() == "VMIS_MISUSER")
                    {
                        Label3.Text = "Vigilance Mis User";
                    }
                    if (Convert.ToString(Session["role"]).ToUpper() == "VMIS_SUPERUSER")
                    {
                        Label3.Text = "Vigilance Super User";
                    }
                    if (Convert.ToString(Session["role"]).ToUpper() == "VMIS_VIEWUSER")
                    {
                        Label3.Text = "Vigilance View User";
                    }
                }
            }
            else
            {
                Cache.Remove("VMISessionId" + Convert.ToString(Session["userid"]));
                String abc = Request.Cookies["VMISessionId"].ToString();
                string CookiePath = ConfigurationManager.AppSettings["UserDefiniedCookiePathFilter"];
                Response.Cookies.Add(new HttpCookie("VMISessionId")
                {
                    HttpOnly = true,
                    Value = "",
                    Path = CookiePath + ";SameSite=Strict",
                    Secure = true
                });
                Session.RemoveAll();
                Session.Abandon();
                Session.Clear();
                //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                //New Code

                //if (Request.Cookies["ASP.NET_SessionId"] != null)
                //{
                //    Response.Cookies["ASP.NET_SessionId"].Value = String.Empty;
                //    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                //}
                //if (Request.Cookies["AuthToken"] != null)
                //{
                //    Response.Cookies["AuthToken"].Value = String.Empty;
                //    Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                //}

                string SSOURL = string.Format(ConfigurationManager.AppSettings["SSODefaultPageWithMessage"], "User logged out successfully.");
                Response.Redirect(SSOURL);
            }
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["userid"] == null)
                {
                    FormsAuthentication.SignOut();
                    Session.Abandon();
                    string SSOURL = string.Format(ConfigurationManager.AppSettings["SSODefaultPageWithMessage"], "User logged out successfully.");
                    Response.Redirect(SSOURL);
                }

                //if (Cache["VMISessionId" + Convert.ToString(Session["userid"])] != null)
                //{
                //    if (Cache["VMISessionId" + Convert.ToString(Session["userid"])].ToString() != Session.SessionID)
                //    {
                //        string SSOURL = string.Format(ConfigurationManager.AppSettings["SSODefaultPageWithMessage"], "User logged out successfully.");
                //        Response.Redirect(SSOURL);
                //    }
                //}

                //First, check for the existence of the Anti-XSS cookie
                var requestCookie = Request.Cookies[AntiXsrfTokenKey];
                Guid requestCookieGuidValue;

                //If the CSRF cookie is found, parse the token from the cookie.
                //Then, set the global page variable and view state user
                //key. The global variable will be used to validate that it matches 
                //in the view state form field in the Page.PreLoad method.
                if (requestCookie != null
                    && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
                {
                    //Set the global token variable so the cookie value can be
                    //validated against the value in the view state form field in
                    //the Page.PreLoad method.
                    _antiXsrfTokenValue = requestCookie.Value;

                    //Set the view state user key, which will be validated by the
                    //framework during each request
                    Page.ViewStateUserKey = _antiXsrfTokenValue;
                }
                //If the CSRF cookie is not found, then this is a new session.
                else
                {
                    //Generate a new Anti-XSRF token
                    _antiXsrfTokenValue = Guid.NewGuid().ToString("N");

                    //Set the view state user key, which will be validated by the
                    //framework during each request
                    //Page.ViewStateUserKey = _antiXsrfTokenValue;

                    //Create the non-persistent CSRF cookie
                    //var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                    //{
                    //    //Set the HttpOnly property to prevent the cookie from
                    //    //being accessed by client side script
                    //    HttpOnly = true,
                    //    //Add the Anti-XSRF token to the cookie value
                    //    Value = _antiXsrfTokenValue,
                    //    Path = "/VigilanceMIS",
                    //};

                    //If we are using SSL, the cookie should be set to secure to
                    //prevent it from being sent over HTTP connections
                    //if (FormsAuthentication.RequireSSL &&
                    //    Request.IsSecureConnection)
                    //{
                    //    responseCookie.Secure = true;
                    //}

                    //Add the CSRF cookie to the response
                    //Response.Cookies.Set(responseCookie);
                }

                //Page.PreLoad += master_Page_PreLoad;
            }
            catch (Exception ex)
            {
                logger.Error($"Page_Init sitemaster.master.cs {ex.ToString()}");
            }


        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            //During the initial page load, add the Anti-XSRF token and user
            //name to the ViewState
            if (!IsPostBack)
            {
                //Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;

                //If a user name is assigned, set the user name
                ViewState[AntiXsrfUserNameKey] =
                       Context.User.Identity.Name ?? String.Empty;
            }
            //During all subsequent post backs to the page, the token value from
            //the cookie should be validated against the token in the view state
            //form field. Additionally user name should be compared to the
            //authenticated users name
            else
            {
                //Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] !=
                         (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of " +
                                        "Anti-XSRF token failed.");
                }
            }
        }

        protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                con.Open();
                cmd.Connection = con;
                String strUniqueID = Convert.ToString(Session["VMISP_TRACE_ID"]);
                String returnurl = ConfigurationManager.AppSettings["SSODefaultPageWithMessage"];
                if (strUniqueID != "")
                {
                    cmd.CommandText = "UPDATE USER_TRACE SET LOGOUTTIME=GETDATE() WHERE UNIQUEID='" + strUniqueID + "'";
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        FormsAuthentication.SignOut();
                        Session.Abandon();
                        Cache.Remove("VMISessionId" + Convert.ToString(Session["userid"]));
                        String abc = Convert.ToString(Request.Cookies["VMISessionId"]);
                        Response.Cookies.Add(new HttpCookie("VMISessionId", ""));
                        Session.RemoveAll();
                        Session.Abandon();
                        Session.Clear();

                        if (Request.Cookies["VMISessionId"] != null)
                        {
                            Response.Cookies["VMISessionId"].Value = String.Empty;
                            Response.Cookies["VMISessionId"].Expires = DateTime.Now.AddMonths(-20);
                        }
                        if (Request.Cookies["AuthToken"] != null)
                        {
                            Response.Cookies["AuthToken"].Value = String.Empty;
                            Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                        }

                        string SSOURL = string.Format(returnurl + "?msg={0}", "User logged out successfully.");

                        Response.Redirect(SSOURL);
                    }
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmd.Dispose();
                con.Dispose();
                con.Close();
            }
        }

        private bool funcCheckCurrentSession()
        {
            bool result = false;
            DataTable dt = new DataTable();
            SqlConnection conView = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdView = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmdView);

            try
            {
                conView.Open();
                cmdView.Connection = conView;
                cmdView.Parameters.Clear();
                cmdView.CommandType = CommandType.StoredProcedure;
                cmdView.CommandText = "[dbo].[spUserConcurrent_CheckSession]";

                cmdView.Parameters.AddWithValue("@p_UserID", Convert.ToString(Session["userid"]));
                cmdView.Parameters.AddWithValue("@p_AuthToken", Convert.ToString(Session["AuthToken"]));

                cmdView.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = true;
                }

            }
            catch (Exception ex)
            {
                logger.Info("Exception under funcCheckCurrentSession: " + ex.ToString());
            }

            finally
            {
                conView.Close();
                sda.Dispose();
                cmdView.Dispose();
                conView.Dispose();
            }
            logger.Info(result);
            return result;
        }

    }
}