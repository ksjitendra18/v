using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class Lodi : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();
        string UNIQUENO = string.Empty;
        string SCALE = string.Empty;
        string ZONE = string.Empty;
        string CIRCLE = string.Empty;

        DateTime? LODIDATE = null;
        DateTime? DOR = null;
        DateTime? DOP = null;
        DateTime? DOCS = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();

                funcbindDropdown();
                funcShow("LIST", null, null, null, null, null);
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
                cmd.CommandText = "[dbo].[spLodi_Ddl]";
                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlScale, ds.Tables[0]);
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

        public void funcClear()
        {
            txtLodiasOnDate.Text = ""; txtLodiNo.Text = ""; txtPFNo.Text = ""; txtVigCaseNo.Text = "";
            txtName.Text = ""; txtRetirementDate.Text = "";
            ddlScale.SelectedIndex = 0; txtCBI.Text = ""; txtPunishmentDate.Text = ""; txtDateofChargeSheet.Text = "";
            txtAllegationsinBrief.Text = ""; txtReasonsForInclusion.Text = "";
            ddlZone.SelectedIndex = 0; ddlCircle.SelectedIndex = 0;
            txtRemarks.Text = ""; txtReasonforDeletion.Text = "";
            btnSubmit.Visible = true; btnUpdate.Visible = false;
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
                txtPFNo_LIST.Enabled = true;
                txtLodiNO_LIST.Enabled = true;
                txtName_LIST.Enabled = true;
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
                txtPFNo_LIST.Enabled = true;
                txtLodiNO_LIST.Enabled = true;
                txtName_LIST.Enabled = true;
                btnSearch.Enabled = true;
            }
            else if (Convert.ToString(ViewState["USERROLE"]).ToUpper().Equals("VMIS_MISUSER"))
            {
                objCommonFunction.EnableAllControls(this.Page);
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
                    string ID = Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper();
                    UNIQUENO = "LODI" + DateTime.Now.ToString("ddMMyy") + ID;
                }
                else
                {
                    UNIQUENO = UPDATEID;
                }

                SCALE = objCommonFunction.ddlSelectedValue(ddlScale);
                ZONE = objCommonFunction.ddlSelectedValue(ddlZone);
                CIRCLE = objCommonFunction.ddlSelectedValue(ddlCircle);

                string LodiasOnDate = txtLodiasOnDate.Text.Trim();
                if (!string.IsNullOrEmpty(LodiasOnDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(LodiasOnDate, out date))
                        LODIDATE = date;
                }

                string RetirementDate = txtRetirementDate.Text.Trim();
                if (!string.IsNullOrEmpty(RetirementDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(RetirementDate, out date))
                        DOR = date;
                }

                string PunishmentDate = txtPunishmentDate.Text.Trim();
                if (!string.IsNullOrEmpty(PunishmentDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(PunishmentDate, out date))
                        DOP = date;
                }

                string DateofChargeSheet = txtDateofChargeSheet.Text.Trim();
                if (!string.IsNullOrEmpty(DateofChargeSheet))
                {
                    DateTime date;
                    if (DateTime.TryParse(DateofChargeSheet, out date))
                        DOCS = date;
                }

                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spLodi]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_UNIQUENO", UNIQUENO);
                cmdSave.Parameters.AddWithValue("@p_LODIASONDATE", LODIDATE);
                cmdSave.Parameters.AddWithValue("@p_LODINO", txtLodiNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_VIGCASENO", txtVigCaseNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_PFNO", txtPFNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_NAME", txtName.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DOR", DOR);
                cmdSave.Parameters.AddWithValue("@p_SCALE", SCALE);
                cmdSave.Parameters.AddWithValue("@p_CBI", txtCBI.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DOP", DOP);
                cmdSave.Parameters.AddWithValue("@p_DOCS", DOCS);
                cmdSave.Parameters.AddWithValue("@p_ALLEGATIONS", txtAllegationsinBrief.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_REASON", txtReasonsForInclusion.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_ZONE", ZONE);
                cmdSave.Parameters.AddWithValue("@p_CIRCLE", CIRCLE);
                cmdSave.Parameters.AddWithValue("@P_DELETED_FROM_LODI", objCommonFunction.ddlSelectedValue(ddlRemove));
                cmdSave.Parameters.AddWithValue("@p_DELETED_REASON", txtReasonforDeletion.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_REMARKS", txtRemarks.Text.Trim());

                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["userid"]));
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                cmdSave.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    lblMsg.Text = Server.HtmlEncode(sqlErrMsgOutput.Value.ToString());
                    funcClear();
                    funcShow("LIST", null, null, null, null, null);
                }
                else
                {
                    lblMsg.Text = Server.HtmlEncode("Error - Lodi Details, please contact to Administrator");
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

        public void funcShow(string VIEW, string UNIQUEID, string VICASENO,string PFNO, string LODINO, string NAME)
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
                cmdView.CommandText = "[dbo].[spLodi_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdView.Parameters.Add(sqlErrMsgOutput);
                cmdView.Parameters.Add(sqlErrCodeOutput);

                cmdView.Parameters.AddWithValue("@p_VIEW", VIEW);
                cmdView.Parameters.AddWithValue("@p_UNIQUEID", UNIQUEID);
                cmdView.Parameters.AddWithValue("@p_VICASENO", VICASENO);
                cmdView.Parameters.AddWithValue("@p_PFNO", PFNO);
                cmdView.Parameters.AddWithValue("@p_LODINO", LODINO);
                cmdView.Parameters.AddWithValue("@p_NAME", NAME);
                cmdView.Parameters.AddWithValue("@p_USERID", Convert.ToString(Session["userid"]));
                cmdView.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["role"]));

                cmdView.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    ViewState["DETAILDATA"] = dt;

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
                        hidUniqueID.Value = Convert.ToString(dt.Rows[0]["LODI_UNIQUEID"]);
                        txtLodiasOnDate.Text = Convert.ToString(dt.Rows[0]["LODIASONDATE"]);
                        txtLodiNo.Text = Convert.ToString(dt.Rows[0]["LODI_LODINO"]);
                        txtPFNo.Text = Convert.ToString(dt.Rows[0]["LODI_PFNO"]);
                        txtVigCaseNo.Text = Convert.ToString(dt.Rows[0]["LODI_VIGCASENO"]);
                        txtName.Text = Convert.ToString(dt.Rows[0]["LODI_NAME"]);
                        txtRetirementDate.Text = Convert.ToString(dt.Rows[0]["DOR"]);
                        objCommonFunction.ddlSetDataValue(ddlScale, Convert.ToString(dt.Rows[0]["LODI_SCALE"]));
                        txtCBI.Text = Convert.ToString(dt.Rows[0]["LODI_CBI"]);
                        txtPunishmentDate.Text = Convert.ToString(dt.Rows[0]["PUNISHMENTDATE"]);
                        txtDateofChargeSheet.Text = Convert.ToString(dt.Rows[0]["DATEOFCHARGESHEET"]);
                        txtAllegationsinBrief.Text = Convert.ToString(dt.Rows[0]["LODI_ALLEGATIONS"]);
                        txtReasonsForInclusion.Text = Convert.ToString(dt.Rows[0]["LODI_REASON"]);

                        txtRemarks.Text = Convert.ToString(dt.Rows[0]["LODI_REMARKS"]);
                        objCommonFunction.ddlSetDataValue(ddlRemove, Convert.ToString(dt.Rows[0]["LODI_DELETED_FROM_LODI"]));
                        txtReasonforDeletion.Text = Convert.ToString(dt.Rows[0]["LODI_DELETED_REASON"]);

                        objCommonFunction.ddlSetDataValue(ddlZone, Convert.ToString(dt.Rows[0]["LODI_ZONE"]));

                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LODI_ZONE"])))
                        {
                            objCommonFunction.funcZoneCircleMaster(ddlCircle, Convert.ToString(dt.Rows[0]["LODI_ZONE"]));
                            objCommonFunction.ddlSetDataValue(ddlCircle, Convert.ToString(dt.Rows[0]["LODI_CIRCLE"]));
                        }

                        tabMain.ActiveTabIndex = 0;
                        btnSubmit.Visible = false;
                        btnUpdate.Visible = true;
                    }

                    funcControlsUserRights();
                }
                else
                {
                    lblMsgSearch.Text = "Record Not Found";
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(txtVigCaseNo_LIST.Text.Trim())) || (!string.IsNullOrEmpty(txtLodiNO_LIST.Text.Trim())) || (!string.IsNullOrEmpty(txtPFNo_LIST.Text.Trim())) || (!string.IsNullOrEmpty(txtName_LIST.Text.Trim())))
            {
                funcShow("SEARCH", null, txtVigCaseNo_LIST.Text.Trim(), txtPFNo_LIST.Text.Trim(), txtLodiNO_LIST.Text.Trim(), txtName_LIST.Text.Trim());
            }
            else
            {
                funcShow("LIST", null, null, null, null, null);
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

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ZONE = objCommonFunction.ddlSelectedValue(ddlZone);

            if (!string.IsNullOrEmpty(ZONE))
            {
                objCommonFunction.funcZoneCircleMaster(ddlCircle, ZONE);
            }
        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper().Equals("VIEW"))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(e.CommandArgument)))
                {
                    funcShow("GET", Convert.ToString(e.CommandArgument), null, null, null, null);
                }
            }
        }

        private Boolean funcValidation()
        {
            Boolean Result = true;
            lblMsg.CssClass = "label label-danger";

            if (string.IsNullOrEmpty(txtLodiasOnDate.Text.Trim()))
            {
                lblMsg.Text = "Please select Lodi as on date.";
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtLodiNo.Text.Trim()))
            {
                lblMsg.Text = "Please enter Lodi No.";
                txtLodiNo.Focus();
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtVigCaseNo.Text.Trim()))
            {
                lblMsg.Text = "Please enter Vigilance Case No.";
                txtVigCaseNo.Focus();
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtPFNo.Text.Trim()))
            {
                lblMsg.Text = "Please enter PF No.";
                txtPFNo.Focus();
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                lblMsg.Text = "Please enter Name.";
                txtName.Focus();
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtRetirementDate.Text.Trim()))
            {
                lblMsg.Text = "Please enter Retirement Date.";
                txtRetirementDate.Focus();
                return Result = false;
            }
            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlScale)))
            {
                lblMsg.Text = "Please select scale.";
                ddlScale.Focus();
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
            if (Convert.ToString(objCommonFunction.ddlSelectedValue(ddlRemove)).Equals("Yes"))
            {
                if (string.IsNullOrEmpty(txtReasonforDeletion.Text.Trim()))
                {
                    lblMsg.Text = "Please enter Reason for Deletion.";
                    txtReasonforDeletion.Focus();
                    return Result = false;
                }
            }
            return Result;
        }
    }
}