using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Security;
using System.Web.SessionState;

namespace VMISP
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest()
        {
            //HttpContext.Current.Response.AddHeader("x-frame-options", "DENY");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");              //Remove Server Header   
            Response.Headers.Remove("X-AspNet-Version");    //Remove X-AspNet-Version Header
            Response.Headers.Remove("X-AspNetMVC-Version"); //Remove X-AspNetMVC-Version Header            
            Response.Headers.Remove("X-Powered-By");        //Remove X-Powered-By Header    
            //Response.AppendHeader("X-XSS-Protection", "0");

           var httpCookie = new HttpCookie("mycookie", "myvalue");
            string CookiePath = ConfigurationManager.AppSettings["UserDefinedCookiePathFilter"];
            Response.Cookies.Add(new HttpCookie("mycookie")
            {
                HttpOnly = true,
                Value = "myvalue",
                Path = CookiePath + ";SameSite=Strict",
                Secure = true
            });
            //httpCookie.Path += ";SameSite=Strict";
            httpCookie.Path += CookiePath + ";SameSite=Strict";
            httpCookie.Secure = true;
            httpCookie.HttpOnly = true;
            Response.SetCookie(httpCookie);

        }
    }
}