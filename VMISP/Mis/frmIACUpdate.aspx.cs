using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;

namespace VMISP.Mis
{
    public partial class frmIACUpdate : System.Web.UI.Page
    {
        #region ** declare Variable **
        string strMsg = string.Empty;
        string strErrMsg = string.Empty;
        string strUser = string.Empty;
        int intErrCode = 0;

        string strIACNO = string.Empty;
        string strDA = string.Empty;
        CommonFunction objCommonFunction = new CommonFunction();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();
            }

            txtIACNo.Focus();
            lblMsg.Text = string.Empty;

            #region ** JS Function  **
            btnSubmit.Attributes.Add("onclick", "return funcValidation_IACUpdate('" + txtIACNo.ClientID + "','" + txtDA.ClientID + "')");
            #endregion
        }

        public void funcSave()
        {
            SqlConnection conSave = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdSave = new SqlCommand();
            try
            {
                strIACNO = txtIACNo.Text.Trim();
                strDA = txtDA.Text;
                strUser = ViewState["USERNAME"].ToString();

                #region ** call StoredProcedure to Save/Update data in Table  **
                conSave.Open();
                cmdSave.Connection = conSave;
                cmdSave.Parameters.Clear();
                cmdSave.CommandType = CommandType.StoredProcedure;
                cmdSave.CommandText = "[dbo].[spIACUser_Update]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmdSave.Parameters.Add(sqlErrMsgOutput);
                cmdSave.Parameters.Add(sqlErrCodeOutput);

                cmdSave.Parameters.AddWithValue("@p_IACNO", strIACNO);
                cmdSave.Parameters.AddWithValue("@p_DA", strDA);
                cmdSave.Parameters.AddWithValue("@p_USER", strUser);

                cmdSave.ExecuteNonQuery();
                cmdSave.CommandTimeout = 0;

                strErrMsg = sqlErrMsgOutput.Value.ToString();
                intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                #endregion
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
            txtIACNo.Text = string.Empty;
            txtDA.Text = string.Empty;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                funcSave();
                funcClear();
                lblMsg.Text = strErrMsg.ToString();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            funcClear();
        }

    }
}