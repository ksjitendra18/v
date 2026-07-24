using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace VMISP.Mis
{
    public partial class frmVigilanceUpdate : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                funcbindDropdown();     //Bind Circle Office DropDown List
                funcbindFormMasterDropdown(); //bind Register dropDown
            }

            lblMsg.Text = string.Empty;

            #region ** JS Function  **
            ddlField.Attributes.Add("onchange", "funchideUnhide_VigilanceUpdate('" + ddlField.ClientID + "','" + lblValueCaption.ClientID + "')");
            btnSubmit.Attributes.Add("onclick", "return funcValidation_VigilanceUpdate('" + txtRNo.ClientID + "','" + ddlField.ClientID + "','" + txtBASICPAY.ClientID + "','" + ddlDACOZOHO.ClientID + "','" + ddlRegister.ClientID + "','" + ddlPenaltyProceeding.ClientID + "')");
            #endregion
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
                cmd.Parameters.AddWithValue("@p_FORMTYPE", "VIGILANCE");
                cmd.CommandTimeout = 0;
                sda.Fill(dtCircleOffice);

                objCommonFunction.bindDropdownList(ddlDACOZOHO, dtCircleOffice);
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

        public void funcbindFormMasterDropdown()
        {
            DataSet dsFormMaster = new DataSet();
            SqlConnection conBind = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdBind = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmdBind);

            try
            {
                conBind.Open();
                cmdBind.Connection = conBind;
                cmdBind.Parameters.Clear();
                cmdBind.CommandType = CommandType.StoredProcedure;
                cmdBind.CommandText = "[dbo].[spMasterForm_Ddl]";
                cmdBind.Parameters.AddWithValue("@p_FORMTYPE", "VIGILANCE");
                cmdBind.CommandTimeout = 0;
                sda.Fill(dsFormMaster);

                if (dsFormMaster.Tables[4].Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlRegister, dsFormMaster.Tables[4]);      //Bind Register dropDown List
                }

                if (dsFormMaster.Tables[5].Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlPenaltyProceeding, dsFormMaster.Tables[5]);      //Bind Penalty Proceeding dropDown List
                }
            }
            catch (Exception es)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
            }
            finally
            {
                cmdBind.Dispose();
                sda.Dispose();
                conBind.Dispose();
                conBind.Close();
            }
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
                cmdSave.CommandText = "[dbo].[spVigilanceUser_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_RNO", txtRNo.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_FIELD", objCommonFunction.ddlSelectedValue(ddlField));
                cmdSave.Parameters.AddWithValue("@p_BASICPAY", txtBASICPAY.Text.Trim());
                cmdSave.Parameters.AddWithValue("@p_DACOZOHO", objCommonFunction.ddlSelectedValue(ddlDACOZOHO));
                cmdSave.Parameters.AddWithValue("@p_REGISTER", objCommonFunction.ddlSelectedValue(ddlRegister));
                cmdSave.Parameters.AddWithValue("@p_PENALTYPROCEEDING", objCommonFunction.convertToInt(objCommonFunction.ddlSelectedValue(ddlPenaltyProceeding)));
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

        public void funcClear()
        {
            txtRNo.Text = "";
            txtBASICPAY.Text = "";
            objCommonFunction.ddlSetDataValue(ddlDACOZOHO, "0");
            objCommonFunction.ddlSetDataValue(ddlRegister, "0");
            objCommonFunction.ddlSetDataValue(ddlPenaltyProceeding, "0");
        }

        public void funcHideUnhide()
        {
            if (hidColumnDataType.Value.ToUpper() == "BASICPAY")
            {
                lblValueCaption.Text = "Basic Pay :";
                divDACOZOHO.Style.Add("display", "none");
                divREGISTER.Style.Add("display", "none");
                divPENALTYPROCEEDING.Style.Add("display", "none");
                divBASICPAY.Style.Add("display", "block");
            }
            else if (hidColumnDataType.Value.ToUpper() == "DA_CO_ZO_HO")
            {
                lblValueCaption.Text = "DA_CO/ZO/HO :";
                divBASICPAY.Style.Add("display", "none");
                divREGISTER.Style.Add("display", "none");
                divPENALTYPROCEEDING.Style.Add("display", "none");
                divDACOZOHO.Style.Add("display", "block");
            }
            else if (hidColumnDataType.Value.ToUpper() == "REGISTER")
            {
                lblValueCaption.Text = "Register :";
                divBASICPAY.Style.Add("display", "none");
                divDACOZOHO.Style.Add("display", "none");
                divPENALTYPROCEEDING.Style.Add("display", "none");
                divREGISTER.Style.Add("display", "block");
            }
            else if (hidColumnDataType.Value.ToUpper() == "PENALTYPROCEEDING")
            {
                lblValueCaption.Text = "Penalty Proceeding :";
                divBASICPAY.Style.Add("display", "none");
                divDACOZOHO.Style.Add("display", "none");
                divREGISTER.Style.Add("display", "none");
                divPENALTYPROCEEDING.Style.Add("display", "block");
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

    }
}