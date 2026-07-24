using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;

namespace VMISP.Mis
{
    public partial class VigilanceMonitoring : System.Web.UI.Page
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
        string strRNO = string.Empty;
        string strBRCOMPLAINT = string.Empty;
        string strCIRCLEOFFICE = string.Empty;
        string strNAME = string.Empty;
        string strPFNO = string.Empty;
        string strSOURCE = string.Empty;
        string strREGISTER = string.Empty;
        string strACCOUNTNAME = string.Empty;
        decimal decAMOUNT = 0;
        string strFINAL = string.Empty;
        string strDESIGNATION = string.Empty;
        string strLAPSENATURE = string.Empty;
        string strDAREFNO = string.Empty;
        string strSCALE = string.Empty;
        string strUS = string.Empty;
        string strCBIRCNO1 = string.Empty;
        string strNATUREOFACCOUNT = string.Empty;
        string strNAPUNDA = string.Empty;
        string strCONNECTEDVIGCASE = string.Empty;
        string strSTATE = string.Empty;
        string strNATCHSHEET = string.Empty;
        string strSTATUSCODE = string.Empty;
        string strNATURECASE = string.Empty;
        string strSTATUS = string.Empty;
        string strHOSTATUS = string.Empty;
        string strVIEW = string.Empty;
        string strRNODATE = string.Empty;
        string strPDREFNO = string.Empty;
        string EXTERNALSOURCE = string.Empty;
        string BANKNAME = string.Empty;
        string DESKUSERREMARKS = string.Empty;

        DateTime? dtRNODATE = null;
        DateTime? dtCHARGEDATE = null;
        DateTime? dtRC1DATE = null;
        DateTime? dtRETIREMENTDATE = null;
        DateTime? dtSUSPENSION = null;
        DateTime? dtDAORDDATE = null;
        DateTime? dtOCCURDATE = null;
        DateTime? dtREVOCATIONDATE = null;
        DateTime? dtEXTERNALSOURCEDATE = null;

        DateTime? dtLETTERSENTDATE = null;
        DateTime? dtREMINDERDATE = null;
        DateTime? dtREPLYRECEIVEDDATE = null;
        DateTime? dtSANCTIONORDER = null;
        string LETTERSENTTO = string.Empty;

        string ZONENEW = string.Empty;
        string CIRCLENEW = string.Empty;

        CommonFunction objCommonFunction = new CommonFunction();
        #endregion

