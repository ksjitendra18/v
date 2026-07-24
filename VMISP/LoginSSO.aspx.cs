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

namespace VMISP
{
    public partial class LoginSSO : System.Web.UI.Page
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {

            logger.Info("Login: Inside Page Load");

            logger.Info("Key received from SSO initiator");
            logger.Info(Request.Form.Count);
            foreach (var item in Request.Form.AllKeys)
            {
                logger.Info(item);
                logger.Info(Request.Form[item]);
            }

            if (Request.Form["userid"] != null && Request.Form["userid"] != string.Empty)
            {
                 string userName = Request.Form["userid"];
                //logger.Info("Login: Inside Page Load");
                //Response.Redirect("~/Default.aspx");
                try
                {
                    MembershipUser mu = Membership.GetUser(userName);
                    logger.Info("line 1");
                    //DateTime passworddate = mu.LastPasswordChangedDate;
                    logger.Info("line 2");
                    DateTime d1 = DateTime.Now;
                    logger.Info("line 3");
                    //TimeSpan t = d1 - passworddate;
                    logger.Info("line 4");
                    //double NrOfDays = t.TotalDays;
                    logger.Info("line 5");
                    WebProfile wp = WebProfile.GetProfile(userName);
                    logger.Info("line 6");
                    Session["solname"] = wp.solname;
                    logger.Info("line 7");
                    Session["nameofuser"] = wp.nameofuser;
                    logger.Info("line 8");
                    Session["changepwd"] = wp.changepwd;
                    logger.Info("line 9");
                    Session["userid"] = userName;
                    logger.Info("line 10");
                    //Session["sol"] = Convert.ToInt64(wp.sol).ToString("0000");
                    logger.Info("line 11");
                    Session["solid"] = wp.sol + "00";
                    logger.Info("line 12");
                    Session["hosol"] = WebConfigurationManager.AppSettings["hosol"].ToString();
                    logger.Info("line 13");

                    //funcUpdateUserTrace();      //Update User Trace Log
                    logger.Info("line 14");
                    //if (NrOfDays >= 90)
                    //{
                    //    logger.Info("line 15");
                    //    wp.changepwd = "0";
                    //    logger.Info("line 16");
                    //    Response.Redirect("~/changePwd.aspx", true);
                    //    logger.Info("line 17");
                    //    return;
                    //}

                    //if (Cache["VMISessionId" + Session["userid"].ToString()] == null)
                    //{
                    //    logger.Info("line 18");
                    //    Cache.Add("VMISessionId" + Session["userid"].ToString(), Session.SessionID, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["CacheTimeOut"])), System.Web.Caching.CacheItemPriority.NotRemovable, (System.Web.Caching.CacheItemRemovedCallback)null);
                    //}
                    //else if (hiddenforcelogin.Value == "1")
                    //{
                    //    logger.Info("line 19");
                    //    Cache["VMISessionId" + Session["userid"].ToString()] = Session.SessionID;
                    //}
                    //Response.Redirect("~/changePwd.aspx", true);
                    Response.Redirect("~/Default.aspx", true);
                    //if (wp.changepwd == "1")
                    //{
                    //    logger.Info("line 20");
                    //    Response.Redirect("~/changePwd.aspx", true);
                    //    return;
                    //}
                }
                catch (Exception exLogin1)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(exLogin1);
                }
            }

