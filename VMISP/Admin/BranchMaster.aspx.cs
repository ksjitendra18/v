using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;

namespace VMISP.Admin
{
    public partial class BranchMaster : System.Web.UI.Page
    {
        #region ** declare Variable **
        string strMode = string.Empty;
        string strErrMsg = string.Empty;
        string strUser = string.Empty;
        int intErrCode = 0;

        string strCIRCLE = string.Empty;
        string strSOLID = string.Empty;
        string strBRANCH = string.Empty;
        string strTYPE = string.Empty;
        string strBRANCHTYPE = string.Empty;
        string strBRPARENTCODE = string.Empty;
        string strACTIVE = string.Empty;

        CommonFunction objCommonFunction = new CommonFunction();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ddlCircleOffice.Focus();

            #region ** JS Function **
            btnSave.Attributes.Add("onclick", "return funcBranchMaster_Validation('" + ddlCircleOffice.ClientID + "','" + ddlBranchType.ClientID + "','" + txtSolID.ClientID + "','" + txtBranchName.ClientID + "','" + ddlType.ClientID + "')");
            #endregion
        }

        public void funcClear()
        {
            txtBranchName.Text = string.Empty;
            txtSolID.Text = string.Empty;
            btnUpdate.Visible = false;
            btnSave.Visible = true;
            btnDelete.Visible = false;
            txtSolID.ReadOnly = false;
            ddlCircleOffice.Focus();
            chkActive.Checked = false;
        }

        public void funcSave(string p_strMode)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                #region ** assign Control Value **
                strCIRCLE = objCommonFunction.ddlSelectedValue(ddlCircleOffice);
                strBRPARENTCODE = ddlCircleOffice.SelectedValue.Substring(0, 4).ToString();
                if (p_strMode.ToUpper() == "U")
                {
                    strSOLID = ViewState["SOLID"].ToString();
                }
                else
                {
                    strSOLID = txtSolID.Text.Trim();
                }
                strBRANCH = txtBranchName.Text;
                strTYPE = objCommonFunction.ddlSelectedText(ddlType);
                strBRANCHTYPE = objCommonFunction.ddlSelectedText(ddlBranchType);
                strUser = Session["userid"].ToString();
                strACTIVE = objCommonFunction.chkSelected(chkActive);
                #endregion

                #region ** call StoredProcedure to Save/Update data in Table  **
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spBranchMaster_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(sqlErrMsgOutput);
                cmd.Parameters.Add(sqlErrCodeOutput);

                cmd.Parameters.AddWithValue("@p_BR_PARENT_CODE", strBRPARENTCODE);
                cmd.Parameters.AddWithValue("@p_BR_PARENT_CODE2", strCIRCLE);
                cmd.Parameters.AddWithValue("@p_SOLID", strSOLID);
                cmd.Parameters.AddWithValue("@p_BRANCHNAME", strBRANCH);
                cmd.Parameters.AddWithValue("@p_TYPE", strTYPE);
                cmd.Parameters.AddWithValue("@p_BRANCHTYPE", strBRANCHTYPE);
                cmd.Parameters.AddWithValue("@p_MODIFIED_BY", strUser);
                cmd.Parameters.AddWithValue("@p_MODE", p_strMode);
                cmd.Parameters.AddWithValue("@p_USER", strUser);
                cmd.Parameters.AddWithValue("@p_ACTIVE", strACTIVE);

                cmd.ExecuteNonQuery();
                cmd.CommandTimeout = 0;
                gvBranch.DataBind();

                strErrMsg = sqlErrMsgOutput.Value.ToString();
                intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                #endregion
            }
            catch (Exception es)
            {
                lblMsg.Text = es.ToString();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                strMode = "I";
                funcSave(strMode);
                lblMsg.Text = strErrMsg.ToString();

                if (intErrCode == 1)
                {
                    funcClear();
                }
            }

            catch (Exception e1)
            {
                lblMsg.Text = e1.Message;
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e1);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            strMode = "U";
            try
            {
                funcSave(strMode);
                lblMsg.Text = strErrMsg.ToString();

                if (intErrCode == 2)
                {
                    funcClear();
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = " UPDATE [BRANCH_MASTER] SET ACTIVE='N' WHERE Branch_name=@Branch_name and SOLID=@SOLID ";
                cmd.Parameters.AddWithValue("SOLID", txtSolID.Text.ToString());
                cmd.Parameters.AddWithValue("Branch_name", txtBranchName.Text.ToString());
                cmd.Parameters.AddWithValue("Br_type", ddlBranchType.SelectedItem.Value.ToString());
                cmd.ExecuteNonQuery();

                gvBranch.DataBind();
                lblMsg.Visible = true;
                lblMsg.Text = "Branch Deleted Successfully";
                lblMsg.ForeColor = System.Drawing.Color.Green;
                funcClear();

            }

            catch (Exception e1)
            {
                lblMsg.Visible = true;
                lblMsg.Text = e1.Message;
                lblMsg.ForeColor = System.Drawing.Color.Red;
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e1);
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            funcClear();
        }

        protected void gvBranch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strBranchCode = string.Empty;
            string strBranchName = string.Empty;
            string strCircleCode = string.Empty;
            string strCheck = string.Empty;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            btnSave.Visible = false;
            lblMsg.Text = string.Empty;

            try
            {
                if (e.CommandName.ToUpper() == "VIEW")
                {
                    string strCommandArgument = e.CommandArgument.ToString();
                    string[] strValue = strCommandArgument.Split('~');
                    strBranchCode = strValue[0];
                    strBranchName = strValue[1];
                    strCircleCode = strValue[2];
                    strCheck = strValue[3];

                    if (strBranchCode != "")
                    {
                        objCommonFunction.ddlSetDataValue(ddlCircleOffice, strCircleCode);
                        txtSolID.Text = strBranchCode;
                        ViewState["SOLID"] = strBranchCode;
                        txtBranchName.Text = strBranchName;
                        objCommonFunction.chkSetData(chkActive, strCheck);
                        txtSolID.ReadOnly = true;
                        txtBranchName.Focus();
                    }
                }
            }
            catch (Exception eg)
            {
                eg.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(eg);
            }
        }

        protected void gvBranch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover",
                "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#20B2AA';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            }
        }

    }
}