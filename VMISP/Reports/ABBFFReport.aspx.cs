using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using VMISP.Models;
using System.Reflection;

namespace VMISP.Reports
{
    public partial class ABBFFReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            lblMsg.Text = "";
            btnExcelDownload.Visible = false;
        }

        protected void btnGetDetails_Click(object sender, EventArgs e)
        {
            using (vigcontext context = new vigcontext())
            {
                var data = context.ABBFFs.Where(x => x.STATUS_CODE != "15").ToList();

                if (data != null && data.Count > 0)
                {                    
                    ViewState["DETAILDATA"] = JsonConvert.SerializeObject(data);
                    gvDetails.DataSource = data;
                    gvDetails.DataBind();
                    btnExcelDownload.Visible = true;
                }
                else
                {
                    lblMsg.Text = "No Details found for the selected criteria";
                    lblMsg.ForeColor = System.Drawing.Color.White;
                    lblMsg.Visible = true;
                    gvDetails.DataSource = null;
                    gvDetails.DataBind();
                }
            };
        }

        protected void btnExcelDownload_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Convert.ToString(ViewState["DETAILDATA"])))
            {
                DataTable dtDetails = new DataTable();
                List<ABBFF> list = JsonConvert.DeserializeObject<List<ABBFF>>(ViewState["DETAILDATA"].ToString());
                foreach (var prop in typeof(ABBFF).GetProperties())
                {
                    Type dataType = prop.PropertyType;
                    if (dataType.Name.Contains("Nullable"))
                    {
                        dataType = Nullable.GetUnderlyingType(prop.PropertyType);
                    }
                    dtDetails.Columns.Add(prop.Name, dataType);
                }

                foreach (var item in list)
                {
                    DataRow row = dtDetails.NewRow();
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                    dtDetails.Rows.Add(row);
                }

                funcConvertToExcel(dtDetails);
            }
        }

        public void funcConvertToExcel(DataTable dt)
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "ABBFF");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("", "");
                    Response.AddHeader("content-disposition", "attachment;filename=ABBFFDetails.xlsx");
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
    }
}