            TextBox txtUserName = (TextBox)Login1.FindControl("UserName");
            txtUserName.Attributes.Add("autocomplete", "off");
            TextBox txtPassword = (TextBox)Login1.FindControl("Password");
            txtPassword.Attributes.Add("autocomplete", "off");
        }

        //protected void aspLogin_Authenticate(object sender, AuthenticateEventArgs e)
        //{
        //    try
        //    {
        //        String strPassword = Login1.Password;
        //        String strUniqueID = hidUniqueID.Value;

        //        String extractPass = strPassword.Substring(4, ((strPassword.Length - 4) - strUniqueID.Length));
        //        int startIndex = strPassword.Length - strUniqueID.Length;

        //        String getUniqueid = strPassword.Substring(startIndex, 36);

        //        String realPass = "";

        //        int k = 0;
        //        for (int j = 0; j < extractPass.Length; j++)
        //        {
        //            if (k == 0)
        //            {
        //                realPass = realPass + extractPass[j];
        //                k = 1;
        //            }
        //            else if (k == 1)
        //            {
        //                k = 2;
        //            }
        //            else if (k == 2)
        //            {
        //                k = 0;
        //            }
        //        }

        //        if (getUniqueid == strUniqueID)
        //        {
        //            if (Membership.ValidateUser(Login1.UserName, realPass))
        //            {
        //                e.Authenticated = true;
        //            }
        //            else
        //            {
        //                e.Authenticated = false;
        //            }
        //        }
        //        else
        //        {
        //            e.Authenticated = false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
        //        ((Literal)Login1.FindControl("FailureText")).Text = ((Literal)Login1.FindControl("FailureText")).Text + " " + ex.Message;
        //        return;
        //    }

        //}

        //protected void Login1_LoggedIn(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        MembershipUser mu = Membership.GetUser(Login1.UserName);
        //        DateTime passworddate = mu.LastPasswordChangedDate;
        //        DateTime d1 = DateTime.Now;
        //        TimeSpan t = d1 - passworddate;
        //        double NrOfDays = t.TotalDays;
        //        WebProfile wp = WebProfile.GetProfile(Login1.UserName);
        //        Session["solname"] = wp.solname;
        //        Session["nameofuser"] = wp.nameofuser;
        //        Session["changepwd"] = wp.changepwd;
        //        Session["userid"] = Login1.UserName;
        //        Session["sol"] = Convert.ToInt64(wp.sol).ToString("0000");
        //        Session["solid"] = wp.sol + "00";
        //        Session["hosol"] = WebConfigurationManager.AppSettings["hosol"].ToString();

        //        funcUpdateUserTrace();      //Update User Trace Log

        //        if (NrOfDays >= 90)
        //        {
        //            wp.changepwd = "0";
        //            Response.Redirect("~/changePwd.aspx", true);
        //            return;
        //        }

        //        if (Cache["VMISessionId" + Session["userid"].ToString()] == null)
        //        {
        //            Cache.Add("VMISessionId" + Session["userid"].ToString(), Session.SessionID, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["CacheTimeOut"])), System.Web.Caching.CacheItemPriority.NotRemovable, (System.Web.Caching.CacheItemRemovedCallback)null);
        //        }
        //        else if (hiddenforcelogin.Value == "1")
        //        {
        //            Cache["VMISessionId" + Session["userid"].ToString()] = Session.SessionID;
        //        }
        //        if (wp.changepwd == "1")
        //        {
        //            Response.Redirect("~/changePwd.aspx", true);
        //            return;
        //        }
        //    }
        //    catch (Exception exLogin1)
        //    {
        //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(exLogin1);
        //    }
        //}

        //protected void Login1_LoggingIn(object sender, LoginCancelEventArgs e)
        //{
        //    try
        //    {
        //        String GUID = Guid.NewGuid().ToString();
        //        Session["AuthToken"] = GUID;
        //        Response.Cookies.Add(new HttpCookie("AuthToken", GUID));

        //        if (Cache["VMISessionId" + Login1.UserName] != null && hiddenforcelogin.Value == "0")
        //        {
        //            ((Literal)Login1.FindControl("FailureText")).Text = "Already Logged in! Click OK to Force Login";
        //            Cache.Add("VMISessionId" + Login1.UserName, Session.SessionID, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10), System.Web.Caching.CacheItemPriority.High, (System.Web.Caching.CacheItemRemovedCallback)null);
        //            e.Cancel = true;
        //            hiddenforcelogin.Value = "1";
        //            return;
        //        }

        //        String[] roles = Roles.GetRolesForUser(Login1.UserName);
        //        IEnumerable<string> qry = from info_role in roles where info_role.Contains("VMIS_") select info_role;
        //        if (qry.Count() == 1)
        //            Session["role"] = qry.First();
        //        else
        //        {
        //            Session["role"] = "";
        //        }
        //        if (Session["role"].ToString() == "")
        //        {
        //            ((Literal)Login1.FindControl("FailureText")).Text = "Invalid Username/Password";
        //            e.Cancel = true;
        //        }
        //    }
        //    catch (Exception exLogin1)
        //    {
        //        ((Label)Login1.FindControl("resetlabel")).Text = exLogin1.Message;
        //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(exLogin1);
        //    }
        //}

        //protected void ResetButton_Click(object sender, EventArgs e)
        //{
        //    string MAILIP = WebConfigurationManager.AppSettings["MAIL_IP"].ToString();

        //    ((Label)Login1.FindControl("lblLocked")).Text = "";
        //    ((Label)Login1.FindControl("lblLocked")).Visible = false;

        //    SmtpClient sc = new SmtpClient(MAILIP, 25);
        //    try
        //    {
        //        MembershipUser mu = Membership.GetUser(Login1.UserName);
        //        if (mu != null)
        //        {
        //            WebProfile wp = WebProfile.GetProfile(Login1.UserName);
        //            wp.changepwd = "1";
        //            wp.Save();
        //            if (mu.IsLockedOut) mu.UnlockUser();
        //            sc.Send("itdsw@pnb.co.in", mu.Email, "Vigilance MIS Portal : New Password for User: " + Login1.UserName, "Your password has been reset\n\nNew Password: " + mu.ResetPassword());
        //            ((Label)Login1.FindControl("resetlabel")).Text = "New Password sent to your email ( " + mu.Email + " ).";
        //            Membership.UpdateUser(mu);
        //        }
        //        else
        //        {
        //            ((Label)Login1.FindControl("resetlabel")).Text = "User doesn't exist";
        //        }
        //    }
        //    catch (Exception exResetButton)
        //    {
        //        ((Label)Login1.FindControl("resetlabel")).Text = exResetButton.Message;
        //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(exResetButton);
        //    }
        //}

        //protected void LoginButton_Click(object sender, EventArgs e)
        //{
        //    ((Literal)Login1.FindControl("FailureText")).Visible = true;
        //    ((Label)Login1.FindControl("lblLocked")).Visible = false;
        //    try
        //    {
        //        MembershipUser mu = Membership.GetUser(Login1.UserName);
        //        if (mu != null)
        //        {
        //            if (mu.IsLockedOut)
        //            {
        //                ((Literal)Login1.FindControl("FailureText")).Text = "";
        //                ((Literal)Login1.FindControl("FailureText")).Visible = false;
        //                ((Label)Login1.FindControl("lblLocked")).Visible = true;
        //            }
        //        }
        //    }
        //    catch (Exception exLoginButton)
        //    {
        //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(exLoginButton);
        //    }
        //}

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
    }
}
