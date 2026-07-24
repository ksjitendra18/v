using ClosedXML.Excel;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace VMISP.Mis
{
    public partial class SanctionDataUpload : System.Web.UI.Page
    {
        string strErrMsg = string.Empty;
        int intErrCode = 0;
        int TotalRowInsert = 0;

        string UNIQUEID = string.Empty;
        string FILEUPLOADFOR = string.Empty;
        string ROWNO = string.Empty;
        string SINO = string.Empty;
        string SPNO = string.Empty;
        string RCNO = string.Empty;
        DateTime? RCDATE = null;
        DateTime? RECEIVED_REPORT_DATE = null;
        string PFNO = string.Empty;
        string NAME = string.Empty;
        string DESIGNATION = string.Empty;
        string CIRCLE_SOLID = string.Empty;
        string BRANCH_SOLID = string.Empty;
        string DA_SOLID = string.Empty;
        string DA_VIEW = string.Empty;
        DateTime? LETTER_TO_CBI_DATE = null;
        DateTime? LETTER_TO_CVC_DATE = null;
        string CVC_VIEW = string.Empty;
        DateTime? LETTER_TO_DA_DATE = null;
        DateTime? DA_ORDER_TOCBI_DATE = null;
        string STATUS_CODE = string.Empty;
        string REMARKS = string.Empty;
        string LETTER_TO_CBI_SENTBY = string.Empty;

        string FILENAME = string.Empty;
        string FILEEXTENSION = string.Empty;
        string FOLDERPATH = string.Empty;
        string FILEPATH = string.Empty;

        CommonFunction objCommonFunction = new CommonFunction();
        StringBuilder strScript = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();

            }

            lblMsg.Text = string.Empty;
        }

        public Boolean funcValidation(string TYPE)
        {
            Boolean Reslut = true;
            if (TYPE.Equals("VERIFY"))
            {
                if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlDataUploadFor)))
                {
                    lblMsg.Text = "Please select Data upload for";
                    ddlDataUploadFor.Focus();
                    Reslut = false;
                }

                if (!(fileUpload.HasFile))
                {
                    lblMsg.Text = "Please select file";
                    fileUpload.Focus();
                    Reslut = false;
                }
            }

            else if (TYPE.Equals("DOWNLOAD"))
            {
                if (string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlDataUploadFor)))
                {
                    lblMsg.Text = "Please select Data upload for";
                    ddlDataUploadFor.Focus();
                    Reslut = false;
                }
            }

            return Reslut;
        }

        public void funcDownloadFileFormat(string VIEW)
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
                cmd.CommandText = "[dbo].[spSanctionFileFormat]";
                cmd.Parameters.AddWithValue("@p_TABLENAME", VIEW);
                cmd.CommandTimeout = 0;

                sda.Fill(dt);

                if (VIEW.Equals("INVESTIGATION"))
                {
                    funcConvertToExcel_Investigation(dt);
                }
                else if (VIEW.Equals("PROSECUTION"))
                {
                    funcConvertToExcel_Prosecution(dt);
                }
            }
            catch (Exception e)
            {
                lblMsg.Text = e.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e);
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
            ddlDataUploadFor.SelectedIndex = 0;
            btnSubmit.Visible = false;
        }

        public void funcConvertToExcel_Investigation(DataTable dt)
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Investigation");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("", "");
                    Response.AddHeader("content-disposition", "attachment;filename=Investigation.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception e1)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e1);
            }
        }

        public void funcConvertToExcel_Prosecution(DataTable dt)
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Investigation");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("", "");
                    Response.AddHeader("content-disposition", "attachment;filename=Investigation.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception e1)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e1);
            }
        }

        protected void funcExcelVerify_Investigation(string FILEPATH, string FILEEXTENSION, string FILEUPLOADFOR)
        {
            string conStr = "";

            switch (FILEEXTENSION)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }

            conStr = String.Format(conStr, FILEPATH);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                ROWNO = Convert.ToString(row["ROWNO"]);
                SINO = Convert.ToString(row["SINO"]);
                RCNO = Convert.ToString(row["RCNO"]);

                if (string.IsNullOrEmpty(ROWNO))
                {
                    lblMsg.Text = "Row Number can not be blank.";
                    lblMsg.CssClass = "label label-danger";
                    return;
                }

                if (string.IsNullOrEmpty(SINO))
                {
                    lblMsg.Text = "SI Number can not be blank." + " Row Number of Excel " + ROWNO;
                    lblMsg.CssClass = "label label-danger";
                    return;
                }

                if (string.IsNullOrEmpty(RCNO))
                {
                    lblMsg.Text = "RC Number can not be blank." + " Row Number of Excel " + ROWNO;
                    lblMsg.CssClass = "label label-danger";
                    return;
                }

                if (string.IsNullOrEmpty(Convert.ToString(row["RCDATE"])))
                {
                    lblMsg.Text = "RC Date can not be blank." + " Row Number of Excel " + ROWNO;
                    lblMsg.CssClass = "label label-danger";
                    return;
                }

                SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                SqlCommand cmd = new SqlCommand();

                try
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spSanctionDataVerify]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_ROWNO", ROWNO);
                    cmd.Parameters.AddWithValue("@p_SINO", SINO);
                    cmd.Parameters.AddWithValue("@p_RCNO", RCNO);
                    cmd.Parameters.AddWithValue("@p_FILEUPLOADFOR", FILEUPLOADFOR);

                    cmd.CommandTimeout = 0;

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        strErrMsg = sqlErrMsgOutput.Value.ToString();
                        intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                        lblMsg.Text = strErrMsg;
                        lblMsg.CssClass = "label label-danger";

                        if (intErrCode.Equals(2))
                        {
                            btnSubmit.Visible = true;
                            ViewState["INVESTIGATION"] = dt;
                            lblMsg.CssClass = "label label-success";
                        }
                    }
                }
                catch (Exception e4)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e4);
                }

                finally
                {
                    cmd.Dispose();
                    con.Dispose();
                    con.Close();
                }
            }
        }

        protected void funcExcelVerify_Prosecution(string FILEPATH, string FILEEXTENSION, string FILEUPLOADFOR)
        {
            string conStr = "";

            switch (FILEEXTENSION)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }

            conStr = String.Format(conStr, FILEPATH);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            foreach (DataRow row in dt.Rows)
            {
                ROWNO = Convert.ToString(row["ROWNO"]);
                SPNO = Convert.ToString(row["SPNO"]);
                RCNO = Convert.ToString(row["RCNO"]);

                if (string.IsNullOrEmpty(ROWNO))
                {
                    lblMsg.Text = "Row Number can not be blank.";
                    lblMsg.CssClass = "label label-danger";
                    return;
                }

                if (string.IsNullOrEmpty(SPNO))
                {
                    lblMsg.Text = "SP Number can not be blank." + " Row Number of Excel " + ROWNO;
                    lblMsg.CssClass = "label label-danger";
                    return;
                }

                if (string.IsNullOrEmpty(RCNO))
                {
                    lblMsg.Text = "RC Number can not be blank." + " Row Number of Excel " + ROWNO;
                    lblMsg.CssClass = "label label-danger";
                    return;
                }

                if (string.IsNullOrEmpty(Convert.ToString(row["RCDATE"])))
                {
                    lblMsg.Text = "RC Date can not be blank." + " Row Number of Excel " + ROWNO;
                    lblMsg.CssClass = "label label-danger";
                    return;
                }

                SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                SqlCommand cmd = new SqlCommand();

                try
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spSanctionDataVerify]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_ROWNO", ROWNO);
                    cmd.Parameters.AddWithValue("@p_SINO", SPNO);
                    cmd.Parameters.AddWithValue("@p_RCNO", RCNO);
                    cmd.Parameters.AddWithValue("@p_FILEUPLOADFOR", FILEUPLOADFOR);

                    cmd.CommandTimeout = 0;

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        strErrMsg = sqlErrMsgOutput.Value.ToString();
                        intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                        lblMsg.Text = strErrMsg;
                        lblMsg.CssClass = "label label-danger";

                        if (intErrCode.Equals(2))
                        {
                            btnSubmit.Visible = true;
                            ViewState["PROSECUTION"] = dt;
                            lblMsg.CssClass = "label label-success";
                        }
                    }
                }
                catch (Exception e4)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e4);
                }

                finally
                {
                    cmd.Dispose();
                    con.Dispose();
                    con.Close();
                }
            }
        }

        protected void funcSubmit_Investigation()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["INVESTIGATION"]);
            if (dt != null)
            {
                SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                SqlTransaction txn = cmd.Connection.BeginTransaction();
                cmd.Transaction = txn;

                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        SINO = Convert.ToString(row["SINO"]);
                        RCNO = Convert.ToString(row["RCNO"]);
                        string strRCDATE = Convert.ToString(row["RCDATE"]);
                        string strREPORTDATE = Convert.ToString(row["RECEIVED_REPORT_DATE"]);
                        PFNO = Convert.ToString(row["PFNO"]);
                        NAME = Convert.ToString(row["NAME"]);
                        DESIGNATION = Convert.ToString(row["DESIGNATION"]);
                        CIRCLE_SOLID = Convert.ToString(row["CIRCLE_SOLID"]);
                        BRANCH_SOLID = Convert.ToString(row["BRANCH_SOLID"]);
                        DA_SOLID = Convert.ToString(row["DA_SOLID"]);
                        DA_VIEW = Convert.ToString(row["DA_VIEW"]);
                        string strCBIDATE = Convert.ToString(row["LETTER_TO_CBI_DATE"]);
                        LETTER_TO_CBI_SENTBY = Convert.ToString(row["LETTER_TO_CBI_SENTBY"]);
                        STATUS_CODE = Convert.ToString(row["STATUS_CODE"]);
                        REMARKS = Convert.ToString(row["REMARKS"]);

                        if (!string.IsNullOrEmpty(strRCDATE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strRCDATE, out date))
                                RCDATE = date;
                        }

                        if (!string.IsNullOrEmpty(strREPORTDATE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strREPORTDATE, out date))
                                RECEIVED_REPORT_DATE = date;
                        }

                        if (!string.IsNullOrEmpty(strCBIDATE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strCBIDATE, out date))
                                LETTER_TO_CBI_DATE = date;
                        }

                        string ID = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                        UNIQUEID = "SFI" + DateTime.Now.ToString("ddMMyy") + ID;

                        cmd.Connection = con;
                        cmd.Parameters.Clear();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[dbo].[spSanctionForInvestigation_Upload]";

                        SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                        SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(sqlErrMsgOutput);
                        cmd.Parameters.Add(sqlErrCodeOutput);

                        cmd.Parameters.AddWithValue("@p_UNIQUENO", UNIQUEID);
                        cmd.Parameters.AddWithValue("@p_SINO", SINO);
                        cmd.Parameters.AddWithValue("@p_RCNO", RCNO);
                        cmd.Parameters.AddWithValue("@p_PFNO", PFNO);
                        cmd.Parameters.AddWithValue("@p_NAME", NAME);
                        cmd.Parameters.AddWithValue("@p_DESIGNATION", DESIGNATION);
                        cmd.Parameters.AddWithValue("@p_CIRCLE", CIRCLE_SOLID);
                        cmd.Parameters.AddWithValue("@p_BRANCH", BRANCH_SOLID);
                        cmd.Parameters.AddWithValue("@p_DA", DA_SOLID);
                        cmd.Parameters.AddWithValue("@p_DAVIEW", DA_VIEW);
                        cmd.Parameters.AddWithValue("@p_LETTERTOCBISENTBY", LETTER_TO_CBI_SENTBY);
                        cmd.Parameters.AddWithValue("@p_STATUS", STATUS_CODE);
                        cmd.Parameters.AddWithValue("@p_REMARKS", REMARKS);
                        cmd.Parameters.AddWithValue("@p_RCDATE", RCDATE);
                        cmd.Parameters.AddWithValue("@p_REPORTDATE", RECEIVED_REPORT_DATE);
                        cmd.Parameters.AddWithValue("@p_LETTERTOCBIDATE", LETTER_TO_CBI_DATE);

                        cmd.Parameters.AddWithValue("@p_USER", Session["userid"].ToString());
                        cmd.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                        cmd.Parameters.AddWithValue("@p_USERROLE", Session["role"].ToString());
                        cmd.CommandTimeout = 0;

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            TotalRowInsert = TotalRowInsert + 1;
                            strErrMsg = sqlErrMsgOutput.Value.ToString();
                            intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                        }

                        else
                        {
                            lblMsg.Text = "Error during Insert data in Sanction for Investigation.";
                            return;
                        }
                    }

                    if (intErrCode.Equals(1))
                    {
                        txn.Commit();//Success all
                        lblMsg.Text = "Total " + TotalRowInsert + " - " + strErrMsg;
                    }
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    lblMsg.Text = ex.Message;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
                }
                finally
                {
                    txn.Dispose();
                    cmd.Dispose();
                    con.Close();
                    con.Dispose();
                }
            }
            else
            {
                lblMsg.Text = "Error - Upload Investigation Details";
                lblMsg.CssClass = "label label-danger";
            }
        }

        protected void funcSubmit_Prosecution()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)ViewState["PROSECUTION"]);
            if (dt != null)
            {
                SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                SqlTransaction txn = cmd.Connection.BeginTransaction();
                cmd.Transaction = txn;

                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        SPNO = Convert.ToString(row["SPNO"]);
                        RCNO = Convert.ToString(row["RCNO"]);
                        string strRCDATE = Convert.ToString(row["RCDATE"]);
                        string strREPORTDATE = Convert.ToString(row["RECEIVED_REPORT_DATE"]);
                        PFNO = Convert.ToString(row["PFNO"]);
                        NAME = Convert.ToString(row["NAME"]);
                        DESIGNATION = Convert.ToString(row["DESIGNATION"]);
                        CIRCLE_SOLID = Convert.ToString(row["CIRCLE_SOLID"]);
                        BRANCH_SOLID = Convert.ToString(row["BRANCH_SOLID"]);
                        DA_SOLID = Convert.ToString(row["DA_SOLID"]);
                        DA_VIEW = Convert.ToString(row["DA_VIEW"]);
                        string strCBIDATE = Convert.ToString(row["LETTER_TO_CBI_DATE"]);
                        string stsCVCDATE = Convert.ToString(row["LETTER_TO_CVC_DATE"]);
                        CVC_VIEW = Convert.ToString(row["CVC_VIEW"]);
                        string strDADATE = Convert.ToString(row["LETTER_TO_DA_DATE"]);
                        string strDACBIDATE = Convert.ToString(row["DA_ORDER_TOCBI_DATE"]);
                        STATUS_CODE = Convert.ToString(row["STATUS_CODE"]);
                        REMARKS = Convert.ToString(row["REMARKS"]);

                        if (!string.IsNullOrEmpty(strRCDATE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strRCDATE, out date))
                                RCDATE = date;
                        }

                        if (!string.IsNullOrEmpty(strREPORTDATE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strREPORTDATE, out date))
                                RECEIVED_REPORT_DATE = date;
                        }

                        if (!string.IsNullOrEmpty(strCBIDATE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strCBIDATE, out date))
                                LETTER_TO_CBI_DATE = date;
                        }

                        if (!string.IsNullOrEmpty(stsCVCDATE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(stsCVCDATE, out date))
                                LETTER_TO_CVC_DATE = date;
                        }

                        if (!string.IsNullOrEmpty(strDADATE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDADATE, out date))
                                LETTER_TO_DA_DATE = date;
                        }

                        if (!string.IsNullOrEmpty(strDACBIDATE))
                        {
                            DateTime date;
                            if (DateTime.TryParse(strDACBIDATE, out date))
                                DA_ORDER_TOCBI_DATE = date;
                        }

                        string ID = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                        UNIQUEID = "SFP" + DateTime.Now.ToString("ddMMyy") + ID;

                        cmd.Connection = con;
                        cmd.Parameters.Clear();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[dbo].[spSanctionForProsecution_Upload]";

                        SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                        SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(sqlErrMsgOutput);
                        cmd.Parameters.Add(sqlErrCodeOutput);

                        cmd.Parameters.AddWithValue("@p_UNIQUENO", UNIQUEID);
                        cmd.Parameters.AddWithValue("@p_SINO", SPNO);
                        cmd.Parameters.AddWithValue("@p_RCNO", RCNO);
                        cmd.Parameters.AddWithValue("@p_PFNO", PFNO);
                        cmd.Parameters.AddWithValue("@p_NAME", NAME);
                        cmd.Parameters.AddWithValue("@p_DESIGNATION", DESIGNATION);
                        cmd.Parameters.AddWithValue("@p_CIRCLE", CIRCLE_SOLID);
                        cmd.Parameters.AddWithValue("@p_BRANCH", BRANCH_SOLID);
                        cmd.Parameters.AddWithValue("@p_DA", DA_SOLID);
                        cmd.Parameters.AddWithValue("@p_DAVIEW", DA_VIEW);
                        cmd.Parameters.AddWithValue("@p_STATUS", STATUS_CODE);
                        cmd.Parameters.AddWithValue("@p_REMARKS", REMARKS);

                        cmd.Parameters.AddWithValue("@p_RCDATE", RCDATE);
                        cmd.Parameters.AddWithValue("@p_REPORTDATE", RECEIVED_REPORT_DATE);
                        cmd.Parameters.AddWithValue("@p_LETTERTOCBIDATE", LETTER_TO_CBI_DATE);
                        cmd.Parameters.AddWithValue("@p_LETTERTOCVCDATE", LETTER_TO_CVC_DATE);
                        cmd.Parameters.AddWithValue("@p_LETTERTODADATE", LETTER_TO_DA_DATE);
                        cmd.Parameters.AddWithValue("@p_DAORDERTOCBIDATE", DA_ORDER_TOCBI_DATE);

                        cmd.Parameters.AddWithValue("@p_USER", Session["userid"].ToString());
                        cmd.Parameters.AddWithValue("@p_USERIP", objCommonFunction.funcGetUserIP());
                        cmd.Parameters.AddWithValue("@p_USERROLE", Session["role"].ToString());
                        cmd.CommandTimeout = 0;

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            TotalRowInsert = TotalRowInsert + 1;
                            strErrMsg = sqlErrMsgOutput.Value.ToString();
                            intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);
                        }

                        else
                        {
                            lblMsg.Text = "Error during Insert data in Sanction for Prosecution.";
                            return;
                        }
                    }

                    if (intErrCode.Equals(1))
                    {
                        txn.Commit();//Success all
                        lblMsg.Text = "Total " + TotalRowInsert + " - " + strErrMsg;
                    }
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    lblMsg.Text = ex.Message;
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
                }
                finally
                {
                    txn.Dispose();
                    cmd.Dispose();
                    con.Close();
                    con.Dispose();
                }
            }
            else
            {
                lblMsg.Text = "Error - Upload Prosecution Details";
                lblMsg.CssClass = "label label-danger";
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (funcValidation("VERIFY") == true)
            {
                try
                {
                    FILEUPLOADFOR = objCommonFunction.ddlSelectedValue(ddlDataUploadFor);

                    if (fileUpload.HasFile)
                    {
                        FILENAME = Path.GetFileName(fileUpload.PostedFile.FileName);
                        FILEEXTENSION = Path.GetExtension(fileUpload.PostedFile.FileName);
                        FOLDERPATH = ConfigurationManager.AppSettings["ExcelFolderPath"];
                        FILEPATH = Server.MapPath(FOLDERPATH + FILENAME);

                        fileUpload.SaveAs(FILEPATH);

                        if (FILEUPLOADFOR.ToUpper().Equals("INVESTIGATION"))
                        {
                            funcExcelVerify_Investigation(FILEPATH, FILEEXTENSION, FILEUPLOADFOR);
                        }
                        else if (FILEUPLOADFOR.ToUpper().Equals("PROSECUTION"))
                        {
                            funcExcelVerify_Prosecution(FILEPATH, FILEEXTENSION, FILEUPLOADFOR);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.Message;
                    lblMsg.CssClass = "label label-danger";
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            FILEUPLOADFOR = objCommonFunction.ddlSelectedValue(ddlDataUploadFor);
            if (FILEUPLOADFOR.Equals("INVESTIGATION"))
            {
                funcSubmit_Investigation();
            }

            else if (FILEUPLOADFOR.Equals("PROSECUTION"))
            {
                funcSubmit_Prosecution();
            }
        }

        protected void btndownload_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (funcValidation("DOWNLOAD") == true)
            {
                funcDownloadFileFormat(objCommonFunction.ddlSelectedValue(ddlDataUploadFor));
            }
        }

    }
}