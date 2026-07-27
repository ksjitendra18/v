using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using VMISP;
using System.Net.Mail;
using NLog;
using Newtonsoft.Json;
using VMISP.SSO;

namespace VMISP
{
    public partial class Login : System.Web.UI.Page
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {
            //TextBox txtUserName = (TextBox)Login1.FindControl("UserName");
            //txtUserName.Attributes.Add("autocomplete", "off");
            //TextBox txtPassword = (TextBox)Login1.FindControl("Password");
            //txtPassword.Attributes.Add("autocomplete", "off");

            //logger.Info(Request.Form["enc_token"]);

            if (!string.IsNullOrEmpty(Request.Form["enc_token"]))
            {
                logger.Info("Page Load: Request Received from SSO" + Request.Form["enc_token"]);
                //Request Received from SSO
                string SSOReqData = JsonConvert.SerializeObject(new { SSOToken = Request.Form["enc_token"] });
                string SSORespData = SSOLayer.GETSSOData(SSOReqData);
                logger.Info(SSORespData);
                if (string.IsNullOrEmpty(SSORespData))
                {
                    var errurl = string.Format(System.Configuration.ConfigurationManager.AppSettings["SSODefaultPageWithMessage"], "Invalid Token");
                    Response.Redirect(errurl);
                }
                else
                {

                    SSOResponse ssodata = JsonConvert.DeserializeObject<SSOResponse>(SSORespData);
                    Session["returnURL"] = ssodata.returnURL;
                    if (ssodata == null)
                    {
                        var errurl = string.Format(ssodata.returnURL + "?msg={0}", "Unable to decrypt token");
                        Response.Redirect(errurl);
                    }
                    else
                    {
                        if (ssodata.Username.Equals(Request.Form["userid"]))
                        {
                            logger.Info(SSORespData);
                            funcSSOLogin(ssodata.Username, ssodata.returnURL);

                        }
                        else
                        {
                            var errurl = string.Format(ssodata.returnURL + "?msg={0}", "SSO Data and Posted Data does not match");
                            Response.Redirect(errurl);
                        }
                    }
                }
            }
            /*else
            {
                logger.Info(Request.Form["enc_token"]);
                logger.Info("Request not received from SSO.");
                string SSOURL = string.Format(System.Configuration.ConfigurationManager.AppSettings["SSODefaultPageWithMessage"], "Session Ended.");
                Response.Redirect(SSOURL);
            }*/

            Session["returnURL"] = "https://10.192.3.99/ssouat/sso.php";
            //funcSSOLogin("5180079", "https://10.192.3.99/ssouat/sso.php");
            //funcSSOLogin("5224580", "https://10.192.3.99/ssouat/sso.php");
            //funcSSOLogin("5167639", "https://10.192.3.99/ssouat/sso.php"); // VMIS_DESKUSER
            //funcSSOLogin("5224579", "https://10.192.3.99/ssouat/sso.php"); //admin
            //funcSSOLogin("5213381", "https://10.192.3.99/ssouat/sso.php");
           funcSSOLogin("5224503", "https://10.192.3.99/ssouat/sso.php"); //checker
            //funcSSOLogin("5224563", "https://10.192.3.99/ssouat/sso.php"); //mis
            //funcSSOLogin("5224579", "https://10.192.3.99/ssouat/sso.php");
        }

        private static readonly string[] PrimaryRoles =
{
 "VMIS_ADMIN",
"VMIS_CHECKER",
"VMIS_DESKUSER",
"VMIS_MISUSER",
"VMIS_SUPERUSER",
"VMIS_VIEWUSER"
    // Add future mutually-exclusive roles here
};

        private static readonly string[] SecondaryRoles =
        {
    "VMIS_CHECKER"
    // Add future independent roles here
};

