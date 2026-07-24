using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using VMISP.DataAccessLayer;

namespace VMISP.Master
{
    public partial class ZoneChiefManager : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();
        MasterData objMasterData = new MasterData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (objCommonFunction.funcCheckUserRights("ZONE_CHIEF_MANAGER") == false)
                {
                    Response.Redirect("~/Logout.aspx");
                }

                objMasterData.funcZoneMaster(ddlZone, "ZO");
            }
        }

        private void funcClear()
        {
            ddlType.SelectedIndex = 0;
            txtCMName.Text = "";
            txtRemarks.Text = "";
            txtCMName.ToolTip = "";
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
        }

        private void funcShowDetails(string VIEW, string UNIQUEID)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dt.Clear();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spZoneChiefManager_View]";

                cmd.Parameters.AddWithValue("@p_USERID", Convert.ToString(Session["USERID"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));

                cmd.Parameters.AddWithValue("@p_VIEW", VIEW);
                cmd.Parameters.AddWithValue("@p_UNIQUEID", UNIQUEID);
                cmd.Parameters.AddWithValue("@p_ZONE", objCommonFunction.ddlSelectedValue(ddlZone));
                cmd.Parameters.AddWithValue("@p_ZONE_SOLID", txtSolidSearch.Text.Trim());

                cmd.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    gvMain.DataSource = dt;
                    gvMain.DataBind();
                }
                else
                {
                    funcClear();
                    gvMain.DataSource = null;
                    gvMain.DataBind();
                }
            }

            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }

            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        private void funcSubmit(string MODE, string UPDATEID)
        {
            lblMsg.Text = "";
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();
            con.Open();
            cmdSave.Connection = con;
            cmdSave.Parameters.Clear();

            try
            {
                if (MODE.Equals("I"))
                {
                    string ID = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                    UPDATEID = "ZCM" + DateTime.Now.ToString("ddMMyy") + ID;
                }
                
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spZoneChiefManager_Operation]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["USERID"]));
                cmdSave.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmdSave.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());

                cmdSave.Parameters.AddWithValue("@p_UNIQUEID", UPDATEID);
                cmdSave.Parameters.AddWithValue("@p_ZONE_SOLID", objCommonFunction.ddlSelectedValue(ddlZone));
                cmdSave.Parameters.AddWithValue("@p_ZONE_TYPE", objCommonFunction.ddlSelectedValue(ddlType));
                cmdSave.Parameters.AddWithValue("@p_CM_NAME", txtCMName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_REMARKS", txtRemarks.Text.Trim());

                cmdSave.CommandTimeout = 0;

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    funcClear();                    //Reset all controls.
                    funcShowDetails("LIST", null); //Update grid data.
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
                }
                else
                {
                    if (sqlErrCodeOutput.Value.Equals(2))
                    {
                        lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
                        lblMsg.CssClass = "label label-danger";
                    }

                    lblMsg.Text = "Error- Insert/Update Zone Chief Manager Master Details...!";
                    lblMsg.CssClass = "label label-danger";
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmdSave.Dispose();
                con.Close();
                con.Dispose();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (funcValidation() == true)
            {
                funcSubmit("I", "0");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (funcValidation() == true)
            {
                funcSubmit("U", txtCMName.ToolTip);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            funcClear();
            lblMsg.Text = "";
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblMsg.Text = "";

            if (e.CommandName.Equals("SELECT"))
            {
                string Value = e.CommandArgument.ToString();

                if (!string.IsNullOrEmpty(Value))
                {
                    string[] Data = Value.Split('~');
                    txtCMName.ToolTip = Data[0];
                    objCommonFunction.ddlSetDataValue(ddlZone, Data[1]);
                    objCommonFunction.ddlSetDataValue(ddlType, Data[2]);
                    txtCMName.Text = Data[3];
                    txtRemarks.Text = Data[4];
                    btnSubmit.Visible = false;
                    btnUpdate.Visible = true;
                }
                else
                {
                    lblMsg.Text = "Zone Code is null...";
                    lblMsg.CssClass = "label label-danger";

                    btnSubmit.Visible = true;
                    btnUpdate.Visible = false;
                }
            }
            else
            {
                lblMsg.Text = "Invalid Selection....";
                lblMsg.CssClass = "label label-danger";
            }
        }

        private Boolean funcValidation()
        {
            Boolean Result = true;
            lblMsg.Text = "";

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlZone)))
            {
                lblMsg.Text = "Please Select Zone from Dropdown.";
                return Result = false;
            }

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlType)))
            {
                lblMsg.Text = "Please Select Type from Dropdown.";
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtCMName.Text.Trim()))
            {
                lblMsg.Text = "Please enter Chief Manager name of Zone";
                return Result = false;
            }

            return Result;
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlZone)))
            {
                funcShowDetails("LIST", objCommonFunction.ddlSelectedValue(ddlZone));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (!string.IsNullOrEmpty(txtSolidSearch.Text.Trim()))
            {
                funcShowDetails("SEARCH", txtSolidSearch.Text.Trim());
            }
            else
            {
                gvMain.DataSource = null;
                gvMain.DataBind();
                lblMsg.Text = "Record not found";
            }
        }
    }
}