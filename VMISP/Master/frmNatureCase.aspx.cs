using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;

namespace VMISP.Master
{
    public partial class frmNatureCase : System.Web.UI.Page
    {
        #region ** declare Variable **
        string strMode = string.Empty;
        string strMsg = string.Empty;
        string strSearchNo = string.Empty;
        string strErrMsg = string.Empty;
        string strUser = string.Empty;
        int intErrCode = 0;

        string strNATURECODE = string.Empty;
        string strTABLE = string.Empty;
        string strNATURECASE = string.Empty;
        string strACTIVE = string.Empty;
        CommonFunction objCommonFunction = new CommonFunction();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                funcShow("LIST", strTABLE, "LIST"); //for bind grid view on form Load
            }

            txtNatureCode.Focus();
            lblMsg.Text = string.Empty;

            #region ** JS Function  **
            imgGet.Attributes.Add("onclick", "return funcSearchMaster_Validation('" + ddlTable.ClientID + "','" + txtNatureCode.ClientID + "','" + "Select Form Name...!" + "','" + "Enter Code of Nature Case...!" + "')");
            btnSubmit.Attributes.Add("onclick", "return funcNatureCaseMaster_Validation('" + txtNatureCode.ClientID + "','" + ddlTable.ClientID + "','" + txtNatureCase.ClientID + "')");
            btnUpdate.Attributes.Add("onclick", "return funcNatureCaseMaster_Validation('" + txtNatureCode.ClientID + "','" + ddlTable.ClientID + "','" + txtNatureCase.ClientID + "')");
            #endregion
        }

        public void funcSave(string p_strMode)
        {
            SqlConnection conSave = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();

            try
            {
                strNATURECODE = txtNatureCode.Text.Trim();
                strTABLE = objCommonFunction.ddlSelectedValue(ddlTable);
                strNATURECASE = txtNatureCase.Text;
                strACTIVE = objCommonFunction.chkSelected(chkActive);
                strUser = ViewState["USERNAME"].ToString();

                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spNatureCase_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_NATURECODE", strNATURECODE);
                cmdSave.Parameters.AddWithValue("@p_TABLE", strTABLE);
                cmdSave.Parameters.AddWithValue("@p_NATURECASE", strNATURECASE);
                cmdSave.Parameters.AddWithValue("@p_ACTIVE", strACTIVE);
                cmdSave.Parameters.AddWithValue("@p_MODE", @p_strMode);
                cmdSave.Parameters.AddWithValue("@p_USER", strUser);

                cmdSave.ExecuteNonQuery();
                cmdSave.CommandTimeout = 0;

                strErrMsg = sqlErrMsgOutput.Value.ToString();
                intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                ViewState["PROCMSG"] = strErrMsg.ToString();
                ViewState["ERRCODE"] = intErrCode.ToString();

                funcShow("LIST", strTABLE, "LIST"); //for bind grid view on form Load
            }
            catch (Exception es)
            {
                lblMsg.Text = es.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }
            finally
            {
                cmdSave.Dispose();
                conSave.Dispose();
                conSave.Close();
            }
        }

        public void funcShow(string p_strNo, string strValue, string strView)
        {
            SqlConnection conView = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            DataTable dt = new DataTable();
            SqlCommand cmdView = new SqlCommand();
            try
            {
                conView.Open();
                cmdView.Connection = conView;
                cmdView.Parameters.Clear();
                cmdView.CommandType = CommandType.StoredProcedure;
                cmdView.CommandText = "[dbo].[spNatureCase_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_TABLE", strValue);

                cmdView.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmdView);
                sda.Fill(dt);
                ViewState["DETAILDATA"] = dt;

                strErrMsg = sqlErrMsgOutput.Value.ToString();
                intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                if (intErrCode >= 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (strView.ToUpper() == "LIST")
                        {
                            pnlHeader.Visible = false;
                            gvMain.DataSource = dt;
                            gvMain.DataBind();
                        }
                        else if (strView.ToUpper() == "SEARCH")
                        {
                            pnlHeader.Visible = false;
                            gvMain.DataSource = dt;
                            gvMain.DataBind();
                            tabMain.ActiveTabIndex = 1;
                        }
                        else if (strView.ToUpper() == "GET")
                        {
                            funcBindControl(dt);
                        }
                    }
                }

                else
                {
                    lblMsg.Text = strErrMsg.ToString();
                    funcClear();
                }
            }

            catch (Exception es)
            {
                es.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }
            finally
            {
                cmdView.Dispose();
                conView.Dispose();
                conView.Close();
            }
        }

        public void funcBindControl(DataTable dt)
        {
            DataTable dtData = dt;
            tabMain.ActiveTabIndex = 0;
            pnlHeader.Visible = true;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;

            txtNatureCode.Text = dtData.Rows[0]["CODE"].ToString();
            objCommonFunction.ddlSetDataValue(ddlTable, dtData.Rows[0]["FORTABLE"].ToString());
            txtNatureCase.Text = dtData.Rows[0]["NATURECASE"].ToString();
            objCommonFunction.chkSetData(chkActive, dtData.Rows[0]["ACTIVE"].ToString());
            lblEntryBy.Text = dtData.Rows[0]["ENTRYBY"].ToString();
            lblEntryDate.Text = dtData.Rows[0]["ENTRYDATE"].ToString();
            lblModifyBy.Text = dtData.Rows[0]["MODIFYBY"].ToString();
            lblModifyDate.Text = dtData.Rows[0]["MODIFYDATE"].ToString();
        }

        public void funcClear()
        {
            txtNatureCode.Text = string.Empty;
            txtNatureCase.Text = string.Empty;
            ddlTable.SelectedIndex = 0;
            chkActive.Checked = false;
            lblEntryBy.Text = string.Empty;
            lblEntryDate.Text = string.Empty;
            lblModifyBy.Text = string.Empty;
            lblModifyDate.Text = string.Empty;

            pnlHeader.Visible = false;
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            strMode = "I";
            try
            {
                funcSave(strMode);
                lblMsg.Text = ViewState["PROCMSG"].ToString();

                if (ViewState["ERRCODE"].ToString() == "1")
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            strMode = "U";
            try
            {
                funcSave(strMode);
                lblMsg.Text = ViewState["PROCMSG"].ToString();

                if (ViewState["ERRCODE"].ToString() == "2")
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            funcClear();
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            strSearchNo = txtNatureCode.Text.Trim();
            strTABLE = objCommonFunction.ddlSelectedValue(ddlTable);
            funcShow(strSearchNo, strTABLE, "GET");
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void imgSearch_LIST_Click(object sender, ImageClickEventArgs e)
        {
            strSearchNo = txtNatureCode_LIST.Text.Trim();
            if (strSearchNo == "")
            {
                strSearchNo = "LIST";
            }
            funcShow(strSearchNo, strTABLE, "SEARCH");
            lblList.Text = strErrMsg.ToString();
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "VIEW")
                {
                    string strCommandArgument = e.CommandArgument.ToString();
                    string[] strValue = strCommandArgument.Split(',');
                    strNATURECODE = strValue[0];
                    strTABLE = strValue[1];
                    if (strNATURECODE != "")
                    {
                        funcShow(strNATURECODE, strTABLE, "GET");
                    }
                }
            }
            catch (Exception eg)
            {
                eg.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(eg);
            }
        }

        protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMain.PageIndex = e.NewPageIndex;

            DataTable dtPaging = ((DataTable)ViewState["DETAILDATA"]);
            gvMain.DataSource = dtPaging;
            gvMain.DataBind();
        }

        protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtSorting = ((DataTable)ViewState["DETAILDATA"]);
            dtSorting.DefaultView.Sort = e.SortExpression;
            gvMain.DataSource = dtSorting;
            gvMain.DataBind();
        }

        protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
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