        private void funcSSOLogin(string UserId, string url)
        {
            try
            {
                WebProfile wp = WebProfile.GetProfile(UserId);

                if (wp == null)
                {
                    var errurl = string.Format(url + "?msg={0}", "User " + UserId + " not present in VigilanceMIS");
                    Response.Redirect(errurl);
                }
                else
                {
                    logger.Info("User Exists");
                    MembershipUser mu = Membership.GetUser(UserId);
                    Session["solname"] = wp.solname;
                    Session["nameofuser"] = wp.nameofuser;
                    Session["changepwd"] = wp.changepwd;
                    Session["userid"] = UserId;
                    Session["sol"] = Convert.ToInt64(wp.sol).ToString("0000");
                    Session["solid"] = wp.sol + "00";
                    Session["hosol"] = WebConfigurationManager.AppSettings["hosol"].ToString();

                    if (Cache["VMISessionId" + Session["userid"].ToString()] == null)
                    {
                        Cache.Add("VMISessionId" + Session["userid"].ToString(), Session.SessionID, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["CacheTimeOut"])), System.Web.Caching.CacheItemPriority.NotRemovable, (System.Web.Caching.CacheItemRemovedCallback)null);
                    }
                    else if (hiddenforcelogin.Value == "1")
                    {
                        Cache["VMISessionId" + Session["userid"].ToString()] = Session.SessionID;
                    }
                    ;
                    string[] roles = Roles.GetRolesForUser(UserId);

                    //IEnumerable<string> qry = from info_role in roles where info_role.Contains("VMIS_") select info_role;
                    //if (qry.Count() == 1)
                    //{
                    //    Session["role"] = qry.First();
                    //    string GUID = Guid.NewGuid().ToString();
                    //    Session["AuthToken"] = GUID;
                    //    Response.Cookies.Add(new HttpCookie("AuthToken", GUID));
                    //    string UserAuthToken = null;
                    //    UserAuthToken = funcValidateSingleUserLogin(UserId);

                    //    if (UserAuthToken != null)
                    //    {
                    //        funcTerminateUserAlreadyLogin(UserAuthToken);
                    //        Session.Remove(UserAuthToken);  //Remove Seesion
                    //    }
                    //    funcDeactivateAllSessions();
                    //    funcInsertCurrentSession();
                    //    funcUpdateUserTrace("LOGIN SUCCESS", "Others User Login Successfully.", Convert.ToString(Session["AuthToken"]));

                    //    FormsAuthentication.SetAuthCookie(UserId, true);
                    //    Response.Redirect("~/Default.aspx", true);
                    //}
                    //else
                    //{
                    //    string SSOURL = string.Format(url + "?msg={0}", "User Role for user  " + UserId + " not present in VigilanceMIS");
                    //    Response.Redirect(SSOURL);
                    //}


                    string primaryRole = roles.FirstOrDefault(r => PrimaryRoles.Contains(r));

                    if (!string.IsNullOrEmpty(primaryRole))
                    {
                        Session["role"] = primaryRole;

                        // Optional
                        Session["IsChecker"] = roles.Contains("VMIS_CHECKER");
                        Session["Roles"] = roles;

                        string GUID = Guid.NewGuid().ToString();
                        Session["AuthToken"] = GUID;
                        Response.Cookies.Add(new HttpCookie("AuthToken", GUID));

                        string UserAuthToken = funcValidateSingleUserLogin(UserId);

                        if (UserAuthToken != null)
                        {
                            funcTerminateUserAlreadyLogin(UserAuthToken);
                            Session.Remove(UserAuthToken);
                        }

                        funcDeactivateAllSessions();
                        funcInsertCurrentSession();
                        funcUpdateUserTrace("LOGIN SUCCESS",
                            "Others User Login Successfully.",
                            Convert.ToString(Session["AuthToken"]));

                        FormsAuthentication.SetAuthCookie(UserId, true);
                        Response.Redirect("~/Default.aspx", true);
                    }
                    else
                    {
                        string SSOURL = string.Format(url + "?msg={0}",
                            "User Role for user " + UserId + " not present in VigilanceMIS");

                        Response.Redirect(SSOURL);
                    }

                }
            }
            catch (Exception ex)
            {
                //VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }
        protected void aspLogin_Authenticate(object sender, AuthenticateEventArgs e)
        {
            try
            {
                String strPassword = Login1.Password;
                String strUniqueID = hidUniqueID.Value;

                String extractPass = strPassword.Substring(4, ((strPassword.Length - 4) - strUniqueID.Length));
                int startIndex = strPassword.Length - strUniqueID.Length;

                String getUniqueid = strPassword.Substring(startIndex, 36);

                String realPass = "";

                int k = 0;
                for (int j = 0; j < extractPass.Length; j++)
                {
                    if (k == 0)
                    {
                        realPass = realPass + extractPass[j];
                        k = 1;
                    }
                    else if (k == 1)
                    {
                        k = 2;
                    }
                    else if (k == 2)
                    {
                        k = 0;
                    }
                }

                if (getUniqueid == strUniqueID)
                {
                    if (Membership.ValidateUser(Login1.UserName, realPass))
                    {
                        e.Authenticated = true;
                    }
                    else
                    {
                        e.Authenticated = false;
                    }
                }
                else
                {
                    e.Authenticated = false;
                }

            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
                ((Literal)Login1.FindControl("FailureText")).Text = ((Literal)Login1.FindControl("FailureText")).Text + " " + ex.Message;
                return;
            }
        }

        protected void Login1_LoggedIn(object sender, EventArgs e)
        {
            try
            {
                MembershipUser mu = Membership.GetUser(Login1.UserName);
                DateTime passworddate = mu.LastPasswordChangedDate;
                DateTime d1 = DateTime.Now;
                TimeSpan t = d1 - passworddate;
                double NrOfDays = t.TotalDays;
                WebProfile wp = WebProfile.GetProfile(Login1.UserName);
                Session["solname"] = wp.solname;
                Session["nameofuser"] = wp.nameofuser;
                Session["changepwd"] = wp.changepwd;
                Session["userid"] = Login1.UserName;
                Session["sol"] = Convert.ToInt64(wp.sol).ToString("0000");
                Session["solid"] = wp.sol + "00";
                Session["hosol"] = WebConfigurationManager.AppSettings["hosol"].ToString();

                //funcUpdateUserTrace();      //Update User Trace Log

                if (NrOfDays >= 90)
                {
                    wp.changepwd = "0";
                    Response.Redirect("~/changePwd.aspx", true);
                    return;
                }

                if (Cache["VMISessionId" + Session["userid"].ToString()] == null)
                {
                    Cache.Add("VMISessionId" + Session["userid"].ToString(), Session.SessionID, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["CacheTimeOut"])), System.Web.Caching.CacheItemPriority.NotRemovable, (System.Web.Caching.CacheItemRemovedCallback)null);
                }
                else if (hiddenforcelogin.Value == "1")
                {
                    Cache["VMISessionId" + Session["userid"].ToString()] = Session.SessionID;
                }
                if (wp.changepwd == "1")
                {
                    Response.Redirect("~/changePwd.aspx", true);
                    return;
                }
            }
            catch (Exception exLogin1)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(exLogin1);
            }
        }

        protected void Login1_LoggingIn(object sender, LoginCancelEventArgs e)
        {
            try
            {
                String GUID = Guid.NewGuid().ToString();
                Session["AuthToken"] = GUID;
                Response.Cookies.Add(new HttpCookie("AuthToken", GUID));

                if (Cache["VMISessionId" + Login1.UserName] != null && hiddenforcelogin.Value == "0")
                {
                    ((Literal)Login1.FindControl("FailureText")).Text = "Already Logged in! Click OK to Force Login";
                    Cache.Add("VMISessionId" + Login1.UserName, Session.SessionID, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10), System.Web.Caching.CacheItemPriority.High, (System.Web.Caching.CacheItemRemovedCallback)null);
                    e.Cancel = true;
                    hiddenforcelogin.Value = "1";
                    return;
                }

                String[] roles = Roles.GetRolesForUser(Login1.UserName);
                IEnumerable<string> qry = from info_role in roles where info_role.Contains("VMIS_") select info_role;
                if (qry.Count() == 1)
                    Session["role"] = qry.First();
                else
                {
                    Session["role"] = "";
                }
                if (Session["role"].ToString() == "")
                {
                    ((Literal)Login1.FindControl("FailureText")).Text = "Invalid Username/Password";
                    e.Cancel = true;
                }
            }
            catch (Exception exLogin1)
            {
                ((Label)Login1.FindControl("resetlabel")).Text = exLogin1.Message;
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(exLogin1);
            }
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            string MAILIP = WebConfigurationManager.AppSettings["MAIL_IP"].ToString();

            ((Label)Login1.FindControl("lblLocked")).Text = "";
            ((Label)Login1.FindControl("lblLocked")).Visible = false;

            SmtpClient sc = new SmtpClient(MAILIP, 25);
            try
            {
                MembershipUser mu = Membership.GetUser(Login1.UserName);
                if (mu != null)
                {
                    WebProfile wp = WebProfile.GetProfile(Login1.UserName);
                    wp.changepwd = "1";
                    wp.Save();

                    if (mu.IsLockedOut) mu.UnlockUser();
                    string newpassword = mu.ResetPassword();


                    sc.Send("itdsw@pnb.co.in", mu.Email, "Vigilance MIS Portal : New Password for User: " + Login1.UserName, "Your password has been reset\n\nNew Password: " + newpassword);
                    ((Label)Login1.FindControl("resetlabel")).Text = "New Password sent to your email ( " + mu.Email + " ).";
                    Membership.UpdateUser(mu);
                }
                else
                {
                    ((Label)Login1.FindControl("resetlabel")).Text = "User doesn't exist";
                }
            }
            catch (Exception exResetButton)
            {
                ((Label)Login1.FindControl("resetlabel")).Text = exResetButton.Message;
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(exResetButton);
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            ((Literal)Login1.FindControl("FailureText")).Visible = true;
            ((Label)Login1.FindControl("lblLocked")).Visible = false;
            try
            {
                MembershipUser mu = Membership.GetUser(Login1.UserName);
                if (mu != null)
                {
                    if (mu.IsLockedOut)
                    {
                        ((Literal)Login1.FindControl("FailureText")).Text = "";
                        ((Literal)Login1.FindControl("FailureText")).Visible = false;
                        ((Label)Login1.FindControl("lblLocked")).Visible = true;
                    }
                }
            }
            catch (Exception exLoginButton)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(exLoginButton);
            }
        }

        //protected void funcUpdateUserTrace()
        //{
        //    HttpContext ctxObject = HttpContext.Current;
        //    string strUserIP = (ctxObject.Request.UserHostAddress != null) ? ctxObject.Request.UserHostAddress : String.Empty;

        //    string strLogin = Session["userid"].ToString();
        //    string strUserName = Session["nameofuser"].ToString();
        //    string strSolInfo = Session["solid"].ToString() + "  " + Session["solname"].ToString();
        //    string strUserRole = Session["role"].ToString();

        //    if (strLogin != "" && strUserRole != "")
        //    {
        //        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
        //        SqlCommand cmd = new SqlCommand();

        //        try
        //        {
        //            con.Open();
        //            cmd.Connection = con;
        //            string strUniqueID = "";

        //            Random oRandomID = new Random();
        //            string Alphabet = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        //            for (int i = 0; i < 10; i++)
        //            {
        //                strUniqueID = strUniqueID + Alphabet[oRandomID.Next(Alphabet.Length)];
        //            }

        //            strUniqueID = strUniqueID + DateTime.Now.ToString("_ddMMyy_hhmmssfff") + "_" + strLogin;
        //            Session["VMISP_TRACE_ID"] = strUniqueID;

        //            cmd.CommandText = "";
        //            cmd.Parameters.Clear();
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "[dbo].[spUserTrace_Update]";
        //            cmd.Parameters.AddWithValue("@p_UNIQUEID", strUniqueID);
        //            cmd.Parameters.AddWithValue("@p_USERIP", strUserIP);
        //            cmd.Parameters.AddWithValue("@p_LOGINID", strLogin);
        //            cmd.Parameters.AddWithValue("@p_USERNAME", strUserName);
        //            cmd.Parameters.AddWithValue("@p_SOLINFO", strSolInfo);
        //            cmd.Parameters.AddWithValue("@p_USERROLE", strUserRole);
        //            cmd.ExecuteNonQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //    }
        //}

        private bool funcDeactivateAllSessions()
        {
            bool result = false;
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "";
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spUserConcurrent_Deactivate]";
                cmd.Parameters.AddWithValue("@p_UserID", Convert.ToString(Session["userid"]));
                if (cmd.ExecuteNonQuery() > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                logger.Info("Exception under funcDeactivateAllSessions: " + ex.ToString());
            }
            finally
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
            return result;
        }

        private bool funcInsertCurrentSession()
        {
            bool result = false;
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "";
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spUserConcurrent_InsertSession]";
                cmd.Parameters.AddWithValue("@p_UserID", Convert.ToString(Session["userid"]));
                cmd.Parameters.AddWithValue("@p_AuthToken", Convert.ToString(Session["AuthToken"]));
                if (cmd.ExecuteNonQuery() > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                logger.Info("Exception under funcInsertCurrentSession: " + ex.ToString());
            }
            finally
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
            return result;
        }
        protected void funcUpdateUserTrace(string STATUS, string MSG, string TOKEN)
        {
            HttpContext ctxObject = HttpContext.Current;
            string strUserIP = (ctxObject.Request.UserHostAddress != null) ? ctxObject.Request.UserHostAddress : String.Empty;
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                con.Open();
                cmd.Connection = con;
                string UNIQUEID = "";
                string UserID = "";


                UserID = Convert.ToString(Session["userid"]);
                Random oRandomID = new Random();
                string Alphabet = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                for (int i = 0; i < 10; i++)
                {
                    UNIQUEID = UNIQUEID + Alphabet[oRandomID.Next(Alphabet.Length)];
                }

                UNIQUEID = UNIQUEID + DateTime.Now.ToString("_ddMMyy_hhmmssfff") + "_" + Convert.ToString(Session["userid"]);
                Session["VMISP_TRACE_ID"] = UNIQUEID;
                cmd.CommandText = "";
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spUserTrace_Updatenew]";
                cmd.Parameters.AddWithValue("@p_UNIQUEID", UNIQUEID);
                cmd.Parameters.AddWithValue("@p_USERIP", strUserIP);
                cmd.Parameters.AddWithValue("@p_LOGINID", UserID);
                cmd.Parameters.AddWithValue("@p_USERNAME", Convert.ToString(Session["nameofuser"]));
                cmd.Parameters.AddWithValue("@p_SOLINFO", Session["solid"].ToString() + "  " + Session["solname"].ToString());
                cmd.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));
                cmd.Parameters.AddWithValue("@p_STATUS", STATUS);
                cmd.Parameters.AddWithValue("@p_MESSAGE", MSG);
                cmd.Parameters.AddWithValue("@p_TOKEN", TOKEN);
                if (cmd.ExecuteNonQuery() > 0) { }
                else
                { return; }
            }
            catch (Exception ex)
            {
                logger.Info("Exception under funcUpdateUserTrace: " + ex.ToString());
            }
            finally
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
        }
        private string funcValidateSingleUserLogin(string UserID)
        {

            SqlConnection connection = null;
            SqlCommand command = new SqlCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                connection.Open();

                command.CommandText = @"exec spValidateSingleUserLogin @LoginDate,@LOGINID";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("LoginDate", System.DateTime.Now.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("LOGINID", UserID);

                command.Connection = connection;
                sqlDataAdapter.SelectCommand = command;

                sqlDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["STATUS"].ToString() == "LOGIN SUCCESS")
                        {
                            return dr["AuthToken"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Exception under funcValidateSingleUserLogin: " + ex.ToString());
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }

            return null;
        }

        private string funcTerminateUserAlreadyLogin(string OldUserAuthToken)
        {
            HttpContext ctxObject = HttpContext.Current;
            string strUserIP = (ctxObject.Request.UserHostAddress != null) ? ctxObject.Request.UserHostAddress : String.Empty;
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            string Result = string.Empty;
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spUserLoginTrace_Operation]";
                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(sqlErrMsgOutput);
                cmd.Parameters.Add(sqlErrCodeOutput);

                cmd.Parameters.AddWithValue("@p_USERIP", strUserIP);
                cmd.Parameters.AddWithValue("@p_LOGINID", Convert.ToString(Session["userid"]));
                cmd.Parameters.AddWithValue("@p_USERNAME", Convert.ToString(Session["nameofuser"]));
                cmd.Parameters.AddWithValue("@p_SOLINFO", Convert.ToString(Session["sol"]));
                cmd.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));
                cmd.Parameters.AddWithValue("@p_STATUS", "SESSION TIMEOUT");
                cmd.Parameters.AddWithValue("@p_ERRORMSG", "User session has been Terminated");
                cmd.Parameters.AddWithValue("@p_AUTHTOKEN", OldUserAuthToken);


                cmd.CommandTimeout = 0;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    Result = Convert.ToString(sqlErrMsgOutput.Value);
                }
            }
            catch (Exception ex)
            {
                logger.Info("Exception under funcTerminateUserAlreadyLogin: " + ex.ToString());
            }
            finally
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
            return Result;
        }
    }
}
