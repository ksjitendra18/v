using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Configuration;

namespace VMISP.Search
{
    public partial class RetirementCases : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void funcGetDetails()
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
                cmd.CommandText = "[dbo].[spRetirementCases_Details]";
                cmd.Parameters.AddWithValue("@p_FROMDATE", txtFromDate.Text.Trim());
                cmd.Parameters.AddWithValue("@p_TODATE", txtToDate.Text.Trim());

                cmd.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    ViewState["DETAILS"] = dt;
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    btnExcelDownload.Visible = true;
                }
                else
                {
                    ViewState["DETAILS"] = null;
                    gvDetails.DataSource = null;
                    gvDetails.DataBind();
                    lblMsg.Text = "Record not found.";
                    btnExcelDownload.Visible = false;
                }
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

        protected void btnGetDetails_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFromDate.Text))
            {
                lblMsg.Text = "Please select From Date.";
                txtFromDate.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtToDate.Text))
            {
                lblMsg.Text = "Please select From Date.";
                txtFromDate.Focus();
                return;
            }

            funcGetDetails();
        }

        public void funcConvertToExcel(DataTable dt)
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "RetirementCases");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("", "");
                    Response.AddHeader("content-disposition", "attachment;filename=RetirementCases.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception eExcel)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(eExcel);
            }
        }

        protected void btnExcelDownload_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Convert.ToString(ViewState["DETAILS"])))
            {
                DataTable dtDetails = ((DataTable)ViewState["DETAILS"]);
                funcConvertToExcel(dtDetails);
            }
        }
    }
}