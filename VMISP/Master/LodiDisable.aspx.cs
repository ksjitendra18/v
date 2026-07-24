using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace VMISP.Master
{
    public partial class LodiDisable : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (objCommonFunction.funcCheckUserRights("LODI_DISABLE") == false)
                {
                    Response.Redirect("~/Logout.aspx");
                }
            }
        }

        public void funcShow()
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
                cmdView.CommandText = "[dbo].[spLodiDisable_View]";

                cmdView.Parameters.AddWithValue("@p_YEAR", objCommonFunction.ddlSelectedValue(ddlYear));
                cmdView.Parameters.AddWithValue("@p_USERID", Convert.ToString(Session["USERID"]));
                cmdView.Parameters.AddWithValue("@p_USERROLE", Convert.ToString(Session["ROLE"]));

                cmdView.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["TOTAL_RECORDS"]) != "0")
                    {
                        lblTotalRecords.Text = Convert.ToString(dt.Rows[0]["TOTAL_RECORDS"]);
                        pnlDetails.Visible = true;
                    }
                    else
                    {
                        lblTotalRecords.Text = "";
                        lblMsg.Text = "Record not found for selected parameters";
                        pnlDetails.Visible = false;
                    }
                }
                else
                {
                    lblTotalRecords.Text = "";
                    lblMsg.Text = "Record not found for selected parameters";
                    pnlDetails.Visible = false;
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

        private void funcSubmit()
        {
            lblMsg.Text = "";
            lblUpdateMsg.Text = "";
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();
            con.Open();
            cmdSave.Connection = con;
            cmdSave.Parameters.Clear();

            try
            {
                string ID = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                string REFNO = "LD" + DateTime.Now.ToString("ddMMyy") + ID;

                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spLodi_Disable]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_USER", Convert.ToString(Session["USERID"]));
                cmdSave.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmdSave.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmdSave.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());

                cmdSave.Parameters.AddWithValue("@p_REFNO", REFNO);
                cmdSave.Parameters.AddWithValue("@p_YEAR", objCommonFunction.ddlSelectedValue(ddlYear));
                cmdSave.Parameters.AddWithValue("@p_REMARKS", txtRemarks.Text);
                cmdSave.CommandTimeout = 0;

                if (cmdSave.ExecuteNonQuery() > 0)
                {
                    txtRemarks.Text = "";
                    lblTotalRecords.Text = "";
                    lblUpdateMsg.Text = Convert.ToString(sqlErrMsgOutput.Value);
                }
                else
                {
                    lblUpdateMsg.Text = "Error- Insert/Update Zone Chief Manager Master Details...!";
                    lblUpdateMsg.CssClass = "label label-danger";
                }
            }
            catch (Exception ex)
            {
                lblUpdateMsg.Text = ex.Message;
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmdSave.Dispose();
                con.Close();
                con.Dispose();
            }
        }

        protected void btnGetDetails_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblUpdateMsg.Text = "";
            lblTotalRecords.Text = "";

            if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlYear)))
            {
                lblMsg.Text = "Please Select Year from Dropdown.";
                return;
            }

            funcShow();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblUpdateMsg.Text = "";
            if (string.IsNullOrEmpty(txtRemarks.Text))
            {
                lblUpdateMsg.Text = "Please Enter Remarks.";
                return;
            }

            funcSubmit();
        }
    }
}