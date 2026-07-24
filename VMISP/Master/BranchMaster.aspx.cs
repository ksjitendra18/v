using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace VMISP.Master
{
    public partial class BranchMaster : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (objCommonFunction.funcCheckUserRights("BRANCH_MASTER") == false)
                {
                    Response.Redirect("~/Logout.aspx");
                }

                funBindDropDown();

                Label lblMenuName = (Label)Master.FindControl("lblMenuName");
                Label lblPageName = (Label)Master.FindControl("lblPageName");

                lblMenuName.Text = "Master Management";
                lblPageName.Text = "Branch Master";
            }
        }

        public void funBindDropDown()
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            ds.Tables.Clear();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spBranchMaster_Ddl]";

                cmd.Parameters.AddWithValue("@p_TYPE", "DEFAULT");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_USERID", Convert.ToString(Session["USERID"]));
                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlCircle, ds.Tables[0]);
                    objCommonFunction.bindDropdownList(ddlState, ds.Tables[1]);
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

        private void funcClear()
        {
            ddlState.SelectedIndex = 0;
            txtSolID.ToolTip = "";
            txtSolID.Text = "";
            txtBranchName.Text = "";
            txtBranchAddress.Text = "";
            txtEmailID.Text = "";

            lnkSubmit.Visible = true;
            lnkUpdate.Visible = false;
        }

        private void funcShowDetails(string VIEW, string UNIQUEID)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dt.Clear();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spBranchMaster_View]";

                cmd.Parameters.AddWithValue("@p_USERID", Convert.ToString(Session["USERID"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));

                cmd.Parameters.AddWithValue("@p_VIEW", VIEW);
                cmd.Parameters.AddWithValue("@p_UNIQUEID", UNIQUEID);
                cmd.Parameters.AddWithValue("@p_CIRCLE", objCommonFunction.ddlSelectedValue(ddlCircle));
                cmd.Parameters.AddWithValue("@p_BRN_SOLID", txtSolidSearch.Text.Trim());

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
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();
            con.Open();
            cmdSave.Connection = con;
            cmdSave.Parameters.Clear();

            try
            {
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spBranchMaster_Operation]";

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
                cmdSave.Parameters.AddWithValue("@p_CIRCLESOLID", objCommonFunction.ddlSelectedValue(ddlCircle));
                cmdSave.Parameters.AddWithValue("@p_BRN_SOLID", txtSolID.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BRN_NAME", txtBranchName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BRN_ADDRESS", txtBranchAddress.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BRN_STATE", objCommonFunction.ddlSelectedText(ddlState));
                cmdSave.Parameters.AddWithValue("@p_BRN_EMAILID", txtEmailID.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ACTIVE", objCommonFunction.ddlSelectedValue(ddlActive));

                cmdSave.CommandTimeout = 0;

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    funcClear();    //Reset all controls.
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

                    lblMsg.Text = "Error- Insert/Update Branch Master Details...!";
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

        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            if (funcValidation() == true)
            {
                funcSubmit("I", "0");
            }
        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            if (funcValidation() == true)
            {
                funcSubmit("U", txtSolID.ToolTip);
            }
        }

        protected void lnkReset_Click(object sender, EventArgs e)
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
                    txtSolID.ToolTip = Data[0];
                    objCommonFunction.ddlSetDataValue(ddlCircle, Data[1]);
                    txtSolID.Text = Data[2];
                    txtBranchName.Text = Data[3];
                    txtBranchAddress.Text = Data[4];
                    objCommonFunction.ddlSetData(ddlState, Data[5], true);
                    objCommonFunction.ddlSetDataValue(ddlActive, Data[6]);
                    txtEmailID.Text = Data[7];
                    lnkSubmit.Visible = false;
                    lnkUpdate.Visible = true;
                }
                else
                {
                    lblMsg.Text = "Branch Code is null...";
                    lblMsg.CssClass = "label label-danger";

                    lnkSubmit.Visible = true;
                    lnkUpdate.Visible = false;
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

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlCircle)))
            {
                lblMsg.Text = "Please Select circle from Dropdown.";
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtSolID.Text.Trim()))
            {
                lblMsg.Text = "Please enter Solid of Branch";
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtBranchName.Text.Trim()))
            {
                lblMsg.Text = "Please enter branch name";
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtBranchAddress.Text.Trim()))
            {
                lblMsg.Text = "Please enter branch address";
                return Result = false;
            }
            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlState)))
            {
                lblMsg.Text = "Please Select state from Dropdown.";
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtEmailID.Text.Trim()))
            {
                lblMsg.Text = "Please enter branch email id";
                return Result = false;
            }

            return Result;
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlCircle)))
            {
                funcShowDetails("LIST", objCommonFunction.ddlSelectedValue(ddlCircle));
            }
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
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