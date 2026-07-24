using System;
using System.Web;
using System.Web.Security;
using VMISP.DataAccessLayer;

namespace VMISP
{
    public partial class Logout : System.Web.UI.Page
    {
        MasterData objMasterData = new MasterData();
        protected void Page_Load(object sender, EventArgs e)
        {
            funcLogout();
        }

        private void funcLogout()
        {
            string RESULT = "";
            try
            {
                string UNIQUEID = Convert.ToString(Session["VMISP_TRACE_ID"]);

                RESULT = objMasterData.funcLogout(UNIQUEID); //Call Logout function.

                if (RESULT.Equals("Y"))
                {
                    FormsAuthentication.SignOut();
                    Cache.Remove("VMISessionId" + Session["USERID"].ToString());
                    String abc = Request.Cookies["VMISessionId"].ToString();
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

                    Response.Redirect("~/Login.aspx", true);
                }
                else
                {
                    FormsAuthentication.SignOut();
                    Session.Abandon();
                    Session.RemoveAll();
                    Session.Clear();
                    Response.Redirect("~/Login.aspx", true);
                }
            }
            catch (Exception ex)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                Session.RemoveAll();
                Session.Clear();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
                Response.Redirect("~/Login.aspx", true);
            }
        }
    }
}