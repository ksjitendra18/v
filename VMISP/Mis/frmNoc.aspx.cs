using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class frmNoc : System.Web.UI.Page
    {
        #region ** declare Variable **
        string strMode = string.Empty;
        string strMsg = string.Empty;
        string strSearchNo = string.Empty;
        string strErrMsg = string.Empty;
        string strUser = string.Empty;
        string strUserRole = string.Empty;
        int intErrCode = 0;

        int intCode = 0;
        string strSNO = string.Empty;
        DateTime? dtRECDATECOMP = null;
        string strBRCOMPLAINT = string.Empty;
        string strCIRCLEOFFICE = string.Empty;
        string strPFNO = string.Empty;
        DateTime? dtCLEARANCEDT = null;
        string strNAME = string.Empty;
        string strDESIGNATION = string.Empty;
        string strSTATE = string.Empty;
        string strSCALE = string.Empty;
        string strREMARKS = string.Empty;
        string strHOREMARKS = string.Empty;
        string strVIEW = string.Empty;
        string DESKUSERREMARKS = string.Empty;
        string BANKNAME = string.Empty;

        DateTime? dtLETTERSENTDATE = null;
        DateTime? dtREMINDERDATE = null;
        DateTime? dtREPLYRECEIVEDDATE = null;
        string LETTERSENTTO = string.Empty;

        string ZONENEW = string.Empty;
        string CIRCLENEW = string.Empty;

        CommonFunction objCommonFunction = new CommonFunction();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();
                funcShow(null, "LIST", null, null); //for bind grid view on form Load
                funcbindDropdown();     //Bind Circle Office DropDown List
            }

            txtSNo.Focus();
            lblMsg.Text = string.Empty;
            funcControlsUserRights();

            #region ** JS Function  **
            imgGet.Attributes.Add("onclick", "return funcSearch_Validation('" + txtSNo.ClientID + "','" + "Please Enter S Number" + "')");
            btnSubmit.Attributes.Add("onclick", "return funcValidation_NOC('" + txtSNo.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            btnUpdate.Attributes.Add("onclick", "return funcValidation_NOC('" + txtSNo.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            //btnDelete.Attributes.Add("onclick", "return funcSearch_Validation('" + txtSNo.ClientID + "','" + "Please Enter S Number" + "')");

            txtCompRecDate.Attributes.Add("readonly", "readonly");
            txtClearanceDate.Attributes.Add("readonly", "readonly");
            txtLetterSentDate.Attributes.Add("readonly", "readonly");
            txtReminderDate.Attributes.Add("readonly", "readonly");
            txtReplyReceivedDate.Attributes.Add("readonly", "readonly");
            #endregion
        }

        public void funcbindDropdown()
        {

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            try
            {
                #region ** call StoredProcedure to bind Circle Office dropDown  **
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spNOC_Ddl]";
                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlCircleOffice, ds.Tables[0]);
                    objCommonFunction.bindDropdownList(ddlState, ds.Tables[1]);
                    objCommonFunction.bindDropdownList_SELECT(ddlScale, ds.Tables[2]);
                    objCommonFunction.bindDropdownList(ddlLetterSentTo, ds.Tables[3]);
                    objCommonFunction.bindDropdownList(ddlZoneNew, ds.Tables[4]);
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
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public void funcSave(string p_strMode)
        {
            SqlConnection conSave = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();
            try
            {
                #region ** assign Control Value **
                intCode = objCommonFunction.convertToIntToolTip(txtSNo);
                strSNO = txtSNo.Text.Trim();
                strBRCOMPLAINT = txtBRComplaint.Text;
                strPFNO = txtPFNo.Text;
                strNAME = txtName.Text;
                strDESIGNATION = txtDesignation.Text;
                strREMARKS = txtRemarks.Text;
                strHOREMARKS = txtHORemarks.Text;
                strUser = ViewState["USERNAME"].ToString();
                strUserRole = ViewState["USERROLE"].ToString();
                DESKUSERREMARKS = txtDealingOfficerRemarks.Text.Trim();
                BANKNAME = objCommonFunction.ddlSelectedValue(ddlBankName);


                if (strUserRole.ToUpper() != "VMIS_DESKUSER")
                {
                    ZONENEW = objCommonFunction.ddlSelectedValue(ddlZoneNew);
                    CIRCLENEW = objCommonFunction.ddlSelectedValue(ddlCircleNew);
                    strCIRCLEOFFICE = objCommonFunction.ddlSelectedText(ddlCircleOffice);
                    strSCALE = objCommonFunction.ddlSelectedValue_Scale(ddlScale);
                    strSTATE = objCommonFunction.ddlSelectedText(ddlState);
                    LETTERSENTTO = objCommonFunction.ddlSelectedValue(ddlLetterSentTo);
                }
                

                #region ** convert Date **
                string strRECDATECOMP = txtCompRecDate.Text.Trim();
                if (!string.IsNullOrEmpty(strRECDATECOMP))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRECDATECOMP, out date))
                        dtRECDATECOMP = date;
                }

                string strClearanceDate = txtClearanceDate.Text.Trim();
                if (!string.IsNullOrEmpty(strClearanceDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strClearanceDate, out date))
                        dtCLEARANCEDT = date;
                }

                string strLetterSentDate = txtLetterSentDate.Text.Trim();
                if (!string.IsNullOrEmpty(strLetterSentDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strLetterSentDate, out date))
                        dtLETTERSENTDATE = date;
                }

                string strReminderDate = txtReminderDate.Text.Trim();
                if (!string.IsNullOrEmpty(strReminderDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strReminderDate, out date))
                        dtREMINDERDATE = date;
                }

                string strReplyReceivedDate = txtReplyReceivedDate.Text.Trim();
                if (!string.IsNullOrEmpty(strReplyReceivedDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strReplyReceivedDate, out date))
                        dtREPLYRECEIVEDDATE = date;
                }

                #endregion
                #endregion

                #region ** call StoredProcedure to Save/Update data in Table  **
                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spNOC_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_CODE", intCode);
                cmdSave.Parameters.AddWithValue("@p_SNO", strSNO);
                cmdSave.Parameters.AddWithValue("@p_RECDATECOMP", dtRECDATECOMP);
                cmdSave.Parameters.AddWithValue("@p_BRCOMPLAINT", strBRCOMPLAINT);
                cmdSave.Parameters.AddWithValue("@p_CIRCLEOFFICE", strCIRCLEOFFICE);
                cmdSave.Parameters.AddWithValue("@p_PFNO", strPFNO);
                cmdSave.Parameters.AddWithValue("@p_CLEARANCEDT", dtCLEARANCEDT);
                cmdSave.Parameters.AddWithValue("@p_NAME", strNAME);
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", strDESIGNATION);
                cmdSave.Parameters.AddWithValue("@p_STATE", strSTATE);
                cmdSave.Parameters.AddWithValue("@p_SCALE", strSCALE);
                cmdSave.Parameters.AddWithValue("@p_REMARKS", strREMARKS);
                cmdSave.Parameters.AddWithValue("@p_HOREMARKS", strHOREMARKS);
                cmdSave.Parameters.AddWithValue("@p_MODE", @p_strMode);
                cmdSave.Parameters.AddWithValue("@p_USER", strUser);
                cmdSave.Parameters.AddWithValue("@p_USERROLE", strUserRole);
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                cmdSave.Parameters.AddWithValue("@p_DESK_USER_REMARKS", DESKUSERREMARKS);
                cmdSave.Parameters.AddWithValue("@p_BANKNAME", BANKNAME);

                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTO", LETTERSENTTO);
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTDATE", dtLETTERSENTDATE);
                cmdSave.Parameters.AddWithValue("@p_REMINDERDATE", dtREMINDERDATE);
                cmdSave.Parameters.AddWithValue("@p_REPLYRECEIVEDDATE", dtREPLYRECEIVEDDATE);
                cmdSave.Parameters.AddWithValue("@p_ZONENEW", ZONENEW);
                cmdSave.Parameters.AddWithValue("@p_CIRCLENEW", CIRCLENEW);

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                    lblMsg.Text = strErrMsg;
                    funcClear();
                }
                else
                {
                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                    lblMsg.Text = strErrMsg;
                }
                #endregion
            }
            catch (Exception es)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }
            finally
            {
                cmdSave.Clone();
                conSave.Dispose();
                conSave.Close();
            }
        }

        public void funcShow(string p_strNo, string p_strView, string p_strPFNO, string p_strNAME)
        {
            DataTable dt = new DataTable();
            SqlConnection conView = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdView = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmdView);

            try
            {
                #region ** call StoredProcedure to View the Data of Complaint  **
                conView.Open();
                cmdView.Connection = conView;
                cmdView.Parameters.Clear();
                cmdView.CommandType = CommandType.StoredProcedure;
                cmdView.CommandText = "[dbo].[spNOC_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_VIEW", p_strView);
                cmdView.Parameters.AddWithValue("@p_PFNO", p_strPFNO);
                cmdView.Parameters.AddWithValue("@p_NAME", p_strNAME);

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

        public void funcDelete(string p_strRNo, string p_strUser)
        {
            try
            {
                #region ** call StoredProcedure to View the Data of Complaint  **
                SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spNOC_Delete]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(sqlErrMsgOutput);
                cmd.Parameters.Add(sqlErrCodeOutput);

                cmd.Parameters.AddWithValue("@p_RNO", p_strRNo);
                cmd.Parameters.AddWithValue("@p_USER", p_strUser);

                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();

                strErrMsg = sqlErrMsgOutput.Value.ToString();
                intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                #endregion
            }

            catch (Exception es)
            {
                //lblMsg.Text = es.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }
        }

        public void funcBindControl(DataTable dt)
        {
            DataTable dtData = dt;
            tabMain.ActiveTabIndex = 0;
            pnlHeader.Visible = true;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            btnDelete.Visible = false;

            txtSNo.ToolTip = dtData.Rows[0]["CODE"].ToString();
            txtSNo.Text = dtData.Rows[0]["SNO"].ToString();
            txtCompRecDate.Text = dtData.Rows[0]["RECDATE"].ToString();
            txtBRComplaint.Text = dtData.Rows[0]["BRCOMPLAINT"].ToString();
            objCommonFunction.ddlSetData(ddlCircleOffice, dtData.Rows[0]["CIRCLEOFFICE"].ToString(), true);
            hidCircleOffice.Value = dtData.Rows[0]["CIRCLEOFFICE"].ToString();
            txtPFNo.Text = dtData.Rows[0]["PFNO"].ToString();
            txtClearanceDate.Text = dtData.Rows[0]["CLOSUREDATE"].ToString();
            txtName.Text = dtData.Rows[0]["NAME"].ToString();
            txtDesignation.Text = dtData.Rows[0]["DESIGNATION"].ToString();
            objCommonFunction.ddlSetData(ddlState, dtData.Rows[0]["STATE"].ToString(), true);
            hidState.Value = dtData.Rows[0]["STATE"].ToString();
            objCommonFunction.ddlSetDataValue_Scale(ddlScale, dtData.Rows[0]["SCLAECODE"].ToString());
            hidScale.Value = dtData.Rows[0]["SCLAECODE"].ToString();
            txtRemarks.Text = dtData.Rows[0]["REMARKS"].ToString();
            txtDealingOfficerRemarks.Text = Convert.ToString(dtData.Rows[0]["DESK_USER_REMARKS"]);
            lblEntryBy.Text = dtData.Rows[0]["ENTRYBY"].ToString();
            lblEntryDate.Text = dtData.Rows[0]["ENTRYDATE"].ToString();
            lblModifyBy.Text = dtData.Rows[0]["MODIFYBY"].ToString();
            lblModifyDate.Text = dtData.Rows[0]["MODIFYDATE"].ToString();
            objCommonFunction.ddlSetDataValue(ddlBankName, Convert.ToString(dtData.Rows[0]["BANKNAME"]));

            txtLetterSentDate.Text = Convert.ToString(dtData.Rows[0]["LETTERSENTDATE"]);
            txtReminderDate.Text = Convert.ToString(dtData.Rows[0]["REMINDERDATE"]);
            txtReplyReceivedDate.Text = Convert.ToString(dtData.Rows[0]["REPLYRECEIVEDDATE"]);
            objCommonFunction.ddlSetDataValue(ddlLetterSentTo, Convert.ToString(dtData.Rows[0]["LETTERSENTTO"]));
            hidLetterSentTo.Value = Convert.ToString(dtData.Rows[0]["LETTERSENTTO"]);

            objCommonFunction.ddlSetDataValue(ddlZoneNew, Convert.ToString(dtData.Rows[0]["NEWZONE"]));
            string ZONE = Convert.ToString(dtData.Rows[0]["NEWZONE"]);
            if (!string.IsNullOrEmpty(ZONE))
            {
                objCommonFunction.funcZoneCircleMaster(ddlCircleNew, ZONE);
                objCommonFunction.ddlSetDataValue(ddlCircleNew, Convert.ToString(dtData.Rows[0]["NEWCIRCLE"]));
            }
        }

        public void funcClear()
        {
            txtSNo.ToolTip = string.Empty;
            txtSNo.Text = string.Empty;
            txtCompRecDate.Text = string.Empty;
            txtBRComplaint.Text = string.Empty;
            ddlCircleOffice.SelectedIndex = 0;
            hidCircleOffice.Value = string.Empty;
            txtPFNo.Text = string.Empty;
            txtClearanceDate.Text = string.Empty;
            txtName.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            ddlState.SelectedIndex = 0;
            ddlScale.SelectedIndex = 0;
            txtRemarks.Text = string.Empty;
            txtHORemarks.Text = string.Empty;
            hidScale.Value = string.Empty;

            lblEntryBy.Text = string.Empty;
            lblEntryDate.Text = string.Empty;
            lblModifyBy.Text = string.Empty;
            lblModifyDate.Text = string.Empty;

            pnlHeader.Visible = false;
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            txtDealingOfficerRemarks.Text = "";
            ddlBankName.SelectedIndex = 0;
            funcControlsUserRights();

            ddlLetterSentTo.SelectedIndex = 0;
            txtLetterSentDate.Text = "";
            txtReminderDate.Text = "";
            txtReplyReceivedDate.Text = "";
            hidLetterSentTo.Value = "";

            ddlZoneNew.SelectedIndex = 0;
            if (ddlCircleNew.Items.Count > 0)
            {
                ddlCircleNew.Items.Clear();
            }

        }

        public void funcControlsUserRights()
        {
            strUserRole = ViewState["USERROLE"].ToString();

            if (strUserRole.ToUpper() == "VMIS_VIEWUSER")
            {
                objCommonFunction.DisableAllControls(this.Page);
                btnSubmit.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                btnCancel.Visible = false;

                ceCompRecDate.Enabled = false;
                ceClearanceDate.Enabled = false;

                txtRNo_LIST.Enabled = true;
                txtPFNumber_LIST.Enabled = true;
                txtName_LIST.Enabled = true;
            }
            else if (strUserRole.ToUpper() == "VMIS_DESKUSER")
            {
                objCommonFunction.DisableAllControls(this.Page);
                pnlHOStatus.Visible = true;
                txtDealingOfficerRemarks.Enabled = true;
                txtHORemarks.Enabled = true;
                btnSubmit.Visible = false;
                btnUpdate.Visible = true;
                btnUpdate.Enabled = true;
                btnDelete.Visible = false;
                btnCancel.Visible = false;

                ceCompRecDate.Enabled = false;
                ceClearanceDate.Enabled = false;

                txtRNo_LIST.Enabled = true;
                txtPFNumber_LIST.Enabled = true;
                txtName_LIST.Enabled = true;
            }
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
                    txtSNo.Focus();
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
                strSNO = txtSNo.Text.Trim();
                strUser = ViewState["USERNAME"].ToString();

                funcDelete(strSNO, strUser);
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
            strSearchNo = txtSNo.Text.Trim();
            funcShow(strSearchNo, "GET", null, null);
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void imgSearch_LIST_Click(object sender, ImageClickEventArgs e)
        {
            strSearchNo = txtRNo_LIST.Text.Trim();
            strPFNO = txtPFNumber_LIST.Text.Trim();
            strNAME = txtName_LIST.Text;
            strVIEW = "SEARCH";

            if (strSearchNo == "" && strPFNO == "" && strNAME == "")
            {
                strVIEW = "LIST";
            }
            funcShow(strSearchNo, strVIEW, strPFNO, strNAME);
            lblList.Text = strErrMsg.ToString();
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "VIEW")
                {
                    strSNO = e.CommandArgument.ToString();
                    if (strSNO != "")
                    {
                        funcShow(strSNO, "VIEW", null, null);
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

        protected void tabMain_ActiveTabChanged(object sender, EventArgs e)
        {
            if (tabMain.ActiveTab == tabList)
            {
                funcShow(null, "LIST", null, null); //for bind grid view on form Load
            }

            //Code hereTabContainer
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

        protected void ddlZoneNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ZONE = objCommonFunction.ddlSelectedValue(ddlZoneNew);

            if (!string.IsNullOrEmpty(ZONE))
            {
                objCommonFunction.funcZoneCircleMaster(ddlCircleNew, ZONE);
            }
        }

    }
}