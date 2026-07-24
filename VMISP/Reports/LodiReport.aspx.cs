using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Configuration;

namespace VMISP.Reports
{
    public partial class LodiReport : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();

                //objCommonFunction.disableControlsTextBox(txtFromDate);
                //objCommonFunction.disableControlsTextBox(txtToDate);
            }
        }

        private void funcSearch()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dt.Clear();
            try
            {
                string FROMDATE = txtFromDate.Text.Trim();
                string TODATE = txtToDate.Text.Trim();

                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spLodi_Report]";

                cmd.Parameters.AddWithValue("@p_USER", Convert.ToString(ViewState["USERNAME"]));
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(ViewState["USERROLE"]));
                cmd.Parameters.AddWithValue("@p_FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@p_TODATE", TODATE);
                cmd.Parameters.AddWithValue("@p_PFNO", txtPFNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@p_LODINO", txtLodiNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@p_VIGCASENO", txtVigCaseNo.Text.Trim());

                cmd.CommandTimeout = 0;
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    ViewState["DETAILDATA"] = dt;
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    btnExcelDownload.Visible = true;
                    lastUpdated.Visible = true;
                }
                else
                {
                    ViewState["DETAILDATA"] = null;
                    lblMsg.Text = "Record not Found...";
                    gvDetails.DataSource = null;
                    gvDetails.DataBind();
                    btnExcelDownload.Visible = false;
                    lastUpdated.Visible = false;
                }
            }

            catch (Exception es)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
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
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "LODI");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("", "");
                    Response.AddHeader("content-disposition", "attachment;filename=LodiDetails.xlsx");
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


        protected void btnGetDetails_Click(object sender, EventArgs e)
        {
            funcSearch();
        }

        protected void btnExcelDownload_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Convert.ToString(ViewState["DETAILDATA"])))
            {
                DataTable dtDetails = ((DataTable)ViewState["DETAILDATA"]);
                funcConvertToExcel(dtDetails);
            }
        }
    }
}