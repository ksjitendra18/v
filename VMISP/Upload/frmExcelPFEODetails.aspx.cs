using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VMISP.Upload
{
    public partial class frmExcelPFEODetails : System.Web.UI.Page
    {
        StringBuilder strScript = new StringBuilder();
        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                lblMsg.Text = string.Empty;
            }

            btnUpload.Attributes.Add("onclick", "return funcUpload_Validation('" + ddlTableName.ClientID + "')");
        }

        protected void funcExcelImport_Get(string p_strFilePath, string p_strExtension)
        {
            lblMsg.Text = "";
            string conStr = "";
            string PFNUMBER = "";
            string PFNUMBERMAIN = "";
            DataTable dt = new DataTable();
            DataTable dtGrid = new DataTable();
            switch (p_strExtension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, p_strFilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
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
                DataTable dt1 = new DataTable();
                try
                {
                    PFNUMBER = Convert.ToString(row["PFNUMBER"]);
                }
                catch (Exception eee)
                {
                    VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(eee);
                    lblMsg.Text = "Upload Failed ! Please check your EXECL";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (!string.IsNullOrEmpty(PFNUMBERMAIN))
                {
                    PFNUMBERMAIN = PFNUMBERMAIN + '|'.ToString();
                }

                if (string.IsNullOrEmpty(PFNUMBER))
                {
                    lblMsg.Text = "Upload Failed ! Please check your EXECL";
                    strScript.Append("<script language=JavaScript>");
                    strScript.Append("document.body.onload=function(){alert('" + lblMsg.Text + "')}</script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                    return;
                }
                else
                {
                    PFNUMBERMAIN = PFNUMBERMAIN + "^" + Convert.ToString(row["PFNUMBER"]) + "^";
                }
            }
            try
            {
                SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spExcelImportDetailsEO_Get]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(sqlErrMsgOutput);
                cmd.Parameters.Add(sqlErrCodeOutput);

                cmd.Parameters.AddWithValue("@p_PFNUMBER", PFNUMBERMAIN);
                cmd.Parameters.AddWithValue("@p_TABLENAME", objCommonFunction.ddlSelectedValue(ddlTableName));
                cmd.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dtGrid);
                con.Close();

                if (Convert.ToInt32(sqlErrCodeOutput.Value) >= 0)
                {
                    if (dtGrid.Rows.Count > 0)
                    {
                        gvMain.DataSource = dtGrid;
                        gvMain.DataBind();
                        lblMsg.Text = "Exists PF Number Details Display....";
                        ViewState["DETAILDATA"] = dtGrid;
                        lnkPrint_Excel.Visible = true;
                        lnkPrint_PDF.Visible = true;
                    }
                    else
                    {
                        gvMain.DataSource = null;
                        gvMain.DataBind();
                        ViewState["DETAILDATA"] = null;
                        lblMsg.Text = "PF Number does't Exists in table...!";
                        lnkPrint_Excel.Visible = false;
                        lnkPrint_PDF.Visible = false;
                    }
                }
                else
                {
                    gvMain.DataSource = null;
                    gvMain.DataBind();
                    ViewState["DETAILDATA"] = null;
                    lblMsg.Text = "PF Number does't Exists in table...!";
                    lnkPrint_Excel.Visible = false;
                    lnkPrint_PDF.Visible = false;
                }

            }
            catch (Exception ex)
            {
                //throw ex;
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }

            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + lblMsg.Text + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
        }

        public void funcbindColumnName()
        {
            DataTable dt = new DataTable();
            string p_strTableValue = string.Empty;
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spUploadTableColumn_Get]";
                cmd.Parameters.AddWithValue("@p_TABLENAME", "PF");
                cmd.CommandTimeout = 0;

                sda.Fill(dt);
                funcConvertToExcel(dt);
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

        public void funcConvertToExcel(DataTable dt)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "EXCEL");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("", "");
                Response.AddHeader("content-disposition", "attachment;filename=PFExcelFormat.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        public void funcPrintToExcel()
        {
            try
            {
                String strFileName = objCommonFunction.ddlSelectedText(ddlTableName) + " Details";
                DataTable dtDetails = ((DataTable)ViewState["DETAILDATA"]);
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtDetails, strFileName);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("", "");
                    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + ".xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception e4)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e4);
            }
        }

        public void funcPrintToPDF()
        {
            try
            {
                String strFileName = objCommonFunction.ddlSelectedText(ddlTableName) + " Details";
                DataTable dtDetails = ((DataTable)ViewState["DETAILDATA"]);
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dtDetails;
                GridView1.DataBind();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView1.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                //Document pdfDoc = new Document(PageSize.LARGE_CROWN_OCTAVO, 1f, 1f, 1f, 0f);
                Document pdfDoc = new Document();
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();

                htmlparser.Parse(sr);
                pdfDoc.Close();
                Response.Write(pdfDoc);
                Response.End();
            }
            catch (Exception e5)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e5);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (fileUpload.HasFile)
                {
                    string FileName = Path.GetFileName(fileUpload.PostedFile.FileName);
                    string Extension = Path.GetExtension(fileUpload.PostedFile.FileName);
                    string FolderPath = ConfigurationManager.AppSettings["ExcelFolderPath"];
                    string FilePath = Server.MapPath(FolderPath + FileName);
                    fileUpload.SaveAs(FilePath);

                    funcExcelImport_Get(FilePath, Extension);
                }
            }
            catch (Exception e7)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e7);
            }
        }

        protected void lnkExcel_Click(object sender, EventArgs e)
        {
            funcbindColumnName();
        }

        protected void lnkPrint_Excel_Click(object sender, EventArgs e)
        {
            funcPrintToExcel();
        }

        protected void lnkPrint_PDF_Click(object sender, EventArgs e)
        {
            funcPrintToPDF();
        }
    }
}