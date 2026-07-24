using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace VMISP.DataAccessLayer
{
    public class MasterData
    {
        CommonFunction objCommonFunction = new CommonFunction();
        public void funcRoleMaster(DropDownList objDropDownList)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dt.Clear();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spUserRole_Ddl]";

                cmd.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmd.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(objDropDownList, dt);
                }
            }

            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }

            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public void funcRoleDescriptionMaster(DropDownList ddlDescription, DropDownList ddlUserType, string ROLE)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            ds.Clear();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spRoleDescription_Ddl]";

                cmd.Parameters.AddWithValue("@p_DDL_ROLE", ROLE);
                cmd.Parameters.AddWithValue("@p_LOGIN_ROLE", HttpContext.Current.Session["ROLE"]);
                cmd.Parameters.AddWithValue("@p_SOLID", HttpContext.Current.Session["SOLID"]);
                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlDescription, ds.Tables[0]);

                    if (ddlUserType != null)
                    {
                        objCommonFunction.bindDropdownList(ddlUserType, ds.Tables[1]);
                    }
                }
            }

            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }

            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public string funcCircleZone(string CIRCLESOLID)
        {
            String strResult = String.Empty;
            SqlConnection conGetData = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdGetData = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conGetData.Open();
                cmdGetData.Connection = conGetData;
                cmdGetData.Parameters.Clear();
                cmdGetData.CommandType = CommandType.StoredProcedure;
                cmdGetData.CommandText = "[dbo].[spZoneCode_Get]";

                cmdGetData.Parameters.AddWithValue("@p_CIRCLEID", CIRCLESOLID);

                cmdGetData.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmdGetData);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    strResult = Convert.ToString(dt.Rows[0]["CODE"]);
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmdGetData.Dispose();
                conGetData.Dispose();
                conGetData.Close();
            }
            return strResult;
        }

        public string funcCircleZoneMaster(string CIRCLESOLID, string VIEW)
        {
            String strResult = String.Empty;
            SqlConnection conGetData = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdGetData = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conGetData.Open();
                cmdGetData.Connection = conGetData;
                cmdGetData.Parameters.Clear();
                cmdGetData.CommandType = CommandType.StoredProcedure;
                cmdGetData.CommandText = "[dbo].[spCircleZoneMaster_Ddl]";

                cmdGetData.Parameters.AddWithValue("@p_CIRCLEID", CIRCLESOLID);
                cmdGetData.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmdGetData.Parameters.AddWithValue("@p_VIEW", VIEW);

                cmdGetData.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmdGetData);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    strResult = Convert.ToString(dt.Rows[0]["NAME"]);
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmdGetData.Dispose();
                conGetData.Dispose();
                conGetData.Close();
            }
            return strResult;
        }

        public void funcCircleBranch(DropDownList objDropDownList, string CIRCLESOLID, string VIEW)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spCircleBranchMaster_Ddl]";

                cmd.Parameters.AddWithValue("@p_CIRCLEID", CIRCLESOLID);
                cmd.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmd.Parameters.AddWithValue("@p_VIEW", VIEW);

                cmd.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(objDropDownList, dt);
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmd.Dispose();
                con.Dispose();
                con.Close();
            }
        }

        public void funcBranchMaster(DropDownList objDropDownList, string BRANCHSOLID, string VIEW)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spBranchMaster_Ddl]";

                cmd.Parameters.AddWithValue("@p_SOLID", BRANCHSOLID);
                cmd.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmd.Parameters.AddWithValue("@p_VIEW", VIEW);

                cmd.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(objDropDownList, dt);
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmd.Dispose();
                con.Dispose();
                con.Close();
            }
        }

        public string funcGetBranchCircle(string SOLID)
        {
            String strResult = String.Empty;
            SqlConnection conGetData = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdGetData = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conGetData.Open();
                cmdGetData.Connection = conGetData;
                cmdGetData.Parameters.Clear();
                cmdGetData.CommandType = CommandType.StoredProcedure;
                cmdGetData.CommandText = "[dbo].[spBranchCircle_Get]";

                cmdGetData.Parameters.AddWithValue("@p_SOLID", SOLID);

                cmdGetData.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmdGetData);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    strResult = Convert.ToString(dt.Rows[0]["CODE"]);
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmdGetData.Dispose();
                conGetData.Dispose();
                conGetData.Close();
            }
            return strResult;
        }

        public void funcBindBranchCircle(DropDownList objDropDownList, string BRANCH_SOLID, string VIEW)
        {
            String strResult = String.Empty;
            SqlConnection conGetData = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdGetData = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conGetData.Open();
                cmdGetData.Connection = conGetData;
                cmdGetData.Parameters.Clear();
                cmdGetData.CommandType = CommandType.StoredProcedure;
                cmdGetData.CommandText = "[dbo].[spBranchCircle_Ddl]";

                cmdGetData.Parameters.AddWithValue("@p_SOLID", BRANCH_SOLID);
                cmdGetData.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmdGetData.Parameters.AddWithValue("@p_VIEW", VIEW);

                cmdGetData.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmdGetData);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(objDropDownList, dt);
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmdGetData.Dispose();
                conGetData.Dispose();
                conGetData.Close();
            }
        }

        public string funcUserEmailID(string USERID)
        {
            String strResult = String.Empty;
            SqlConnection conGetData = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdGetData = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conGetData.Open();
                cmdGetData.Connection = conGetData;
                cmdGetData.Parameters.Clear();
                cmdGetData.CommandType = CommandType.StoredProcedure;
                cmdGetData.CommandText = "[dbo].[spUserEmaild_Get]";

                cmdGetData.Parameters.AddWithValue("@p_USERID", USERID);

                cmdGetData.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmdGetData);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    strResult = Convert.ToString(dt.Rows[0]["EMAILID"]);
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmdGetData.Dispose();
                conGetData.Dispose();
                conGetData.Close();
            }

            return strResult;
        }

        public void funcCircleMaster(DropDownList objDropDownList, string VIEW)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dt.Clear();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spCircleMaster_Ddl]";

                cmd.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmd.Parameters.AddWithValue("@p_SOLID", HttpContext.Current.Session["SOLID"]);
                cmd.Parameters.AddWithValue("@p_VIEW", VIEW);
                cmd.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(objDropDownList, dt);
                }
            }

            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }

            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public void funcMasterEmailID(TextBox objTextBox, string VIEW)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spMasterEmailID_Get]";

                cmd.Parameters.AddWithValue("@p_SOLID", HttpContext.Current.Session["SOLID"]);
                cmd.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmd.Parameters.AddWithValue("@p_VIEW", VIEW);

                cmd.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    objTextBox.Text = Convert.ToString(dt.Rows[0]["EMAILID"]).Trim();
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmd.Dispose();
                con.Dispose();
                con.Close();
            }
        }

        public string funcUpdateMailStatusofForgetPassword(string USERID, string MAIL_BODY)
        {
            string Result = "N";
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spForgetPasswordMailStatus_Update]";

                cmd.Parameters.AddWithValue("@p_USERID", USERID);
                cmd.Parameters.AddWithValue("@p_MAIL_BODY", MAIL_BODY);

                cmd.CommandTimeout = 0;

                if (cmd.ExecuteNonQuery() > 0)
                {
                    Result = "Y";
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmd.Dispose();
                cmd.Dispose();
                con.Close();
            }

            return Result;
        }

        public string funcGetPasswordUserID(string USERID, string OLDPASSWORD)
        {
            String Result = String.Empty;
            SqlConnection conGetData = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdGetData = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conGetData.Open();
                cmdGetData.Connection = conGetData;
                cmdGetData.Parameters.Clear();
                cmdGetData.CommandType = CommandType.StoredProcedure;
                cmdGetData.CommandText = "[dbo].[spPasswordUserID_Get]";

                cmdGetData.Parameters.AddWithValue("@p_USERID", USERID);
                cmdGetData.Parameters.AddWithValue("@p_OLDPASSWORD", OLDPASSWORD);

                cmdGetData.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmdGetData);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Result = Convert.ToString(dt.Rows[0]["USERID"]);
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmdGetData.Dispose();
                conGetData.Dispose();
                conGetData.Close();
            }
            return Result;
        }

        public DataTable funcValidateForgetPasswordUID(string UID)
        {
            SqlConnection conGetData = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdGetData = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conGetData.Open();
                cmdGetData.Connection = conGetData;
                cmdGetData.Parameters.Clear();
                cmdGetData.CommandType = CommandType.StoredProcedure;
                cmdGetData.CommandText = "[dbo].[spFogetPasswordUIDValidate]";

                cmdGetData.Parameters.AddWithValue("@p_UID", UID);

                cmdGetData.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmdGetData);
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    //retrun
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmdGetData.Dispose();
                conGetData.Dispose();
                conGetData.Close();
            }

            return dt;
        }

        public DataTable funcUploadedDocument(string UNIQUEID, string REQUESTEDFORM)
        {
            SqlConnection conGetData = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmdGetData = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conGetData.Open();
                cmdGetData.Connection = conGetData;
                cmdGetData.Parameters.Clear();
                cmdGetData.CommandType = CommandType.StoredProcedure;
                cmdGetData.CommandText = "[dbo].[spUplodedFile_Get]";

                cmdGetData.Parameters.AddWithValue("@p_UNIQUEID", UNIQUEID);
                cmdGetData.Parameters.AddWithValue("@p_REQUESTFORM", REQUESTEDFORM);

                cmdGetData.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmdGetData);
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    //retrun
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmdGetData.Dispose();
                conGetData.Dispose();
                conGetData.Close();
            }

            return dt;
        }

        public string funcLogout(string UNIQUEID)
        {
            string Result = "N";
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spLogout]";

                cmd.Parameters.AddWithValue("@p_UNIQUEID", UNIQUEID);

                cmd.CommandTimeout = 0;

                if (cmd.ExecuteNonQuery() > 0)
                {
                    Result = "Y";
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmd.Dispose();
                cmd.Dispose();
                con.Close();
            }

            return Result;
        }

        public DataTable funcGetTableFormat(string TABLENAME)
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
                cmd.CommandText = "[dbo].[spTableFormat_Get]";
                cmd.Parameters.AddWithValue("@p_TABLENAME", TABLENAME);
                cmd.CommandTimeout = 0;

                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
            return dt;
        }

        public string funcDelete(string UNIQUEID, string TABLENAME)
        {
            string Result = "N";
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            SqlTransaction txn = cmd.Connection.BeginTransaction();
            cmd.Transaction = txn;

            try
            {
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDelete]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(sqlErrMsgOutput);
                cmd.Parameters.Add(sqlErrCodeOutput);

                cmd.Parameters.AddWithValue("@p_UNIQUEID", UNIQUEID);
                cmd.Parameters.AddWithValue("@p_USER", HttpContext.Current.Session["USERID"]);
                cmd.Parameters.AddWithValue("@p_TABLENAME", TABLENAME);

                cmd.CommandTimeout = 0;

                if (cmd.ExecuteNonQuery() > 0)
                {
                    txn.Commit();//Success all
                    Result = Convert.ToString(sqlErrMsgOutput.Value);
                }
            }
            catch (Exception ex)
            {
                txn.Rollback();
                Result = "Y";
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
                throw ex;
            }
            finally
            {
                txn.Dispose();
                cmd.Dispose();
                cmd.Dispose();
                con.Close();
            }

            return Result;
        }

        public void funcStatusMaster(DropDownList objDropDownList, string TYPE)
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
                cmd.CommandText = "[dbo].[spStatusMaster_Ddl]";
                cmd.Parameters.AddWithValue("@p_USERID", HttpContext.Current.Session["USERID"]);
                cmd.Parameters.AddWithValue("@p_SOLID", HttpContext.Current.Session["SOLID"]);
                cmd.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
                cmd.CommandTimeout = 0;

                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(objDropDownList, dt);
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public void funcStatusMaster(DropDownList objDropDownList1, DropDownList objDropDownList2, string TYPE)
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
                cmd.CommandText = "[dbo].[spStatusMaster_Ddl]";
                cmd.Parameters.AddWithValue("@p_USERID", HttpContext.Current.Session["USERID"]);
                cmd.Parameters.AddWithValue("@p_SOLID", HttpContext.Current.Session["SOLID"]);
                cmd.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
                cmd.CommandTimeout = 0;

                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(objDropDownList1, dt);
                    objCommonFunction.bindDropdownList(objDropDownList2, dt);
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public void funcMasterEmail_Get(TextBox objTextBox, string SOLID, string FORM)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dt.Clear();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spEmailMaster_Get]";

                cmd.Parameters.AddWithValue("@p_SOLID", SOLID);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    if (FORM.Equals("EMAIL_MASTER"))
                    {
                        objTextBox.Text = Convert.ToString(dt.Rows[0]["EMAILID"]);
                        objTextBox.ToolTip = Convert.ToString(dt.Rows[0]["UNIQUEID"]);
                    }
                    else
                    {
                        objTextBox.Text = Convert.ToString(dt.Rows[0]["EMAILID"]);
                        objTextBox.ToolTip = "";
                    }
                }
                else
                {
                    objTextBox.Text = "";
                    objTextBox.ToolTip = "";
                }
            }

            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }

            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public void funcZoneCircle(DropDownList objDropDownList, string ZONESOLID, string VIEW)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spZoneCircleMaster_Ddl]";

                cmd.Parameters.AddWithValue("@p_SOLID", ZONESOLID);
                cmd.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmd.Parameters.AddWithValue("@p_VIEW", VIEW);

                cmd.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(objDropDownList, dt);
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmd.Dispose();
                con.Dispose();
                con.Close();
            }
        }

        public void funcZoneMaster(DropDownList objDropDownList, string VIEW)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dt.Clear();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spZoneMaster_Ddl]";

                cmd.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmd.Parameters.AddWithValue("@p_SOLID", HttpContext.Current.Session["SOLID"]);
                cmd.Parameters.AddWithValue("@p_VIEW", VIEW);
                cmd.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(objDropDownList, dt);
                }
            }

            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }

            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public void funcScaleMaster(DropDownList objDropDownList, string VIEW)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dt.Clear();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spScaleMaster_Ddl]";

                cmd.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmd.Parameters.AddWithValue("@p_VIEW", VIEW);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(objDropDownList, dt);
                }
            }

            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }

            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public void funcUserType_Ddl(DropDownList ddlUserType)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dt.Clear();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spUserType_Ddl]";

                cmd.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmd.Parameters.AddWithValue("@p_USERSOLID", HttpContext.Current.Session["SOLID"]);
                cmd.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlUserType, dt);
                }
            }

            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }

            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public void funcZoneTypeCM(TextBox objTextBox, string ZONE, string ZONETYPE)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dt.Clear();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spZoneTypeCM_Ddl]";

                cmd.Parameters.AddWithValue("@p_ROLE", HttpContext.Current.Session["ROLE"]);
                cmd.Parameters.AddWithValue("@p_SOLID", HttpContext.Current.Session["SOLID"]);
                cmd.Parameters.AddWithValue("@p_ZONE", ZONE);
                cmd.Parameters.AddWithValue("@p_ZONE_TYPE", ZONETYPE);
                cmd.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    objTextBox.Text = Convert.ToString(dt.Rows[0]["ZO_CM"]);
                }
            }

            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }

            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public string funcLockedError(string REQID, string FORMNAME, string OPERATION, string FUNCTION, string PROCNAME, string ERRORMSG)
        {
            string Result = "N";
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spLockedError]";

                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(sqlErrCodeOutput);

                cmd.Parameters.AddWithValue("@p_USER", HttpContext.Current.Session["USERID"]);
                cmd.Parameters.AddWithValue("@p_SOLID", HttpContext.Current.Session["SOLID"]);
                cmd.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                cmd.Parameters.AddWithValue("@p_REQID", REQID);
                cmd.Parameters.AddWithValue("@p_FORMNAME", FORMNAME);
                cmd.Parameters.AddWithValue("@p_OPERATION", OPERATION);
                cmd.Parameters.AddWithValue("@p_FUNCTION", FUNCTION);
                cmd.Parameters.AddWithValue("@p_PROCNAME", PROCNAME);
                cmd.Parameters.AddWithValue("@p_BROWSER", HttpContext.Current.Request.Browser.Browser);
                cmd.Parameters.AddWithValue("@p_ERRORMSG", ERRORMSG);

                cmd.CommandTimeout = 0;

                if (cmd.ExecuteNonQuery() > 0)
                {
                    Result = "Y";
                }
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
            finally
            {
                cmd.Dispose();
                cmd.Dispose();
                con.Close();
            }

            return Result;
        }

    }
}