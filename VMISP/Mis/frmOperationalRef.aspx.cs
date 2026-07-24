using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class frmOperationalRef : System.Web.UI.Page
    {
        DateTime? dtRECDATECOMP = null;
        DateTime? dtSOURCEDATE = null;
        DateTime? dtSENTFORINVDATE = null;
        DateTime? dtDTIAC = null;
        DateTime? dtDTOFINVREPORT = null;
        DateTime? dtCLOSUREDT = null;
        DateTime? dtRYSENT = null;
        DateTime? dtLETTERSENTDATE = null;
        DateTime? dtREMINDERDATE = null;
        DateTime? dtREPLYRECEIVEDDATE = null;
        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                funcShow(null, "LIST", null, null, null, null, null, null, null); //for bind grid view on form Load
                funcbindDropdown();     //Bind All DropDown Lists
            }

            funcControlsUserRights();

            #region ** JS Function  **
            btnSubmit.Attributes.Add("onclick", "return funcValidation_OperationalRef('" + txtRNo.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            btnUpdate.Attributes.Add("onclick", "return funcValidation_OperationalRef('" + txtRNo.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            txtAmount.Attributes.Add("onkeypress", "return isNumbericDecimal(event,'" + txtAmount.ClientID + "')");

            txtCompRecDate.Attributes.Add("readonly", "readonly");
            txtClosureDate.Attributes.Add("readonly", "readonly");
            txtIACDate.Attributes.Add("readonly", "readonly");
            txtSourceDate.Attributes.Add("readonly", "readonly");
            txtSentForInvDate.Attributes.Add("readonly", "readonly");
            txtDateForINVReport.Attributes.Add("readonly", "readonly");
            txtRYSent.Attributes.Add("readonly", "readonly");
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
                cmd.CommandText = "[dbo].[spOperationalRef_Ddl]";
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

                string strRECDATECOMP = txtCompRecDate.Text.Trim();
                if (!string.IsNullOrEmpty(strRECDATECOMP))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRECDATECOMP, out date))
                        dtRECDATECOMP = date;
                }

                string strDTIAC = txtIACDate.Text.Trim();
                if (!string.IsNullOrEmpty(strDTIAC))
                {
                    DateTime date;
                    if (DateTime.TryParse(strDTIAC, out date))
                        dtDTIAC = date;
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

                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spOperationalRef_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_CODE", objCommonFunction.convertToIntToolTip(txtRNo));
                cmdSave.Parameters.AddWithValue("@p_RNO", txtRNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_COMPNO", txtCompNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ACCUSED", txtAccused.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", txtDesignation.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PRESENTPOSTING", txtPresentPosting.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BRCOMPLAINT", txtBRComplaint.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ZONE", objCommonFunction.ddlSelectedText(ddlZone));
                cmdSave.Parameters.AddWithValue("@p_CIRCLEOFFICE", objCommonFunction.ddlSelectedText(ddlCircleOffice));
                cmdSave.Parameters.AddWithValue("@p_REGION", txtRegion.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_RECDATECOMP", dtRECDATECOMP);
                cmdSave.Parameters.AddWithValue("@p_SOURCE", txtSource.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_SOURCEREF", objCommonFunction.ddlSelectedText(ddlSourceRef));
                cmdSave.Parameters.AddWithValue("@p_SOURCEDATE", dtSOURCEDATE);
                cmdSave.Parameters.AddWithValue("@p_SENTTO", txtSentTo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_SENTFORINVDATE", dtSENTFORINVDATE);
                cmdSave.Parameters.AddWithValue("@p_ACCOUNTNAME", txtAccountName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_AMOUNT", objCommonFunction.convertToDecimal(txtAmount));
                cmdSave.Parameters.AddWithValue("@p_ALLEGATIONS", txtAllegations.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DTIAC", dtDTIAC);
                cmdSave.Parameters.AddWithValue("@p_STATUS", txtStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_HOSTATUS", txtHOStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_STATUSCODE", objCommonFunction.ddlSelectedValue(ddlStatusCode));
                cmdSave.Parameters.AddWithValue("@p_NATURE", objCommonFunction.ddlSelectedValue(ddlNature));
                cmdSave.Parameters.AddWithValue("@p_DTOFINVREPORT", dtDTOFINVREPORT);
                cmdSave.Parameters.AddWithValue("@p_CASENO", txtCaseNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CASECLOSE", txtClose.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CLOSUREDT", dtCLOSUREDT);
                cmdSave.Parameters.AddWithValue("@p_RYSENT", dtRYSENT);
                cmdSave.Parameters.AddWithValue("@p_REASONSFORCLOSURE", txtRessonsForClosure.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CLOSURE", strCLOSURE);
                cmdSave.Parameters.AddWithValue("@p_BANKNAME", objCommonFunction.ddlSelectedValue(ddlBankName));
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTO", objCommonFunction.ddlSelectedValue(ddlLetterSentTo));
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTDATE", dtLETTERSENTDATE);
                cmdSave.Parameters.AddWithValue("@p_REMINDERDATE", dtREMINDERDATE);
                cmdSave.Parameters.AddWithValue("@p_REPLYRECEIVEDDATE", dtREPLYRECEIVEDDATE);
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                cmdSave.Parameters.AddWithValue("@p_DESK_USER_REMARKS", txtDealingOfficerRemarks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ZONENEW", objCommonFunction.ddlSelectedValue(ddlZoneNew));
                cmdSave.Parameters.AddWithValue("@p_CIRCLENEW", objCommonFunction.ddlSelectedValue(ddlCircleNew));
                cmdSave.Parameters.AddWithValue("@p_PFNO", txtPFNumber.Text.Trim());
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

        public void funcShow(string p_strNo, string p_strView, string p_strACCOUNTNAME, string p_strSTATUS, string p_strSOURCE, string p_strSOURCEREF, string p_strALLEGATIONS, string p_strBRANCH, string p_strCIRCLE)
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
                cmdView.CommandText = "[dbo].[spOperationalRef_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_VIEW", p_strView);
                cmdView.Parameters.AddWithValue("@p_ACCOUNTNAME", p_strACCOUNTNAME);
                cmdView.Parameters.AddWithValue("@p_STATUS", p_strSTATUS);
                cmdView.Parameters.AddWithValue("@p_SOURCE", p_strSOURCE);
                cmdView.Parameters.AddWithValue("@p_SOURCEREF", p_strSOURCEREF);
                cmdView.Parameters.AddWithValue("@p_ALLEGATIONS", p_strALLEGATIONS);
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
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
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
            txtCompNo.Text = Convert.ToString(dtData.Rows[0]["COMPNO"]);
            objCommonFunction.chkSetData(chkClosureDate, Convert.ToString(dtData.Rows[0]["CLOSURE"]));
            lblClosureDate.Text = Convert.ToString(dtData.Rows[0]["CLOSUREDATE"]);
            txtAccused.Text = Convert.ToString(dtData.Rows[0]["ACCUSED"]);
            txtAllegations.Text = Convert.ToString(dtData.Rows[0]["ALLEGATIONS"]);
            txtCaseNo.Text = Convert.ToString(dtData.Rows[0]["CASENO"]);
            txtIACDate.Text = Convert.ToString(dtData.Rows[0]["IACDATE"]);
            txtPresentPosting.Text = Convert.ToString(dtData.Rows[0]["PRESENTPOSTING"]);
            objCommonFunction.ddlSetData(ddlZone, Convert.ToString(dtData.Rows[0]["ZONE"]), true);
            txtSource.Text = Convert.ToString(dtData.Rows[0]["SOURCE"]);
            txtSourceDate.Text = Convert.ToString(dtData.Rows[0]["SOURCEDATE"]);
            objCommonFunction.ddlSetData(ddlSourceRef, Convert.ToString(dtData.Rows[0]["SOURCEREF"]), true);
            txtAccountName.Text = Convert.ToString(dtData.Rows[0]["ACCOUNTNAME"]);
            txtSentForInvDate.Text = Convert.ToString(dtData.Rows[0]["SENTFORINVDATE"]);
            txtSentTo.Text = Convert.ToString(dtData.Rows[0]["SENTTO"]);
            txtRegion.Text = Convert.ToString(dtData.Rows[0]["REGION"]);
            txtAmount.Text = Convert.ToString(dtData.Rows[0]["AMOUNT"]);
            txtDateForINVReport.Text = Convert.ToString(dtData.Rows[0]["INVREPORTDATE"]);
            txtDesignation.Text = Convert.ToString(dtData.Rows[0]["DESIGNATION"]);
            txtClose.Text = Convert.ToString(dtData.Rows[0]["CASECLOSE"]);
            txtRYSent.Text = Convert.ToString(dtData.Rows[0]["RYSENTDATE"]);
            txtRessonsForClosure.Text = Convert.ToString(dtData.Rows[0]["REASONSFORCLOSURE"]);
            txtStatus.Text = Convert.ToString(dtData.Rows[0]["STATUS"]);

            objCommonFunction.ddlSetDataValue(ddlStatusCode, Convert.ToString(dtData.Rows[0]["STATUSCODE"]));
            if (objCommonFunction.ddlSelectedValue(ddlStatusCode) == "0" && Convert.ToString(dtData.Rows[0]["STATUSCODE"]) != "0")
            {
                lblStatusCodeMIS.Text = Convert.ToString(dtData.Rows[0]["STATUSCODE"]);
            }

            objCommonFunction.ddlSetDataValue(ddlNature, Convert.ToString(dtData.Rows[0]["NATURE"]));
            if (objCommonFunction.ddlSelectedValue(ddlNature) == "0")
            {
                lblNatureMIS.Text = Convert.ToString(dtData.Rows[0]["NATURE"]);
                pnlNatureMIS.Visible = true;
            }
            txtDealingOfficerRemarks.Text = Convert.ToString(dtData.Rows[0]["DESK_USER_REMARKS"]);
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

            txtPFNumber.Text = Convert.ToString(dtData.Rows[0]["PFNO"]);
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
            txtIACDate.Text = string.Empty;
            txtPresentPosting.Text = string.Empty;
            ddlZone.SelectedIndex = 0;
            txtSource.Text = string.Empty;
            txtSourceDate.Text = string.Empty;
            ddlSourceRef.SelectedIndex = 0;
            txtAccountName.Text = string.Empty;
            ddlStatusCode.SelectedIndex = 0;
            lblStatusCodeMIS.Text = string.Empty;
            txtSentForInvDate.Text = string.Empty;
            txtSentTo.Text = string.Empty;
            txtRegion.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtDateForINVReport.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            ddlNature.SelectedIndex = 0;
            lblNatureMIS.Text = string.Empty;
            txtClose.Text = string.Empty;
            txtRYSent.Text = string.Empty;
            txtRessonsForClosure.Text = string.Empty;
            txtStatus.Text = string.Empty;
            txtHOStatus.Text = string.Empty;
            chkClosureDate.Checked = false;
            lblClosureDate.Text = string.Empty;
            ddlBankName.SelectedIndex = 0;
            txtDealingOfficerRemarks.Text = "";
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            pnlNatureMIS.Visible = false;
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
        }

        public void funcControlsUserRights()
        {
            if (Convert.ToString(Session["role"]).ToUpper() == "VMIS_VIEWUSER")
            {
                objCommonFunction.DisableAllControls(this.Page);
                btnSubmit.Visible = false;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;
                txtRNo_LIST.Enabled = true;
                txtAccountName_LIST.Enabled = true;
                txtStatus_LIST.Enabled = true;
                txtSource_LIST.Enabled = true;
                txtSourceRef_LIST.Enabled = true;
                txtAllegations_LIST.Enabled = true;
                txtBranch_LIST.Enabled = true;
                txtCircle_LIST.Enabled = true;
            }
            else if (Convert.ToString(Session["role"]).ToUpper() == "VMIS_DESKUSER")
            {
                objCommonFunction.DisableAllControls(this.Page);
                pnlHOStatus.Visible = true;
                txtHOStatus.Enabled = true;
                txtDealingOfficerRemarks.Enabled = true;
                btnSubmit.Visible = false;
                btnUpdate.Visible = true;
                btnUpdate.Enabled = true;
                btnCancel.Visible = false;

                txtRNo_LIST.Enabled = true;
                txtAccountName_LIST.Enabled = true;
                txtStatus_LIST.Enabled = true;
                txtSource_LIST.Enabled = true;
                txtSourceRef_LIST.Enabled = true;
                txtAllegations_LIST.Enabled = true;
                txtBranch_LIST.Enabled = true;
                txtCircle_LIST.Enabled = true;

                btnSearch_List.Enabled = true;

                foreach (GridViewRow row in gvMain.Rows)
                {
                    Button btnView = ((Button)row.FindControl("btnView")) as Button;
                    btnView.Enabled = true;
                }

                btnGet.Enabled = true;
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
            if (string.IsNullOrEmpty(txtRNo.Text.Trim()))
            {
                lblMsg.Text = "Please Enter Operational R Number.";
                return;
            }

            funcShow(txtRNo.Text.Trim(), "GET", null, null, null, null, null, null, null);
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "VIEW")
                {
                    if (Convert.ToString(e.CommandArgument) != "")
                    {
                        funcShow(Convert.ToString(e.CommandArgument), "VIEW", null, null, null, null, null, null, null);
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
                funcShow("LIST", "LIST", null, null, null, null, null, null, null); //for bind grid view on List Tab Load
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
            string strSearchNo = txtRNo_LIST.Text.Trim();
            string strACCOUNTNAME = txtAccountName_LIST.Text.Trim();
            string strSTATUS = txtStatus_LIST.Text.Trim();
            string strSOURCE = txtSource_LIST.Text.Trim();
            string strSOURCEREF = txtSourceRef_LIST.Text.Trim();
            string strALLEGATIONS = txtAllegations_LIST.Text.Trim();
            string strBRCOMPLAINT = txtBranch_LIST.Text.Trim();
            string strCIRCLEOFFICE = txtCircle_LIST.Text.Trim();
            string strVIEW = "SEARCH";

            if (strSearchNo == "" && strACCOUNTNAME == "" && strSTATUS == "" && strSOURCE == "" && strSOURCEREF == "" && strALLEGATIONS == "" && strBRCOMPLAINT == "" && strCIRCLEOFFICE == "")
            {
                strVIEW = "LIST";
            }
            funcShow(strSearchNo, strVIEW, strACCOUNTNAME, strSTATUS, strSOURCE, strSOURCEREF, strALLEGATIONS, strBRCOMPLAINT, strCIRCLEOFFICE);
        }
    }
}