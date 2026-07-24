using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace VMISP
{
    public partial class frmChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //   ChangePassword1.NewPasswordRegularExpression = "^(?=.*[a-z])(?=.*[0-9])$";
            //((?!" +   Session["userid"].ToString() + ").)*"  +  "
        }

        public static string GenerateHash(string pwd, string saltAsBase64)
        {
            byte[] p1 = Convert.FromBase64String(saltAsBase64);
            return GenerateHash(pwd, p1);
        }

        public static string GenerateHash(string pwd, byte[] saltAsByteArray)
        {
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] p1 = saltAsByteArray;
            byte[] p2 = System.Text.Encoding.Unicode.GetBytes(pwd);
            byte[] data = new byte[p1.Length + p2.Length];
            p1.CopyTo(data, 0);
            p2.CopyTo(data, p1.Length);
            byte[] result = sha.ComputeHash(data);
            string res = Convert.ToBase64String(result);
            return res;
        }

        protected void ChangePassword1_ContinueButtonClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx", false);
        }

        protected void ChangePassword1_ChangedPassword(object sender, EventArgs e)
        {
            WebProfile wp = WebProfile.GetProfile(Session["userid"].ToString());
            wp.changepwd = "0";
            Session["changepwd"] = "0";
            wp.Save();
        }

        protected void ChangePassword1_CancelButtonClick(object sender, EventArgs e)
        {
            try
            {
                WebProfile wp = WebProfile.GetProfile(Session["userid"].ToString());
                if (wp.changepwd == "0")
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                else
                {
                    Session.Abandon();
                    Cache.Remove("VMISessionId" + Session["userid"].ToString());
                    Response.Redirect("~/Login.aspx", true);
                }
            }
            catch
            {
                Session.Abandon();
                Cache.Remove("VMISessionId" + Session["userid"].ToString());
                Response.Redirect("~/Login.aspx", true);
            }
        }

        protected void ChangePassword1_ChangingPassword(object sender, LoginCancelEventArgs e)
        {

            bool cancel_operation = false;
            string failure_txt = "";
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["AuthDB"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select TOP 5 Password,Salt from PWD_LOG where UserId=@userid order by ChangedTime desc");
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("userid", Session["userid"].ToString());
            SqlDataReader sdr = cmd.ExecuteReader();
            bool last5passwords = false;
            while (sdr.Read())
            {
                string salt = sdr["Salt"].ToString();
                string pwd = sdr["Password"].ToString();
                string newhash = GenerateHash(ChangePassword1.NewPassword, salt);
                if (newhash == pwd) { last5passwords = true; break; }
            }

            sdr.Close();
            conn.Close();
            
            if (last5passwords)
            {
                failure_txt += "One of the Last 5 passwords<br/>";
                cancel_operation = true;
            }


            if (ChangePassword1.NewPassword.ToLower().Contains(Session["userid"].ToString().ToLower()))
            {
                failure_txt += "Password contains User-ID<br/>";
                cancel_operation = true;
            }
            Match abc = Regex.Match(ChangePassword1.NewPassword, "[a-zA-Z]");
            if (!abc.Success)
            {
                failure_txt += "Password has no alphabet<br/>";
                cancel_operation = true;
            }
            Match abc1 = Regex.Match(ChangePassword1.NewPassword, "[^a-zA-Z0-9]");
            if (!abc1.Success)
            {
                failure_txt += "Password has no Spl.Chars<br/>";
                cancel_operation = true;
            }
            Match abc2 = Regex.Match(ChangePassword1.NewPassword, "[0-9]");
            if (!abc2.Success)
            {
                failure_txt += "Password has no number<br/>";
                cancel_operation = true;
            }
            if (ChangePassword1.NewPassword.Length < 6)
            {
                failure_txt += "Password Length &lt; 6<br/>";
                cancel_operation = true;
            }
            if (cancel_operation)
            {
                ((Literal)ChangePassword1.ChangePasswordTemplateContainer.FindControl("FailureText")).Text = failure_txt;
                e.Cancel = true;
            }

        }

    }
}
