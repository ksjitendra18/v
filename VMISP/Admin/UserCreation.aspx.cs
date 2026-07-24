using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.Profile;
using System.Net.Mail;
using System.Data;

namespace VMISP
{
    public partial class UserCreation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region ** JS Function **
            btnSearch.Attributes.Add("onclick", "return funcUserCreationSearch_Validation('" + TxtPF.ClientID + "')");
            TxtEmail.Attributes.Add("onblur", "return funcEMailValidate('" + TxtEmail.ClientID + "')");
            BtnSubmit.Attributes.Add("onclick", "return funcUserCreation_Validation('" + TxtPF.ClientID + "','" + TxtName.ClientID + "','" + TxtEmail.ClientID + "','" + DDPOP.ClientID + "','" + DDLocation.ClientID + "')");
            #endregion
        }


        protected void DDLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLocation.SelectedValue == "VMIS_CHECKER")
            {
                chkZones.Visible = true;
                BindZones();
            }
            else
            {
                chkZones.Visible = false;
            }
        }

        private void BindZones()
        {
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[spCircleMaster_Ddl]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_TYPE", "DEFAULT");
                    cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                    cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                    cmd.Parameters.AddWithValue("@p_USERID", Convert.ToString(Session["USERID"]));

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                    sda.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataColumn col in ds.Tables[0].Columns)
                        {
                            System.Diagnostics.Debug.WriteLine(col.ColumnName);
                        }
                    }

                    //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    //{
                    //    ddlZone.DataSource = ds.Tables[0];
                    //    ddlZone.DataTextField = "NAME";   // adjust as per SP output
                    //    ddlZone.DataValueField = "CODE"; // adjust as per SP output
                    //    ddlZone.DataBind();

                    //    ddlZone.Items.Insert(0, new ListItem("Select Zone", ""));
                    //}

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        chkZones.DataSource = ds.Tables[0];
                        chkZones.DataTextField = "NAME";
                        chkZones.DataValueField = "CODE";
                        chkZones.DataBind();
                    }
                }
            }
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

        private void AssignRole(string userName, string role)
        {
            if (!Roles.RoleExists(role))
                return;

            // Secondary role -> simply add if missing
            if (SecondaryRoles.Contains(role))
            {
                if (!Roles.IsUserInRole(userName, role))
                    Roles.AddUserToRole(userName, role);

                return;
            }

            // Primary role -> remove other primary roles only
            foreach (string primaryRole in PrimaryRoles)
            {
                if (primaryRole != role && Roles.IsUserInRole(userName, primaryRole))
                {
                    Roles.RemoveUserFromRole(userName, primaryRole);
                }
            }

            if (!Roles.IsUserInRole(userName, role))
                Roles.AddUserToRole(userName, role);
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (TxtPF.Text == "99999")
            {
                LblResponse.Text = "This number is restricted for creating the user";
                return;
            }

            MembershipUser mu = Membership.GetUser(TxtPF.Text);
            if (mu == null)
            {
                try
                {
                    MembershipUser MUser = Membership.CreateUser(TxtPF.Text, "pnb123", TxtEmail.Text);
                    ProfileBase p = WebProfile.Create(TxtPF.Text);

                    //if (Session["role"].ToString() == "VMIS_SUPERUSER")
                    //{
                    p.SetPropertyValue("sol", DDPOP.SelectedValue.ToString());
                    p.SetPropertyValue("solname", DDPOP.SelectedItem.Text);
                    p.SetPropertyValue("nameofuser", TxtName.Text);
                    p.Save();

                    if (DDLocation.SelectedIndex != 0)
                    {
                        //Roles.AddUserToRole(TxtPF.Text, DDLocation.SelectedValue.ToString());
                        AssignRole(TxtPF.Text, DDLocation.SelectedValue);

                        if (DDLocation.SelectedValue == "VMIS_CHECKER")
                        {
                            AssignRole(TxtPF.Text, "VMIS_DESK_USER");

                            SaveMakerCheckerMapping(TxtPF.Text);
                        }
                    }

                    //if (DDLocation.SelectedIndex == 1)
                    //{
                    //    Roles.AddUserToRole(TxtPF.Text, "VMIS_" + DDPOP.SelectedValue.ToString());
                    //}
                    //if (DDLocation.SelectedIndex == 2)
                    //{
                    //    Roles.AddUserToRole(TxtPF.Text, "VMIS_ADMIN");
                    //}
                    //if (DDLocation.SelectedIndex == 3)
                    //{
                    //    Roles.AddUserToRole(TxtPF.Text, "VMIS_USER");
                    //}
                    // }

                    LblResponse.Text = "User created";
                    LblResponse.CssClass = "successString";
                }
                catch (Exception e1)
                {
                    LblResponse.Text = "User creation failed\n" + e1.Message;
                    LblResponse.CssClass = "errorString";
                    Membership.DeleteUser(TxtPF.Text);
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e1);
                }

            }
            else
            {
                try
                {
                    WebProfile p = WebProfile.GetProfile(TxtPF.Text);
                    if (p == null)
                    {
                        ProfileBase p1 = WebProfile.Create(TxtPF.Text);
                        p = WebProfile.GetProfile(TxtPF.Text);

                    }
                    else
                    {
                        //if (Session["role"].ToString() == "VMIS_SUPERUSER")
                        //{
                        p.GetPropertyValue("sol");
                        p.SetPropertyValue("sol", DDPOP.SelectedValue.ToString());

                        p.GetPropertyValue("solname");
                        p.SetPropertyValue("solname", DDPOP.SelectedItem.Text);

                        p.GetPropertyValue("nameofuser");
                        p.SetPropertyValue("nameofuser", TxtName.Text);
                        p.Save();
                        //string[] roles = Roles.GetRolesForUser(TxtPF.Text);
                        //IEnumerable<string> qry = from info_role in roles where info_role.Contains("VMIS_") select info_role;

                        //if (qry.Count() == 1)
                        //{
                        //    if (DDLocation.SelectedIndex != 0)
                        //    {
                        //        Roles.RemoveUserFromRole(TxtPF.Text, qry.First());
                        //        Roles.AddUserToRole(TxtPF.Text, DDLocation.SelectedValue.ToString());
                        //        if (DDLocation.SelectedValue == "VMIS_CHECKER")
                        //        {
                        //            SaveMakerCheckerMapping(TxtPF.Text);
                        //        }
                        //        else
                        //        {
                        //            DeactivateCheckerMappings(TxtPF.Text);

                        //        }
                        //    }
                        //    //if (DDLocation.SelectedIndex == 2)
                        //    //{
                        //    //    Roles.RemoveUserFromRole(TxtPF.Text, qry.First());
                        //    //    Roles.AddUserToRole(TxtPF.Text, "VMIS_ADMIN");
                        //    //}
                        //    //if (DDLocation.SelectedIndex == 3)
                        //    //{
                        //    //    Roles.RemoveUserFromRole(TxtPF.Text, qry.First());
                        //    //    Roles.AddUserToRole(TxtPF.Text, "VMIS_USER");
                        //    //}
                        //}
                        //else
                        //{
                        //    if (DDLocation.SelectedIndex != 0)
                        //    {
                        //        Roles.AddUserToRole(TxtPF.Text, DDLocation.SelectedValue.ToString());
                        //        if (DDLocation.SelectedValue == "VMIS_CHECKER")
                        //        {
                        //            SaveMakerCheckerMapping(TxtPF.Text);
                        //        }
                        //    }

                        //    //if (DDLocation.SelectedIndex == 1)
                        //    //{
                        //    //    Roles.AddUserToRole(TxtPF.Text, "VMIS_" + DDPOP.SelectedValue.ToString());
                        //    //}
                        //    //if (DDLocation.SelectedIndex == 2)
                        //    //{
                        //    //    Roles.AddUserToRole(TxtPF.Text, "VMIS_ADMIN");
                        //    //}
                        //    //if (DDLocation.SelectedIndex == 3)
                        //    //{
                        //    //    Roles.AddUserToRole(TxtPF.Text, "VMIS_USER");
                        //    //}
                        //}
                        ////}
                        ///
                        AssignRole(TxtPF.Text, DDLocation.SelectedValue);

                        if (DDLocation.SelectedValue == "VMIS_CHECKER")
                        {
                            AssignRole(TxtPF.Text, "VMIS_DESK_USER");

                            SaveMakerCheckerMapping(TxtPF.Text);
                        }
                        else
                        {
                            if (Roles.IsUserInRole(TxtPF.Text, "VMIS_CHECKER"))
                            {
                                RemoveSecondaryRole(TxtPF.Text, "VMIS_CHECKER");
                                DeactivateCheckerMappings(TxtPF.Text);
                            }
                        }
                    }
                    mu.Email = TxtEmail.Text;
                    Membership.UpdateUser(mu);
                    LblResponse.Text = "User Updated";
                    LblResponse.CssClass = "successString";
                }
                catch (Exception e1)
                {
                    LblResponse.Text = "User creation failed\n" + e1.Message;
                    LblResponse.CssClass = "errorString";
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e1);
                }
                finally
                {

                }
            }
        }

        private void RemoveSecondaryRole(string userName, string role)
        {
            if (Roles.IsUserInRole(userName, role))
                Roles.RemoveUserFromRole(userName, role);
        }

        //    private void SaveMakerCheckerMapping(string userPF)
        //    {
        //        MembershipUser mu = Membership.GetUser(userPF);

        //        if (mu == null)
        //            return;

        //        Guid userId = (Guid)mu.ProviderUserKey;

        //        using (SqlConnection con = new SqlConnection(
        //            WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
        //        {
        //            con.Open();

        //            // Deactivate old mappings
        //            SqlCommand cmdInsert = new SqlCommand(@"
        //INSERT INTO MakerCheckerMapping
        //(
        //    UserId,
        //    UserPF,
        //    ZoneSolID,
        //    IsMaker,
        //    IsChecker,
        //    IsActive,
        //    CreatedBy
        //)
        //VALUES
        //(
        //    @UserId,
        //    @UserPF,
        //    @ZoneSolID,
        //    0,
        //    1,
        //    1,
        //    @CreatedBy
        //)", con);

        //            cmdInsert.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = userId;
        //            cmdInsert.Parameters.Add("@UserPF", SqlDbType.VarChar).Value = userPF;
        //            cmdInsert.Parameters.Add("@ZoneSolID", SqlDbType.VarChar);
        //            cmdInsert.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = User.Identity.Name;

        //            foreach (ListItem item in chkZones.Items)
        //            {
        //                if (!item.Selected)
        //                    continue;

        //                cmdInsert.Parameters["@ZoneSolID"].Value = item.Value;
        //                cmdInsert.ExecuteNonQuery();
        //            }
        //        }
        //    }



        private void SaveMakerCheckerMapping(string userPF)
        {
            MembershipUser mu = Membership.GetUser(userPF);

            if (mu == null)
                return;

            Guid userId = (Guid)mu.ProviderUserKey;

            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                con.Open();

                // Disable all old mappings
                SqlCommand deactivate = new SqlCommand(@"
            UPDATE MakerCheckerMapping
            SET IsActive = 0
            WHERE UserPF = @UserPF", con);

                deactivate.Parameters.AddWithValue("@UserPF", userPF);
                deactivate.ExecuteNonQuery();

                foreach (ListItem item in chkZones.Items)
                {
                    if (!item.Selected)
                        continue;

                    SqlCommand check = new SqlCommand(@"
                SELECT COUNT(*)
                FROM MakerCheckerMapping
                WHERE UserPF=@UserPF
                  AND ZoneSolID=@Zone", con);

                    check.Parameters.AddWithValue("@UserPF", userPF);
                    check.Parameters.AddWithValue("@Zone", item.Value);

                    int exists = (int)check.ExecuteScalar();

                    if (exists > 0)
                    {
                        SqlCommand update = new SqlCommand(@"
                    UPDATE MakerCheckerMapping
                    SET
                        IsActive=1,
                        IsChecker=1,
                        IsMaker=0
                    WHERE UserPF=@UserPF
                      AND ZoneSolID=@Zone", con);

                        update.Parameters.AddWithValue("@UserPF", userPF);
                        update.Parameters.AddWithValue("@Zone", item.Value);

                        update.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand insert = new SqlCommand(@"
                    INSERT INTO MakerCheckerMapping
                    (
                        UserId,
                        UserPF,
                        ZoneSolID,
                        IsMaker,
                        IsChecker,
                        IsActive,
                        CreatedBy
                    )
                    VALUES
                    (
                        @UserId,
                        @UserPF,
                        @Zone,
                        0,
                        1,
                        1,
                        @CreatedBy
                    )", con);

                        insert.Parameters.AddWithValue("@UserId", userId);
                        insert.Parameters.AddWithValue("@UserPF", userPF);
                        insert.Parameters.AddWithValue("@Zone", item.Value);
                        insert.Parameters.AddWithValue("@CreatedBy", User.Identity.Name);

                        insert.ExecuteNonQuery();
                    }
                }
            }
        }


        private void DeactivateCheckerMappings(string userPF)
        {
            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
            UPDATE MakerCheckerMapping
            SET IsActive = 0
            WHERE UserPF = @UserPF", con);

                cmd.Parameters.AddWithValue("@UserPF", userPF);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        //private void SaveMakerCheckerMapping(string userPF)
        //{
        //    MembershipUser mu = Membership.GetUser(userPF);

        //    if (mu == null)
        //        return;

        //    Guid userId = (Guid)mu.ProviderUserKey;

        //    using (SqlConnection con = new SqlConnection(
        //        WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
        //    {
        //        con.Open();

        //        // Deactivate old mappings
        //        SqlCommand cmdDeactivate = new SqlCommand(@"
        //    UPDATE MakerCheckerMapping
        //    SET IsActive = 0
        //    WHERE UserPF = @UserPF", con);

        //        cmdDeactivate.Parameters.AddWithValue("@UserPF", userPF);
        //        cmdDeactivate.ExecuteNonQuery();

        //        // Insert new mapping
        //        SqlCommand cmdInsert = new SqlCommand(@"
        //    INSERT INTO MakerCheckerMapping
        //    (
        //        UserId,
        //        UserPF,
        //        ZoneSolID,
        //        IsMaker,
        //        IsChecker,
        //        IsActive,
        //        CreatedBy
        //    )
        //    VALUES
        //    (
        //        @UserId,
        //        @UserPF,
        //        @ZoneSolID,
        //        0,
        //        1,
        //        1,
        //        @CreatedBy
        //    )", con);

        //        cmdInsert.Parameters.AddWithValue("@UserId", userId.ToString());
        //        cmdInsert.Parameters.AddWithValue("@UserPF", userPF);
        //        cmdInsert.Parameters.AddWithValue("@ZoneSolID", ddlZone.SelectedValue);
        //        cmdInsert.Parameters.AddWithValue("@CreatedBy", User.Identity.Name);

        //        cmdInsert.ExecuteNonQuery();
        //    }
        //}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            MembershipUser mu = Membership.GetUser(TxtPF.Text);
            if (mu != null)
            {
                WebProfile wp = WebProfile.GetProfile(TxtPF.Text);
                if (wp == null)
                {
                    LblResponse.Text = "Profile has not been available! Please enter the details";
                    return;
                }
                if (wp.nameofuser == "")
                {
                    LblResponse.Text = "Profile has not been available! Please enter the details";
                    return;
                }
                TxtName.Text = wp.nameofuser;
                TxtEmail.Text = mu.Email;

                string[] roles = Roles.GetRolesForUser(TxtPF.Text);

                string primaryRole = roles.FirstOrDefault(r => PrimaryRoles.Contains(r));

                bool isChecker = roles.Contains("VMIS_CHECKER");

                if (!string.IsNullOrEmpty(primaryRole))
                {
                    DDLocation.SelectedValue = primaryRole;

                    if (isChecker)
                    {
                        chkZones.Visible = true;
                        BindZones();
                        LoadCheckerZones(TxtPF.Text);
                    }
                    else
                    {
                        chkZones.Visible = false;
                    }
                }
                //string[] roles = Roles.GetRolesForUser(TxtPF.Text);
                //IEnumerable<string> qry = from info_role in roles where info_role.Contains("VMIS_") select info_role;
                //if (qry.Count() == 1)
                //{
                //    try
                //    {
                //        string POP = qry.First();

                //        DDPOP.DataBind();
                //        DDLocation.DataBind();

                //        DDPOP.SelectedValue = wp.sol;
                //        DDLocation.SelectedValue = POP;

                //        if (POP == "VMIS_CHECKER")
                //        {
                //            chkZones.Visible = true;
                //            BindZones();
                //            LoadCheckerZones(TxtPF.Text);
                //        }
                //        else
                //        {
                //            chkZones.Visible = false;
                //        }
                //    }
                //    catch (Exception es)
                //    {
                //        LblResponse.Text = "User exists at: " + wp.solname;
                //        BtnSubmit.Enabled = true;
                //        btnResetPassword.Enabled = true;
                //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
                //        return;
                //    }
                //}
                //else
                //{
                //    LblResponse.Text = "User exists but roles not assigned for this application!";
                //    LblResponse.CssClass = "errorString";
                //    TxtLL.Text = "";
                //    TxtMobile.Text = "";
                //}
            }

            else
            {
                TxtName.Text = "";
                TxtEmail.Text = "";
                TxtLL.Text = "";
                TxtMobile.Text = "";
                DDPOP.SelectedIndex = -1;
                DDLocation.SelectedIndex = -1;
                LblResponse.Text = "User ID doesn't exist! </br> Please update details to create new User-ID";
            }

            BtnSubmit.Enabled = true;
            btnResetPassword.Enabled = true;
        }

        private void LoadCheckerZones(string userPF)
        {
            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT ZoneSolID
            FROM MakerCheckerMapping
            WHERE UserPF = @UserPF
              AND IsActive = 1", con);

                cmd.Parameters.AddWithValue("@UserPF", userPF);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                HashSet<string> zones = new HashSet<string>();

                while (dr.Read())
                {
                    zones.Add(dr["ZoneSolID"].ToString());
                }

                foreach (ListItem item in chkZones.Items)
                {
                    item.Selected = zones.Contains(item.Value);
                }
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (TxtPF.Text.Trim() != "")
            {
                MembershipUser mu = Membership.GetUser(TxtPF.Text);
                if (mu != null)
                {
                    if (mu.IsLockedOut)
                    {
                        mu.UnlockUser();
                    }
                    string pwd = mu.ResetPassword();
                    mu.ChangePassword(pwd, "pnb123");
                    WebProfile p = WebProfile.GetProfile(TxtPF.Text);
                    p.GetPropertyValue("changepwd");
                    p.SetPropertyValue("changepwd", "1".ToString());
                    p.Save();
                    LblResponse.Text = "Password reset!";
                }
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            string[] roles = Roles.GetRolesForUser(TxtPF.Text);
            IEnumerable<string> qry = from info_role in roles where info_role.Contains("VMIS_") select info_role;
            if (qry.Count() == 1)
            {
                Roles.RemoveUserFromRole(TxtPF.Text, qry.First());
                LblResponse.Text = "Roles for PAYFEE has been revoked for " + TxtPF.Text;
            }
        }

    }
}