        Logger logger = LogManager.GetCurrentClassLogger();
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!ClientScript.IsStartupScriptRegistered(GetType(), "MaskedEditFix"))
            {
                ClientScript.RegisterStartupScript(GetType(), "MaskedEditFix", String.Format("<script type='text/javascript' src='{0}'></script>", Page.ResolveUrl("../Js/MaskedEditFix.js")));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();
                funcShow(null, "LIST", null, null, null, null, null, null, null); //for bind grid view on form Load
                funcbindDropdown();     //Bind All DropDown Lists
            }

            txtRNo.Focus();
            lblMsg.Text = string.Empty;
            funcControlsUserRights();

            #region ** JS Function  **
            btnSubmit.Attributes.Add("onclick", "return funcValidation_VigilanceTemp('" + txtRNo.ClientID + "','" + txtName.ClientID + "','" + txtPFNo.ClientID + "','" + chkRNoDate.ClientID + "','" + ddlScale.ClientID + "','" + ddlStatusCode.ClientID + "','" + txtNAPUNDA.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            btnUpdate.Attributes.Add("onclick", "return funcValidation_VigilanceTemp('" + txtRNo.ClientID + "','" + txtName.ClientID + "','" + txtPFNo.ClientID + "','" + chkRNoDate.ClientID + "','" + ddlScale.ClientID + "','" + ddlStatusCode.ClientID + "','" + txtNAPUNDA.ClientID + "','" + ddlCircleOffice.ClientID + "')");

            imgGet.Attributes.Add("onclick", "return funcSearch_Validation('" + txtRNo.ClientID + "','" + "Please Enter Vigilance Number" + "')");
            txtAmount.Attributes.Add("onkeypress", "return isNumbericDecimal(event,'" + txtAmount.ClientID + "')");

            txtChargeDate.Attributes.Add("onblur", "return checkDate('" + txtChargeDate.ClientID + "')");
            txtRNoDate.Attributes.Add("onblur", "return checkDate('" + txtRNoDate.ClientID + "')");
            txtRC1Date.Attributes.Add("onblur", "return checkDate('" + txtRC1Date.ClientID + "')");
            txtRetirementDate.Attributes.Add("onblur", "return checkDate('" + txtRetirementDate.ClientID + "')");
            txtSuspensionDate.Attributes.Add("onblur", "return checkDate('" + txtSuspensionDate.ClientID + "')");
            txtDAOrdDate.Attributes.Add("onblur", "return checkDate('" + txtDAOrdDate.ClientID + "')");
            txtOccurDate.Attributes.Add("onblur", "return checkDate('" + txtOccurDate.ClientID + "')");
            txtRevocationDate.Attributes.Add("onblur", "return checkDate('" + txtOccurDate.ClientID + "')");

            #region ** readOnly Date Contols **
            txtChargeDate.Attributes.Add("readonly", "readonly");
            txtRNoDate.Attributes.Add("readonly", "readonly");
            txtRC1Date.Attributes.Add("readonly", "readonly");
            txtRetirementDate.Attributes.Add("readonly", "readonly");
            txtSuspensionDate.Attributes.Add("readonly", "readonly");
            txtDAOrdDate.Attributes.Add("readonly", "readonly");
            txtOccurDate.Attributes.Add("readonly", "readonly");
            txtRevocationDate.Attributes.Add("readonly", "readonly");

            txtLetterSentDate.Attributes.Add("readonly", "readonly");
            txtReminderDate.Attributes.Add("readonly", "readonly");
            txtReplyReceivedDate.Attributes.Add("readonly", "readonly");

            #endregion
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

                logger.Info("funcbindDropdown started at " + DateTime.Now);
                con.Open();
                logger.Info("SQL Connection opened.");
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spVigilanceMIS_Ddl]";
                cmd.CommandTimeout = 0;
                sda.Fill(ds);
                logger.Info("Stored procedure executed. Tables returned: " + ds.Tables.Count);
                if (ds.Tables.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlCircleOffice, ds.Tables[0]);
                    objCommonFunction.bindDropdownList(ddlState, ds.Tables[1]);
                    objCommonFunction.bindDropdownList_SELECT(ddlScale, ds.Tables[2]);
                    objCommonFunction.bindDropdownList(ddlLetterSentTo, ds.Tables[3]);
                    objCommonFunction.bindDropdownList(ddlStatusCode, ds.Tables[4]);
                    objCommonFunction.bindDropdownList(ddlZoneNew, ds.Tables[5]);

                    logger.Info("Dropdowns bound. ddlZoneNew items: " + ddlZoneNew.Items.Count);
                     
                    foreach (ListItem item in ddlZoneNew.Items)
                    {
                        logger.Info("ddlZoneNew item: " + item.Text + " - " + item.Value);
                    }
                }
                else
                {
                    logger.Info("No tables returned from stored procedure.");
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
                intCode = objCommonFunction.convertToIntToolTip(txtRNo);
                strRNO = txtRNo.Text.Trim();
                strBRCOMPLAINT = txtBRComplaint.Text;
                strNAME = txtName.Text;
                strPFNO = txtPFNo.Text;
                strSOURCE = txtSource.Text;
                strACCOUNTNAME = txtAccountName.Text;
                decAMOUNT = objCommonFunction.convertToDecimal(txtAmount);
                strDESIGNATION = txtDesignation.Text;
                strPDREFNO = txtPDRefNo.Text;
                strLAPSENATURE = txtLapseNature.Text;
                strDAREFNO = txtDARefNo.Text;
                strCBIRCNO1 = txtCbiRcNo1.Text;
                strCONNECTEDVIGCASE = txtConnectedVigCase.Text;
                strNAPUNDA = txtNAPUNDA.Text;
                strSTATUS = txtStatus.Text;
                strHOSTATUS = txtHOStatus.Text;
                strUser = ViewState["USERNAME"].ToString();
                strUserRole = ViewState["USERROLE"].ToString();
                EXTERNALSOURCE = txtExternalSource.Text.Trim();
                BANKNAME = objCommonFunction.ddlSelectedValue(ddlBankName);

                //for R NO Date
                strRNODATE = objCommonFunction.chkSelected(chkRNoDate);
                if (lblRNoDate.Text.ToString() != "")
                {
                    strRNODATE = "N";
                    txtRNoDate.Text = lblRNoDate.Text;
                    string strRNoDateTemp = txtRNoDate.Text.Trim();
                    if (!string.IsNullOrEmpty(strRNoDateTemp))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRNoDateTemp, out date))
                            dtRNODATE = date;
                    }
                }

                if (strUserRole.ToUpper() != "VMIS_DESKUSER")
                {
                    ZONENEW = objCommonFunction.ddlSelectedValue(ddlZoneNew);
                    CIRCLENEW = objCommonFunction.ddlSelectedValue(ddlCircleNew);
                    strCIRCLEOFFICE = objCommonFunction.ddlSelectedValue(ddlCircleOffice);
                    strUS = objCommonFunction.ddlSelectedText(ddlUS);
                    strSTATUSCODE = objCommonFunction.ddlSelectedValue(ddlStatusCode);
                    //strNATUREOFACCOUNT = objCommonFunction.ddlSelectedValue(ddlAccountNature);
                    strSCALE = objCommonFunction.ddlSelectedValue_Scale(ddlScale);
                    strNATCHSHEET = objCommonFunction.ddlSelectedValue(ddlNatCHSheet);
                    strFINAL = objCommonFunction.ddlSelectedText(ddlFinal);
                    strSTATE = objCommonFunction.ddlSelectedValue(ddlState);
                    //strREGISTER = objCommonFunction.ddlSelectedValue(ddlRegister);
                    LETTERSENTTO = objCommonFunction.ddlSelectedValue(ddlLetterSentTo);
                }

                #region ** convert Date **
                string strCHARGEDATE = txtChargeDate.Text.Trim();
                if (!string.IsNullOrEmpty(strCHARGEDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strCHARGEDATE, out date))
                        dtCHARGEDATE = date;
                }

                string strRC1DATE = txtRC1Date.Text.Trim();
                if (!string.IsNullOrEmpty(strRC1DATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRC1DATE, out date))
                        dtRC1DATE = date;
                }

                string strdtRETIREMENTDATE = txtRetirementDate.Text.Trim();
                if (!string.IsNullOrEmpty(strdtRETIREMENTDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strdtRETIREMENTDATE, out date))
                        dtRETIREMENTDATE = date;
                }

                string strSUSPENSION = txtSuspensionDate.Text.Trim();
                if (!string.IsNullOrEmpty(strSUSPENSION))
                {
                    DateTime date;
                    if (DateTime.TryParse(strSUSPENSION, out date))
                        dtSUSPENSION = date;
                }

                string strOCCURDATE = txtOccurDate.Text.Trim();
                if (!string.IsNullOrEmpty(strOCCURDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strOCCURDATE, out date))
                        dtOCCURDATE = date;
                }

                string strREVOCATIONDATE = txtRevocationDate.Text.Trim();
                if (!string.IsNullOrEmpty(strREVOCATIONDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strREVOCATIONDATE, out date))
                        dtREVOCATIONDATE = date;
                }

                string strDAORDDATE = txtDAOrdDate.Text.Trim();
                if (!string.IsNullOrEmpty(strDAORDDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strDAORDDATE, out date))
                        dtDAORDDATE = date;
                }

                string EXTERNALSOURCEDATE = txtExternalSourceDate.Text.Trim();
                if (!string.IsNullOrEmpty(EXTERNALSOURCEDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(EXTERNALSOURCEDATE, out date))
                        dtEXTERNALSOURCEDATE = date;
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

                string strSanctionOrderDate = txtSanctionOrder.Text.Trim();
                if (!string.IsNullOrEmpty(strSanctionOrderDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strSanctionOrderDate, out date))
                        dtSANCTIONORDER = date;
                }
                #endregion

                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spVigilanceMIS_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_CODE", intCode);
                cmdSave.Parameters.AddWithValue("@p_RNO", strRNO);
                cmdSave.Parameters.AddWithValue("@p_BRCOMPLAINT", strBRCOMPLAINT);
                cmdSave.Parameters.AddWithValue("@p_CIRCLEOFFICE", strCIRCLEOFFICE);
                cmdSave.Parameters.AddWithValue("@p_NAME", strNAME);
                cmdSave.Parameters.AddWithValue("@p_PFNO", strPFNO);
                cmdSave.Parameters.AddWithValue("@p_SOURCE", strSOURCE);
                cmdSave.Parameters.AddWithValue("@p_REGISTER", strREGISTER);
                cmdSave.Parameters.AddWithValue("@p_ACCOUNTNAME", strACCOUNTNAME);
                cmdSave.Parameters.AddWithValue("@p_AMOUNT", decAMOUNT);
                cmdSave.Parameters.AddWithValue("@p_FINAL", strFINAL);
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", strDESIGNATION);
                cmdSave.Parameters.AddWithValue("@p_PDREFNO", strPDREFNO);
                cmdSave.Parameters.AddWithValue("@p_LAPSENATURE", strLAPSENATURE);
                cmdSave.Parameters.AddWithValue("@p_DAREFNO", strDAREFNO);
                cmdSave.Parameters.AddWithValue("@p_US", strUS);
                cmdSave.Parameters.AddWithValue("@p_CBIRCNO1", strCBIRCNO1);
                cmdSave.Parameters.AddWithValue("@p_NATUREOFACCOUNT", strNATUREOFACCOUNT);
                cmdSave.Parameters.AddWithValue("@p_NAPUNDA", strNAPUNDA);
                cmdSave.Parameters.AddWithValue("@p_CONNECTEDVIGCASE", strCONNECTEDVIGCASE);
                cmdSave.Parameters.AddWithValue("@p_STATE", strSTATE);
                cmdSave.Parameters.AddWithValue("@p_NATCHSHEET", strNATCHSHEET);
                cmdSave.Parameters.AddWithValue("@p_STATUS", strSTATUS);
                cmdSave.Parameters.AddWithValue("@p_HOSTATUS", strHOSTATUS);
                cmdSave.Parameters.AddWithValue("@p_STATUSCODE", strSTATUSCODE);
                cmdSave.Parameters.AddWithValue("@p_SCALE", strSCALE);
                cmdSave.Parameters.AddWithValue("@p_CHK_RNODATE", strRNODATE);
                cmdSave.Parameters.AddWithValue("@p_EXTERNALSOURCE", EXTERNALSOURCE);
                cmdSave.Parameters.AddWithValue("@p_BANKNAME", BANKNAME);

                cmdSave.Parameters.AddWithValue("@p_RNODATE", dtRNODATE);
                cmdSave.Parameters.AddWithValue("@p_CHARGEDATE", dtCHARGEDATE);
                cmdSave.Parameters.AddWithValue("@p_RC1DATE", dtRC1DATE);
                cmdSave.Parameters.AddWithValue("@p_RETIREMENTDATE", dtRETIREMENTDATE);
                cmdSave.Parameters.AddWithValue("@p_SUSPENSION", dtSUSPENSION);
                cmdSave.Parameters.AddWithValue("@p_DAORDDATE", dtDAORDDATE);
                cmdSave.Parameters.AddWithValue("@p_OCCURDATE", dtOCCURDATE);
                cmdSave.Parameters.AddWithValue("@p_REVOCATIONDATE", dtREVOCATIONDATE);
                cmdSave.Parameters.AddWithValue("@p_EXTERNALSOURCEDATE", dtEXTERNALSOURCEDATE);

                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTO", LETTERSENTTO);
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTDATE", dtLETTERSENTDATE);
                cmdSave.Parameters.AddWithValue("@p_REMINDERDATE", dtREMINDERDATE);
                cmdSave.Parameters.AddWithValue("@p_REPLYRECEIVEDDATE", dtREPLYRECEIVEDDATE);

                cmdSave.Parameters.AddWithValue("@p_ZONENEW", ZONENEW);
                cmdSave.Parameters.AddWithValue("@p_CIRCLENEW", CIRCLENEW);
                cmdSave.Parameters.AddWithValue("@p_SANCTIONORDER", dtSANCTIONORDER);

                cmdSave.Parameters.AddWithValue("@p_MODE", @p_strMode);
                cmdSave.Parameters.AddWithValue("@p_USER", strUser);
                cmdSave.Parameters.AddWithValue("@p_USERROLE", strUserRole);

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

        public void funcShow(string p_strNo, string p_strView, string p_strACCOUNTNAME, string p_strNAME, string p_strCBIRCNO1, string p_strSTATUS, string p_strPFNUMBER, string p_strBRANCH, string p_strCIRCLE)
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
                cmdView.CommandText = "[dbo].[spVigilanceMIS_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_VIEW", p_strView);
                cmdView.Parameters.AddWithValue("@p_ACCOUNTNAME", p_strACCOUNTNAME);
                cmdView.Parameters.AddWithValue("@p_NAME", p_strNAME);
                cmdView.Parameters.AddWithValue("@p_CBIRCNO1", p_strCBIRCNO1);
                cmdView.Parameters.AddWithValue("@p_STATUS", p_strSTATUS);
                cmdView.Parameters.AddWithValue("@p_PFNUMBER", p_strPFNUMBER);
                cmdView.Parameters.AddWithValue("@p_BRANCH", p_strBRANCH);
                cmdView.Parameters.AddWithValue("@p_CIRCLE", p_strCIRCLE);

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

            txtRNo.ToolTip = dtData.Rows[0]["CODE"].ToString();
            txtRNo.Text = dtData.Rows[0]["RNO"].ToString();
            txtBRComplaint.Text = dtData.Rows[0]["BRANCH"].ToString();
            txtName.Text = dtData.Rows[0]["NAME"].ToString();
            txtPFNo.Text = dtData.Rows[0]["PFNO"].ToString();
            txtSource.Text = dtData.Rows[0]["SOURCE"].ToString();
            txtAccountName.Text = dtData.Rows[0]["ACCOUNTNAME"].ToString();
            txtAmount.Text = dtData.Rows[0]["AMOUNT"].ToString();
            txtDesignation.Text = dtData.Rows[0]["DESIGNATION"].ToString();
            txtLapseNature.Text = dtData.Rows[0]["LAPSENATURE"].ToString();
            txtDARefNo.Text = dtData.Rows[0]["DAREFNO"].ToString();
            txtCbiRcNo1.Text = dtData.Rows[0]["CBIRCNO1"].ToString();
            txtConnectedVigCase.Text = dtData.Rows[0]["CONNECTEDVIGCASE"].ToString();
            txtNAPUNDA.Text = dtData.Rows[0]["NAPUNDA"].ToString();
            txtPDRefNo.Text = dtData.Rows[0]["PDREFNO"].ToString();

            txtChargeDate.Text = dtData.Rows[0]["CHARGEDATE"].ToString();
            txtRC1Date.Text = dtData.Rows[0]["RC1DATE"].ToString();
            txtRetirementDate.Text = dtData.Rows[0]["RETIREMENTDATE"].ToString();
            txtSuspensionDate.Text = dtData.Rows[0]["SUSPENSION"].ToString();
            txtOccurDate.Text = dtData.Rows[0]["OCCURDATE"].ToString();
            txtDAOrdDate.Text = dtData.Rows[0]["DAORDDATE"].ToString();
            txtRevocationDate.Text = dtData.Rows[0]["REVOCATIONDATE"].ToString();
            objCommonFunction.chkSetData(chkRNoDate, dtData.Rows[0]["CHK_RNODATE"].ToString());
            lblRNoDate.Text = dtData.Rows[0]["RNODATE"].ToString();

            objCommonFunction.ddlSetDataValue(ddlCircleOffice, dtData.Rows[0]["CIRCLEOFFICE"].ToString());
            hidCircleOffice.Value = dtData.Rows[0]["CIRCLEOFFICE"].ToString();
            objCommonFunction.ddlSetDataValue_Scale(ddlScale, dtData.Rows[0]["SCALE"].ToString());
            hidScale.Value = dtData.Rows[0]["SCALE"].ToString();
            objCommonFunction.ddlSetDataValue(ddlStatusCode, dtData.Rows[0]["STATUSCODE"].ToString());
            hidStatusCode.Value = dtData.Rows[0]["STATUSCODE"].ToString();
            objCommonFunction.ddlSetDataValue(ddlNatCHSheet, dtData.Rows[0]["NATCHSHEET"].ToString());
            hidNatCHSheet.Value = dtData.Rows[0]["NATCHSHEET"].ToString();
            objCommonFunction.ddlSetData(ddlFinal, dtData.Rows[0]["FINAL"].ToString(), true);
            hidFinal.Value = dtData.Rows[0]["FINAL"].ToString();
            objCommonFunction.ddlSetDataValue(ddlState, dtData.Rows[0]["STATE"].ToString());
            hidState.Value = dtData.Rows[0]["STATE"].ToString();
            objCommonFunction.ddlSetData(ddlUS, dtData.Rows[0]["US"].ToString(), true);
            hidUS.Value = dtData.Rows[0]["US"].ToString();
            hidRegister.Value = dtData.Rows[0]["REGISTER"].ToString();

            if (objCommonFunction.ddlSelectedValue(ddlStatusCode) == "0")
            {
                lblStatusCodeMIS.Text = dtData.Rows[0]["STATUSCODE"].ToString();
            }

            //objCommonFunction.ddlSetDataValue(ddlAccountNature, dtData.Rows[0]["ACCOUNTNATURE"].ToString());
            //hidNatureCase.Value = dtData.Rows[0]["ACCOUNTNATURE"].ToString();
            //if (objCommonFunction.ddlSelectedValue(ddlAccountNature) == "0")
            //{
            //    lblNatureMIS.Text = dtData.Rows[0]["ACCOUNTNATURE"].ToString();
            //}
            txtStatus.Text = dtData.Rows[0]["STATUS"].ToString();

            txtExternalSource.Text = Convert.ToString(dtData.Rows[0]["EXTERNAL_SOURCE"]);
            txtExternalSourceDate.Text = Convert.ToString(dtData.Rows[0]["EXTERNALSOURCEDATE"]);
            objCommonFunction.ddlSetDataValue(ddlBankName, Convert.ToString(dtData.Rows[0]["BANK_NAME"]));

            lblEntryBy.Text = dtData.Rows[0]["ENTRYBY"].ToString();
            lblEntryDate.Text = dtData.Rows[0]["ENTRYDATE"].ToString();
            lblModifyBy.Text = dtData.Rows[0]["MODIFYBY"].ToString();
            lblModifyDate.Text = dtData.Rows[0]["MODIFYDATE"].ToString();
            txtSanctionOrder.Text = Convert.ToString(dtData.Rows[0]["SANCTIONORDERDATE"]);

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
            txtRNo.ToolTip = string.Empty;
            txtRNo.Text = string.Empty;
            txtBRComplaint.Text = string.Empty;
            txtName.Text = string.Empty;
            txtPFNo.Text = string.Empty;
            txtSource.Text = string.Empty;
            txtAccountName.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            txtLapseNature.Text = string.Empty;
            txtDARefNo.Text = string.Empty;
            txtCbiRcNo1.Text = string.Empty;
            txtConnectedVigCase.Text = string.Empty;
            txtStatus.Text = string.Empty;
            txtHOStatus.Text = string.Empty;
            txtNAPUNDA.Text = string.Empty;
            txtPDRefNo.Text = string.Empty;

            txtRNoDate.Text = string.Empty;
            txtChargeDate.Text = string.Empty;
            txtRC1Date.Text = string.Empty;
            txtRetirementDate.Text = string.Empty;
            txtSuspensionDate.Text = string.Empty;
            txtOccurDate.Text = string.Empty;
            txtDAOrdDate.Text = string.Empty;
            txtRevocationDate.Text = string.Empty;

            chkRNoDate.Checked = false;
            lblRNoDate.Text = "";
            ddlCircleOffice.SelectedIndex = 0;
            ddlUS.SelectedIndex = 0;
            ddlStatusCode.SelectedIndex = 0;
            ddlScale.SelectedIndex = 0;
            //ddlAccountNature.SelectedIndex = 0;
            ddlNatCHSheet.SelectedIndex = 0;
            ddlFinal.SelectedIndex = 0;
            ddlState.SelectedIndex = 0;
            //ddlRegister.SelectedIndex = 0;

            hidCircleOffice.Value = "";
            hidNatCHSheet.Value = "";
            hidNatureCase.Value = "";
            hidScale.Value = "";
            hidUS.Value = "";
            hidScale.Value = "";
            hidRegister.Value = "";

            lblNatureMIS.Text = string.Empty;
            lblStatusCodeMIS.Text = string.Empty;
            lblEntryBy.Text = string.Empty;
            lblEntryDate.Text = string.Empty;
            lblModifyBy.Text = string.Empty;
            lblModifyDate.Text = string.Empty;

            txtExternalSource.Text = string.Empty;
            txtExternalSourceDate.Text = string.Empty;
            ddlBankName.SelectedIndex = 0;

            pnlHeader.Visible = false;
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;

            ddlLetterSentTo.SelectedIndex = 0;
            txtLetterSentDate.Text = "";
            txtReminderDate.Text = "";
            txtReplyReceivedDate.Text = "";
            hidLetterSentTo.Value = "";
            txtSanctionOrder.Text = "";

            ddlZoneNew.SelectedIndex = 0;
            if (ddlCircleNew.Items.Count > 0)
            {
                ddlCircleNew.Items.Clear();
            }

            funcControlsUserRights();
        }

        public void funcreadOnly()
        {
            objCommonFunction.disableControlsTextBox(txtBRComplaint);
            objCommonFunction.disableControlsTextBox(txtPDRefNo);
            objCommonFunction.disableControlsTextBox(txtName);
            objCommonFunction.disableControlsTextBox(txtPFNo);
            objCommonFunction.disableControlsTextBox(txtSource);
            objCommonFunction.disableControlsTextBox(txtAccountName);
            objCommonFunction.disableControlsTextBox(txtAmount);
            objCommonFunction.disableControlsTextBox(txtDesignation);
            objCommonFunction.disableControlsTextBox(txtLapseNature);
            objCommonFunction.disableControlsTextBox(txtDARefNo);
            objCommonFunction.disableControlsTextBox(txtCbiRcNo1);
            objCommonFunction.disableControlsTextBox(txtConnectedVigCase);
            objCommonFunction.disableControlsTextBox(txtStatus);
            objCommonFunction.disableControlsTextBox(txtNAPUNDA);

            txtChargeDate.Attributes.Add("readonly", "readonly");
            txtRNoDate.Attributes.Add("readonly", "readonly");
            txtRC1Date.Attributes.Add("readonly", "readonly");
            txtRetirementDate.Attributes.Add("readonly", "readonly");
            txtSuspensionDate.Attributes.Add("readonly", "readonly");
            txtDAOrdDate.Attributes.Add("readonly", "readonly");
            txtSanctionOrder.Attributes.Add("readonly", "readonly");

            objCommonFunction.disableControlsDropDownList(ddlCircleOffice);
            objCommonFunction.disableControlsDropDownList(ddlUS);
            objCommonFunction.disableControlsDropDownList(ddlStatusCode);
            objCommonFunction.disableControlsDropDownList(ddlAccountNature);
            objCommonFunction.disableControlsDropDownList(ddlScale);
            objCommonFunction.disableControlsDropDownList(ddlNatCHSheet);
            objCommonFunction.disableControlsDropDownList(ddlFinal);
            objCommonFunction.disableControlsDropDownList(ddlState);
            objCommonFunction.disableControlsDropDownList(ddlRegister);

            btnShowLapseNature_MODAL.Enabled = false;
            btnShowAccountName_MODAL.Enabled = false;
            btnShowStatus_MODAL.Enabled = false;

            #region ** readOnly Calenders Controls **
            ceRNoDate.Enabled = false;
            ceChargeDate.Enabled = false;
            ceRC1Date.Enabled = false;
            ceRetirementDate.Enabled = false;
            ceSuspensionDate.Enabled = false;
            ceOccurDate.Enabled = false;
            ceDAOrdDate.Enabled = false;
            ceRevocationDate.Enabled = false;
            ceSanctionOrder.Enabled = false;
            #endregion

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
                    txtRNo.Focus();
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
            strSearchNo = txtRNo.Text.Trim();
            funcShow(strSearchNo, "GET", null, null, null, null, null, null, null);
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void imgSearch_LIST_Click(object sender, ImageClickEventArgs e)
        {
            strSearchNo = txtRNo_LIST.Text.Trim();
            strACCOUNTNAME = txtAccountName_LIST.Text;
            strNAME = txtName_LIST.Text;
            strCBIRCNO1 = txtCBIRCNO_LIST.Text.Trim();
            strSTATUS = txtStatus_LIST.Text.Trim();
            strPFNO = txtPFNumber_LIST.Text.Trim();
            strBRCOMPLAINT = txtBranch_LIST.Text.Trim();
            strCIRCLEOFFICE = txtCircle_LIST.Text.Trim();
            strVIEW = "SEARCH";

            if (strSearchNo == "" && strACCOUNTNAME == "" && strNAME == "" && strCBIRCNO1 == "" && strSTATUS == "" && strPFNO == "" && strBRCOMPLAINT == "" && strCIRCLEOFFICE == "")
            {
                strVIEW = "LIST";
            }
            funcShow(strSearchNo, strVIEW, strACCOUNTNAME, strNAME, strCBIRCNO1, strSTATUS, strPFNO, strBRCOMPLAINT, strCIRCLEOFFICE);
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
                        funcShow(strRNO, "VIEW", null, null, null, null, null, null, null);
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
                funcShow(null, "LIST", null, null, null, null, null, null, null); //for bind grid view on List Tab Load
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
        protected void btnShowLapseNature_MODAL_Click(object sender, EventArgs e)
        {
            txtLapseNature_MODAL.Text = "";
            txtLapseNature_MODAL.Text = txtLapseNature.Text;
            modalPopUp_LapseNature.Show();
            txtLapseNature.Text = "";
        }

        protected void btnCloseMODAL_LapseNature_Click(object sender, EventArgs e)
        {
            txtLapseNature.Text = "";
            txtLapseNature.Text = txtLapseNature_MODAL.Text;
            objCommonFunction.removeTextBoxFirstComma(txtLapseNature);
            modalPopUp_LapseNature.Hide();
            txtLapseNature_MODAL.Text = "";
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