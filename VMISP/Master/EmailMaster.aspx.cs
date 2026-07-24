using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;


namespace VMISP.Master
{
    public partial class EmailMaster : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (objCommonFunction.funcCheckUserRights("EMAIL_MASTER") == false)
                {
                    Response.Redirect("~/Logout.aspx");
                }
            }
        }

        private void funcSubmit(string MODE, string UPDATEID)
        {
            lblMsg.Text = "";
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();
            con.Open();
            cmdSave.Connection = con;
            cmdSave.Parameters.Clear();

            try
            {
                if (string.IsNullOrEmpty(UPDATEID))
                {
                    string ID = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                    UPDATEID = "EM" + DateTime.Now.ToString("ddMMyy") + ID;
                }

                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spEmailMaster]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_MODE", MODE);
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["USERID"]));
                cmdSave.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());

                cmdSave.Parameters.AddWithValue("@p_UNIQUEID", UPDATEID);
                cmdSave.Parameters.AddWithValue("@p_AUTHORITY", objCommonFunction.ddlSelectedValue(ddlAuthority));
                cmdSave.Parameters.AddWithValue("@p_AUTHORITY_DETAIL_SOLID", objCommonFunction.ddlSelectedValue(ddlAuthorityDetail));
                cmdSave.Parameters.AddWithValue("@p_AUTHORITY_DETAIL_NAME", objCommonFunction.ddlSelectedText(ddlAuthorityDetail));
                cmdSave.Parameters.AddWithValue("@p_EMAILID", txtEmailID.Text.Trim());

                cmdSave.CommandTimeout = 0;

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    funcClear();    //Reset all controls.
                    lblMsg.Text = sqlErrMsgOutput.Value.ToString();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert(' " + lblMsg.Text + "');", true);
                }
                else
                {
                    if (Convert.ToInt32(sqlErrCodeOutput.Value).Equals(2))
                    {
                        lblMsg.Text = sqlErrMsgOutput.Value.ToString();
                        lblMsg.CssClass = "label label-danger";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "failalert('" + lblMsg.Text + "');", true);
                    }
                    else
                    {
                        lblMsg.Text = "Error- Insert/Update STATUS Details...!";
                        lblMsg.CssClass = "label label-danger";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "failalert('" + lblMsg.Text + "');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmdSave.Dispose();
                con.Close();
                con.Dispose();
            }
        }

        private void funcClear()
        {
            txtEmailID.Text = "";
            ddlAuthority.SelectedIndex = 0;
            ddlAuthorityDetail.Items.Clear();
        }

        private Boolean funcValidation()
        {
            Boolean Result = true;

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlAuthority)))
            {
                lblMsg.Text = "Please Select Authority from Dropdown.";
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "failalert('" + lblMsg.Text + "');", true);
                ddlAuthority.Focus();
                return Result = false;
            }
            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlAuthorityDetail)))
            {
                lblMsg.Text = "Please Select Authority Detail from Dropdown.";
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "failalert('" + lblMsg.Text + "');", true);
                ddlAuthorityDetail.Focus();
                return Result = false;
            }
            if (string.IsNullOrEmpty(txtEmailID.Text.Trim()))
            {
                lblMsg.Text = "Please enter Email ID";
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "failalert('" + lblMsg.Text + "');", true);
                txtEmailID.Focus();
                return Result = false;
            }

            return Result;
        }

        protected void ddlAuthority_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DisciplinaryAuthority = objCommonFunction.ddlSelectedValue(ddlAuthority);
            if (DisciplinaryAuthority != "0")
            {
                objCommonFunction.funcDisciplinaryAuthority(ddlAuthorityDetail, Session["ROLE"].ToString(), DisciplinaryAuthority);
            }
        }

        protected void ddlAuthorityDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            string AuthorityDetail = objCommonFunction.ddlSelectedValue(ddlAuthorityDetail);
            if (AuthorityDetail != "0")
            {
                objCommonFunction.funcMasterEmail_Get(txtEmailID, AuthorityDetail, "EMAIL_MASTER");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (funcValidation() == true)
            {
                if (!string.IsNullOrEmpty(txtEmailID.ToolTip))
                {
                    funcSubmit("U", txtEmailID.ToolTip);
                }
                else
                {
                    funcSubmit("I", "");
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            funcClear();
            lblMsg.Text = "";
        }
    }
}