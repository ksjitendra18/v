using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class frmRTI : System.Web.UI.Page
    {
        DateTime? dtRTIRECDATE = null;
        DateTime? dtSOURCEDATE = null;
        DateTime? dtSENTFORINVDATE = null;
        DateTime? dtDTIAC = null;
        DateTime? dtDTOFINVREPORT = null;
        DateTime? dtRYSENT = null;
        DateTime? dtCLOSUREDT = null;
        DateTime? dtLETTERSENTDATE = null;
        DateTime? dtREMINDERDATE = null;
        DateTime? dtREPLYRECEIVEDDATE = null;
        CommonFunction objCommonFunction = new CommonFunction();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                funcShow(null, "LIST", null, null, null); //for bind grid view on form Load
                funcbindDropdown();     //Bind All DropDown Lists
            }

            lblMsg.Text = string.Empty;
            funcControlsUserRights();

            #region ** JS Function  **
            txtAmount.Attributes.Add("onkeypress", "return isNumbericDecimal(event,'" + txtAmount.ClientID + "')");
            txtCatANo.Attributes.Add("onkeypress", "return IsNumeric(event,'" + txtAmount.ClientID + "')");
            txtCatBNo.Attributes.Add("onkeypress", "return IsNumeric(event,'" + txtAmount.ClientID + "')");
            txtASNo.Attributes.Add("onkeypress", "return IsNumeric(event,'" + txtAmount.ClientID + "')");

            txtRTIRecDate.Attributes.Add("readonly", "readonly");
            txtSourceDate.Attributes.Add("readonly", "readonly");
            txtSentForInvDate.Attributes.Add("readonly", "readonly");
            txtIACDate.Attributes.Add("readonly", "readonly");
            txtDateForINVReport.Attributes.Add("readonly", "readonly");
            txtRYSent.Attributes.Add("readonly", "readonly");
            txtClosureDate.Attributes.Add("readonly", "readonly");
            txtLetterSentDate.Attributes.Add("readonly", "readonly");
            txtReminderDate.Attributes.Add("readonly", "readonly");
            txtReplyReceivedDate.Attributes.Add("readonly", "readonly");

            btnSubmit.Attributes.Add("onclick", "return funcValidation_RTI('" + txtRTINo.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            btnUpdate.Attributes.Add("onclick", "return funcValidation_RTI('" + txtRTINo.ClientID + "','" + ddlCircleOffice.ClientID + "')");

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
                cmd.CommandText = "[dbo].[spRTI_Ddl]";
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

        public void funcBindControl(DataTable dtData)
        {
            tabMain.ActiveTabIndex = 0;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;

            //BIND TEXT BOX CONTROL
            txtRTINo.ToolTip = Convert.ToString(dtData.Rows[0]["CODE"]);
            txtRTINo.Text = Convert.ToString(dtData.Rows[0]["RTINO"]);
            txtBRComplaint.Text = Convert.ToString(dtData.Rows[0]["BRCOMPLAINT"]);
            txtSource.Text = Convert.ToString(dtData.Rows[0]["SOURCE"]);
            txtAccountName.Text = Convert.ToString(dtData.Rows[0]["ACCOUNTNAME"]);
            txtAllegations.Text = Convert.ToString(dtData.Rows[0]["ALLEGATIONS"]);
            txtAmount.Text = Convert.ToString(dtData.Rows[0]["AMOUNT"]);
            txtAccused.Text = Convert.ToString(dtData.Rows[0]["ACCUSED"]);
            txtDesignation.Text = Convert.ToString(dtData.Rows[0]["DESIGNATION"]);
            txtPresentPosting.Text = Convert.ToString(dtData.Rows[0]["PRESENTPOSTING"]);
            txtSentTo.Text = Convert.ToString(dtData.Rows[0]["SENTTO"]);
            txtCatANo.Text = Convert.ToString(dtData.Rows[0]["CATANO"]);
            txtCatBNo.Text = Convert.ToString(dtData.Rows[0]["CATBNO"]);
            txtASNo.Text = Convert.ToString(dtData.Rows[0]["ASNO"]);
            txtPendingWith.Text = Convert.ToString(dtData.Rows[0]["PENDINGWITH"]);
            txtNameINVOfficial.Text = Convert.ToString(dtData.Rows[0]["NAMEINVOFFICIAL"]);
            txtClose.Text = Convert.ToString(dtData.Rows[0]["CASECLOSE"]);
            txtRNO.Text = Convert.ToString(dtData.Rows[0]["RNO"]);
            txtAPlan.Text = Convert.ToString(dtData.Rows[0]["APLAN"]);
            txtRegister.Text = Convert.ToString(dtData.Rows[0]["REGISTER"]);
            txtReasonsForClosure.Text = Convert.ToString(dtData.Rows[0]["REASONSFORCLOSURE"]);
            txtStatus.Text = Convert.ToString(dtData.Rows[0]["STATUS"]);

            //BIND DATE CONTROLS
            txtRTIRecDate.Text = Convert.ToString(dtData.Rows[0]["RECRTIDATE"]);
            txtSourceDate.Text = Convert.ToString(dtData.Rows[0]["SOURCEDATE"]);
            txtIACDate.Text = Convert.ToString(dtData.Rows[0]["IACDATE"]);
            txtSentForInvDate.Text = Convert.ToString(dtData.Rows[0]["SENTFORINVDATE"]);
            txtDateForINVReport.Text = Convert.ToString(dtData.Rows[0]["INVREPORTDATE"]);
            txtRYSent.Text = Convert.ToString(dtData.Rows[0]["RYSENTDATE"]);
            objCommonFunction.chkSetData(chkClosureDate, Convert.ToString(dtData.Rows[0]["CLOSURE"]));
            lblClosureDate.Text = Convert.ToString(dtData.Rows[0]["CLOSUREDATE"]);

            //BIND DROP DOWN CONTROLS
            objCommonFunction.ddlSetData(ddlSourceRef, Convert.ToString(dtData.Rows[0]["SOURCEREF"]), true);
            objCommonFunction.ddlSetData(ddlZone, Convert.ToString(dtData.Rows[0]["ZONE"]), true);
            objCommonFunction.ddlSetData(ddlCircleOffice, Convert.ToString(dtData.Rows[0]["CIRCLEOFFICE"]), true);
            objCommonFunction.ddlSetData(ddlNature, Convert.ToString(dtData.Rows[0]["NATURE"]), true);
            objCommonFunction.ddlSetDataValue(ddlStatusCode, Convert.ToString(dtData.Rows[0]["STATUSCODE"]));
            if (objCommonFunction.ddlSelectedValue(ddlStatusCode) == "0" && Convert.ToString(dtData.Rows[0]["STATUSCODE"]) != "0")
            {
                lblStatusCodeMIS.Text = Convert.ToString(dtData.Rows[0]["STATUSCODE"]);
            }
            objCommonFunction.ddlSetDataValue(ddlBankName, Convert.ToString(dtData.Rows[0]["BANKNAME"]));

            //BIND LABEL CONTROLS
            txtDealingOfficerRemarks.Text = Convert.ToString(dtData.Rows[0]["DESK_USER_REMARKS"]);
            txtLetterSentDate.Text = Convert.ToString(dtData.Rows[0]["LETTERSENTDATE"]);
            txtReminderDate.Text = Convert.ToString(dtData.Rows[0]["REMINDERDATE"]);
            txtReplyReceivedDate.Text = Convert.ToString(dtData.Rows[0]["REPLYRECEIVEDDATE"]);
            objCommonFunction.ddlSetDataValue(ddlLetterSentTo, Convert.ToString(dtData.Rows[0]["LETTERSENTTO"]));

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
            txtRTINo.ToolTip = string.Empty;
            txtRTINo.Text = string.Empty;
            txtRTIRecDate.Text = string.Empty;
            txtSourceDate.Text = string.Empty;
            txtBRComplaint.Text = string.Empty;
            ddlCircleOffice.SelectedIndex = 0;
            txtSource.Text = string.Empty;
            txtAccountName.Text = string.Empty;
            txtAllegations.Text = string.Empty;
            txtStatus.Text = string.Empty;
            ddlStatusCode.SelectedIndex = 0;
            txtClosureDate.Text = string.Empty;
            ddlSourceRef.SelectedIndex = 0;
            txtAmount.Text = string.Empty;
            txtAccused.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            txtPresentPosting.Text = string.Empty;
            ddlZone.SelectedIndex = 0;
            txtSentTo.Text = string.Empty;
            txtSentForInvDate.Text = string.Empty;
            txtCatANo.Text = string.Empty;
            txtCatBNo.Text = string.Empty;
            txtASNo.Text = string.Empty;
            txtIACDate.Text = string.Empty;
            txtPendingWith.Text = string.Empty;
            txtNameINVOfficial.Text = string.Empty;
            txtDateForINVReport.Text = string.Empty;
            txtClose.Text = string.Empty;
            txtRNO.Text = string.Empty;
            txtRYSent.Text = string.Empty;
            txtAPlan.Text = string.Empty;
            txtRegister.Text = string.Empty;
            ddlNature.SelectedIndex = 0;
            txtReasonsForClosure.Text = string.Empty;
            lblClosureDate.Text = string.Empty;
            chkClosureDate.Checked = false;

            btnSubmit.Visible = true;
            btnUpdate.Visible = false;

            ddlBankName.SelectedIndex = 0;

            txtDealingOfficerRemarks.Text = "";

            ddlLetterSentTo.SelectedIndex = 0;
            txtLetterSentDate.Text = "";
            txtReminderDate.Text = "";
            txtReplyReceivedDate.Text = "";


            ddlZoneNew.SelectedIndex = 0;
            if (ddlCircleNew.Items.Count > 0)
            {
                ddlCircleNew.Items.Clear();
            }

            funcControlsUserRights();
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
                    string strClosureDate = txtClosureDate.Text.Trim();
                    if (!string.IsNullOrEmpty(strClosureDate))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strClosureDate, out date))
                            dtCLOSUREDT = date;
                    }
                }

                string strRTIRECDATE = txtRTIRecDate.Text.Trim();
                if (!string.IsNullOrEmpty(strRTIRECDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRTIRECDATE, out date))
                        dtRTIRECDATE = date;
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

                string strDTIAC = txtIACDate.Text.Trim();
                if (!string.IsNullOrEmpty(strDTIAC))
                {
                    DateTime date;
                    if (DateTime.TryParse(strDTIAC, out date))
                        dtDTIAC = date;
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

                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spRTI_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_CODE", objCommonFunction.convertToIntToolTip(txtRTINo));
                cmdSave.Parameters.AddWithValue("@p_RTINO", txtRTINo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BRCOMPLAINT", txtBRComplaint.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_SOURCE", txtSource.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ACCOUNTNAME", txtAccountName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ALLEGATIONS", txtAllegations.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_AMOUNT", objCommonFunction.convertToDecimal(txtAmount));
                cmdSave.Parameters.AddWithValue("@p_ACCUSED", txtAccused.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", txtDesignation.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PRESENTPOSTING", txtPresentPosting.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_SENTTO", txtSentTo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CATANO", objCommonFunction.convertToInt(txtCatANo));
                cmdSave.Parameters.AddWithValue("@p_CATBNO", objCommonFunction.convertToInt(txtCatBNo));
                cmdSave.Parameters.AddWithValue("@p_ASNO", objCommonFunction.convertToInt(txtASNo));
                cmdSave.Parameters.AddWithValue("@p_PENDINGWITH", txtPendingWith.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NAMEOFINVOFFICIAL", txtNameINVOfficial.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CASECLOSE", txtClose.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_RNO", txtRNO.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_APLAN", txtAPlan.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_REGISTER", txtRegister.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_REASONSFORCLOSURE", txtReasonsForClosure.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_STATUS", txtStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_HOSTATUS", txtHOStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTO", objCommonFunction.ddlSelectedValue(ddlLetterSentTo));
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTDATE", dtLETTERSENTDATE);
                cmdSave.Parameters.AddWithValue("@p_REMINDERDATE", dtREMINDERDATE);
                cmdSave.Parameters.AddWithValue("@p_REPLYRECEIVEDDATE", dtREPLYRECEIVEDDATE);
                cmdSave.Parameters.AddWithValue("@p_STATUSCODE", objCommonFunction.ddlSelectedValue(ddlStatusCode));
                cmdSave.Parameters.AddWithValue("@p_ZONE", objCommonFunction.ddlSelectedText(ddlZone));
                cmdSave.Parameters.AddWithValue("@p_CIRCLEOFFICE", objCommonFunction.ddlSelectedText(ddlCircleOffice));
                cmdSave.Parameters.AddWithValue("@p_SOURCEREF", objCommonFunction.ddlSelectedText(ddlSourceRef));
                cmdSave.Parameters.AddWithValue("@p_NATURE", objCommonFunction.ddlSelectedText(ddlNature));
                cmdSave.Parameters.AddWithValue("@p_DESK_USER_REMARKS", txtDealingOfficerRemarks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BANKNAME", objCommonFunction.ddlSelectedValue(ddlBankName));
                cmdSave.Parameters.AddWithValue("@p_ZONENEW", objCommonFunction.ddlSelectedValue(ddlZoneNew));
                cmdSave.Parameters.AddWithValue("@p_CIRCLENEW", objCommonFunction.ddlSelectedValue(ddlCircleNew));
                cmdSave.Parameters.AddWithValue("@p_CLOSURE", strCLOSURE);
                cmdSave.Parameters.AddWithValue("@p_RTIRECDATE", dtRTIRECDATE);
                cmdSave.Parameters.AddWithValue("@p_SOURCEDATE", dtSOURCEDATE);
                cmdSave.Parameters.AddWithValue("@p_SENTFORINVDATE", dtSENTFORINVDATE);
                cmdSave.Parameters.AddWithValue("@p_DTIAC", dtDTIAC);
                cmdSave.Parameters.AddWithValue("@p_DTOFINVREPORT", dtDTOFINVREPORT);
                cmdSave.Parameters.AddWithValue("@p_CLOSUREDT", dtCLOSUREDT);
                cmdSave.Parameters.AddWithValue("@p_RYSENT", dtRYSENT);
                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["userid"]));
                cmdSave.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));
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

        public void funcShow(string p_strNo, string p_strView, string p_strSTATUS, string p_strBRANCH, string p_strCIRCLE)
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
                cmdView.CommandText = "[dbo].[spRTI_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_VIEW", p_strView);
                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_STATUS", p_strSTATUS);
                cmdView.Parameters.AddWithValue("@p_BRANCH", p_strBRANCH);
                cmdView.Parameters.AddWithValue("@p_CIRCLE", p_strCIRCLE);

                cmdView.CommandTimeout = 0;
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
                conView.Close();
                sda.Dispose();
                cmdView.Dispose();
                conView.Dispose();
            }
        }

        public void funcControlsUserRights()
        {
            if (Convert.ToString(ViewState["USERROLE"]).ToUpper() == "VMIS_VIEWUSER")
            {
                objCommonFunction.DisableAllControls(this.Page);
                btnSubmit.Visible = false;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;

                txtRTINo_LIST.Enabled = true;
                txtStatus_LIST.Enabled = true;
                txtBranch_LIST.Enabled = true;
                txtCircle_LIST.Enabled = true;
            }
            else if (Convert.ToString(ViewState["USERROLE"]).ToUpper() == "VMIS_DESKUSER")
            {
                objCommonFunction.DisableAllControls(this.Page);
                pnlHOStatus.Visible = true;
                txtHOStatus.Enabled = true;
                txtDealingOfficerRemarks.Enabled = true;
                btnSubmit.Visible = false;
                btnUpdate.Visible = true;
                btnUpdate.Enabled = true;
                btnCancel.Visible = false;

                txtRTINo_LIST.Enabled = true;
                txtStatus_LIST.Enabled = true;
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
            lblMsg.Text = "";
            if (string.IsNullOrEmpty(txtRTINo.Text.Trim()))
            {
                lblMsg.Text = "Please Enter Complaint Number.";
                return;
            }

            funcShow(txtRTINo.Text.Trim(), "GET", null, null, null);
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "VIEW")
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(e.CommandArgument)))
                    {
                        funcShow(Convert.ToString(e.CommandArgument), "VIEW", null, null, null);
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
                funcShow(null, "LIST", null, null, null); //for bind grid view on List Tab Load
            }

            //Code hereTabContainer
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
            string strSearchNo = txtRTINo_LIST.Text.Trim();
            string strSTATUS = txtStatus_LIST.Text.Trim();
            string strBRCOMPLAINT = txtBranch_LIST.Text.Trim();
            string strCIRCLEOFFICE = txtCircle_LIST.Text.Trim();

            string strVIEW = "SEARCH";

            if (strSearchNo == "" && strSTATUS == "" && strBRCOMPLAINT == "" && strCIRCLEOFFICE == "")
            {
                strVIEW = "LIST";
            }

            funcShow(strSearchNo, strVIEW, strSTATUS, strBRCOMPLAINT, strCIRCLEOFFICE);
        }
    }
}