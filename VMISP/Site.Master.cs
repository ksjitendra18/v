using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace VMISP
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Cache-Control", "no-cache"); //HTTP 1.1            
            Response.AddHeader("Cache-Control", "no-store"); // HTTP 1.1
            Response.AddHeader("Cache-Control", "must-revalidate"); // HTTP 
            Response.AddHeader("Pragma", "no-cache"); // HTTP 1.1 
            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Label1.Text = Convert.ToString(Session["solname"]);
                    Label2.Text = Convert.ToString(Session["nameofuser"]);

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
                String abc = Request.Cookies["ASP.NET_SessionId"].ToString();
                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                Session.RemoveAll();
                Session.Abandon();
                Session.Clear();

                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    Response.Cookies["ASP.NET_SessionId"].Value = String.Empty;
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                }
                if (Request.Cookies["AuthToken"] != null)
                {
                    Response.Cookies["AuthToken"].Value = String.Empty;
                    Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                }

                Response.Redirect("~/Login.aspx", true);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["userid"] == null)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            if (Session["changepwd"].ToString() == "1")
            {
                Response.Redirect("~/changePwd.aspx");

            }

            if (Cache["VMISessionId" + Session["userid"].ToString()] != null)
            {
                if (Cache["VMISessionId" + Session["userid"].ToString()].ToString() != Session.SessionID)
                {
                    Response.Redirect("~/Login.aspx");
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
                String strUniqueID = Session["VMISP_TRACE_ID"].ToString();

                if (strUniqueID != "")
                {
                    cmd.CommandText = "UPDATE USER_TRACE SET LOGOUTTIME=GETDATE() WHERE UNIQUEID='" + strUniqueID + "'";
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        FormsAuthentication.SignOut();
                        Session.Abandon();
                        Cache.Remove("VMISessionId" + Session["userid"].ToString());
                        String abc = Request.Cookies["ASP.NET_SessionId"].ToString();
                        Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                        Session.RemoveAll();
                        Session.Abandon();
                        Session.Clear();

                        if (Request.Cookies["ASP.NET_SessionId"] != null)
                        {
                            Response.Cookies["ASP.NET_SessionId"].Value = String.Empty;
                            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                        }
                        if (Request.Cookies["AuthToken"] != null)
                        {
                            Response.Cookies["AuthToken"].Value = String.Empty;
                            Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                        }

                        Response.Redirect("~/Login.aspx", true);
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

    }
}
