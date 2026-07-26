using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class frmComplaint : System.Web.UI.Page
    {
        string EOUNIQUEID = string.Empty;

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
        DateTime? dtEODOR = null;

        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                funcShow(null, "LIST", null, null, null, null, null, null, null, null); //for bind grid view on form Load
                funcbindDropdown();     //Bind Circle Office DropDown List
            }

            funcControlsUserRights();

            txtAmount.Attributes.Add("onkeypress", "return isNumbericDecimal(event,'" + txtAmount.ClientID + "')");
            btnSubmit.Attributes.Add("onclick", "return funcValidation_Complaint('" + txtRNo.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            btnUpdate.Attributes.Add("onclick", "return funcValidation_Complaint('" + txtRNo.ClientID + "','" + ddlCircleOffice.ClientID + "')");
        }

        protected void btnFetchIAC_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCaseNo.Text))
            {
                lblMsg.Text = "Please enter Case/IAC No.";
                return;
            }

            FetchIACDetails(txtCaseNo.Text.Trim());
        }

        private void FetchIACDetails(string iacNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[spIACStructure_View]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_VIEW", "GET");
                    cmd.Parameters.AddWithValue("@p_SEARCHNO", iacNo);
                    cmd.Parameters.AddWithValue("@p_ACCOUNTNAME", DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_PFNUMBER", DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_ACCUSED", DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_STATUS", DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_BRANCH", DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_CIRCLE", DBNull.Value);

                    SqlParameter errMsg =
                        new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000)
                        {
                            Direction = ParameterDirection.Output
                        };

                    SqlParameter errCode =
                        new SqlParameter("@o_ERRCODE", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                    cmd.Parameters.Add(errMsg);
                    cmd.Parameters.Add(errCode);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        lblMsg.Text = Convert.ToString(errMsg.Value);
                        return;
                    }
                }
            }

            DataRow dr = dt.Rows[0];

            // Populate Complaint screen
            txtAccused.Text = Convert.ToString(dr["ACCUSED"]);
            txtPFNumber.Text = Convert.ToString(dr["PFNUMBER"]);
            txtDesignation.Text = Convert.ToString(dr["DESIGNATION"]);
            txtAccountName.Text = Convert.ToString(dr["ACCOUNTNAME"]);
            txtAmount.Text = Convert.ToString(dr["AMOUNT"]);
            txtStatus.Text = Convert.ToString(dr["STATUS"]);

            txtBRComplaint.Text = Convert.ToString(dr["BRCOMPLAINT"]);
            txtSource.Text = Convert.ToString(dr["SOURCE"]);

            txtIACDate.Text = Convert.ToString(dr["RECDATE"]);

            objCommonFunction.ddlSetData(
                ddlCircleOffice,
                Convert.ToString(dr["CIRCLEOFFICE"]),
                true);

            objCommonFunction.ddlSetData(
                ddlZone,
                Convert.ToString(dr["ZONE"]),
                true);

            lblMsg.Text = "IAC details fetched successfully.";
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
                cmdSave.CommandText = "[dbo].[spComplaint_Update]";

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
                cmdSave.Parameters.AddWithValue("@p_NAMEOFINVOFFICIAL", txtNameINVOfficial.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DTOFINVREPORT", dtDTOFINVREPORT);
                cmdSave.Parameters.AddWithValue("@p_CASENO", txtCaseNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CASECLOSE", txtClose.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CLOSUREDT", dtCLOSUREDT);
                cmdSave.Parameters.AddWithValue("@p_RYSENT", dtRYSENT);
                cmdSave.Parameters.AddWithValue("@p_REASONSFORCLOSURE", txtRessonsForClosure.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CLOSURE", strCLOSURE);
                cmdSave.Parameters.AddWithValue("@p_DESK_USER_REMARKS", txtDealingOfficerRemarks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PFNUMBER", txtPFNumber.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BANKNAME", objCommonFunction.ddlSelectedValue(ddlBankName));
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTO", objCommonFunction.ddlSelectedValue(ddlLetterSentTo));
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTDATE", dtLETTERSENTDATE);
                cmdSave.Parameters.AddWithValue("@p_REMINDERDATE", dtREMINDERDATE);
                cmdSave.Parameters.AddWithValue("@p_REPLYRECEIVEDDATE", dtREPLYRECEIVEDDATE);
                cmdSave.Parameters.AddWithValue("@p_MARKEDFORINVESTIGATION", "");
                cmdSave.Parameters.AddWithValue("@p_ZONENEW", objCommonFunction.ddlSelectedValue(ddlZoneNew));
                cmdSave.Parameters.AddWithValue("@p_CIRCLENEW", objCommonFunction.ddlSelectedValue(ddlCircleNew));
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["userid"]));
                cmdSave.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    funcClear();
                    lblMsg.Text = sqlErrMsgOutput.Value.ToString();
                }
                else
                {
                    lblMsg.Text = sqlErrMsgOutput.Value.ToString();
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
        }

        public void funcShow(string p_strNo, string p_strView, string p_strBRCOMPLAINT, string p_strACCUSED, string p_strALLEGATIONS, string p_strSTATUS, string p_strINTERNALREFNO, string p_strACCOUNTNAME, string p_strEXTERNALSOURCE, string p_strCIRCLE)
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
                cmdView.CommandText = "[dbo].[spComplaint_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_VIEW", p_strView);
                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_BRANCH", p_strBRCOMPLAINT);
                cmdView.Parameters.AddWithValue("@p_ACCUSED", p_strACCUSED);
                cmdView.Parameters.AddWithValue("@p_ALLEGATIONS", p_strALLEGATIONS);
                cmdView.Parameters.AddWithValue("@p_STATUS", p_strSTATUS);
                cmdView.Parameters.AddWithValue("@p_INTERNALREFNO", p_strINTERNALREFNO);
                cmdView.Parameters.AddWithValue("@p_ACCOUNTNAME", p_strACCOUNTNAME);
                cmdView.Parameters.AddWithValue("@p_EXTERNALSOURCE", p_strEXTERNALSOURCE);
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
                            funcShowEODetails(dt.Rows[0]["RNO"].ToString());
                        }
                        else if (p_strView.ToUpper() == "VIEW")
                        {
                            funcBindControl(dt);
                            funcShowEODetails(dt.Rows[0]["RNO"].ToString());
                        }
                    }
                    else
                    {
                        lblList.Text = "Record not found.";
                    }

                    funcControlsUserRights();
                }

                else
                {
                    lblMsg.Text = sqlErrMsgOutput.Value.ToString();
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

        public void funcBindControl(DataTable dt)
        {
            DataTable dtData = dt;
            tabMain.ActiveTabIndex = 0;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;

            txtRNo.ToolTip = Convert.ToString(dtData.Rows[0]["CODE"]);
            txtRNo.Text = Convert.ToString(dtData.Rows[0]["RNO"]);
            txtCompRecDate.Text = Convert.ToString(dtData.Rows[0]["COMPRECDATE"]);
            txtBRComplaint.Text = Convert.ToString(dtData.Rows[0]["BRCOMPLAINT"]);
            objCommonFunction.ddlSetData(ddlCircleOffice, Convert.ToString(dtData.Rows[0]["CIRCLEOFFICE"]), true);
            txtCompNo.Text = Convert.ToString(dtData.Rows[0]["COMPNO"]);
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
            txtNameINVOfficial.Text = Convert.ToString(dtData.Rows[0]["NAMEOFINVOFFICIAL"]);
            txtClose.Text = Convert.ToString(dtData.Rows[0]["CASECLOSE"]);
            txtRYSent.Text = Convert.ToString(dtData.Rows[0]["RYSENTDATE"]);
            txtRessonsForClosure.Text = Convert.ToString(dtData.Rows[0]["REASONSFORCLOSURE"]);
            txtStatus.Text = Convert.ToString(dtData.Rows[0]["STATUS"]);
            objCommonFunction.chkSetData(chkClosureDate, Convert.ToString(dtData.Rows[0]["CLOSURE"]));
            lblClosureDate.Text = Convert.ToString(dtData.Rows[0]["CLOSUREDATE"]);
            txtPFNumber.Text = Convert.ToString(dtData.Rows[0]["PFNUMBER"]);
            objCommonFunction.ddlSetDataValue(ddlBankName, Convert.ToString(dtData.Rows[0]["BANKNAME"]));
            objCommonFunction.ddlSetDataValue(ddlStatusCode, Convert.ToString(dtData.Rows[0]["STATUSCODE"]));

            if (objCommonFunction.ddlSelectedValue(ddlStatusCode) == "0" && Convert.ToString(dtData.Rows[0]["STATUSCODE"]) != "0")
            {
                lblStatusCodeMIS.Text = Convert.ToString(dtData.Rows[0]["STATUSCODE"]);
            }

            txtDealingOfficerRemarks.Text = Convert.ToString(dtData.Rows[0]["DESK_USER_REMARKS"]);
            txtLetterSentDate.Text = Convert.ToString(dtData.Rows[0]["LETTERSENTDATE"]);
            txtReminderDate.Text = Convert.ToString(dtData.Rows[0]["REMINDERDATE"]);
            txtReplyReceivedDate.Text = Convert.ToString(dtData.Rows[0]["REPLYRECEIVEDDATE"]);
            objCommonFunction.ddlSetDataValue(ddlLetterSentTo, Convert.ToString(dtData.Rows[0]["LETTERSENTTO"]));
            hidLetterSentTo.Value = Convert.ToString(dtData.Rows[0]["LETTERSENTTO"]);
            objCommonFunction.ddlSetDataValue(ddlZoneNew, Convert.ToString(dtData.Rows[0]["NEWZONE"]));
            objCommonFunction.funcZoneCircleMaster(ddlCircleNew, Convert.ToString(dtData.Rows[0]["NEWZONE"]));
            objCommonFunction.ddlSetDataValue(ddlCircleNew, Convert.ToString(dtData.Rows[0]["NEWCIRCLE"]));

            lblMsg.Text = "";
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
                cmd.CommandText = "[dbo].[spComplaint_Ddl]";
                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlCircleOffice, ds.Tables[0]);
                    objCommonFunction.bindDropdownList(ddlZone, ds.Tables[1]);
                    objCommonFunction.bindDropdownList(ddlLetterSentTo, ds.Tables[2]);
                    objCommonFunction.bindDropdownList(ddlStatusCode, ds.Tables[3]);
                    objCommonFunction.bindDropdownList(ddlSourceRef, ds.Tables[4]);
                    objCommonFunction.bindDropdownList(ddlZoneNew, ds.Tables[5]);
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
            txtNameINVOfficial.Text = string.Empty;
            txtClose.Text = string.Empty;
            txtRYSent.Text = string.Empty;
            txtRessonsForClosure.Text = string.Empty;
            txtStatus.Text = string.Empty;
            txtHOStatus.Text = string.Empty;
            chkClosureDate.Checked = false;
            lblClosureDate.Text = string.Empty;

            btnSubmit.Visible = true;
            btnUpdate.Visible = false;

            txtDealingOfficerRemarks.Text = "";
            txtPFNumber.Text = "";
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

            funcClearEODetails();
            gvEODetails.DataSource = null;
            gvEODetails.DataBind();
            lblMsg.Text = "";
            funcControlsUserRights();
        }

        public void funcControlsUserRights()
        {
            if (Convert.ToString(ViewState["USERROLE"]).ToUpper().Equals("VMIS_VIEWUSER"))
            {
                objCommonFunction.DisableAllControls(this.Page);
                txtRNo_LIST.Enabled = true;
                txtBranch_LIST.Enabled = true;
                txtAccused_LIST.Enabled = true;
                txtAllegations_LIST.Enabled = true;
                txtCircle_LIST.Enabled = true;
                txtInternalRefNo_LIST.Enabled = true;
                txtAccountName_LIST.Enabled = true;
                txtExternalSource_LIST.Enabled = true;
                txtStatus_LIST.Enabled = true;

                btnSubmit.Visible = false;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;
                btnSearch_List.Enabled = true;
            }
            else if (Convert.ToString(ViewState["USERROLE"]).ToUpper().Equals("VMIS_DESKUSER"))
            {
                objCommonFunction.DisableAllControls(this.Page);
                txtRNo_LIST.Enabled = true;
                txtBranch_LIST.Enabled = true;
                txtAccused_LIST.Enabled = true;
                txtAllegations_LIST.Enabled = true;
                txtCircle_LIST.Enabled = true;
                txtInternalRefNo_LIST.Enabled = true;
                txtAccountName_LIST.Enabled = true;
                txtExternalSource_LIST.Enabled = true;
                txtStatus_LIST.Enabled = true;

                pnlHOStatus.Visible = true;
                txtHOStatus.Enabled = true;

                btnUpdate.Visible = true;
                btnUpdate.Enabled = true;
                txtDealingOfficerRemarks.Enabled = true;
                btnSearch_List.Enabled = true;
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
                lblMsg.Text = "Please Enter Complaint Number.";
                return;
            }

            funcShow(txtRNo.Text.Trim(), "GET", null, null, null, null, null, null, null, null);
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "VIEW")
            {
                if (!string.IsNullOrEmpty(Convert.ToString(e.CommandArgument)))
                {
                    funcShow(Convert.ToString(e.CommandArgument), "VIEW", null, null, null, null, null, null, null, null);
                }
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

        protected void btnAddEO_Click(object sender, EventArgs e)
        {
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
                if (funcAddEO())
                {
                    funcClearEODetails();
                    funcShowEODetails(txtRNo.Text.Trim());
                }
            }
        }

        private bool funcAddEO()
        {
            SqlConnection conSave = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();
            string MODE = string.Empty;
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
                cmdSave.CommandText = "[dbo].[spComplaintEO_Add]";

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
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(ViewState["USERNAME"]));

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
                    return true;
                }
                else
                {
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
                    return false;
                }

            }
            catch (Exception es)
            {
                lblMsg.Text = es.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
                return false;
            }
            finally
            {
                cmdSave.Dispose();
                conSave.Dispose();
                conSave.Close();
            }
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
                cmdView.CommandText = "[dbo].[spComplaintEO_View]";

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
            con.Open();
            cmdSave.Connection = con;
            cmdSave.Parameters.Clear();

            try
            {
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spComplaintEO_Delete]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_EO_UNIQUEID", EOUniqueID);
                cmdSave.Parameters.AddWithValue("@p_UNIQUEID", UniqueID);
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(ViewState["USERNAME"]));

                cmdSave.CommandTimeout = 0;

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
                    funcShowEODetails(UniqueID); //Update grid data.
                }
                else
                {
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
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


        protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            DataRowView drv = (DataRowView)e.Row.DataItem;

            string approvalStatus = Convert.ToString(drv["APPROVALSTATUS"]);

            Button btn = (Button)e.Row.FindControl("btnView");

            switch (approvalStatus)
            {
                case "P":
                    btn.Enabled = false;
                    btn.Text = "Pending";
                    btn.CssClass = "btn btn-sm btn-warning";
                    break;

                case "C":
                    btn.Enabled = true;
                    btn.Text = "Edit";
                    btn.CssClass = "btn btn-sm btn-info";
                    break;

                case "X":
                    // Rejected by checker - locked from editing for now.
                    btn.Enabled = false;
                    btn.Text = "Rejected";
                    btn.CssClass = "btn btn-sm btn-danger";
                    break;

                default:
                    // Existing records (NULL) and approved records.
                    // Keep current behaviour.
                    btn.Enabled = true;
                    btn.Text = "Edit";
                    btn.CssClass = "btn btn-sm btn-danger";
                    break;
            }
        }

        protected void btnSearch_List_Click(object sender, EventArgs e)
        {
            string VIEW = "SEARCH";

            if (txtRNo_LIST.Text.Trim() == "" && txtBranch_LIST.Text.Trim() == "" && txtAccused_LIST.Text.Trim() == "" && txtAllegations_LIST.Text.Trim() == "" && txtStatus_LIST.Text.Trim() == "" && txtInternalRefNo_LIST.Text.Trim() == "" && txtAccountName_LIST.Text.Trim() == "" && txtExternalSource_LIST.Text.Trim() == "" && txtCircle_LIST.Text.Trim() == "")
            {
                VIEW = "LIST";
            }

            funcShow(txtRNo_LIST.Text.Trim(), VIEW, txtBranch_LIST.Text.Trim(), txtAccused_LIST.Text.Trim(), txtAllegations_LIST.Text.Trim(), txtStatus_LIST.Text.Trim(), txtInternalRefNo_LIST.Text.Trim(), txtAccountName_LIST.Text.Trim(), txtExternalSource_LIST.Text.Trim(), txtCircle_LIST.Text.Trim());
        }
    }
}