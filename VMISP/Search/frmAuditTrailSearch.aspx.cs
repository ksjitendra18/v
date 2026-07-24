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
    public partial class frmAuditTrailSearch : System.Web.UI.Page
    {
        #region ** declare Variable **
        string strMsg = string.Empty;
        string strSearchNo = string.Empty;
        string strErrMsg = string.Empty;
        string strUser = string.Empty;
        string strUserRole = string.Empty;
        string strColumnValue = string.Empty;
        string strTableValue = string.Empty;
        string strChkValue = string.Empty;

        string strTABLENAME = string.Empty;
        string strCOLUMNNAME = string.Empty;
        string strENTERVALUE = string.Empty;
        DateTime? dtENTERDATE = null;
        CommonFunction objCommonFunction = new CommonFunction();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();
            }

            ddlTableName.Focus();
            lblMsg.Text = string.Empty;

            #region ** readOnly Controls **
            txtEnterDate.Attributes.Add("readonly", "readonly");
            ddlColumnName.Attributes.Add("onchange", "funchideUnhide('" + ddlColumnName.ClientID + "','" + lblValueCaption.ClientID + "')");
            #endregion
        }

        public void funcShow()
        {
            DataSet ds = new DataSet();
            DataTable dtGrid = new DataTable();
            DataTable dtDetails = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlDataAdapter sda = new SqlDataAdapter();

            string strENTERDATE = txtEnterDate.Text.Trim();
            if (!string.IsNullOrEmpty(strENTERDATE))
            {
                DateTime date;
                if (DateTime.TryParse(strENTERDATE, out date))
                    dtENTERDATE = date;
            }

            strTABLENAME = objCommonFunction.ddlSelectedValue(ddlTableName);
            strCOLUMNNAME = objCommonFunction.ddlSelectedText(ddlColumnName);
            strENTERVALUE = txtEnterValue.Text.Trim();
            strChkValue = objCommonFunction.chkSelected(chkShowAllData);

            if (strENTERVALUE == "")
            {
                strENTERVALUE = dtENTERDATE.ToString();
            }

            try
            {
                #region ** call StoredProcedure to View the Data of Complaint  **
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spHistoryTableWiseSearch_View]";

                cmd.Parameters.AddWithValue("@p_TABLENAME", strTABLENAME);
                cmd.Parameters.AddWithValue("@p_COLUMNNAME", strCOLUMNNAME);
                cmd.Parameters.AddWithValue("@p_ENTERVALUE", strENTERVALUE);
                cmd.Parameters.AddWithValue("@p_VIEW", strChkValue);

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
                    btnPDF.Visible = false;

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
                #endregion
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

        public void funcHideUnhide()
        {
            if (hidColumnDataType.Value.ToUpper() == "DATETIME")
            {
                lblValueCaption.Text = "Enter Date :";
                tdDate.Style.Add("display", "block");
                tdText.Style.Add("display", "none");
            }
            else
            {
                lblValueCaption.Text = "Enter Value :";
                tdDate.Style.Add("display", "none");
                tdText.Style.Add("display", "block");
            }
        }

        public void funcConvertToExcel()
        {
            String strFileName = objCommonFunction.ddlSelectedText(ddlTableName) + " Audit Trail Details";

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
            String strFileName = objCommonFunction.ddlSelectedText(ddlTableName) + " Audit Trail Details";

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
            try
            {
                DataTable dt = new DataTable();

                #region ** call StoredProcedure to get Column Name of Table  **
                SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spHistoryTableColumn_Get]";

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
                #endregion

            }
            catch (Exception e)
            {
                lblMsg.Text = e.ToString();
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

        protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMain.PageIndex = e.NewPageIndex;

            DataTable dtPaging = ((DataTable)ViewState["DETAILDATA"]);
            gvMain.DataSource = dtPaging;
            gvMain.DataBind();
        }

        protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtSorting = ((DataTable)ViewState["DETAILDATA"]);
            gvMain.DataSource = dtSorting;
            gvMain.DataBind();

            if (dtSorting != null)
            {
                DataView dataView = new DataView(dtSorting);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                gvMain.DataSource = dataView;
                gvMain.DataBind();
            }
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }

        private string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";
                    break;

                case "DESC":
                    GridViewSortDirection = "ASC";
                    break;
            }

            return GridViewSortDirection;
        }

        protected void ddlTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
            funcbindColumnName(strTableValue);

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