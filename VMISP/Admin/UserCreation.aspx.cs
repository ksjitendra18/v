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
                ShowCheckerScope(true);
                BindModuleGroups();
                BindZones();
                LoadCheckerScope(TxtPF.Text);
            }
            else
            {
                ShowCheckerScope(false);
            }
        }

        /// <summary>
        /// The checker scope is a module group x zone grant, so both lists appear and
        /// disappear together -- one without the other grants nothing.
        /// </summary>
        private void ShowCheckerScope(bool visible)
        {
            trCheckerScope.Visible = visible;
            trCheckerZones.Visible = visible;
        }

        /// <summary>
        /// Binds the module groups a checker can be granted. The group, not the module, is
        /// what is granted: 'Vigilance and IAC' is one tick, 'Complaint and MISC' another.
        /// spCheckerGroup_Ddl returns the member modules so the admin can see what a tick
        /// actually covers without knowing the module registry.
        /// </summary>
        private void BindModuleGroups()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[spCheckerGroup_Ddl]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                }
            }

            chkModuleGroups.Items.Clear();

            foreach (DataRow row in dt.Rows)
            {
                string groupCode = Convert.ToString(row["GroupCode"]);
                string groupName = Convert.ToString(row["GroupName"]);
                string modules = Convert.ToString(row["Modules"]);

                string text = string.IsNullOrEmpty(modules)
                    ? groupName
                    : groupName + "  (" + modules + ")";

                chkModuleGroups.Items.Add(new ListItem(text, groupCode));
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

                            if (!SaveCheckerScope(TxtPF.Text))
                                return;   // message already on LblResponse
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

                            if (!SaveCheckerScope(TxtPF.Text))
                                return;   // message already on LblResponse
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



        /// <summary>
        /// Saves the checker's scope as the ticked groups x ticked zones.
        ///
        /// Replaces the old per-zone "SELECT COUNT(*) then UPDATE-or-INSERT" loop, which was
        /// a race -- two concurrent saves produced duplicate mappings, and a duplicate mapping
        /// duplicated every row in that checker's inbox. spCheckerScope_Save does the whole
        /// thing as one MERGE, and revokes anything no longer ticked.
        /// </summary>
        /// <returns>false if the selection is incomplete or the save failed; message in LblResponse.</returns>
        private bool SaveCheckerScope(string userPF)
        {
            MembershipUser mu = Membership.GetUser(userPF);

            if (mu == null)
                return false;

            Guid userId = (Guid)mu.ProviderUserKey;

            string groups = string.Join(",", chkModuleGroups.Items
                                                .Cast<ListItem>()
                                                .Where(i => i.Selected)
                                                .Select(i => i.Value));

            string zones = string.Join(",", chkZones.Items
                                               .Cast<ListItem>()
                                               .Where(i => i.Selected)
                                               .Select(i => i.Value));

            // A group with no zone, or a zone with no group, grants nothing at all. Saying so
            // is better than saving a checker who will open an empty inbox and not know why.
            if (string.IsNullOrEmpty(groups) || string.IsNullOrEmpty(zones))
            {
                LblResponse.Text = "Select at least one module group and at least one zone for this checker.";
                LblResponse.CssClass = "errorString";
                return false;
            }

            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[spCheckerScope_Save]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_USERPF", userPF);
                    cmd.Parameters.AddWithValue("@p_USERID", userId);
                    cmd.Parameters.AddWithValue("@p_GROUPS", groups);
                    cmd.Parameters.AddWithValue("@p_ZONES", zones);
                    cmd.Parameters.AddWithValue("@p_CREATEDBY", User.Identity.Name);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    if (Convert.ToInt32(sqlErrCodeOutput.Value) != 1)
                    {
                        LblResponse.Text = Convert.ToString(sqlErrMsgOutput.Value);
                        LblResponse.CssClass = "errorString";
                        return false;
                    }
                }
            }

            return true;
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
                        ShowCheckerScope(true);
                        BindModuleGroups();
                        BindZones();
                        LoadCheckerScope(TxtPF.Text);
                    }
                    else
                    {
                        ShowCheckerScope(false);
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

        /// <summary>
        /// Ticks the groups and zones this checker currently holds, so the screen opens
        /// showing what is actually granted rather than an empty form.
        /// </summary>
        private void LoadCheckerScope(string userPF)
        {
            HashSet<string> groups = new HashSet<string>();
            HashSet<string> zones = new HashSet<string>();

            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[spCheckerScope_Get]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_USERPF", userPF);

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            groups.Add(Convert.ToString(dr["GroupCode"]));
                            zones.Add(Convert.ToString(dr["ZoneSolID"]));
                        }
                    }
                }
            }

            foreach (ListItem item in chkModuleGroups.Items)
            {
                item.Selected = groups.Contains(item.Value);
            }

            foreach (ListItem item in chkZones.Items)
            {
                item.Selected = zones.Contains(item.Value);
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