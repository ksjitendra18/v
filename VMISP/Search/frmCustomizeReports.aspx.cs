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
    public partial class frmCustomizeReports : System.Web.UI.Page
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

        CommonFunction objCommonFunction = new CommonFunction();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();
            }

            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            ddlType.Attributes.Add("onchange", "funchideUnhide_REPORT('" + ddlType.ClientID + "','" + lblValueCaption.ClientID + "')");
            btnGet.Attributes.Add("onclick", "return funcCustomizedReport_Validation('" + ddlType.ClientID + "','" + ddlTableName.ClientID + "')");

            ddlTableName.Focus();
        }

        protected void ddlTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                strTableValue = objCommonFunction.ddlSelectedValue(ddlTableName);
                funcbindColumnName(strTableValue);
                lblColumnHeader.Text = objCommonFunction.ddlSelectedText(ddlTableName) + " Column";
            }
            catch (Exception ed)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ed);
            }
        }

        public void funcbindColumnName(string p_strTableValue)
        {
            try
            {
                DataTable dt = new DataTable();
                string strToolTip = string.Empty;
                #region ** call StoredProcedure to get Column Name of Table  **
                SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                cn.Open();
                SqlCommand cmd = new SqlCommand();
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

                    chkColumnName.Visible = true;
                    chkColumnName.DataSource = dt;
                    chkColumnName.DataTextField = "COLUMNNAME";
                    chkColumnName.DataValueField = "COLUMNVALUE";
                    chkColumnName.DataBind();
                }

                else
                {
                    chkColumnName.ClearSelection();
                    chkColumnName.SelectedIndex = -1;
                    chkColumnName.Visible = false;
                }
                #endregion

            }
            catch (Exception e)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(e);
            }
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                funcShow();
                funcHideUnhide();
            }
            catch (Exception eg)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(eg);
            }
        }

        public void funcShow()
        {
            string strSelectColumn = string.Empty;
            string strType = string.Empty;
            string strColumnName_WHERE = string.Empty;
            string strCondition_WHERE = string.Empty;
            string strConditionValue_WHERE = string.Empty;
            string strFromDate_WHERE = string.Empty;
            string strToDate_WHERE = string.Empty;

            DataTable dtDetails = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlDataAdapter sda = new SqlDataAdapter();

            for (int i = 0; i < chkColumnName.Items.Count; i++)
            {
                if (chkColumnName.Items[i].Selected)
                {

                    strChkValue = strChkValue + chkColumnName.Items[i].Text + ",";
                }
            }

            strTABLENAME = objCommonFunction.ddlSelectedValue(ddlTableName);
            strSelectColumn = objCommonFunction.removeStringLastComma(strChkValue);
            strColumnName_WHERE = txtColumnName_WHERE.Text.Trim();
            strCondition_WHERE = objCommonFunction.ddlSelectedValue(ddlCondition_WHERE);
            strConditionValue_WHERE = txtConditionValue_WHERE.Text.Trim();
            strType = objCommonFunction.ddlSelectedValue(ddlType);
            strFromDate_WHERE = txtFromDate.Text.Trim();
            strToDate_WHERE = txtToDate.Text.Trim();

            try
            {
                #region ** call StoredProcedure to View the Data of Complaint  **
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spCustomize_Report]";

                cmd.Parameters.AddWithValue("@p_TABLENAME", strTABLENAME);
                cmd.Parameters.AddWithValue("@p_COLUMNNAME", strSelectColumn);
                cmd.Parameters.AddWithValue("@p_TYPE", strType);
                cmd.Parameters.AddWithValue("@p_COLUMNNAME_WHERE", strColumnName_WHERE);
                cmd.Parameters.AddWithValue("@p_CONDITION_WHERE", strCondition_WHERE);
                cmd.Parameters.AddWithValue("@p_CODITIONVALUE_WHERE", strConditionValue_WHERE);
                cmd.Parameters.AddWithValue("@p_FROMDATE_WHERE", strFromDate_WHERE);
                cmd.Parameters.AddWithValue("@p_TODATE_WHERE", strToDate_WHERE);

                cmd.CommandTimeout = 0;
                sda.SelectCommand = cmd;
                sda.Fill(dtDetails);

                if (dtDetails.Rows.Count > 0)
                {
                    btnExcel.Visible = true;
                    btnPDF.Visible = true;
                    pnlNoRecords.Visible = false;
                    pnlGridDetails.Visible = true;
                    ViewState["PRINT"] = dtDetails;
                    gvMain.DataSource = dtDetails;
                    gvMain.DataBind();
                }
                else
                {
                    btnExcel.Visible = false;
                    btnPDF.Visible = false;
                    ViewState["PRINT"] = null;
                    pnlGridDetails.Visible = false;
                    pnlNoRecords.Visible = true;
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

        public void funcConvertToExcel()
        {
            try
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
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        public void funcConvertToPDF()
        {
            try
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
            catch (Exception ep)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ep);
            }
        }

        public void funcHideUnhide()
        {
            if (hidColumnDataType.Value.ToUpper() == "DATE")
            {
                lblValueCaption.Text = "Date :";
                tdDate.Style.Add("display", "block");
                tdText.Style.Add("display", "none");
            }
            else
            {
                lblValueCaption.Text = "Value :";
                tdDate.Style.Add("display", "none");
                tdText.Style.Add("display", "block");
            }

        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            try
            {
                funcConvertToPDF();
            }
            catch (Exception ex)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
            }
        }

        protected void btnExel_Click(object sender, EventArgs e)
        {
            try
            {
                funcConvertToExcel();
            }
            catch (Exception eb)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(eb);
            }
        }


    }
}