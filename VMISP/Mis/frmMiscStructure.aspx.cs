using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using VMISP.DataAccessLayer;

namespace VMISP.Mis
{
    public partial class frmMiscStructure : System.Web.UI.Page
    {
        DateTime? dtRECDATECOMP = null;
        DateTime? dtSOURCEDATE = null;
        DateTime? dtSENTFORINVDATE = null;
        DateTime? dtNPADATE = null;
        DateTime? dtDTOFINVREPORT = null;
        DateTime? dtCLOSUREDT = null;
        DateTime? dtINVESTIGATIONDATE = null;
        DateTime? dtLETTERSENTDATE = null;
        DateTime? dtREMINDERDATE = null;
        DateTime? dtREPLYRECEIVEDDATE = null;
        DateTime? dtEODOR = null;

        string strCLOSURE = string.Empty;
        string EOUNIQUEID = string.Empty;

        CommonFunction objCommonFunction = new CommonFunction();
        MasterData objMasterData = new MasterData();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                funcShow(null, "LIST", null, null, null, null, null, null, null); //for bind grid view on form Load
                funcbindDropdown();     //Bind All DropDown Lists
            }

            lblMsg.Text = string.Empty;
            funcControlsUserRights();

            #region ** JS Function  **
            txtAmount.Attributes.Add("onkeypress", "return isNumbericDecimal(event,'" + txtAmount.ClientID + "')");

