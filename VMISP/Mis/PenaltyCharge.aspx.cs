using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class PenaltyCharge : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();
        string UNIQUENO = string.Empty;
        string STATUS = string.Empty;
        string ZONE = string.Empty;
        string CIRCLE = string.Empty;

        DateTime? CHARGESHEETDATE = null;
        DateTime? FINALORDERDATE = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();

                funcbindDropdown();
                funcShow("LIST", null, null, null, null);
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
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spPenaltyCharge_Ddl]";
                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlStatus, ds.Tables[0]);
                    objCommonFunction.bindDropdownList(ddlZone, ds.Tables[1]);
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

        public void funcControlsUserRights()
        {
            if (Convert.ToString(ViewState["USERROLE"]).ToUpper().Equals("VMIS_VIEWUSER"))
            {
                objCommonFunction.DisableAllControls(this.Page);

                foreach (GridViewRow row in gvDetails.Rows)
                {
                    Button btnView = ((Button)row.FindControl("btnView")) as Button;
                    btnView.Enabled = true;
                }

                txtVigCaseNo_LIST.Enabled = true;
                txtPNCNO_LIST.Enabled = true;
                txtPFNoOfDA_LIST.Enabled = true;
                btnSearch.Enabled = true;
            }
            else if (Convert.ToString(ViewState["USERROLE"]).ToUpper().Equals("VMIS_DESKUSER"))
            {
                objCommonFunction.DisableAllControls(this.Page);

                foreach (GridViewRow row in gvDetails.Rows)
                {
                    Button btnView = ((Button)row.FindControl("btnView")) as Button;
                    btnView.Enabled = true;
                }

                txtVigCaseNo_LIST.Enabled = true;
                txtPNCNO_LIST.Enabled = true;
                txtPFNoOfDA_LIST.Enabled = true;
                btnSearch.Enabled = true;
            }
            else if (Convert.ToString(ViewState["USERROLE"]).ToUpper().Equals("VMIS_MISUSER"))
            {
                objCommonFunction.EnableAllControls(this.Page);
            }
        }

        public void funcClear()
        {
            txtPNCNo.Text = ""; txtEOName.Text = ""; txtEODesignation.Text = ""; txtVigCaseNo.Text = "";
            txtChargeSheetDate.Text = ""; txtNatureOfChargeSheet.Text = ""; txtLapsesCharges.Text = "";
            ddlStatus.SelectedIndex = 0; txtAccountName.Text = ""; txtFinalOrderDate.Text = ""; txtPenaltyImposed.Text = "";
            txtPFNumberOfDA.Text = ""; txtNameOfDA.Text = "";
            ddlZone.SelectedIndex = 0; ddlCircle.SelectedIndex = 0;
            txtPostingOfDA.Text = ""; txtDesignationOfDA.Text = "";
            btnSubmit.Visible = true; btnUpdate.Visible = false;
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ZONE = objCommonFunction.ddlSelectedValue(ddlZone);

            if (!string.IsNullOrEmpty(ZONE))
            {
                objCommonFunction.funcZoneCircleMaster(ddlCircle, ZONE);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (funcValidation() == true)
            {
                funcSave("I", null);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(txtPNCNO_LIST.Text.Trim())) || (!string.IsNullOrEmpty(txtVigCaseNo_LIST.Text.Trim())) || (!string.IsNullOrEmpty(txtPFNoOfDA_LIST.Text.Trim())))
            {
                funcShow("SEARCH", null, txtPNCNO_LIST.Text.Trim(), txtVigCaseNo_LIST.Text.Trim(), txtPFNoOfDA_LIST.Text.Trim());
            }
            else
            {
                funcShow("LIST", null, null, null, null);
            }
        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper().Equals("VIEW"))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(e.CommandArgument)))
                {
                    funcShow("GET", Convert.ToString(e.CommandArgument), null, null, null);
                }
            }
        }

        public void funcShow(string VIEW, string UNIQUEID, string PNCNO, string VIGNO, string DAPFNO)
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
                cmdView.CommandText = "[dbo].[spPenaltyCharge_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_VIEW", VIEW);
                cmdView.Parameters.AddWithValue("@p_UNIQUEID", UNIQUEID);
                cmdView.Parameters.AddWithValue("@p_PNCNO", PNCNO);
                cmdView.Parameters.AddWithValue("@p_VICASENO", VIGNO);
                cmdView.Parameters.AddWithValue("@p_DAPFNO", DAPFNO);
                cmdView.Parameters.AddWithValue("@p_USERID", Convert.ToString(Session["userid"]));
                cmdView.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));

                cmdView.CommandTimeout = 0;
                sda.Fill(dt);
                ViewState["DETAILDATA"] = dt;
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
                        hidUniqueID.Value = Convert.ToString(dt.Rows[0]["PC_UNIQUEID"]);

                        txtPNCNo.Text = Convert.ToString(dt.Rows[0]["PC_PNCNO"]);
                        txtVigCaseNo.Text = Convert.ToString(dt.Rows[0]["PC_VIGCASENO"]);
                        txtEOName.Text = Convert.ToString(dt.Rows[0]["PC_EO_NAME"]);
                        txtEODesignation.Text = Convert.ToString(dt.Rows[0]["PC_EO_DESIGNATION"]);
                        txtChargeSheetDate.Text = Convert.ToString(dt.Rows[0]["CHARGESHEETDATE"]);
                        txtNatureOfChargeSheet.Text = Convert.ToString(dt.Rows[0]["PC_NATURE_OF_CHARGESHEEET"]);
                        txtLapsesCharges.Text = Convert.ToString(dt.Rows[0]["PC_LAPSES_CHARGES"]);
                        txtAccountName.Text = Convert.ToString(dt.Rows[0]["PC_ACCOUNT_NAME"]);
                        txtFinalOrderDate.Text = Convert.ToString(dt.Rows[0]["FODATE"]);
                        txtPenaltyImposed.Text = Convert.ToString(dt.Rows[0]["PC_PENALTY_IMPOSED"]);
                        txtPFNumberOfDA.Text = Convert.ToString(dt.Rows[0]["PC_DA_PFNO"]);
                        txtNameOfDA.Text = Convert.ToString(dt.Rows[0]["PC_DA_NAME"]);
                        txtPostingOfDA.Text = Convert.ToString(dt.Rows[0]["PC_DA_POSTING"]);
                        txtDesignationOfDA.Text = Convert.ToString(dt.Rows[0]["PC_DA_DESIGNATION"]);

                        objCommonFunction.ddlSetDataValue(ddlStatus, Convert.ToString(dt.Rows[0]["PC_STATUS_CODE"]));
                        objCommonFunction.ddlSetDataValue(ddlZone, Convert.ToString(dt.Rows[0]["PC_ZONE"]));
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["PC_ZONE"])))
                        {
                            objCommonFunction.funcZoneCircleMaster(ddlCircle, Convert.ToString(dt.Rows[0]["PC_ZONE"]));
                            objCommonFunction.ddlSetDataValue(ddlCircle, Convert.ToString(dt.Rows[0]["PC_CIRCLE"]));
                        }

                        tabMain.ActiveTabIndex = 0;
                        btnSubmit.Visible = false;
                        btnUpdate.Visible = true;
                    }

                    funcControlsUserRights();
                }
                else
                {
                    lblMsgSearch.Text = "Not Found...!!";
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
                    UNIQUENO = "PC" + DateTime.Now.ToString("ddMMyy") + ID;
                }
                else
                {
                    UNIQUENO = UPDATEID;
                }

                ZONE = objCommonFunction.ddlSelectedValue(ddlZone);
                CIRCLE = objCommonFunction.ddlSelectedValue(ddlCircle);
                STATUS = objCommonFunction.ddlSelectedValue(ddlStatus);

                string ChargeSheetDate = txtChargeSheetDate.Text.Trim();
                if (!string.IsNullOrEmpty(ChargeSheetDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(ChargeSheetDate, out date))
                        CHARGESHEETDATE = date;
                }

                string FinalOrderDate = txtFinalOrderDate.Text.Trim();
                if (!string.IsNullOrEmpty(FinalOrderDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(FinalOrderDate, out date))
                        FINALORDERDATE = date;
                }

                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spPenaltyCharge]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_UNIQUENO", UNIQUENO);

                cmdSave.Parameters.AddWithValue("@p_PC_PNCNO", txtPNCNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PC_VIGCASENO", txtVigCaseNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PC_EO_NAME", txtEOName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PC_EO_DESIGNATION", txtEODesignation.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PC_NATURE_OF_CHARGESHEEET", txtNatureOfChargeSheet.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PC_LAPSES_CHARGES", txtLapsesCharges.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PC_ACCOUNT_NAME", txtAccountName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PC_PENALTY_IMPOSED", txtPenaltyImposed.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PC_DA_PFNO", txtPFNumberOfDA.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PC_DA_NAME", txtNameOfDA.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PC_DA_POSTING", txtPostingOfDA.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PC_DA_DESIGNATION", txtDesignationOfDA.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PC_CHARGESHEET_DATE", CHARGESHEETDATE);
                cmdSave.Parameters.AddWithValue("@p_PC_FO_DATE", FINALORDERDATE);
                cmdSave.Parameters.AddWithValue("@p_PC_STATUS", STATUS);
                cmdSave.Parameters.AddWithValue("@p_PC_ZONE", ZONE);
                cmdSave.Parameters.AddWithValue("@p_PC_CIRCLE", CIRCLE);
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["userid"]));
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                cmdSave.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    lblMsg.Text = Server.HtmlEncode(sqlErrMsgOutput.Value.ToString());
                    funcClear();
                    funcShow("LIST", null, null, null, null);
                }
                else
                {
                    Int32 intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == 2)
                    {
                        lblMsg.Text = Server.HtmlEncode(sqlErrMsgOutput.Value.ToString());
                    }
                    else
                    {
                        lblMsg.Text = Server.HtmlEncode("Error - Insert/ Update Penalty Charge, please contact to Administrator");
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

        private Boolean funcValidation()
        {
            Boolean Result = true;
            lblMsg.CssClass = "label label-danger";

            if (string.IsNullOrEmpty(txtPNCNo.Text.Trim()))
            {
                lblMsg.Text = "Please enter PNC No.";
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtVigCaseNo.Text.Trim()))
            {
                lblMsg.Text = "Please enter Vigilance Case No.";
                txtVigCaseNo.Focus();
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtEOName.Text.Trim()))
            {
                lblMsg.Text = "Please enter EO Name.";
                txtEOName.Focus();
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtEODesignation.Text.Trim()))
            {
                lblMsg.Text = "Please enter EO Designation.";
                txtEODesignation.Focus();
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtChargeSheetDate.Text.Trim()))
            {
                lblMsg.Text = "Please enter Charge Sheet Date.";
                txtChargeSheetDate.Focus();
                return Result = false;
            }
            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlStatus)))
            {
                lblMsg.Text = "Please select Status.";
                ddlStatus.Focus();
                return Result = false;
            }
            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlZone)))
            {
                lblMsg.Text = "Please select zone.";
                ddlZone.Focus();
                return Result = false;
            }
            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlCircle)))
            {
                lblMsg.Text = "Please select circle.";
                ddlCircle.Focus();
                return Result = false;
            }

            return Result;
        }
    }
}