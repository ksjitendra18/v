using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class frmWBStructure : System.Web.UI.Page
    {
        #region ** declare Variable **
        string strMode = string.Empty;
        string strMsg = string.Empty;
        string strSearchNo = string.Empty;
        string strErrMsg = string.Empty;
        string strUser = string.Empty;
        string strUserRole = string.Empty;
        string strVIEW = string.Empty;
        int intErrCode = 0;

        int intCode = 0;
        string strRNO = string.Empty;
        string strCOMPNO = string.Empty;
        string strACCUSED = string.Empty;
        string strDESIGNATION = string.Empty;
        string strPRESENTPOSTING = string.Empty;
        string strBRCOMPLAINT = string.Empty;
        string strZONE = string.Empty;
        string strCIRCLEOFFICE = string.Empty;
        string strREGION = string.Empty;
        DateTime? dtRECDATECOMP = null;
        string strSOURCE = string.Empty;
        string strSOURCEREF = string.Empty;
        DateTime? dtSOURCEDATE = null;
        string strSENTTO = string.Empty;
        DateTime? dtSENTFORINVDATE = null;
        string strACCOUNTNAME = string.Empty;
        decimal decAMOUNT = 0;
        string strALLEGATIONS = string.Empty;
        string strSTATUS = string.Empty;
        string strHOSTATUS = string.Empty;
        string strSTATUSCODE = string.Empty;
        string strREGISTER = string.Empty;
        DateTime? dtDTOFINVREPORT = null;
        string strCASENO = string.Empty;
        string strCASECLOSE = string.Empty;
        DateTime? dtCLOSUREDT = null;
        DateTime? dtRYSENT = null;
        string strCLOSURE = string.Empty;
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
                funcShow(null, null, null, null, "LIST"); //for bind grid view on form Load
                funcbindDropdown();     //Bind All DropDown Lists
            }

            txtRNo.Focus();
            lblMsg.Text = string.Empty;
            funcControlsUserRights();

            #region ** JS Function  **
            imgGet.Attributes.Add("onclick", "return funcSearch_Validation('" + txtRNo.ClientID + "','" + "Please Enter R Number" + "')");
            btnSubmit.Attributes.Add("onclick", "return funcValidation_WB('" + txtRNo.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            btnUpdate.Attributes.Add("onclick", "return funcValidation_WB('" + txtRNo.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            //btnDelete.Attributes.Add("onclick", "return funcSearch_Validation('" + txtRNo.ClientID + "','" + "Please Enter R Number" + "')");
            txtAmount.Attributes.Add("onkeypress", "return isNumbericDecimal(event,'" + txtAmount.ClientID + "')");

            txtCompRecDate.Attributes.Add("readonly", "readonly");
            txtClosureDate.Attributes.Add("readonly", "readonly");
            txtSourceDate.Attributes.Add("readonly", "readonly");
            txtSentForInvDate.Attributes.Add("readonly", "readonly");
            txtDateForINVReport.Attributes.Add("readonly", "readonly");
            txtRYSent.Attributes.Add("readonly", "readonly");
            txtLetterSentDate.Attributes.Add("readonly", "readonly");
            txtReminderDate.Attributes.Add("readonly", "readonly");
            txtReplyReceivedDate.Attributes.Add("readonly", "readonly");
            #endregion
        }

        public void funcSave(string p_strMode)
        {
            SqlConnection conSave = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();
            try
            {
                #region ** assign Control Value **
                intCode = objCommonFunction.convertToIntToolTip(txtRNo);
                strRNO = txtRNo.Text.Trim();
                strBRCOMPLAINT = txtBRComplaint.Text;
                strCOMPNO = txtCompNo.Text;
                strACCUSED = txtAccused.Text;
                strALLEGATIONS = txtAllegations.Text;
                strCASENO = txtCaseNo.Text;
                strPRESENTPOSTING = txtPresentPosting.Text;
                strSOURCE = txtSource.Text;
                strSOURCEREF = txtSourceReference.Text;
                strACCOUNTNAME = txtAccountName.Text;
                strSENTTO = txtSentTo.Text;
                strREGION = txtRegion.Text;
                decAMOUNT = objCommonFunction.convertToDecimal(txtAmount);
                strDESIGNATION = txtDesignation.Text;
                strREGISTER = txtRegister.Text;
                strCASECLOSE = txtClose.Text;
                strSTATUS = txtStatus.Text;
                strHOSTATUS = txtHOStatus.Text;
                strUser = ViewState["USERNAME"].ToString();
                strUserRole = ViewState["USERROLE"].ToString();
                DESKUSERREMARKS = txtDealingOfficerRemarks.Text.Trim();
                BANKNAME = objCommonFunction.ddlSelectedValue(ddlBankName);


                #region ** For Closure Date **
                strCLOSURE = objCommonFunction.chkSelected(chkClosureDate);
                if (lblClosureDate.Text.ToString() != "")
                {
                    strCLOSURE = "N";
                    txtClosureDate.Text = lblClosureDate.Text;
                    string strClosureDate = txtClosureDate.Text.Trim();
                    if (!string.IsNullOrEmpty(strClosureDate))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strClosureDate, out date))
                            dtCLOSUREDT = date;
                    }
                }
                #endregion

                if (strUserRole.ToUpper() != "VMIS_DESKUSER")
                {
                    ZONENEW = objCommonFunction.ddlSelectedValue(ddlZoneNew);
                    CIRCLENEW = objCommonFunction.ddlSelectedValue(ddlCircleNew);
                    strCIRCLEOFFICE = objCommonFunction.ddlSelectedText(ddlCircleOffice);
                    strZONE = objCommonFunction.ddlSelectedText(ddlZone);
                    strSTATUSCODE = objCommonFunction.ddlSelectedValue(ddlStatusCode);
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

                string strSOURCEDATE = txtSourceDate.Text.Trim();
                if (!string.IsNullOrEmpty(strSOURCEDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strSOURCEDATE, out date))
                        dtSOURCEDATE = date;
                }

                string strSENTFORINVDATE = txtSentForInvDate.Text.Trim();
                if (!string.IsNullOrEmpty(strSENTFORINVDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strSENTFORINVDATE, out date))
                        dtSENTFORINVDATE = date;
                }

                string strDTOFINVREPORT = txtDateForINVReport.Text.Trim();
                if (!string.IsNullOrEmpty(strDTOFINVREPORT))
                {
                    DateTime date;
                    if (DateTime.TryParse(strDTOFINVREPORT, out date))
                        dtDTOFINVREPORT = date;
                }

                string strRYSENT = txtRYSent.Text.Trim();
                if (!string.IsNullOrEmpty(strRYSENT))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRYSENT, out date))
                        dtRYSENT = date;
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
                cmdSave.CommandText = "[dbo].[spWBStructure_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_CODE", intCode);
                cmdSave.Parameters.AddWithValue("@p_RNO", strRNO);
                cmdSave.Parameters.AddWithValue("@p_COMPNO", strCOMPNO);
                cmdSave.Parameters.AddWithValue("@p_ACCUSED", strACCUSED);
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", strDESIGNATION);
                cmdSave.Parameters.AddWithValue("@p_PRESENTPOSTING", strPRESENTPOSTING);
                cmdSave.Parameters.AddWithValue("@p_BRCOMPLAINT", strBRCOMPLAINT);
                cmdSave.Parameters.AddWithValue("@p_ZONE", strZONE);
                cmdSave.Parameters.AddWithValue("@p_CIRCLEOFFICE", strCIRCLEOFFICE);
                cmdSave.Parameters.AddWithValue("@p_REGION", strREGION);
                cmdSave.Parameters.AddWithValue("@p_RECDATECOMP", dtRECDATECOMP);
                cmdSave.Parameters.AddWithValue("@p_SOURCE", strSOURCE);
                cmdSave.Parameters.AddWithValue("@p_SOURCEREF", strSOURCEREF);
                cmdSave.Parameters.AddWithValue("@p_SOURCEDATE", dtSOURCEDATE);
                cmdSave.Parameters.AddWithValue("@p_SENTTO", strSENTTO);
                cmdSave.Parameters.AddWithValue("@p_SENTFORINVDATE", dtSENTFORINVDATE);
                cmdSave.Parameters.AddWithValue("@p_ACCOUNTNAME", strACCOUNTNAME);
                cmdSave.Parameters.AddWithValue("@p_AMOUNT", decAMOUNT);
                cmdSave.Parameters.AddWithValue("@p_ALLEGATIONS", strALLEGATIONS);
                cmdSave.Parameters.AddWithValue("@p_STATUS", strSTATUS);
                cmdSave.Parameters.AddWithValue("@p_HOSTATUS", strHOSTATUS);
                cmdSave.Parameters.AddWithValue("@p_STATUSCODE", strSTATUSCODE);
                cmdSave.Parameters.AddWithValue("@p_REGISTER", strREGISTER);
                cmdSave.Parameters.AddWithValue("@p_DTOFINVREPORT", dtDTOFINVREPORT);
                cmdSave.Parameters.AddWithValue("@p_CASENO", strCASENO);
                cmdSave.Parameters.AddWithValue("@p_CASECLOSE", strCASECLOSE);
                cmdSave.Parameters.AddWithValue("@p_CLOSUREDT", dtCLOSUREDT);
                cmdSave.Parameters.AddWithValue("@p_RYSENT", dtRYSENT);
                cmdSave.Parameters.AddWithValue("@p_MODE", @p_strMode);
                cmdSave.Parameters.AddWithValue("@p_USER", strUser);
                cmdSave.Parameters.AddWithValue("@p_USERROLE", strUserRole);
                cmdSave.Parameters.AddWithValue("@p_CLOSURE", strCLOSURE);
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                cmdSave.Parameters.AddWithValue("@p_DESK_USER_REMARKS", DESKUSERREMARKS);
                cmdSave.Parameters.AddWithValue("@p_BANKNAME", BANKNAME);

                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTO", LETTERSENTTO);
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTDATE", dtLETTERSENTDATE);
                cmdSave.Parameters.AddWithValue("@p_REMINDERDATE", dtREMINDERDATE);
                cmdSave.Parameters.AddWithValue("@p_REPLYRECEIVEDDATE", dtREPLYRECEIVEDDATE);

                cmdSave.Parameters.AddWithValue("@p_ZONENEW", ZONENEW);
                cmdSave.Parameters.AddWithValue("@p_CIRCLENEW", CIRCLENEW);

                cmdSave.CommandTimeout = 0;
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
                cmdSave.Dispose();
                conSave.Dispose();
                conSave.Close();
            }
        }

        public void funcShow(string p_strNo, string p_strBranch, string p_strCircle, string p_strAccountName, string strView)
        {
            SqlConnection conView = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdView = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                #region ** call StoredProcedure to View the Data of Complaint  **
                conView.Open();
                cmdView.Connection = conView;
                cmdView.Parameters.Clear();
                cmdView.CommandType = CommandType.StoredProcedure;
                cmdView.CommandText = "[dbo].[spWBStructure_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_BRANCH", p_strBranch);
                cmdView.Parameters.AddWithValue("@p_CIRCLE", p_strCircle);
                cmdView.Parameters.AddWithValue("@p_ACCOUNTNAME", p_strAccountName);
                cmdView.Parameters.AddWithValue("@p_VIEW", strView);

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
                        else if (strView.ToUpper() == "VIEW")
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
                cmdView.Dispose();
                conView.Dispose();
                conView.Close();
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
                cmd.CommandText = "[dbo].[spWBStructure_Delete]";

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
                lblMsg.Text = es.ToString();
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

            txtRNo.ToolTip = dtData.Rows[0]["CODE"].ToString();
            txtRNo.Text = dtData.Rows[0]["RNO"].ToString();
            txtCompRecDate.Text = dtData.Rows[0]["COMPRECDATE"].ToString();
            txtBRComplaint.Text = dtData.Rows[0]["BRCOMPLAINT"].ToString();
            objCommonFunction.ddlSetData(ddlCircleOffice, dtData.Rows[0]["CIRCLEOFFICE"].ToString(), true);
            hidCircleOffice.Value = dtData.Rows[0]["CIRCLEOFFICE"].ToString();
            txtCompNo.Text = dtData.Rows[0]["COMPNO"].ToString();
            //txtClosureDate.Text = dtData.Rows[0]["CLOSUREDATE"].ToString();
            objCommonFunction.chkSetData(chkClosureDate, dtData.Rows[0]["CLOSURE"].ToString());
            lblClosureDate.Text = dtData.Rows[0]["CLOSUREDATE"].ToString();
            txtAccused.Text = dtData.Rows[0]["ACCUSED"].ToString();
            txtAllegations.Text = dtData.Rows[0]["ALLEGATIONS"].ToString();
            txtCaseNo.Text = dtData.Rows[0]["CASENO"].ToString();
            txtPresentPosting.Text = dtData.Rows[0]["PRESENTPOSTING"].ToString();
            objCommonFunction.ddlSetData(ddlZone, dtData.Rows[0]["ZONE"].ToString(), true);
            hidZone.Value = dtData.Rows[0]["ZONE"].ToString();
            txtSource.Text = dtData.Rows[0]["SOURCE"].ToString();
            txtSourceDate.Text = dtData.Rows[0]["SOURCEDATE"].ToString();
            txtSourceReference.Text = dtData.Rows[0]["SOURCEREF"].ToString();
            txtAccountName.Text = dtData.Rows[0]["ACCOUNTNAME"].ToString();
            hidStatusCode.Value = dtData.Rows[0]["STATUSCODE"].ToString();
            txtSentForInvDate.Text = dtData.Rows[0]["SENTFORINVDATE"].ToString();
            txtSentTo.Text = dtData.Rows[0]["SENTTO"].ToString();
            txtRegion.Text = dtData.Rows[0]["REGION"].ToString();
            txtAmount.Text = dtData.Rows[0]["AMOUNT"].ToString();
            txtDateForINVReport.Text = dtData.Rows[0]["INVREPORTDATE"].ToString();
            txtDesignation.Text = dtData.Rows[0]["DESIGNATION"].ToString();
            txtRegister.Text = dtData.Rows[0]["REGISTER"].ToString();
            txtClose.Text = dtData.Rows[0]["CASECLOSE"].ToString();
            txtRYSent.Text = dtData.Rows[0]["RYSENTDATE"].ToString();
            txtStatus.Text = dtData.Rows[0]["STATUS"].ToString();

            objCommonFunction.ddlSetDataValue(ddlStatusCode, dtData.Rows[0]["STATUSCODE"].ToString());
            if (objCommonFunction.ddlSelectedValue(ddlStatusCode) == "0" && dtData.Rows[0]["STATUSCODE"].ToString() != "0")
            {
                lblStatusCodeMIS.Text = dtData.Rows[0]["STATUSCODE"].ToString();
            }
            txtDealingOfficerRemarks.Text = Convert.ToString(dtData.Rows[0]["DESK_USER_REMARKS"]);
            objCommonFunction.ddlSetDataValue(ddlBankName, Convert.ToString(dtData.Rows[0]["BANKNAME"]));

            lblEntryBy.Text = dtData.Rows[0]["ENTRYBY"].ToString();
            lblEntryDate.Text = dtData.Rows[0]["ENTRYDATE"].ToString();
            lblModifyBy.Text = dtData.Rows[0]["MODIFYBY"].ToString();
            lblModifyDate.Text = dtData.Rows[0]["MODIFYDATE"].ToString();

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
                cmd.CommandText = "[dbo].[spWB_Ddl]";
                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlCircleOffice, ds.Tables[0]);
                    objCommonFunction.bindDropdownList(ddlZone, ds.Tables[1]);
                    objCommonFunction.bindDropdownList(ddlLetterSentTo, ds.Tables[2]);
                    objCommonFunction.bindDropdownList(ddlStatusCode, ds.Tables[3]);
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

        public void funcClear()
        {
            txtRNo.ToolTip = string.Empty;
            txtRNo.Text = string.Empty;
            txtCompRecDate.Text = string.Empty;
            txtBRComplaint.Text = string.Empty;
            ddlCircleOffice.SelectedIndex = 0;
            txtCompNo.Text = string.Empty;
            txtClosureDate.Text = string.Empty;
            txtAccused.Text = string.Empty;
            txtAllegations.Text = string.Empty;
            txtCaseNo.Text = string.Empty;
            txtPresentPosting.Text = string.Empty;
            ddlZone.SelectedIndex = 0;
            txtSource.Text = string.Empty;
            txtSourceDate.Text = string.Empty;
            txtSourceReference.Text = string.Empty;
            txtAccountName.Text = string.Empty;
            ddlStatusCode.SelectedIndex = 0;
            lblStatusCodeMIS.Text = string.Empty;
            txtSentForInvDate.Text = string.Empty;
            txtSentTo.Text = string.Empty;
            txtRegion.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtDateForINVReport.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            txtClose.Text = string.Empty;
            txtRYSent.Text = string.Empty;
            txtRegister.Text = string.Empty;
            txtStatus.Text = string.Empty;
            txtHOStatus.Text = string.Empty;
            chkClosureDate.Checked = false;
            lblClosureDate.Text = string.Empty;

            lblEntryBy.Text = string.Empty;
            lblEntryDate.Text = string.Empty;
            lblModifyBy.Text = string.Empty;
            lblModifyDate.Text = string.Empty;
            txtDealingOfficerRemarks.Text = "";
            pnlHeader.Visible = false;
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            ddlBankName.SelectedIndex = 0;

            ddlLetterSentTo.SelectedIndex = 0;
            txtLetterSentDate.Text = "";
            txtReminderDate.Text = "";
            txtReplyReceivedDate.Text = "";
            hidLetterSentTo.Value = "";

            ddlZoneNew.SelectedIndex = 0;
            if (ddlCircleNew.Items.Count > 0)
            {
                ddlCircleNew.SelectedIndex = 0;
            }

            funcControlsUserRights();
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

                txtRNo_LIST.Enabled = true;
                txtBranch_LIST.Enabled = true;
                txtCircle_LIST.Enabled = true;
                txtAccountName_LIST.Enabled = true;

                ceCompRecDate.Enabled = false;
                ceClosureDate.Enabled = false;
                ceSourceDate.Enabled = false;
                ceSentForInvDate.Enabled = false;
                ceDateForINVReport.Enabled = false;
                ceRYSent.Enabled = false;
            }
            else if (strUserRole.ToUpper() == "VMIS_DESKUSER")
            {
                objCommonFunction.DisableAllControls(this.Page);
                pnlHOStatus.Visible = true;
                txtHOStatus.Enabled = true;
                txtDealingOfficerRemarks.Enabled = true;
                btnSubmit.Visible = false;
                btnUpdate.Visible = true;
                btnUpdate.Enabled = true;
                btnDelete.Visible = false;
                btnCancel.Visible = false;

                txtRNo_LIST.Enabled = true;
                txtBranch_LIST.Enabled = true;
                txtCircle_LIST.Enabled = true;
                txtAccountName_LIST.Enabled = true;

                ceCompRecDate.Enabled = false;
                ceClosureDate.Enabled = false;
                ceSourceDate.Enabled = false;
                ceSentForInvDate.Enabled = false;
                ceDateForINVReport.Enabled = false;
                ceRYSent.Enabled = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            funcSave("I");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            funcSave("U");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                strRNO = txtRNo.Text.Trim();
                strUser = ViewState["USERNAME"].ToString();

                funcDelete(strRNO, strUser);
                lblMsg.Text = strErrMsg.ToString();
                funcClear();
            }

            catch (Exception ed)
            {
                ed.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            funcClear();
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            strSearchNo = txtRNo.Text.Trim();
            funcShow(strSearchNo, null, null, null, "GET");
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void imgSearch_LIST_Click(object sender, ImageClickEventArgs e)
        {
            strSearchNo = txtRNo_LIST.Text.Trim();
            strBRCOMPLAINT = txtBranch_LIST.Text.Trim();
            strCIRCLEOFFICE = txtCircle_LIST.Text.Trim();
            strACCOUNTNAME = txtAccountName_LIST.Text.Trim();
            strVIEW = "SEARCH";

            if (strSearchNo == "" && strBRCOMPLAINT == "" && strCIRCLEOFFICE == "" && strACCOUNTNAME == "")
            {
                strVIEW = "LIST";
            }

            funcShow(strSearchNo, strBRCOMPLAINT, strCIRCLEOFFICE, strACCOUNTNAME, strVIEW);
            lblList.Text = strErrMsg.ToString();
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "VIEW")
                {
                    strRNO = e.CommandArgument.ToString();
                    if (strRNO != "")
                    {
                        funcShow(strRNO, null, null, null, "VIEW");
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
                funcShow(null, null, null, null, "LIST"); //for bind grid view on List Tab Load
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

        protected void btnShowBranch_MODAL_Click(object sender, EventArgs e)
        {
            txtBranch_MODAL.Text = "";
            txtBranch_MODAL.Text = txtBRComplaint.Text;
            modalPopUp_Branch.Show();
            txtBRComplaint.Text = "";
        }

        protected void btnCloseBranch_MODAL_Click(object sender, EventArgs e)
        {
            txtBRComplaint.Text = "";
            txtBRComplaint.Text = txtBranch_MODAL.Text;
            objCommonFunction.removeTextBoxFirstComma(txtBRComplaint);
            modalPopUp_Branch.Hide();
            txtBranch_MODAL.Text = "";
        }

        protected void btnShowAllegations_MODAL_Click(object sender, EventArgs e)
        {
            txtAllegations_MODAL.Text = "";
            txtAllegations_MODAL.Text = txtAllegations.Text;
            modalPopUp_Allegations.Show();
            txtAllegations.Text = "";
        }

        protected void btnCloseMODAL_Allegations_Click(object sender, EventArgs e)
        {
            txtAllegations.Text = "";
            txtAllegations.Text = txtAllegations_MODAL.Text;
            objCommonFunction.removeTextBoxFirstComma(txtAllegations);
            modalPopUp_Allegations.Hide();
            txtAllegations_MODAL.Text = "";
        }

        protected void btnShowAccountName_MODAL_Click(object sender, EventArgs e)
        {
            txtAccountName_MODAL.Text = "";
            txtAccountName_MODAL.Text = txtAccountName.Text;
            modalPopUp_AccountName.Show();
            txtAccountName.Text = "";
        }

        protected void btnCloseMODAL_AccountName_Click(object sender, EventArgs e)
        {
            txtAccountName.Text = "";
            txtAccountName.Text = txtAccountName_MODAL.Text;
            objCommonFunction.removeTextBoxFirstComma(txtAccountName);
            modalPopUp_AccountName.Hide();
            txtAccountName_MODAL.Text = "";
        }

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

        protected void ddlZoneNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ZONE = objCommonFunction.ddlSelectedValue(ddlZoneNew);

            if (!string.IsNullOrEmpty(ZONE))
            {
                objCommonFunction.funcZoneCircleMaster(ddlCircleNew, ZONE);
            }
        }

        #endregion

    }
}