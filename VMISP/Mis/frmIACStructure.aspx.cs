using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class frmIACStructure : System.Web.UI.Page
    {
        DateTime? dtRECDATECOMP = null;
        DateTime? dtCLOSUREDT = null;
        DateTime? dtRETDATE = null;
        DateTime? dtLETTERSENTDATE = null;
        DateTime? dtREMINDERDATE = null;
        DateTime? dtREPLYRECEIVEDDATE = null;
        DateTime? dtLETTERSENTTODADATE = null;

        DateTime? dtABBFFCaseSubmissionDate = null;
        DateTime? dtABBFFReplyDate = null;
        DateTime? dtABBFFAdviceReceiveDate = null;
        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                funcShow("LIST", "LIST", null, null, null, null, null, null); //for bind grid view on form Load
                funcbindDropdown();     //Bind All DropDown Lists
            }

            funcControlsUserRights();

            btnGet.Attributes.Add("onclick", "return funcSearch_Validation('" + txtIACNo.ClientID + "','" + "Please Enter IAC Number" + "')");
            btnSubmit.Attributes.Add("onclick", "return funcValidation_IAC('" + txtIACNo.ClientID + "','" + txtDAView.ClientID + "','" + txtIACView.ClientID + "','" + txtCVOView.ClientID + "','" + ddlStatusCode.ClientID + "','" + chkClosureDate.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            btnUpdate.Attributes.Add("onclick", "return funcValidation_IAC('" + txtIACNo.ClientID + "','" + txtDAView.ClientID + "','" + txtIACView.ClientID + "','" + txtCVOView.ClientID + "','" + ddlStatusCode.ClientID + "','" + chkClosureDate.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            txtAmount.Attributes.Add("onkeypress", "return isNumbericDecimal(event,'" + txtAmount.ClientID + "')");

            txtCompRecDate.Attributes.Add("readonly", "readonly");
            txtClosureDate.Attributes.Add("readonly", "readonly");
            txtRetDate.Attributes.Add("readonly", "readonly");

            txtLetterSentDate.Attributes.Add("readonly", "readonly");
            txtReminderDate.Attributes.Add("readonly", "readonly");
            txtReplyReceivedDate.Attributes.Add("readonly", "readonly");
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
                cmd.CommandText = "[dbo].[spIAC_Ddl]";
                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlCircleOffice, ds.Tables[0]);
                    objCommonFunction.bindDropdownList(ddlZone, ds.Tables[1]);
                    objCommonFunction.bindDropdownList(ddlLetterSentTo, ds.Tables[2]);
                    objCommonFunction.bindDropdownList(ddlStatusCode, ds.Tables[3]);
                    objCommonFunction.bindDropdownList(ddlNature, ds.Tables[4]);
                    objCommonFunction.bindDropdownList(ddlZoneNew, ds.Tables[5]);
                    objCommonFunction.bindDropdownList(ddlScale, ds.Tables[6]);
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

                string strRETDATE = txtRetDate.Text.Trim();
                if (!string.IsNullOrEmpty(strRETDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRETDATE, out date))
                        dtRETDATE = date;
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

                string strLetterSentToDADate = txtLetterSentToDADate.Text.Trim();
                if (!string.IsNullOrEmpty(strLetterSentToDADate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strLetterSentToDADate, out date))
                        dtLETTERSENTTODADATE = date;
                }

                string strABBFFCaseSubmissionDate = txtABBFFCaseSubmissionDate.Text.Trim();
                if (!string.IsNullOrEmpty(strABBFFCaseSubmissionDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strABBFFCaseSubmissionDate, out date))
                        dtABBFFCaseSubmissionDate = date;
                }

                string strABBFFReplyDate = txtABBFFReplyDate.Text.Trim();
                if (!string.IsNullOrEmpty(strABBFFReplyDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strABBFFReplyDate, out date))
                        dtABBFFReplyDate = date;
                }

                string strABBFFAdviceReceiveDate = txtABBFFAdviceReceiveDate.Text.Trim();
                if (!string.IsNullOrEmpty(strABBFFAdviceReceiveDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strABBFFAdviceReceiveDate, out date))
                        dtABBFFAdviceReceiveDate = date;
                }

                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spIACStructure_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_SNO", txtIACNo.ToolTip.ToString());
                cmdSave.Parameters.AddWithValue("@p_RECDATECOMP", dtRECDATECOMP);
                cmdSave.Parameters.AddWithValue("@p_BRCOMPLAINT", txtBRComplaint.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CIRCLEOFFICE", objCommonFunction.ddlSelectedText(ddlCircleOffice));
                cmdSave.Parameters.AddWithValue("@p_VIGNO", txtVIGNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CLOSUREDT", dtCLOSUREDT);
                cmdSave.Parameters.AddWithValue("@p_ACCUSED", txtAccused.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DAVIEW", txtDAView.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_MEETNO", txtMeetNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_RETDATE", dtRETDATE);
                cmdSave.Parameters.AddWithValue("@p_IACVIEW", txtIACView.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ZONE", objCommonFunction.ddlSelectedText(ddlZone));
                cmdSave.Parameters.AddWithValue("@p_SOURCE", txtSource.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DA", txtDA.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CVOVIEW", txtCVOView.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ACCOUNTNAME", txtAccountName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_AMOUNT", objCommonFunction.convertToDecimal(txtAmount));
                cmdSave.Parameters.AddWithValue("@p_IACNO1", txtIACNo1.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PFNUMBER", txtPFNumber.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_STATUSCODE", objCommonFunction.ddlSelectedValue(ddlStatusCode));
                cmdSave.Parameters.AddWithValue("@p_NATURE", objCommonFunction.ddlSelectedValue(ddlNature));
                cmdSave.Parameters.AddWithValue("@p_STATUS", txtStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_HOSTATUS", txtHOStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_IACNO", txtIACNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CLOSURE", strCLOSURE);
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTO", objCommonFunction.ddlSelectedValue(ddlLetterSentTo));
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTDATE", dtLETTERSENTDATE);
                cmdSave.Parameters.AddWithValue("@p_REMINDERDATE", dtREMINDERDATE);
                cmdSave.Parameters.AddWithValue("@p_REPLYRECEIVEDDATE", dtREPLYRECEIVEDDATE);
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTODADATE", dtLETTERSENTTODADATE);
                cmdSave.Parameters.AddWithValue("@p_DESK_USER_REMARKS", txtDealingOfficerRemarks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BANKNAME", objCommonFunction.ddlSelectedValue(ddlBankName));
                cmdSave.Parameters.AddWithValue("@p_ZONENEW", objCommonFunction.ddlSelectedValue(ddlZoneNew));
                cmdSave.Parameters.AddWithValue("@p_CIRCLENEW", objCommonFunction.ddlSelectedValue(ddlCircleNew));
                cmdSave.Parameters.AddWithValue("@p_TMSACREFNO", txtTMSACRefNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", txtDesignation.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_SCALE", objCommonFunction.ddlSelectedValue(ddlScale));

                //ABBFF Parameter
                cmdSave.Parameters.AddWithValue("@p_ABBFFCASE", objCommonFunction.ddlSelectedValue(ddlABBFFCase));
                cmdSave.Parameters.AddWithValue("@p_ABBFFREFNO", txtABBFFReferenceNumber.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ABBFFADVICEDETAILS", txtABBFFAdviceDetail.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ABBFFCASESUBMISSIONDATE", dtABBFFCaseSubmissionDate);
                cmdSave.Parameters.AddWithValue("@p_ABBFFREPLYDATE", dtABBFFReplyDate);
                cmdSave.Parameters.AddWithValue("@p_ABBFFADVICERECEIVEDATE", dtABBFFAdviceReceiveDate);

                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["userid"]));
                cmdSave.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                cmdSave.CommandTimeout = 0;

                cmdSave.ExecuteNonQuery();

                // Drive off the proc's own status code rather than the affected-row count: the
                // maker-checker guards (zone missing, record pending, record rejected) refuse
                // the save without running any DML, and their message has to reach the user.
                int errCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                string errMsg = Convert.ToString(sqlErrMsgOutput.Value);

                lblMsg.Text = string.IsNullOrEmpty(errMsg) ? "Error in IAC Insert/ Update." : errMsg;

                if (errCode == 1 || errCode == 2)
                {
                    funcClear();
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

        public void funcShow(string p_strNo, string p_strView, string p_strACCOUNTNAME, string p_strPFNUMBER, string p_strACCUSED, string p_strSTATUS, string p_strBRANCH, string p_strCIRCLE)
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
                cmdView.CommandText = "[dbo].[spIACStructure_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_VIEW", p_strView);
                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_ACCOUNTNAME", p_strACCOUNTNAME);
                cmdView.Parameters.AddWithValue("@p_PFNUMBER", p_strPFNUMBER);
                cmdView.Parameters.AddWithValue("@p_ACCUSED", p_strACCUSED);
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
                conView.Close();
                sda.Dispose();
                cmdView.Dispose();
                conView.Dispose();
            }
        }

        public void funcBindControl(DataTable dtData)
        {
            tabMain.ActiveTabIndex = 0;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;


            txtIACNo.Text = Convert.ToString(dtData.Rows[0]["IACNO"]);
            txtIACNo.ToolTip = Convert.ToString(dtData.Rows[0]["SNO"]);
            txtCompRecDate.Text = Convert.ToString(dtData.Rows[0]["RECDATE"]);
            txtBRComplaint.Text = Convert.ToString(dtData.Rows[0]["BRCOMPLAINT"]);
            objCommonFunction.ddlSetData(ddlCircleOffice, Convert.ToString(dtData.Rows[0]["CIRCLEOFFICE"]), true);
            txtVIGNo.Text = Convert.ToString(dtData.Rows[0]["VIGNO"]);
            txtAccused.Text = Convert.ToString(dtData.Rows[0]["ACCUSED"]);
            txtDAView.Text = Convert.ToString(dtData.Rows[0]["DAVIEW"]);
            txtMeetNo.Text = Convert.ToString(dtData.Rows[0]["MEETNO"]);
            txtRetDate.Text = Convert.ToString(dtData.Rows[0]["RETDATE"]);
            txtIACView.Text = Convert.ToString(dtData.Rows[0]["IACVIEW"]);
            objCommonFunction.ddlSetData(ddlZone, Convert.ToString(dtData.Rows[0]["ZONE"]), true);
            txtSource.Text = Convert.ToString(dtData.Rows[0]["SOURCE"]);
            txtDA.Text = Convert.ToString(dtData.Rows[0]["DA"]);
            txtCVOView.Text = Convert.ToString(dtData.Rows[0]["CVOVIEW"]);
            txtAccountName.Text = Convert.ToString(dtData.Rows[0]["ACCOUNTNAME"]);
            txtAmount.Text = Convert.ToString(dtData.Rows[0]["AMOUNT"]);
            txtIACNo1.Text = Convert.ToString(dtData.Rows[0]["IACNO1"]);
            txtPFNumber.Text = Convert.ToString(dtData.Rows[0]["PFNUMBER"]);
            txtStatus.Text = Convert.ToString(dtData.Rows[0]["STATUS"]);
            objCommonFunction.chkSetData(chkClosureDate, Convert.ToString(dtData.Rows[0]["CLOSURE"]));
            lblClosureDate.Text = Convert.ToString(dtData.Rows[0]["CLOSUREDATE"]);

            objCommonFunction.ddlSetDataValue(ddlStatusCode, Convert.ToString(dtData.Rows[0]["STATUSCODE"]));

            if (objCommonFunction.ddlSelectedValue(ddlStatusCode) == "0")
            {
                lblStatusCodeMIS.Text = Convert.ToString(dtData.Rows[0]["STATUSCODE"]);
            }

            objCommonFunction.ddlSetDataValue(ddlNature, Convert.ToString(dtData.Rows[0]["NATURECODE"]));

            if (objCommonFunction.ddlSelectedValue(ddlNature) == "0")
            {
                lblNatureMIS.Text = Convert.ToString(dtData.Rows[0]["NATURE"]);
            }
            objCommonFunction.ddlSetDataValue(ddlBankName, Convert.ToString(dtData.Rows[0]["BANKNAME"]));
            txtDealingOfficerRemarks.Text = Convert.ToString(dtData.Rows[0]["DESK_USER_REMARKS"]);
            txtLetterSentDate.Text = Convert.ToString(dtData.Rows[0]["LETTERSENTDATE"]);
            txtReminderDate.Text = Convert.ToString(dtData.Rows[0]["REMINDERDATE"]);
            txtReplyReceivedDate.Text = Convert.ToString(dtData.Rows[0]["REPLYRECEIVEDDATE"]);
            objCommonFunction.ddlSetDataValue(ddlLetterSentTo, Convert.ToString(dtData.Rows[0]["LETTERSENTTO"]));
            txtLetterSentToDADate.Text = Convert.ToString(dtData.Rows[0]["LETTERSENTTODADATE"]);
            objCommonFunction.ddlSetDataValue(ddlZoneNew, Convert.ToString(dtData.Rows[0]["NEWZONE"]));
            string ZONE = Convert.ToString(dtData.Rows[0]["NEWZONE"]);
            objCommonFunction.funcZoneCircleMaster(ddlCircleNew, ZONE);
            objCommonFunction.ddlSetDataValue(ddlCircleNew, Convert.ToString(dtData.Rows[0]["NEWCIRCLE"]));
            txtTMSACRefNo.Text = Convert.ToString(dtData.Rows[0]["TMSACREF"]);
            txtDesignation.Text = Convert.ToString(dtData.Rows[0]["DESIGNATION"]);
            objCommonFunction.ddlSetDataValue(ddlScale, Convert.ToString(dtData.Rows[0]["SCALE"]));

            objCommonFunction.ddlSetDataValue(ddlABBFFCase, Convert.ToString(dtData.Rows[0]["ABBFF_CASE"]));
            txtABBFFCaseSubmissionDate.Text = Convert.ToString(dtData.Rows[0]["ABBFF_CASE_SUBMISSION_DATE"]);
            txtABBFFReplyDate.Text = Convert.ToString(dtData.Rows[0]["ABBFF_REPLY_DATE"]);
            txtABBFFReferenceNumber.Text = Convert.ToString(dtData.Rows[0]["ABBFF_REFNO"]);
            txtABBFFAdviceReceiveDate.Text = Convert.ToString(dtData.Rows[0]["ABBFF_ADVICE_RECEIVE_DATE"]);
            txtABBFFAdviceDetail.Text = Convert.ToString(dtData.Rows[0]["ABBFF_ADVICE_DETAIL"]);

            funcApplyCheckerLock(dtData);
        }

        /// <summary>
        /// Fetching a record by IAC number bypasses the grid, so re-apply the same lock here:
        /// a record awaiting verification or rejected by the checker is not editable.
        /// spIACStructure_Update refuses these too -- this only spares the user a pointless save.
        /// </summary>
        private void funcApplyCheckerLock(DataTable dtData)
        {
            if (!dtData.Columns.Contains("APPROVALSTATUS"))
                return;

            string approvalStatus = Convert.ToString(dtData.Rows[0]["APPROVALSTATUS"]);
            string checkerRemarks = dtData.Columns.Contains("CHECKERREMARKS")
                ? Convert.ToString(dtData.Rows[0]["CHECKERREMARKS"])
                : string.Empty;

            if (approvalStatus == "P")
            {
                btnUpdate.Visible = false;
                lblMsg.Text = "This record is pending verification by the checker and cannot be edited.";
            }
            else if (approvalStatus == "X")
            {
                btnUpdate.Visible = false;
                lblMsg.Text = "This record has been rejected by the checker and cannot be edited."
                            + (string.IsNullOrEmpty(checkerRemarks) ? "" : " Remarks: " + checkerRemarks);
            }
            else if (approvalStatus == "C" && !string.IsNullOrEmpty(checkerRemarks))
            {
                lblMsg.Text = "The checker has asked for changes. Remarks: " + checkerRemarks;
            }
        }

        public void funcClear()
        {
            txtIACNo.ToolTip = string.Empty;
            txtIACNo.Text = string.Empty;
            txtCompRecDate.Text = string.Empty;
            txtBRComplaint.Text = string.Empty;
            ddlCircleOffice.SelectedIndex = 0;
            txtVIGNo.Text = string.Empty;
            txtClosureDate.Text = string.Empty;
            txtAccused.Text = string.Empty;
            txtDAView.Text = string.Empty;
            txtMeetNo.Text = string.Empty;
            txtRetDate.Text = string.Empty;
            txtIACView.Text = string.Empty;
            ddlZone.SelectedIndex = 0;
            txtSource.Text = string.Empty;
            txtDA.Text = string.Empty;
            txtCVOView.Text = string.Empty;
            txtAccountName.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtIACNo1.Text = string.Empty;
            txtPFNumber.Text = string.Empty;
            ddlStatusCode.SelectedIndex = 0;
            lblStatusCodeMIS.Text = string.Empty;
            ddlNature.SelectedIndex = 0;
            lblNatureMIS.Text = string.Empty;
            txtStatus.Text = string.Empty;
            txtHOStatus.Text = string.Empty;
            chkClosureDate.Checked = false;
            lblClosureDate.Text = string.Empty;
            txtDealingOfficerRemarks.Text = "";
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            ddlBankName.SelectedIndex = 0;
            ddlLetterSentTo.SelectedIndex = 0;
            txtLetterSentDate.Text = "";
            txtReminderDate.Text = "";
            txtReplyReceivedDate.Text = "";
            funcControlsUserRights();
            ddlZoneNew.SelectedIndex = 0;
            if (ddlCircleNew.Items.Count > 0)
            {
                ddlCircleNew.Items.Clear();
            }

            txtTMSACRefNo.Text = "";
            txtDesignation.Text = "";
            ddlScale.SelectedIndex = 0;

            txtABBFFCaseSubmissionDate.Text = "";
            txtABBFFReplyDate.Text = "";
            txtABBFFReferenceNumber.Text = "";
            txtABBFFAdviceReceiveDate.Text = "";
            txtABBFFAdviceDetail.Text = "";
        }

        public void funcControlsUserRights()
        {
            if (Convert.ToString(Session["role"]).ToUpper() == "VMIS_VIEWUSER")
            {
                objCommonFunction.DisableAllControls(this.Page);
                txtRNo_LIST.Enabled = true;
                txtPFNumber_LIST.Enabled = true;
                txtAccountName_LIST.Enabled = true;
                txtAccused_LIST.Enabled = true;
                txtBranch_LIST.Enabled = true;
                txtCircle_LIST.Enabled = true;
                txtStatus_LIST.Enabled = true;

                btnSubmit.Visible = false;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;
            }
            else if (Convert.ToString(Session["role"]).ToUpper() == "VMIS_DESKUSER")
            {
                objCommonFunction.DisableAllControls(this.Page);
                txtRNo_LIST.Enabled = true;
                txtPFNumber_LIST.Enabled = true;
                txtAccountName_LIST.Enabled = true;
                txtAccused_LIST.Enabled = true;
                txtBranch_LIST.Enabled = true;
                txtCircle_LIST.Enabled = true;
                txtStatus_LIST.Enabled = true;
                pnlHOStatus.Visible = true;
                txtHOStatus.Enabled = true;
                btnUpdate.Visible = true;
                btnUpdate.Enabled = true;
                txtDealingOfficerRemarks.Enabled = true;
                btnSearch_List.Enabled = true;

                foreach (GridViewRow row in gvMain.Rows)
                {
                    Button btnView = row.FindControl("btnView") as Button;

                    if (btnView == null)
                        continue;

                    // Re-enable the row, except where gvMain_RowDataBound locked it because the
                    // record is pending verification or has been rejected. Those stay locked for
                    // every role.
                    btnView.Enabled = btnView.Text == "Edit";
                }

                btnGet.Enabled = true;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            funcSave("I");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            funcSave("U");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            funcClear();
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            funcShow(txtIACNo.Text.Trim(), "GET", null, null, null, null, null, null);
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "VIEW")
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(e.CommandArgument)))
                    {
                        funcShow(Convert.ToString(e.CommandArgument), "VIEW", null, null, null, null, null, null);
                    }
                }
            }
            catch (Exception eg)
            {
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
                funcShow("LIST", "LIST", null, null, null, null, null, null); //for bind grid view on List Tab Load
            }
        }

        protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            e.Row.Attributes.Add("onmouseover",
               "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#20B2AA';");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

            DataRowView drv = e.Row.DataItem as DataRowView;

            if (drv == null || !drv.Row.Table.Columns.Contains("APPROVALSTATUS"))
                return;

            Button btn = e.Row.FindControl("btnView") as Button;

            if (btn == null)
                return;

            // The button label is what funcControlsUserRights() reads back to decide whether a
            // row may be reopened, so keep "Edit" reserved for genuinely editable records.
            switch (Convert.ToString(drv["APPROVALSTATUS"]))
            {
                case "P":
                    // Awaiting the checker. Not the maker's to change until they act.
                    btn.Enabled = false;
                    btn.Text = "Pending";
                    btn.CssClass = "btn btn-sm btn-warning";
                    break;

                case "C":
                    // Pushed back for correction - this is exactly what the maker should reopen.
                    btn.Enabled = true;
                    btn.Text = "Edit";
                    btn.CssClass = "btn btn-sm btn-info";
                    break;

                case "X":
                    // Rejected by the checker - locked.
                    btn.Enabled = false;
                    btn.Text = "Rejected";
                    btn.CssClass = "btn btn-sm btn-danger";
                    break;

                default:
                    // Approved, or a record that predates the workflow (NULL).
                    // Editing it re-queues it for the checker.
                    btn.Enabled = true;
                    btn.Text = "Edit";
                    btn.CssClass = "btn btn-sm btn-danger";
                    break;
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
            string strACCOUNTNAME = txtAccountName_LIST.Text;
            string strPFNUMBER = txtPFNumber_LIST.Text.Trim();
            string strACCUSED = txtAccused_LIST.Text.Trim();
            string strSTATUS = txtStatus_LIST.Text.Trim();
            string strBRCOMPLAINT = txtBranch_LIST.Text.Trim();
            string strCIRCLEOFFICE = txtCircle_LIST.Text.Trim();
            string strVIEW = "SEARCH";

            if (strSearchNo == "" && strACCOUNTNAME == "" && strPFNUMBER == "" && strACCUSED == "" && strSTATUS == "" && strBRCOMPLAINT == "" && strCIRCLEOFFICE == "")
            {
                strVIEW = "LIST";
            }

            funcShow(strSearchNo, strVIEW, strACCOUNTNAME, strPFNUMBER, strACCUSED, strSTATUS, strBRCOMPLAINT, strCIRCLEOFFICE);
        }
    }
}