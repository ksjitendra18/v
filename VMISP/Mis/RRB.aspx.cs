using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class RRB : System.Web.UI.Page
    {
        DateTime? dtRNODATE = null;
        DateTime? dtCHARGEDATE = null;
        DateTime? dtRC1DATE = null;
        DateTime? dtRETIREMENTDATE = null;
        DateTime? dtAPPPODATE = null;
        DateTime? dtAPPEODATE = null;
        DateTime? dtCVO2ADVICEDATE = null;
        DateTime? dt2NDDADATE = null;
        DateTime? dtDAORDDATE = null;
        DateTime? dtISTDADATE = null;
        DateTime? dtCLOSUREDATE = null;
        DateTime? dtCVOADVICEDATE = null;
        DateTime? dtRC2DATE = null;
        DateTime? dtLETTERSENTDATE = null;
        DateTime? dtREMINDERDATE = null;
        DateTime? dtREPLYRECEIVEDDATE = null;
        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                funcShow(null, "LIST", null, null); //for bind grid view on form Load
                funcbindDropdown();     //Bind All DropDown Lists
            }

            lblMsg.Text = string.Empty;
            funcControlsUserRights();

            #region ** JS Function  **
            btnSubmit.Attributes.Add("onclick", "return funcValidation_RRB('" + txtRNo.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            btnUpdate.Attributes.Add("onclick", "return funcValidation_RRB('" + txtRNo.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            #endregion
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

                #region ** convert Date **
                string strRNODATE = txtRNoDate.Text.Trim();
                if (!string.IsNullOrEmpty(strRNODATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRNODATE, out date))
                        dtRNODATE = date;
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

                string strCVO2ADVICEDATE = txtCVO2AdviceDate.Text.Trim();
                if (!string.IsNullOrEmpty(strCVO2ADVICEDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strCVO2ADVICEDATE, out date))
                        dtCVO2ADVICEDATE = date;
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

                string strISTDADATE = txtIstDaDate.Text.Trim();
                if (!string.IsNullOrEmpty(strISTDADATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strISTDADATE, out date))
                        dtISTDADATE = date;
                }

                string strCVOADVICEDATE = txtCVOAdviceDate.Text.Trim();
                if (!string.IsNullOrEmpty(strCVOADVICEDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strCVOADVICEDATE, out date))
                        dtCVOADVICEDATE = date;
                }

                string strRC2DATE = txtRC2Date.Text.Trim();
                if (!string.IsNullOrEmpty(strRC2DATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRC2DATE, out date))
                        dtRC2DATE = date;
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
                cmdSave.CommandText = "[dbo].[spRRB_Operation]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_CODE", objCommonFunction.convertToIntToolTip(txtRNo));
                cmdSave.Parameters.AddWithValue("@p_RNO", txtRNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BRCOMPLAINT", txtBRComplaint.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_RNO1", txtRNo1.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NAMEOFPARTICULARS", txtNameOfParticulars.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NAME", txtName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PFNO", txtPFNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_REGISTER", txtRegister.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_LAPSENATURE", txtLapseNature.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CBIRCNO1", txtCbiRcNo1.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PONAME", txtPOName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CVOADVICE", txtCVOAdvice.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NAPUNDA", txtNaPunDa.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_2DAPROPOSAL", txt2DAProposal.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NATCHSHEET", txtNatCHSheet.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CBIRCNO2", txtCBIRCNo2.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_EONAME", txtEOName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DAPROPOSAL", txtDAProposal.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CVO2ADVICE", txtCVO2Advice.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_STATUS", txtStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_HOSTATUS", txtHOStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DESK_USER_REMARKS", txtDealingOfficerRemarks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CIRCLEOFFICE", objCommonFunction.ddlSelectedText(ddlCircleOffice));
                cmdSave.Parameters.AddWithValue("@p_FINAL", objCommonFunction.ddlSelectedValue(ddlFinal));
                cmdSave.Parameters.AddWithValue("@p_DISPAUTHORITY", objCommonFunction.ddlSelectedText(ddlDispAuthority));
                cmdSave.Parameters.AddWithValue("@p_STATUSCODE", objCommonFunction.ddlSelectedValue(ddlStatusCode));
                cmdSave.Parameters.AddWithValue("@p_SCALE", objCommonFunction.ddlSelectedValue_Scale(ddlScale));
                cmdSave.Parameters.AddWithValue("@p_DISAUTHORITYZONE", objCommonFunction.ddlSelectedValue(ddlDisAuthorityZone));
                cmdSave.Parameters.AddWithValue("@p_BANKNAME", objCommonFunction.ddlSelectedValue(ddlBankName));
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTO", objCommonFunction.ddlSelectedValue(ddlLetterSentTo));
                cmdSave.Parameters.AddWithValue("@p_ZONENEW", objCommonFunction.ddlSelectedValue(ddlZoneNew));
                cmdSave.Parameters.AddWithValue("@p_CIRCLENEW", objCommonFunction.ddlSelectedValue(ddlCircleNew));

                cmdSave.Parameters.AddWithValue("@p_RNODATE", dtRNODATE);
                cmdSave.Parameters.AddWithValue("@p_CHARGEDATE", dtCHARGEDATE);
                cmdSave.Parameters.AddWithValue("@p_RC1DATE", dtRC1DATE);
                cmdSave.Parameters.AddWithValue("@p_RETIREMENTDATE", dtRETIREMENTDATE);
                cmdSave.Parameters.AddWithValue("@p_APPPODATE", dtAPPPODATE);
                cmdSave.Parameters.AddWithValue("@p_APPEODATE", dtAPPEODATE);
                cmdSave.Parameters.AddWithValue("@p_CVO2ADVICEDATE", dtCVO2ADVICEDATE);
                cmdSave.Parameters.AddWithValue("@p_2NDDADATE", dt2NDDADATE);
                cmdSave.Parameters.AddWithValue("@p_DAORDDATE", dtDAORDDATE);
                cmdSave.Parameters.AddWithValue("@p_ISTDADATE", dtISTDADATE);
                cmdSave.Parameters.AddWithValue("@p_CLOSUREDATE", dtCLOSUREDATE);
                cmdSave.Parameters.AddWithValue("@p_CVOADVICEDATE", dtCVOADVICEDATE);
                cmdSave.Parameters.AddWithValue("@p_RC2DATE", dtRC2DATE);
                cmdSave.Parameters.AddWithValue("@p_CLOSURE", strCLOSURE);
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTDATE", dtLETTERSENTDATE);
                cmdSave.Parameters.AddWithValue("@p_REMINDERDATE", dtREMINDERDATE);
                cmdSave.Parameters.AddWithValue("@p_REPLYRECEIVEDDATE", dtREPLYRECEIVEDDATE);
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["userid"]));
                cmdSave.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                cmdSave.ExecuteNonQuery();

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

        public void funcShow(string p_strNo, string p_strView, string p_strNAME, string p_strZONE)
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
                cmdView.CommandText = "[dbo].[spRRB_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_VIEW", p_strView);
                cmdView.Parameters.AddWithValue("@p_NAME", p_strNAME);
                cmdView.Parameters.AddWithValue("@p_ZONE", p_strZONE);

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
                conView.Close();
                sda.Dispose();
                cmdView.Dispose();
                conView.Dispose();
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
                cmd.CommandText = "[dbo].[spRRB_Ddl]";
                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlCircleOffice, ds.Tables[0]);
                    objCommonFunction.bindDropdownList_SELECT(ddlScale, ds.Tables[1]);
                    objCommonFunction.bindDropdownList(ddlLetterSentTo, ds.Tables[2]);
                    objCommonFunction.bindDropdownList(ddlStatusCode, ds.Tables[3]);
                    objCommonFunction.bindDropdownList(ddlZoneNew, ds.Tables[4]);
                }
                #endregion
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

        public void funcBindControl(DataTable dt)
        {
            DataTable dtData = dt;
            tabMain.ActiveTabIndex = 0;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;

            txtRNo.ToolTip = dtData.Rows[0]["CODE"].ToString();
            txtRNo.Text = dtData.Rows[0]["RNO"].ToString();
            txtBRComplaint.Text = dtData.Rows[0]["BRCOMPLAINT"].ToString();
            txtRNo1.Text = dtData.Rows[0]["RNO1"].ToString();
            txtNameOfParticulars.Text = dtData.Rows[0]["NAMEOFPARTICULARS"].ToString();
            txtName.Text = dtData.Rows[0]["NAME"].ToString();
            txtPFNo.Text = dtData.Rows[0]["PFNO"].ToString();
            txtRegister.Text = dtData.Rows[0]["REGISTER"].ToString();
            txtLapseNature.Text = dtData.Rows[0]["LAPSENATURE"].ToString();
            txtCbiRcNo1.Text = dtData.Rows[0]["CBIRCNO1"].ToString();
            txtPOName.Text = dtData.Rows[0]["PONAME"].ToString();
            txtCVOAdvice.Text = dtData.Rows[0]["CVOADVICE"].ToString();
            txtNaPunDa.Text = dtData.Rows[0]["NAPUNDA"].ToString();
            txt2DAProposal.Text = dtData.Rows[0]["DAPROPOSAL2"].ToString();
            txtNatCHSheet.Text = dtData.Rows[0]["NATCHSHEET"].ToString();
            txtCBIRCNo2.Text = dtData.Rows[0]["CBIRCNO2"].ToString();
            txtEOName.Text = dtData.Rows[0]["EONAME"].ToString();
            txtDAProposal.Text = dtData.Rows[0]["DAPROPOSAL"].ToString();
            txtCVO2Advice.Text = dtData.Rows[0]["CVO2ADVICE"].ToString();
            objCommonFunction.chkSetData(chkClosureDate, dtData.Rows[0]["CLOSURE"].ToString());
            lblClosureDate.Text = dtData.Rows[0]["CLOSUREDATE"].ToString();

            txtRNoDate.Text = dtData.Rows[0]["RNODATE"].ToString();
            txtChargeDate.Text = dtData.Rows[0]["CHARGEDATE"].ToString();
            txtRC1Date.Text = dtData.Rows[0]["RC1DATE"].ToString();
            txtRetirementDate.Text = dtData.Rows[0]["RETIREMENTDATE"].ToString();
            txtAppPODate.Text = dtData.Rows[0]["APPPODATE"].ToString();
            txtAppEODate.Text = dtData.Rows[0]["APPEODATE"].ToString();
            txtCVO2AdviceDate.Text = dtData.Rows[0]["CVO2ADVICEDATE"].ToString();
            txt2ndDADate.Text = dtData.Rows[0]["DA2NDDATE"].ToString();
            txtDAOrdDate.Text = dtData.Rows[0]["DAORDDATE"].ToString();
            txtIstDaDate.Text = dtData.Rows[0]["ISTDADATE"].ToString();
            txtCVOAdviceDate.Text = dtData.Rows[0]["CVOADVICEDATE"].ToString();
            txtRC2Date.Text = dtData.Rows[0]["RC2DATE"].ToString();

            objCommonFunction.ddlSetData(ddlCircleOffice, dtData.Rows[0]["CIRCLEOFFICE"].ToString(), true);

            objCommonFunction.ddlSetDataValue_Scale(ddlScale, dtData.Rows[0]["SCALE"].ToString());
            objCommonFunction.ddlSetDataValue(ddlDisAuthorityZone, dtData.Rows[0]["DISAUTHORITYZONE"].ToString());

            objCommonFunction.ddlSetData(ddlDispAuthority, dtData.Rows[0]["DISPAUTHORITY"].ToString(), true);
            objCommonFunction.ddlSetData(ddlFinal, dtData.Rows[0]["FINAL"].ToString(), true);
            objCommonFunction.ddlSetDataValue(ddlStatusCode, dtData.Rows[0]["STATUSCODE"].ToString());
            if (objCommonFunction.ddlSelectedValue(ddlStatusCode) == "0")
            {
                lblStatusCodeMIS.Text = dtData.Rows[0]["STATUSCODE"].ToString();
            }



            txtDealingOfficerRemarks.Text = Convert.ToString(dtData.Rows[0]["DESK_USER_REMARKS"]);
            objCommonFunction.ddlSetDataValue(ddlBankName, Convert.ToString(dtData.Rows[0]["BANKNAME"]));
            txtStatus.Text = dtData.Rows[0]["STATUS"].ToString();
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

        public void funcClear()
        {
            txtDealingOfficerRemarks.Text = "";
            txtRNo.ToolTip = string.Empty;
            txtRNo.Text = string.Empty;
            txtBRComplaint.Text = string.Empty;
            txtRNo1.Text = string.Empty;
            txtNameOfParticulars.Text = string.Empty;
            txtName.Text = string.Empty;
            txtPFNo.Text = string.Empty;
            txtRegister.Text = string.Empty;
            ddlFinal.SelectedIndex = 0;
            txtLapseNature.Text = string.Empty;
            txtCbiRcNo1.Text = string.Empty;
            txtPOName.Text = string.Empty;
            txtCVOAdvice.Text = string.Empty;
            txtNaPunDa.Text = string.Empty;
            txt2DAProposal.Text = string.Empty;
            txtNatCHSheet.Text = string.Empty;
            txtCBIRCNo2.Text = string.Empty;
            txtEOName.Text = string.Empty;
            txtDAProposal.Text = string.Empty;
            txtCVO2Advice.Text = string.Empty;
            txtStatus.Text = string.Empty;
            txtHOStatus.Text = string.Empty;
            chkClosureDate.Checked = false;
            lblClosureDate.Text = string.Empty;

            txtRNoDate.Text = string.Empty;
            txtChargeDate.Text = string.Empty;
            txtRC1Date.Text = string.Empty;
            txtRetirementDate.Text = string.Empty;
            txtAppPODate.Text = string.Empty;
            txtAppEODate.Text = string.Empty;
            txtCVO2AdviceDate.Text = string.Empty;
            txt2ndDADate.Text = string.Empty;
            txtDAOrdDate.Text = string.Empty;
            txtIstDaDate.Text = string.Empty;
            txtClosureDate.Text = string.Empty;
            txtCVOAdviceDate.Text = string.Empty;
            txtRC2Date.Text = string.Empty;
            ddlCircleOffice.SelectedIndex = 0;

            ddlStatusCode.SelectedIndex = 0;
            ddlScale.SelectedIndex = 0;
            ddlDisAuthorityZone.SelectedIndex = 0;
            ddlDispAuthority.SelectedIndex = 0;
            ddlBankName.SelectedIndex = 0;


            lblStatusCodeMIS.Text = string.Empty;
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
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

            funcControlsUserRights();
        }

        public void funcControlsUserRights()
        {
            if (Convert.ToString(ViewState["USERROLE"]).Equals("VMIS_VIEWUSER"))
            {
                objCommonFunction.DisableAllControls(this.Page);
                btnSubmit.Visible = false;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;

                txtRNo_LIST.Enabled = true;
                txtName_LIST.Enabled = true;
                txtZone_LIST.Enabled = true;
            }
            else if (Convert.ToString(ViewState["USERROLE"]).Equals("VMIS_DESKUSER"))
            {
                objCommonFunction.DisableAllControls(this.Page);
                pnlHOStatus.Visible = true;
                btnSubmit.Visible = false;
                btnUpdate.Visible = true;
                btnUpdate.Enabled = true;
                btnCancel.Visible = false;
                txtHOStatus.Enabled = true;
                txtDealingOfficerRemarks.Enabled = true;

                txtRNo_LIST.Enabled = true;
                txtName_LIST.Enabled = true;
                txtZone_LIST.Enabled = true;
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

            funcShow(txtRNo.Text.Trim(), "GET", null, null);
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "VIEW")
            {
                if (!string.IsNullOrEmpty(Convert.ToString(e.CommandArgument)))
                {
                    funcShow(Convert.ToString(e.CommandArgument), "VIEW", null, null);
                }
            }
        }

        protected void tabMain_ActiveTabChanged(object sender, EventArgs e)
        {
            if (tabMain.ActiveTab == tabList)
            {
                funcShow(null, "LIST", null, null); //for bind grid view on List Tab Load
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
            string strNAME = txtName_LIST.Text;
            string strZONE = txtZone_LIST.Text;
            string strVIEW = "SEARCH";

            if (strSearchNo == "" && strNAME == "" && strZONE == "")
            {
                strVIEW = "LIST";
            }

            funcShow(strSearchNo, strVIEW, strNAME, strZONE);
        }
    }
}