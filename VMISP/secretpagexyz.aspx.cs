using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Profile;
using System.Data;
using System.Web.Security;

namespace VMISP
{
    public partial class secretpagexyz : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Roles.CreateRole("VMIS_ADMIN");
            Roles.CreateRole("VMIS_DESKUSER");
            Roles.CreateRole("VMIS_MISUSER");
            Roles.CreateRole("VMIS_SUPERUSER");
            Roles.CreateRole("VMIS_VIEWUSER");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Roles.AddUserToRole("99999", "VMIS_SUPERUSER");
           /*MembershipUser mu = Membership.CreateUser("99999","pnbxyz123");
            Roles.AddUserToRole("340920", "VMIS_MISUSER");
            ----------------------------------USERS FOR ho----
            SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCE"].ConnectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = "Select DNO,Branch_name from BRANCH_MASTER where br_type in ('CO','ZAO','HO')";
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                MembershipUser mu1 = Membership.GetUser(sdr["DNO"].ToString() + "00");
                if (mu1 == null)
                {

                    MembershipUser mu = Membership.CreateUser(sdr["DNO"].ToString()+"00", "pnb123", "ruchi.radha@pnb.co.in");
                    ProfileBase p = WebProfile.Create(mu.ToString());
                    p.SetPropertyValue("sol", sdr["DNO"].ToString());
                    p.SetPropertyValue("solname", sdr["Branch_name"].ToString());
                    p.SetPropertyValue("nameofuser", sdr["Branch_name"].ToString());
                    p.Save();

                    string[] roles = Roles.GetRolesForUser(mu.ToString());
                    IEnumerable<string> qry = from info_role in roles where info_role.Contains("VIG_") select info_role;

                    if (qry.Count() == 1)
                    {
                        Roles.RemoveUserFromRole(mu.ToString(), qry.First());
                        Roles.AddUserToRole(mu.ToString(), "VIG_COMMON");
                    }
                    else 
                    {
                        Roles.AddUserToRole(mu.ToString(), "VIG_COMMON");
                    }
                }
                else 
                {
                    WebProfile p = WebProfile.GetProfile(mu1.ToString());
                    if (p == null)
                    {
                        ProfileBase p1 = WebProfile.Create(mu1.ToString());
                        p = WebProfile.GetProfile(mu1.ToString());
                    }
                    else
                    { 
                            p.GetPropertyValue("sol");
                            p.SetPropertyValue("sol", sdr["DNO"].ToString());

                            p.GetPropertyValue("solname");
                            p.SetPropertyValue("solname", sdr["Branch_name"].ToString());


                            p.GetPropertyValue("nameofuser");
                            p.SetPropertyValue("nameofuser", sdr["Branch_name"].ToString());

                            p.Save();
                            string[] roles = Roles.GetRolesForUser(mu1.ToString());
                            IEnumerable<string> qry = from info_role in roles where info_role.Contains("VIG_") select info_role;

                            if (qry.Count() == 1)
                            {
                                Roles.RemoveUserFromRole(mu1.ToString(), qry.First());
                                Roles.AddUserToRole(mu1.ToString(), "VIG_COMMON");
                            }
                            else
                            {
                                Roles.AddUserToRole(mu1.ToString(), "VIG_COMMON");                                
                            }
                    }
                    //Roles.AddUserToRole(mu1.ToString(), "VIG_COMMON");
                }
            }

             Code to remove VIG_COMMON role from Branches//

            SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCE"].ConnectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = "Select DNO,Branch_name from BRANCH_MASTER where br_type in ('BO')";
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                MembershipUser mu1 = Membership.GetUser(sdr["DNO"].ToString() + "00");
                WebProfile p = WebProfile.GetProfile(mu1.ToString());

                p.GetPropertyValue("sol");
                p.SetPropertyValue("sol", sdr["DNO"].ToString());

                p.GetPropertyValue("solname");
                p.SetPropertyValue("solname", sdr["Branch_name"].ToString());


                p.GetPropertyValue("nameofuser");
                p.SetPropertyValue("nameofuser", sdr["Branch_name"].ToString());

                p.Save();
                string[] roles = Roles.GetRolesForUser(mu1.ToString());
                IEnumerable<string> qry = from info_role in roles where info_role.Contains("VIG_") select info_role;

                if (qry.Count() == 1)
                {
                    Roles.RemoveUserFromRole(mu1.ToString(), qry.First());
                    Roles.AddUserToRole(mu1.ToString(), "VIG_COMMON");
                }
            }*/
        }
    }
}

