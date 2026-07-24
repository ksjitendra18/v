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

namespace VMISP.Search
{
    public partial class frmFieldWiseSearch : System.Web.UI.Page
    {
        #region ** declare Variable **
        string strMsg = string.Empty;
        string strErrMsg = string.Empty;
        string strTABLEVALUE = string.Empty;
        string strPFNO = string.Empty;
        string strCASENO = string.Empty;
        string strNAME = string.Empty;
        string strSTATUS = string.Empty;
        CommonFunction objCommonFunction = new CommonFunction();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();


            }
            #region **  JS function **
            ddlTableName.Attributes.Add("onchange", "funchideUnhideControls('" + ddlTableName.ClientID + "')");
            #endregion

            ddlTableName.Focus();
            lblMsg.Text = string.Empty;
        }

        public void funcShow()
        {
            DataTable dtGrid = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlDataAdapter sda = new SqlDataAdapter();

            strTABLEVALUE = objCommonFunction.ddlSelectedValue(ddlTableName);
            strPFNO = txtPFNo.Text;
            strCASENO = txtCaseNo.Text;
            strNAME = txtName.Text;
            strSTATUS = txtStatus.Text;

            try
            {

                #region ** call StoredProcedure to View the Data of Complaint  **
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spFieldWiseSearch_View]";

                cmd.Parameters.AddWithValue("@p_TABLENAME", strTABLEVALUE);
                cmd.Parameters.AddWithValue("@p_PFNO", strPFNO);
                cmd.Parameters.AddWithValue("@p_CASENO", strCASENO);
                cmd.Parameters.AddWithValue("@p_NAME", strNAME);
                cmd.Parameters.AddWithValue("@p_STATUS", strSTATUS);

                cmd.CommandTimeout = 0;
                sda.SelectCommand = cmd;
                sda.Fill(dtGrid);

                if (dtGrid.Rows.Count > 0)
                {
                    gvMain.DataSource = dtGrid;
                    gvMain.DataBind();

                    ViewState["DETAILDATA"] = dtGrid;

                    btnExcel.Visible = true;
                    btnPDF.Visible = true;
                }
                else
                {
                    gvMain.DataSource = null;
                    gvMain.DataBind();

                    btnExcel.Visible = false;
                    btnPDF.Visible = false;
                }
                #endregion
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

        public void funcBindGridDetails()
        {
            DataTable dtDetails = ((DataTable)ViewState["DETAILDATA"]);
            gvMain.DataSource = dtDetails;
            gvMain.DataBind();
        }

        public void funcConvertToExcel()
        {
            String strFileName = objCommonFunction.ddlSelectedText(ddlTableName) + " Details";
            
            DataTable dtDetails = ((DataTable)ViewState["DETAILDATA"]);
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = dtDetails;
            GridView1.DataBind();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             "attachment;filename="+strFileName+" .xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridView1.Rows[i].Attributes.Add("class", "textmode");  //Apply text style to each Row
            }
            GridView1.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        public void funcConvertToPDF()
        {
            String strFileName = objCommonFunction.ddlSelectedText(ddlTableName) + " Details";

            DataTable dtDetails = ((DataTable)ViewState["DETAILDATA"]);
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = dtDetails;
            GridView1.DataBind();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename="+strFileName+".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            funcShow();
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
            dtSorting.DefaultView.Sort = e.SortExpression;
            gvMain.DataSource = dtSorting;
            gvMain.DataBind();
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            funcConvertToPDF();
        }

        protected void btnExel_Click(object sender, EventArgs e)
        {
            funcConvertToExcel(); DataTable dtDetails = ((DataTable)ViewState["DETAILDATA"]);

            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = dtDetails;
            GridView1.DataBind();
        }
    }
}