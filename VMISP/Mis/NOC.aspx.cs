using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class NOC : System.Web.UI.Page
    {
        DateTime? dtCLEARANCEDT = null;
        DateTime? dtLETTERSENTDATE = null;
        DateTime? dtREPLYRECEIVEDDATE = null;
        DateTime? dtDOR = null;
        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                funcShow(null, "LIST", null, null); //for bind grid view on form Load
                funcbindDropdown();     //Bind Circle Office DropDown List
            }

            funcControlsUserRights();
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
                cmd.CommandText = "[dbo].[spNOC_Ddl]";
                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    objCommonFunction.bindDropdownList_SELECT(ddlScale, ds.Tables[2]);
                    objCommonFunction.bindDropdownList(ddlLetterSentTo, ds.Tables[3]);
                    objCommonFunction.bindDropdownList(ddlZoneNew, ds.Tables[4]);
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
            string REFNO = "";
            try
            {
                if (MODE.Equals("I"))
                {
                    string ID = Guid.NewGuid().ToString("N").Substring(0, 3).ToUpper();
                    REFNO = "NOC" + DateTime.Now.ToString("ddMMyy") + ID;
                }

                string strClearanceDate = txtClearanceDate.Text.Trim();
                if (!string.IsNullOrEmpty(strClearanceDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strClearanceDate, out date))
                        dtCLEARANCEDT = date;
                }

                string strLetterSentDate = txtLetterSentDate.Text.Trim();
                if (!string.IsNullOrEmpty(strLetterSentDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strLetterSentDate, out date))
                        dtLETTERSENTDATE = date;
                }

                string strReplyReceivedDate = txtReplyReceivedDate.Text.Trim();
                if (!string.IsNullOrEmpty(strReplyReceivedDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strReplyReceivedDate, out date))
                        dtREPLYRECEIVEDDATE = date;
                }

                string strDOR = txtDOR.Text.Trim();
                if (!string.IsNullOrEmpty(strDOR))
                {
                    DateTime date;
                    if (DateTime.TryParse(strDOR, out date))
                        dtDOR = date;
                }

                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spNOC_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_CODE", objCommonFunction.convertToIntToolTip(txtSNo));
                cmdSave.Parameters.AddWithValue("@p_SNO", txtSNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BRCOMPLAINT", txtBRComplaint.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PFNO", txtPFNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CLEARANCEDT", dtCLEARANCEDT);
                cmdSave.Parameters.AddWithValue("@p_NAME", txtName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DESIGNATION", txtDesignation.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_SCALE", objCommonFunction.ddlSelectedValue_Scale(ddlScale));
                cmdSave.Parameters.AddWithValue("@p_REMARKS", txtRemarks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_HOREMARKS", txtHORemarks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DESK_USER_REMARKS", txtDealingOfficerRemarks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BANKNAME", objCommonFunction.ddlSelectedValue(ddlBankName));
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTTO", objCommonFunction.ddlSelectedValue(ddlLetterSentTo));
                cmdSave.Parameters.AddWithValue("@p_LETTERSENTDATE", dtLETTERSENTDATE);
                cmdSave.Parameters.AddWithValue("@p_REPLYRECEIVEDDATE", dtREPLYRECEIVEDDATE);
                cmdSave.Parameters.AddWithValue("@p_ZONENEW", objCommonFunction.ddlSelectedValue(ddlZoneNew));
                cmdSave.Parameters.AddWithValue("@p_CIRCLENEW", objCommonFunction.ddlSelectedValue(ddlCircleNew));
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["userid"]));
                cmdSave.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());

                cmdSave.Parameters.AddWithValue("@p_DOR", dtDOR);
                cmdSave.Parameters.AddWithValue("@p_EMPSOLID", txtSolID.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ACTIVESTATUS", txtActiveStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_REFNO", REFNO);
                cmdSave.Parameters.AddWithValue("@p_REASON", objCommonFunction.ddlSelectedValue(ddlReason)); ;

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
                    funcClear("SAVE");
                }
                else
                {
                    lblMsg.Text = "NOC Insert/ Update Failed.";
                }
            }
            catch (Exception es)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }
            finally
            {
                cmdSave.Clone();
                conSave.Dispose();
                conSave.Close();
            }
        }

        public void funcShow(string p_strNo, string p_strView, string p_strPFNO, string p_strNAME)
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
                cmdView.CommandText = "[dbo].[spNOC_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_VIEW", p_strView);
                cmdView.Parameters.AddWithValue("@p_PFNO", p_strPFNO);
                cmdView.Parameters.AddWithValue("@p_NAME", p_strNAME);

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
                    funcClear("SHOW");
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

            txtSNo.ToolTip = Convert.ToString(dtData.Rows[0]["CODE"]);
            txtSNo.Text = Convert.ToString(dtData.Rows[0]["SNO"]);
            txtBRComplaint.Text = Convert.ToString(dtData.Rows[0]["BRCOMPLAINT"]);
            txtPFNo.Text = Convert.ToString(dtData.Rows[0]["PFNO"]);
            txtClearanceDate.Text = Convert.ToString(dtData.Rows[0]["CLOSUREDATE"]);
            txtName.Text = Convert.ToString(dtData.Rows[0]["NAME"]);
            txtDesignation.Text = Convert.ToString(dtData.Rows[0]["DESIGNATION"]);
            objCommonFunction.ddlSetDataValue_Scale(ddlScale, Convert.ToString(dtData.Rows[0]["SCLAECODE"]));
            txtRemarks.Text = Convert.ToString(dtData.Rows[0]["REMARKS"]);
            txtDealingOfficerRemarks.Text = Convert.ToString(dtData.Rows[0]["DESK_USER_REMARKS"]);
            objCommonFunction.ddlSetDataValue(ddlBankName, Convert.ToString(dtData.Rows[0]["BANKNAME"]));

            txtLetterSentDate.Text = Convert.ToString(dtData.Rows[0]["LETTERSENTDATE"]);
            txtReplyReceivedDate.Text = Convert.ToString(dtData.Rows[0]["REPLYRECEIVEDDATE"]);
            objCommonFunction.ddlSetDataValue(ddlLetterSentTo, Convert.ToString(dtData.Rows[0]["LETTERSENTTO"]));
            hidLetterSentTo.Value = Convert.ToString(dtData.Rows[0]["LETTERSENTTO"]);

            txtDOR.Text = Convert.ToString(dtData.Rows[0]["EMPDOR"]);
            txtSolID.Text = Convert.ToString(dtData.Rows[0]["NOC_EMP_SOLID"]);
            txtActiveStatus.Text = Convert.ToString(dtData.Rows[0]["NOC_EMP_STATUS"]);
            txtActiveStatus.ToolTip = Convert.ToString(dtData.Rows[0]["NOC_REFNO"]);
            objCommonFunction.ddlSetDataValue(ddlReason, Convert.ToString(dtData.Rows[0]["NOC_REASON"]));

            objCommonFunction.ddlSetDataValue(ddlZoneNew, Convert.ToString(dtData.Rows[0]["NEWZONE"]));
            string ZONE = Convert.ToString(dtData.Rows[0]["NEWZONE"]);
            if (!string.IsNullOrEmpty(ZONE))
            {
                objCommonFunction.funcZoneCircleMaster(ddlCircleNew, ZONE);
                objCommonFunction.ddlSetDataValue(ddlCircleNew, Convert.ToString(dtData.Rows[0]["NEWCIRCLE"]));
            }
        }

        public void funcClear(string VIEW)
        {
            if (VIEW != "EMP")
            {
                txtPFNo.Text = string.Empty;
            }

            txtSNo.ToolTip = string.Empty;
            txtSNo.Text = string.Empty;
            txtBRComplaint.Text = string.Empty;
            txtClearanceDate.Text = string.Empty;
            txtName.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            ddlScale.SelectedIndex = 0;
            txtRemarks.Text = string.Empty;
            txtHORemarks.Text = string.Empty;
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            txtDealingOfficerRemarks.Text = "";
            ddlBankName.SelectedIndex = 0;
            funcControlsUserRights();

            ddlLetterSentTo.SelectedIndex = 0;
            ddlReason.SelectedIndex = 0;
            txtLetterSentDate.Text = "";
            txtReplyReceivedDate.Text = "";
            hidLetterSentTo.Value = "";
            txtDOR.Text = "";
            txtSolID.Text = "";
            txtActiveStatus.Text = "";
            txtActiveStatus.ToolTip = "";
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
                txtPFNumber_LIST.Enabled = true;
                txtName_LIST.Enabled = true;
            }
            else if (Convert.ToString(Session["role"]).ToUpper() == "VMIS_DESKUSER")
            {
                objCommonFunction.DisableAllControls(this.Page);
                pnlHOStatus.Visible = true;
                txtDealingOfficerRemarks.Enabled = true;
                txtHORemarks.Enabled = true;
                btnSubmit.Visible = false;
                btnUpdate.Visible = true;
                btnUpdate.Enabled = true;
                btnCancel.Visible = false;

                txtRNo_LIST.Enabled = true;
                txtPFNumber_LIST.Enabled = true;
                txtName_LIST.Enabled = true;

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
            lblMsg.Text = "";
            if (funcValidation() == true)
            {
                funcSave("I");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (funcValidation() == true)
            {
                funcSave("U");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            funcClear("CANCEL");
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (string.IsNullOrEmpty(txtSNo.Text.Trim()))
            {
                lblMsg.Text = "Please enter S No.";
                return;
            }

            funcShow(txtSNo.Text.Trim(), "GET", null, null);
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "VIEW")
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(e.CommandArgument)))
                    {
                        funcShow(Convert.ToString(e.CommandArgument), "VIEW", null, null);
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
            dtSorting.DefaultView.Sort = e.SortExpression;
            gvMain.DataSource = dtSorting;
            gvMain.DataBind();
        }

        protected void tabMain_ActiveTabChanged(object sender, EventArgs e)
        {
            if (tabMain.ActiveTab == tabList)
            {
                funcShow(null, "LIST", null, null); //for bind grid view on form Load
            }

            //Code hereTabContainer
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
            string VIEW = "SEARCH";

            if (string.IsNullOrEmpty(txtRNo_LIST.Text.Trim()) && string.IsNullOrEmpty(txtPFNumber_LIST.Text.Trim()) && string.IsNullOrEmpty(txtName_LIST.Text.Trim()))
            {
                VIEW = "LIST";
            }

            funcShow(txtRNo_LIST.Text.Trim(), VIEW, txtPFNumber_LIST.Text.Trim(), txtName_LIST.Text.Trim());
        }

        protected void btnGetEmpDetails_Click(object sender, EventArgs e)
        {
            //funcClear("EMP");

            if (string.IsNullOrEmpty(txtPFNo.Text))
            {
                lblMsg.Text = "Please enter Employee PF Number.";
                return;
            }
            else
            {
                funcHRMSEmployee();
            }
        }

        private Boolean funcValidation()
        {
            if (string.IsNullOrEmpty(txtSNo.Text))
            {
                lblMsg.Text = "Please enter S Number.";
                return false;
            }

            if (string.IsNullOrEmpty(txtPFNo.Text))
            {
                lblMsg.Text = "Please enter Employee PF Number.";
                return false;
            }

            if (string.IsNullOrEmpty(txtName.Text))
            {
                lblMsg.Text = "Please enter Employee Name.";
                return false;
            }

            if (string.IsNullOrEmpty(txtDesignation.Text))
            {
                lblMsg.Text = "Please enter Employee Designation.";
                return false;
            }

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue_Scale(ddlScale)))
            {
                lblMsg.Text = "Please enter Employee Scale.";
                return false;
            }

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlZoneNew)))
            {
                lblMsg.Text = "Please select new zone.";
                return false;
            }

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlCircleNew)))
            {
                lblMsg.Text = "Please select new circle.";
                return false;
            }

            if (string.IsNullOrEmpty(txtBRComplaint.Text))
            {
                lblMsg.Text = "Please enter Branch complaint.";
                return false;
            }

            return true;
        }

        private void funcHRMSEmployee()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtPFNo.Text.Trim()))
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11;

                    string UserID = ConfigurationManager.AppSettings["CBSServiceUserID"];
                    string Password = ConfigurationManager.AppSettings["CBSServicePassword"];
                    string CBSHRMSServiceAPIUrl = ConfigurationManager.AppSettings["CBS_HRMS_SERVICE_EMP"];

                    using (HttpClient clientCBSRequest = new HttpClient())
                    {
                        HttpRequestMessage RequestCBSMessage = new HttpRequestMessage();
                        RequestCBSMessage.RequestUri = new Uri(CBSHRMSServiceAPIUrl + txtPFNo.Text.Trim());
                        RequestCBSMessage.Method = HttpMethod.Get;
                        RequestCBSMessage.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(UserID + ":" + Password)));
                        RequestCBSMessage.Headers.Add("Accept", "application/json");

                        //Send Request to CBS for Fetch Fastag Recharge Data
                        var resultCBS = clientCBSRequest.SendAsync(RequestCBSMessage).Result;

                        if (resultCBS.IsSuccessStatusCode)
                        {
                            var dataCBS = resultCBS.Content.ReadAsStringAsync().Result;
                            HRMS resultHRMS = JsonConvert.DeserializeObject<HRMS>(dataCBS);

                            txtPFNo.Text = resultHRMS.PFNumber;
                            txtPFNo.ToolTip = resultHRMS.PFNumber;
                            txtName.Text = resultHRMS.EmployeeName;
                            txtDesignation.Text = resultHRMS.Designation;
                            objCommonFunction.ddlSetDataValue_Scale(ddlScale, resultHRMS.Grade);
                            txtDOR.Text = resultHRMS.RetirementDate;
                            txtSolID.Text = "Solid " + resultHRMS.SolId + ", Branch Code " + resultHRMS.BranchCode;
                            txtActiveStatus.Text = resultHRMS.Status;

                            DataTable dtSolDetails = objCommonFunction.funcGetBranchName(resultHRMS.BranchCode);

                            if(dtSolDetails.Rows.Count > 0)
                            {
                                txtBRComplaint.Text = Convert.ToString(dtSolDetails.Rows[0]["NAME"]);

                                if (Convert.ToString(dtSolDetails.Rows[0]["BRN_TYPE"]).Equals("ZO"))
                                {
                                    objCommonFunction.ddlSetDataValue(ddlZoneNew, Convert.ToString(resultHRMS.BranchCode));
                                }

                                else if (Convert.ToString(dtSolDetails.Rows[0]["BRN_TYPE"]).Equals("CO"))
                                {
                                    objCommonFunction.ddlSetDataValue(ddlZoneNew, Convert.ToString(dtSolDetails.Rows[0]["ZONE"]));
                                    if (!string.IsNullOrEmpty(Convert.ToString(dtSolDetails.Rows[0]["ZONE"])))
                                    {
                                        objCommonFunction.funcZoneCircleMaster(ddlCircleNew, Convert.ToString(dtSolDetails.Rows[0]["ZONE"]));
                                        objCommonFunction.ddlSetDataValue(ddlCircleNew, Convert.ToString(dtSolDetails.Rows[0]["SOLID"]));
                                    }
                                }

                                else //(Convert.ToString(dtSolDetails.Rows[0]["BRN_TYPE"]).Equals("BO"))
                                {
                                    objCommonFunction.ddlSetDataValue(ddlZoneNew, Convert.ToString(dtSolDetails.Rows[0]["ZONE"]));
                                    if (!string.IsNullOrEmpty(Convert.ToString(dtSolDetails.Rows[0]["ZONE"])))
                                    {
                                        objCommonFunction.funcZoneCircleMaster(ddlCircleNew, Convert.ToString(dtSolDetails.Rows[0]["ZONE"]));
                                        objCommonFunction.ddlSetDataValue(ddlCircleNew, Convert.ToString(dtSolDetails.Rows[0]["CIRCLE"]));
                                    }
                                }
                            }
                        }
                        else
                        {
                            txtPFNo.Text = "";
                            txtPFNo.ToolTip = "";
                            txtName.Text = "";
                            txtDesignation.Text = "";
                            ddlScale.SelectedIndex = 0;
                            txtDOR.Text = "";
                            txtSolID.Text = "";
                            txtActiveStatus.Text = "";
                            ddlZoneNew.SelectedIndex = 0;
                            ddlZoneNew.SelectedIndex = 0;
                            lblMsg.Text = "Record not found.";
                        }
                    }
                }
            }
            catch (Exception exHRMS)
            {
                txtName.Text = exHRMS.Message;
            }
        }

        private static bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}