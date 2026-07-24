using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace VMISP.Mis
{
    public partial class frmComplaintUpdate : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                funcbindDropdown();     //Bind Circle Office DropDown List
            }

            btnSubmit.Attributes.Add("onclick", "return funcValidation_ComplaintUpdate('" + txtRNo.ClientID + "','" + ddlField.ClientID + "','" + ddlCircleOffice.ClientID + "','" + txtSentTo.ClientID + "')");
        }

        public void funcSave()
        {
            SqlConnection conSave = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();
            try
            {
                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spComplaintUser_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_RNO", txtRNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_FIELD", objCommonFunction.ddlSelectedValue(ddlField));
                cmdSave.Parameters.AddWithValue("@p_CIRCLEOFFICE", objCommonFunction.ddlSelectedText(ddlCircleOffice));
                cmdSave.Parameters.AddWithValue("@p_SENTTO", txtSentTo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["userid"]));

                cmdSave.ExecuteNonQuery();
                cmdSave.CommandTimeout = 0;

                lblMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
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

        public void funcbindDropdown()
        {

            DataTable dtCircleOffice = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spCircleOffice_Ddl]";
                cmd.Parameters.AddWithValue("@p_FORMTYPE", "COMPLAINT");
                cmd.CommandTimeout = 0;
                sda.Fill(dtCircleOffice);

                if (dtCircleOffice.Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlCircleOffice, dtCircleOffice);
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

        public void funcClear()
        {
            txtRNo.Text = string.Empty;
            ddlCircleOffice.SelectedIndex = 0;
            txtSentTo.Text = string.Empty;
        }

        public void funcHideUnhide()
        {
            if (hidColumnDataType.Value.ToUpper() == "CIRCLE")
            {
                lblValueCaption.Text = "Circle Office :";
                ddlCircleOffice.Visible = true;
                txtSentTo.Visible = false;

                txtSentTo.Text = "";
            }
            else
            {
                lblValueCaption.Text = "Sent To :";
                ddlCircleOffice.Visible = false;
                txtSentTo.Visible = true;
                ddlCircleOffice.SelectedIndex = 0;
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            funcSave();
            funcClear();
            funcHideUnhide();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            funcClear();
        }

        protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (objCommonFunction.ddlSelectedValue(ddlField).ToUpper().Equals("CIRCLE"))
            {
                lblValueCaption.Text = "Circle Office :";
                ddlCircleOffice.Visible = true;
                txtSentTo.Visible = false;
                txtSentTo.Text = "";
            }
            if (objCommonFunction.ddlSelectedValue(ddlField).ToUpper().Equals("SENTTO"))
            {
                lblValueCaption.Text = "Sent To :";
                ddlCircleOffice.Visible = false;
                txtSentTo.Visible = true;
                ddlCircleOffice.SelectedIndex = 0;
            }
            else
            {
                lblValueCaption.Text = "";
                ddlCircleOffice.Visible = false;
                txtSentTo.Visible = false;
                ddlCircleOffice.SelectedIndex = 0;
                txtSentTo.Text = "";
            }
        }
    }
}