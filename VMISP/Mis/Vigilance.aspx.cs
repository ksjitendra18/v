using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class Vigilance : System.Web.UI.Page
    {
        DateTime? dtRECDATECOMP = null;
        DateTime? dtRNODATE = null;
        DateTime? dtCHARGEDATE = null;
        DateTime? dtRC1DATE = null;
        DateTime? dtRETIREMENTDATE = null;
        DateTime? dtSUSPENSION = null;
        DateTime? dtRECREPORTDATE = null;
        DateTime? dtOCCURDATE = null;
        DateTime? dtOMCVCDATE = null;
        DateTime? dtFIRDATE = null;
        DateTime? dtAPPPODATE = null;
        DateTime? dtAPPEODATE = null;
        DateTime? dtLASTRHDATE = null;
        DateTime? dtFINALDATE = null;
        DateTime? dtCONENQDATE = null;
        DateTime? dtBASICDATE = null;
        DateTime? dtAPPCDIDATE = null;
        DateTime? dtCVO2ADVICEDATE = null;
        DateTime? dtADVICESENTTODADATE = null;
        DateTime? dtWRITTENBRIEFCODATE = null;
        DateTime? dt2NDDADATE = null;
        DateTime? dtDAORDDATE = null;
        DateTime? dtREGULATDATE = null;
        DateTime? dtISTDADATE = null;
        DateTime? dtREVOCATIONDATE = null;
        DateTime? dtREVIEWDATE = null;
        DateTime? dtCLOSUREDATE = null;
        DateTime? dtCVOADVICEDATE = null;
        DateTime? dtREFERTOCVCDATE = null;
        DateTime? dtRC2DATE = null;
        DateTime? dtCOMMITMENTDATE = null;
        DateTime? dtCHSHEETFILEDDATE = null;
        DateTime? dtCOREPLYDATE = null;
        DateTime? dtTARGETDATE = null;
        DateTime? dtPLACEINPRESENTSCALEDATE = null;
        DateTime? dtSANCTIONORDERDATE = null;
        DateTime? dtERCODATE = null;
        DateTime? dtCVC2REF = null;
        DateTime? dtRECCVC2 = null;
        DateTime? dtAPPEAL = null;
        DateTime? dtPRELIMENQ = null;
        DateTime? dtREGUENQ = null;
        DateTime? dtWRITTENBRIEFPO = null;
        DateTime? dtSANCTIONREFUSED = null;
        DateTime? dtSANCTIONRECIVED = null;
        DateTime? dtCSOREPDATE = null;
        DateTime? dtA1CSCVC = null;
        DateTime? dtA1EOPOCVC = null;
        DateTime? dtA2FOCVC = null;
        DateTime? dtLETTERSENTDATE = null;
        DateTime? dtREMINDERDATE = null;
        DateTime? dtREPLYRECEIVEDDATE = null;
        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                funcShow(null, "LIST", null, null, null, null, null, null, null, null); //for bind grid view on form Load
                funcbindDropdown();     //Bind All DropDown Lists
            }

            lblMsg.Text = string.Empty;
            funcControlsUserRights();

            #region ** JS Function  **
            btnSubmit.Attributes.Add("onclick", "return funcValidation_Vigilance('" + txtRNo.ClientID + "','" + txtName.ClientID + "','" + txtPFNo.ClientID + "','" + ddlDisAuthoritysCircle.ClientID + "','" + chkRNoDate.ClientID + "','" + ddlScale.ClientID + "','" + ddlStatusCode.ClientID + "','" + txtDAOrdDate.ClientID + "','" + ddlPenaltyType.ClientID + "','" + txtNAPUNDA.ClientID + "','" + ddlRegister.ClientID + "','" + ddlCircleOffice.ClientID + "','" + ddlPenaltyProceedings.ClientID + "')");
            btnUpdate.Attributes.Add("onclick", "return funcValidation_Vigilance('" + txtRNo.ClientID + "','" + txtName.ClientID + "','" + txtPFNo.ClientID + "','" + ddlDisAuthoritysCircle.ClientID + "','" + chkRNoDate.ClientID + "','" + ddlScale.ClientID + "','" + ddlStatusCode.ClientID + "','" + txtDAOrdDate.ClientID + "','" + ddlPenaltyType.ClientID + "','" + txtNAPUNDA.ClientID + "','" + ddlRegister.ClientID + "','" + ddlCircleOffice.ClientID + "','" + ddlPenaltyProceedings.ClientID + "')");

            txtAmount.Attributes.Add("onkeypress", "return isNumbericDecimal(event,'" + txtAmount.ClientID + "')");
            txtLodiNo.Attributes.Add("onkeypress", "return isNumberic(event,'" + txtLodiNo.ClientID + "')");

            txtChargeDate.Attributes.Add("onblur", "return checkDate('" + txtChargeDate.ClientID + "')");
            txtRNoDate.Attributes.Add("onblur", "return checkDate('" + txtRNoDate.ClientID + "')");
            txtIstDaDate.Attributes.Add("onblur", "return checkDate('" + txtIstDaDate.ClientID + "')");
            txtCVOAdviceDate.Attributes.Add("onblur", "return checkDate('" + txtCVOAdviceDate.ClientID + "')");
            txtCVO2AdviceDate.Attributes.Add("onblur", "return checkDate('" + txtCVO2AdviceDate.ClientID + "')");
            txtPlaceinPresentScaleDate.Attributes.Add("onblur", "return checkDate('" + txtPlaceinPresentScaleDate.ClientID + "')");
            txtCompRecDate.Attributes.Add("onblur", "return checkDate('" + txtCompRecDate.ClientID + "')");
            txtAppEODate.Attributes.Add("onblur", "return checkDate('" + txtAppEODate.ClientID + "')");
            txtSanctionOrderDate.Attributes.Add("onblur", "return checkDate('" + txtSanctionOrderDate.ClientID + "')");
            txtCVC2Ref.Attributes.Add("onblur", "return checkDate('" + txtCVC2Ref.ClientID + "')");
            txtReferToCVCDate.Attributes.Add("onblur", "return checkDate('" + txtReferToCVCDate.ClientID + "')");
            txt2ndDADate.Attributes.Add("onblur", "return checkDate('" + txt2ndDADate.ClientID + "')");
            txtSanctionRefusedDate.Attributes.Add("onblur", "return checkDate('" + txtSanctionRefusedDate.ClientID + "')");
            txtConEnqDate.Attributes.Add("onblur", "return checkDate('" + txtConEnqDate.ClientID + "')");
            txtRC1Date.Attributes.Add("onblur", "return checkDate('" + txtRC1Date.ClientID + "')");
            txtOMCVCDate.Attributes.Add("onblur", "return checkDate('" + txtOMCVCDate.ClientID + "')");
            txtAppPODate.Attributes.Add("onblur", "return checkDate('" + txtAppPODate.ClientID + "')");
            txtRecCVC2.Attributes.Add("onblur", "return checkDate('" + txtRecCVC2.ClientID + "')");
            txtReviewDate.Attributes.Add("onblur", "return checkDate('" + txtReviewDate.ClientID + "')");
            txtRetirementDate.Attributes.Add("onblur", "return checkDate('" + txtRetirementDate.ClientID + "')");
            txtFinalDate.Attributes.Add("onblur", "return checkDate('" + txtFinalDate.ClientID + "')");
            txtSuspensionDate.Attributes.Add("onblur", "return checkDate('" + txtSuspensionDate.ClientID + "')");
            txtDAOrdDate.Attributes.Add("onblur", "return checkDate('" + txtDAOrdDate.ClientID + "')");
            txtRC2Date.Attributes.Add("onblur", "return checkDate('" + txtRC2Date.ClientID + "')");
            txtCSOREPDate.Attributes.Add("onblur", "return checkDate('" + txtCSOREPDate.ClientID + "')");
            txtA1CSCVC.Attributes.Add("onblur", "return checkDate('" + txtA1CSCVC.ClientID + "')");
            txtA1EOPOCVC.Attributes.Add("onblur", "return checkDate('" + txtA1EOPOCVC.ClientID + "')");
            txtA2FOCVC.Attributes.Add("onblur", "return checkDate('" + txtA2FOCVC.ClientID + "')");
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
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spVigilance_Ddl]";
                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlCircleOffice, ds.Tables[0]);
                    objCommonFunction.bindDropdownList(ddlDisAuthoritysCircle, ds.Tables[0]);
                    objCommonFunction.bindDropdownList(ddlState, ds.Tables[1]);
                    objCommonFunction.bindDropdownList(ddlZone, ds.Tables[2]);
                    objCommonFunction.bindDropdownList_SELECT(ddlScale, ds.Tables[3]);
                    objCommonFunction.bindDropdownList(ddlLetterSentTo, ds.Tables[4]);
                    objCommonFunction.bindDropdownList(ddlStatusCode, ds.Tables[5]);
                    objCommonFunction.bindDropdownList(ddlNature, ds.Tables[6]);
                    objCommonFunction.bindDropdownList(ddlPenaltyType, ds.Tables[7]);
                    objCommonFunction.bindDropdownList(ddlRegister, ds.Tables[8]);
                    objCommonFunction.bindDropdownList(ddlPenaltyProceedings, ds.Tables[9]);
                    objCommonFunction.bindDropdownList(ddlZoneNew, ds.Tables[10]);
                }
            }

            catch (Exception es)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }

            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public void funcSave(string MODE)
        {
            SqlConnection conSave = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();

            try
            {
                string strCLOSURE = objCommonFunction.chkSelected(chkClosureDate);
                if (lblClosureDate.Text.ToString() != "")
                {
                    strCLOSURE = "N";
                    txtClosureDate.Text = lblClosureDate.Text;
                    string strCLOSUREDATE = txtClosureDate.Text.Trim();
                    if (!string.IsNullOrEmpty(strCLOSUREDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strCLOSUREDATE, out date))
                            dtCLOSUREDATE = date;
                    }
                }

                //for R NO Date
                string strRNODATE = objCommonFunction.chkSelected(chkRNoDate);
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

                #region ** convert Date **
                string strRECDATECOMP = txtCompRecDate.Text.Trim();
                if (!string.IsNullOrEmpty(strRECDATECOMP))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRECDATECOMP, out date))
                        dtRECDATECOMP = date;
                }

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

                string strRECREPORTDATE = txtRecReportDate.Text.Trim();
                if (!string.IsNullOrEmpty(strRECREPORTDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRECREPORTDATE, out date))
                        dtRECREPORTDATE = date;
                }

                string strOCCURDATE = txtOccurDate.Text.Trim();
                if (!string.IsNullOrEmpty(strOCCURDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strOCCURDATE, out date))
                        dtOCCURDATE = date;
                }

                string strOMCVCDATE = txtOMCVCDate.Text.Trim();
                if (!string.IsNullOrEmpty(strOMCVCDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strOMCVCDATE, out date))
                        dtOMCVCDATE = date;
                }

                string strFIRDATE = txtFIRDate.Text.Trim();
                if (!string.IsNullOrEmpty(strFIRDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strFIRDATE, out date))
                        dtFIRDATE = date;
                }

                string strAPPPODATE = txtAppPODate.Text.Trim();
                if (!string.IsNullOrEmpty(strAPPPODATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strAPPPODATE, out date))
                        dtAPPPODATE = date;
                }

                string strAPPEODATE = txtAppEODate.Text.Trim();
                if (!string.IsNullOrEmpty(strAPPEODATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strAPPEODATE, out date))
                        dtAPPEODATE = date;
                }

                string strLASTRHDATE = txtLastRHDate.Text.Trim();
                if (!string.IsNullOrEmpty(strLASTRHDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strLASTRHDATE, out date))
                        dtLASTRHDATE = date;
                }

                string strFINALDATE = txtFinalDate.Text.Trim();
                if (!string.IsNullOrEmpty(strFINALDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strFINALDATE, out date))
                        dtFINALDATE = date;
                }

                string strCONENQDATE = txtConEnqDate.Text.Trim();
                if (!string.IsNullOrEmpty(strCONENQDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strCONENQDATE, out date))
                        dtCONENQDATE = date;
                }

                string strBASICDATE = txtBasicPayDate.Text.Trim();
                if (!string.IsNullOrEmpty(strBASICDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strBASICDATE, out date))
                        dtBASICDATE = date;
                }

                string strAPPCDIDATE = txtAppCDIDate.Text.Trim();
                if (!string.IsNullOrEmpty(strAPPCDIDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strAPPCDIDATE, out date))
                        dtAPPCDIDATE = date;
                }

                string strCVO2ADVICEDATE = txtCVO2AdviceDate.Text.Trim();
                if (!string.IsNullOrEmpty(strCVO2ADVICEDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strCVO2ADVICEDATE, out date))
                        dtCVO2ADVICEDATE = date;
                }

                string strADVICESENTTODADATE = txtAdviceSentToDADate.Text.Trim();
                if (!string.IsNullOrEmpty(strADVICESENTTODADATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strADVICESENTTODADATE, out date))
                        dtADVICESENTTODADATE = date;
                }

                string strWRITTENBRIEFCODATE = txtWrittenBriefCODate.Text.Trim();
                if (!string.IsNullOrEmpty(strWRITTENBRIEFCODATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strWRITTENBRIEFCODATE, out date))
                        dtWRITTENBRIEFCODATE = date;
                }

                string str2NDDADATE = txt2ndDADate.Text.Trim();
                if (!string.IsNullOrEmpty(str2NDDADATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(str2NDDADATE, out date))
                        dt2NDDADATE = date;
                }

                string strDAORDDATE = txtDAOrdDate.Text.Trim();
                if (!string.IsNullOrEmpty(strDAORDDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strDAORDDATE, out date))
                        dtDAORDDATE = date;
                }

                string strREGULATDATE = txtRegulatDate.Text.Trim();
                if (!string.IsNullOrEmpty(strREGULATDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strREGULATDATE, out date))
                        dtREGULATDATE = date;
                }

                string strISTDADATE = txtIstDaDate.Text.Trim();
                if (!string.IsNullOrEmpty(strISTDADATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strISTDADATE, out date))
                        dtISTDADATE = date;
                }

                string strREVOCATIONDATE = txtRevocationDate.Text.Trim();
                if (!string.IsNullOrEmpty(strREVOCATIONDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strREVOCATIONDATE, out date))
                        dtREVOCATIONDATE = date;
                }

                string strREVIEWDATE = txtReviewDate.Text.Trim();
                if (!string.IsNullOrEmpty(strREVIEWDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strREVIEWDATE, out date))
                        dtREVIEWDATE = date;
                }

                string strCVOADVICEDATE = txtCVOAdviceDate.Text.Trim();
                if (!string.IsNullOrEmpty(strCVOADVICEDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strCVOADVICEDATE, out date))
                        dtCVOADVICEDATE = date;
                }

                string strREFERTOCVCDATE = txtReferToCVCDate.Text.Trim();
                if (!string.IsNullOrEmpty(strREFERTOCVCDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strREFERTOCVCDATE, out date))
                        dtREFERTOCVCDATE = date;
                }

                string strRC2DATE = txtRC2Date.Text.Trim();
                if (!string.IsNullOrEmpty(strRC2DATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRC2DATE, out date))
                        dtRC2DATE = date;
                }

                string strCOMMITMENTDATE = txtCommitmentDate.Text.Trim();
                if (!string.IsNullOrEmpty(strCOMMITMENTDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strCOMMITMENTDATE, out date))
                        dtCOMMITMENTDATE = date;
                }

                string strCHSHEETFILEDDATE = txtCHSheetFiledDate.Text.Trim();
                if (!string.IsNullOrEmpty(strCHSHEETFILEDDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strCHSHEETFILEDDATE, out date))
                        dtCHSHEETFILEDDATE = date;
                }

                string strCOREPLYDATE = txtCOReplyDate.Text.Trim();
                if (!string.IsNullOrEmpty(strCOREPLYDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strCOREPLYDATE, out date))
                        dtCOREPLYDATE = date;
                }

                string strTARGETDATE = txtTargetDate.Text.Trim();
                if (!string.IsNullOrEmpty(strTARGETDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strTARGETDATE, out date))
                        dtTARGETDATE = date;
                }

                string strPLACEINPRESENTSCALEDATE = txtPlaceinPresentScaleDate.Text.Trim();
                if (!string.IsNullOrEmpty(strPLACEINPRESENTSCALEDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strPLACEINPRESENTSCALEDATE, out date))
                        dtPLACEINPRESENTSCALEDATE = date;
                }

                string strSANCTIONORDERDATE = txtSanctionOrderDate.Text.Trim();
                if (!string.IsNullOrEmpty(strSANCTIONORDERDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strSANCTIONORDERDATE, out date))
                        dtSANCTIONORDERDATE = date;
                }

                string strERCODATE = txtERCODate.Text.Trim();
                if (!string.IsNullOrEmpty(strERCODATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strERCODATE, out date))
                        dtERCODATE = date;
                }

                string strCVC2REF = txtCVC2Ref.Text.Trim();
                if (!string.IsNullOrEmpty(strCVC2REF))
                {
                    DateTime date;
                    if (DateTime.TryParse(strCVC2REF, out date))
                        dtCVC2REF = date;
                }

                string strRECCVC2 = txtRecCVC2.Text.Trim();
                if (!string.IsNullOrEmpty(strRECCVC2))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRECCVC2, out date))
                        dtRECCVC2 = date;
                }

                string strAPPEAL = txtAppeal.Text.Trim();
                if (!string.IsNullOrEmpty(strAPPEAL))
                {
                    DateTime date;
                    if (DateTime.TryParse(strAPPEAL, out date))
                        dtAPPEAL = date;
                }

                string strPRELIMENQ = txtPrelimEnq.Text.Trim();
                if (!string.IsNullOrEmpty(strPRELIMENQ))
                {
                    DateTime date;
                    if (DateTime.TryParse(strPRELIMENQ, out date))
                        dtPRELIMENQ = date;
                }

                string strREGUENQ = txtReguEnq.Text.Trim();
                if (!string.IsNullOrEmpty(strREGUENQ))
                {
                    DateTime date;
                    if (DateTime.TryParse(strREGUENQ, out date))
                        dtREGUENQ = date;
                }

                string strWRITTENBRIEFPO = txtWrittenBriefPO.Text.Trim();
                if (!string.IsNullOrEmpty(strWRITTENBRIEFPO))
                {
                    DateTime date;
                    if (DateTime.TryParse(strWRITTENBRIEFPO, out date))
                        dtWRITTENBRIEFPO = date;
                }

                string strSANCTIONRECIVED = txtSanctionRecvDate.Text.Trim();
                if (!string.IsNullOrEmpty(strSANCTIONRECIVED))
                {
                    DateTime date;
                    if (DateTime.TryParse(strSANCTIONRECIVED, out date))
                        dtSANCTIONRECIVED = date;
                }

                string strSANCTIONREFUSED = txtSanctionRefusedDate.Text.Trim();
                if (!string.IsNullOrEmpty(strSANCTIONREFUSED))
                {
                    DateTime date;
                    if (DateTime.TryParse(strSANCTIONREFUSED, out date))
                        dtSANCTIONREFUSED = date;
                }

                string strCSOREPDATE = txtCSOREPDate.Text.Trim();
                if (!string.IsNullOrEmpty(strCSOREPDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strCSOREPDATE, out date))
                        dtCSOREPDATE = date;
                }

                string strA1CSCVC = txtA1CSCVC.Text.Trim();
                if (!string.IsNullOrEmpty(strA1CSCVC))
                {
                    DateTime date;
                    if (DateTime.TryParse(strA1CSCVC, out date))
                        dtA1CSCVC = date;
                }

                string strA1EOPOCVC = txtA1EOPOCVC.Text.Trim();
                if (!string.IsNullOrEmpty(strA1EOPOCVC))
                {
                    DateTime date;
                    if (DateTime.TryParse(strA1EOPOCVC, out date))
                        dtA1EOPOCVC = date;
                }

                string strA2FOCVC = txtA2FOCVC.Text.Trim();
                if (!string.IsNullOrEmpty(strA2FOCVC))
                {
                    DateTime date;
                    if (DateTime.TryParse(strA2FOCVC, out date))
                        dtA2FOCVC = date;
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

                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spVigilance_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_CODE", objCommonFunction.convertToIntToolTip(txtRNo));
                cmdSave.Parameters.AddWithValue("@p_RNO", txtRNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BRCOMPLAINT", txtBRComplaint.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_RNO1", txtRNo1.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NAMEOFPARTICULARS", txtNameOfParticulars.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NAME", txtName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PFNO", txtPFNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PRESENTPOSTING", txtPresentPosting.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_SOURCE", txtSource.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ACCOUNTNAME", txtAccountName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_AMOUNT", objCommonFunction.convertToDecimal(txtAmount));
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", txtDesignation.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DREFNO", txtDRefNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_LAPSENATURE", txtLapseNature.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_REASONSFORINCLUSION", txtReasonsforInclusion.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DAREFNO", txtDARefNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DELETIONREASONS", txtDeletionReasons.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_US", txtUS.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_INVOFFICERNAME", txtInvOfficerName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_RECOMMOFCVC", txtRecommofCVC.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CBIRCNO1", txtCbiRcNo1.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_IRCBIPENDING", txtIRCBIPending.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CBIRECOM", txtCBIRecom.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_POLFIRNO", txtPolFirNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_RCSOURCE", txtRCSource.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_INVESTIG", txtInvestig.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CVCOMNO", txtCVCOMNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CVC2PROPOSED", txtCVC2Proposed.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ADV1AWAITED", txtADV1Awaited.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NATUREOFACCOUNT", txtNatureofAccount.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PONAME", txtPOName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_POCBI", txtPOCBI.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DTSHEAR", txtDTSHear.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PUNISHMENTPROPOSEDBYDA", txtPunishmentProposedbyDA.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PENALTY", txtPenalty.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DISPAUTHORITY", txtDispAuthority.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PREVCASEPUNISHMENT", txtPrevCasePunishment.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BASICPAY", txtBasicPay.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CVOADVICE", txtCVOAdvice.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_LODINO", txtLodiNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CONNECTEDVIGCASE", txtConnectedVigCase.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_FIELD1", txtField1.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_2DAPROPOSAL", txt2DAProposal.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_2NDPENDING", txt2ndPending.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_RECOM", txtReComp.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NOAWARDS", txtNoAwardS.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PROPOSEDACTIONTOCVC", txtProposedActiontoCVC.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_REGINVOK", txtRegInvok.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NAPUNDA", txtNAPUNDA.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CBIRCNO2", txtCBIRCNo2.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_EONAME", txtEOName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CDINAME", txtCDIName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ADV2AWT", txtAdv2Awt.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_STATUSINBRIEF", txtStatusinBrief.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_LODINEW", txtLodiNew.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_EOCDI", txtEoCdi.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DAPROPOSAL", txtDAProposal.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ISTPENDING", txtIstPending.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CVO2ADVICE", txtCVO2Advice.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CVCADVICEII", txtCVCAdbiceII.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_STATUS", txtStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_HOSTATUS", txtHOStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CHK_RNODATE", strRNODATE);
                cmdSave.Parameters.AddWithValue("@p_LODICODE", txtLodiCode.Text.Trim().Trim());
                cmdSave.Parameters.AddWithValue("@p_LODIINCLUSIONREASON", txtLodiInclusionReason.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_LODIDELETIONREASON", txtLodiDeletionReason.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DESK_USER_REMARKS", txtDealingOfficerRemarks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_TMSACREFNO", txtTMSACRefNo.Text.Trim());

                cmdSave.Parameters.AddWithValue("@p_RECDATECOMP", dtRECDATECOMP);
                cmdSave.Parameters.AddWithValue("@p_RNODATE", dtRNODATE);
                cmdSave.Parameters.AddWithValue("@p_CHARGEDATE", dtCHARGEDATE);
                cmdSave.Parameters.AddWithValue("@p_RC1DATE", dtRC1DATE);
                cmdSave.Parameters.AddWithValue("@p_RETIREMENTDATE", dtRETIREMENTDATE);
                cmdSave.Parameters.AddWithValue("@p_SUSPENSION", dtSUSPENSION);
                cmdSave.Parameters.AddWithValue("@p_RECREPORTDATE", dtRECREPORTDATE);
                cmdSave.Parameters.AddWithValue("@p_OCCURDATE", dtOCCURDATE);
                cmdSave.Parameters.AddWithValue("@p_OMCVCDATE", dtOMCVCDATE);
                cmdSave.Parameters.AddWithValue("@p_FIRDATE", dtFIRDATE);
                cmdSave.Parameters.AddWithValue("@p_APPPODATE", dtAPPPODATE);
                cmdSave.Parameters.AddWithValue("@p_APPEODATE", dtAPPEODATE);
                cmdSave.Parameters.AddWithValue("@p_LASTRHDATE", dtLASTRHDATE);
                cmdSave.Parameters.AddWithValue("@p_FINALDATE", dtFINALDATE);
                cmdSave.Parameters.AddWithValue("@p_CONENQDATE", dtCONENQDATE);
                cmdSave.Parameters.AddWithValue("@p_BASICDATE", dtBASICDATE);
                cmdSave.Parameters.AddWithValue("@p_APPCDIDATE", dtAPPCDIDATE);
                cmdSave.Parameters.AddWithValue("@p_CVO2ADVICEDATE", dtCVO2ADVICEDATE);
                cmdSave.Parameters.AddWithValue("@p_ADVICESENTTODADATE", dtADVICESENTTODADATE);
                cmdSave.Parameters.AddWithValue("@p_WRITTENBRIEFCODATE", dtWRITTENBRIEFCODATE);
                cmdSave.Parameters.AddWithValue("@p_2NDDADATE", dt2NDDADATE);
                cmdSave.Parameters.AddWithValue("@p_DAORDDATE", dtDAORDDATE);
                cmdSave.Parameters.AddWithValue("@p_REGULATDATE", dtREGULATDATE);
                cmdSave.Parameters.AddWithValue("@p_ISTDADATE", dtISTDADATE);
                cmdSave.Parameters.AddWithValue("@p_REVOCATIONDATE", dtREVOCATIONDATE);
                cmdSave.Parameters.AddWithValue("@p_REVIEWDATE", dtREVIEWDATE);
                cmdSave.Parameters.AddWithValue("@p_CLOSUREDATE", dtCLOSUREDATE);
                cmdSave.Parameters.AddWithValue("@p_CVOADVICEDATE", dtCVOADVICEDATE);
                cmdSave.Parameters.AddWithValue("@p_REFERTOCVCDATE", dtREFERTOCVCDATE);
                cmdSave.Parameters.AddWithValue("@p_RC2DATE", dtRC2DATE);
                cmdSave.Parameters.AddWithValue("@p_COMMITMENTDATE", dtCOMMITMENTDATE);
                cmdSave.Parameters.AddWithValue("@p_CHSHEETFILEDDATE", dtCHSHEETFILEDDATE);
                cmdSave.Parameters.AddWithValue("@p_COREPLYDATE", dtCOREPLYDATE);
                cmdSave.Parameters.AddWithValue("@p_TARGETDATE", dtTARGETDATE);
                cmdSave.Parameters.AddWithValue("@p_PLACEINPRESENTSCALEDATE", dtPLACEINPRESENTSCALEDATE);
                cmdSave.Parameters.AddWithValue("@p_SANCTIONORDERDATE", dtSANCTIONORDERDATE);
                cmdSave.Parameters.AddWithValue("@p_ERCODATE", dtERCODATE);
                cmdSave.Parameters.AddWithValue("@p_CVC2REF", dtCVC2REF);
                cmdSave.Parameters.AddWithValue("@p_RECCVC2", dtRECCVC2);
                cmdSave.Parameters.AddWithValue("@p_APPEAL", dtAPPEAL);
                cmdSave.Parameters.AddWithValue("@p_PRELIMENQ", dtPRELIMENQ);
                cmdSave.Parameters.AddWithValue("@p_REGUENQ", dtREGUENQ);
                cmdSave.Parameters.AddWithValue("@p_WRITTENBRIEFPO", dtWRITTENBRIEFPO);
                cmdSave.Parameters.AddWithValue("@p_SANCTIONRECIVED", dtSANCTIONRECIVED);
                cmdSave.Parameters.AddWithValue("@p_SANCTIONREFUSED", dtSANCTIONREFUSED);
                cmdSave.Parameters.AddWithValue("@p_CSOREPDATE", dtCSOREPDATE);
                cmdSave.Parameters.AddWithValue("@p_A1CSCVC", dtA1CSCVC);
                cmdSave.Parameters.AddWithValue("@p_A1EOPOCVC", dtA1EOPOCVC);
                cmdSave.Parameters.AddWithValue("@p_A2FOCVC", dtA2FOCVC);
                cmdSave.Parameters.AddWithValue("@p_CLOSURE", strCLOSURE);
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTDATE", dtLETTERSENTDATE);
                cmdSave.Parameters.AddWithValue("@p_REMINDERDATE", dtREMINDERDATE);
                cmdSave.Parameters.AddWithValue("@p_REPLYRECEIVEDDATE", dtREPLYRECEIVEDDATE);

                cmdSave.Parameters.AddWithValue("@p_REGISTER", objCommonFunction.ddlSelectedValue(ddlRegister));
                cmdSave.Parameters.AddWithValue("@p_BANKNAME", objCommonFunction.ddlSelectedValue(ddlBankName));
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTO", objCommonFunction.ddlSelectedValue(ddlLetterSentTo));
                cmdSave.Parameters.AddWithValue("@p_CIRCLEOFFICE", objCommonFunction.ddlSelectedText(ddlCircleOffice));
                cmdSave.Parameters.AddWithValue("@p_ZONENEW", objCommonFunction.ddlSelectedValue(ddlZoneNew));
                cmdSave.Parameters.AddWithValue("@p_CIRCLENEW", objCommonFunction.ddlSelectedValue(ddlCircleNew));
                cmdSave.Parameters.AddWithValue("@p_STATUSCODE", objCommonFunction.ddlSelectedValue(ddlStatusCode));
                cmdSave.Parameters.AddWithValue("@p_ZONE", objCommonFunction.ddlSelectedText(ddlZone));
                cmdSave.Parameters.AddWithValue("@p_NATURECASE", objCommonFunction.ddlSelectedValue(ddlNature));
                cmdSave.Parameters.AddWithValue("@p_SCALE", objCommonFunction.ddlSelectedValue_Scale(ddlScale));
                cmdSave.Parameters.AddWithValue("@p_FINAL", objCommonFunction.ddlSelectedText(ddlFinal));
                cmdSave.Parameters.AddWithValue("@p_DISAUTHORITYCIRCLE", objCommonFunction.ddlSelectedValue(ddlDisAuthoritysCircle));
                cmdSave.Parameters.AddWithValue("@p_LODICASE", objCommonFunction.ddlSelectedText(ddlLodiCase));
                cmdSave.Parameters.AddWithValue("@p_STATE", objCommonFunction.ddlSelectedText(ddlState));
                cmdSave.Parameters.AddWithValue("@p_NATCHSHEET", objCommonFunction.ddlSelectedValue(ddlNatCHSheet));
                cmdSave.Parameters.AddWithValue("@p_PENALTYTYPE", objCommonFunction.ddlSelectedText(ddlPenaltyType));
                cmdSave.Parameters.AddWithValue("@p_PENALTYPROCEEDINGS", objCommonFunction.convertToInt(objCommonFunction.ddlSelectedValue(ddlPenaltyProceedings)));

                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["userid"]));
                cmdSave.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                cmdSave.CommandTimeout = 0;

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
                    funcClear();
                }
                else
                {
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
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

        public void funcShow(string p_strNo, string p_strView, string p_strACCOUNTNAME, string p_strNAME, string p_strCBIRCNO1, string p_strCVCOMNO, string p_strSTATUS, string p_strPFNUMBER, string p_strBRANCH, string p_strCIRCLE)
        {
            SqlConnection conView = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdView = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {

                conView.Open();
                cmdView.Connection = conView;
                cmdView.Parameters.Clear();
                cmdView.CommandType = CommandType.StoredProcedure;
                cmdView.CommandText = "[dbo].[spVigilance_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_VIEW", p_strView);
                cmdView.Parameters.AddWithValue("@p_ACCOUNTNAME", p_strACCOUNTNAME);
                cmdView.Parameters.AddWithValue("@p_NAME", p_strNAME);
                cmdView.Parameters.AddWithValue("@p_CBIRCNO1", p_strCBIRCNO1);
                cmdView.Parameters.AddWithValue("@p_CVCOMNO", p_strCVCOMNO);
                cmdView.Parameters.AddWithValue("@p_STATUS", p_strSTATUS);
                cmdView.Parameters.AddWithValue("@p_PFNUMBER", p_strPFNUMBER);
                cmdView.Parameters.AddWithValue("@p_BRANCH", p_strBRANCH);
                cmdView.Parameters.AddWithValue("@p_CIRCLE", p_strCIRCLE);

                cmdView.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmdView);
                sda.Fill(dt);
                ViewState["DETAILDATA"] = dt;

                if (Convert.ToInt32(sqlErrCodeOutput.Value) >= 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (p_strView.ToUpper() == "LIST")
                        {
                            gvMain.DataSource = dt;
                            gvMain.DataBind();
                        }
                        else if (p_strView.ToUpper() == "SEARCH")
                        {
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
                    lblMsg.Text = "Record npt found";
                    funcClear();
                }
            }

            catch (Exception es)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }
            finally
            {
                cmdView.Dispose();
                conView.Dispose();
                conView.Close();
            }
        }

        public void funcBindControl(DataTable dtData)
        {
            tabMain.ActiveTabIndex = 0;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;

            txtRNo.ToolTip = Convert.ToString(dtData.Rows[0]["CODE"]);
            txtRNo.Text = Convert.ToString(dtData.Rows[0]["RNO"]);
            txtBRComplaint.Text = Convert.ToString(dtData.Rows[0]["BRCOMPLAINT"]);
            txtRNo1.Text = Convert.ToString(dtData.Rows[0]["RNO1"]);
            txtNameOfParticulars.Text = Convert.ToString(dtData.Rows[0]["NAMEOFPARTICULARS"]);
            txtName.Text = Convert.ToString(dtData.Rows[0]["NAME"]);
            txtPFNo.Text = Convert.ToString(dtData.Rows[0]["PFNO"]);
            txtPresentPosting.Text = Convert.ToString(dtData.Rows[0]["PRESENTPOSTING"]);
            txtSource.Text = Convert.ToString(dtData.Rows[0]["SOURCE"]);
            txtAccountName.Text = Convert.ToString(dtData.Rows[0]["ACCOUNTNAME"]);
            txtAmount.Text = Convert.ToString(dtData.Rows[0]["AMOUNT"]);
            txtDesignation.Text = Convert.ToString(dtData.Rows[0]["DESIGNATION"]);
            txtDRefNo.Text = Convert.ToString(dtData.Rows[0]["DREFNO"]);
            txtLapseNature.Text = Convert.ToString(dtData.Rows[0]["LAPSENATURE"]);
            txtReasonsforInclusion.Text = Convert.ToString(dtData.Rows[0]["REASONSFORINCLUSION"]);
            txtDARefNo.Text = Convert.ToString(dtData.Rows[0]["DAREFNO"]);
            txtDeletionReasons.Text = Convert.ToString(dtData.Rows[0]["DELETIONREASONS"]);
            txtUS.Text = Convert.ToString(dtData.Rows[0]["US"]);
            txtInvOfficerName.Text = Convert.ToString(dtData.Rows[0]["INVOFFICERNAME"]);
            txtRecommofCVC.Text = Convert.ToString(dtData.Rows[0]["RECOMMOFCVC"]);
            txtCbiRcNo1.Text = Convert.ToString(dtData.Rows[0]["CBIRCNO1"]);
            txtIRCBIPending.Text = Convert.ToString(dtData.Rows[0]["IRCBIPENDING"]);
            txtCBIRecom.Text = Convert.ToString(dtData.Rows[0]["CBIRECOM"]);
            txtPolFirNo.Text = Convert.ToString(dtData.Rows[0]["POLFIRNO"]);
            txtRCSource.Text = Convert.ToString(dtData.Rows[0]["RCSOURCE"]);
            txtInvestig.Text = Convert.ToString(dtData.Rows[0]["INVESTIG"]);
            txtCVCOMNo.Text = Convert.ToString(dtData.Rows[0]["CVCOMNO"]);
            txtCVC2Proposed.Text = Convert.ToString(dtData.Rows[0]["CVC2PROPOSED"]);
            txtADV1Awaited.Text = Convert.ToString(dtData.Rows[0]["ADV1AWAITED"]);
            txtNatureofAccount.Text = Convert.ToString(dtData.Rows[0]["NATUREOFACCOUNT"]);
            txtPOName.Text = Convert.ToString(dtData.Rows[0]["PONAME"]);
            txtPOCBI.Text = Convert.ToString(dtData.Rows[0]["POCBI"]);
            txtDTSHear.Text = Convert.ToString(dtData.Rows[0]["DTSHEAR"]);
            txtPunishmentProposedbyDA.Text = Convert.ToString(dtData.Rows[0]["PUNISHMENTPROPOSEDBYDA"]);
            txtPenalty.Text = Convert.ToString(dtData.Rows[0]["PENALTY"]);
            txtDispAuthority.Text = Convert.ToString(dtData.Rows[0]["DISPAUTHORITY"]);
            txtPrevCasePunishment.Text = Convert.ToString(dtData.Rows[0]["PREVCASEPUNISHMENT"]);
            txtWrittenBriefPO.Text = Convert.ToString(dtData.Rows[0]["WRITTENBRIEFPO"]);
            txtBasicPay.Text = Convert.ToString(dtData.Rows[0]["BASICPAY"]);
            txtCVOAdvice.Text = Convert.ToString(dtData.Rows[0]["CVOADVICE"]);
            txtLodiNo.Text = Convert.ToString(dtData.Rows[0]["LODINO"]);
            txtConnectedVigCase.Text = Convert.ToString(dtData.Rows[0]["CONNECTEDVIGCASE"]);
            txtField1.Text = Convert.ToString(dtData.Rows[0]["FIELD1"]);
            txt2DAProposal.Text = Convert.ToString(dtData.Rows[0]["DAPROPOSAL2"]);
            txt2ndPending.Text = Convert.ToString(dtData.Rows[0]["PENDING2ND"]);
            txtReComp.Text = Convert.ToString(dtData.Rows[0]["RECOM"]);
            txtNoAwardS.Text = Convert.ToString(dtData.Rows[0]["NOAWARDS"]);
            txtProposedActiontoCVC.Text = Convert.ToString(dtData.Rows[0]["PROPOSEDACTIONTOCVC"]);
            txtRegInvok.Text = Convert.ToString(dtData.Rows[0]["REGINVOK"]);
            txtCBIRCNo2.Text = Convert.ToString(dtData.Rows[0]["CBIRCNO2"]);
            txtEOName.Text = Convert.ToString(dtData.Rows[0]["EONAME"]);
            txtCDIName.Text = Convert.ToString(dtData.Rows[0]["CDINAME"]);
            txtAdv2Awt.Text = Convert.ToString(dtData.Rows[0]["ADV2AWT"]);
            txtStatusinBrief.Text = Convert.ToString(dtData.Rows[0]["STATUSINBRIEF"]);
            txtLodiNew.Text = Convert.ToString(dtData.Rows[0]["LODINEW"]);
            txtEoCdi.Text = Convert.ToString(dtData.Rows[0]["EOCDI"]);
            txtDAProposal.Text = Convert.ToString(dtData.Rows[0]["DAPROPOSAL"]);
            txtIstPending.Text = Convert.ToString(dtData.Rows[0]["ISTPENDING"]);
            txtCVCAdbiceII.Text = Convert.ToString(dtData.Rows[0]["CVCADVICEII"]);
            txtNAPUNDA.Text = Convert.ToString(dtData.Rows[0]["NAPUNDA"]);
            txtLodiCode.Text = Convert.ToString(dtData.Rows[0]["LODICODE"]);
            txtLodiInclusionReason.Text = Convert.ToString(dtData.Rows[0]["LODIINCLUSIONREASON"]);
            txtLodiDeletionReason.Text = Convert.ToString(dtData.Rows[0]["LODIDELETIONREASON"]);

            txtCompRecDate.Text = Convert.ToString(dtData.Rows[0]["RECDATECOMP"]);
            txtChargeDate.Text = Convert.ToString(dtData.Rows[0]["CHARGEDATE"]);
            txtRC1Date.Text = Convert.ToString(dtData.Rows[0]["RC1DATE"]);
            txtRetirementDate.Text = Convert.ToString(dtData.Rows[0]["RETIREMENTDATE"]);
            txtSuspensionDate.Text = Convert.ToString(dtData.Rows[0]["SUSPENSION"]);
            txtRecReportDate.Text = Convert.ToString(dtData.Rows[0]["RECREPORTDATE"]);
            txtOccurDate.Text = Convert.ToString(dtData.Rows[0]["OCCURDATE"]);
            txtOMCVCDate.Text = Convert.ToString(dtData.Rows[0]["OMCVCDATE"]);
            txtFIRDate.Text = Convert.ToString(dtData.Rows[0]["FIRDATE"]);
            txtAppPODate.Text = Convert.ToString(dtData.Rows[0]["APPPODATE"]);
            txtAppEODate.Text = Convert.ToString(dtData.Rows[0]["APPEODATE"]);
            txtLastRHDate.Text = Convert.ToString(dtData.Rows[0]["LASTRHDATE"]);
            txtFinalDate.Text = Convert.ToString(dtData.Rows[0]["FINALDATE"]);
            txtConEnqDate.Text = Convert.ToString(dtData.Rows[0]["CONENQDATE"]);
            txtBasicPayDate.Text = Convert.ToString(dtData.Rows[0]["BASICDATE"]);
            txtAppCDIDate.Text = Convert.ToString(dtData.Rows[0]["APPCDIDATE"]);
            txtCVO2AdviceDate.Text = Convert.ToString(dtData.Rows[0]["CVO2ADVICEDATE"]);
            txtAdviceSentToDADate.Text = Convert.ToString(dtData.Rows[0]["ADVICESENTTODADATE"]);
            txtWrittenBriefCODate.Text = Convert.ToString(dtData.Rows[0]["WRITTENBRIEFCODATE"]);
            txt2ndDADate.Text = Convert.ToString(dtData.Rows[0]["DA2NDDATE"]);
            txtDAOrdDate.Text = Convert.ToString(dtData.Rows[0]["DAORDDATE"]);
            txtRegulatDate.Text = Convert.ToString(dtData.Rows[0]["REGULATDATE"]);
            txtIstDaDate.Text = Convert.ToString(dtData.Rows[0]["ISTDADATE"]);
            txtRevocationDate.Text = Convert.ToString(dtData.Rows[0]["REVOCATIONDATE"]);
            txtReviewDate.Text = Convert.ToString(dtData.Rows[0]["REVIEWDATE"]);
            txtCVOAdviceDate.Text = Convert.ToString(dtData.Rows[0]["CVOADVICEDATE"]);
            txtReferToCVCDate.Text = Convert.ToString(dtData.Rows[0]["REFERTOCVCDATE"]);
            txtRC2Date.Text = Convert.ToString(dtData.Rows[0]["RC2DATE"]);
            txtCommitmentDate.Text = Convert.ToString(dtData.Rows[0]["COMMITMENTDATE"]);
            txtCHSheetFiledDate.Text = Convert.ToString(dtData.Rows[0]["CHSHEETFILEDDATE"]);
            txtCOReplyDate.Text = Convert.ToString(dtData.Rows[0]["COREPLYDATE"]);
            txtTargetDate.Text = Convert.ToString(dtData.Rows[0]["TARGETDATE"]);
            txtPlaceinPresentScaleDate.Text = Convert.ToString(dtData.Rows[0]["PLACEINPRESENTSCALEDATE"]);
            txtSanctionOrderDate.Text = Convert.ToString(dtData.Rows[0]["SANCTIONORDERDATE"]);
            txtERCODate.Text = Convert.ToString(dtData.Rows[0]["ERCODATE"]);
            txtRecCVC2.Text = Convert.ToString(dtData.Rows[0]["RECCVC2"]);
            txtCVC2Ref.Text = Convert.ToString(dtData.Rows[0]["CVC2REF"]);
            txtAppeal.Text = Convert.ToString(dtData.Rows[0]["APPEAL"]);
            txtPrelimEnq.Text = Convert.ToString(dtData.Rows[0]["PRELIMENQ"]);
            txtReguEnq.Text = Convert.ToString(dtData.Rows[0]["REGUENQ"]);
            txtSanctionRecvDate.Text = Convert.ToString(dtData.Rows[0]["SANCTIONRECIVED"]);
            txtSanctionRefusedDate.Text = Convert.ToString(dtData.Rows[0]["SANCTIONREFUSED"]);
            txtCSOREPDate.Text = Convert.ToString(dtData.Rows[0]["CSOREPDATE"]);
            txtA1CSCVC.Text = Convert.ToString(dtData.Rows[0]["A1CSCVC"]);
            txtA1EOPOCVC.Text = Convert.ToString(dtData.Rows[0]["A1EOPOCVC"]);
            txtA2FOCVC.Text = Convert.ToString(dtData.Rows[0]["A2FOCVC"]);
            txtCVO2Advice.Text = Convert.ToString(dtData.Rows[0]["CVO2ADVICE"]);

            objCommonFunction.chkSetData(chkClosureDate, Convert.ToString(dtData.Rows[0]["CLOSURE"]));
            lblClosureDate.Text = Convert.ToString(dtData.Rows[0]["CLOSUREDATE"]);
            objCommonFunction.chkSetData(chkRNoDate, Convert.ToString(dtData.Rows[0]["CHK_RNODATE"]));
            lblRNoDate.Text = Convert.ToString(dtData.Rows[0]["RNODATE"]);

            objCommonFunction.ddlSetData(ddlCircleOffice, Convert.ToString(dtData.Rows[0]["CIRCLEOFFICE"]), true);
            objCommonFunction.ddlSetData(ddlZone, Convert.ToString(dtData.Rows[0]["ZONE"]), true);
            objCommonFunction.ddlSetDataValue_Scale(ddlScale, Convert.ToString(dtData.Rows[0]["SCALE"]));
            objCommonFunction.ddlSetDataValue(ddlStatusCode, Convert.ToString(dtData.Rows[0]["STATUSCODE"]));
            objCommonFunction.ddlSetDataValue(ddlNatCHSheet, Convert.ToString(dtData.Rows[0]["NATCHSHEET"]));
            objCommonFunction.ddlSetData(ddlFinal, Convert.ToString(dtData.Rows[0]["FINAL"]), true);
            objCommonFunction.ddlSetData(ddlState, Convert.ToString(dtData.Rows[0]["STATE"]), true);
            objCommonFunction.ddlSetData(ddlPenaltyType, Convert.ToString(dtData.Rows[0]["PENALTYTYPE"]), true);
            objCommonFunction.ddlSetData(ddlLodiCase, Convert.ToString(dtData.Rows[0]["LODICASE"]), true);
            objCommonFunction.ddlSetDataValue(ddlRegister, Convert.ToString(dtData.Rows[0]["REGISTER"]));
            objCommonFunction.ddlSetDataValue(ddlPenaltyProceedings, Convert.ToString(dtData.Rows[0]["PENALTYPROCEEDING"]));
            objCommonFunction.ddlSetDataValue(ddlDisAuthoritysCircle, Convert.ToString(dtData.Rows[0]["DISAUTHORITYCIRCLE"]));
            if (objCommonFunction.ddlSelectedValue(ddlRegister) == "")
            {
                lblRegister.Text = Convert.ToString(dtData.Rows[0]["REGISTER"]);
            }

            if (objCommonFunction.ddlSelectedValue(ddlStatusCode) == "0")
            {
                lblStatusCodeMIS.Text = Convert.ToString(dtData.Rows[0]["STATUSCODE"]);
            }

            objCommonFunction.ddlSetDataValue(ddlNature, Convert.ToString(dtData.Rows[0]["NATURE"]));
            if (objCommonFunction.ddlSelectedValue(ddlNature) == "0")
            {
                lblNatureMIS.Text = Convert.ToString(dtData.Rows[0]["NATURE"]);
            }
            objCommonFunction.ddlSetDataValue(ddlBankName, Convert.ToString(dtData.Rows[0]["BANKNAME"]));


            txtDealingOfficerRemarks.Text = Convert.ToString(dtData.Rows[0]["DESK_USER_REMARKS"]);
            txtStatus.Text = Convert.ToString(dtData.Rows[0]["STATUS"]);
            txtLetterSentDate.Text = Convert.ToString(dtData.Rows[0]["LETTERSENTDATE"]);
            txtReminderDate.Text = Convert.ToString(dtData.Rows[0]["REMINDERDATE"]);
            txtReplyReceivedDate.Text = Convert.ToString(dtData.Rows[0]["REPLYRECEIVEDDATE"]);
            objCommonFunction.ddlSetDataValue(ddlLetterSentTo, Convert.ToString(dtData.Rows[0]["LETTERSENTTO"]));
            objCommonFunction.ddlSetDataValue(ddlZoneNew, Convert.ToString(dtData.Rows[0]["NEWZONE"]));
            string ZONE = Convert.ToString(Convert.ToString(dtData.Rows[0]["NEWZONE"]));
            if (!string.IsNullOrEmpty(ZONE))
            {
                objCommonFunction.funcZoneCircleMaster(ddlCircleNew, ZONE);
                objCommonFunction.ddlSetDataValue(ddlCircleNew, Convert.ToString(dtData.Rows[0]["NEWCIRCLE"]));
            }
            txtTMSACRefNo.Text = Convert.ToString(dtData.Rows[0]["TMSACREF"]);
        }

        public void funcClear()
        {
            txtRNo.ToolTip = string.Empty;
            txtRNo.Text = string.Empty;
            txtBRComplaint.Text = string.Empty;
            txtRNo1.Text = string.Empty;
            txtNameOfParticulars.Text = string.Empty;
            txtName.Text = string.Empty;
            txtPFNo.Text = string.Empty;
            txtPresentPosting.Text = string.Empty;
            txtSource.Text = string.Empty;
            txtAccountName.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            txtDRefNo.Text = string.Empty;
            txtLapseNature.Text = string.Empty;
            txtReasonsforInclusion.Text = string.Empty;
            txtDARefNo.Text = string.Empty;
            txtDeletionReasons.Text = string.Empty;
            txtUS.Text = string.Empty;
            txtInvOfficerName.Text = string.Empty;
            txtRecommofCVC.Text = string.Empty;
            txtCbiRcNo1.Text = string.Empty;
            txtIRCBIPending.Text = string.Empty;
            txtCBIRecom.Text = string.Empty;
            txtPolFirNo.Text = string.Empty;
            txtRCSource.Text = string.Empty;
            txtInvestig.Text = string.Empty;
            txtCVCOMNo.Text = string.Empty;
            txtCVC2Proposed.Text = string.Empty;
            txtADV1Awaited.Text = string.Empty;
            txtNatureofAccount.Text = string.Empty;
            txtPOName.Text = string.Empty;
            txtPOCBI.Text = string.Empty;
            txtCVC2Ref.Text = string.Empty;
            txtDTSHear.Text = string.Empty;
            txtPunishmentProposedbyDA.Text = string.Empty;
            txtAppeal.Text = string.Empty;
            txtPenalty.Text = string.Empty;
            txtDispAuthority.Text = string.Empty;
            //txtDisAuthoritysCircle.Text = string.Empty;
            txtPrevCasePunishment.Text = string.Empty;
            txtBasicPay.Text = string.Empty;
            txtCVOAdvice.Text = string.Empty;
            txtLodiNo.Text = string.Empty;
            txtConnectedVigCase.Text = string.Empty;
            txtField1.Text = string.Empty;
            txt2DAProposal.Text = string.Empty;
            txt2ndPending.Text = string.Empty;
            txtReComp.Text = string.Empty;
            txtNoAwardS.Text = string.Empty;
            txtProposedActiontoCVC.Text = string.Empty;
            txtRegInvok.Text = string.Empty;
            txtCBIRCNo2.Text = string.Empty;
            txtEOName.Text = string.Empty;
            txtCDIName.Text = string.Empty;
            txtPrelimEnq.Text = string.Empty;
            txtAdv2Awt.Text = string.Empty;
            txtRecCVC2.Text = string.Empty;
            txtReguEnq.Text = string.Empty;
            txtStatusinBrief.Text = string.Empty;
            txtLodiNew.Text = string.Empty;
            txtEoCdi.Text = string.Empty;
            txtDAProposal.Text = string.Empty;
            txtIstPending.Text = string.Empty;
            txtCVCAdbiceII.Text = string.Empty;
            txtStatus.Text = string.Empty;
            txtHOStatus.Text = string.Empty;
            txtNAPUNDA.Text = string.Empty;
            txtLodiCode.Text = string.Empty;
            txtLodiInclusionReason.Text = string.Empty;
            txtLodiDeletionReason.Text = string.Empty;

            txtCompRecDate.Text = string.Empty;
            txtRNoDate.Text = string.Empty;
            txtChargeDate.Text = string.Empty;
            txtRC1Date.Text = string.Empty;
            txtRetirementDate.Text = string.Empty;
            txtSuspensionDate.Text = string.Empty;
            txtRecReportDate.Text = string.Empty;
            txtOccurDate.Text = string.Empty;
            txtOMCVCDate.Text = string.Empty;
            txtFIRDate.Text = string.Empty;
            txtAppPODate.Text = string.Empty;
            txtAppEODate.Text = string.Empty;
            txtLastRHDate.Text = string.Empty;
            txtFinalDate.Text = string.Empty;
            txtConEnqDate.Text = string.Empty;
            txtBasicPayDate.Text = string.Empty;
            txtAppCDIDate.Text = string.Empty;
            txtCVO2AdviceDate.Text = string.Empty;
            txtAdviceSentToDADate.Text = string.Empty;
            txtWrittenBriefCODate.Text = string.Empty;
            txt2ndDADate.Text = string.Empty;
            txtDAOrdDate.Text = string.Empty;
            txtRegulatDate.Text = string.Empty;
            txtIstDaDate.Text = string.Empty;
            txtRevocationDate.Text = string.Empty;
            txtReviewDate.Text = string.Empty;
            txtClosureDate.Text = string.Empty;
            txtCVOAdviceDate.Text = string.Empty;
            txtReferToCVCDate.Text = string.Empty;
            txtRC2Date.Text = string.Empty;
            txtCommitmentDate.Text = string.Empty;
            txtCHSheetFiledDate.Text = string.Empty;
            txtCOReplyDate.Text = string.Empty;
            txtTargetDate.Text = string.Empty;
            txtPlaceinPresentScaleDate.Text = string.Empty;
            txtSanctionOrderDate.Text = string.Empty;
            txtERCODate.Text = string.Empty;
            txtWrittenBriefPO.Text = string.Empty;
            txtSanctionRecvDate.Text = string.Empty;
            txtSanctionRefusedDate.Text = string.Empty;
            txtCSOREPDate.Text = string.Empty;
            txtA1CSCVC.Text = string.Empty;
            txtA1EOPOCVC.Text = string.Empty;
            txtA2FOCVC.Text = string.Empty;
            txtCVO2Advice.Text = string.Empty;
            txtDealingOfficerRemarks.Text = "";
            chkClosureDate.Checked = false;
            lblClosureDate.Text = "";
            chkRNoDate.Checked = false;
            lblRNoDate.Text = "";
            ddlCircleOffice.SelectedIndex = 0;
            ddlZone.SelectedIndex = 0;
            ddlStatusCode.SelectedIndex = 0;
            ddlScale.SelectedIndex = 0;
            ddlNature.SelectedIndex = 0;
            ddlNatCHSheet.SelectedIndex = 0;
            ddlFinal.SelectedIndex = 0;
            ddlState.SelectedIndex = 0;
            ddlPenaltyType.SelectedIndex = 0;
            ddlLodiCase.SelectedIndex = 0;
            ddlRegister.SelectedIndex = 0;
            ddlPenaltyProceedings.SelectedIndex = 0;
            ddlDisAuthoritysCircle.SelectedIndex = 0;

            lblNatureMIS.Text = string.Empty;
            lblStatusCodeMIS.Text = string.Empty;
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            ddlBankName.SelectedIndex = 0;

            ddlLetterSentTo.SelectedIndex = 0;
            txtLetterSentDate.Text = "";
            txtReminderDate.Text = "";
            txtReplyReceivedDate.Text = "";
            
            ddlZoneNew.SelectedIndex = 0;
            if (ddlCircleNew.Items.Count > 0)
            {
                ddlCircleNew.Items.Clear();
            }
            txtTMSACRefNo.Text = "";

            funcControlsUserRights();
        }

        public void funcControlsUserRights()
        {
            if (Convert.ToString(Session["role"]).ToUpper().Equals("VMIS_VIEWUSER"))
            {
                objCommonFunction.DisableAllControls(this.Page);
                btnSubmit.Visible = false;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;

                txtRNo_LIST.Enabled = true;
                txtAccountName_LIST.Enabled = true;
                txtName_LIST.Enabled = true;
                txtCBIRCNO_LIST.Enabled = true;
                txtCVCOMNO_LIST.Enabled = true;
                txtStatus_LIST.Enabled = true;
                txtPFNumber_LIST.Enabled = true;
                txtBranch_LIST.Enabled = true;
                txtCircle_LIST.Enabled = true;
            }
            else if (Convert.ToString(Session["role"]).ToUpper().Equals("VMIS_DESKUSER"))
            {
                objCommonFunction.DisableAllControls(this.Page);
                pnlHOStatus.Visible = true;
                txtHOStatus.Enabled = true;
                txtDealingOfficerRemarks.Enabled = true;
                btnSubmit.Visible = false;
                btnUpdate.Enabled = true;
                btnCancel.Visible = false;

                txtRNo_LIST.Enabled = true;
                txtAccountName_LIST.Enabled = true;
                txtName_LIST.Enabled = true;
                txtCBIRCNO_LIST.Enabled = true;
                txtCVCOMNO_LIST.Enabled = true;
                txtStatus_LIST.Enabled = true;
                txtPFNumber_LIST.Enabled = true;
                txtBranch_LIST.Enabled = true;
                txtCircle_LIST.Enabled = true;
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            funcClear();
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            funcShow(txtRNo.Text.Trim(), "GET", null, null, null, null, null, null, null, null);
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (Convert.ToString(e.CommandArgument).ToUpper().Equals("VIEW"))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(e.CommandArgument)))
                    {
                        funcShow(Convert.ToString(e.CommandArgument), "VIEW", null, null, null, null, null, null, null, null);
                    }
                }
            }
            catch (Exception eg)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(eg);
            }
        }

        protected void tabMain_ActiveTabChanged(object sender, EventArgs e)
        {
            if (tabMain.ActiveTab == tabList)
            {
                funcShow(null, "LIST", null, null, null, null, null, null, null, null); //for bind grid view on List Tab Load
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

        protected void btnSearch_List_Click(object sender, EventArgs e)
        {
            string VIEW = "SEARCH";

            if (string.IsNullOrEmpty(txtRNo_LIST.Text.Trim()) && string.IsNullOrEmpty(txtAccountName_LIST.Text) && string.IsNullOrEmpty(txtName_LIST.Text) && string.IsNullOrEmpty(txtCBIRCNO_LIST.Text.Trim()) && string.IsNullOrEmpty(txtCVCOMNO_LIST.Text.Trim()) && string.IsNullOrEmpty(txtStatus_LIST.Text.Trim()) && string.IsNullOrEmpty(txtPFNumber_LIST.Text.Trim()) && string.IsNullOrEmpty(txtBranch_LIST.Text.Trim()) && string.IsNullOrEmpty(txtCircle_LIST.Text.Trim()))
            {
                VIEW = "LIST";
            }

            funcShow(txtRNo_LIST.Text.Trim(), VIEW, txtAccountName_LIST.Text, txtName_LIST.Text, txtCBIRCNO_LIST.Text.Trim(), txtCVCOMNO_LIST.Text.Trim(), txtStatus_LIST.Text.Trim(), txtPFNumber_LIST.Text.Trim(), txtBranch_LIST.Text.Trim(), txtCircle_LIST.Text.Trim());
        }
    }
}