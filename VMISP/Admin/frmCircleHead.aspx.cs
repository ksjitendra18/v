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
    public partial class frmCircleHead : System.Web.UI.Page
    {
        #region ** declare Variable **
        string strMode = string.Empty;
        string strErrMsg = string.Empty;
        int intErrCode = 0;

        string strCIRCLE = string.Empty;
        string strSOLID = string.Empty;
        string strDNO = string.Empty;
        string strCIRCLEHEAD = string.Empty;

        CommonFunction objCommonFunction = new CommonFunction();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ddlCircleOffice.Focus();

            #region ** JS Function **
            btnSave.Attributes.Add("onclick", "return funcCircleHead_Validation('" + ddlCircleOffice.ClientID + "','" + txtCircleHeadName.ClientID + "')");
            #endregion
        }

        public void funcClear()
        {
            txtCircleHeadName.Text = string.Empty;
            btnUpdate.Visible = false;
            btnSave.Visible = true;
            ddlCircleOffice.Focus();
        }

        public void funcSave(string p_strMode)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                strCIRCLE = objCommonFunction.ddlSelectedValue(ddlCircleOffice);
                strCIRCLEHEAD = txtCircleHeadName.Text.Trim();
                strDNO = txtCircleHeadName.ToolTip.Trim();

                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spCircleHead_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(sqlErrMsgOutput);
                cmd.Parameters.Add(sqlErrCodeOutput);

                cmd.Parameters.AddWithValue("@p_MODE", p_strMode);
                cmd.Parameters.AddWithValue("@p_SOLID", strCIRCLE);
                cmd.Parameters.AddWithValue("@p_DNO", strDNO);
                cmd.Parameters.AddWithValue("@p_CIRCLEHEAD", strCIRCLEHEAD);
                cmd.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["userid"]));

                if (cmd.ExecuteNonQuery() > 0)
                {
                    lblMsg.Text = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                    gvMain.DataBind();
                    funcClear();
                }
                else
                {
                    lblMsg.Text = "Error - Update Circle Head.";
                }
            }
            catch (Exception es)
            {
                lblMsg.Text = es.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }
            finally
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            funcSave("I");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            funcSave("U");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            funcClear();
            lblMsg.Text = "";
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strCircleCode = string.Empty;
            string strCircleName = string.Empty;
            string strCircleDNo = string.Empty;
            string strCircleHead = string.Empty;
            lblMsg.Text = string.Empty;

            try
            {
                if (e.CommandName.ToUpper() == "VIEW")
                {
                    string strCommandArgument = e.CommandArgument.ToString();
                    string[] strValue = strCommandArgument.Split('~');
                    strCircleCode = strValue[0];
                    strCircleName = strValue[1];
                    strCircleDNo = strValue[2];
                    strCircleHead = strValue[3];
                    if (strCircleCode != "")
                    {
                        objCommonFunction.ddlSetDataValue(ddlCircleOffice, strCircleCode);
                        txtCircleHeadName.Text = strCircleHead;
                        txtCircleHeadName.ToolTip = strCircleDNo;
                        ViewState["SOLID"] = strCircleCode;
                        btnUpdate.Visible = true;
                        btnSave.Visible = false;
                        txtCircleHeadName.Focus();
                    }
                }
            }
            catch (Exception eg)
            {
                eg.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(eg);
            }
        }
    }
}