            //txtCompRecDate.Attributes.Add("readonly", "readonly");
            txtClosureDate.Attributes.Add("readonly", "readonly");
            txtNPADate.Attributes.Add("readonly", "readonly");
            txtSourceDate.Attributes.Add("readonly", "readonly");
            txtInvestigationDate.Attributes.Add("readonly", "readonly");
            txtDateForINVReport.Attributes.Add("readonly", "readonly");
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
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spMISC_Ddl]";
                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlCircleOffice, ds.Tables[0]);
                    objCommonFunction.bindDropdownList(ddlZone, ds.Tables[1]);
                    objCommonFunction.bindDropdownList(ddlLetterSentTo, ds.Tables[2]);
                    objCommonFunction.bindDropdownList(ddlStatusCode, ds.Tables[3]);
                    objCommonFunction.bindDropdownList(ddlSourceRef, ds.Tables[4]);
                    objCommonFunction.bindDropdownList(ddlNature, ds.Tables[5]);
                    objCommonFunction.bindDropdownList(ddlZoneNew, ds.Tables[6]);
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
                #region ** For Closure Date **
                strCLOSURE = objCommonFunction.chkSelected(chkClosureDate);
                if (Convert.ToString(lblClosureDate.Text) != "")
                {
                    strCLOSURE = "N";
                    txtClosureDate.Text = lblClosureDate.Text;
                    String strClosureDate = txtClosureDate.Text.Trim();
                    if (!String.IsNullOrEmpty(strClosureDate))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strClosureDate, out date))
                            dtCLOSUREDT = date;
                    }
                }
                #endregion

                #region ** convert Date **
                string strRECDATECOMP = txtCompRecDate.Text.Trim();
                if (!string.IsNullOrEmpty(strRECDATECOMP))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRECDATECOMP, out date))
                        dtRECDATECOMP = date;
                }

                String strNPADATE = txtNPADate.Text.Trim();
                if (!String.IsNullOrEmpty(strNPADATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strNPADATE, out date))
                        dtNPADATE = date;
                }

                string strSOURCEDATE = txtSourceDate.Text.Trim();
                if (!string.IsNullOrEmpty(strSOURCEDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strSOURCEDATE, out date))
                        dtSOURCEDATE = date;
                }

                string strINVESTIGATIONDATE = txtInvestigationDate.Text.Trim();
                if (!string.IsNullOrEmpty(strINVESTIGATIONDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strINVESTIGATIONDATE, out date))
                        dtINVESTIGATIONDATE = date;
                }

                string strDTOFINVREPORT = txtDateForINVReport.Text.Trim();
                if (!string.IsNullOrEmpty(strDTOFINVREPORT))
                {
                    DateTime date;
                    if (DateTime.TryParse(strDTOFINVREPORT, out date))
                        dtDTOFINVREPORT = date;
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
                cmdSave.CommandText = "[dbo].[spMiscStructure_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_CODE", objCommonFunction.convertToIntToolTip(txtRNo));
                cmdSave.Parameters.AddWithValue("@p_RNO", txtRNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_COMPNO", txtCompNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ACCUSED", txtAccused.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", txtDesignation.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_FINALACTION", txtFinalAction.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BRCOMPLAINT", txtBRComplaint.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ZONE", objCommonFunction.ddlSelectedText(ddlZone));
                cmdSave.Parameters.AddWithValue("@p_CIRCLEOFFICE", objCommonFunction.ddlSelectedText(ddlCircleOffice));
                cmdSave.Parameters.AddWithValue("@p_TYPE", txtType.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_RECDATECOMP", dtRECDATECOMP);
                cmdSave.Parameters.AddWithValue("@p_SOURCE", txtSource.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_SOURCEREF", objCommonFunction.ddlSelectedText(ddlSourceRef));
                cmdSave.Parameters.AddWithValue("@p_SOURCEDATE", dtSOURCEDATE);
                cmdSave.Parameters.AddWithValue("@p_SENTTO", "");
                cmdSave.Parameters.AddWithValue("@p_SENTFORINVDATE", dtSENTFORINVDATE);
                cmdSave.Parameters.AddWithValue("@p_ACCOUNTNAME", txtAccountName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_AMOUNT", objCommonFunction.convertToDecimal(txtAmount));
                cmdSave.Parameters.AddWithValue("@p_ALLEGATIONS", txtAllegations.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NPADATE", dtNPADATE);
                cmdSave.Parameters.AddWithValue("@p_STATUS", txtStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_HOSTATUS", txtHOStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_STATUSCODE", objCommonFunction.ddlSelectedValue(ddlStatusCode));
                cmdSave.Parameters.AddWithValue("@p_NATURE", objCommonFunction.ddlSelectedValue(ddlNature));
                cmdSave.Parameters.AddWithValue("@p_DTOFINVREPORT", dtDTOFINVREPORT);
                cmdSave.Parameters.AddWithValue("@p_NATURECOMP", txtNatureComp.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CASECLOSE", txtClose.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CLOSUREDT", dtCLOSUREDT);
                cmdSave.Parameters.AddWithValue("@p_INVESTIGATIONDATE", dtINVESTIGATIONDATE);
                cmdSave.Parameters.AddWithValue("@p_REASONSFORCLOSURE", txtRessonsForClosure.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["USERID"]));
                cmdSave.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["ROLE"]));
                cmdSave.Parameters.AddWithValue("@p_CLOSURE", strCLOSURE);

                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTO", objCommonFunction.ddlSelectedValue(ddlLetterSentTo));
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTDATE", dtLETTERSENTDATE);
                cmdSave.Parameters.AddWithValue("@p_REMINDERDATE", dtREMINDERDATE);
                cmdSave.Parameters.AddWithValue("@p_REPLYRECEIVEDDATE", dtREPLYRECEIVEDDATE);

                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                cmdSave.Parameters.AddWithValue("@p_DESK_USER_REMARKS", txtDealingOfficerRemarks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BANKNAME", objCommonFunction.ddlSelectedValue(ddlBankName));
                cmdSave.Parameters.AddWithValue("@p_PFNO", txtPFNumber.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ZONENEW", objCommonFunction.ddlSelectedValue(ddlZoneNew));
                cmdSave.Parameters.AddWithValue("@p_CIRCLENEW", objCommonFunction.ddlSelectedValue(ddlCircleNew));
                cmdSave.Parameters.AddWithValue("@p_ZONE_TYPE", objCommonFunction.ddlSelectedValue(ddlZoneType));
                cmdSave.Parameters.AddWithValue("@p_ZONE_CM", txtZOCM.Text.Trim());

                cmdSave.CommandTimeout = 0;

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    funcClear();
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
                }
                else
                {
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
                }
            }
            catch (Exception es)
            {
                lblMsg.Text = es.Message.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }
            finally
            {
                cmdSave.Dispose();
                conSave.Dispose();
                conSave.Close();
            }
        }

        public void funcShow(string p_strNo, string p_strView, string p_strBRCOMPLAINT, string p_strSTATUS, string p_strCIRCLEOFFICE, string p_strSOURCE, string p_strSOURCEREF, string p_strCOMPNO, string p_strACCOUNTNAME)
        {
            SqlConnection conView = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdView = new SqlCommand();

            try
            {
                DataTable dt = new DataTable();
                conView.Open();
                cmdView.Connection = conView;
                cmdView.Parameters.Clear();
                cmdView.CommandType = CommandType.StoredProcedure;
                cmdView.CommandText = "[dbo].[spMiscStructure_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_VIEW", p_strView);
                cmdView.Parameters.AddWithValue("@p_BRANCH", p_strBRCOMPLAINT);
                cmdView.Parameters.AddWithValue("@p_STATUS", p_strSTATUS);
                cmdView.Parameters.AddWithValue("@p_CIRCLEOFFICE", p_strCIRCLEOFFICE);
                cmdView.Parameters.AddWithValue("@p_SOURCE", p_strSOURCE);
                cmdView.Parameters.AddWithValue("@p_SOURCEREF", p_strSOURCEREF);
                cmdView.Parameters.AddWithValue("@p_COMPNO", p_strCOMPNO);
                cmdView.Parameters.AddWithValue("@p_ACCOUNTNAME", p_strACCOUNTNAME);

                cmdView.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmdView);
                sda.Fill(dt);
                ViewState["DETAILDATA"] = dt;

                if (Convert.ToInt32(sqlErrCodeOutput.Value) >= 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (p_strView.ToUpper().Equals("LIST"))
                        {
                            gvMain.DataSource = dt;
                            gvMain.DataBind();
                        }
                        else if (p_strView.ToUpper().Equals("SEARCH"))
                        {
                            gvMain.DataSource = dt;
                            gvMain.DataBind();
                            tabMain.ActiveTabIndex = 1;
                        }
                        else if (p_strView.ToUpper().Equals("GET"))
                        {
                            funcBindControl(dt);
                            funcShowEODetails(Convert.ToString(dt.Rows[0]["RNO"]));
                        }
                        else if (p_strView.ToUpper().Equals("VIEW"))
                        {
                            funcBindControl(dt);
                            funcShowEODetails(Convert.ToString(dt.Rows[0]["RNO"]));
                        }
                    }

                    funcControlsUserRights();
                }

                else
                {
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput);
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
            txtCompRecDate.Text = Convert.ToString(dtData.Rows[0]["COMPRECDATE"]);
            txtBRComplaint.Text = Convert.ToString(dtData.Rows[0]["BRCOMPLAINT"]);
            objCommonFunction.ddlSetData(ddlCircleOffice, Convert.ToString(dtData.Rows[0]["CIRCLEOFFICE"]), true);
            hidCircleOffice.Value = Convert.ToString(dtData.Rows[0]["CIRCLEOFFICE"]);
            txtCompNo.Text = Convert.ToString(dtData.Rows[0]["COMPNO"]);
            txtClosureDate.Text = Convert.ToString(dtData.Rows[0]["CLOSUREDATE"]);
            txtAccused.Text = Convert.ToString(dtData.Rows[0]["ACCUSED"]);
            txtAllegations.Text = Convert.ToString(dtData.Rows[0]["ALLEGATIONS"]);
            txtFinalAction.Text = Convert.ToString(dtData.Rows[0]["FINALACTION"]);
            txtNPADate.Text = Convert.ToString(dtData.Rows[0]["NPADATE"]);
            txtType.Text = Convert.ToString(dtData.Rows[0]["TYPE"]);
            objCommonFunction.ddlSetData(ddlZone, Convert.ToString(dtData.Rows[0]["ZONE"]), true);
            hidZone.Value = Convert.ToString(dtData.Rows[0]["ZONE"]);
            txtSource.Text = Convert.ToString(dtData.Rows[0]["SOURCE"]);
            txtSourceDate.Text = Convert.ToString(dtData.Rows[0]["SOURCEDATE"]);
            objCommonFunction.ddlSetData(ddlSourceRef, Convert.ToString(dtData.Rows[0]["SOURCEREF"]), true);
            hidSourceRef.Value = Convert.ToString(dtData.Rows[0]["SOURCEREF"]);
            txtAccountName.Text = Convert.ToString(dtData.Rows[0]["ACCOUNTNAME"]);
            txtInvestigationDate.Text = Convert.ToString(dtData.Rows[0]["INVESTIGATIONDATE"]);
            txtNatureComp.Text = Convert.ToString(dtData.Rows[0]["NATURECOMP"]);
            txtAmount.Text = Convert.ToString(dtData.Rows[0]["AMOUNT"]);
            txtDateForINVReport.Text = Convert.ToString(dtData.Rows[0]["INVREPORTDATE"]);
            txtDesignation.Text = Convert.ToString(dtData.Rows[0]["DESIGNATION"]);
            txtClose.Text = Convert.ToString(dtData.Rows[0]["CASECLOSE"]);
            txtRessonsForClosure.Text = Convert.ToString(dtData.Rows[0]["REASONSFORCLOSURE"]);
            txtStatus.Text = Convert.ToString(dtData.Rows[0]["STATUS"]);
            objCommonFunction.ddlSetData(ddlSourceRef, Convert.ToString(dtData.Rows[0]["SOURCEREF"]), true);
            hidSourceRef.Value = Convert.ToString(dtData.Rows[0]["SOURCEREF"]);
            objCommonFunction.chkSetData(chkClosureDate, Convert.ToString(dtData.Rows[0]["CLOSURE"]));
            lblClosureDate.Text = Convert.ToString(dtData.Rows[0]["CLOSUREDATE"]);

            objCommonFunction.ddlSetDataValue(ddlStatusCode, Convert.ToString(dtData.Rows[0]["STATUSCODE"]));
            hidStatusCode.Value = Convert.ToString(dtData.Rows[0]["STATUSCODE"]);
            if (objCommonFunction.ddlSelectedValue(ddlStatusCode) == "0" && Convert.ToString(dtData.Rows[0]["STATUSCODE"]) != "0")
            {
                lblStatusCodeMIS.Text = Convert.ToString(dtData.Rows[0]["STATUSCODE"]);
            }

            objCommonFunction.ddlSetDataValue(ddlNature, Convert.ToString(dtData.Rows[0]["NATURE"]));
            hidNature.Value = Convert.ToString(dtData.Rows[0]["NATURE"]);
            if (objCommonFunction.ddlSelectedValue(ddlNature) == "0" && Convert.ToString(dtData.Rows[0]["NATURE"]) != "0")
            {
                lblNatureMIS.Text = Convert.ToString(dtData.Rows[0]["NATURE"]);
                pnlNatureMIS.Visible = true;
            }

            objCommonFunction.ddlSetDataValue(ddlBankName, Convert.ToString(dtData.Rows[0]["BANKNAME"]));
            txtDealingOfficerRemarks.Text = Convert.ToString(dtData.Rows[0]["DESK_USER_REMARKS"]);
            txtLetterSentDate.Text = Convert.ToString(dtData.Rows[0]["LETTERSENTDATE"]);
            txtReminderDate.Text = Convert.ToString(dtData.Rows[0]["REMINDERDATE"]);
            txtReplyReceivedDate.Text = Convert.ToString(dtData.Rows[0]["REPLYRECEIVEDDATE"]);
            objCommonFunction.ddlSetDataValue(ddlLetterSentTo, Convert.ToString(dtData.Rows[0]["LETTERSENTTO"]));
            hidLetterSentTo.Value = Convert.ToString(dtData.Rows[0]["LETTERSENTTO"]);
            txtPFNumber.Text = Convert.ToString(dtData.Rows[0]["PFNO"]);

            objCommonFunction.ddlSetDataValue(ddlZoneNew, Convert.ToString(dtData.Rows[0]["NEWZONE"]));
            string ZONE = Convert.ToString(dtData.Rows[0]["NEWZONE"]);
            if (!string.IsNullOrEmpty(ZONE))
            {
                objCommonFunction.funcZoneCircleMaster(ddlCircleNew, ZONE);
                objCommonFunction.ddlSetDataValue(ddlCircleNew, Convert.ToString(dtData.Rows[0]["NEWCIRCLE"]));
            }

            objCommonFunction.ddlSetDataValue(ddlZoneType, Convert.ToString(dtData.Rows[0]["ZONE_TYPE"]));
            txtZOCM.Text = Convert.ToString(dtData.Rows[0]["ZONE_CM"]);
            lblMsg.Text = "";
        }

        public void funcClear()
        {
            txtRNo.ToolTip = String.Empty;
            txtRNo.Text = String.Empty;
            txtCompRecDate.Text = String.Empty;
            txtBRComplaint.Text = String.Empty;
            ddlCircleOffice.SelectedIndex = 0;
            txtCompNo.Text = String.Empty;
            txtClosureDate.Text = String.Empty;
            txtAccused.Text = String.Empty;
            txtAllegations.Text = String.Empty;
            txtType.Text = String.Empty;
            txtNPADate.Text = String.Empty;
            txtFinalAction.Text = String.Empty;
            ddlZone.SelectedIndex = 0;
            txtSource.Text = String.Empty;
            txtSourceDate.Text = String.Empty;
            ddlSourceRef.SelectedIndex = 0;
            txtAccountName.Text = String.Empty;
            ddlStatusCode.SelectedIndex = 0;
            lblStatusCodeMIS.Text = String.Empty;
            txtInvestigationDate.Text = String.Empty;
            txtNatureComp.Text = String.Empty;
            txtAmount.Text = String.Empty;
            txtDateForINVReport.Text = String.Empty;
            txtDesignation.Text = String.Empty;
            ddlNature.SelectedIndex = 0;
            lblNatureMIS.Text = String.Empty;
            txtClose.Text = String.Empty;
            txtRessonsForClosure.Text = String.Empty;
            txtStatus.Text = String.Empty;
            txtHOStatus.Text = String.Empty;
            chkClosureDate.Checked = false;
            lblClosureDate.Text = String.Empty;
            txtDealingOfficerRemarks.Text = "";
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            pnlNatureMIS.Visible = false;
            ddlBankName.SelectedIndex = 0;
            ddlZoneType.SelectedIndex = 0;
            txtZOCM.Text = "";

            ddlLetterSentTo.SelectedIndex = 0;
            txtLetterSentDate.Text = "";
            txtReminderDate.Text = "";
            txtReplyReceivedDate.Text = "";
            hidLetterSentTo.Value = "";

            txtPFNumber.Text = "";

            funcControlsUserRights();

            ddlZoneNew.SelectedIndex = 0;
            if (ddlCircleNew.Items.Count > 0)
            {
                ddlCircleNew.Items.Clear();
            }

            funcClearEODetails();
            gvEODetails.DataSource = null;
            gvEODetails.DataBind();
            lblMsg.Text = "";
            txtZOCM.Text = "";
            ddlZoneType.SelectedIndex = 0;
        }

        public void funcControlsUserRights()
        {
            if (Convert.ToString(Session["ROLE"]).ToUpper().Equals("VMIS_VIEWUSER"))
            {
                objCommonFunction.DisableAllControls(this.Page);
                txtRNo_LIST.Enabled = true;
                txtBranch_LIST.Enabled = true;
                txtStatus_LIST.Enabled = true;
                txtCircle_LIST.Enabled = true;
                txtSource_LIST.Enabled = true;
                txtSourceRef_LIST.Enabled = true;
                txtCompNo_LIST.Enabled = true;
                txtAccountName_LIST.Enabled = true;

                btnSubmit.Visible = false;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;
                btnSearch_List.Enabled = true;
            }
            else if (Convert.ToString(Session["ROLE"]).ToUpper().Equals("VMIS_DESKUSER"))
            {
                objCommonFunction.DisableAllControls(this.Page);
                txtRNo_LIST.Enabled = true;
                txtBranch_LIST.Enabled = true;
                txtStatus_LIST.Enabled = true;
                txtCircle_LIST.Enabled = true;
                txtSource_LIST.Enabled = true;
                txtSourceRef_LIST.Enabled = true;
                txtCompNo_LIST.Enabled = true;
                txtAccountName_LIST.Enabled = true;

                pnlHOStatus.Visible = true;
                txtHOStatus.Enabled = true;
                txtDealingOfficerRemarks.Enabled = true;
                btnUpdate.Visible = true;
                btnUpdate.Enabled = true;
                btnSearch_List.Enabled = true;

                foreach (GridViewRow row in gvMain.Rows)
                {
                    Button btnView = ((Button)row.FindControl("btnView")) as Button;
                    btnView.Enabled = true;
                }

                btnGet.Enabled = true;
            }
        }

        private bool funcAddEO()
        {
            SqlConnection conSave = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();
            string MODE = string.Empty;
            Boolean Result = false;
            try
            {
                if (string.IsNullOrEmpty(btnAddEO.ToolTip))
                {
                    string ID = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                    EOUNIQUEID = "EO" + DateTime.Now.ToString("ddMMyy") + ID;
                    MODE = "I";
                }
                else
                {
                    EOUNIQUEID = btnAddEO.ToolTip;
                    MODE = "U";
                }
                string TYPE = objCommonFunction.ddlSelectedValue(ddlType_D);

                string strEORetirementDate = txtRetirementDate_D.Text.Trim();
                if (!string.IsNullOrEmpty(strEORetirementDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strEORetirementDate, out date))
                        dtEODOR = date;
                }

                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spMiscEO_Add]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_EOUNIQUEID", EOUNIQUEID);
                cmdSave.Parameters.AddWithValue("@p_UNIQUEID", txtRNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_TYPE", TYPE);
                cmdSave.Parameters.AddWithValue("@p_PFNUMBER", txtPFNumber_D.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NAME", txtName_D.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", txtDesignation_D.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DOR", dtEODOR);
                cmdSave.Parameters.AddWithValue("@p_DEALTHWITH", objCommonFunction.ddlSelectedValue(ddlDealtWith_D));
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["USERID"]));

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    Result = true;
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
                }
                else
                {
                    lblMsg.Text = "Error in Insert/ Update MISC Details.";
                }

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

            return Result;
        }

        private void funcClearEODetails()
        {
            ddlType_D.SelectedIndex = 0;
            txtPFNumber_D.Text = "";
            txtName_D.Text = "";
            txtDesignation_D.Text = "";
            txtRetirementDate_D.Text = "";
            ddlDealtWith_D.SelectedIndex = 0;
            btnAddEO.Text = "Add";
            btnAddEO.ToolTip = "";
        }

        private void funcShowEODetails(string UniqueID)
        {
            DataTable dt = new DataTable();
            SqlConnection conView = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdView = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmdView);

            try
            {
                conView.Open();
                cmdView.Connection = conView;
                cmdView.Parameters.Clear();
                cmdView.CommandType = CommandType.StoredProcedure;
                cmdView.CommandText = "[dbo].[spMiscEO_View]";

                cmdView.Parameters.AddWithValue("@p_UNIQUEID", UniqueID);

                cmdView.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    gvEODetails.DataSource = dt;
                    gvEODetails.DataBind();
                }
                else
                {
                    gvEODetails.DataSource = null;
                    gvEODetails.DataBind();
                }
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

        private void funcDeleteEOODetails(string EOUniqueID, string UniqueID)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();

            try
            {
                con.Open();
                cmdSave.Connection = con;
                cmdSave.Parameters.Clear();
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spMiscEO_Delete]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_EO_UNIQUEID", EOUniqueID);
                cmdSave.Parameters.AddWithValue("@p_UNIQUEID", UniqueID);
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["USERID"]));

                cmdSave.CommandTimeout = 0;

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
                    funcShowEODetails(UniqueID); //Update grid data.
                }
                else
                {
                    lblMsg.Text = "Error in deleting: MISC Details";
                }
            }
            catch (Exception es)
            {
                lblMsg.Text = es.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }
            finally
            {
                cmdSave.Dispose();
                con.Close();
                con.Dispose();
            }
        }

        protected void ddlZoneNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ZONE = objCommonFunction.ddlSelectedValue(ddlZoneNew);

            if (!string.IsNullOrEmpty(ZONE))
            {
                ddlZoneType.SelectedIndex = 0;
                txtZOCM.Text = "";
                objCommonFunction.funcZoneCircleMaster(ddlCircleNew, ZONE);
            }
            else
            {
                ddlZoneType.SelectedIndex = 0;
                txtZOCM.Text = "";
                ddlCircleNew.Items.Clear();
            }
        }

        protected void ddlZoneType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ZONETYPE = objCommonFunction.ddlSelectedValue(ddlZoneType);

            if (!string.IsNullOrEmpty(ZONETYPE))
            {
                objMasterData.funcZoneTypeCM(txtZOCM, objCommonFunction.ddlSelectedValue(ddlZoneNew), ZONETYPE);
            }
            else
            {
                txtZOCM.Text = "";
            }
        }

        private Boolean funcValidation(string MODE)
        {
            Boolean Result = true;
            lblMsg.Text = "";

            if (string.IsNullOrEmpty(txtRNo.Text.Trim()))
            {
                lblMsg.Text = "Please enter R Number...!";
                return Result = false;
            }

            if (string.IsNullOrEmpty(txtCompRecDate.Text.Trim()))
            {
                lblMsg.Text = "Please enter Complaint Recv Date...!";
                return Result = false;
            }

            //if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlCircleOffice)))
            //{
            //    lblMsg.Text = "Please select circle from dropdown";
            //    return Result = false;
            //}

            if (string.IsNullOrEmpty(txtBRComplaint.Text.Trim()))
            {
                lblMsg.Text = "Please enter Branch Complaint...!";
                return Result = false;
            }

            //if (string.IsNullOrEmpty(txtCompNo.Text.Trim()))
            //{
            //    lblMsg.Text = "Please enter Comp No...!";
            //    return Result = false;
            //}

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlZoneNew)))
            {
                lblMsg.Text = "Please select new zone from dropdown";
                return Result = false;
            }

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlZoneType)))
            {
                lblMsg.Text = "Please select type from dropdown";
                return Result = false;
            }

            if (string.IsNullOrEmpty(txtZOCM.Text.Trim()))
            {
                lblMsg.Text = "Please enter Chief Manager Name...!";
                return Result = false;
            }

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlCircleNew)))
            {
                lblMsg.Text = "Please select new circle from dropdown";
                return Result = false;
            }

            if (string.IsNullOrEmpty(txtStatus.Text.Trim()))
            {
                lblMsg.Text = "Please enter status...!";
                return Result = false;
            }

            return Result;
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "VIEW")
                {
                    string strRNO = Convert.ToString(e.CommandArgument);
                    if (!string.IsNullOrEmpty(strRNO))
                    {
                        funcShow(strRNO, "VIEW", null, null, null, null, null, null, null);
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
                funcShow(null, "LIST", null, null, null, null, null, null, null); //for bind grid view on List Tab Load
            }
        }

        protected void gvEODetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            btnAddEO.Text = "Add";

            if (e.CommandName.ToUpper() == "DELETE")
            {
                string Value = e.CommandArgument.ToString();

                if (!string.IsNullOrEmpty(Value))
                {
                    string[] Data = Value.Split('~');
                    string EOUNIQUEID = Data[0];
                    string UNIQUEID = Data[1];

                    funcDeleteEOODetails(EOUNIQUEID, UNIQUEID);
                }
                else
                {
                    lblMsg.Text = "Unique ID is null...";
                }
            }

            if (e.CommandName.ToUpper() == "VIEW")
            {
                string Value = e.CommandArgument.ToString();
                btnAddEO.Text = "Update";

                if (!string.IsNullOrEmpty(Value))
                {
                    string[] Data = Value.Split('~');

                    //Bind Control case of Update details
                    btnAddEO.ToolTip = Data[0];
                    objCommonFunction.ddlSetDataValue(ddlType_D, Data[1]);
                    txtPFNumber_D.Text = Data[2];
                    txtName_D.Text = Data[3];
                    txtDesignation_D.Text = Data[4];
                    txtRetirementDate_D.Text = Data[5];
                    objCommonFunction.ddlSetDataValue(ddlDealtWith_D, Data[6]);
                }
                else
                {
                    btnAddEO.ToolTip = "";
                    ddlType_D.SelectedIndex = 0;
                    txtPFNumber_D.Text = "";
                    txtName_D.Text = "";
                    txtDesignation_D.Text = "";
                    txtRetirementDate_D.Text = "";
                    ddlDealtWith_D.SelectedIndex = 0;
                    lblMsg.Text = "Unique ID is null...";
                }
            }
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            funcShow(txtRNo.Text.Trim(), "GET", null, null, null, null, null, null, null);
        }

        protected void btnAddEO_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlType_D)))
            {
                lblMsg.Text = "Please select type from dropdown";
                return;
            }
            else if (string.IsNullOrEmpty(txtPFNumber_D.Text))
            {
                lblMsg.Text = "Please enter PF Number";
                return;
            }
            else if (string.IsNullOrEmpty(txtName_D.Text))
            {
                lblMsg.Text = "Please enter name";
                return;
            }
            else if (string.IsNullOrEmpty(txtDesignation_D.Text))
            {
                lblMsg.Text = "Please enter Designation";
                return;
            }
            else if (string.IsNullOrEmpty(txtRetirementDate_D.Text))
            {
                lblMsg.Text = "Please select Retirement Date";
                return;
            }
            else
            {
                if (funcAddEO() == true)
                {
                    funcClearEODetails();
                    funcShowEODetails(txtRNo.Text.Trim());
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (funcValidation("I") == true)
            {
                funcSave("I");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (funcValidation("U") == true)
            {
                funcSave("U");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            funcClear();
        }

        protected void btnSearch_List_Click(object sender, EventArgs e)
        {
            lblList.Text = "";
            string VIEW = "SEARCH";
            if (txtRNo_LIST.Text.Trim() == "" && txtBranch_LIST.Text.Trim() == "" && txtStatus_LIST.Text.Trim() == "" && txtCircle_LIST.Text.Trim() == "" && txtSource.Text.Trim() == "" && txtSourceRef_LIST.Text.Trim() == "" && txtCompNo_LIST.Text.Trim() == "" && txtAccountName_LIST.Text.Trim() == "")
            {
                VIEW = "LIST";
            }

            funcShow(txtRNo_LIST.Text.Trim(), VIEW, txtBranch_LIST.Text.Trim(), txtStatus_LIST.Text.Trim(), txtCircle_LIST.Text.Trim(), txtSource.Text.Trim(), txtSourceRef_LIST.Text.Trim(), txtCompNo_LIST.Text.Trim(), txtAccountName_LIST.Text.Trim());
        }
    }
}