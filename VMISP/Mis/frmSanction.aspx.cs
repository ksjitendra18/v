using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace VMISP.Mis
{
    public partial class frmSanction : System.Web.UI.Page
    {
        #region ** declare Variable **
        string strMode = string.Empty;
        string strMsg = string.Empty;
        string strVIEW = string.Empty;
        string strSearchNo = string.Empty;
        string strErrMsg = string.Empty;
        string strUser = string.Empty;
        string strUserRole = string.Empty;
        int intErrCode = 0;

        int intCode = 0;
        string strRCNO = string.Empty;
        string strNAME = string.Empty;
        string strDESIGNATION = string.Empty;
        string strSANCTIONPROSECUTION = string.Empty;
        string strPFNUMBER = string.Empty;
        string strSTATUS = string.Empty;
        string strHOSTATUS = string.Empty;

        DateTime? dtRCDATE = null;
        DateTime? dtRECVDATE = null;
        DateTime? dtSANCTIONREFUSED = null;
        DateTime? dtCVC = null;

        CommonFunction objCommonFunction = new CommonFunction();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();
                funcShow(null, "LIST", null); //for bind grid view on form Load
            }

            txtRCNo.Focus();
            lblMsg.Text = string.Empty;
            funcControlsUserRights();

            #region ** JS Function  **
            txtRCDate.Attributes.Add("readonly", "readonly");
            txtRecvDate.Attributes.Add("readonly", "readonly");
            txtSanctionRefusedDate.Attributes.Add("readonly", "readonly");
            txtCVCDate.Attributes.Add("readonly", "readonly");

            imgGet.Attributes.Add("onclick", "return funcSearch_Validation('" + txtRCNo.ClientID + "','" + "Please Enter RC Number" + "')");
            btnSubmit.Attributes.Add("onclick", "return funcValidation_Sanction('" + txtRCNo.ClientID + "','" + ddlSanctionforProsecution.ClientID + "','" + txtSanctionRefusedDate.ClientID + "','" + txtCVCDate.ClientID + "')");
            btnUpdate.Attributes.Add("onclick", "return funcValidation_Sanction('" + txtRCNo.ClientID + "','" + ddlSanctionforProsecution.ClientID + "','" + txtSanctionRefusedDate.ClientID + "','" + txtCVCDate.ClientID + "')");
            #endregion
        }

        public void funcBindControl(DataTable dt)
        {
            DataTable dtData = dt;
            tabMain.ActiveTabIndex = 0;
            pnlHeader.Visible = true;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;

            //BIND TEXT BOX CONTROL
            txtRCNo.ToolTip = dtData.Rows[0]["CODE"].ToString();
            txtRCNo.Text = dtData.Rows[0]["RCNO"].ToString();

            txtName.Text = dtData.Rows[0]["NAME"].ToString();
            txtDesignation.Text = dtData.Rows[0]["DESIGNATION"].ToString();
            txtPFNumber.Text = dtData.Rows[0]["PFNUMBER"].ToString();
            txtStatus.Text = dtData.Rows[0]["STATUS"].ToString();

            //BIND DATE CONTROLS
            txtRCDate.Text = dtData.Rows[0]["RCDATE"].ToString();
            txtRecvDate.Text = dtData.Rows[0]["RECVDATE"].ToString();
            txtSanctionRefusedDate.Text = dtData.Rows[0]["REFUSEDDATE"].ToString();
            txtCVCDate.Text = dtData.Rows[0]["CVCDATE"].ToString();

            //BIND DROP DOWN CONTROLS
            objCommonFunction.ddlSetDataValue(ddlSanctionforProsecution, dtData.Rows[0]["SANCTIONPROSECUTION"].ToString());
            hidSanctionforProsecution.Value = dtData.Rows[0]["SANCTIONPROSECUTION"].ToString();

            //BIND LABEL CONTROLS
            lblEntryBy.Text = dtData.Rows[0]["ENTRYBY"].ToString();
            lblEntryDate.Text = dtData.Rows[0]["ENTRYDATE"].ToString();
            lblModifyBy.Text = dtData.Rows[0]["MODIFYBY"].ToString();
            lblModifyDate.Text = dtData.Rows[0]["MODIFYDATE"].ToString();
        }

        public void funcClear()
        {
            txtRCNo.ToolTip = string.Empty;
            txtRCNo.Text = string.Empty;
            txtName.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            txtPFNumber.Text = string.Empty;
            txtStatus.Text = string.Empty;

            txtRCDate.Text = string.Empty;
            txtRecvDate.Text = string.Empty;
            txtSanctionRefusedDate.Text = string.Empty;
            txtCVCDate.Text = string.Empty;

            ddlSanctionforProsecution.SelectedIndex = 0;

            pnlHeader.Visible = false;
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;

            funcControlsUserRights();
        }

        public void funcControlsUserRights()
        {
            strUserRole = ViewState["USERROLE"].ToString();

            if (strUserRole.ToUpper() == "VMIS_VIEWUSER")
            {
                funcreadOnly();

                btnSubmit.Visible = false;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;
            }
            else if (strUserRole.ToUpper() == "VMIS_DESKUSER")
            {
                funcreadOnly();

                pnlHOStatus.Visible = true;
                btnSubmit.Visible = false;
                btnUpdate.Visible = true;
                btnCancel.Visible = false;
            }
        }

        public void funcSave(string p_strMode)
        {
            SqlConnection conSave = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();
            try
            {
                #region ** assign Control Value **
                intCode = objCommonFunction.convertToIntToolTip(txtRCNo);
                strRCNO = txtRCNo.Text;

                strNAME = txtName.Text.Trim();
                strDESIGNATION = txtDesignation.Text.Trim();
                strPFNUMBER = txtPFNumber.Text.Trim();
                strSTATUS = txtStatus.Text;
                strHOSTATUS = txtHOStatus.Text;
                strUser = ViewState["USERNAME"].ToString();
                strUserRole = ViewState["USERROLE"].ToString();

                if (strUserRole.ToUpper() == "VMIS_DESKUSER")
                {
                    strSANCTIONPROSECUTION = hidSanctionforProsecution.Value;
                }
                else
                {
                    strSANCTIONPROSECUTION = objCommonFunction.ddlSelectedValue(ddlSanctionforProsecution);
                }

                #region ** convert Date **
                string strRCDATE = txtRCDate.Text.Trim();
                if (!string.IsNullOrEmpty(strRCDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRCDATE, out date))
                        dtRCDATE = date;
                }

                string strRECVDATE = txtRecvDate.Text.Trim();
                if (!string.IsNullOrEmpty(strRECVDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRECVDATE, out date))
                        dtRECVDATE = date;
                }

                string strREFUSEDDATE = txtSanctionRefusedDate.Text.Trim();
                if (!string.IsNullOrEmpty(strREFUSEDDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strREFUSEDDATE, out date))
                        dtSANCTIONREFUSED = date;
                }

                string strCVCDATE = txtCVCDate.Text.Trim();
                if (!string.IsNullOrEmpty(strCVCDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strCVCDATE, out date))
                        dtCVC = date;
                }
                #endregion
                #endregion

                #region ** call StoredProcedure to Save/Update data in Table  **
                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spSanction_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_CODE", intCode);
                cmdSave.Parameters.AddWithValue("@p_RCNO", strRCNO);
                cmdSave.Parameters.AddWithValue("@p_NAME", strNAME);
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", strDESIGNATION);
                cmdSave.Parameters.AddWithValue("@p_PFNUMBER", strPFNUMBER);
                cmdSave.Parameters.AddWithValue("@p_STATUS", strSTATUS);
                cmdSave.Parameters.AddWithValue("@p_HOSTATUS", strHOSTATUS);
                cmdSave.Parameters.AddWithValue("@p_SANCTIONPROSECUTION", strSANCTIONPROSECUTION);
                cmdSave.Parameters.AddWithValue("@p_RCDATE", dtRCDATE);
                cmdSave.Parameters.AddWithValue("@p_RECVDATE", dtRECVDATE);
                cmdSave.Parameters.AddWithValue("@p_SANCTIONREFUSED", dtSANCTIONREFUSED);
                cmdSave.Parameters.AddWithValue("@p_CVCDATE", dtCVC);
                cmdSave.Parameters.AddWithValue("@p_MODE", @p_strMode);
                cmdSave.Parameters.AddWithValue("@p_USER", strUser);
                cmdSave.Parameters.AddWithValue("@p_USERROLE", strUserRole);

                cmdSave.ExecuteNonQuery();
                cmdSave.CommandTimeout = 0;

                strErrMsg = sqlErrMsgOutput.Value.ToString();
                intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                #endregion
            }
            catch (Exception es)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }
            finally
            {
                cmdSave.Dispose();
                conSave.Dispose();
                conSave.Close();
            }
        }

        public void funcShow(string p_strNo, string p_strView, string p_strSTATUS)
        {
            DataTable dt = new DataTable();
            SqlConnection conView = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdView = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmdView);

            try
            {
                #region ** call StoredProcedure to View the Data of RTI  **
                conView.Open();
                cmdView.Connection = conView;
                cmdView.Parameters.Clear();
                cmdView.CommandType = CommandType.StoredProcedure;
                cmdView.CommandText = "[dbo].[spSanction_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_VIEW", p_strView);
                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_STATUS", p_strSTATUS);

                cmdView.CommandTimeout = 0;
                sda.Fill(dt);
                ViewState["DETAILDATA"] = dt;

                strErrMsg = sqlErrMsgOutput.Value.ToString();
                intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                if (intErrCode >= 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (p_strView.ToUpper() == "LIST")
                        {
                            pnlHeader.Visible = false;
                            gvMain.DataSource = dt;
                            gvMain.DataBind();
                        }
                        else if (p_strView.ToUpper() == "SEARCH")
                        {
                            pnlHeader.Visible = false;
                            gvMain.DataSource = dt;
                            gvMain.DataBind();
                            tabMain.ActiveTabIndex = 1;
                        }
                        else if (p_strView.ToUpper() == "GET")
                        {
                            funcBindControl(dt);
                        }
                        else if (p_strView.ToUpper() == "VIEW")
                        {
                            funcBindControl(dt);
                        }
                    }

                    funcControlsUserRights();
                }

                else
                {
                    lblMsg.Text = strErrMsg.ToString();
                    funcClear();
                }
                #endregion
            }

            catch (Exception es)
            {
                es.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }

            finally
            {
                conView.Close();
                sda.Dispose();
                cmdView.Dispose();
                conView.Dispose();
            }
        }

        public void funcreadOnly()
        {
            objCommonFunction.disableControlsTextBox(txtName);
            objCommonFunction.disableControlsTextBox(txtDesignation);
            objCommonFunction.disableControlsTextBox(txtPFNumber);
            objCommonFunction.disableControlsTextBox(txtStatus);

            objCommonFunction.disableControlsDropDownList(ddlSanctionforProsecution);

            ceRCDate.Enabled = false;
            ceRecvDate.Enabled = false;
            ceSanctionRefusedDate.Enabled = false;
            ceCVCDate.Enabled = false;

            btnShowStatus_MODAL.Enabled = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            strMode = "I";
            try
            {
                funcSave(strMode);
                funcClear();
                lblMsg.Text = strErrMsg.ToString();
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
                lblMsg.Text = strErrMsg.ToString();

                if (intErrCode == 2)
                {
                    funcClear();
                }
                else if (intErrCode == 3)
                {
                    txtRCNo.Focus();
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
                strRCNO = txtRCNo.Text.Trim();
                strUser = ViewState["USERNAME"].ToString();

                lblMsg.Text = strErrMsg.ToString();
                funcClear();
            }

            catch (Exception ed)
            {
                ed.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ed);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            funcClear();
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            strSearchNo = txtRCNo.Text.Trim();
            funcShow(strSearchNo, "GET", null);
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void imgSearch_LIST_Click(object sender, ImageClickEventArgs e)
        {
            strSearchNo = txtRCNo_LIST.Text.Trim();
            strSTATUS = txtStatus_LIST.Text;
            strVIEW = "SEARCH";

            if (strSearchNo == "" && strSTATUS == "")
            {
                strVIEW = "LIST";
            }

            funcShow(strSearchNo, strVIEW, strSTATUS);
            lblList.Text = strErrMsg.ToString();
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "VIEW")
                {
                    strRCNO = e.CommandArgument.ToString();
                    if (strRCNO != "")
                    {
                        funcShow(strRCNO, "VIEW", null);
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
            gvMain.DataSource = dtSorting;
            gvMain.DataBind();

            if (dtSorting != null)
            {
                DataView dataView = new DataView(dtSorting);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                gvMain.DataSource = dataView;
                gvMain.DataBind();
            }
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }

        private string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";
                    break;

                case "DESC":
                    GridViewSortDirection = "ASC";
                    break;
            }

            return GridViewSortDirection;
        }

        protected void tabMain_ActiveTabChanged(object sender, EventArgs e)
        {
            if (tabMain.ActiveTab == tabList)
            {
                funcShow(null, "LIST", null); //for bind grid view on List Tab Load
            }
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

        #region ** modal popup code **
        protected void btnShowStatus_MODAL_Click(object sender, EventArgs e)
        {
            txtStatus_MODAL.Text = "";
            txtStatus_MODAL.Text = txtStatus.Text;
            modalPopUp_Status.Show();
            txtStatus.Text = "";
        }

        protected void btnCloseMODAL_Status_Click(object sender, EventArgs e)
        {
            txtStatus.Text = "";
            txtStatus.Text = txtStatus_MODAL.Text;
            objCommonFunction.removeTextBoxFirstComma(txtStatus);
            modalPopUp_Status.Hide();
            txtStatus_MODAL.Text = "";
        }

        #endregion
    }
}