using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using ClosedXML.Excel;

namespace VMISP.Search
{
    public partial class frmEOSearch : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMsg.Text = string.Empty;
            }
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            funcShow();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            funcConvertToExcel();
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            funcConvertToPDF();
        }

        private void funcShow()
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
                cmdView.CommandText = "[dbo].[spEOSearch_View]";

                cmdView.Parameters.AddWithValue("@p_TABLENAME", objCommonFunction.ddlSelectedValue(ddlTableName));
                cmdView.Parameters.AddWithValue("@p_UNIQUEID", txtRNO.Text.Trim());
                cmdView.Parameters.AddWithValue("@p_PFNUMBER", txtEOPFNumber.Text.Trim());

                cmdView.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    gvEODetails.DataSource = dt;
                    gvEODetails.DataBind();

                    ViewState["PRINT"] = dt;
                    btnExcel.Visible = true;
                    btnPDF.Visible = true;
                }
                else
                {
                    gvEODetails.DataSource = null;
                    gvEODetails.DataBind();
                    lblMsg.Text = "No Erring Officer Found";
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

        public void funcConvertToExcel()
        {
            String strFileName = objCommonFunction.ddlSelectedText(ddlTableName) + " EO Details";

            DataTable dtDetails = ((DataTable)ViewState["PRINT"]);
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

        public void funcConvertToPDF()
        {
            String strFileName = objCommonFunction.ddlSelectedText(ddlTableName) + " EO Details";

            DataTable dtDetails = ((DataTable)ViewState["PRINT"]);
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

        public void funcClear()
        {
            txtEOPFNumber.Text = "";
            txtRNO.Text = "";
        }

    }
}