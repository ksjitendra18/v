using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class SR : System.Web.UI.Page
    {
        DateTime? dtSRDATE = null;
        DateTime? dtCLOSUREDT = null;
        DateTime? dtRECDATECOMP = null;
        DateTime? dtLETTERSENTDATE = null;
        DateTime? dtREMINDERDATE = null;
        DateTime? dtREPLYRECEIVEDDATE = null;
        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                funcShow(null, "LIST", null); //for bind grid view on form Load
                funcbindDropdown();     //Bind All DropDown Lists                
            }

            funcControlsUserRights();

            btnSubmit.Attributes.Add("onclick", "return funcValidation_SR('" + txtSRNo.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            btnUpdate.Attributes.Add("onclick", "return funcValidation_SR('" + txtSRNo.ClientID + "','" + ddlCircleOffice.ClientID + "')");
            txtAmount.Attributes.Add("onkeypress", "return isNumbericDecimal(event,'" + txtAmount.ClientID + "')");
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
                cmd.CommandText = "[dbo].[spSR_Ddl]";
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

                string strSRDATE = txtSRDate.Text.Trim();
                if (!string.IsNullOrEmpty(strSRDATE))
                {
                    DateTime date;
                    if (DateTime.TryParse(strSRDATE, out date))
                        dtSRDATE = date;
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
                cmdSave.CommandText = "[dbo].[spSRStructure_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_CODE", objCommonFunction.convertToIntToolTip(txtSRNo));
                cmdSave.Parameters.AddWithValue("@p_SRNO", txtSRNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BRCOMPLAINT", txtBRComplaint.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_RNO", txtRNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ACCUSED", txtAccused.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ALLEGATIONS", txtAllegations.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_AMOUNT", objCommonFunction.convertToDecimal(txtAmount));
                cmdSave.Parameters.AddWithValue("@p_REGION", txtRegion.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_FINALACTION", txtFinalAction.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PRESENTPOSTING", txtPresentPosting.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ACCOUNTNAME", txtAccountName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CASECLOSE", txtClose.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", txtDesignation.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_INVESTIGATION", txtInvestigation.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_STATUS", txtStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_HOSTATUS", txtHOStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DESK_USER_REMARKS", txtDealingOfficerRemarks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CIRCLEOFFICE", objCommonFunction.ddlSelectedText(ddlCircleOffice));
                cmdSave.Parameters.AddWithValue("@p_STATUSCODE", objCommonFunction.ddlSelectedValue(ddlStatusCode));
                cmdSave.Parameters.AddWithValue("@p_NATURE", objCommonFunction.ddlSelectedValue(ddlNature));
                cmdSave.Parameters.AddWithValue("@p_BANKNAME", objCommonFunction.ddlSelectedValue(ddlBankName));
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTO", objCommonFunction.ddlSelectedValue(ddlLetterSentTo));
                cmdSave.Parameters.AddWithValue("@p_ZONENEW", objCommonFunction.ddlSelectedValue(ddlZoneNew));
                cmdSave.Parameters.AddWithValue("@p_CIRCLENEW", objCommonFunction.ddlSelectedValue(ddlCircleNew));
                cmdSave.Parameters.AddWithValue("@p_ZONE", objCommonFunction.ddlSelectedText(ddlZone));
                cmdSave.Parameters.AddWithValue("@p_CLOSURE", strCLOSURE);
                cmdSave.Parameters.AddWithValue("@p_SRDATE", dtSRDATE);
                cmdSave.Parameters.AddWithValue("@p_CLOSUREDT", dtCLOSUREDT);
                cmdSave.Parameters.AddWithValue("@p_RECDATECOMP", dtRECDATECOMP);
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTDATE", dtLETTERSENTDATE);
                cmdSave.Parameters.AddWithValue("@p_REMINDERDATE", dtREMINDERDATE);
                cmdSave.Parameters.AddWithValue("@p_REPLYRECEIVEDDATE", dtREPLYRECEIVEDDATE);
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
                conSave.Close();
                conSave.Dispose();
            }
        }

        public void funcShow(string NO, string VIEW, string CIRCLEOFFICE)
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
                cmd.CommandText = "[dbo].[spSRStructure_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(sqlErrMsgOutput);
                cmd.Parameters.Add(sqlErrCodeOutput);

                cmd.Parameters.AddWithValue("@p_SEARCHNO", NO);
                cmd.Parameters.AddWithValue("@p_VIEW", VIEW);
                cmd.Parameters.AddWithValue("@p_CIRCLEOFFICE", CIRCLEOFFICE);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                ViewState["DETAILDATA"] = dt;


                if (dt.Rows.Count > 0)
                {
                    if (VIEW.ToUpper() == "LIST")
                    {
                        gvMain.DataSource = dt;
                        gvMain.DataBind();
                    }
                    else if (VIEW.ToUpper() == "SEARCH")
                    {
                        gvMain.DataSource = dt;
                        gvMain.DataBind();
                        tabMain.ActiveTabIndex = 1;
                    }
                    else if (VIEW.ToUpper() == "GET")
                    {
                        funcBindControl(dt);
                    }
                    else if (VIEW.ToUpper() == "VIEW")
                    {
                        funcBindControl(dt);
                    }

                    funcControlsUserRights();
                }
                else
                {
                    lblMsg.Text = "Record Not Found";
                    funcClear();
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

        public void funcBindControl(DataTable dtData)
        {
            tabMain.ActiveTabIndex = 0;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;

            txtSRNo.ToolTip = Convert.ToString(dtData.Rows[0]["CODE"]);
            txtSRNo.Text = Convert.ToString(dtData.Rows[0]["SRNO"]);
            txtSRDate.Text = Convert.ToString(dtData.Rows[0]["SRDATE"]);
            txtBRComplaint.Text = Convert.ToString(dtData.Rows[0]["BRCOMPLAINT"]);
            objCommonFunction.ddlSetData(ddlCircleOffice, Convert.ToString(dtData.Rows[0]["CIRCLEOFFICE"]), true);
            txtRNo.Text = Convert.ToString(dtData.Rows[0]["RNO"]);
            txtCompRecDate.Text = Convert.ToString(dtData.Rows[0]["COMPRECDATE"]);
            txtAccused.Text = Convert.ToString(dtData.Rows[0]["ACCUSED"]);
            txtAllegations.Text = Convert.ToString(dtData.Rows[0]["ALLEGATIONS"]);
            txtAmount.Text = Convert.ToString(dtData.Rows[0]["AMOUNT"]);
            txtFinalAction.Text = Convert.ToString(dtData.Rows[0]["FINALACTION"]);
            objCommonFunction.ddlSetData(ddlZone, Convert.ToString(dtData.Rows[0]["ZONE"]), true);
            txtRegion.Text = Convert.ToString(dtData.Rows[0]["REGION"]);
            txtPresentPosting.Text = Convert.ToString(dtData.Rows[0]["PRESENTPOSTING"]);
            txtAccountName.Text = Convert.ToString(dtData.Rows[0]["ACCOUNTNAME"]);
            txtClose.Text = Convert.ToString(dtData.Rows[0]["CASECLOSE"]);
            txtInvestigation.Text = Convert.ToString(dtData.Rows[0]["INVESTIGATION"]);
            txtDesignation.Text = Convert.ToString(dtData.Rows[0]["DESIGNATION"]);
            txtStatus.Text = Convert.ToString(dtData.Rows[0]["STATUS"]);

            objCommonFunction.ddlSetDataValue(ddlStatusCode, Convert.ToString(dtData.Rows[0]["STATUSCODE"]));
            if (objCommonFunction.ddlSelectedValue(ddlStatusCode) == "0")
            {
                lblStatusCodeMIS.Text = Convert.ToString(dtData.Rows[0]["STATUSCODE"]);
            }

            objCommonFunction.ddlSetData(ddlNature, Convert.ToString(dtData.Rows[0]["NATURE"]), true);
            if (objCommonFunction.ddlSelectedValue(ddlNature) == "0")
            {
                lblNatureMIS.Text = Convert.ToString(dtData.Rows[0]["NATURE"]);
                pnlNatureMIS.Visible = true;
            }
            objCommonFunction.chkSetData(chkClosureDate, Convert.ToString(dtData.Rows[0]["CLOSURE"]));
            txtDealingOfficerRemarks.Text = Convert.ToString(dtData.Rows[0]["DESK_USER_REMARKS"]);
            objCommonFunction.ddlSetDataValue(ddlBankName, Convert.ToString(dtData.Rows[0]["BANKNAME"]));


            lblClosureDate.Text = Convert.ToString(dtData.Rows[0]["CLOSUREDATE"]);
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
            txtSRNo.ToolTip = string.Empty;
            txtSRNo.Text = string.Empty;
            txtSRDate.Text = string.Empty;
            txtBRComplaint.Text = string.Empty;
            txtDealingOfficerRemarks.Text = "";
            txtRNo.Text = string.Empty;
            txtClosureDate.Text = string.Empty;
            txtAccused.Text = string.Empty;
            txtAllegations.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtCompRecDate.Text = string.Empty;
            txtFinalAction.Text = string.Empty;
            txtRegion.Text = string.Empty;
            txtPresentPosting.Text = string.Empty;
            txtAccountName.Text = string.Empty;
            txtClose.Text = string.Empty;
            lblStatusCodeMIS.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            txtInvestigation.Text = string.Empty;
            txtStatus.Text = string.Empty;
            txtHOStatus.Text = string.Empty;

            ddlStatusCode.SelectedIndex = 0;
            ddlCircleOffice.SelectedIndex = 0;
            ddlZone.SelectedIndex = 0;
            ddlNature.SelectedIndex = 0;
            lblNatureMIS.Text = string.Empty;
            chkClosureDate.Checked = false;
            lblClosureDate.Text = string.Empty;

            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            pnlNatureMIS.Visible = false;
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

            funcControlsUserRights();
        }

        public void funcreadOnly()
        {
            objCommonFunction.disableControlsTextBox(txtBRComplaint);
            objCommonFunction.disableControlsTextBox(txtRNo);
            objCommonFunction.disableControlsTextBox(txtAccused);
            objCommonFunction.disableControlsTextBox(txtAllegations);
            objCommonFunction.disableControlsTextBox(txtRegion);
            objCommonFunction.disableControlsTextBox(txtFinalAction);
            objCommonFunction.disableControlsTextBox(txtPresentPosting);
            objCommonFunction.disableControlsTextBox(txtInvestigation);
            objCommonFunction.disableControlsTextBox(txtAccountName);
            objCommonFunction.disableControlsTextBox(txtAmount);
            objCommonFunction.disableControlsTextBox(txtDesignation);
            objCommonFunction.disableControlsTextBox(txtClose);
            objCommonFunction.disableControlsTextBox(txtStatus);

            objCommonFunction.disableControlsDropDownList(ddlCircleOffice);
            objCommonFunction.disableControlsDropDownList(ddlZone);
            objCommonFunction.disableControlsDropDownList(ddlStatusCode);
            objCommonFunction.disableControlsDropDownList(ddlNature);

            chkClosureDate.Enabled = false;
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
                txtCircleOffice_LIST.Enabled = true;
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
                txtCircleOffice_LIST.Enabled = true;
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
                lblMsg.Text = "Please Enter SR Number.";
                return;
            }

            funcShow(txtSRNo.Text.Trim(), "GET", null);
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Convert.ToString(e.CommandName).ToUpper() == "VIEW")
            {
                if (!string.IsNullOrEmpty(Convert.ToString(e.CommandArgument)))
                {
                    funcShow(Convert.ToString(e.CommandArgument), "VIEW", null);
                }
            }
        }

        protected void tabMain_ActiveTabChanged(object sender, EventArgs e)
        {
            if (tabMain.ActiveTab == tabList)
            {
                funcShow(null, "LIST", null); //for bind grid view on List Tab Load
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

            if (string.IsNullOrEmpty(txtRNo_LIST.Text.Trim()) && string.IsNullOrEmpty(txtCircleOffice_LIST.Text.Trim()))
            {
                VIEW = "LIST";
            }

            funcShow(txtRNo_LIST.Text.Trim(), VIEW, txtCircleOffice_LIST.Text.Trim());
        }
    }
}