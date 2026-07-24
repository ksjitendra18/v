using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VMISP.Search
{
    public partial class frmTableWiseSearch : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMsg.Text = string.Empty;
            }

            ddlColumnName.Attributes.Add("onchange", "funchideUnhide('" + ddlColumnName.ClientID + "')");
        }

        public void funcShow()
        {
            DataSet ds = new DataSet();
            DataTable dtGrid = new DataTable();
            DataTable dtDetails = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlDataAdapter sda = new SqlDataAdapter();

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spTableWiseSearch_View]";

                cmd.Parameters.AddWithValue("@p_TABLENAME", objCommonFunction.ddlSelectedValue(ddlTableName));
                cmd.Parameters.AddWithValue("@p_COLUMNNAME", objCommonFunction.ddlSelectedText(ddlColumnName));
                cmd.Parameters.AddWithValue("@p_ENTERVALUE", txtEnterValue.Text.Trim());
                cmd.Parameters.AddWithValue("@p_VIEW", objCommonFunction.chkSelected(chkShowAllData));

                cmd.CommandTimeout = 0;
                sda.SelectCommand = cmd;
                sda.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    dtGrid = ds.Tables[0];
                    dtDetails = ds.Tables[1];

                    gvMain.DataSource = dtGrid;
                    gvMain.DataBind();
                    btnExcel.Visible = true;
                    btnPDF.Visible = true;

                    ViewState["DETAILDATA"] = dtGrid;
                    ViewState["PRINT"] = dtDetails;
                }
                else
                {
                    gvMain.DataSource = null;
                    gvMain.DataBind();
                    btnExcel.Visible = false;
                    btnPDF.Visible = false;

                    ViewState["DETAILDATA"] = null;
                    ViewState["PRINT"] = null;
                }
            }

            catch (Exception es)
            {
                es.ToString();
            }
            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public void funcHideUnhide()
        {
            if (hidColumnDataType.Value.ToUpper() == "DATETIME")
            {
                divDate.Style.Add("display", "block");
                divText.Style.Add("display", "none");
            }
            else
            {
                divDate.Style.Add("display", "none");
                divText.Style.Add("display", "block");
            }

        }

        public void funcConvertToExcel()
        {
            String strFileName = objCommonFunction.ddlSelectedText(ddlTableName) + " Details";

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
            String strFileName = objCommonFunction.ddlSelectedText(ddlTableName) + " Details";

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
            Document pdfDoc = new Document();
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }

        public void funcbindColumnName(string p_strTableValue)
        {
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = cn;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spTableColumn_Get]";

                cmd.Parameters.AddWithValue("@p_TABLENAME", p_strTableValue);

                cmd.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ddlColumnName.DataSource = dt;
                    ddlColumnName.DataTextField = "COLUMNNAME";
                    ddlColumnName.DataValueField = "COLUMNVALUE";
                    ddlColumnName.DataBind();
                }

                else
                {
                    ddlColumnName.SelectedIndex = 0;
                }
            }
            catch (Exception e)
            {
                lblMsg.Text = e.ToString();
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e);
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
                cn.Close();
            }
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            funcShow();
            funcHideUnhide();
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            funcConvertToPDF();
        }

        protected void btnExel_Click(object sender, EventArgs e)
        {
            funcConvertToExcel();

            DataTable dtDetails = ((DataTable)ViewState["DETAILDATA"]);
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = dtDetails;
            GridView1.DataBind();
        }

        protected void ddlTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            funcbindColumnName(objCommonFunction.ddlSelectedValue(ddlTableName));

            gvMain.DataSource = null;
            gvMain.DataBind();
        }

        protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover",
                "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#20B2AA';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            }
        }

    }
}