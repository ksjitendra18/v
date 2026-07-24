using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using VMISP.DataAccessLayer;
using VMISP.Models;

namespace VMISP.Mis
{
    public partial class frmABBFF : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();
        MasterData objMasterData = new MasterData();


        DateTime? dtABBFFRecDate = null;
        DateTime? dtSourceDate = null;
        DateTime? dtDtofOccurance = null;
        DateTime? dtDtofDetection = null;
        DateTime? dtDtofReporttoRBI = null;
        DateTime? dtFIRDate = null;
        DateTime? dtDtofNPA = null;
        DateTime? dtHOSACDate = null;
        DateTime? dtCaseSubmissionDate = null;
        DateTime? dtABBFFReplydt = null;
        DateTime? dtReplySenttoABBFFDt = null;
        DateTime? dtABBFFAdviceRecDate = null;
        DateTime? dtClosureDt = null;
        DateTime? dtRetirementDate_D = null;

        string strCLOSURE = string.Empty;
        string EOUNIQUEID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //  funcShow(null, "LIST", null, null, null, null, null, null, null); //for bind grid view on form Load
                    funcbindDropdown();     //Bind All DropDown Lists
                    funbinddtab2(); // Binding Details on Tab 2

                }
                lblMsgSearch.Visible = false;
                lblMsgSearch.Text = "";
                if (chkClosureDate.Checked)
                    txtClosureDt.Enabled = true;
                else
                    txtClosureDt.Enabled = false;

                if (Convert.ToString(Session["ROLE"]).Equals("VMIS_MISUSER"))
                    txtDeskUserRrmks.Enabled = false;
            }
           catch
            {

            }
        }

        protected void tabMain_ActiveTabChanged(object sender, EventArgs e)
        {

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
                    objCommonFunction.bindDropdownList(ddlNewZone, ds.Tables[6]);
                    objCommonFunction.bindDropdownList(ddlZone_List, ds.Tables[6]);
                    //objCommonFunction.bindDropdownList(ddlbr, ds.Tables[6]);
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
                cmdView.CommandText = "[dbo].[spABBFFEO_View]";

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

        public void funcSave(string MODE)
        {
            SqlConnection conSave = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();

            try
            {
                string closureChecked = "N";
                if (chkClosureDate.Checked)
                    closureChecked = "Y";
                else
                    closureChecked = "N";

                #region **Convert Date**
                string strABBFFRecDate = txtABBFFRecDate.Text.Trim();
                if (!string.IsNullOrEmpty(strABBFFRecDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strABBFFRecDate, out date))
                        dtABBFFRecDate = date;
                }

                string strSourceDate = txtSourceDate.Text.Trim();
                if (!string.IsNullOrEmpty(strSourceDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strSourceDate, out date))
                        dtSourceDate = date;
                }

                string strDtofOccurance = txtDtofOccurance.Text.Trim();
                if (!string.IsNullOrEmpty(strDtofOccurance))
                {
                    DateTime date;
                    if (DateTime.TryParse(strDtofOccurance, out date))
                        dtDtofOccurance = date;
                }

                string strDtofDetection = txtDtofDetection.Text.Trim();
                if (!string.IsNullOrEmpty(strDtofDetection))
                {
                    DateTime date;
                    if (DateTime.TryParse(strDtofDetection, out date))
                        dtDtofDetection = date;
                }

                string strDtofReporttoRBI = txtDtofReporttoRBI.Text.Trim();
                if (!string.IsNullOrEmpty(strDtofReporttoRBI))
                {
                    DateTime date;
                    if (DateTime.TryParse(strDtofReporttoRBI, out date))
                        dtDtofReporttoRBI = date;
                }

                string strFIRDate = txtFIRDate.Text.Trim();
                if (!string.IsNullOrEmpty(strFIRDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strFIRDate, out date))
                        dtFIRDate = date;
                }

                string strDtofNPA = txtDtofNPA.Text.Trim();
                if (!string.IsNullOrEmpty(strDtofNPA))
                {
                    DateTime date;
                    if (DateTime.TryParse(strDtofNPA, out date))
                        dtDtofNPA = date;
                }

                string strHOSACDate = txtHOSACDate.Text.Trim();
                if (!string.IsNullOrEmpty(strHOSACDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strHOSACDate, out date))
                        dtHOSACDate = date;
                }

                string strCaseSubmissionDate = txtCaseSubmissionDate.Text.Trim();
                if (!string.IsNullOrEmpty(strCaseSubmissionDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strCaseSubmissionDate, out date))
                        dtCaseSubmissionDate = date;
                }

                //string strABBFFReplydt = txtABBFFReplydt.Text.Trim();
                //if (!string.IsNullOrEmpty(strABBFFReplydt))
                //{
                //    DateTime date;
                //    if (DateTime.TryParse(strABBFFReplydt, out date))
                //        dtABBFFReplydt = date;
                //}

                string strReplySenttoABBFFDt = txtReplySenttoABBFFDt.Text.Trim();
                if (!string.IsNullOrEmpty(strReplySenttoABBFFDt))
                {
                    DateTime date;
                    if (DateTime.TryParse(strReplySenttoABBFFDt, out date))
                        dtReplySenttoABBFFDt = date;
                }

                string strABBFFAdviceRecDate = txtABBFFAdviceRecDate.Text.Trim();
                if (!string.IsNullOrEmpty(strABBFFAdviceRecDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(strABBFFAdviceRecDate, out date))
                        dtABBFFAdviceRecDate = date;
                }
                string strClosureDt = txtClosureDt.Text.Trim();
                if (!string.IsNullOrEmpty(strClosureDt))
                {
                    DateTime date;
                    if (DateTime.TryParse(strClosureDt, out date))
                        dtClosureDt = date;
                }
                string strRetirementDate_D = txtRetirementDate_D.Text.Trim();
                if (!string.IsNullOrEmpty(strRetirementDate_D))
                {
                    DateTime date;
                    if (DateTime.TryParse(strRetirementDate_D, out date))
                        dtRetirementDate_D = date;
                }
                #endregion

                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spABBFFStructure_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_RNO", txtRNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_RECDATE", dtABBFFRecDate);
                cmdSave.Parameters.AddWithValue("@p_SOURCEREF", objCommonFunction.ddlSelectedValue(ddlSourceRef));
                cmdSave.Parameters.AddWithValue("@p_SOURCEDATE", dtSourceDate);
                cmdSave.Parameters.AddWithValue("@p_DATEOFOCCURANCE", dtDtofOccurance);
                cmdSave.Parameters.AddWithValue("@p_DATEOFDETECTION", dtDtofDetection);
                cmdSave.Parameters.AddWithValue("@p_DATEOFREPORTINGTORBI", dtDtofReporttoRBI);
                cmdSave.Parameters.AddWithValue("@p_FRAUD_COMMITED_BY", objCommonFunction.ddlSelectedValue(ddlFraudCommitedby));
                cmdSave.Parameters.AddWithValue("@p_FMR_NO", txtFMRNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_FIR", objCommonFunction.ddlSelectedValue(ddlFIR));
                cmdSave.Parameters.AddWithValue("@p_FIR_DATE", dtFIRDate);
                cmdSave.Parameters.AddWithValue("@p_DATE_OF_NPA", dtDtofNPA);
                cmdSave.Parameters.AddWithValue("@p_ACCOUNT_NAME", txtAccName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_AMOUNT", txtAmt.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BR_COMPLAINT", txtBRComplaint.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_BRANCH", txtBranchOffice.Text.Trim());
                // cmdSave.Parameters.AddWithValue("@p_ZONE", txtZone.Text.Trim());
                // cmdSave.Parameters.AddWithValue("@p_CIRCLE_OFFICE", txtCircleOffice.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_HOSAC_DATE", dtHOSACDate);
                cmdSave.Parameters.AddWithValue("@p_MODUS_OPERANDI", txtModusOperandi.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ABBFF_CASE_SUBMISSION_DATE", dtCaseSubmissionDate);

                cmdSave.Parameters.AddWithValue("@p_ABBFF_OBSERVATIONS", txtABBFFReplydt.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_REPLY_SENT_TO_ABBF_DT", dtReplySenttoABBFFDt);
                cmdSave.Parameters.AddWithValue("@p_ABBFF_REFERENCE_NO", txtABBFFRefNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ABBFF_ADVICE_RCVD_DT", dtABBFFAdviceRecDate);

                cmdSave.Parameters.AddWithValue("@p_ABBFF_ADVICE_DETAILS", txtABBFFAdviceDetails.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_STATUS", txtStatus.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_STATUS_CODE", objCommonFunction.ddlSelectedValue(ddlStatusCode));
                cmdSave.Parameters.AddWithValue("@p_CONNECTED_SOP_NUMBER", txtConnectSOPNumber.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CASE_CLOSE", objCommonFunction.ddlSelectedValue(ddlCaseCloseStatus));
                cmdSave.Parameters.AddWithValue("@p_CLOSURE_DATE", dtClosureDt);
                cmdSave.Parameters.AddWithValue("@p_CLOSURE_CHECKED", closureChecked);
                cmdSave.Parameters.AddWithValue("@p_CODE", "12433");
                cmdSave.Parameters.AddWithValue("@p_DESK_USER_REMARKS", txtDeskUserRrmks.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_CHANNEL", "MANUALENTRY");
                cmdSave.Parameters.AddWithValue("@p_NEW_ZONE", objCommonFunction.ddlSelectedValue(ddlNewZone));
                cmdSave.Parameters.AddWithValue("@p_NEW_CIRCLE", objCommonFunction.ddlSelectedValue(ddlNewCircle));

                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["USERID"]));
                cmdSave.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["ROLE"]));


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

        public void funcClear()
        {
            txtRNo.Enabled = true;
            txtRNo.Text = String.Empty;
            txtABBFFRecDate.Text = String.Empty;
            ddlSourceRef.SelectedIndex = 0;
            txtSourceDate.Text = String.Empty;
            txtDtofOccurance.Text = String.Empty;
            txtDtofDetection.Text = String.Empty;
            txtDtofReporttoRBI.Text = String.Empty;
            ddlFraudCommitedby.SelectedIndex = 0;
            txtFMRNo.Text = String.Empty;
            ddlFIR.SelectedIndex = 0;
            txtFIRDate.Text = String.Empty;
            txtDtofNPA.Text = String.Empty;
            txtAccName.Text = String.Empty;
            txtAmt.Text = String.Empty;
            txtBRComplaint.Text = String.Empty;
            //txtZone.Text = String.Empty;
            //txtCircleOffice.Text = String.Empty;
            txtHOSACDate.Text = String.Empty;
            txtModusOperandi.Text = String.Empty;
            txtCaseSubmissionDate.Text = String.Empty;
            txtABBFFReplydt.Text = String.Empty;
            txtReplySenttoABBFFDt.Text = String.Empty;
            txtABBFFRefNo.Text = String.Empty;
            txtABBFFAdviceRecDate.Text = String.Empty;
            txtABBFFAdviceDetails.Text = String.Empty;
            txtStatus.Text = String.Empty;
            ddlStatusCode.SelectedIndex = 0;
            txtConnectSOPNumber.Text = String.Empty;
            ddlCaseCloseStatus.SelectedIndex = 0;
            txtClosureDt.Text = String.Empty;
            chkClosureDate.Checked = false;
            txtDeskUserRrmks.Text = String.Empty;
            ddlNewZone.SelectedIndex = 0;
            ddlNewCircle.SelectedIndex = 0;

            funcClearEODetails();

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

            if (string.IsNullOrEmpty(txtABBFFRecDate.Text.Trim()))
            {
                lblMsg.Text = "Please enter REC  Date...!";
                return Result = false;
            }

            if (string.IsNullOrEmpty(txtStatus.Text.Trim()))
            {
                lblMsg.Text = "Please enter Status...!";
                return Result = false;
            }

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlNewZone)))
            {
                lblMsg.Text = "Please select new zone from dropdown";
                return Result = false;
            }

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlNewCircle)))
            {
                lblMsg.Text = "Please select new circle from dropdown";
                return Result = false;
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
            txtNPANo.Text = "";
            txtIACNo.Text = "";
            txtVigNo.Text = "";
            btnAddEO.ToolTip = "";
            DataTable dt = null;
            gvEODetails.DataSource = dt;
            gvEODetails.DataBind();
        }

        private bool funcAddEO()
        {
            Boolean Result = false;
            if (!string.IsNullOrEmpty(txtRNo.Text.Trim()))
            {
                SqlConnection conSave = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                SqlCommand cmdSave = new SqlCommand();
                string MODE = string.Empty;

                try
                {
                    if (string.IsNullOrEmpty(btnAddEO.ToolTip))
                    {
                        string ID = Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper();
                        EOUNIQUEID = "ABBFFEO" + ID + DateTime.Now.ToString("ddMMyyhhmmss");
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
                            dtRetirementDate_D = date;
                    }

                    conSave.Open();
                    cmdSave.Connection = conSave;
                    cmdSave.Parameters.Clear();
                    cmdSave.CommandType = CommandType.StoredProcedure;
                    cmdSave.CommandText = "[dbo].[spABBFFEO_Add]";

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
                    cmdSave.Parameters.AddWithValue("@p_DOR", dtRetirementDate_D);
                    cmdSave.Parameters.AddWithValue("@p_DEALTHWITH", objCommonFunction.ddlSelectedValue(ddlDealtWith_D));
                    cmdSave.Parameters.AddWithValue("@p_RELATEDNPANO", txtNPANo.Text.Trim());
                    cmdSave.Parameters.AddWithValue("@p_RELATEDCASENOIAC", txtIACNo.Text.Trim());
                    cmdSave.Parameters.AddWithValue("@p_RELATEDCASENOVIG", txtVigNo.Text.Trim());
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
            }
            else
            {
                lblMsg.Text = "R.No Cannot be null or empty";
            }
            return Result;
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
                    funcShowEODetails(UNIQUEID);
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
                    txtNPANo.Text = Data[7];
                    txtIACNo.Text = Data[8];
                    txtVigNo.Text = Data[9];
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
                    txtNPANo.Text = "";
                    txtIACNo.Text = "";
                    txtVigNo.Text = "";
                    lblMsg.Text = "Unique ID is null...";
                }
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
                cmdSave.CommandText = "[dbo].[spABBFFEO_Delete]";

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
                    lblMsg.Text = "Error in deleting: ABBFF Details";
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (funcValidation("I"))
            {
                funcSave("i");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (funcValidation("I"))
            {
                funcSave("U");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mis/frmABBFF.aspx", true);
        }

        protected void ddlNewZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ZONE = objCommonFunction.ddlSelectedValue(ddlNewZone);

            if (!string.IsNullOrEmpty(ZONE))
            {
                objCommonFunction.funcZoneCircleMaster(ddlNewCircle, ZONE);
            }
            else
            {
                ddlNewCircle.Items.Clear();
            }
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            txtRNo.Enabled = false;
            funcShow(txtRNo.Text.Trim(), "GET", null, null, null, null, null, null, null);
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
                cmdView.CommandText = "[dbo].[spABBFFStructure_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_SEARCHNO", p_strNo);
                cmdView.Parameters.AddWithValue("@p_VIEW", p_strView);


                cmdView.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmdView);
                sda.Fill(dt);
                ViewState["DETAILDATA"] = dt;

                if (Convert.ToInt32(sqlErrCodeOutput.Value) >= 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (p_strView.ToUpper().Equals("GET"))
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
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;

            txtRNo.Text = Convert.ToString(dtData.Rows[0]["RNO"]);
            txtABBFFRecDate.Text = Convert.ToDateTime(dtData.Rows[0]["RECDATE"]).ToString("dd/MM/yyyy");
            objCommonFunction.ddlSetData(ddlSourceRef, Convert.ToString(dtData.Rows[0]["SOURCEREF"]), true);
            txtSourceDate.Text = Convert.ToDateTime(dtData.Rows[0]["SOURCEDATE"]).ToString("dd/MM/yyyy");
            txtDtofOccurance.Text = Convert.ToDateTime(dtData.Rows[0]["DATEOFOCCURANCE"]).ToString("dd/MM/yyyy");
            txtDtofDetection.Text = Convert.ToDateTime(dtData.Rows[0]["DATEOFDETECTION"]).ToString("dd/MM/yyyy");
            txtDtofReporttoRBI.Text = Convert.ToDateTime(dtData.Rows[0]["DATEOFREPORTINGTORBI"]).ToString("dd/MM/yyyy");
            ddlFraudCommitedby.SelectedValue = Convert.ToString(dtData.Rows[0]["FRAUD_COMMITED_BY"]);
            //objCommonFunction.ddlSetData(ddlFraudCommitedby, Convert.ToString(dtData.Rows[0]["FRAUD_COMMITED_BY"]), true);
            txtFMRNo.Text = Convert.ToString(dtData.Rows[0]["FMR_NO"]);
            ddlFIR.SelectedValue = Convert.ToString(dtData.Rows[0]["FIR"]);
            //objCommonFunction.ddlSetData(ddlFIR, Convert.ToString(dtData.Rows[0]["FIR"]), true);
            txtFIRDate.Text = Convert.ToDateTime(dtData.Rows[0]["FIR_DATE"]).ToString("dd/MM/yyyy");
            txtDtofNPA.Text = Convert.ToDateTime(dtData.Rows[0]["DATE_OF_NPA"]).ToString("dd/MM/yyyy");
            txtAccName.Text = Convert.ToString(dtData.Rows[0]["ACCOUNT_NAME"]);
            txtAmt.Text = Convert.ToString(dtData.Rows[0]["AMOUNT"]);
            txtBRComplaint.Text = Convert.ToString(dtData.Rows[0]["BR_COMPLAINT"]);
            txtBranchOffice.Text = Convert.ToString(dtData.Rows[0]["BRANCH_OFFICE"]);
            //txtZone.Text = Convert.ToString(dtData.Rows[0]["ZONE"]);
            //txtCircleOffice.Text = Convert.ToString(dtData.Rows[0]["CIRCLE_OFFICE"]);
            txtHOSACDate.Text = Convert.ToDateTime(dtData.Rows[0]["HOSAC_DATE"]).ToString("dd/MM/yyyy");
            txtModusOperandi.Text = Convert.ToString(dtData.Rows[0]["MODUS_OPERANDI"]);
            txtCaseSubmissionDate.Text = Convert.ToDateTime(dtData.Rows[0]["ABBFF_CASE_SUBMISSION_DATE"]).ToString("dd/MM/yyyy");
            txtABBFFReplydt.Text = Convert.ToString(dtData.Rows[0]["ABBFF_OBSERVATIONS"]);
            txtReplySenttoABBFFDt.Text = string.IsNullOrEmpty(Convert.ToString(dtData.Rows[0]["REPLY_SENT_TO_ABBF_DT"]))? "" :  Convert.ToDateTime(dtData.Rows[0]["REPLY_SENT_TO_ABBF_DT"]).ToString("dd/MM/yyyy");
            txtABBFFRefNo.Text = Convert.ToString(dtData.Rows[0]["ABBFF_REFERENCE_NO"]);
            txtABBFFAdviceRecDate.Text = string.IsNullOrEmpty(Convert.ToString(dtData.Rows[0]["ABBFF_ADVICE_RCVD_DT"]))?"": Convert.ToDateTime(dtData.Rows[0]["ABBFF_ADVICE_RCVD_DT"]).ToString("dd/MM/yyyy");
            txtABBFFAdviceDetails.Text = Convert.ToString(dtData.Rows[0]["ABBFF_ADVICE_DETAILS"]);
            txtStatus.Text = Convert.ToString(dtData.Rows[0]["STATUS"]);
            ddlStatusCode.SelectedValue = Convert.ToString(dtData.Rows[0]["STATUS_CODE"]);

            // objCommonFunction.ddlSetDataValue(ddlStatusCode, Convert.ToString(dtData.Rows[0]["STATUS_CODE"]));
            txtConnectSOPNumber.Text = Convert.ToString(dtData.Rows[0]["CONNECTED_SOP_NUMBER"]);
            ddlCaseCloseStatus.SelectedValue = Convert.ToString(dtData.Rows[0]["CASE_CLOSE"]);
            // objCommonFunction.ddlSetData(ddlCaseCloseStatus, Convert.ToString(dtData.Rows[0]["CASE_CLOSE"]), true);
            if(Convert.ToString(dtData.Rows[0]["CLOSURE_CHECK"]) == "Y")            
                chkClosureDate.Checked = true;            
            else
                chkClosureDate.Checked = false;

            txtClosureDt.Text = string.IsNullOrEmpty(Convert.ToString(dtData.Rows[0]["CLOSURE_DATE"]))?"": Convert.ToDateTime(dtData.Rows[0]["CLOSURE_DATE"]).ToString("dd/MM/yyyy");
            txtDeskUserRrmks.Text = Convert.ToString(dtData.Rows[0]["DESK_USER_REMARKS"]);
            ddlNewZone.SelectedValue = Convert.ToString(dtData.Rows[0]["NEW_ZONE"]);
            if (!string.IsNullOrEmpty(ddlNewZone.SelectedValue))
            {
                string ZONE = objCommonFunction.ddlSelectedValue(ddlNewZone);

                if (!string.IsNullOrEmpty(ZONE))
                {
                    objCommonFunction.funcZoneCircleMaster(ddlNewCircle, ZONE);
                }
                else
                {
                    ddlNewCircle.Items.Clear();
                }
            }
            ddlNewCircle.SelectedValue = Convert.ToString(dtData.Rows[0]["NEW_CIRCLE"]);
            //  objCommonFunction.ddlSetData(ddlNewZone, Convert.ToString(dtData.Rows[0]["NEW_ZONE"]), true);
            // objCommonFunction.ddlSetData(ddlNewCircle, Convert.ToString(dtData.Rows[0]["NEW_CIRCLE"]), true);

        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper().Equals("VIEW"))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(e.CommandArgument)))
                {
                    lblMsg.Text = "";
                    txtRNo.Enabled = false;
                    tabMain.ActiveTabIndex = 0;
                    funcShow(Convert.ToString(e.CommandArgument), "GET", null, null, null, null, null, null, null);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtR_No_LIST.Text)||string.IsNullOrEmpty(txt_FMR_NO.Text)||string.IsNullOrEmpty(ddlZone_List.SelectedValue)||string.IsNullOrEmpty(ddlCircle_List.SelectedValue))
            {
                using (vigcontext context = new vigcontext())
                {
                    var data = context.ABBFFs.Where(x => x.RNO == txtR_No_LIST.Text || x.FMR_NO == txt_FMR_NO.Text || x.NEW_ZONE == ddlZone_List.SelectedValue || x.NEW_CIRCLE == ddlCircle_List.SelectedValue).ToList();
                    if(data!=null && data.Count>0)
                    {
                        gvDetails.DataSource = data;
                        gvDetails.DataBind();
                    }
                    else
                    {
                        lblMsgSearch.Text = "No Details found for the selected criteria";
                        lblMsgSearch.ForeColor = System.Drawing.Color.White;
                        lblMsgSearch.Visible = true;
                        gvDetails.DataSource = null;
                        gvDetails.DataBind();
                    }
                };
            }
            else
            {
                lblMsgSearch.Text = "Please select any of the above criteria";
                lblMsgSearch.ForeColor = System.Drawing.Color.White;
                lblMsgSearch.Visible = true;
            }
        }

        private void funbinddtab2()
        {
            using (vigcontext context = new vigcontext())
            {
                var data = context.ABBFFs.ToList();
                if (data != null && data.Count > 0)
                {
                    gvDetails.DataSource = data;
                    gvDetails.DataBind();
                }
                else
                {  
                    gvDetails.DataSource = null;
                    gvDetails.DataBind();
                }
            };
        }

        protected void ddlZone_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ZONE = objCommonFunction.ddlSelectedValue(ddlZone_List);

            if (!string.IsNullOrEmpty(ZONE))
            {
                objCommonFunction.funcZoneCircleMaster(ddlCircle_List, ZONE);
            }
            else
            {
                ddlNewCircle.Items.Clear();
            }
        }

        protected void chkClosureDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClosureDate.Checked)
                txtClosureDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            else
                txtClosureDt.Text = "";

        }

        protected void custchkClosureDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = chkClosureDate.Checked;
        }
    }
}