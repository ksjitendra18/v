using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class frmVigilance : System.Web.UI.Page
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
        string strRNO1 = string.Empty;
        string strNAMEOFPARTICULARS = string.Empty;
        string strNAME = string.Empty;
        string strPFNO = string.Empty;
        string strPRESENTPOSTING = string.Empty;
        string strZONE = string.Empty;
        string strSOURCE = string.Empty;
        string strREGISTER = string.Empty;
        string strACCOUNTNAME = string.Empty;
        decimal decAMOUNT = 0;
        string strFINAL = string.Empty;
        string strDESIGNATION = string.Empty;
        string strDREFNO = string.Empty;
        string strLAPSENATURE = string.Empty;
        string strREASONSFORINCLUSION = string.Empty;
        string strDAREFNO = string.Empty;
        string strSCALE = string.Empty;
        string strDELETIONREASONS = string.Empty;
        string strUS = string.Empty;
        string strINVOFFICERNAME = string.Empty;
        string strRECOMMOFCVC = string.Empty;
        string strCBIRCNO1 = string.Empty;
        string strIRCBIPENDING = string.Empty;
        string strCBIRECOM = string.Empty;
        string strPOLFIRNO = string.Empty;
        string strRCSOURCE = string.Empty;
        string strINVESTIG = string.Empty;
        string strCVCOMNO = string.Empty;
        string strCVC2PROPOSED = string.Empty;
        string strADV1AWAITED = string.Empty;
        string strNATUREOFACCOUNT = string.Empty;
        string strPONAME = string.Empty;
        string strPOCBI = string.Empty;
        string strDTSHEAR = string.Empty;
        string strPUNISHMENTPROPOSEDBYDA = string.Empty;
        string strPENALTY = string.Empty;
        string strDISPAUTHORITY = string.Empty;
        string strDISAUTHORITYCIRCLE = string.Empty;
        string strPREVCASEPUNISHMENT = string.Empty;
        string strBASICPAY = string.Empty;
        string strCVOADVICE = string.Empty;
        string strNAPUNDA = string.Empty;
        string strLODINO = string.Empty;
        string strLODICASE = string.Empty;
        string strCONNECTEDVIGCASE = string.Empty;
        string strFIELD1 = string.Empty;
        string str2DAPROPOSAL = string.Empty;
        string str2NDPENDING = string.Empty;
        string strSTATE = string.Empty;
        string strNATCHSHEET = string.Empty;
        string strRECOM = string.Empty;
        string strNOAWARDS = string.Empty;
        string strPROPOSEDACTIONTOCVC = string.Empty;
        string strREGINVOK = string.Empty;
        string strCBIRCNO2 = string.Empty;
        string strEONAME = string.Empty;
        string strCDINAME = string.Empty;
        string strADV2AWT = string.Empty;
        string strSTATUSINBRIEF = string.Empty;
        string strLODINEW = string.Empty;
        string strEOCDI = string.Empty;
        string strDAPROPOSAL = string.Empty;
        string strISTPENDING = string.Empty;
        string strSTATUSCODE = string.Empty;
        string strCVO2ADVICE = string.Empty;
        string strCVCADVICEII = string.Empty;
        string strNATURECASE = string.Empty;
        string strSTATUS = string.Empty;
        string strHOSTATUS = string.Empty;
        string strCLOSURE = string.Empty;
        string strVIEW = string.Empty;
        string strPENALTYTYPE = string.Empty;
        string strRNODATE = string.Empty;
        string strLODICODE = string.Empty;
        string strLODIINCLUSIONREASON = string.Empty;
        string strLODIDELETIONREASON = string.Empty;
        string strTMSACRef = string.Empty;

        int intPENALTYPROCEEDINGS = 0;

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
                funcShow(null, "LIST", null, null, null, null, null, null, null, null); //for bind grid view on form Load
                funcbindDropdown();     //Bind All DropDown Lists
            }

            txtRNo.Focus();
            lblMsg.Text = string.Empty;
            funcControlsUserRights();

            #region ** JS Function  **
            btnSubmit.Attributes.Add("onclick", "return funcValidation_Vigilance('" + txtRNo.ClientID + "','" + txtName.ClientID + "','" + txtPFNo.ClientID + "','" + ddlDisAuthoritysCircle.ClientID + "','" + chkRNoDate.ClientID + "','" + ddlScale.ClientID + "','" + ddlStatusCode.ClientID + "','" + txtDAOrdDate.ClientID + "','" + ddlPenaltyType.ClientID + "','" + txtNAPUNDA.ClientID + "','" + ddlRegister.ClientID + "','" + ddlCircleOffice.ClientID + "','" + ddlPenaltyProceedings.ClientID + "')");
            btnUpdate.Attributes.Add("onclick", "return funcValidation_Vigilance('" + txtRNo.ClientID + "','" + txtName.ClientID + "','" + txtPFNo.ClientID + "','" + ddlDisAuthoritysCircle.ClientID + "','" + chkRNoDate.ClientID + "','" + ddlScale.ClientID + "','" + ddlStatusCode.ClientID + "','" + txtDAOrdDate.ClientID + "','" + ddlPenaltyType.ClientID + "','" + txtNAPUNDA.ClientID + "','" + ddlRegister.ClientID + "','" + ddlCircleOffice.ClientID + "','" + ddlPenaltyProceedings.ClientID + "')");

            imgGet.Attributes.Add("onclick", "return funcSearch_Validation('" + txtRNo.ClientID + "','" + "Please Enter Vigilance Number" + "')");
            //btnDelete.Attributes.Add("onclick", "return funcSearch_Validation('" + txtRNo.ClientID + "','" + "Please Enter Vigilance Number" + "')");
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

            #region ** readOnly Date Contols **
            txtRecReportDate.Attributes.Add("readonly", "readonly");
            txtOccurDate.Attributes.Add("readonly", "readonly");
            txtFIRDate.Attributes.Add("readonly", "readonly");
            txtLastRHDate.Attributes.Add("readonly", "readonly");
            txtBasicPayDate.Attributes.Add("readonly", "readonly");
            txtAppCDIDate.Attributes.Add("readonly", "readonly");
            txtAdviceSentToDADate.Attributes.Add("readonly", "readonly");
            txtWrittenBriefCODate.Attributes.Add("readonly", "readonly");
            txtRegulatDate.Attributes.Add("readonly", "readonly");
            txtRevocationDate.Attributes.Add("readonly", "readonly");
            txtClosureDate.Attributes.Add("readonly", "readonly");
            txtCommitmentDate.Attributes.Add("readonly", "readonly");
            txtCHSheetFiledDate.Attributes.Add("readonly", "readonly");
            txtCOReplyDate.Attributes.Add("readonly", "readonly");
            txtTargetDate.Attributes.Add("readonly", "readonly");
            txtERCODate.Attributes.Add("readonly", "readonly");
            txtAppeal.Attributes.Add("readonly", "readonly");
            txtPrelimEnq.Attributes.Add("readonly", "readonly");
            txtReguEnq.Attributes.Add("readonly", "readonly");
            txtWrittenBriefPO.Attributes.Add("readonly", "readonly");
            txtSanctionRecvDate.Attributes.Add("readonly", "readonly");
            txtCSOREPDate.Attributes.Add("readonly", "readonly");
            txtA1CSCVC.Attributes.Add("readonly", "readonly");
            txtA1EOPOCVC.Attributes.Add("readonly", "readonly");
            txtA2FOCVC.Attributes.Add("readonly", "readonly");
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
                intCode = objCommonFunction.convertToIntToolTip(txtRNo);
                strRNO = txtRNo.Text.Trim();
                strBRCOMPLAINT = txtBRComplaint.Text;
                strRNO1 = txtRNo1.Text;
                strNAMEOFPARTICULARS = txtNameOfParticulars.Text;
                strNAME = txtName.Text;
                strPFNO = txtPFNo.Text;
                strPRESENTPOSTING = txtPresentPosting.Text;
                strSOURCE = txtSource.Text;
                strACCOUNTNAME = txtAccountName.Text;
                decAMOUNT = objCommonFunction.convertToDecimal(txtAmount);
                strDESIGNATION = txtDesignation.Text;
                strDREFNO = txtDRefNo.Text;
                strLAPSENATURE = txtLapseNature.Text;
                strREASONSFORINCLUSION = txtReasonsforInclusion.Text;
                strDAREFNO = txtDARefNo.Text;
                strDELETIONREASONS = txtDeletionReasons.Text;
                strUS = txtUS.Text;
                strINVOFFICERNAME = txtInvOfficerName.Text;
                strRECOMMOFCVC = txtRecommofCVC.Text;
                strCBIRCNO1 = txtCbiRcNo1.Text;
                strIRCBIPENDING = txtIRCBIPending.Text;
                strCBIRECOM = txtCBIRecom.Text;
                strPOLFIRNO = txtPolFirNo.Text;
                strRCSOURCE = txtRCSource.Text;
                strINVESTIG = txtInvestig.Text;
                strCVCOMNO = txtCVCOMNo.Text;
                strCVC2PROPOSED = txtCVC2Proposed.Text;
                strADV1AWAITED = txtADV1Awaited.Text;
                strNATUREOFACCOUNT = txtNatureofAccount.Text;
                strPONAME = txtPOName.Text;
                strPOCBI = txtPOCBI.Text;
                strDTSHEAR = txtDTSHear.Text;
                strPUNISHMENTPROPOSEDBYDA = txtPunishmentProposedbyDA.Text;
                strPENALTY = txtPenalty.Text;
                strDISPAUTHORITY = txtDispAuthority.Text;
                strPREVCASEPUNISHMENT = txtPrevCasePunishment.Text;
                strBASICPAY = txtBasicPay.Text;
                strCVOADVICE = txtCVOAdvice.Text;
                strLODINO = txtLodiNo.Text;
                strCONNECTEDVIGCASE = txtConnectedVigCase.Text;
                strFIELD1 = txtField1.Text;
                str2DAPROPOSAL = txt2DAProposal.Text;
                str2NDPENDING = txt2ndPending.Text;
                strRECOM = txtReComp.Text;
                strNOAWARDS = txtNoAwardS.Text;
                strPROPOSEDACTIONTOCVC = txtProposedActiontoCVC.Text;
                strREGINVOK = txtRegInvok.Text;
                strCBIRCNO2 = txtCBIRCNo2.Text;
                strEONAME = txtEOName.Text;
                strCDINAME = txtCDIName.Text;
                strADV2AWT = txtAdv2Awt.Text;
                strSTATUSINBRIEF = txtStatusinBrief.Text;
                strLODINEW = txtLodiNew.Text;
                strEOCDI = txtEoCdi.Text;
                strNAPUNDA = txtNAPUNDA.Text;
                strDAPROPOSAL = txtDAProposal.Text;
                strISTPENDING = txtIstPending.Text;
                strCVCADVICEII = txtCVCAdbiceII.Text;
                strSTATUS = txtStatus.Text;
                strCVO2ADVICE = txtCVO2Advice.Text;
                strHOSTATUS = txtHOStatus.Text;
                strUser = ViewState["USERNAME"].ToString();
                strUserRole = ViewState["USERROLE"].ToString();
                strLODICODE = txtLodiCode.Text.Trim();
                strLODIINCLUSIONREASON = txtLodiInclusionReason.Text.Trim();
                strLODIDELETIONREASON = txtLodiDeletionReason.Text.Trim();
                DESKUSERREMARKS = txtDealingOfficerRemarks.Text.Trim();
                BANKNAME = objCommonFunction.ddlSelectedValue(ddlBankName);
                strTMSACRef = txtTMSACRefNo.Text;

                #region ** For Closure Date **
                strCLOSURE = objCommonFunction.chkSelected(chkClosureDate);
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
                #endregion

                if (strUserRole.ToUpper() != "VMIS_DESKUSER")
                {
                    ZONENEW = objCommonFunction.ddlSelectedValue(ddlZoneNew);
                    CIRCLENEW = objCommonFunction.ddlSelectedValue(ddlCircleNew);
                    strCIRCLEOFFICE = objCommonFunction.ddlSelectedText(ddlCircleOffice);
                    strZONE = objCommonFunction.ddlSelectedText(ddlZone);
                    strSTATUSCODE = objCommonFunction.ddlSelectedValue(ddlStatusCode);
                    strNATURECASE = objCommonFunction.ddlSelectedValue(ddlNature);
                    strSCALE = objCommonFunction.ddlSelectedValue_Scale(ddlScale);
                    strNATCHSHEET = objCommonFunction.ddlSelectedValue(ddlNatCHSheet);
                    strFINAL = objCommonFunction.ddlSelectedText(ddlFinal);
                    strSTATE = objCommonFunction.ddlSelectedText(ddlState);
                    strPENALTYTYPE = objCommonFunction.ddlSelectedText(ddlPenaltyType);
                    strLODICASE = objCommonFunction.ddlSelectedText(ddlLodiCase);
                    strREGISTER = objCommonFunction.ddlSelectedValue(ddlRegister);
                    intPENALTYPROCEEDINGS = objCommonFunction.convertToInt(objCommonFunction.ddlSelectedValue(ddlPenaltyProceedings));
                    strDISAUTHORITYCIRCLE = objCommonFunction.ddlSelectedValue(ddlDisAuthoritysCircle);
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
                #endregion

                #region ** call StoredProcedure to Save/Update data in Table  **
                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spVigilance_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_CODE", intCode);
                cmdSave.Parameters.AddWithValue("@p_RNO", strRNO);
                cmdSave.Parameters.AddWithValue("@p_BRCOMPLAINT", strBRCOMPLAINT);
                cmdSave.Parameters.AddWithValue("@p_CIRCLEOFFICE", strCIRCLEOFFICE);
                cmdSave.Parameters.AddWithValue("@p_RNO1", strRNO1);
                cmdSave.Parameters.AddWithValue("@p_NAMEOFPARTICULARS", strNAMEOFPARTICULARS);
                cmdSave.Parameters.AddWithValue("@p_NAME", strNAME);
                cmdSave.Parameters.AddWithValue("@p_PFNO", strPFNO);
                cmdSave.Parameters.AddWithValue("@p_PRESENTPOSTING", strPRESENTPOSTING);
                cmdSave.Parameters.AddWithValue("@p_SOURCE", strSOURCE);
                cmdSave.Parameters.AddWithValue("@p_REGISTER", strREGISTER);
                cmdSave.Parameters.AddWithValue("@p_ACCOUNTNAME", strACCOUNTNAME);
                cmdSave.Parameters.AddWithValue("@p_AMOUNT", decAMOUNT);
                cmdSave.Parameters.AddWithValue("@p_FINAL", strFINAL);
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", strDESIGNATION);
                cmdSave.Parameters.AddWithValue("@p_DREFNO", strDREFNO);
                cmdSave.Parameters.AddWithValue("@p_LAPSENATURE", strLAPSENATURE);
                cmdSave.Parameters.AddWithValue("@p_REASONSFORINCLUSION", strREASONSFORINCLUSION);
                cmdSave.Parameters.AddWithValue("@p_DAREFNO", strDAREFNO);
                cmdSave.Parameters.AddWithValue("@p_DELETIONREASONS", strDELETIONREASONS);
                cmdSave.Parameters.AddWithValue("@p_US", strUS);
                cmdSave.Parameters.AddWithValue("@p_INVOFFICERNAME", strINVOFFICERNAME);
                cmdSave.Parameters.AddWithValue("@p_RECOMMOFCVC", strRECOMMOFCVC);
                cmdSave.Parameters.AddWithValue("@p_CBIRCNO1", strCBIRCNO1);
                cmdSave.Parameters.AddWithValue("@p_IRCBIPENDING", strIRCBIPENDING);
                cmdSave.Parameters.AddWithValue("@p_CBIRECOM", strCBIRECOM);
                cmdSave.Parameters.AddWithValue("@p_POLFIRNO", strPOLFIRNO);
                cmdSave.Parameters.AddWithValue("@p_RCSOURCE", strRCSOURCE);
                cmdSave.Parameters.AddWithValue("@p_INVESTIG", strINVESTIG);
                cmdSave.Parameters.AddWithValue("@p_CVCOMNO", strCVCOMNO);
                cmdSave.Parameters.AddWithValue("@p_CVC2PROPOSED", strCVC2PROPOSED);
                cmdSave.Parameters.AddWithValue("@p_ADV1AWAITED", strADV1AWAITED);
                cmdSave.Parameters.AddWithValue("@p_NATUREOFACCOUNT", strNATUREOFACCOUNT);
                cmdSave.Parameters.AddWithValue("@p_PONAME", strPONAME);
                cmdSave.Parameters.AddWithValue("@p_POCBI", strPOCBI);
                cmdSave.Parameters.AddWithValue("@p_DTSHEAR", strDTSHEAR);
                cmdSave.Parameters.AddWithValue("@p_PUNISHMENTPROPOSEDBYDA", strPUNISHMENTPROPOSEDBYDA);
                cmdSave.Parameters.AddWithValue("@p_PENALTY", strPENALTY);
                cmdSave.Parameters.AddWithValue("@p_DISPAUTHORITY", strDISPAUTHORITY);
                cmdSave.Parameters.AddWithValue("@p_DISAUTHORITYCIRCLE", strDISAUTHORITYCIRCLE);
                cmdSave.Parameters.AddWithValue("@p_PREVCASEPUNISHMENT", strPREVCASEPUNISHMENT);
                cmdSave.Parameters.AddWithValue("@p_BASICPAY", strBASICPAY);
                cmdSave.Parameters.AddWithValue("@p_CVOADVICE", strCVOADVICE);
                cmdSave.Parameters.AddWithValue("@p_NAPUNDA", strNAPUNDA);
                cmdSave.Parameters.AddWithValue("@p_LODINO", strLODINO);
                cmdSave.Parameters.AddWithValue("@p_LODICASE", strLODICASE);
                cmdSave.Parameters.AddWithValue("@p_CONNECTEDVIGCASE", strCONNECTEDVIGCASE);
                cmdSave.Parameters.AddWithValue("@p_FIELD1", strFIELD1);
                cmdSave.Parameters.AddWithValue("@p_2DAPROPOSAL", str2DAPROPOSAL);
                cmdSave.Parameters.AddWithValue("@p_2NDPENDING", str2NDPENDING);
                cmdSave.Parameters.AddWithValue("@p_STATE", strSTATE);
                cmdSave.Parameters.AddWithValue("@p_NATCHSHEET", strNATCHSHEET);
                cmdSave.Parameters.AddWithValue("@p_RECOM", strRECOM);
                cmdSave.Parameters.AddWithValue("@p_NOAWARDS", strNOAWARDS);
                cmdSave.Parameters.AddWithValue("@p_PROPOSEDACTIONTOCVC", strPROPOSEDACTIONTOCVC);
                cmdSave.Parameters.AddWithValue("@p_REGINVOK", strREGINVOK);
                cmdSave.Parameters.AddWithValue("@p_CBIRCNO2", strCBIRCNO2);
                cmdSave.Parameters.AddWithValue("@p_EONAME", strEONAME);
                cmdSave.Parameters.AddWithValue("@p_CDINAME", strCDINAME);
                cmdSave.Parameters.AddWithValue("@p_ADV2AWT", strADV2AWT);
                cmdSave.Parameters.AddWithValue("@p_STATUSINBRIEF", strSTATUSINBRIEF);
                cmdSave.Parameters.AddWithValue("@p_LODINEW", strLODINEW);
                cmdSave.Parameters.AddWithValue("@p_EOCDI", strEOCDI);
                cmdSave.Parameters.AddWithValue("@p_DAPROPOSAL", strDAPROPOSAL);
                cmdSave.Parameters.AddWithValue("@p_ISTPENDING", strISTPENDING);
                cmdSave.Parameters.AddWithValue("@p_CVO2ADVICE", strCVO2ADVICE);
                cmdSave.Parameters.AddWithValue("@p_CVCADVICEII", strCVCADVICEII);
                cmdSave.Parameters.AddWithValue("@p_STATUS", strSTATUS);
                cmdSave.Parameters.AddWithValue("@p_HOSTATUS", strHOSTATUS);
                cmdSave.Parameters.AddWithValue("@p_STATUSCODE", strSTATUSCODE);
                cmdSave.Parameters.AddWithValue("@p_ZONE", strZONE);
                cmdSave.Parameters.AddWithValue("@p_NATURECASE", strNATURECASE);
                cmdSave.Parameters.AddWithValue("@p_SCALE", strSCALE);
                cmdSave.Parameters.AddWithValue("@p_PENALTYTYPE", strPENALTYTYPE);
                cmdSave.Parameters.AddWithValue("@p_CHK_RNODATE", strRNODATE);
                cmdSave.Parameters.AddWithValue("@p_PENALTYPROCEEDINGS", intPENALTYPROCEEDINGS);
                cmdSave.Parameters.AddWithValue("@p_LODICODE", strLODICODE);
                cmdSave.Parameters.AddWithValue("@p_LODIINCLUSIONREASON", strLODIINCLUSIONREASON);
                cmdSave.Parameters.AddWithValue("@p_LODIDELETIONREASON", strLODIDELETIONREASON);

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
                cmdSave.Parameters.AddWithValue("@p_TMSACREFNO", strTMSACRef);

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

        public void funcShow(string p_strNo, string p_strView, string p_strACCOUNTNAME, string p_strNAME, string p_strCBIRCNO1, string p_strCVCOMNO, string p_strSTATUS, string p_strPFNUMBER, string p_strBRANCH, string p_strCIRCLE)
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
                cmd.CommandText = "[dbo].[spVigilance_Delete]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(sqlErrMsgOutput);
                cmd.Parameters.Add(sqlErrCodeOutput);

                cmd.Parameters.AddWithValue("@p_RNO", p_strRNo);
                cmd.Parameters.AddWithValue("@p_USER", p_strUser);

                cmd.CommandTimeout = 0;
                //cmd.ExecuteNonQuery();

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

            txtRNo.ToolTip = dtData.Rows[0]["CODE"].ToString();
            txtRNo.Text = dtData.Rows[0]["RNO"].ToString();
            txtBRComplaint.Text = dtData.Rows[0]["BRCOMPLAINT"].ToString();
            txtRNo1.Text = dtData.Rows[0]["RNO1"].ToString();
            txtNameOfParticulars.Text = dtData.Rows[0]["NAMEOFPARTICULARS"].ToString();
            txtName.Text = dtData.Rows[0]["NAME"].ToString();
            txtPFNo.Text = dtData.Rows[0]["PFNO"].ToString();
            txtPresentPosting.Text = dtData.Rows[0]["PRESENTPOSTING"].ToString();
            txtSource.Text = dtData.Rows[0]["SOURCE"].ToString();
            txtAccountName.Text = dtData.Rows[0]["ACCOUNTNAME"].ToString();
            txtAmount.Text = dtData.Rows[0]["AMOUNT"].ToString();
            txtDesignation.Text = dtData.Rows[0]["DESIGNATION"].ToString();
            txtDRefNo.Text = dtData.Rows[0]["DREFNO"].ToString();
            txtLapseNature.Text = dtData.Rows[0]["LAPSENATURE"].ToString();
            txtReasonsforInclusion.Text = dtData.Rows[0]["REASONSFORINCLUSION"].ToString();
            txtDARefNo.Text = dtData.Rows[0]["DAREFNO"].ToString();
            txtDeletionReasons.Text = dtData.Rows[0]["DELETIONREASONS"].ToString();
            txtUS.Text = dtData.Rows[0]["US"].ToString();
            txtInvOfficerName.Text = dtData.Rows[0]["INVOFFICERNAME"].ToString();
            txtRecommofCVC.Text = dtData.Rows[0]["RECOMMOFCVC"].ToString();
            txtCbiRcNo1.Text = dtData.Rows[0]["CBIRCNO1"].ToString();
            txtIRCBIPending.Text = dtData.Rows[0]["IRCBIPENDING"].ToString();
            txtCBIRecom.Text = dtData.Rows[0]["CBIRECOM"].ToString();
            txtPolFirNo.Text = dtData.Rows[0]["POLFIRNO"].ToString();
            txtRCSource.Text = dtData.Rows[0]["RCSOURCE"].ToString();
            txtInvestig.Text = dtData.Rows[0]["INVESTIG"].ToString();
            txtCVCOMNo.Text = dtData.Rows[0]["CVCOMNO"].ToString();
            txtCVC2Proposed.Text = dtData.Rows[0]["CVC2PROPOSED"].ToString();
            txtADV1Awaited.Text = dtData.Rows[0]["ADV1AWAITED"].ToString();
            txtNatureofAccount.Text = dtData.Rows[0]["NATUREOFACCOUNT"].ToString();
            txtPOName.Text = dtData.Rows[0]["PONAME"].ToString();
            txtPOCBI.Text = dtData.Rows[0]["POCBI"].ToString();
            txtDTSHear.Text = dtData.Rows[0]["DTSHEAR"].ToString();
            txtPunishmentProposedbyDA.Text = dtData.Rows[0]["PUNISHMENTPROPOSEDBYDA"].ToString();
            txtPenalty.Text = dtData.Rows[0]["PENALTY"].ToString();
            txtDispAuthority.Text = dtData.Rows[0]["DISPAUTHORITY"].ToString();
            //txtDisAuthoritysCircle.Text = dtData.Rows[0]["DISAUTHORITYCIRCLE"].ToString();
            txtPrevCasePunishment.Text = dtData.Rows[0]["PREVCASEPUNISHMENT"].ToString();
            txtWrittenBriefPO.Text = dtData.Rows[0]["WRITTENBRIEFPO"].ToString();
            txtBasicPay.Text = dtData.Rows[0]["BASICPAY"].ToString();
            txtCVOAdvice.Text = dtData.Rows[0]["CVOADVICE"].ToString();
            txtLodiNo.Text = dtData.Rows[0]["LODINO"].ToString();
            txtConnectedVigCase.Text = dtData.Rows[0]["CONNECTEDVIGCASE"].ToString();
            txtField1.Text = dtData.Rows[0]["FIELD1"].ToString();
            txt2DAProposal.Text = dtData.Rows[0]["DAPROPOSAL2"].ToString();
            txt2ndPending.Text = dtData.Rows[0]["PENDING2ND"].ToString();
            txtReComp.Text = dtData.Rows[0]["RECOM"].ToString();
            txtNoAwardS.Text = dtData.Rows[0]["NOAWARDS"].ToString();
            txtProposedActiontoCVC.Text = dtData.Rows[0]["PROPOSEDACTIONTOCVC"].ToString();
            txtRegInvok.Text = dtData.Rows[0]["REGINVOK"].ToString();
            txtCBIRCNo2.Text = dtData.Rows[0]["CBIRCNO2"].ToString();
            txtEOName.Text = dtData.Rows[0]["EONAME"].ToString();
            txtCDIName.Text = dtData.Rows[0]["CDINAME"].ToString();
            txtAdv2Awt.Text = dtData.Rows[0]["ADV2AWT"].ToString();
            txtStatusinBrief.Text = dtData.Rows[0]["STATUSINBRIEF"].ToString();
            txtLodiNew.Text = dtData.Rows[0]["LODINEW"].ToString();
            txtEoCdi.Text = dtData.Rows[0]["EOCDI"].ToString();
            txtDAProposal.Text = dtData.Rows[0]["DAPROPOSAL"].ToString();
            txtIstPending.Text = dtData.Rows[0]["ISTPENDING"].ToString();
            txtCVCAdbiceII.Text = dtData.Rows[0]["CVCADVICEII"].ToString();
            txtNAPUNDA.Text = dtData.Rows[0]["NAPUNDA"].ToString();
            txtLodiCode.Text = dtData.Rows[0]["LODICODE"].ToString();
            txtLodiInclusionReason.Text = dtData.Rows[0]["LODIINCLUSIONREASON"].ToString();
            txtLodiDeletionReason.Text = dtData.Rows[0]["LODIDELETIONREASON"].ToString();

            txtCompRecDate.Text = dtData.Rows[0]["RECDATECOMP"].ToString();
            txtChargeDate.Text = dtData.Rows[0]["CHARGEDATE"].ToString();
            txtRC1Date.Text = dtData.Rows[0]["RC1DATE"].ToString();
            txtRetirementDate.Text = dtData.Rows[0]["RETIREMENTDATE"].ToString();
            txtSuspensionDate.Text = dtData.Rows[0]["SUSPENSION"].ToString();
            txtRecReportDate.Text = dtData.Rows[0]["RECREPORTDATE"].ToString();
            txtOccurDate.Text = dtData.Rows[0]["OCCURDATE"].ToString();
            txtOMCVCDate.Text = dtData.Rows[0]["OMCVCDATE"].ToString();
            txtFIRDate.Text = dtData.Rows[0]["FIRDATE"].ToString();
            txtAppPODate.Text = dtData.Rows[0]["APPPODATE"].ToString();
            txtAppEODate.Text = dtData.Rows[0]["APPEODATE"].ToString();
            txtLastRHDate.Text = dtData.Rows[0]["LASTRHDATE"].ToString();
            txtFinalDate.Text = dtData.Rows[0]["FINALDATE"].ToString();
            txtConEnqDate.Text = dtData.Rows[0]["CONENQDATE"].ToString();
            txtBasicPayDate.Text = dtData.Rows[0]["BASICDATE"].ToString();
            txtAppCDIDate.Text = dtData.Rows[0]["APPCDIDATE"].ToString();
            txtCVO2AdviceDate.Text = dtData.Rows[0]["CVO2ADVICEDATE"].ToString();
            txtAdviceSentToDADate.Text = dtData.Rows[0]["ADVICESENTTODADATE"].ToString();
            txtWrittenBriefCODate.Text = dtData.Rows[0]["WRITTENBRIEFCODATE"].ToString();
            txt2ndDADate.Text = dtData.Rows[0]["DA2NDDATE"].ToString();
            txtDAOrdDate.Text = dtData.Rows[0]["DAORDDATE"].ToString();
            txtRegulatDate.Text = dtData.Rows[0]["REGULATDATE"].ToString();
            txtIstDaDate.Text = dtData.Rows[0]["ISTDADATE"].ToString();
            txtRevocationDate.Text = dtData.Rows[0]["REVOCATIONDATE"].ToString();
            txtReviewDate.Text = dtData.Rows[0]["REVIEWDATE"].ToString();
            txtCVOAdviceDate.Text = dtData.Rows[0]["CVOADVICEDATE"].ToString();
            txtReferToCVCDate.Text = dtData.Rows[0]["REFERTOCVCDATE"].ToString();
            txtRC2Date.Text = dtData.Rows[0]["RC2DATE"].ToString();
            txtCommitmentDate.Text = dtData.Rows[0]["COMMITMENTDATE"].ToString();
            txtCHSheetFiledDate.Text = dtData.Rows[0]["CHSHEETFILEDDATE"].ToString();
            txtCOReplyDate.Text = dtData.Rows[0]["COREPLYDATE"].ToString();
            txtTargetDate.Text = dtData.Rows[0]["TARGETDATE"].ToString();
            txtPlaceinPresentScaleDate.Text = dtData.Rows[0]["PLACEINPRESENTSCALEDATE"].ToString();
            txtSanctionOrderDate.Text = dtData.Rows[0]["SANCTIONORDERDATE"].ToString();
            txtERCODate.Text = dtData.Rows[0]["ERCODATE"].ToString();
            txtRecCVC2.Text = dtData.Rows[0]["RECCVC2"].ToString();
            txtCVC2Ref.Text = dtData.Rows[0]["CVC2REF"].ToString();
            txtAppeal.Text = dtData.Rows[0]["APPEAL"].ToString();
            txtPrelimEnq.Text = dtData.Rows[0]["PRELIMENQ"].ToString();
            txtReguEnq.Text = dtData.Rows[0]["REGUENQ"].ToString();
            txtSanctionRecvDate.Text = dtData.Rows[0]["SANCTIONRECIVED"].ToString();
            txtSanctionRefusedDate.Text = dtData.Rows[0]["SANCTIONREFUSED"].ToString();
            txtCSOREPDate.Text = dtData.Rows[0]["CSOREPDATE"].ToString();
            txtA1CSCVC.Text = dtData.Rows[0]["A1CSCVC"].ToString();
            txtA1EOPOCVC.Text = dtData.Rows[0]["A1EOPOCVC"].ToString();
            txtA2FOCVC.Text = dtData.Rows[0]["A2FOCVC"].ToString();
            txtCVO2Advice.Text = dtData.Rows[0]["CVO2ADVICE"].ToString();

            objCommonFunction.chkSetData(chkClosureDate, dtData.Rows[0]["CLOSURE"].ToString());
            lblClosureDate.Text = dtData.Rows[0]["CLOSUREDATE"].ToString();
            objCommonFunction.chkSetData(chkRNoDate, dtData.Rows[0]["CHK_RNODATE"].ToString());
            lblRNoDate.Text = dtData.Rows[0]["RNODATE"].ToString();

            objCommonFunction.ddlSetData(ddlCircleOffice, dtData.Rows[0]["CIRCLEOFFICE"].ToString(), true);
            hidCircleOffice.Value = dtData.Rows[0]["CIRCLEOFFICE"].ToString();
            objCommonFunction.ddlSetData(ddlZone, dtData.Rows[0]["ZONE"].ToString(), true);
            hidZone.Value = dtData.Rows[0]["ZONE"].ToString();
            objCommonFunction.ddlSetDataValue_Scale(ddlScale, dtData.Rows[0]["SCALE"].ToString());
            hidScale.Value = dtData.Rows[0]["SCALE"].ToString();
            objCommonFunction.ddlSetDataValue(ddlStatusCode, dtData.Rows[0]["STATUSCODE"].ToString());
            hidStatusCode.Value = dtData.Rows[0]["STATUSCODE"].ToString();
            objCommonFunction.ddlSetDataValue(ddlNatCHSheet, dtData.Rows[0]["NATCHSHEET"].ToString());
            hidNatCHSheet.Value = dtData.Rows[0]["NATCHSHEET"].ToString();
            objCommonFunction.ddlSetData(ddlFinal, dtData.Rows[0]["FINAL"].ToString(), true);
            hidFinal.Value = dtData.Rows[0]["FINAL"].ToString();
            objCommonFunction.ddlSetData(ddlState, dtData.Rows[0]["STATE"].ToString(), true);
            hidState.Value = dtData.Rows[0]["STATE"].ToString();
            objCommonFunction.ddlSetData(ddlPenaltyType, dtData.Rows[0]["PENALTYTYPE"].ToString(), true);
            hidPenaltyType.Value = dtData.Rows[0]["PENALTYTYPE"].ToString();
            objCommonFunction.ddlSetData(ddlLodiCase, dtData.Rows[0]["LODICASE"].ToString(), true);
            hidLodiCase.Value = dtData.Rows[0]["LODICASE"].ToString();
            objCommonFunction.ddlSetDataValue(ddlRegister, dtData.Rows[0]["REGISTER"].ToString());
            hidRegister.Value = dtData.Rows[0]["REGISTER"].ToString();
            objCommonFunction.ddlSetDataValue(ddlPenaltyProceedings, dtData.Rows[0]["PENALTYPROCEEDING"].ToString());
            hidPenaltyProceedings.Value = dtData.Rows[0]["PENALTYPROCEEDING"].ToString();
            objCommonFunction.ddlSetDataValue(ddlDisAuthoritysCircle, dtData.Rows[0]["DISAUTHORITYCIRCLE"].ToString());
            hidDisAuthoritysCircle.Value = dtData.Rows[0]["DISAUTHORITYCIRCLE"].ToString();
            if (objCommonFunction.ddlSelectedValue(ddlRegister) == "")
            {
                lblRegister.Text = dtData.Rows[0]["REGISTER"].ToString();
            }

            if (objCommonFunction.ddlSelectedValue(ddlStatusCode) == "0")
            {
                lblStatusCodeMIS.Text = dtData.Rows[0]["STATUSCODE"].ToString();
            }

            objCommonFunction.ddlSetDataValue(ddlNature, dtData.Rows[0]["NATURE"].ToString());
            hidNatureCase.Value = dtData.Rows[0]["NATURE"].ToString();
            if (objCommonFunction.ddlSelectedValue(ddlNature) == "0")
            {
                lblNatureMIS.Text = dtData.Rows[0]["NATURE"].ToString();
            }
            objCommonFunction.ddlSetDataValue(ddlBankName, Convert.ToString(dtData.Rows[0]["BANKNAME"]));


            txtDealingOfficerRemarks.Text = Convert.ToString(dtData.Rows[0]["DESK_USER_REMARKS"]);
            txtStatus.Text = dtData.Rows[0]["STATUS"].ToString();
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

            hidCircleOffice.Value = "";
            hidNatCHSheet.Value = "";
            hidNatureCase.Value = "";
            hidScale.Value = "";
            hidZone.Value = "";
            hidScale.Value = "";
            hidPenaltyType.Value = "";
            hidLodiCase.Value = "";
            hidRegister.Value = "";
            hidPenaltyProceedings.Value = "";
            hidDisAuthoritysCircle.Value = "";

            lblNatureMIS.Text = string.Empty;
            lblStatusCodeMIS.Text = string.Empty;
            lblEntryBy.Text = string.Empty;
            lblEntryDate.Text = string.Empty;
            lblModifyBy.Text = string.Empty;
            lblModifyDate.Text = string.Empty;
            lblRegister.Text = "";

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
                ddlCircleNew.Items.Clear();
            }
            txtTMSACRefNo.Text = "";

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

                #region ** readOnly Calenders Controls **
                ceCompRecDate.Enabled = false;
                ceRNoDate.Enabled = false;
                ceChargeDate.Enabled = false;
                ceRC1Date.Enabled = false;
                ceRetirementDate.Enabled = false;
                ceSuspensionDate.Enabled = false;
                ceRecReportDate.Enabled = false;
                ceOccurDate.Enabled = false;
                ceOMCVCDate.Enabled = false;
                ceFIRDate.Enabled = false;
                ceAppPODate.Enabled = false;
                ceAppEODate.Enabled = false;
                ceLastRHDate.Enabled = false;
                ceFinalDate.Enabled = false;
                ceConEnqDate.Enabled = false;
                ceBasicPayDate.Enabled = false;
                ceAppCDIDate.Enabled = false;
                ceCVO2AdviceDate.Enabled = false;
                ceAdviceSentToDADate.Enabled = false;
                ceWrittenBriefCODate.Enabled = false;
                ce2ndDADate.Enabled = false;
                ceDAOrdDate.Enabled = false;
                ceRegulatDate.Enabled = false;
                ceIstDaDate.Enabled = false;
                ceRevocationDate.Enabled = false;
                ceReviewDate.Enabled = false;
                ceClosureDate.Enabled = false;
                ceCVOAdviceDate.Enabled = false;
                ceReferToCVCDate.Enabled = false;
                ceRC2Date.Enabled = false;
                ceCommitmentDate.Enabled = false;
                ceCHSheetFiledDate.Enabled = false;
                ceCOReplyDate.Enabled = false;
                ceTargetDate.Enabled = false;
                cePlaceinPresentScaleDate.Enabled = false;
                ceSanctionOrderDate.Enabled = false;
                ceERCODate.Enabled = false;
                ceCVC2Ref.Enabled = false;
                ceRecCVC2.Enabled = false;
                ceAppeal.Enabled = false;
                cePrelimEnq.Enabled = false;
                ceReguEnq.Enabled = false;
                ceWrittenBriefPO.Enabled = false;
                ceSanctionRecvDate.Enabled = false;
                ceSanctionRefusedDate.Enabled = false;
                ceCSOREPDate.Enabled = false;
                ceA1CSCVC.Enabled = false;
                ceA1EOPOCVC.Enabled = false;
                ceA2FOCVC.Enabled = false;
                #endregion

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

                #region ** readOnly Calenders Controls **
                ceCompRecDate.Enabled = false;
                ceRNoDate.Enabled = false;
                ceChargeDate.Enabled = false;
                ceRC1Date.Enabled = false;
                ceRetirementDate.Enabled = false;
                ceSuspensionDate.Enabled = false;
                ceRecReportDate.Enabled = false;
                ceOccurDate.Enabled = false;
                ceOMCVCDate.Enabled = false;
                ceFIRDate.Enabled = false;
                ceAppPODate.Enabled = false;
                ceAppEODate.Enabled = false;
                ceLastRHDate.Enabled = false;
                ceFinalDate.Enabled = false;
                ceConEnqDate.Enabled = false;
                ceBasicPayDate.Enabled = false;
                ceAppCDIDate.Enabled = false;
                ceCVO2AdviceDate.Enabled = false;
                ceAdviceSentToDADate.Enabled = false;
                ceWrittenBriefCODate.Enabled = false;
                ce2ndDADate.Enabled = false;
                ceDAOrdDate.Enabled = false;
                ceRegulatDate.Enabled = false;
                ceIstDaDate.Enabled = false;
                ceRevocationDate.Enabled = false;
                ceReviewDate.Enabled = false;
                ceClosureDate.Enabled = false;
                ceCVOAdviceDate.Enabled = false;
                ceReferToCVCDate.Enabled = false;
                ceRC2Date.Enabled = false;
                ceCommitmentDate.Enabled = false;
                ceCHSheetFiledDate.Enabled = false;
                ceCOReplyDate.Enabled = false;
                ceTargetDate.Enabled = false;
                cePlaceinPresentScaleDate.Enabled = false;
                ceSanctionOrderDate.Enabled = false;
                ceERCODate.Enabled = false;
                ceCVC2Ref.Enabled = false;
                ceRecCVC2.Enabled = false;
                ceAppeal.Enabled = false;
                cePrelimEnq.Enabled = false;
                ceReguEnq.Enabled = false;
                ceWrittenBriefPO.Enabled = false;
                ceSanctionRecvDate.Enabled = false;
                ceSanctionRefusedDate.Enabled = false;
                ceCSOREPDate.Enabled = false;
                ceA1CSCVC.Enabled = false;
                ceA1EOPOCVC.Enabled = false;
                ceA2FOCVC.Enabled = false;
                #endregion

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
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ed);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            funcClear();
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            strSearchNo = txtRNo.Text.Trim();
            funcShow(strSearchNo, "GET", null, null, null, null, null, null, null, null);
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void imgSearch_LIST_Click(object sender, ImageClickEventArgs e)
        {
            strSearchNo = txtRNo_LIST.Text.Trim();
            strACCOUNTNAME = txtAccountName_LIST.Text;
            strNAME = txtName_LIST.Text;
            strCBIRCNO1 = txtCBIRCNO_LIST.Text.Trim();
            strCVCOMNO = txtCVCOMNO_LIST.Text.Trim();
            strSTATUS = txtStatus_LIST.Text.Trim();
            strPFNO = txtPFNumber_LIST.Text.Trim();
            strBRCOMPLAINT = txtBranch_LIST.Text.Trim();
            strCIRCLEOFFICE = txtCircle_LIST.Text.Trim();
            strVIEW = "SEARCH";

            if (strSearchNo == "" && strACCOUNTNAME == "" && strNAME == "" && strCBIRCNO1 == "" && strCVCOMNO == "" && strSTATUS == "" && strPFNO == "" && strBRCOMPLAINT == "" && strCIRCLEOFFICE == "")
            {
                strVIEW = "LIST";
            }
            funcShow(strSearchNo, strVIEW, strACCOUNTNAME, strNAME, strCBIRCNO1, strCVCOMNO, strSTATUS, strPFNO, strBRCOMPLAINT, strCIRCLEOFFICE);
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
                        funcShow(strRNO, "VIEW", null, null, null, null, null, null, null, null);
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
                funcShow(null, "LIST", null, null, null, null, null, null, null, null); //for bind grid view on List Tab Load
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
        protected void btnShowNameOfParticulars_MODAL_Click(object sender, EventArgs e)
        {
            txtNameOfParticulars_MODAL.Text = "";
            txtNameOfParticulars_MODAL.Text = txtNameOfParticulars.Text;
            modalPopUp_NameOfParticulars.Show();
            txtNameOfParticulars.Text = "";
        }

        protected void btnCloseNameOfParticulars_MODAL_Click(object sender, EventArgs e)
        {
            txtNameOfParticulars.Text = "";
            txtNameOfParticulars.Text = txtNameOfParticulars_MODAL.Text;
            objCommonFunction.removeTextBoxFirstComma(txtNameOfParticulars);
            modalPopUp_NameOfParticulars.Hide();
            txtNameOfParticulars_MODAL.Text = "";
        }

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