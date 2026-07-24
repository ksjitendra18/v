using ClosedXML.Excel;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VMISP.Upload
{
    public partial class frmExcelUpload : System.Web.UI.Page
    {
        int intErrCode = 0;
        string strMsg = string.Empty;
        string strErrMsg = string.Empty;
        string strUser = string.Empty;
        string strTableValue = string.Empty;
        string strFileName = string.Empty;
        string strExtension = string.Empty;
        string strFolderPath = string.Empty;
        string strFilePath = string.Empty;
        string strCIRCLEOFFICE = string.Empty;
        string strSOURCE = string.Empty;
        string strSTATUS = string.Empty;
        string strSTATUSCODE = string.Empty;
        string strNATURECASE = string.Empty;
        string strPFNUMBER = string.Empty;
        string BankName = string.Empty;
        Int32 intTotalRowInsert = 0;
        Decimal Amount = 0;

        #region ** IAC VARIABLE DECLARE **
        string strROWNO = string.Empty;
        string strSNO = string.Empty;
        decimal decAMOUNT = 0;
        DateTime? dtDTRET = null;
        DateTime? dtDTIAC = null;
        DateTime? dtRECDT = null;
        DateTime? dtDATEIADNOTE = null;
        DateTime? dtCLOSUREDT = null;
        #endregion

        #region ** COMPLAINT VARIABLE DECLARE **
        string RowNo = string.Empty;
        string RNo = string.Empty;
        DateTime? ComplaintRecDate = null;
        DateTime? ClosureDate = null;
        DateTime? IACDate = null;
        DateTime? SourceDate = null;
        DateTime? SentforInvDate = null;
        DateTime? LetterSentDate = null;
        DateTime? RYSent = null;
        DateTime? ForInvReportDate = null;
        #endregion

        #region ** VIGILANCE VARIABLE DECLARE **
        string strRNO = string.Empty;
        string strRNO1 = string.Empty;
        string strNAMEOFPARTICULARS = string.Empty;

        string strNAME = string.Empty;
        string strSCALE = string.Empty;
        string strDESIGNATION = string.Empty;
        string strBRNAME = string.Empty;
        string strSTATE = string.Empty;
        string strLAPSENATURE = string.Empty;
        string strACCTT_NAME = string.Empty;
        string strNATUREOFACCOUNT = string.Empty;
        string strINVESTIG = string.Empty;

        string strCBI_RC_NO1 = string.Empty;
        string strCBI_RC_NO2 = string.Empty;
        string strCBI_ZONE = string.Empty;
        string strRC_SOURCE = string.Empty;

        string strRECOM_CBI = string.Empty;
        string strPROPOSEDACTIONTOCVC = string.Empty;
        string strCVC_2_PROPOSED = string.Empty;

        string strCVC_OM_NO = string.Empty;
        string strRECOMMOFCVC = string.Empty;
        string strNAT_CHSHEET = string.Empty;
        string strREG_INVOK = string.Empty;

        string strNAME_PO = string.Empty;
        string strNAME_EO = string.Empty;
        string strNAME_CDI = string.Empty;


        string strPUNISHMENTPROPOSEDBY = string.Empty;



        string strCVCSADVICEII = string.Empty;
        string strNA_PUN_DA = string.Empty;
        string strPENALTY = string.Empty;
        string strFINAL = string.Empty;

        string strDISP_AUTHORITY = string.Empty;
        string strDISAUTHORITYSCIRCLE = string.Empty;
        string strSTATUS_INBRIEF = string.Empty;


        string strBASICPAY = string.Empty;
        string strPREVCASE_PUNISHMENTS = string.Empty;
        string strLODICASE = string.Empty;
        string strLODINO = string.Empty;


        string strREGISTER = string.Empty;

        string strDAPROPOSAL = string.Empty;

        string strADVICECVOI = string.Empty;
        string strDAPROPOSAL_2 = string.Empty;

        string strADVICECVO2 = string.Empty;

        string strFEILD1 = string.Empty;

        DateTime? dtDTCHARGE = null;
        DateTime? dtDTRNO = null;
        DateTime? dtDTOFRETIREMENT = null;
        DateTime? dtDTOFSUSPENSION = null;
        DateTime? dtDT_RC1 = null;
        DateTime? dtDT_RC2 = null;


        DateTime? dtDTSANCTIONORDER = null;

        DateTime? dtDTREFERTOCVC = null;
        DateTime? dtDT_OM_CVC = null;
        DateTime? dtDT_ERCO = null;
        DateTime? dtDTREPLYCO = null;

        DateTime? dtDT_APP_PO = null;
        DateTime? dtDT_APP_EO = null;
        DateTime? dtDT_APP_CDI = null;





        DateTime? dtREF_CVC_2 = null;
        DateTime? dtREC_CVC_2 = null;
        DateTime? dtDT_ORD_DA = null;
        DateTime? dtREVIEWDATE = null;

        DateTime? dtDTFINAL = null;

        DateTime? dtDATEOFCLOSURE = null;

        DateTime? dtDTOFPLACEMENTINPRESENTSCALE = null;




        DateTime? dtDATEOFCOMPLAINT = null;
        DateTime? dtDT_IST_DA = null;
        DateTime? dtDT_CVO_ADVICE = null;
        DateTime? dtDT_2ND_DA = null;
        DateTime? dtDT_CVO_ADVICE_2 = null;
        DateTime? dtA1C_CVC = null;
        DateTime? dtA1E_CVC = null;
        DateTime? dtA2_CVC = null;

        CommonFunction objCommonFunction = new CommonFunction();
        StringBuilder strScript = new StringBuilder();
        #endregion

        #region ** NOC VARIABLE DECLARE **
        string strNOCROWNO = string.Empty;
        string strNOCSNO = string.Empty;
        DateTime? dtDTRECDT = null;
        DateTime? dtDTCLEARANCEDT = null;
        #endregion

        #region ** RTI VARIABLE DECLARE **
        string strRTIROWNO = string.Empty;
        string strRTINO = string.Empty;
        string strRTIACCUSED = string.Empty;
        string strRTIDESIGNATION = string.Empty;
        string strRTIPRESENTPOSTING = string.Empty;
        string strBRCOMPLAINT = string.Empty;
        string strRTIZONE = string.Empty;
        string strRTICIRCLEOFFICE = string.Empty;
        string strRTISOURCE = string.Empty;
        string strSOURCEREF = string.Empty;
        string strSENTTO = string.Empty;
        Int32 strCATANO = 0;
        Int32 strCATBNO = 0;
        Int32 strASNO = 0;
        string strNATURECOMP = string.Empty;
        string strRTIACCOUNTNAME = string.Empty;
        string strALLEGATIONS = string.Empty;
        string strRTISTATUS = string.Empty;
        string strPENDINGWITH = string.Empty;
        string strNAMEINVOFFICIAL = string.Empty;
        Int32 strDAYSTAKEN = 0;
        string strFINALACTION = string.Empty;
        string strCASECLOSE = string.Empty;
        string strRTIRNO = string.Empty;
        string strAPLAN = string.Empty;
        string strRTISTATUSCODE = string.Empty;
        string strRTIREGISTER = string.Empty;
        string strNATURE = string.Empty;
        string strREASONSFORCLOSURE = string.Empty;
        string strRTIBANKNAME = string.Empty;
        decimal decRTIAMOUNT = 0;

        DateTime? dtRECDATERTI = null;
        DateTime? dtSOURCEDATE = null;
        DateTime? dtSENTFORINVDATE = null;
        DateTime? dtRTIDTIAC = null;
        DateTime? dtDTOFINVREPORT = null;
        DateTime? dtRTICLOSUREDT = null;
        DateTime? dtRYSENT = null;
        #endregion

        #region ** SR VARIABLE DECLARE **
        string strSRROWNO = string.Empty;
        string strSRNO = string.Empty;
        string strSRACCUSED = string.Empty;
        string strSRDESIGNATION = string.Empty;
        string strSRPRESENTPOSTING = string.Empty;
        string strSRBRANCH = string.Empty;
        string strSRZONE = string.Empty;
        string strSRCIRCLEOFFICE = string.Empty;
        string strREGION = string.Empty;
        string strINVESTIGATION = string.Empty;
        string strNATURESR = string.Empty;
        string strAMOUNT = string.Empty;
        string strSRALLEGATIONS = string.Empty;
        string strREMINDERS = string.Empty;
        string strSRSTATUS = string.Empty;
        string strSRPENDINGWITH = string.Empty;
        string strACCOUNT = string.Empty;
        string strZMVIEW = string.Empty;
        string strICVIEW = string.Empty;
        string strSRFINALACTION = string.Empty;
        string strSRCASECLOSE = string.Empty;
        string strSRRNO = string.Empty;
        string strSRICDT = string.Empty;
        string strSRAPLAN = string.Empty;
        string strSRSTATUSCODE = string.Empty;
        string strSRBANKNAME = string.Empty;
        Decimal decSRAMOUNT = 0;

        DateTime? dtDTRECDTSR = null;
        DateTime? dtDATESR = null;
        DateTime? dtSRDTIAC = null;
        DateTime? dtSRCLOSUREDT = null;
        DateTime? dtICDT = null;
        DateTime? dtCMD = null;

        #endregion

        #region ** WB VARIABLE DECLARE **
        string strWBROWNO = string.Empty;
        string strWBRNO = string.Empty;
        string strCOMPNO = string.Empty;
        string strWBACCUSED = string.Empty;
        string strWBDESIGNATION = string.Empty;
        string strWBPRESENTPOSTING = string.Empty;
        string strWBBRCOMPLAINT = string.Empty;
        string strWBZONE = string.Empty;
        string strWBCIRCLEOFFICE = string.Empty;
        string strWBREGION = string.Empty;
        string strWBSOURCE = string.Empty;
        string strWBSOURCEREF = string.Empty;
        string strWBSENTTO = string.Empty;
        Int32 strWBCATANO = 0;
        Int32 strWBCATBNO = 0;
        Int32 strWBASNO = 0;
        string strWBNATURECOMP = string.Empty;
        string strWBACCOUNTNAME = string.Empty;
        string strWBALLEGATIONS = string.Empty;
        string strWBSTATUS = string.Empty;
        string strWBSTATUSCODE = string.Empty;
        string strWBPENDINGWITH = string.Empty;
        string strNAMEOFINVOFFICIAL = string.Empty;
        Int32 strWBDAYSTAKEN = 0;
        string strCASENO = string.Empty;
        string strWBCASECLOSE = string.Empty;
        string strCLOSUREDT = string.Empty;
        string strWBAPLAN = string.Empty;
        string strWBREGISTER = string.Empty;
        string strWBNATURE = string.Empty;
        string strWBREASONSFORCLOSURE = string.Empty;
        string strWBBANKNAME = string.Empty;

        decimal decWBAMOUNT = 0;

        DateTime? dtRECDATECOMP = null;
        DateTime? dtWBSOURCEDATE = null;
        DateTime? dtWBSENTFORINVDATE = null;
        DateTime? dtWBDTIAC = null;
        DateTime? dtWBDTOFINVREPORT = null;
        DateTime? dtWBRYSENT = null;
        #endregion

        #region ** MISC VARIABLE DECLARE **
        string strMISCROWNO = string.Empty;
        string strMISCRNO = string.Empty;
        string strMISCSNO = string.Empty;
        string strMISCCOMPNO = string.Empty;
        string strMISCACCUSED = string.Empty;
        string strMISCDESIGNATION = string.Empty;
        string strMISCPRESENTPOSTING = string.Empty;
        string strMISCBRCOMPLAINT = string.Empty;
        string strMISCZONE = string.Empty;
        string strMISCCIRCLEOFFICE = string.Empty;
        string strMISCREGION = string.Empty;
        string strMISCSOURCE = string.Empty;
        string strMISCSOURCEREF = string.Empty;
        string strMISCSENTTO = string.Empty;
        Int32 strMISCCATANO = 0;
        Int32 strMISCCATBNO = 0;
        Int32 strMISCASNO = 0;
        string strMISCNATURECOMP = string.Empty;
        string strMISCACCOUNTNAME = string.Empty;
        string strMISCALLEGATIONS = string.Empty;
        string strMISCREMINDERS = string.Empty;
        string strMISCSTATUS = string.Empty;
        string strMISCSTATUSCODE = string.Empty;
        string strMISCPENDINGWITH = string.Empty;
        string strMISCNAMEOFINVOFFICIAL = string.Empty;
        Int32 strMISCDAYSTAKEN = 0;
        string strMISCFINALACTION = string.Empty;
        string strMISCCASENO = string.Empty;
        string strMISCCASECLOSE = string.Empty;
        string strTYPE = string.Empty;
        string strMISCAPLAN = string.Empty;
        string strMISCREGISTER = string.Empty;
        string strMISCNATURE = string.Empty;
        string strMISCREASONSFORCLOSURE = string.Empty;
        string strMISCBANKNAME = string.Empty;

        decimal decMISCAMOUNT = 0;

        DateTime? dtMISCRECDATECOMP = null;
        DateTime? dtNPADATE = null;
        DateTime? dtMISCSOURCEDATE = null;
        DateTime? dtDTINVESTIGATION = null;
        DateTime? dtMISCSENTFORINVDATE = null;
        DateTime? dtMISCDTIAC = null;
        DateTime? dtMISCDTOFINVREPORT = null;
        DateTime? dtMISCCLOSUREDT = null;
        DateTime? dtMISCRYSENT = null;
        #endregion

        #region ** SFI VARIABLE DECLARE **

        string strSFISINO = string.Empty;
        string strSFIROWNO = string.Empty;
        string strSFIUNIQUEID = string.Empty;
        string strSFIRCNO = string.Empty;
        string strSFIPFNO = string.Empty;
        string strSFINAME = string.Empty;
        string strSFIDESIGNATION = string.Empty;
        string strSFICIRCLE = string.Empty;
        string strSFIBRANCH = string.Empty;
        string strSFIDA = string.Empty;
        string strSFIDAVIEW = string.Empty;
        string strSFILETTERTOCBISENTBY = string.Empty;
        string strSFISTATUS = string.Empty;
        string strSFIREMARKS = string.Empty;
        string strSFIBANKNAME = string.Empty;

        DateTime? dtSFIRCDATE = null;
        DateTime? dtSFIREPORTRECVDATE = null;
        DateTime? dtSFILETTERTOCBIDATE = null;
        #endregion

        #region ** SFP VARIABLE DECLARE **

        string strSFPRNO = string.Empty;
        string strSFPROWNO = string.Empty;
        string strSFPUNIQUEID = string.Empty;
        string strSFPSPNO = string.Empty;
        string strSFPRCNO = string.Empty;
        string strSFPPFNO = string.Empty;
        string strSFPNAME = string.Empty;
        string strSFPDESIGNATION = string.Empty;
        string strSFPCIRCLE = string.Empty;
        string strSFPBRANCH = string.Empty;
        string strSFPDA = string.Empty;
        string strSFPDAVIEW = string.Empty;
        string strSFPCVCVIEW = string.Empty;
        string strSFPSTATUS = string.Empty;
        string strSFPREMARKS = string.Empty;
        string strSFPBANKNAME = string.Empty;

        DateTime? dtSFPRCDATE = null;
        DateTime? dtSFPREPORTRECVDATE = null;
        DateTime? dtSFPLETTERTOCBIDATE = null;
        DateTime? dtSFPLETTERTOCVCDATE = null;
        DateTime? dtSFPLETTERTODADATE = null;
        DateTime? dtSFPDAORDERTOCBIDATE = null;
        #endregion

        #region ** RRB VARIABLE DECLARE **

        string strRRBRNO = string.Empty;
        string strRRBROWNO = string.Empty;
        string strRRBUNIQUEID = string.Empty;
        string strRRBRNO1 = string.Empty;
        string strRRBNAMEOFPARTICULARS = string.Empty;
        string strRRBNAME = string.Empty;
        string strRRBSCALE = string.Empty;
        string strRRBBRNAME = string.Empty;
        string strRRBCIRCLEOFFICE = string.Empty;
        string strRRBLAPSENATURE = string.Empty;
        string strRRBCBI_RC_NO1 = string.Empty;
        string strRRBCBI_RC_NO2 = string.Empty;
        string strRRBNAT_CHSHEET = string.Empty;
        string strRRBNAME_PO = string.Empty;
        string strRRBNAME_EO = string.Empty;
        string strRRBNA_PUN_DA = string.Empty;
        string strRRBFINAL = string.Empty;
        string strRRBDISP_AUTHORITY = string.Empty;
        string strRRBDISAUTHORITYSZONE = string.Empty;
        string strRRBSTATUS = string.Empty;
        string strRRBSTATUSCODE = string.Empty;
        string strRRBREGISTER = string.Empty;
        string strRRBPFNUMBER = string.Empty;
        string strRRBDAPROPOSAL = string.Empty;
        string strRRBADVICECVOI = string.Empty;
        string strRRBDAPROPOSAL_2 = string.Empty;
        string strRRBADVICECVO2 = string.Empty;
        string strRRBDESK_USER_REMARKS = string.Empty;
        string strRRBBANKNAME = string.Empty;

        DateTime? dtRRBDTCHARGE = null;
        DateTime? dtRRBDTRNO = null;
        DateTime? dtRRBDTOFRETIREMENT = null;
        DateTime? dtRRBDTRC1 = null;
        DateTime? dtRRBDTRC2 = null;
        DateTime? dtRRBDTAPPPO = null;
        DateTime? dtRRBDTAPPEO = null;
        DateTime? dtRRBDTORDDA = null;
        DateTime? dtRRBDATEOFCLOSURE = null;
        DateTime? dtRRBDTISTDA = null;
        DateTime? dtRRBDTCVOADVICE = null;
        DateTime? dtRRBDT2NDDA = null;
        DateTime? dtRRBDTCVOADVICE2 = null;
        #endregion

        #region ** VIGM VARIABLE DECLARE **

        Int32 strVIGMSTATUSCODE = 0;
        Int32 strVIGMSTATE = 0;
        Int32 strVIGMCIRCLEOFFICE = 0;

        decimal decVIGMAMOUNT = 0;

        string strVIGMRNO = string.Empty;
        string strVIGMROWNO = string.Empty;
        string strVIGMUNIQUEID = string.Empty;
        string strVIGMNAME = string.Empty;
        string strVIGMPFNUMBER = string.Empty;
        string strVIGMBRANCH = string.Empty;
        string strVIGMVIGCASE = string.Empty;
        string strVIGMDAREFNO = string.Empty;
        string strVIGMSCALE = string.Empty;
        string strVIGMDESIGNATION = string.Empty;
        string strVIGMUS = string.Empty;
        string strVIGMLAPSENATURE = string.Empty;
        string strVIGMSOURCE = string.Empty;
        string strVIGMACCOUNTNAME = string.Empty;
        string strVIGMCBIRCNO1 = string.Empty;
        string strVIGMSTATUS = string.Empty;
        string strVIGMBANKNAME = string.Empty;
        string strEXTERNALSOURCE = string.Empty;

        DateTime? dtVIGMRNODATE = null;
        DateTime? dtVIGMRETIREMENTDATE = null;
        DateTime? dtVIGMSUSPENSIONDATE = null;
        DateTime? dtVIGMREVOCATIONDATE = null;
        DateTime? dtEXTERNALSOURCEDATE = null;
        DateTime? dtVIGMCLOSUREDATE = null;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();

            }

            strUser = ViewState["USERNAME"].ToString();
            ddlTableName.Focus();
            lblMsg.Text = string.Empty;

            btnVerify.Attributes.Add("onclick", "return funcUpload_Validation('" + ddlTableName.ClientID + "')");
        }

        public void funcbindColumnName(string TABLENAME)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spUploadTableColumn_Get]";
                cmd.Parameters.AddWithValue("@p_TABLENAME", TABLENAME);
                cmd.CommandTimeout = 0;

                sda.Fill(dt);
                funcDownloadExcelFormat(dt, TABLENAME);
            }
            catch (Exception e)
            {
                lblMsg.Text = e.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e);
            }
            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public void funcDownloadExcelFormat(DataTable dt, string FILENAME)
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, FILENAME);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("", "");
                    Response.AddHeader("content-disposition", "attachment;filename=" + FILENAME + ".xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception e1)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e1);
            }
        }

        protected void ddlDownloadFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            string TABLENAME = objCommonFunction.ddlSelectedValue(ddlDownloadFormat);

            if (!string.IsNullOrEmpty(TABLENAME))
            {
                funcbindColumnName(TABLENAME);
            }
        }

        public void funcClear()
        {
            ddlTableName.SelectedIndex = 0;
            btnUpload.Visible = false;
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            try
            {
                strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);

                if (fileUpload.HasFile)
                {
                    strFileName = Path.GetFileName(fileUpload.PostedFile.FileName);
                    strExtension = Path.GetExtension(fileUpload.PostedFile.FileName);
                    strFolderPath = ConfigurationManager.AppSettings["ExcelFolderPath"];
                    strFilePath = Server.MapPath(strFolderPath + strFileName);
                    fileUpload.SaveAs(strFilePath);

                    if (strTableValue.Equals("COMPLAINT"))
                    {
                        funcExcelVerify_COMPLAINT(strFilePath, strExtension);
                    }
                    else if (strTableValue.ToUpper().Equals("IAC"))
                    {
                        funcExcelVerify_IAC(strFilePath, strExtension);
                    }
                    else if (strTableValue.Equals("MISC"))
                    {
                        funcExcelVerify_MISC(strFilePath, strExtension);
                    }
                    else if (strTableValue.Equals("NOC"))
                    {
                        funcExcelVerify_NOC(strFilePath, strExtension);
                    }
                    else if (strTableValue.Equals("RTI"))
                    {
                        funcExcelVerify_RTI(strFilePath, strExtension);
                    }
                    else if (strTableValue.Equals("RRB"))
                    {
                        funcExcelVerify_RRB(strFilePath, strExtension);
                    }
                    else if (strTableValue.Equals("SR"))
                    {
                        funcExcelVerify_SR(strFilePath, strExtension);
                    }
                    else if (strTableValue.Equals("SANCTION_FOR_INVESTIGATION"))
                    {
                        funcExcelVerify_SanctionForInvestigation(strFilePath, strExtension);
                    }
                    else if (strTableValue.Equals("SANCTION_FOR_PROSECUTION"))
                    {
                        funcExcelVerify_SanctionForProsecution(strFilePath, strExtension);
                    }
                    else if (strTableValue.ToUpper().Equals("VIGILANCE"))
                    {
                        funcExcelVerify_VIG(strFilePath, strExtension);
                    }
                    else if (strTableValue.Equals("VIGILANCEMIS"))
                    {
                        funcExcelVerify_VigilanceMIS(strFilePath, strExtension);
                    }
                    else if (strTableValue.Equals("WB"))
                    {
                        funcExcelVerify_WB(strFilePath, strExtension);
                    }
                    else if (strTableValue.Equals("LODI"))
                    {
                        funcExcelVerify_LODI(strFilePath, strExtension);
                    }
                }
            }
            catch (Exception e12)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e12);
            }
        }


        protected void funcExcelVerify_LODI(string p_strFilePath, string p_strExtension)
        {
            string conStr = "";
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    RowNo = Convert.ToString(row["ROWNO"]);
                    RNo = Convert.ToString(row["LODINO"]);
                }
                catch (Exception e5)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e5);
                    lblMsg.Text = "Upload Failed ! Please check your Excel Sheet";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if ((!string.IsNullOrEmpty(RowNo)) && (!string.IsNullOrEmpty(RNo)) && (!string.IsNullOrEmpty(BankName)))
                {
                    RowNo = Convert.ToString(row["ROWNO"]);
                    RNo = Convert.ToString(row["LODINO"]);
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spExcelVerify_Get]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SNO", RNo);
                    cmd.Parameters.AddWithValue("@p_ROWNO", RowNo);
                    cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e6)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e6);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();

            if (intErrCode == 1)
            {
                btnUpload.Visible = true;
                ViewState["LODIEXCELDETAILS"] = dt;
            }
        }

        protected void funcExcelVerify_COMPLAINT(string p_strFilePath, string p_strExtension)
        {
            string conStr = "";
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    RowNo = Convert.ToString(row["RowNo"]);
                    RNo = Convert.ToString(row["RNo"]);
                    BankName = Convert.ToString(row["BankName"]);
                }
                catch (Exception e5)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e5);
                    lblMsg.Text = "Upload Failed ! Please check your Excel Sheet";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if ((!string.IsNullOrEmpty(RowNo)) && (!string.IsNullOrEmpty(RNo)) && (!string.IsNullOrEmpty(BankName)))
                {
                    RowNo = Convert.ToString(row["RowNo"]);
                    RNo = Convert.ToString(row["RNo"]);
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spExcelVerify_Get]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SNO", RNo);
                    cmd.Parameters.AddWithValue("@p_ROWNO", RowNo);
                    cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e6)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e6);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();

            if (intErrCode == 1)
            {
                btnUpload.Visible = true;
                ViewState["COMPEXCELDETAILS"] = dt;
            }
        }

        protected void funcExcelVerify_IAC(string p_strFilePath, string p_strExtension)
        {
            string conStr = "";
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strSNO = row["IACNO"].ToString();
                }
                catch (Exception e3)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e3);
                    lblMsg.Text = "Upload Failed ! Please check your Excel Sheet";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strSNO != "")
                {
                    strROWNO = row["ROWNO"].ToString();
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spExcelVerify_Get]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SNO", strSNO);
                    cmd.Parameters.AddWithValue("@p_ROWNO", strROWNO);
                    cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e4)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e4);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();
            if (intErrCode == 1)
            {
                btnUpload.Visible = true;
                ViewState["IACEXCELDETAILS"] = dt;
            }
        }

        protected void funcExcelVerify_VIG(string p_strFilePath, string p_strExtension)
        {
            string conStr = "";
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strSNO = row["RNO"].ToString();
                }
                catch (Exception e5)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e5);
                    lblMsg.Text = "Upload Failed ! Please check your Excel Sheet";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strSNO != "")
                {
                    strROWNO = row["ROWNO"].ToString();
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spExcelVerify_Get]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SNO", strSNO);
                    cmd.Parameters.AddWithValue("@p_ROWNO", strROWNO);
                    cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e6)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e6);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();

            if (intErrCode == 1)
            {
                btnUpload.Visible = true;
                ViewState["VIGEXCELDETAILS"] = dt;
            }
        }

        protected void funcExcelVerify_NOC(string p_strFilePath, string p_strExtension)
        {
            string conStr = "";
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strNOCSNO = row["SNO"].ToString();
                }
                catch (Exception e5)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e5);
                    lblMsg.Text = "Upload Failed ! Please check your Excel Sheet";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strSNO != "")
                {
                    strNOCROWNO = row["ROWNO"].ToString();
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spExcelVerify_Get]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SNO", strNOCSNO);
                    cmd.Parameters.AddWithValue("@p_ROWNO", strNOCROWNO);
                    cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e6)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e6);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();

            if (intErrCode == 1)
            {
                btnUpload.Visible = true;
                ViewState["NOCEXCELDETAILS"] = dt;
            }
        }

        protected void funcExcelVerify_RTI(string p_strFilePath, string p_strExtension)
        {
            string conStr = "";
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strRTINO = row["RTINO"].ToString();
                }
                catch (Exception e5)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e5);
                    lblMsg.Text = "Upload Failed ! Please check your Excel Sheet";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strRTINO != "")
                {
                    strRTIROWNO = row["ROWNO"].ToString();
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spExcelVerify_Get]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SNO", strRTINO);
                    cmd.Parameters.AddWithValue("@p_ROWNO", strRTIROWNO);
                    cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e6)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e6);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();

            if (intErrCode == 1)
            {
                btnUpload.Visible = true;
                ViewState["RTIEXCELDETAILS"] = dt;
            }
        }

        protected void funcExcelVerify_SR(string p_strFilePath, string p_strExtension)
        {
            string conStr = "";
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strSRNO = row["SRNO"].ToString();
                }
                catch (Exception e5)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e5);
                    lblMsg.Text = "Upload Failed ! Please check your Excel Sheet";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strSRNO != "")
                {
                    strSRROWNO = row["ROWNO"].ToString();
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spExcelVerify_Get]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SNO", strSRNO);
                    cmd.Parameters.AddWithValue("@p_ROWNO", strSRROWNO);
                    cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e6)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e6);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();

            if (intErrCode == 1)
            {
                btnUpload.Visible = true;
                ViewState["SREXCELDETAILS"] = dt;
            }
        }

        protected void funcExcelVerify_WB(string p_strFilePath, string p_strExtension)
        {
            string conStr = "";
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strWBRNO = row["RNO"].ToString();
                }
                catch (Exception e5)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e5);
                    lblMsg.Text = "Upload Failed ! Please check your Excel Sheet";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strWBRNO != "")
                {
                    strWBROWNO = row["ROWNO"].ToString();
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spExcelVerify_Get]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SNO", strWBRNO);
                    cmd.Parameters.AddWithValue("@p_ROWNO", strWBROWNO);
                    cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e6)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e6);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();

            if (intErrCode == 1)
            {
                btnUpload.Visible = true;
                ViewState["WBEXCELDETAILS"] = dt;
            }
        }

        protected void funcExcelVerify_MISC(string p_strFilePath, string p_strExtension)
        {
            string conStr = "";
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strMISCRNO = row["RNO"].ToString();
                }
                catch (Exception e5)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e5);
                    lblMsg.Text = "Upload Failed ! Please check your Excel Sheet";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strMISCRNO != "")
                {
                    strMISCROWNO = row["ROWNO"].ToString();
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spExcelVerify_Get]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SNO", strMISCRNO);
                    cmd.Parameters.AddWithValue("@p_ROWNO", strMISCROWNO);
                    cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e6)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e6);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();

            if (intErrCode == 1)
            {
                btnUpload.Visible = true;
                ViewState["MISCEXCELDETAILS"] = dt;
            }
        }

        protected void funcExcelVerify_SanctionForInvestigation(string p_strFilePath, string p_strExtension)
        {
            string conStr = "";
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strSFISINO = row["SFI_SINO"].ToString();
                }
                catch (Exception e5)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e5);
                    lblMsg.Text = "Upload Failed ! Please check your Excel Sheet";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strSFISINO != "")
                {
                    strSFIROWNO = row["ROWNO"].ToString();
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spExcelVerify_Get]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SNO", strSFISINO);
                    cmd.Parameters.AddWithValue("@p_ROWNO", strSFIROWNO);
                    cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e6)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e6);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();

            if (intErrCode == 1)
            {
                btnUpload.Visible = true;
                ViewState["SFIEXCELDETAILS"] = dt;
            }
        }

        protected void funcExcelVerify_SanctionForProsecution(string p_strFilePath, string p_strExtension)
        {
            string conStr = "";
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strSFPRNO = row["SFP_SPNO"].ToString();
                }
                catch (Exception e5)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e5);
                    lblMsg.Text = "Upload Failed ! Please check your Excel Sheet";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strSFPRNO != "")
                {
                    strSFPROWNO = row["ROWNO"].ToString();
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spExcelVerify_Get]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SNO", strSFPRNO);
                    cmd.Parameters.AddWithValue("@p_ROWNO", strSFPROWNO);
                    cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e6)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e6);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();

            if (intErrCode == 1)
            {
                btnUpload.Visible = true;
                ViewState["SFPEXCELDETAILS"] = dt;
            }
        }

        protected void funcExcelVerify_RRB(string p_strFilePath, string p_strExtension)
        {
            string conStr = "";
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strRRBRNO = row["RNO"].ToString();
                }
                catch (Exception e5)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e5);
                    lblMsg.Text = "Upload Failed ! Please check your Excel Sheet";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strRRBRNO != "")
                {
                    strRRBROWNO = row["ROWNO"].ToString();
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spExcelVerify_Get]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SNO", strRRBRNO);
                    cmd.Parameters.AddWithValue("@p_ROWNO", strRRBROWNO);
                    cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e6)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e6);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();

            if (intErrCode == 1)
            {
                btnUpload.Visible = true;
                ViewState["RRBEXCELDETAILS"] = dt;
            }
        }

        protected void funcExcelVerify_VigilanceMIS(string p_strFilePath, string p_strExtension)
        {
            string conStr = "";
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strVIGMRNO = row["VIGM_RNO"].ToString();
                }
                catch (Exception e5)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e5);
                    lblMsg.Text = "Upload Failed ! Please check your Excel Sheet";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strVIGMRNO != "")
                {
                    strVIGMROWNO = row["ROWNO"].ToString();
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spExcelVerify_Get]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SNO", strVIGMRNO);
                    cmd.Parameters.AddWithValue("@p_ROWNO", strVIGMROWNO);
                    cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e6)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e6);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();

            if (intErrCode == 1)
            {
                btnUpload.Visible = true;
                ViewState["VIGMEXCELDETAILS"] = dt;
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);

            if (fileUpload.HasFile)
            {
                strFileName = Path.GetFileName(fileUpload.PostedFile.FileName);
                strExtension = Path.GetExtension(fileUpload.PostedFile.FileName);
                strFolderPath = ConfigurationManager.AppSettings["ExcelFolderPath"];
                strFilePath = Server.MapPath(strFolderPath + strFileName);
                fileUpload.SaveAs(strFilePath);
            }

            if (strTableValue.Equals("COMPLAINT"))
            {
                funcExcelImport_COMPLAINT();
            }
            else if (strTableValue.ToUpper() == "IAC")
            {
                funcExcelImport_IAC();
            }
            else if (strTableValue.Equals("MISC"))
            {
                funcExcelImport_MISC();
            }
            else if (strTableValue.Equals("NOC"))
            {
                funcExcelImport_NOC();
            }
            else if (strTableValue.Equals("RTI"))
            {
                funcExcelImport_RTI();
            }
            else if (strTableValue.Equals("RRB"))
            {
                funcExcelImport_RRB();
            }
            else if (strTableValue.Equals("SR"))
            {
                funcExcelImport_SR();
            }
            else if (strTableValue.Equals("SANCTION_FOR_INVESTIGATION"))
            {
                funcExcelImport_SanctionForInvestigation();
            }
            else if (strTableValue.Equals("SANCTION_FOR_PROSECUTION"))
            {
                funcExcelImport_SanctionForProsecution();
            }

            else if (strTableValue.ToUpper() == "VIGILANCE")
            {
                funcExcelImport_Vigilance();
            }
            else if (strTableValue.Equals("VIGILANCEMIS"))
            {
                funcExcelImport_VigilanceMIS();
            }
            else if (strTableValue.Equals("WB"))
            {
                funcExcelImport_WB();
            }
            else if (strTableValue.Equals("LODI"))
            {
                funcExcelImport_LODI();
            }

            funcClear();
        }

        protected void funcExcelImport_COMPLAINT()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["COMPEXCELDETAILS"]);
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlTransaction txn = null;

            try
            {
                con.Open();
                cmd.Connection = con;
                txn = cmd.Connection.BeginTransaction();
                cmd.Transaction = txn;

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string ID = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                        string UniqueID = "CMP" + DateTime.Now.ToString("ddMMyy") + ID;
                        Amount = objCommonFunction.convertToDecimal(Convert.ToString(row["Amount"]));

                        string strCompRecDt = Convert.ToString(row["ComplaintRecDate"]);
                        if (!string.IsNullOrEmpty(strCompRecDt))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strCompRecDt, out date))
                                ComplaintRecDate = date;
                        }

                        string strClosureDate = Convert.ToString(row["ClosureDate"]);
                        if (!string.IsNullOrEmpty(strClosureDate))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strClosureDate, out date))
                                ClosureDate = date;
                        }

                        string strIACDate = Convert.ToString(row["IACDate"]);
                        if (!string.IsNullOrEmpty(strIACDate))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strIACDate, out date))
                                IACDate = date;
                        }

                        string strSourceDate = Convert.ToString(row["SourceDate"]);
                        if (!string.IsNullOrEmpty(strSourceDate))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strSourceDate, out date))
                                SourceDate = date;
                        }

                        string strSentforInvDate = Convert.ToString(row["SentforInvDate"]);
                        if (!string.IsNullOrEmpty(strSentforInvDate))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strSentforInvDate, out date))
                                SentforInvDate = date;
                        }

                        string strForInvReportDate = Convert.ToString(row["ForInvReportDate"]);
                        if (!string.IsNullOrEmpty(strForInvReportDate))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strForInvReportDate, out date))
                                ForInvReportDate = date;
                        }

                        string strLetterSentDate = Convert.ToString(row["LetterSentDate"]);
                        if (!string.IsNullOrEmpty(strLetterSentDate))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strLetterSentDate, out date))
                                LetterSentDate = date;
                        }

                        string strRYSent = Convert.ToString(row["RYSent"]);
                        if (!string.IsNullOrEmpty(strRYSent))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strRYSent, out date))
                                RYSent = date;
                        }

                        cmd.Parameters.Clear();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[dbo].[spComplaintExcel_Import]";

                        SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                        SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(sqlErrMsgOutput);
                        cmd.Parameters.Add(sqlErrCodeOutput);

                        cmd.Parameters.AddWithValue("@p_RNO", Convert.ToString(row["RNo"]));
                        cmd.Parameters.AddWithValue("@p_ComplaintNo", Convert.ToString(row["ComplaintNo"]));
                        cmd.Parameters.AddWithValue("@p_ComplaintRecDate", ComplaintRecDate);
                        cmd.Parameters.AddWithValue("@p_BRComplaint", Convert.ToString(row["BRComplaint"]));
                        cmd.Parameters.AddWithValue("@p_Zone", Convert.ToString(row["Zone"]));
                        cmd.Parameters.AddWithValue("@p_Circle", Convert.ToString(row["Circle"]));
                        cmd.Parameters.AddWithValue("@p_InternalRefNo", Convert.ToString(row["InternalRefNo"]));
                        cmd.Parameters.AddWithValue("@p_Accused", Convert.ToString(row["Accused"]));
                        cmd.Parameters.AddWithValue("@p_Allegations", Convert.ToString(row["Allegations"]));
                        cmd.Parameters.AddWithValue("@p_IACNo", Convert.ToString(row["IACNo"]));
                        cmd.Parameters.AddWithValue("@p_PresentPosting", Convert.ToString(row["PresentPosting"]));
                        cmd.Parameters.AddWithValue("@p_State", Convert.ToString(row["State"]));
                        cmd.Parameters.AddWithValue("@p_SentTo", Convert.ToString(row["SentTo"]));
                        cmd.Parameters.AddWithValue("@p_SourceDate", SourceDate);
                        cmd.Parameters.AddWithValue("@p_SourceRef", Convert.ToString(row["SourceRef"]));
                        cmd.Parameters.AddWithValue("@p_AccountName", Convert.ToString(row["AccountName"]));
                        cmd.Parameters.AddWithValue("@p_ExternalSource", Convert.ToString(row["ExternalSource"]));
                        cmd.Parameters.AddWithValue("@p_Region", Convert.ToString(row["Region"]));
                        cmd.Parameters.AddWithValue("@p_Close", Convert.ToString(row["Close"]));
                        cmd.Parameters.AddWithValue("@p_ForInvReportDate", ForInvReportDate);
                        cmd.Parameters.AddWithValue("@p_Designation", Convert.ToString(row["Designation"]));
                        cmd.Parameters.AddWithValue("@p_NameINVOfficial", Convert.ToString(row["NameINVOfficial"]));
                        cmd.Parameters.AddWithValue("@p_StatusCode", Convert.ToString(row["StatusCode"]));
                        cmd.Parameters.AddWithValue("@p_ClosureReasons", Convert.ToString(row["ClosureReasons"]));
                        cmd.Parameters.AddWithValue("@p_PFNumber", Convert.ToString(row["PFNumber"]));
                        cmd.Parameters.AddWithValue("@p_LetterSentDate", LetterSentDate);
                        cmd.Parameters.AddWithValue("@p_LetterSentTo", Convert.ToString(row["LetterSentTo"]));
                        cmd.Parameters.AddWithValue("@p_BankName", Convert.ToString(row["BankName"]));
                        cmd.Parameters.AddWithValue("@p_Status", Convert.ToString(row["Status"]));

                        cmd.Parameters.AddWithValue("@p_ClosureDate", ClosureDate);
                        cmd.Parameters.AddWithValue("@p_IACDate", IACDate);
                        cmd.Parameters.AddWithValue("@p_Amount", Amount);
                        cmd.Parameters.AddWithValue("@p_SentforInvDate", SentforInvDate);
                        cmd.Parameters.AddWithValue("@p_RYSent", RYSent);

                        cmd.Parameters.AddWithValue("@p_ADDUSER", Convert.ToString(Session["userid"]));
                        cmd.Parameters.AddWithValue("@p_ADDUSERIP", objCommonFunction.funcGetUserIP());

                        cmd.CommandTimeout = 0;
                        intErrCode = 0;
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            intTotalRowInsert = intTotalRowInsert + 1;
                            strErrMsg = sqlErrMsgOutput.Value.ToString();
                            intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                        }
                        else
                        {
                            lblMsg.Text = "Error during Insert Complaint Data";
                            return;
                        }

                    }
                    if (intErrCode.Equals(1))
                    {
                        txn.Commit();
                        lblMsg.Text = intTotalRowInsert + " records added successfully";
                    }
                }
                else
                {
                    lblMsg.Text = "Error - no record in Uploaded Excel sheet....!";
                    return;
                }
            }
            catch (Exception ex)
            {
                txn.Rollback();
                lblMsg.Text = ex.Message;
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                txn.Dispose();
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
        }

        protected void funcExcelImport_MISC()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["MISCEXCELDETAILS"]);

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strMISCRNO = row["RNO"].ToString();
                }
                catch (Exception)
                {
                    lblMsg.Text = "Upload Failed ! Please check your EXECL";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strMISCRNO != "")
                {
                    #region ** assian Variable Value **
                    strMISCROWNO = Convert.ToString(row["ROWNO"]);
                    strMISCSNO = Convert.ToString(row["RNO"]);
                    strMISCCOMPNO = Convert.ToString(row["COMPNO"]);
                    strMISCACCUSED = Convert.ToString(row["ACCUSED"]);
                    strMISCDESIGNATION = Convert.ToString(row["DESIGNATION"]);
                    strMISCPRESENTPOSTING = Convert.ToString(row["PRESENTPOSTING"]);
                    strMISCBRCOMPLAINT = Convert.ToString(row["BRCOMPLAINT"]);
                    strMISCZONE = Convert.ToString(row["ZONE"]);
                    strMISCCIRCLEOFFICE = Convert.ToString(row["CIRCLEOFFICE"]);
                    strMISCREGION = Convert.ToString(row["REGION"]);
                    strMISCSOURCE = Convert.ToString(row["SOURCE"]);
                    strMISCSOURCEREF = Convert.ToString(row["SOURCEREF"]);
                    strMISCSENTTO = Convert.ToString(row["SENTTO"]);
                    strMISCCATANO = objCommonFunction.convertToInt(Convert.ToString(row["CATANO"]));
                    strMISCCATBNO = objCommonFunction.convertToInt(Convert.ToString(row["CATBNO"]));
                    strMISCASNO = objCommonFunction.convertToInt(Convert.ToString(row["ASNO"]));
                    strMISCNATURECOMP = Convert.ToString(row["NATURECOMP"]);
                    strMISCACCOUNTNAME = Convert.ToString(row["ACCOUNTNAME"]);
                    strMISCALLEGATIONS = Convert.ToString(row["ALLEGATIONS"]);
                    strMISCREMINDERS = Convert.ToString(row["REMINDERS"]);
                    strMISCSTATUS = Convert.ToString(row["STATUS"]);
                    strMISCSTATUSCODE = Convert.ToString(row["STATUSCODE"]);
                    strMISCPENDINGWITH = Convert.ToString(row["PENDINGWITH"]);
                    strMISCNAMEOFINVOFFICIAL = Convert.ToString(row["NAMEOFINVOFFICIAL"]);
                    strMISCDAYSTAKEN = objCommonFunction.convertToInt(Convert.ToString(row["DAYSTAKEN"]));
                    strMISCFINALACTION = Convert.ToString(row["FINALACTION"]);
                    strMISCCASENO = Convert.ToString(row["CASENO"]);
                    strMISCCASECLOSE = Convert.ToString(row["CASECLOSE"]);
                    strTYPE = Convert.ToString(row["TYPE"]);
                    strMISCAPLAN = Convert.ToString(row["APLAN"]);
                    strMISCREGISTER = Convert.ToString(row["REGISTER"]);
                    strMISCNATURE = Convert.ToString(row["NATURE"]);
                    strMISCREASONSFORCLOSURE = Convert.ToString(row["REASONSFORCLOSURE"]);
                    strMISCBANKNAME = Convert.ToString(row["BANKNAME"]);

                    TextBox txtAmount = new TextBox();
                    txtAmount.Text = row["AMOUNT"].ToString();
                    decMISCAMOUNT = objCommonFunction.convertToDecimal(txtAmount);
                    #endregion

                    #region ** convert Date **
                    string strRECDATECOMP = row["RECDATECOMP"].ToString();
                    if (!string.IsNullOrEmpty(strRECDATECOMP))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRECDATECOMP, out date))
                            dtMISCRECDATECOMP = date;
                    }

                    string strNPADATE = row["NPADATE"].ToString();
                    if (!string.IsNullOrEmpty(strNPADATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strNPADATE, out date))
                            dtNPADATE = date;
                    }

                    string strSOURCEDATE = row["SOURCEDATE"].ToString();
                    if (!string.IsNullOrEmpty(strSOURCEDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSOURCEDATE, out date))
                            dtMISCSOURCEDATE = date;
                    }

                    string strDTINVESTIGATION = row["DTINVESTIGATION"].ToString();
                    if (!string.IsNullOrEmpty(strDTINVESTIGATION))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strDTINVESTIGATION, out date))
                            dtDTINVESTIGATION = date;
                    }

                    string strSENTFORINVDATE = row["SENTFORINVDATE"].ToString();
                    if (!string.IsNullOrEmpty(strSENTFORINVDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSENTFORINVDATE, out date))
                            dtMISCSENTFORINVDATE = date;
                    }

                    string strDTIAC = row["DTIAC"].ToString();
                    if (!string.IsNullOrEmpty(strDTIAC))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strDTIAC, out date))
                            dtMISCDTIAC = date;
                    }

                    string strDTOFINVREPORT = row["DTOFINVREPORT"].ToString();
                    if (!string.IsNullOrEmpty(strDTOFINVREPORT))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strDTOFINVREPORT, out date))
                            dtMISCDTOFINVREPORT = date;
                    }

                    string strCLOSUREDT = row["CLOSUREDT"].ToString();
                    if (!string.IsNullOrEmpty(strCLOSUREDT))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strCLOSUREDT, out date))
                            dtMISCCLOSUREDT = date;
                    }

                    string strRYSENT = row["RYSENT"].ToString();
                    if (!string.IsNullOrEmpty(strRYSENT))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRYSENT, out date))
                            dtMISCRYSENT = date;
                    }
                    #endregion
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spMISCExcel_Import]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_RNO", strMISCRNO);
                    cmd.Parameters.AddWithValue("@p_COMPNO", strMISCCOMPNO);
                    cmd.Parameters.AddWithValue("@p_ACCUSED", strMISCACCUSED);
                    cmd.Parameters.AddWithValue("@p_DESIGNATION", strMISCDESIGNATION);
                    cmd.Parameters.AddWithValue("@p_PRESENTPOSTING", strMISCPRESENTPOSTING);
                    cmd.Parameters.AddWithValue("@p_BRCOMPLAINT", strMISCBRCOMPLAINT);
                    cmd.Parameters.AddWithValue("@p_ZONE", strMISCZONE);
                    cmd.Parameters.AddWithValue("@p_CIRCLEOFFICE", strMISCCIRCLEOFFICE);
                    cmd.Parameters.AddWithValue("@p_REGION", strMISCREGION);
                    cmd.Parameters.AddWithValue("@p_RECDATECOMP", dtMISCRECDATECOMP);
                    cmd.Parameters.AddWithValue("@p_NPADATE", dtNPADATE);
                    cmd.Parameters.AddWithValue("@p_SOURCE", strMISCSOURCE);
                    cmd.Parameters.AddWithValue("@p_SOURCEREF", strMISCSOURCEREF);
                    cmd.Parameters.AddWithValue("@p_SOURCEDATE", dtMISCSOURCEDATE);
                    cmd.Parameters.AddWithValue("@p_DTINVESTIGATION", dtDTINVESTIGATION);
                    cmd.Parameters.AddWithValue("@p_SENTTO", strMISCSENTTO);
                    cmd.Parameters.AddWithValue("@p_SENTFORINVDATE", dtMISCSENTFORINVDATE);
                    cmd.Parameters.AddWithValue("@p_CATANO", strMISCCATANO);
                    cmd.Parameters.AddWithValue("@p_CATBNO", strMISCCATBNO);
                    cmd.Parameters.AddWithValue("@p_ASNO", strMISCASNO);
                    cmd.Parameters.AddWithValue("@p_NATURECOMP", strMISCNATURECOMP);
                    cmd.Parameters.AddWithValue("@p_ACCOUNTNAME", strMISCACCOUNTNAME);
                    cmd.Parameters.AddWithValue("@p_AMOUNT", decMISCAMOUNT);
                    cmd.Parameters.AddWithValue("@p_ALLEGATIONS", strMISCALLEGATIONS);
                    cmd.Parameters.AddWithValue("@p_REMINDERS", strMISCREMINDERS);
                    cmd.Parameters.AddWithValue("@p_DTIAC", dtMISCDTIAC);
                    cmd.Parameters.AddWithValue("@p_STATUS", strMISCSTATUS);
                    cmd.Parameters.AddWithValue("@p_STATUSCODE", strMISCSTATUSCODE);
                    cmd.Parameters.AddWithValue("@p_PENDINGWITH", strMISCPENDINGWITH);
                    cmd.Parameters.AddWithValue("@p_NAMEOFINVOFFICIAL", strMISCNAMEOFINVOFFICIAL);
                    cmd.Parameters.AddWithValue("@p_DTOFINVREPORT", dtMISCDTOFINVREPORT);
                    cmd.Parameters.AddWithValue("@p_DAYSTAKEN", strMISCDAYSTAKEN);
                    cmd.Parameters.AddWithValue("@p_FINALACTION", strMISCFINALACTION);
                    cmd.Parameters.AddWithValue("@p_CASENO", strMISCCASENO);
                    cmd.Parameters.AddWithValue("@p_CASECLOSE", strMISCCASECLOSE);
                    cmd.Parameters.AddWithValue("@p_CLOSUREDT", dtMISCCLOSUREDT);
                    cmd.Parameters.AddWithValue("@p_TYPE", strTYPE);
                    cmd.Parameters.AddWithValue("@p_RYSENT", dtMISCRYSENT);
                    cmd.Parameters.AddWithValue("@p_APLAN", strMISCAPLAN);
                    cmd.Parameters.AddWithValue("@p_REGISTER", strMISCREGISTER);
                    cmd.Parameters.AddWithValue("@p_NATURE", strMISCNATURE);
                    cmd.Parameters.AddWithValue("@p_REASONSFORCLOSURE", strMISCREASONSFORCLOSURE);
                    cmd.Parameters.AddWithValue("@p_BANKNAME", strMISCBANKNAME);

                    cmd.Parameters.AddWithValue("@p_ADDUSER", Session["userid"].ToString());
                    cmd.Parameters.AddWithValue("@p_ADDUSERIP", objCommonFunction.funcGetUserIP());

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e12)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e12);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void funcExcelImport_RRB()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["RRBEXCELDETAILS"]);

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strRRBRNO = row["RNO"].ToString();
                }
                catch (Exception)
                {
                    lblMsg.Text = "Upload Failed ! Please check your EXECL";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strRRBRNO != "")
                {
                    #region ** assian Variable Value **
                    strRRBROWNO = Convert.ToString(row["ROWNO"]);

                    string ID = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                    strRRBUNIQUEID = "RRB" + DateTime.Now.ToString("ddMMyy") + ID;

                    strRRBRNO1 = Convert.ToString(row["RNO1"]);
                    strRRBNAMEOFPARTICULARS = Convert.ToString(row["NAMEOFPARTICULARS"]);
                    strRRBNAME = Convert.ToString(row["NAME"]);
                    strRRBSCALE = Convert.ToString(row["SCALE"]);
                    strRRBBRNAME = Convert.ToString(row["BRNAME"]);
                    strRRBCIRCLEOFFICE = Convert.ToString(row["CIRCLEOFFICE"]);
                    strRRBLAPSENATURE = Convert.ToString(row["LAPSENATURE"]);
                    strRRBCBI_RC_NO1 = Convert.ToString(row["CBI_RC_NO1"]);
                    strRRBCBI_RC_NO2 = Convert.ToString(row["CBI_RC_NO2"]);
                    strRRBNAT_CHSHEET = Convert.ToString(row["NAT_CHSHEET"]);
                    strRRBNAME_PO = Convert.ToString(row["NAME_PO"]);
                    strRRBNAME_EO = Convert.ToString(row["NAME_EO"]);
                    strRRBNA_PUN_DA = Convert.ToString(row["NA_PUN_DA"]);
                    strRRBFINAL = Convert.ToString(row["FINAL"]);
                    strRRBDISP_AUTHORITY = Convert.ToString(row["DISP_AUTHORITY"]);
                    strRRBDISAUTHORITYSZONE = Convert.ToString(row["DISAUTHORITYSZONE"]);
                    strRRBSTATUS = Convert.ToString(row["STATUS"]);
                    strRRBSTATUSCODE = Convert.ToString(row["STATUSCODE"]);
                    strRRBREGISTER = Convert.ToString(row["REGISTER"]);
                    strRRBPFNUMBER = Convert.ToString(row["PFNUMBER"]);
                    strRRBDAPROPOSAL = Convert.ToString(row["DAPROPOSAL"]);
                    strRRBADVICECVOI = Convert.ToString(row["ADVICECVOI"]);
                    strRRBDAPROPOSAL_2 = Convert.ToString(row["DAPROPOSAL_2"]);
                    strRRBADVICECVO2 = Convert.ToString(row["ADVICECVO2"]);
                    strRRBDESK_USER_REMARKS = Convert.ToString(row["DESK_USER_REMARKS"]);
                    strRRBBANKNAME = Convert.ToString(row["BANKNAME"]);

                    #endregion

                    #region ** convert Date **
                    string strRRBDTCHARGE = row["DTCHARGE"].ToString();
                    if (!string.IsNullOrEmpty(strRRBDTCHARGE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRRBDTCHARGE, out date))
                            dtRRBDTCHARGE = date;
                    }

                    string strRRBDTRNO = row["DTRNO"].ToString();
                    if (!string.IsNullOrEmpty(strRRBDTRNO))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRRBDTRNO, out date))
                            dtRRBDTRNO = date;
                    }

                    string strRRBDTOFRETIREMENT = row["DTOFRETIREMENT"].ToString();
                    if (!string.IsNullOrEmpty(strRRBDTOFRETIREMENT))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRRBDTOFRETIREMENT, out date))
                            dtRRBDTOFRETIREMENT = date;
                    }

                    string strRRBDTRC1 = row["DT_RC1"].ToString();
                    if (!string.IsNullOrEmpty(strRRBDTRC1))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRRBDTRC1, out date))
                            dtRRBDTRC1 = date;
                    }

                    string strRRBDTRC2 = row["DT_RC2"].ToString();
                    if (!string.IsNullOrEmpty(strRRBDTRC2))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRRBDTRC2, out date))
                            dtRRBDTRC2 = date;
                    }

                    string strRRBDTAPPPO = row["DT_APP_PO"].ToString();
                    if (!string.IsNullOrEmpty(strRRBDTAPPPO))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRRBDTAPPPO, out date))
                            dtRRBDTAPPPO = date;
                    }

                    string strRRBDTAPPEO = row["DT_APP_EO"].ToString();
                    if (!string.IsNullOrEmpty(strRRBDTAPPEO))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRRBDTAPPEO, out date))
                            dtRRBDTAPPEO = date;
                    }

                    string strRRBDTORDDA = row["DT_ORD_DA"].ToString();
                    if (!string.IsNullOrEmpty(strRRBDTORDDA))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRRBDTORDDA, out date))
                            dtRRBDTORDDA = date;
                    }

                    string strRRBDATEOFCLOSURE = row["DATEOFCLOSURE"].ToString();
                    if (!string.IsNullOrEmpty(strRRBDATEOFCLOSURE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRRBDATEOFCLOSURE, out date))
                            dtRRBDATEOFCLOSURE = date;
                    }

                    string strRRBDTISTDA = row["DT_IST_DA"].ToString();
                    if (!string.IsNullOrEmpty(strRRBDTISTDA))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRRBDTISTDA, out date))
                            dtRRBDTISTDA = date;
                    }

                    string strRRBDTCVOADVICE = row["DT_CVO_ADVICE"].ToString();
                    if (!string.IsNullOrEmpty(strRRBDTCVOADVICE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRRBDTCVOADVICE, out date))
                            dtRRBDTCVOADVICE = date;
                    }

                    string strRRBDT2NDDA = row["DT_2ND_DA"].ToString();
                    if (!string.IsNullOrEmpty(strRRBDT2NDDA))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRRBDT2NDDA, out date))
                            dtRRBDT2NDDA = date;
                    }

                    string strRRBDTCVOADVICE2 = row["DT_CVO_ADVICE_2"].ToString();
                    if (!string.IsNullOrEmpty(strRRBDTCVOADVICE2))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRRBDTCVOADVICE2, out date))
                            dtRRBDTCVOADVICE2 = date;
                    }
                    #endregion
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spRRBExcel_Import]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_RNO", strRRBRNO);
                    cmd.Parameters.AddWithValue("@p_RNO1", strRRBRNO1);
                    cmd.Parameters.AddWithValue("@p_NAMEOFPARTICULARS", strRRBNAMEOFPARTICULARS);
                    cmd.Parameters.AddWithValue("@p_DTCHARGE", dtRRBDTCHARGE);
                    cmd.Parameters.AddWithValue("@p_DTRNO", dtRRBDTRNO);
                    cmd.Parameters.AddWithValue("@p_NAME", strRRBNAME);
                    cmd.Parameters.AddWithValue("@p_SCALE", strRRBSCALE);
                    cmd.Parameters.AddWithValue("@p_DTOFRETIREMENT", dtRRBDTOFRETIREMENT);
                    cmd.Parameters.AddWithValue("@p_BRNAME", strRRBBRNAME);
                    cmd.Parameters.AddWithValue("@p_CIRCLEOFFICE", strRRBCIRCLEOFFICE);
                    cmd.Parameters.AddWithValue("@p_LAPSENATURE", strRRBLAPSENATURE);
                    cmd.Parameters.AddWithValue("@p_CBI_RC_NO1", strRRBCBI_RC_NO1);
                    cmd.Parameters.AddWithValue("@p_DT_RC1", dtRRBDTRC1);
                    cmd.Parameters.AddWithValue("@p_CBI_RC_NO2", strRRBCBI_RC_NO2);
                    cmd.Parameters.AddWithValue("@p_DT_RC2", dtRRBDTRC2);
                    cmd.Parameters.AddWithValue("@p_NAT_CHSHEET", strRRBNAT_CHSHEET);
                    cmd.Parameters.AddWithValue("@p_DT_APP_PO", dtRRBDTAPPPO);
                    cmd.Parameters.AddWithValue("@p_DT_APP_EO", dtRRBDTAPPEO);
                    cmd.Parameters.AddWithValue("@p_NAME_PO", strRRBNAME_PO);
                    cmd.Parameters.AddWithValue("@p_NAME_EO", strRRBNAME_EO);
                    cmd.Parameters.AddWithValue("@p_DT_ORD_DA", dtRRBDTORDDA);
                    cmd.Parameters.AddWithValue("@p_NA_PUN_DA", strRRBNA_PUN_DA);
                    cmd.Parameters.AddWithValue("@p_FINAL", strRRBFINAL);
                    cmd.Parameters.AddWithValue("@p_DISP_AUTHORITY", strRRBDISP_AUTHORITY);
                    cmd.Parameters.AddWithValue("@p_DISAUTHORITYSZONE", strRRBDISAUTHORITYSZONE);
                    cmd.Parameters.AddWithValue("@p_STATUS", strRRBSTATUS);
                    cmd.Parameters.AddWithValue("@p_STATUSCODE", strRRBSTATUSCODE);
                    cmd.Parameters.AddWithValue("@p_DATEOFCLOSURE", dtRRBDATEOFCLOSURE);
                    cmd.Parameters.AddWithValue("@p_REGISTER", strRRBREGISTER);
                    cmd.Parameters.AddWithValue("@p_PFNUMBER", strRRBPFNUMBER);
                    cmd.Parameters.AddWithValue("@p_DT_IST_DA", dtRRBDTISTDA);
                    cmd.Parameters.AddWithValue("@p_DAPROPOSAL", strRRBDAPROPOSAL);
                    cmd.Parameters.AddWithValue("@p_DT_CVO_ADVICE", dtRRBDTCVOADVICE);
                    cmd.Parameters.AddWithValue("@p_ADVICECVOI", strRRBADVICECVOI);
                    cmd.Parameters.AddWithValue("@p_DT_2ND_DA", dtRRBDT2NDDA);
                    cmd.Parameters.AddWithValue("@p_DAPROPOSAL_2", strRRBDAPROPOSAL_2);
                    cmd.Parameters.AddWithValue("@p_DT_CVO_ADVICE_2", dtRRBDTCVOADVICE2);
                    cmd.Parameters.AddWithValue("@p_ADVICECVO2", strRRBADVICECVO2);
                    cmd.Parameters.AddWithValue("@p_DESK_USER_REMARKS", strRRBDESK_USER_REMARKS);
                    cmd.Parameters.AddWithValue("@p_BANKNAME", strRRBBANKNAME);

                    cmd.Parameters.AddWithValue("@p_ADDUSER", Session["userid"].ToString());
                    cmd.Parameters.AddWithValue("@p_ADDUSERIP", objCommonFunction.funcGetUserIP());

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e12)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e12);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void funcExcelImport_RTI()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["RTIEXCELDETAILS"]);

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strRTINO = row["RTINO"].ToString();
                }
                catch (Exception)
                {
                    lblMsg.Text = "Upload Failed ! Please check your EXECL";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strRTINO != "")
                {
                    #region ** assian Variable Value **
                    strRTIROWNO = Convert.ToString(row["ROWNO"]);
                    strRTINO = Convert.ToString(row["RTINO"]);
                    strRTIACCUSED = Convert.ToString(row["ACCUSED"]);
                    strRTIDESIGNATION = Convert.ToString(row["DESIGNATION"]);
                    strRTIPRESENTPOSTING = Convert.ToString(row["PRESENTPOSTING"]);
                    strBRCOMPLAINT = Convert.ToString(row["BRCOMPLAINT"]);
                    strRTIZONE = Convert.ToString(row["ZONE"]);
                    strRTICIRCLEOFFICE = Convert.ToString(row["CIRCLEOFFICE"]);
                    strRTISOURCE = Convert.ToString(row["SOURCE"]);
                    strSOURCEREF = Convert.ToString(row["SOURCEREF"]);
                    strSENTTO = Convert.ToString(row["SENTTO"]);
                    strCATANO = objCommonFunction.convertToInt(row["CATANO"].ToString());
                    strCATBNO = objCommonFunction.convertToInt(row["CATBNO"].ToString());
                    strASNO = objCommonFunction.convertToInt(row["ASNO"].ToString());
                    strNATURECOMP = Convert.ToString(row["NATURECOMP"]);
                    strRTIACCOUNTNAME = Convert.ToString(row["ACCOUNTNAME"]);
                    strALLEGATIONS = Convert.ToString(row["ALLEGATIONS"]);
                    strRTISTATUS = Convert.ToString(row["STATUS"]);
                    strPENDINGWITH = Convert.ToString(row["PENDINGWITH"]);
                    strNAMEINVOFFICIAL = Convert.ToString(row["NAMEINVOFFICIAL"]);
                    strDAYSTAKEN = objCommonFunction.convertToInt(row["DAYSTAKEN"].ToString());
                    strFINALACTION = Convert.ToString(row["FINALACTION"]);
                    strCASECLOSE = Convert.ToString(row["CASECLOSE"]);
                    strRTIRNO = Convert.ToString(row["RNO"]);
                    strAPLAN = Convert.ToString(row["APLAN"]);
                    strRTISTATUSCODE = Convert.ToString(row["STATUSCODE"]);
                    strRTIREGISTER = Convert.ToString(row["REGISTER"]);
                    strNATURE = Convert.ToString(row["NATURE"]);
                    strREASONSFORCLOSURE = Convert.ToString(row["REASONSFORCLOSURE"]);
                    strRTIBANKNAME = Convert.ToString(row["BANKNAME"]);

                    TextBox txtAmount = new TextBox();
                    txtAmount.Text = row["AMOUNT"].ToString();
                    decRTIAMOUNT = objCommonFunction.convertToDecimal(txtAmount);
                    #endregion

                    #region ** convert Date **
                    string strRECDATERTI = row["RECDATERTI"].ToString();
                    if (!string.IsNullOrEmpty(strRECDATERTI))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRECDATERTI, out date))
                            dtRECDATERTI = date;
                    }

                    string strSOURCEDATE = row["SOURCEDATE"].ToString();
                    if (!string.IsNullOrEmpty(strSOURCEDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSOURCEDATE, out date))
                            dtSOURCEDATE = date;
                    }

                    string strSENTFORINVDATE = row["SENTFORINVDATE"].ToString();
                    if (!string.IsNullOrEmpty(strSENTFORINVDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSENTFORINVDATE, out date))
                            dtSENTFORINVDATE = date;
                    }

                    string strDTIAC = row["DTIAC"].ToString();
                    if (!string.IsNullOrEmpty(strDTIAC))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strDTIAC, out date))
                            dtRTIDTIAC = date;
                    }

                    string strDTOFINVREPORT = row["DTOFINVREPORT"].ToString();
                    if (!string.IsNullOrEmpty(strDTOFINVREPORT))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strDTOFINVREPORT, out date))
                            dtDTOFINVREPORT = date;
                    }

                    string strCLOSUREDT = row["CLOSUREDT"].ToString();
                    if (!string.IsNullOrEmpty(strCLOSUREDT))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strCLOSUREDT, out date))
                            dtRTICLOSUREDT = date;
                    }

                    string strRYSENT = row["RYSENT"].ToString();
                    if (!string.IsNullOrEmpty(strRYSENT))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRYSENT, out date))
                            dtRYSENT = date;
                    }
                    #endregion
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spRTIExcel_Import]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_RTINO", strRTINO);
                    cmd.Parameters.AddWithValue("@p_ACCUSED", strRTIACCUSED);
                    cmd.Parameters.AddWithValue("@p_DESIGNATION", strRTIDESIGNATION);
                    cmd.Parameters.AddWithValue("@p_PRESENTPOSTING", strRTIPRESENTPOSTING);
                    cmd.Parameters.AddWithValue("@p_BRCOMPLAINT", strBRCOMPLAINT);
                    cmd.Parameters.AddWithValue("@p_ZONE", strRTIZONE);
                    cmd.Parameters.AddWithValue("@p_CIRCLEOFFICE", strRTICIRCLEOFFICE);
                    cmd.Parameters.AddWithValue("@p_RECDATERTI", dtRECDATERTI);
                    cmd.Parameters.AddWithValue("@p_SOURCE", strRTISOURCE);
                    cmd.Parameters.AddWithValue("@p_SOURCEREF", strSOURCEREF);
                    cmd.Parameters.AddWithValue("@p_SOURCEDATE", dtSOURCEDATE);
                    cmd.Parameters.AddWithValue("@p_SENTTO", strSENTTO);
                    cmd.Parameters.AddWithValue("@p_SENTFORINVDATE", dtSENTFORINVDATE);
                    cmd.Parameters.AddWithValue("@p_CATANO", strCATANO);
                    cmd.Parameters.AddWithValue("@p_CATBNO", strCATBNO);
                    cmd.Parameters.AddWithValue("@p_ASNO", strASNO);
                    cmd.Parameters.AddWithValue("@p_NATURECOMP", strNATURECOMP);
                    cmd.Parameters.AddWithValue("@p_ACCOUNTNAME", strRTIACCOUNTNAME);
                    cmd.Parameters.AddWithValue("@p_AMOUNT", decRTIAMOUNT);
                    cmd.Parameters.AddWithValue("@p_ALLEGATIONS", strALLEGATIONS);
                    cmd.Parameters.AddWithValue("@p_DTIAC", dtRTIDTIAC);
                    cmd.Parameters.AddWithValue("@p_STATUS", strRTISTATUS);
                    cmd.Parameters.AddWithValue("@p_PENDINGWITH", strPENDINGWITH);
                    cmd.Parameters.AddWithValue("@p_NAMEINVOFFICIAL", strNAMEINVOFFICIAL);
                    cmd.Parameters.AddWithValue("@p_DTOFINVREPORT", dtDTOFINVREPORT);
                    cmd.Parameters.AddWithValue("@p_DAYSTAKEN", strDAYSTAKEN);
                    cmd.Parameters.AddWithValue("@p_FINALACTION", strFINALACTION);
                    cmd.Parameters.AddWithValue("@p_CASECLOSE", strCASECLOSE);
                    cmd.Parameters.AddWithValue("@p_CLOSUREDT", strCLOSUREDT);
                    cmd.Parameters.AddWithValue("@p_RNO", strRTIRNO);
                    cmd.Parameters.AddWithValue("@p_RYSENT", dtRYSENT);
                    cmd.Parameters.AddWithValue("@p_APLAN", strAPLAN);
                    cmd.Parameters.AddWithValue("@p_STATUSCODE", strRTISTATUSCODE);
                    cmd.Parameters.AddWithValue("@p_REGISTER", strRTIREGISTER);
                    cmd.Parameters.AddWithValue("@p_NATURE", strNATURE);
                    cmd.Parameters.AddWithValue("@p_REASONSFORCLOSURE", strREASONSFORCLOSURE);
                    cmd.Parameters.AddWithValue("@p_BANKNAME", strRTIBANKNAME);

                    cmd.Parameters.AddWithValue("@p_ADDUSER", Session["userid"].ToString());
                    cmd.Parameters.AddWithValue("@p_ADDUSERIP", objCommonFunction.funcGetUserIP());

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e12)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e12);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void funcExcelImport_SR()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["SREXCELDETAILS"]);

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strSRNO = row["SRNO"].ToString();
                }
                catch (Exception)
                {
                    lblMsg.Text = "Upload Failed ! Please check your EXECL";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strSRNO != "")
                {
                    #region ** assian Variable Value **
                    strSRROWNO = Convert.ToString(row["ROWNO"]);
                    strSRNO = Convert.ToString(row["SRNO"]);
                    strSRACCUSED = Convert.ToString(row["ACCUSED"]);
                    strSRDESIGNATION = Convert.ToString(row["DESIGNATION"]);
                    strSRPRESENTPOSTING = Convert.ToString(row["PRESENTPOSTING"]);
                    strSRBRANCH = Convert.ToString(row["BRANCH"]);
                    strSRZONE = Convert.ToString(row["ZONE"]);
                    strSRCIRCLEOFFICE = Convert.ToString(row["CIRCLEOFFICE"]);
                    strREGION = Convert.ToString(row["REGION"]);
                    strINVESTIGATION = Convert.ToString(row["INVESTIGATION"]);
                    strNATURESR = Convert.ToString(row["NATURESR"]);
                    strAMOUNT = Convert.ToString(row["AMOUNT"]);
                    strSRALLEGATIONS = Convert.ToString(row["ALLEGATIONS"]);
                    strREMINDERS = Convert.ToString(row["REMINDERS"]);
                    strSRSTATUS = Convert.ToString(row["STATUS"]);
                    strSRPENDINGWITH = Convert.ToString(row["PENDINGWITH"]);
                    strACCOUNT = Convert.ToString(row["ACCOUNT"]);
                    strZMVIEW = Convert.ToString(row["ZMVIEW"]);
                    strICVIEW = Convert.ToString(row["ICVIEW"]);
                    strSRFINALACTION = Convert.ToString(row["FINALACTION"]);
                    strSRCASECLOSE = Convert.ToString(row["CASECLOSE"]);
                    strSRRNO = Convert.ToString(row["RNO"]);
                    strSRICDT = Convert.ToString(row["ICDT"]);
                    strSRAPLAN = Convert.ToString(row["APLAN"]);
                    strSRSTATUSCODE = Convert.ToString(row["STATUSCODE"]);
                    strSRBANKNAME = Convert.ToString(row["BANKNAME"]);

                    TextBox txtAmount = new TextBox();
                    txtAmount.Text = row["AMOUNT"].ToString();
                    decSRAMOUNT = objCommonFunction.convertToDecimal(txtAmount);

                    #endregion

                    #region ** convert Date **
                    string strRECDTSR = row["RECDTSR"].ToString();
                    if (!string.IsNullOrEmpty(strRECDTSR))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRECDTSR, out date))
                            dtDTRECDTSR = date;
                    }

                    string strDATESR = row["DATESR"].ToString();
                    if (!string.IsNullOrEmpty(strDATESR))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strDATESR, out date))
                            dtDATESR = date;
                    }

                    string strDTIAC = row["DTIAC"].ToString();
                    if (!string.IsNullOrEmpty(strDTIAC))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strDTIAC, out date))
                            dtSRDTIAC = date;
                    }

                    string strCLOSUREDT = row["CLOSUREDT"].ToString();
                    if (!string.IsNullOrEmpty(strCLOSUREDT))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strCLOSUREDT, out date))
                            dtSRCLOSUREDT = date;
                    }

                    string strICDT = row["ICDT"].ToString();
                    if (!string.IsNullOrEmpty(strICDT))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strICDT, out date))
                            dtICDT = date;
                    }

                    string strCMD = row["CMD"].ToString();
                    if (!string.IsNullOrEmpty(strCMD))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strCMD, out date))
                            dtCMD = date;
                    }
                    #endregion
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spSRExcel_Import]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SRNO", strSRNO);
                    cmd.Parameters.AddWithValue("@p_ACCUSED", strSRACCUSED);
                    cmd.Parameters.AddWithValue("@p_DESIGNATION", strSRDESIGNATION);
                    cmd.Parameters.AddWithValue("@p_PRESENTPOSTING", strSRPRESENTPOSTING);
                    cmd.Parameters.AddWithValue("@p_BRANCH", strSRBRANCH);
                    cmd.Parameters.AddWithValue("@p_ZONE", strSRZONE);
                    cmd.Parameters.AddWithValue("@p_CIRCLEOFFICE", strSRCIRCLEOFFICE);
                    cmd.Parameters.AddWithValue("@p_REGION", strREGION);
                    cmd.Parameters.AddWithValue("@p_RECDTSR", dtDTRECDTSR);
                    cmd.Parameters.AddWithValue("@p_DATESR", dtDATESR);
                    cmd.Parameters.AddWithValue("@p_INVESTIGATION", strINVESTIGATION);
                    cmd.Parameters.AddWithValue("@p_DTIAC", dtSRDTIAC);
                    cmd.Parameters.AddWithValue("@p_NATURESR", strNATURESR);
                    cmd.Parameters.AddWithValue("@p_AMOUNT", strAMOUNT);
                    cmd.Parameters.AddWithValue("@p_ALLEGATIONS", strSRALLEGATIONS);
                    cmd.Parameters.AddWithValue("@p_REMINDERS", strREMINDERS);
                    cmd.Parameters.AddWithValue("@p_STATUS", strSRSTATUS);
                    cmd.Parameters.AddWithValue("@p_PENDINGWITH", strSRPENDINGWITH);
                    cmd.Parameters.AddWithValue("@p_ACCOUNT", strACCOUNT);
                    cmd.Parameters.AddWithValue("@p_ZMVIEW", strZMVIEW);
                    cmd.Parameters.AddWithValue("@p_ICVIEW", strICVIEW);
                    cmd.Parameters.AddWithValue("@p_FINALACTION", strSRFINALACTION);
                    cmd.Parameters.AddWithValue("@p_CASECLOSE", strSRCASECLOSE);
                    cmd.Parameters.AddWithValue("@p_CLOSUREDT", dtSRCLOSUREDT);
                    cmd.Parameters.AddWithValue("@p_RNO", strSRRNO);
                    cmd.Parameters.AddWithValue("@p_ICDT", strSRICDT);
                    cmd.Parameters.AddWithValue("@p_APLAN", strSRAPLAN);
                    cmd.Parameters.AddWithValue("@p_CMD", dtCMD);
                    cmd.Parameters.AddWithValue("@p_STATUSCODE", strSRSTATUSCODE);
                    cmd.Parameters.AddWithValue("@p_BANKNAME", strSRBANKNAME);

                    cmd.Parameters.AddWithValue("@p_ADDUSER", Session["userid"].ToString());
                    cmd.Parameters.AddWithValue("@p_ADDUSERIP", objCommonFunction.funcGetUserIP());

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e12)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e12);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void funcExcelImport_SanctionForInvestigation()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["SFIEXCELDETAILS"]);

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strSFISINO = Convert.ToString(row["SFI_SINO"]);

                    string ID = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                    strSFIUNIQUEID = "SFI" + DateTime.Now.ToString("ddMMyy") + ID;

                    strSFIRCNO = Convert.ToString(row["SFI_RCNO"]);
                    strSFIPFNO = Convert.ToString(row["SFI_PFNO"]);
                    strSFINAME = Convert.ToString(row["SFI_NAME"]);
                    strSFIDESIGNATION = Convert.ToString(row["SFI_DESIGNATION"]);
                    strSFICIRCLE = Convert.ToString(row["SFI_CIRCLE"]);
                    strSFIBRANCH = Convert.ToString(row["SFI_BRANCH"]);
                    strSFIDA = Convert.ToString(row["SFI_DA"]);
                    strSFIDAVIEW = Convert.ToString(row["SFI_DA_VIEW"]);
                    strSFILETTERTOCBISENTBY = Convert.ToString(row["SFI_LETTER_TO_CBI_SENTBY"]);
                    strSFISTATUS = Convert.ToString(row["SFI_STATUS"]);
                    strSFIREMARKS = Convert.ToString(row["SFI_REMARKS"]);
                    strSFIBANKNAME = Convert.ToString(row["BANKNAME"]);
                }
                catch (Exception)
                {
                    lblMsg.Text = "Upload Failed ! Please check your EXECL";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strSFISINO != "")
                {
                    #region ** assian Variable Value **
                    strSFIROWNO = Convert.ToString(row["ROWNO"]);

                    #endregion

                    #region ** convert Date **
                    string strSFIRCDATE = row["SFI_RCDATE"].ToString();
                    if (!string.IsNullOrEmpty(strSFIRCDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSFIRCDATE, out date))
                            dtSFIRCDATE = date;
                    }

                    string strSFIREPORTRECVDATE = row["SFI_REPORT_RECV_DATE"].ToString();
                    if (!string.IsNullOrEmpty(strSFIREPORTRECVDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSFIREPORTRECVDATE, out date))
                            dtSFIREPORTRECVDATE = date;
                    }

                    string strSFILETTERTOCBIDATE = row["SFI_LETTER_TO_CBI_DATE"].ToString();
                    if (!string.IsNullOrEmpty(strSFILETTERTOCBIDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSFILETTERTOCBIDATE, out date))
                            dtSFILETTERTOCBIDATE = date;
                    }

                    #endregion
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spSFIExcel_Import]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SFI_UNIQUEID", strSFIUNIQUEID);
                    cmd.Parameters.AddWithValue("@p_SFI_SINO", strSFISINO);
                    cmd.Parameters.AddWithValue("@p_SFI_RCNO", strSFIRCNO);
                    cmd.Parameters.AddWithValue("@p_SFI_RCDATE", dtSFIRCDATE);
                    cmd.Parameters.AddWithValue("@p_SFI_REPORT_RECV_DATE", dtSFIREPORTRECVDATE);
                    cmd.Parameters.AddWithValue("@p_SFI_PFNO", strSFIPFNO);
                    cmd.Parameters.AddWithValue("@p_SFI_NAME", strSFINAME);
                    cmd.Parameters.AddWithValue("@p_SFI_DESIGNATION", strSFIDESIGNATION);
                    cmd.Parameters.AddWithValue("@p_SFI_CIRCLE", strSFICIRCLE);
                    cmd.Parameters.AddWithValue("@p_SFI_BRANCH", strSFIBRANCH);
                    cmd.Parameters.AddWithValue("@p_SFI_DA", strSFIDA);
                    cmd.Parameters.AddWithValue("@p_SFI_DA_VIEW", strSFIDAVIEW);
                    cmd.Parameters.AddWithValue("@p_SFI_LETTER_TO_CBI_DATE", dtSFILETTERTOCBIDATE);
                    cmd.Parameters.AddWithValue("@p_SFI_LETTER_TO_CBI_SENTBY", strSFILETTERTOCBISENTBY);
                    cmd.Parameters.AddWithValue("@p_SFI_STATUS", strSFISTATUS);
                    cmd.Parameters.AddWithValue("@p_SFI_REMARKS", strSFIREMARKS);
                    cmd.Parameters.AddWithValue("@p_BANKNAME", strSFIBANKNAME);

                    cmd.Parameters.AddWithValue("@p_ADDUSER", Session["userid"].ToString());
                    cmd.Parameters.AddWithValue("@p_SFI_ADDUSER_ROLE", Session["role"].ToString());
                    cmd.Parameters.AddWithValue("@p_ADDUSERIP", objCommonFunction.funcGetUserIP());

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e12)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e12);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void funcExcelImport_SanctionForProsecution()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["SFPEXCELDETAILS"]);

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strSFPRNO = row["SFP_SPNO"].ToString();
                }
                catch (Exception)
                {
                    lblMsg.Text = "Upload Failed ! Please check your EXECL";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strSFPRNO != "")
                {
                    #region ** assian Variable Value **
                    strSFPROWNO = Convert.ToString(row["ROWNO"]);

                    string ID = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                    strSFPUNIQUEID = "SFP" + DateTime.Now.ToString("ddMMyy") + ID;

                    strSFPSPNO = Convert.ToString(row["SFP_SPNO"]);
                    strSFPRCNO = Convert.ToString(row["SFP_RCNO"]);
                    strSFPPFNO = Convert.ToString(row["SFP_PFNO"]);
                    strSFPNAME = Convert.ToString(row["SFP_NAME"]);
                    strSFPDESIGNATION = Convert.ToString(row["SFP_DESIGNATION"]);
                    strSFPCIRCLE = Convert.ToString(row["SFP_CIRCLE"]);
                    strSFPBRANCH = Convert.ToString(row["SFP_BRANCH"]);
                    strSFPDA = Convert.ToString(row["SFP_DA"]);
                    strSFPDAVIEW = Convert.ToString(row["SFP_DA_VIEW"]);
                    strSFPCVCVIEW = Convert.ToString(row["SFP_CVC_VIEW"]);
                    strSFPSTATUS = Convert.ToString(row["SFP_STATUS"]);
                    strSFPREMARKS = Convert.ToString(row["SFP_REMARKS"]);
                    strSFPBANKNAME = Convert.ToString(row["BANKNAME"]);

                    #endregion

                    #region ** convert Date **
                    string strSFPRCDATE = row["SFP_RCDATE"].ToString();
                    if (!string.IsNullOrEmpty(strSFPRCDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSFPRCDATE, out date))
                            dtSFPRCDATE = date;
                    }

                    string strSFPREPORTRECVDATE = row["SFP_REPORT_RECV_DATE"].ToString();
                    if (!string.IsNullOrEmpty(strSFPREPORTRECVDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSFPREPORTRECVDATE, out date))
                            dtSFPREPORTRECVDATE = date;
                    }

                    string strSFPLETTERTOCBIDATE = row["SFP_LETTER_TO_CBI_DATE"].ToString();
                    if (!string.IsNullOrEmpty(strSFPLETTERTOCBIDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSFPLETTERTOCBIDATE, out date))
                            dtSFPLETTERTOCBIDATE = date;
                    }

                    string strSFPLETTERTOCVCDATE = row["SFP_LETTER_TO_CVC_DATE"].ToString();
                    if (!string.IsNullOrEmpty(strSFPLETTERTOCVCDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSFPLETTERTOCVCDATE, out date))
                            dtSFPLETTERTOCVCDATE = date;
                    }

                    string strSFPLETTERTODADATE = row["SFP_LETTER_TO_DA_DATE"].ToString();
                    if (!string.IsNullOrEmpty(strSFPLETTERTODADATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSFPLETTERTODADATE, out date))
                            dtSFPLETTERTODADATE = date;
                    }

                    string strSFPDAORDERTOCBIDATE = row["SFP_DAORDER_TOCBI_DATE"].ToString();
                    if (!string.IsNullOrEmpty(strSFPDAORDERTOCBIDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSFPDAORDERTOCBIDATE, out date))
                            dtSFPDAORDERTOCBIDATE = date;
                    }
                    #endregion
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spSFPCExcel_Import]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SFP_UNIQUEID", strSFPUNIQUEID);
                    cmd.Parameters.AddWithValue("@p_SFP_SPNO", strSFPSPNO);
                    cmd.Parameters.AddWithValue("@p_SFP_RCNO", strSFPRCNO);
                    cmd.Parameters.AddWithValue("@p_SFP_RCDATE", dtSFPRCDATE);
                    cmd.Parameters.AddWithValue("@p_SFP_REPORT_RECV_DATE", dtSFPREPORTRECVDATE);
                    cmd.Parameters.AddWithValue("@p_SFP_PFNO", strSFPPFNO);
                    cmd.Parameters.AddWithValue("@p_SFP_NAME", strSFPNAME);
                    cmd.Parameters.AddWithValue("@p_SFP_DESIGNATION", strSFPDESIGNATION);
                    cmd.Parameters.AddWithValue("@p_SFP_CIRCLE", strSFPCIRCLE);
                    cmd.Parameters.AddWithValue("@p_SFP_BRANCH", strSFPBRANCH);
                    cmd.Parameters.AddWithValue("@p_SFP_DA", strSFPDA);
                    cmd.Parameters.AddWithValue("@p_SFP_DA_VIEW", strSFPDAVIEW);
                    cmd.Parameters.AddWithValue("@p_SFP_LETTER_TO_CBI_DATE", dtSFPLETTERTOCBIDATE);
                    cmd.Parameters.AddWithValue("@p_SFP_LETTER_TO_CVC_DATE", dtSFPLETTERTOCVCDATE);
                    cmd.Parameters.AddWithValue("@p_SFP_CVC_VIEW", strSFPCVCVIEW);
                    cmd.Parameters.AddWithValue("@p_SFP_LETTER_TO_DA_DATE", dtSFPLETTERTODADATE);
                    cmd.Parameters.AddWithValue("@p_SFP_DAORDER_TOCBI_DATE", dtSFPDAORDERTOCBIDATE);
                    cmd.Parameters.AddWithValue("@p_SFP_STATUS", strSFPSTATUS);
                    cmd.Parameters.AddWithValue("@p_SFP_REMARKS", strSFPREMARKS);
                    cmd.Parameters.AddWithValue("@p_BANKNAME", strSFPBANKNAME);

                    cmd.Parameters.AddWithValue("@p_ADDUSER", Session["userid"].ToString());
                    cmd.Parameters.AddWithValue("@p_SFP_ADDUSER_ROLE", Session["role"].ToString());
                    cmd.Parameters.AddWithValue("@p_ADDUSERIP", objCommonFunction.funcGetUserIP());

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e12)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e12);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void funcExcelImport_VigilanceMIS()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["VIGMEXCELDETAILS"]);

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strVIGMRNO = row["VIGM_RNO"].ToString();
                }
                catch (Exception)
                {
                    lblMsg.Text = "Upload Failed ! Please check your EXECL";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strVIGMRNO != "")
                {
                    #region ** assian Variable Value **
                    strVIGMROWNO = Convert.ToString(row["ROWNO"]);

                    string ID = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                    strVIGMUNIQUEID = "VIGM" + DateTime.Now.ToString("ddMMyy") + ID;

                    strVIGMNAME = Convert.ToString(row["VIGM_NAME"]);
                    strVIGMPFNUMBER = Convert.ToString(row["VIGM_PFNUMBER"]);
                    strVIGMBRANCH = Convert.ToString(row["VIGM_BRANCH"]);
                    strVIGMVIGCASE = Convert.ToString(row["VIGM_VIGCASE"]);
                    strVIGMDAREFNO = Convert.ToString(row["VIGM_DAREFNO"]);
                    strVIGMSCALE = Convert.ToString(row["VIGM_SCALE"]);
                    strVIGMDESIGNATION = Convert.ToString(row["VIGM_DESIGNATION"]);
                    strVIGMUS = Convert.ToString(row["VIGM_US"]);
                    strVIGMLAPSENATURE = Convert.ToString(row["VIGM_LAPSENATURE"]);
                    strVIGMSOURCE = Convert.ToString(row["VIGM_SOURCE"]);
                    strVIGMACCOUNTNAME = Convert.ToString(row["VIGM_ACCOUNTNAME"]);
                    strVIGMCBIRCNO1 = Convert.ToString(row["VIGM_CBIRCNO1"]);
                    strVIGMSTATUS = Convert.ToString(row["VIGM_STATUS"]);
                    strVIGMBANKNAME = Convert.ToString(row["BANK_NAME"]);
                    strEXTERNALSOURCE = Convert.ToString(row["EXTERNAL_SOURCE"]);

                    strVIGMSTATUSCODE = objCommonFunction.convertToInt(row["VIGM_STATUSCODE"].ToString());
                    strVIGMSTATE = objCommonFunction.convertToInt(Convert.ToString(row["VIGM_STATE"]));
                    strVIGMCIRCLEOFFICE = objCommonFunction.convertToInt(Convert.ToString(row["VIGM_CIRCLEOFFICE"]));

                    TextBox txtAmount = new TextBox();
                    txtAmount.Text = row["VIGM_AMOUNT"].ToString();
                    decVIGMAMOUNT = objCommonFunction.convertToDecimal(txtAmount);
                    #endregion

                    #region ** convert Date **
                    string strVIGMRNODATE = row["VIGM_RNODATE"].ToString();
                    if (!string.IsNullOrEmpty(strVIGMRNODATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strVIGMRNODATE, out date))
                            dtVIGMRNODATE = date;
                    }

                    string strVIGMRETIREMENTDATE = row["VIGM_RETIREMENTDATE"].ToString();
                    if (!string.IsNullOrEmpty(strVIGMRETIREMENTDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strVIGMRETIREMENTDATE, out date))
                            dtVIGMRETIREMENTDATE = date;
                    }

                    string strVIGMSUSPENSIONDATE = row["VIGM_SUSPENSIONDATE"].ToString();
                    if (!string.IsNullOrEmpty(strVIGMSUSPENSIONDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strVIGMSUSPENSIONDATE, out date))
                            dtVIGMSUSPENSIONDATE = date;
                    }

                    string strVIGMREVOCATIONDATE = row["VIGM_REVOCATIONDATE"].ToString();
                    if (!string.IsNullOrEmpty(strVIGMREVOCATIONDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strVIGMREVOCATIONDATE, out date))
                            dtVIGMREVOCATIONDATE = date;
                    }

                    string strEXTERNALSOURCEDATE = row["EXTERNAL_SOURCE_DATE"].ToString();
                    if (!string.IsNullOrEmpty(strEXTERNALSOURCEDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strEXTERNALSOURCEDATE, out date))
                            dtEXTERNALSOURCEDATE = date;
                    }

                    string strVIGMCLOSUREDATE = row["VIGM_CLOSURE_DATE"].ToString();
                    if (!string.IsNullOrEmpty(strVIGMCLOSUREDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strVIGMCLOSUREDATE, out date))
                            dtVIGMCLOSUREDATE = date;
                    }
                    #endregion
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spVIGMExcel_Import]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_VIGM_RNO", strVIGMRNO);
                    cmd.Parameters.AddWithValue("@p_VIGM_RNODATE", dtVIGMRNODATE);
                    cmd.Parameters.AddWithValue("@p_VIGM_NAME", strVIGMNAME);
                    cmd.Parameters.AddWithValue("@p_VIGM_PFNUMBER", strVIGMPFNUMBER);
                    cmd.Parameters.AddWithValue("@p_VIGM_STATUSCODE", strVIGMSTATUSCODE);
                    cmd.Parameters.AddWithValue("@p_VIGM_RETIREMENTDATE", dtVIGMRETIREMENTDATE);
                    cmd.Parameters.AddWithValue("@p_VIGM_STATE", strVIGMSTATE);
                    cmd.Parameters.AddWithValue("@p_VIGM_CIRCLEOFFICE", strVIGMCIRCLEOFFICE);
                    cmd.Parameters.AddWithValue("@p_VIGM_BRANCH", strVIGMBRANCH);
                    cmd.Parameters.AddWithValue("@p_VIGM_VIGCASE", strVIGMVIGCASE);
                    cmd.Parameters.AddWithValue("@p_VIGM_DAREFNO", strVIGMDAREFNO);
                    cmd.Parameters.AddWithValue("@p_VIGM_SCALE", strVIGMSCALE);
                    cmd.Parameters.AddWithValue("@p_VIGM_DESIGNATION", strVIGMDESIGNATION);
                    cmd.Parameters.AddWithValue("@p_VIGM_US", strVIGMUS);
                    cmd.Parameters.AddWithValue("@p_VIGM_SUSPENSIONDATE", dtVIGMSUSPENSIONDATE);
                    cmd.Parameters.AddWithValue("@p_VIGM_REVOCATIONDATE", dtVIGMREVOCATIONDATE);
                    cmd.Parameters.AddWithValue("@p_VIGM_LAPSENATURE", strVIGMLAPSENATURE);
                    cmd.Parameters.AddWithValue("@p_VIGM_AMOUNT", decVIGMAMOUNT);
                    cmd.Parameters.AddWithValue("@p_VIGM_SOURCE", strVIGMSOURCE);
                    cmd.Parameters.AddWithValue("@p_VIGM_ACCOUNTNAME", strVIGMACCOUNTNAME);
                    cmd.Parameters.AddWithValue("@p_VIGM_CBIRCNO1", strVIGMCBIRCNO1);
                    cmd.Parameters.AddWithValue("@p_VIGM_STATUS", strVIGMSTATUS);
                    cmd.Parameters.AddWithValue("@p_BANK_NAME", strVIGMBANKNAME);
                    cmd.Parameters.AddWithValue("@p_EXTERNAL_SOURCE", strEXTERNALSOURCE);
                    cmd.Parameters.AddWithValue("@p_EXTERNAL_SOURCE_DATE", dtEXTERNALSOURCEDATE);
                    cmd.Parameters.AddWithValue("@p_VIGM_CLOSURE_DATE", dtVIGMCLOSUREDATE);

                    cmd.Parameters.AddWithValue("@p_ADDUSER", Session["userid"].ToString());
                    cmd.Parameters.AddWithValue("@p_ADDUSERIP", objCommonFunction.funcGetUserIP());

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e12)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e12);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void funcExcelImport_WB()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["WBEXCELDETAILS"]);

            foreach (DataRow row in dt.Rows)
            {
                DataTable dt1 = new DataTable();
                try
                {
                    strWBRNO = row["RNO"].ToString();
                }
                catch (Exception)
                {
                    lblMsg.Text = "Upload Failed ! Please check your EXECL";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strWBRNO != "")
                {
                    #region ** assian Variable Value **
                    strWBROWNO = Convert.ToString(row["ROWNO"]);
                    strCOMPNO = Convert.ToString(row["COMPNO"]);
                    strWBACCUSED = Convert.ToString(row["ACCUSED"]);
                    strWBDESIGNATION = Convert.ToString(row["DESIGNATION"]);
                    strWBPRESENTPOSTING = Convert.ToString(row["PRESENTPOSTING"]);
                    strWBBRCOMPLAINT = Convert.ToString(row["BRCOMPLAINT"]);
                    strWBZONE = Convert.ToString(row["ZONE"]);
                    strWBCIRCLEOFFICE = Convert.ToString(row["CIRCLEOFFICE"]);
                    strWBREGION = Convert.ToString(row["REGION"]);
                    strWBSOURCE = Convert.ToString(row["SOURCE"]);
                    strWBSOURCEREF = Convert.ToString(row["SOURCEREF"]);
                    strWBSENTTO = Convert.ToString(row["SENTTO"]);
                    strWBCATANO = objCommonFunction.convertToInt(Convert.ToString(row["CATANO"]));
                    strWBCATBNO = objCommonFunction.convertToInt(Convert.ToString(row["CATBNO"]));
                    strWBASNO = objCommonFunction.convertToInt(Convert.ToString(row["ASNO"]));
                    strWBNATURECOMP = Convert.ToString(row["NATURECOMP"]);
                    strWBACCOUNTNAME = Convert.ToString(row["ACCOUNTNAME"]);
                    strWBALLEGATIONS = Convert.ToString(row["ALLEGATIONS"]);
                    strWBSTATUS = Convert.ToString(row["STATUS"]);
                    strWBSTATUSCODE = Convert.ToString(row["STATUSCODE"]);
                    strWBPENDINGWITH = Convert.ToString(row["PENDINGWITH"]);
                    strNAMEOFINVOFFICIAL = Convert.ToString(row["NAMEOFINVOFFICIAL"]);
                    strWBDAYSTAKEN = objCommonFunction.convertToInt(Convert.ToString(row["DAYSTAKEN"]));
                    strCASENO = Convert.ToString(row["CASENO"]);
                    strWBCASECLOSE = Convert.ToString(row["CASECLOSE"]);
                    strCLOSUREDT = Convert.ToString(row["CLOSUREDT"]);
                    strWBAPLAN = Convert.ToString(row["APLAN"]);
                    strWBREGISTER = Convert.ToString(row["REGISTER"]);
                    strWBNATURE = Convert.ToString(row["NATURE"]);
                    strWBREASONSFORCLOSURE = Convert.ToString(row["REASONSFORCLOSURE"]);
                    strWBBANKNAME = Convert.ToString(row["BANKNAME"]);

                    TextBox txtAmount = new TextBox();
                    txtAmount.Text = row["AMOUNT"].ToString();
                    decWBAMOUNT = objCommonFunction.convertToDecimal(txtAmount);
                    #endregion

                    #region ** convert Date **
                    string strRECDATECOMP = row["RECDATECOMP"].ToString();
                    if (!string.IsNullOrEmpty(strRECDATECOMP))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRECDATECOMP, out date))
                            dtRECDATECOMP = date;
                    }

                    string strSOURCEDATE = row["SOURCEDATE"].ToString();
                    if (!string.IsNullOrEmpty(strSOURCEDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSOURCEDATE, out date))
                            dtWBSOURCEDATE = date;
                    }

                    string strSENTFORINVDATE = row["SENTFORINVDATE"].ToString();
                    if (!string.IsNullOrEmpty(strSENTFORINVDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSENTFORINVDATE, out date))
                            dtWBSENTFORINVDATE = date;
                    }

                    string strDTIAC = row["DTIAC"].ToString();
                    if (!string.IsNullOrEmpty(strDTIAC))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strDTIAC, out date))
                            dtWBDTIAC = date;
                    }

                    string strDTOFINVREPORT = row["DTOFINVREPORT"].ToString();
                    if (!string.IsNullOrEmpty(strDTOFINVREPORT))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strDTOFINVREPORT, out date))
                            dtWBDTOFINVREPORT = date;
                    }

                    string strRYSENT = row["RYSENT"].ToString();
                    if (!string.IsNullOrEmpty(strRYSENT))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRYSENT, out date))
                            dtWBRYSENT = date;
                    }
                    #endregion
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spWBExcel_Import]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_RNO", strWBRNO);
                    cmd.Parameters.AddWithValue("@p_COMPNO", strCOMPNO);
                    cmd.Parameters.AddWithValue("@p_ACCUSED", strWBACCUSED);
                    cmd.Parameters.AddWithValue("@p_DESIGNATION", strWBDESIGNATION);
                    cmd.Parameters.AddWithValue("@p_PRESENTPOSTING", strWBPRESENTPOSTING);
                    cmd.Parameters.AddWithValue("@p_BRCOMPLAINT", strWBBRCOMPLAINT);
                    cmd.Parameters.AddWithValue("@p_ZONE", strWBZONE);
                    cmd.Parameters.AddWithValue("@p_CIRCLEOFFICE", strWBCIRCLEOFFICE);
                    cmd.Parameters.AddWithValue("@p_REGION", strWBREGION);
                    cmd.Parameters.AddWithValue("@p_RECDATECOMP", dtRECDATECOMP);
                    cmd.Parameters.AddWithValue("@p_SOURCE", strWBSOURCE);
                    cmd.Parameters.AddWithValue("@p_SOURCEREF", strWBSOURCEREF);
                    cmd.Parameters.AddWithValue("@p_SOURCEDATE", dtWBSOURCEDATE);
                    cmd.Parameters.AddWithValue("@p_SENTTO", strWBSENTTO);
                    cmd.Parameters.AddWithValue("@p_SENTFORINVDATE", dtWBSENTFORINVDATE);
                    cmd.Parameters.AddWithValue("@p_CATANO", strWBCATANO);
                    cmd.Parameters.AddWithValue("@p_CATBNO", strWBCATBNO);
                    cmd.Parameters.AddWithValue("@p_ASNO", strWBASNO);
                    cmd.Parameters.AddWithValue("@p_NATURECOMP", strWBNATURECOMP);
                    cmd.Parameters.AddWithValue("@p_ACCOUNTNAME", strWBACCOUNTNAME);
                    cmd.Parameters.AddWithValue("@p_AMOUNT", decWBAMOUNT);
                    cmd.Parameters.AddWithValue("@p_ALLEGATIONS", strWBALLEGATIONS);
                    cmd.Parameters.AddWithValue("@p_DTIAC", dtWBDTIAC);
                    cmd.Parameters.AddWithValue("@p_STATUS", strWBSTATUS);
                    cmd.Parameters.AddWithValue("@p_STATUSCODE", strWBSTATUSCODE);
                    cmd.Parameters.AddWithValue("@p_PENDINGWITH", strWBPENDINGWITH);
                    cmd.Parameters.AddWithValue("@p_NAMEOFINVOFFICIAL", strNAMEOFINVOFFICIAL);
                    cmd.Parameters.AddWithValue("@p_DTOFINVREPORT", dtWBDTOFINVREPORT);
                    cmd.Parameters.AddWithValue("@p_DAYSTAKEN", strWBDAYSTAKEN);
                    cmd.Parameters.AddWithValue("@p_CASENO", strCASENO);
                    cmd.Parameters.AddWithValue("@p_CASECLOSE", strWBCASECLOSE);
                    cmd.Parameters.AddWithValue("@p_CLOSUREDT", strCLOSUREDT);
                    cmd.Parameters.AddWithValue("@p_RYSENT", dtWBRYSENT);
                    cmd.Parameters.AddWithValue("@p_APLAN", strWBAPLAN);
                    cmd.Parameters.AddWithValue("@p_REGISTER", strWBREGISTER);
                    cmd.Parameters.AddWithValue("@p_NATURE", strWBNATURE);
                    cmd.Parameters.AddWithValue("@p_REASONSFORCLOSURE", strWBREASONSFORCLOSURE);
                    cmd.Parameters.AddWithValue("@p_BANKNAME", strWBBANKNAME);

                    cmd.Parameters.AddWithValue("@p_ADDUSER", Session["userid"].ToString());
                    cmd.Parameters.AddWithValue("@p_ADDUSERIP", objCommonFunction.funcGetUserIP());

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception e12)
                {
                    //throw ex;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e12);
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();
        }

        protected void funcExcelImport_LODI()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["LODIEXCELDETAILS"]);
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlTransaction txn = null;
            string UNIQUENO = string.Empty;
            string LODINO = string.Empty;
            DateTime? LODI_AS_ON_DATE = null;
            DateTime? LODI_EMP_DOR = null;
            DateTime? LODI_PUNISHMENT_DATE = null;
            DateTime? LODI_DATE_OF_CHARGE_SHEET = null;
            Int32 TotalRow = 0;
            string ROWNO = string.Empty;

            try
            {
                con.Open();
                cmd.Connection = con;
                txn = cmd.Connection.BeginTransaction();
                cmd.Transaction = txn;

                if (dt.Rows.Count > 0)
                {
                    TotalRow = dt.Rows.Count;

                    foreach (DataRow row in dt.Rows)
                    {
                        string ID = Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper();
                        UNIQUENO = "LODI" + DateTime.Now.ToString("ddMMyy") + ID;

                        string strLODI_AS_ON_DATE = Convert.ToString(row["LODI_AS_ON_DATE"]);
                        if (!string.IsNullOrEmpty(strLODI_AS_ON_DATE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strLODI_AS_ON_DATE, out date))
                                LODI_AS_ON_DATE = date;
                        }

                        string strEMP_DOR = Convert.ToString(row["EMP_DOR"]);
                        if (!string.IsNullOrEmpty(strEMP_DOR))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strEMP_DOR, out date))
                                LODI_EMP_DOR = date;
                        }

                        string strPUNISHMENT_DATE = Convert.ToString(row["PUNISHMENT_DATE"]);
                        if (!string.IsNullOrEmpty(strPUNISHMENT_DATE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strPUNISHMENT_DATE, out date))
                                LODI_PUNISHMENT_DATE = date;
                        }

                        string strDATE_OF_CHARGE_SHEET = Convert.ToString(row["DATE_OF_CHARGE_SHEET"]);
                        if (!string.IsNullOrEmpty(strDATE_OF_CHARGE_SHEET))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDATE_OF_CHARGE_SHEET, out date))
                                LODI_DATE_OF_CHARGE_SHEET = date;
                        }

                        LODINO = Convert.ToString(row["LODINO"]);
                        ROWNO = Convert.ToString(row["ROWNO"]);

                        cmd.Parameters.Clear();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[dbo].[spLodiExcel_Import]";

                        SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                        SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(sqlErrMsgOutput);
                        cmd.Parameters.Add(sqlErrCodeOutput);

                        cmd.Parameters.AddWithValue("@p_UNIQUENO", UNIQUENO);
                        cmd.Parameters.AddWithValue("@p_LODIASONDATE", LODI_AS_ON_DATE);
                        cmd.Parameters.AddWithValue("@p_LODINO", LODINO);
                        cmd.Parameters.AddWithValue("@p_VIGCASENO", Convert.ToString(row["VIG_CASE_NO"]));
                        cmd.Parameters.AddWithValue("@p_PFNO", Convert.ToString(row["PFNO"]));
                        cmd.Parameters.AddWithValue("@p_NAME", Convert.ToString(row["NAME"]));
                        cmd.Parameters.AddWithValue("@p_DOR", LODI_EMP_DOR);
                        cmd.Parameters.AddWithValue("@p_SCALE", Convert.ToString(row["SCALE"]));
                        cmd.Parameters.AddWithValue("@p_CBI", Convert.ToString(row["CBI"]));
                        cmd.Parameters.AddWithValue("@p_DOP", LODI_PUNISHMENT_DATE);
                        cmd.Parameters.AddWithValue("@p_DOCS", LODI_DATE_OF_CHARGE_SHEET);
                        cmd.Parameters.AddWithValue("@p_ALLEGATIONS", Convert.ToString(row["ALLEGATIONS"]));
                        cmd.Parameters.AddWithValue("@p_REASON", Convert.ToString(row["REASON"]));
                        cmd.Parameters.AddWithValue("@p_ZONE", Convert.ToString(row["ZONE_SOLID"]));
                        cmd.Parameters.AddWithValue("@p_CIRCLE", Convert.ToString(row["CIRCLE_SOLID"]));
                        cmd.Parameters.AddWithValue("@P_DELETED_FROM_LODI", Convert.ToString(row["DELETED_FROM_LODI"]));
                        cmd.Parameters.AddWithValue("@p_DELETED_REASON", Convert.ToString(row["DELETED_REASON"]));
                        cmd.Parameters.AddWithValue("@p_REMARKS", Convert.ToString(row["REMARKS"]));

                        cmd.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["userid"]));
                        cmd.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                        cmd.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));

                        cmd.CommandTimeout = 0;
                        intErrCode = 0;
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            intTotalRowInsert = intTotalRowInsert + 1;
                            strErrMsg = sqlErrMsgOutput.Value.ToString();
                            intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                        }
                        else
                        {
                            lblMsg.Text = "Error during Insert Lodi Data";
                            return;
                        }

                    }
                    if (intErrCode.Equals(1))
                    {
                        txn.Commit();
                        lblMsg.Text = intTotalRowInsert + " records added successfully";
                    }
                }
                else
                {
                    lblMsg.Text = "Error - no record in Uploaded Excel sheet....!";
                    return;
                }
            }
            catch (Exception ex)
            {
                txn.Rollback();
                lblMsg.Text = "Row No : " + ROWNO + " Lodi No : " + LODINO + " Exception : " + ex.Message;
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                txn.Dispose();
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
        }

        protected void funcExcelImport_Vigilance()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["VIGEXCELDETAILS"]);
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlTransaction txn = null;
            string UNIQUENO = string.Empty;
            Int32 TotalRow = 0;
            string ROWNO = string.Empty;
            try
            {
                con.Open();
                cmd.Connection = con;
                txn = cmd.Connection.BeginTransaction();
                cmd.Transaction = txn;

                if (dt.Rows.Count > 0)
                {
                    TotalRow = dt.Rows.Count;

                    foreach (DataRow row in dt.Rows)
                    {
                        TextBox txtAmount = new TextBox();
                        txtAmount.Text = row["AMOUNT"].ToString();
                        decAMOUNT = objCommonFunction.convertToDecimal(txtAmount);

                        #region ** convert Date **
                        string strDTCHARGE = row["DTCHARGE"].ToString();
                        if (!string.IsNullOrEmpty(strDTCHARGE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDTCHARGE, out date))
                                dtDTCHARGE = date;
                        }

                        string strDTRNO = row["DTRNO"].ToString();
                        if (!string.IsNullOrEmpty(strDTRNO))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDTRNO, out date))
                                dtDTRNO = date;
                        }

                        string strDTOFRETIREMENT = row["DTOFRETIREMENT"].ToString();
                        if (!string.IsNullOrEmpty(strDTOFRETIREMENT))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDTOFRETIREMENT, out date))
                                dtDTOFRETIREMENT = date;
                        }

                        string strDTOFSUSPENSION = row["DTOFSUSPENSION"].ToString();
                        if (!string.IsNullOrEmpty(strDTOFSUSPENSION))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDTOFSUSPENSION, out date))
                                dtDTOFSUSPENSION = date;
                        }

                        string strDT_RC1 = row["DT_RC1"].ToString();
                        if (!string.IsNullOrEmpty(strDT_RC1))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDT_RC1, out date))
                                dtDT_RC1 = date;
                        }

                        string strDT_RC2 = row["DT_RC2"].ToString();
                        if (!string.IsNullOrEmpty(strDT_RC2))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDT_RC2, out date))
                                dtDT_RC2 = date;
                        }

                        string strDTSANCTIONORDER = row["DTSANCTIONORDER"].ToString();
                        if (!string.IsNullOrEmpty(strDTSANCTIONORDER))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDTSANCTIONORDER, out date))
                                dtDTSANCTIONORDER = date;
                        }

                        string strDTREFERTOCVC = row["DTREFERTOCVC"].ToString();
                        if (!string.IsNullOrEmpty(strDTREFERTOCVC))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDTREFERTOCVC, out date))
                                dtDTREFERTOCVC = date;
                        }

                        string strDT_OM_CVC = row["DT_OM_CVC"].ToString();
                        if (!string.IsNullOrEmpty(strDT_OM_CVC))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDT_OM_CVC, out date))
                                dtDT_OM_CVC = date;
                        }

                        string strDT_ERCO = row["DT_ERCO"].ToString();
                        if (!string.IsNullOrEmpty(strDT_ERCO))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDT_ERCO, out date))
                                dtDT_ERCO = date;
                        }

                        string strDTREPLYCO = row["DTREPLYCO"].ToString();
                        if (!string.IsNullOrEmpty(strDTREPLYCO))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDTREPLYCO, out date))
                                dtDTREPLYCO = date;
                        }

                        string strDT_APP_PO = row["DT_APP_PO"].ToString();
                        if (!string.IsNullOrEmpty(strDT_APP_PO))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDT_APP_PO, out date))
                                dtDT_APP_PO = date;
                        }

                        string strDT_APP_EO = row["DT_APP_EO"].ToString();
                        if (!string.IsNullOrEmpty(strDT_APP_EO))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDT_APP_EO, out date))
                                dtDT_APP_EO = date;
                        }

                        string strDT_APP_CDI = row["DT_APP_CDI"].ToString();
                        if (!string.IsNullOrEmpty(strDT_APP_CDI))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDT_APP_CDI, out date))
                                dtDT_APP_CDI = date;
                        }

                        string strREF_CVC_2 = row["REF_CVC_2"].ToString();
                        if (!string.IsNullOrEmpty(strREF_CVC_2))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strREF_CVC_2, out date))
                                dtREF_CVC_2 = date;
                        }

                        string strREC_CVC_2 = row["REC_CVC_2"].ToString();
                        if (!string.IsNullOrEmpty(strREC_CVC_2))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strREC_CVC_2, out date))
                                dtREC_CVC_2 = date;
                        }

                        string strDT_ORD_DA = row["DT_ORD_DA"].ToString();
                        if (!string.IsNullOrEmpty(strDT_ORD_DA))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDT_ORD_DA, out date))
                                dtDT_ORD_DA = date;
                        }

                        string strREVIEWDATE = row["REVIEWDATE"].ToString();
                        if (!string.IsNullOrEmpty(strREVIEWDATE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strREVIEWDATE, out date))
                                dtREVIEWDATE = date;
                        }

                        string strDTFINAL = row["DTFINAL"].ToString();
                        if (!string.IsNullOrEmpty(strDTFINAL))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDTFINAL, out date))
                                dtDTFINAL = date;
                        }

                        string strDATEOFCLOSURE = row["DATEOFCLOSURE"].ToString();
                        if (!string.IsNullOrEmpty(strDATEOFCLOSURE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDATEOFCLOSURE, out date))
                                dtDATEOFCLOSURE = date;
                        }

                        string strDTOFPLACEMENTINPRESENTSCALE = row["DTOFPLACEMENTINPRESENTSCALE"].ToString();
                        if (!string.IsNullOrEmpty(strDTOFPLACEMENTINPRESENTSCALE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDTOFPLACEMENTINPRESENTSCALE, out date))
                                dtDTOFPLACEMENTINPRESENTSCALE = date;
                        }

                        string strDATEOFCOMPLAINT = row["DATEOFCOMPLAINT"].ToString();
                        if (!string.IsNullOrEmpty(strDATEOFCOMPLAINT))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDATEOFCOMPLAINT, out date))
                                dtDATEOFCOMPLAINT = date;
                        }

                        string strDT_IST_DA = row["DT_IST_DA"].ToString();
                        if (!string.IsNullOrEmpty(strDT_IST_DA))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDT_IST_DA, out date))
                                dtDT_IST_DA = date;
                        }

                        string strDT_CVO_ADVICE = row["DT_CVO_ADVICE"].ToString();
                        if (!string.IsNullOrEmpty(strDT_CVO_ADVICE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDT_CVO_ADVICE, out date))
                                dtDT_CVO_ADVICE = date;
                        }

                        string strDT_2ND_DA = row["DT_2ND_DA"].ToString();
                        if (!string.IsNullOrEmpty(strDT_2ND_DA))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDT_2ND_DA, out date))
                                dtDT_2ND_DA = date;
                        }

                        string strDT_CVO_ADVICE_2 = row["DT_CVO_ADVICE_2"].ToString();
                        if (!string.IsNullOrEmpty(strDT_CVO_ADVICE_2))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDT_CVO_ADVICE_2, out date))
                                dtDT_CVO_ADVICE_2 = date;
                        }

                        string strA1C_CVC = row["A1C_CVC"].ToString();
                        if (!string.IsNullOrEmpty(strA1C_CVC))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strA1C_CVC, out date))
                                dtA1C_CVC = date;
                        }

                        string strA1E_CVC = row["A1E_CVC"].ToString();
                        if (!string.IsNullOrEmpty(strA1E_CVC))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strA1E_CVC, out date))
                                dtA1E_CVC = date;
                        }

                        string strA2_CVC = row["A2_CVC"].ToString();
                        if (!string.IsNullOrEmpty(strA2_CVC))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strA2_CVC, out date))
                                dtA2_CVC = date;
                        }

                        #endregion

                        ROWNO = Convert.ToString(row["ROWNO"]);
                        UNIQUENO = Convert.ToString(row["RNO"]);

                        cmd.Connection = con;
                        cmd.Parameters.Clear();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[dbo].[spVigilanceExcel_Import]";

                        SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                        SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(sqlErrMsgOutput);
                        cmd.Parameters.Add(sqlErrCodeOutput);

                        cmd.Parameters.AddWithValue("@p_RNO", Convert.ToString(row["RNO"]));
                        cmd.Parameters.AddWithValue("@p_RNO1", Convert.ToString(row["RNO1"]));
                        cmd.Parameters.AddWithValue("@p_NAMEOFPARTICULARS", Convert.ToString(row["NAMEOFPARTICULARS"]));
                        cmd.Parameters.AddWithValue("@p_NAME", Convert.ToString(row["NAME"]));
                        cmd.Parameters.AddWithValue("@p_SCALE", Convert.ToString(row["SCALE"]));
                        cmd.Parameters.AddWithValue("@p_DESIGNATION", Convert.ToString(row["DESIGNATION"]));
                        cmd.Parameters.AddWithValue("@p_BRNAME", Convert.ToString(row["BRNAME"]));
                        cmd.Parameters.AddWithValue("@p_CIRCLEOFFICE", Convert.ToString(row["CIRCLEOFFICE"]));
                        cmd.Parameters.AddWithValue("@p_STATE", Convert.ToString(row["STATE"]));
                        cmd.Parameters.AddWithValue("@p_LAPSENATURE", Convert.ToString(row["LAPSENATURE"]));
                        cmd.Parameters.AddWithValue("@p_SOURCE", Convert.ToString(row["SOURCE"]));
                        cmd.Parameters.AddWithValue("@p_ACCTT_NAME", Convert.ToString(row["ACCTT_NAME"]));
                        cmd.Parameters.AddWithValue("@p_AMOUNT", decAMOUNT);
                        cmd.Parameters.AddWithValue("@p_NATUREOFACCOUNT", Convert.ToString(row["NATUREOFACCOUNT"]));
                        cmd.Parameters.AddWithValue("@p_INVESTIG ", Convert.ToString(row["INVESTIG"]));
                        cmd.Parameters.AddWithValue("@p_CBI_RC_NO1 ", Convert.ToString(row["CBI_RC_NO1"]));
                        cmd.Parameters.AddWithValue("@p_CBI_RC_NO2 ", Convert.ToString(row["CBI_RC_NO2"]));
                        cmd.Parameters.AddWithValue("@p_CBI_ZONE ", Convert.ToString(row["CBI_ZONE"]));
                        cmd.Parameters.AddWithValue("@p_RC_SOURCE", Convert.ToString(row["RC_SOURCE"]));
                        cmd.Parameters.AddWithValue("@p_RECOM_CBI ", Convert.ToString(row["RECOM_CBI"]));
                        cmd.Parameters.AddWithValue("@p_PROPOSEDACTIONTOCVC ", Convert.ToString(row["PROPOSEDACTIONTOCVC"]));
                        cmd.Parameters.AddWithValue("@p_CVC_2_PROPOSED ", Convert.ToString(row["CVC_2_PROPOSED"]));
                        cmd.Parameters.AddWithValue("@p_CVC_OM_NO ", Convert.ToString(row["CVC_OM_NO"]));
                        cmd.Parameters.AddWithValue("@p_RECOMMOFCVC ", Convert.ToString(row["RECOMMOFCVC"]));
                        cmd.Parameters.AddWithValue("@p_NAT_CHSHEET ", Convert.ToString(row["NAT_CHSHEET"]));
                        cmd.Parameters.AddWithValue("@p_REG_INVOK ", Convert.ToString(row["REG_INVOK"]));
                        cmd.Parameters.AddWithValue("@p_NAME_PO ", Convert.ToString(row["NAME_PO"]));
                        cmd.Parameters.AddWithValue("@p_NAME_EO ", Convert.ToString(row["NAME_EO"]));
                        cmd.Parameters.AddWithValue("@p_NAME_CDI ", Convert.ToString(row["NAME_CDI"]));
                        cmd.Parameters.AddWithValue("@p_PUNISHMENTPROPOSEDBY ", Convert.ToString(row["PUNISHMENTPROPOSEDBY"]));
                        cmd.Parameters.AddWithValue("@p_CVCSADVICEII ", Convert.ToString(row["CVCSADVICEII"]));
                        cmd.Parameters.AddWithValue("@p_NA_PUN_DA ", Convert.ToString(row["NA_PUN_DA"]));
                        cmd.Parameters.AddWithValue("@p_PENALTY ", Convert.ToString(row["PENALTY"]));
                        cmd.Parameters.AddWithValue("@p_FINAL ", Convert.ToString(row["FINAL"]));
                        cmd.Parameters.AddWithValue("@p_DISP_AUTHORITY ", Convert.ToString(row["DISP_AUTHORITY"]));
                        cmd.Parameters.AddWithValue("@p_DISAUTHORITYSCIRCLE ", Convert.ToString(row["DISAUTHORITYSCIRCLE"]));
                        cmd.Parameters.AddWithValue("@p_STATUS ", Convert.ToString(row["STATUS"]));
                        cmd.Parameters.AddWithValue("@p_STATUS_INBRIEF ", Convert.ToString(row["STATUS_INBRIEF"]));
                        cmd.Parameters.AddWithValue("@p_STATUSCODE ", Convert.ToString(row["STATUSCODE"]));
                        cmd.Parameters.AddWithValue("@p_BASICPAY ", Convert.ToString(row["BASICPAY"]));
                        cmd.Parameters.AddWithValue("@p_PREVCASE_PUNISHMENTS ", Convert.ToString(row["PREVCASE_PUNISHMENTS"]));
                        cmd.Parameters.AddWithValue("@p_LODICASE ", Convert.ToString(row["LODICASE"]));
                        cmd.Parameters.AddWithValue("@p_LODINO ", Convert.ToString(row["LODINO"]));
                        cmd.Parameters.AddWithValue("@p_NATURECASE ", Convert.ToString(row["NATURECASE"]));
                        cmd.Parameters.AddWithValue("@p_REGISTER ", Convert.ToString(row["REGISTER"]));
                        cmd.Parameters.AddWithValue("@p_PFNUMBER ", Convert.ToString(row["PFNUMBER"]));
                        cmd.Parameters.AddWithValue("@p_DAPROPOSAL ", Convert.ToString(row["DAPROPOSAL"]));
                        cmd.Parameters.AddWithValue("@p_ADVICECVOI ", Convert.ToString(row["ADVICECVOI"]));
                        cmd.Parameters.AddWithValue("@p_DAPROPOSAL_2 ", Convert.ToString(row["DAPROPOSAL_2"]));
                        cmd.Parameters.AddWithValue("@p_ADVICECVO2 ", Convert.ToString(row["ADVICECVO2"]));
                        cmd.Parameters.AddWithValue("@p_FEILD1 ", Convert.ToString(row["FEILD1"]));
                        cmd.Parameters.AddWithValue("@p_DESK_USER_REMARKS", Convert.ToString(row["DESK_USER_REMARKS"]));
                        cmd.Parameters.AddWithValue("@p_BANKNAME", Convert.ToString(row["BANKNAME"]));

                        //Date Columns
                        cmd.Parameters.AddWithValue("@p_DTCHARGE", dtDTCHARGE);
                        cmd.Parameters.AddWithValue("@p_DTRNO", dtDTRNO);
                        cmd.Parameters.AddWithValue("@p_DTOFRETIREMENT", dtDTOFRETIREMENT);
                        cmd.Parameters.AddWithValue("@p_DTOFSUSPENSION", dtDTOFSUSPENSION);
                        cmd.Parameters.AddWithValue("@p_DT_RC1", dtDT_RC1);
                        cmd.Parameters.AddWithValue("@p_DT_RC2", dtDT_RC2);
                        cmd.Parameters.AddWithValue("@p_DTSANCTIONORDER", dtDTSANCTIONORDER);
                        cmd.Parameters.AddWithValue("@p_DTREFERTOCVC", dtDTREFERTOCVC);
                        cmd.Parameters.AddWithValue("@p_DT_OM_CVC", dtDT_OM_CVC);
                        cmd.Parameters.AddWithValue("@p_DT_ERCO", dtDT_ERCO);
                        cmd.Parameters.AddWithValue("@p_DTREPLYCO", dtDTREPLYCO);
                        cmd.Parameters.AddWithValue("@p_DT_APP_PO", dtDT_APP_PO);
                        cmd.Parameters.AddWithValue("@p_DT_APP_EO", dtDT_APP_EO);
                        cmd.Parameters.AddWithValue("@p_DT_APP_CDI", dtDT_APP_CDI);
                        cmd.Parameters.AddWithValue("@p_REF_CVC_2", dtREF_CVC_2);
                        cmd.Parameters.AddWithValue("@p_REC_CVC_2", dtREC_CVC_2);
                        cmd.Parameters.AddWithValue("@p_DT_ORD_DA", dtDT_ORD_DA);
                        cmd.Parameters.AddWithValue("@p_REVIEWDATE", dtREVIEWDATE);
                        cmd.Parameters.AddWithValue("@p_DTFINAL", dtDTFINAL);
                        cmd.Parameters.AddWithValue("@p_DATEOFCLOSURE", dtDATEOFCLOSURE);
                        cmd.Parameters.AddWithValue("@p_DTOFPLACEMENTINPRESENTSCALE", dtDTOFPLACEMENTINPRESENTSCALE);
                        cmd.Parameters.AddWithValue("@p_DATEOFCOMPLAINT", dtDATEOFCOMPLAINT);
                        cmd.Parameters.AddWithValue("@p_DT_IST_DA", dtDT_IST_DA);
                        cmd.Parameters.AddWithValue("@p_DT_CVO_ADVICE", dtDT_CVO_ADVICE);
                        cmd.Parameters.AddWithValue("@p_DT_2ND_DA", dtDT_2ND_DA);
                        cmd.Parameters.AddWithValue("@p_DT_CVO_ADVICE_2", dtDT_CVO_ADVICE_2);
                        cmd.Parameters.AddWithValue("@p_A1C_CVC", dtA1C_CVC);
                        cmd.Parameters.AddWithValue("@p_A1E_CVC", dtA1E_CVC);
                        cmd.Parameters.AddWithValue("@p_A2_CVC", dtA2_CVC);
                        cmd.Parameters.AddWithValue("@p_TMSACREFNO", Convert.ToString(row["TMSACREFNO"]));
                        cmd.Parameters.AddWithValue("@p_ADDUSERIP", objCommonFunction.funcGetUserIP());
                        cmd.Parameters.AddWithValue("@p_ADDUSER", Convert.ToString(Session["userid"]));

                        cmd.CommandTimeout = 0;
                        intErrCode = 0;
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            intTotalRowInsert = intTotalRowInsert + 1;
                            strErrMsg = sqlErrMsgOutput.Value.ToString();
                            intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                        }
                        else
                        {
                            lblMsg.Text = "Error during Insert Vigilance Data";
                            return;
                        }
                    }
                    if (intErrCode.Equals(1))
                    {
                        txn.Commit();
                        lblMsg.Text = intTotalRowInsert + " records added successfully";
                    }
                }
                else
                {
                    lblMsg.Text = "Error - no record in Uploaded Excel sheet....!";
                    return;
                }
            }
            catch (Exception ex)
            {
                txn.Rollback();
                lblMsg.Text = "Row No : " + ROWNO + " R No : " + UNIQUENO + " Exception : " + ex.Message;
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                txn.Dispose();
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
        }

        protected void funcExcelImport_IAC()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["IACEXCELDETAILS"]);
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlTransaction txn = null;
            string UNIQUENO = string.Empty;
            Int32 TotalRow = 0;
            string ROWNO = string.Empty;
            try
            {
                con.Open();
                cmd.Connection = con;
                txn = cmd.Connection.BeginTransaction();
                cmd.Transaction = txn;

                if (dt.Rows.Count > 0)
                {
                    TotalRow = dt.Rows.Count;
                    foreach (DataRow row in dt.Rows)
                    {
                        decAMOUNT = objCommonFunction.convertToDecimal(Convert.ToString(row["AMOUNT"]));

                        string strDTRET = Convert.ToString(row["DTRET"]);
                        if (!string.IsNullOrEmpty(strDTRET))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDTRET, out date))
                                dtDTRET = date;
                        }

                        string strDTIAC = Convert.ToString(row["DTIAC"]);
                        if (!string.IsNullOrEmpty(strDTIAC))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDTIAC, out date))
                                dtDTIAC = date;
                        }

                        string strRECDT = Convert.ToString(row["RECDT"]);
                        if (!string.IsNullOrEmpty(strRECDT))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strRECDT, out date))
                                dtRECDT = date;
                        }

                        string strDATEIADNOTE = Convert.ToString(row["DATEIADNOTE"]);
                        if (!string.IsNullOrEmpty(strDATEIADNOTE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDATEIADNOTE, out date))
                                dtDATEIADNOTE = date;
                        }

                        string strCLOSUREDT = Convert.ToString(row["CLOSUREDT"]);
                        if (!string.IsNullOrEmpty(strCLOSUREDT))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strCLOSUREDT, out date))
                                dtCLOSUREDT = date;
                        }

                        UNIQUENO = Convert.ToString(row["IACNO"]);
                        ROWNO = Convert.ToString(row["ROWNO"]);

                        cmd.Connection = con;
                        cmd.Parameters.Clear();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[dbo].[spIACExcel_Import]";

                        SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                        SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(sqlErrMsgOutput);
                        cmd.Parameters.Add(sqlErrCodeOutput);

                        cmd.Parameters.AddWithValue("@p_MEETNO", Convert.ToString(row["MEETNO"]));
                        cmd.Parameters.AddWithValue("@p_IACNO", Convert.ToString(row["IACNO"]));
                        cmd.Parameters.AddWithValue("@p_IACNO_1", Convert.ToString(row["IACNO_1"]));
                        cmd.Parameters.AddWithValue("@p_VIGNO", Convert.ToString(row["VIGNO"]));
                        cmd.Parameters.AddWithValue("@p_ACCUSED", Convert.ToString(row["ACCUSED"]));
                        cmd.Parameters.AddWithValue("@p_PFNUMBER", Convert.ToString(row["PFNUMBER"]));
                        cmd.Parameters.AddWithValue("@p_SOURCE", Convert.ToString(row["SOURCE"]));
                        cmd.Parameters.AddWithValue("@p_PRESENTPOSTING", Convert.ToString(row["PRESENTPOSTING"]));
                        cmd.Parameters.AddWithValue("@p_NAMEOFTHEBRANCH", Convert.ToString(row["NAMEOFTHEBRANCH"]));
                        cmd.Parameters.AddWithValue("@p_ZONE", Convert.ToString(row["ZONE"]));
                        cmd.Parameters.AddWithValue("@p_CIRCLEOFFICE", Convert.ToString(row["CIRCLEOFFICE"]));
                        cmd.Parameters.AddWithValue("@p_ACCOUNTNAME", Convert.ToString(row["ACNAME"]));
                        cmd.Parameters.AddWithValue("@p_NATURECASE", Convert.ToString(row["NATURECASE"]));
                        cmd.Parameters.AddWithValue("@p_DA", Convert.ToString(row["DA"]));
                        cmd.Parameters.AddWithValue("@p_STATUS", Convert.ToString(row["STATUS"]));
                        cmd.Parameters.AddWithValue("@p_DAVIEW", Convert.ToString(row["DAVIEW"]));
                        cmd.Parameters.AddWithValue("@p_IACVIEW", Convert.ToString(row["IACVIEW"]));
                        cmd.Parameters.AddWithValue("@p_CVOVIEW", Convert.ToString(row["CVOVIEW"]));
                        cmd.Parameters.AddWithValue("@p_STATUSCODE", Convert.ToString(row["STATUSCODE"]));
                        cmd.Parameters.AddWithValue("@p_AMOUNT", decAMOUNT);
                        cmd.Parameters.AddWithValue("@p_TABLENAME", strTableValue);
                        cmd.Parameters.AddWithValue("@p_DTRET", dtDTRET);
                        cmd.Parameters.AddWithValue("@p_DTIAC", dtDTIAC);
                        cmd.Parameters.AddWithValue("@p_RECDT", dtRECDT);
                        cmd.Parameters.AddWithValue("@p_DATEIADNOTE", dtDATEIADNOTE);
                        cmd.Parameters.AddWithValue("@p_CLOSUREDT", dtCLOSUREDT);
                        cmd.Parameters.AddWithValue("@p_NEWZONESOLID", Convert.ToString(row["NEWZONESOLID"]));
                        cmd.Parameters.AddWithValue("@p_NEWCIRCLESOLID", Convert.ToString(row["NEWCIRCLESOLID"]));
                        cmd.Parameters.AddWithValue("@p_BANKNAME", Convert.ToString(row["BANKNAME"]));
                        cmd.Parameters.AddWithValue("@p_DESIGNATION", Convert.ToString(row["DESIGNATION"]));
                        cmd.Parameters.AddWithValue("@p_SCALE", Convert.ToString(row["SCALE"]));
                        cmd.Parameters.AddWithValue("@p_TMSACREFNO", Convert.ToString(row["TMSACREFNO"]));

                        cmd.Parameters.AddWithValue("@p_USER", strUser);
                        cmd.Parameters.AddWithValue("@p_ADDUSERIP", objCommonFunction.funcGetUserIP());

                        cmd.CommandTimeout = 0;
                        intErrCode = 0;
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            intTotalRowInsert = intTotalRowInsert + 1;
                            strErrMsg = sqlErrMsgOutput.Value.ToString();
                            intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                        }
                        else
                        {
                            lblMsg.Text = "Error during Insert IAC Data";
                            //return;
                        }

                    }
                    if (intErrCode.Equals(1))
                    {
                        txn.Commit();
                        lblMsg.Text = intTotalRowInsert + " records added successfully";
                    }
                }
                else
                {
                    lblMsg.Text = "Error - no record in Uploaded Excel sheet....!";
                    return;
                }
            }
            catch (Exception ex)
            {
                txn.Rollback();
                lblMsg.Text = "Row No : " + ROWNO + " IAC No : " + UNIQUENO + " Exception : " + ex.Message;
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                txn.Dispose();
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
        }

        protected void funcExcelImport_NOC()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["NOCEXCELDETAILS"]);
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlTransaction txn = null;
            string UNIQUENO = string.Empty;
            Int32 TotalRow = 0;
            string ROWNO = string.Empty;

            try
            {
                con.Open();
                cmd.Connection = con;
                txn = cmd.Connection.BeginTransaction();
                cmd.Transaction = txn;

                if (dt.Rows.Count > 0)
                {
                    TotalRow = dt.Rows.Count;

                    foreach (DataRow row in dt.Rows)
                    {
                        string strRECDT = Convert.ToString(row["RECDT"]);
                        if (!string.IsNullOrEmpty(strRECDT))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strRECDT, out date))
                                dtDTRECDT = date;
                        }

                        string strCLEARANCEDT = Convert.ToString(row["CLEARANCEDT"]);
                        if (!string.IsNullOrEmpty(strCLEARANCEDT))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strCLEARANCEDT, out date))
                                dtDTCLEARANCEDT = date;
                        }

                        ROWNO = Convert.ToString(row["ROWNO"]);
                        UNIQUENO = Convert.ToString(row["SNO"]);

                        cmd.Parameters.Clear();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[dbo].[spNOCExcel_Import]";

                        SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                        SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(sqlErrMsgOutput);
                        cmd.Parameters.Add(sqlErrCodeOutput);

                        cmd.Parameters.AddWithValue("@p_SNO", Convert.ToString(row["SNO"]));
                        cmd.Parameters.AddWithValue("@p_NAME", Convert.ToString(row["NAME"]));
                        cmd.Parameters.AddWithValue("@p_BRANCH", Convert.ToString(row["BRANCH"]));
                        cmd.Parameters.AddWithValue("@p_STATE", Convert.ToString(row["STATE"]));
                        cmd.Parameters.AddWithValue("@p_CIRCLEOFFICE", Convert.ToString(row["CIRCLEOFFICE"]));
                        cmd.Parameters.AddWithValue("@p_DESIGNATION", Convert.ToString(row["DESIGNATION"]));
                        cmd.Parameters.AddWithValue("@p_PFNO", Convert.ToString(row["PFNO"]));
                        cmd.Parameters.AddWithValue("@p_REMARKS", Convert.ToString(row["REMARKS"]));
                        cmd.Parameters.AddWithValue("@p_SCALE", Convert.ToString(row["SCALE"]));
                        cmd.Parameters.AddWithValue("@p_BANKNAME", Convert.ToString(row["BANKNAME"]));
                        cmd.Parameters.AddWithValue("@p_ADDUSER", Session["userid"].ToString());
                        cmd.Parameters.AddWithValue("@p_ADDUSERIP", objCommonFunction.funcGetUserIP());

                        cmd.Parameters.AddWithValue("@p_DTRECDT", dtDTRECDT);
                        cmd.Parameters.AddWithValue("@p_DTCLEARANCEDT", dtDTCLEARANCEDT);

                        cmd.CommandTimeout = 0;
                        intErrCode = 0;
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            intTotalRowInsert = intTotalRowInsert + 1;
                            strErrMsg = sqlErrMsgOutput.Value.ToString();
                            intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                        }
                        else
                        {
                            lblMsg.Text = "Error during Insert NOC Data";
                            return;
                        }
                    }
                    if (intErrCode.Equals(1))
                    {
                        txn.Commit();
                        lblMsg.Text = intTotalRowInsert + " records added successfully";
                    }
                }
                else
                {
                    lblMsg.Text = "Error - no record in Uploaded Excel sheet....!";
                    return;
                }
            }
            catch (Exception ex)
            {
                txn.Rollback();
                lblMsg.Text = "Row No : " + ROWNO + " S No : " + UNIQUENO + " Exception : " + ex.Message;
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                txn.Dispose();
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
        }

    }
}