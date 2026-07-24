using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class SanctionForProsecution : System.Web.UI.Page
    {
        string strErrMsg = string.Empty;
        int intErrCode = 0;
        string UNIQUENO = string.Empty;
        DateTime? RCDATE = null;
        DateTime? DOR = null;
        DateTime? REPORTDATE = null;

        string CIRCLE = string.Empty;
        string BRANCH = string.Empty;
        string DAVIEW = string.Empty;
        DateTime? LETTERTOCBIDATE = null;
        DateTime? LETTERTOCVCDATE = null;
        string CVCVIEW = string.Empty;
        DateTime? LETTERTODADATE = null;
        DateTime? DAORDERTOCBIDATE = null;
        string STATUS = string.Empty;
        string BANKNAME = string.Empty;
        DateTime? dtLETTERSENTDATE = null;
        DateTime? dtREMINDERDATE = null;
        DateTime? dtREPLYRECEIVEDDATE = null;
        string LETTERSENTTO = string.Empty;

        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();
                hidUserRole.Value = Session["role"].ToString();

                funcBindMasterDropdownList("DEFAULT", null);
                funcShow("LIST", null, null, null);

                objCommonFunction.disableControlsTextBox(txtRCDate);
                objCommonFunction.disableControlsTextBox(txtReportDate);
                objCommonFunction.disableControlsTextBox(txtLetterToCBIDate);
                objCommonFunction.disableControlsTextBox(txtLetterToCVCDate);
                objCommonFunction.disableControlsTextBox(txtLetterToDADate);
                objCommonFunction.disableControlsTextBox(txtDAOrderToCBIDate);
                objCommonFunction.disableControlsTextBox(txtLetterSentDate);
                objCommonFunction.disableControlsTextBox(txtReminderDate);
                objCommonFunction.disableControlsTextBox(txtReplyReceivedDate);
            }
        }

        public void funcControlsUserRights()
        {
            string UserRole = Convert.ToString(ViewState["USERROLE"]);

            if (UserRole.ToUpper().Equals("VMIS_VIEWUSER"))
            {
                objCommonFunction.DisableAllControls(this.Page);

                foreach (GridViewRow row in gvDetails.Rows)
                {
                    Button btnView = ((Button)row.FindControl("btnView")) as Button;
                    btnView.Enabled = true;
                }

                txtRCNO_LIST.Enabled = true;
                txtSPNO_LIST.Enabled = true;
                btnSearch.Enabled = true;
            }
            else if (UserRole.ToUpper().Equals("VMIS_DESKUSER"))
            {
                objCommonFunction.DisableAllControls(this.Page);
                txtDealingOfficerRemarks.Enabled = true;
                btnSubmit.Visible = false;
                txtRCNO_LIST.Enabled = true;
                txtSPNO_LIST.Enabled = true;
                btnSearch.Enabled = true;
                btnReset.Enabled = true;
                btnUpdate.Enabled = true;

                foreach (GridViewRow row in gvDetails.Rows)
                {
                    Button btnView = ((Button)row.FindControl("btnView")) as Button;
                    btnView.Enabled = true;
                }
            }
            else if (UserRole.ToUpper().Equals("VMIS_MISUSER"))
            {
                objCommonFunction.EnableAllControls(this.Page);
                objCommonFunction.disableControlsTextBox(txtDealingOfficerRemarks);
            }
        }

        public void funcBindMasterDropdownList(string VIEW, string CIRCLE)
        {
            DataSet dsMaster = new DataSet();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spSanctionForProsecution_Ddl]";
                cmd.Parameters.AddWithValue("@p_VIEW", VIEW);
                cmd.Parameters.AddWithValue("@p_CIRCLE", CIRCLE);

                cmd.CommandTimeout = 0;
                sda.Fill(dsMaster);

                if (dsMaster != null)
                {
                    if (VIEW.Equals("DEFAULT"))
                    {
                        objCommonFunction.bindDropdownList(ddlCircle, dsMaster.Tables[0]);
                        objCommonFunction.bindDropdownList(ddlStatus, dsMaster.Tables[2]);
                        objCommonFunction.bindDropdownList(ddlLetterSentTo, dsMaster.Tables[3]);
                    }

                    else if (VIEW.Equals("BRANCH"))
                    {
                        objCommonFunction.bindDropdownList(ddlBranch, dsMaster.Tables[0]);
                    }
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

        public void funcShow(string VIEW, string UNIQUEID, string SINO, string RCNO)
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
                cmdView.CommandText = "[dbo].[spSanctionForProsecution_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_VIEW", VIEW);
                cmdView.Parameters.AddWithValue("@p_UNIQUEID", UNIQUEID);
                cmdView.Parameters.AddWithValue("@p_SINO", SINO);
                cmdView.Parameters.AddWithValue("@p_RCNO", RCNO);
                cmdView.Parameters.AddWithValue("@p_USERID", Session["userid"].ToString());
                cmdView.Parameters.AddWithValue("@p_USERROLE", Session["role"].ToString());

                cmdView.CommandTimeout = 0;
                sda.Fill(dt);
                ViewState["DETAILDATA"] = dt;

                strErrMsg = sqlErrMsgOutput.Value.ToString();
                intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                if (dt.Rows.Count > 0)
                {
                    if (VIEW.Equals("LIST"))
                    {
                        gvDetails.DataSource = dt;
                        gvDetails.DataBind();
                    }
                    if (VIEW.Equals("SEARCH"))
                    {
                        gvDetails.DataSource = dt;
                        gvDetails.DataBind();
                    }
                    if (VIEW.Equals("GET"))
                    {
                        hidUniqueID.Value = Convert.ToString(dt.Rows[0]["SFP_UNIQUEID"]);
                        txtSPNumber.Text = Convert.ToString(dt.Rows[0]["SFP_SPNO"]);
                        txtRCNumber.Text = Convert.ToString(dt.Rows[0]["SFP_RCNO"]);
                        txtRCDate.Text = Convert.ToString(dt.Rows[0]["RCDATE"]);
                        txtReportDate.Text = Convert.ToString(dt.Rows[0]["REPORTDATE"]);
                        txtPFNumber.Text = Convert.ToString(dt.Rows[0]["SFP_PFNO"]);
                        txtName.Text = Convert.ToString(dt.Rows[0]["SFP_NAME"]);
                        txtDOR.Text = Convert.ToString(dt.Rows[0]["DOR"]);
                        txtDesignation.Text = Convert.ToString(dt.Rows[0]["SFP_DESIGNATION"]);

                        objCommonFunction.ddlSetDataValue(ddlCircle, Convert.ToString(dt.Rows[0]["SFP_CIRCLE"]));
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["SFP_CIRCLE"])))
                        {
                            funcBindMasterDropdownList("BRANCH", Convert.ToString(dt.Rows[0]["SFP_CIRCLE"]));
                            objCommonFunction.ddlSetDataValue(ddlBranch, Convert.ToString(dt.Rows[0]["SFP_BRANCH"]));
                        }

                        txtDA.Text = Convert.ToString(dt.Rows[0]["SFP_DA"]);
                        objCommonFunction.ddlSetDataValue(ddlDAView, Convert.ToString(dt.Rows[0]["SFP_DA_VIEW"]));
                        txtLetterToCBIDate.Text = Convert.ToString(dt.Rows[0]["LETTERTOCBIDATE"]);
                        txtLetterToCVCDate.Text = Convert.ToString(dt.Rows[0]["LETTERTOCVCDATE"]);
                        objCommonFunction.ddlSetDataValue(ddlCVCView, Convert.ToString(dt.Rows[0]["SFP_CVC_VIEW"]));
                        txtLetterToDADate.Text = Convert.ToString(dt.Rows[0]["LETTERTODADATE"]);
                        txtDAOrderToCBIDate.Text = Convert.ToString(dt.Rows[0]["DAORDERTOCBIDATE"]);
                        objCommonFunction.ddlSetDataValue(ddlStatus, Convert.ToString(dt.Rows[0]["SFP_STATUS"]));
                        txtRemarks.Text = Convert.ToString(dt.Rows[0]["SFP_REMARKS"]);
                        txtDealingOfficerRemarks.Text = Convert.ToString(dt.Rows[0]["SFP_DESK_REMARKS"]);
                        txtAccountName.Text = Convert.ToString(dt.Rows[0]["SFP_ACCOUNT_NAME"]);
                        objCommonFunction.ddlSetDataValue(ddlBankName, Convert.ToString(dt.Rows[0]["BANKNAME"]));

                        txtLetterSentDate.Text = Convert.ToString(dt.Rows[0]["LETTERSENTDATE"]);
                        txtReminderDate.Text = Convert.ToString(dt.Rows[0]["REMINDERDATE"]);
                        txtReplyReceivedDate.Text = Convert.ToString(dt.Rows[0]["REPLYRECEIVEDDATE"]);
                        objCommonFunction.ddlSetDataValue(ddlLetterSentTo, Convert.ToString(dt.Rows[0]["LETTERSENTTO"]));
                        objCommonFunction.ddlSetDataValue(ddlCBIEOW, Convert.ToString(dt.Rows[0]["SFP_CBIEOW"]));

                        tabMain.ActiveTabIndex = 0;
                        btnSubmit.Visible = false;
                        btnUpdate.Visible = true;
                    }

                    funcControlsUserRights();
                }
                else
                {
                    lblMsg.Text = strErrMsg.ToString();
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

        public void funcSave(string MODE, string UPDATEID)
        {
            SqlConnection conSave = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();
            try
            {
                if (MODE.Equals("I"))
                {
                    string ID = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                    UNIQUENO = "SFP" + DateTime.Now.ToString("ddMMyy") + ID;
                }
                else
                {
                    UNIQUENO = UPDATEID;
                }

                CIRCLE = objCommonFunction.ddlSelectedValue(ddlCircle);
                BRANCH = objCommonFunction.ddlSelectedValue(ddlBranch);
                DAVIEW = objCommonFunction.ddlSelectedValue(ddlDAView);
                CVCVIEW = objCommonFunction.ddlSelectedValue(ddlCVCView);
                STATUS = objCommonFunction.ddlSelectedValue(ddlStatus);
                BANKNAME = objCommonFunction.ddlSelectedValue(ddlBankName);
                LETTERSENTTO = objCommonFunction.ddlSelectedValue(ddlLetterSentTo);

                string RCDate = txtRCDate.Text.Trim();
                if (!string.IsNullOrEmpty(RCDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(RCDate, out date))
                        RCDATE = date;
                }

                string DORDate = txtDOR.Text.Trim();
                if (!string.IsNullOrEmpty(DORDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(DORDate, out date))
                        DOR = date;
                }

                string ReportDate = txtReportDate.Text.Trim();
                if (!string.IsNullOrEmpty(ReportDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(ReportDate, out date))
                        REPORTDATE = date;
                }

                string LetterToCBIDate = txtLetterToCBIDate.Text.Trim();
                if (!string.IsNullOrEmpty(LetterToCBIDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(LetterToCBIDate, out date))
                        LETTERTOCBIDATE = date;
                }

                string LetterToCVCDate = txtLetterToCVCDate.Text.Trim();
                if (!string.IsNullOrEmpty(LetterToCVCDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(LetterToCVCDate, out date))
                        LETTERTOCVCDATE = date;
                }

                string LetterToDADate = txtLetterToDADate.Text.Trim();
                if (!string.IsNullOrEmpty(LetterToDADate))
                {
                    DateTime date;
                    if (DateTime.TryParse(LetterToDADate, out date))
                        LETTERTODADATE = date;
                }

                string DAOrderToCBIDate = txtDAOrderToCBIDate.Text.Trim();
                if (!string.IsNullOrEmpty(DAOrderToCBIDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(DAOrderToCBIDate, out date))
                        DAORDERTOCBIDATE = date;
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
                cmdSave.CommandText = "[dbo].[spSanctionForProsecution]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_UNIQUENO", UNIQUENO);
                cmdSave.Parameters.AddWithValue("@p_SPNO", txtSPNumber.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_RCNO", txtRCNumber.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PFNO", txtPFNumber.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NAME", txtName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DOR", DOR);
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", txtDesignation.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CIRCLE", CIRCLE);
                cmdSave.Parameters.AddWithValue("@p_BRANCH", BRANCH);
                cmdSave.Parameters.AddWithValue("@p_DA", txtDA.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DAVIEW", DAVIEW);
                cmdSave.Parameters.AddWithValue("@p_CVCVIEW", CVCVIEW);
                cmdSave.Parameters.AddWithValue("@p_STATUS", STATUS);
                cmdSave.Parameters.AddWithValue("@p_REMARKS", txtRemarks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DESK_REMARKS", txtDealingOfficerRemarks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_RCDATE", RCDATE);
                cmdSave.Parameters.AddWithValue("@p_REPORTDATE", REPORTDATE);
                cmdSave.Parameters.AddWithValue("@p_LETTERTOCBIDATE", LETTERTOCBIDATE);
                cmdSave.Parameters.AddWithValue("@p_LETTERTOCVCDATE", LETTERTOCVCDATE);
                cmdSave.Parameters.AddWithValue("@p_LETTERTODADATE", LETTERTODADATE);
                cmdSave.Parameters.AddWithValue("@p_DAORDERTOCBIDATE", DAORDERTOCBIDATE);
                cmdSave.Parameters.AddWithValue("@p_ACCOUNTNAME", txtAccountName.Text.Trim());

                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTO", LETTERSENTTO);
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTDATE", dtLETTERSENTDATE);
                cmdSave.Parameters.AddWithValue("@p_REMINDERDATE", dtREMINDERDATE);
                cmdSave.Parameters.AddWithValue("@p_REPLYRECEIVEDDATE", dtREPLYRECEIVEDDATE);
                cmdSave.Parameters.AddWithValue("@p_CBIEOW", objCommonFunction.ddlSelectedValue(ddlCBIEOW));

                cmdSave.Parameters.AddWithValue("@p_USER", Session["userid"].ToString());
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                cmdSave.Parameters.AddWithValue("@p_USERROLE", Session["role"].ToString());
                cmdSave.Parameters.AddWithValue("@p_BANKNAME", BANKNAME);


                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    lblMsg.Text = Server.HtmlEncode(sqlErrMsgOutput.Value.ToString());
                    funcClear();
                    funcShow("LIST", null, null, null);
                }
                else
                {
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == 2)
                    {
                        lblMsg.Text = Server.HtmlEncode(sqlErrMsgOutput.Value.ToString());
                    }
                    else
                    {
                        lblMsg.Text = Server.HtmlEncode("Error - Insert/ Update Sanction For Prosecution, please contact to Administrator");
                    }
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

        public void funcClear()
        {
            txtSPNumber.Text = ""; txtRCNumber.Text = ""; txtRCDate.Text = ""; txtReportDate.Text = "";
            txtPFNumber.Text = ""; txtName.Text = ""; txtDOR.Text = ""; txtDesignation.Text = "";
            ddlCircle.SelectedIndex = 0; txtDA.Text = ""; ddlDAView.SelectedIndex = 0;
            txtLetterToCBIDate.Text = ""; txtLetterToCVCDate.Text = ""; ddlCVCView.SelectedIndex = 0;
            txtLetterToDADate.Text = ""; txtDAOrderToCBIDate.Text = ""; ddlStatus.SelectedIndex = 0;
            txtRemarks.Text = ""; txtDealingOfficerRemarks.Text = ""; txtAccountName.Text = "";
            ddlBranch.Items.Clear();
            ddlBankName.SelectedIndex = 0; ddlCBIEOW.SelectedIndex = 0;

            ddlLetterSentTo.SelectedIndex = 0;
            txtLetterSentDate.Text = "";
            txtReminderDate.Text = "";
            txtReplyReceivedDate.Text = "";

            btnSubmit.Visible = true; btnUpdate.Visible = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (funcValidation() == true)
            {
                funcSave("I", null);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (funcValidation() == true)
            {
                funcSave("U", hidUniqueID.Value);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            funcClear();
            lblMsg.Text = "";
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CIRCLE = objCommonFunction.ddlSelectedValue(ddlCircle);

            if (CIRCLE != "0")
            {
                funcBindMasterDropdownList("BRANCH", CIRCLE);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(txtSPNO_LIST.Text.Trim())) || (!string.IsNullOrEmpty(txtRCNO_LIST.Text.Trim())))
            {
                funcShow("SEARCH", null, txtSPNO_LIST.Text.Trim(), txtRCNO_LIST.Text.Trim());
            }
            else
            {
                funcShow("LIST", null, null, null);
            }
        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper().Equals("VIEW"))
            {
                UNIQUENO = e.CommandArgument.ToString();

                if (!string.IsNullOrEmpty(UNIQUENO))
                {
                    funcShow("GET", UNIQUENO, null, null);
                }
            }
        }

        private Boolean funcValidation()
        {
            Boolean Result = true;
            lblMsg.Text = "";

            if (string.IsNullOrEmpty(txtSPNumber.Text.Trim()))
            {
                lblMsg.Text = "Please enter SP Number.";
                return Result = false;
            }

            if (string.IsNullOrEmpty(txtRCNumber.Text.Trim()))
            {
                lblMsg.Text = "Please enter RC Number.";
                return Result = false;
            }

            if (string.IsNullOrEmpty(txtRCDate.Text.Trim()))
            {
                lblMsg.Text = "Please enter RC Date.";
                return Result = false;
            }

            if (string.IsNullOrEmpty(txtReportDate.Text.Trim()))
            {
                lblMsg.Text = "Please enter Date of Report Received.";
                return Result = false;
            }

            if (string.IsNullOrEmpty(txtPFNumber.Text.Trim()))
            {
                lblMsg.Text = "Please enter PF Number.";
                return Result = false;
            }

            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                lblMsg.Text = "Please enter Name.";
                return Result = false;
            }

            if (string.IsNullOrEmpty(txtDesignation.Text.Trim()))
            {
                lblMsg.Text = "Please enter Designation.";
                return Result = false;
            }

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlCircle)))
            {
                lblMsg.Text = "Please select Circle.";
                return Result = false;
            }

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlBranch)))
            {
                lblMsg.Text = "Please select Branch.";
                return Result = false;
            }

            if (string.IsNullOrEmpty(txtDA.Text.Trim()))
            {
                lblMsg.Text = "Please select DA.";
                return Result = false;
            }

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlCBIEOW)))
            {
                lblMsg.Text = "CBI/EOW Request for.";
                return Result = false;
            }

            if (string.IsNullOrEmpty(txtRemarks.Text.Trim()))
            {
                lblMsg.Text = "Please enter MIS User Remarks.";
                return Result = false;
            }

            if (hidUserRole.Value.Equals("VMIS_DESKUSER"))
            {
                if (string.IsNullOrEmpty(txtDealingOfficerRemarks.Text.Trim()))
                {
                    lblMsg.Text = "Please enter Desk User Dealing Officer Remarks...!";
                    return false;
                }
            }

            return Result;
        }
    }
}