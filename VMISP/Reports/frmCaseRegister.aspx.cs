using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using ClosedXML.Excel;
using System.Configuration;
using System.Data.OleDb;
using System.Text;
using iTextSharp.text.pdf;

namespace VMISP.Reports
{
    public partial class frmCaseRegister : System.Web.UI.Page
    {
        string strErrMsg = string.Empty;
        int intErrCode = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            txtFromDate.Focus();

            #region ** JS Function  **
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            #endregion
        }

        private void funcCase_Get(string strView, string strNO)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string strFROMDATE = txtFromDate.Text.Trim();
            string strTODATE = txtToDate.Text.Trim();

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spCaseRegister_View]";

                SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(sqlErrMsgOutput);
                cmd.Parameters.Add(sqlErrCodeOutput);

                cmd.Parameters.AddWithValue("@p_FROMDATE", strFROMDATE);
                cmd.Parameters.AddWithValue("@p_TODATE", strTODATE);
                cmd.Parameters.AddWithValue("@p_VIEW", strView);
                cmd.Parameters.AddWithValue("@p_NO", strNO);
                cmd.CommandTimeout = 0;
                sda.Fill(dt);

                strErrMsg = sqlErrMsgOutput.Value.ToString();
                intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                if (intErrCode >= 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (strView.ToUpper() == "COMPLAINT")
                        {
                            gvComplaint.DataSource = dt;
                            gvComplaint.DataBind();
                            ViewState["COMPLAINT_DETAILDATA"] = dt;
                            funcEnableDisable(strView);
                        }

                        else if (strView.ToUpper() == "IAC")
                        {
                            gvIAC.DataSource = dt;
                            gvIAC.DataBind();
                            ViewState["IAC_DETAILDATA"] = dt;
                            funcEnableDisable(strView);
                        }

                        else if (strView.ToUpper() == "VIGILANCE")
                        {
                            gvVigilance.DataSource = dt;
                            gvVigilance.DataBind();
                            ViewState["VIGILANCE_DETAILDATA"] = dt;
                            funcEnableDisable(strView);
                        }
                    }
                    else
                    {
                        lblMsg.Text = strView +  " " + " Data not found between " + " " +  strFROMDATE + " and " + strTODATE;
                    }
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

        private void funcClear()
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            btnComplaint.Visible = true;
            lblMsg.Text = "";
            ceFromDate.Enabled = true;
            ceToDate.Enabled = true;
        }

        private void funcEnableDisable(string strTYPE)
        {
            ceFromDate.Enabled = false;
            ceToDate.Enabled = false;

            if (strTYPE.ToUpper() == "COMPLAINT")
            {
                btnVigilance.Visible = false;
                btnIAC.Visible = true;
                tabMain.ActiveTabIndex = 0;
                btnComplaint.ForeColor = System.Drawing.Color.Yellow;
                lnkDOWNLOAD_COMPLAINT.Visible = true;
                lnkDOWNLOAD_COMPLAINT.ForeColor = System.Drawing.Color.OrangeRed;
            }

            else if (strTYPE.ToUpper() == "IAC")
            {
                btnVigilance.Visible = true;
                tabMain.ActiveTabIndex = 1;
                btnIAC.ForeColor = System.Drawing.Color.Yellow;
                lnkDOWNLOAD_IAC.Visible = true;
                lnkDOWNLOAD_IAC.ForeColor = System.Drawing.Color.OrangeRed;
            }

            else if (strTYPE.ToUpper() == "VIGILANCE")
            {
                tabMain.ActiveTabIndex = 2;
                btnVigilance.ForeColor = System.Drawing.Color.Yellow;
                lnkDOWNLOAD_VIGILANCE.Visible = true;
                lnkDOWNLOAD_VIGILANCE.ForeColor = System.Drawing.Color.OrangeRed;
            }
        }

        public string funcStringNo(string strTYPE)
        {
            string strNO = "";
            string strNOMAIN = "";
            DataTable dtData = new DataTable();

            if (strTYPE.ToUpper() == "IAC")
            {
                dtData = (DataTable)ViewState["COMPLAINT_DETAILDATA"];
            }
            else if (strTYPE.ToUpper() == "VIGILANCE")
            {
                dtData = (DataTable)ViewState["IAC_DETAILDATA"];
            }

            if (dtData.Rows.Count > 0)
            {
                foreach (DataRow row in dtData.Rows)
                {
                    try
                    {
                        strNO = row["CASENO"].ToString();
                    }
                    catch (Exception exIAC)
                    {
                        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(exIAC);
                    }

                    if (strNOMAIN != "")
                    {
                        strNOMAIN = strNOMAIN + '~'.ToString();
                    }

                    if (strNO != "")
                    {
                        strNOMAIN = strNOMAIN + "^" + strNO + "^";
                    }
                }
            }

            return strNOMAIN;
        }

        public void funcPrintToExcel(string strTYPE)
        {
            String strFileName = strTYPE + " Details";

            DataTable dtDetails = new DataTable();
            if (strTYPE.ToUpper() == "COMPLAINT")
            {
                dtDetails = ((DataTable)ViewState["COMPLAINT_DETAILDATA"]);
            }
            else if (strTYPE.ToUpper() == "IAC")
            {
                dtDetails = ((DataTable)ViewState["IAC_DETAILDATA"]);
            }
            else if (strTYPE.ToUpper() == "VIGILANCE")
            {
                dtDetails = ((DataTable)ViewState["VIGILANCE_DETAILDATA"]);
            }

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

        protected void btnComplaintSubmit_Click(object sender, EventArgs e)
        {
            funcCase_Get("COMPLAINT", null);
        }

        protected void btnIACSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string strCOMPLAINTIACNO = funcStringNo("IAC");
                funcCase_Get("IAC", strCOMPLAINTIACNO);
            }
            catch (Exception exIACSUBMIT)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(exIACSUBMIT);
            }
        }

        protected void btnVigilanceSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string strIACVIGILANCENO = funcStringNo("VIGILANCE");
                funcCase_Get("VIGILANCE", strIACVIGILANCENO);
            }
            catch (Exception exVIGSUBMIT)
            {
                VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(exVIGSUBMIT);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            funcClear();
        }

        protected void gvComplaint_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover",
                "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#20B2AA';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            }
        }

        protected void gvComplaint_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvComplaint.PageIndex = e.NewPageIndex;

            DataTable dtPaging = ((DataTable)ViewState["COMPLAINT_DETAILDATA"]);
            gvComplaint.DataSource = dtPaging;
            gvComplaint.DataBind();
        }

        protected void gvComplaint_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtSorting = ((DataTable)ViewState["COMPLAINT_DETAILDATA"]);
            gvComplaint.DataSource = dtSorting;
            gvComplaint.DataBind();

            if (dtSorting != null)
            {
                DataView dataView = new DataView(dtSorting);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                gvComplaint.DataSource = dataView;
                gvComplaint.DataBind();
            }
        }

        protected void gvIAC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIAC.PageIndex = e.NewPageIndex;

            DataTable dtPaging = ((DataTable)ViewState["IAC_DETAILDATA"]);
            gvIAC.DataSource = dtPaging;
            gvIAC.DataBind();
        }

        protected void gvIAC_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtSorting = ((DataTable)ViewState["IAC_DETAILDATA"]);
            gvIAC.DataSource = dtSorting;
            gvIAC.DataBind();

            if (dtSorting != null)
            {
                DataView dataView = new DataView(dtSorting);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                gvIAC.DataSource = dataView;
                gvIAC.DataBind();
            }
        }

        protected void gvVigilance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVigilance.PageIndex = e.NewPageIndex;

            DataTable dtPaging = ((DataTable)ViewState["VIGILANCE_DETAILDATA"]);
            gvVigilance.DataSource = dtPaging;
            gvVigilance.DataBind();
        }

        protected void gvVigilance_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtSorting = ((DataTable)ViewState["VIGILANCE_DETAILDATA"]);
            gvVigilance.DataSource = dtSorting;
            gvVigilance.DataBind();

            if (dtSorting != null)
            {
                DataView dataView = new DataView(dtSorting);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                gvVigilance.DataSource = dataView;
                gvVigilance.DataBind();
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

        protected void lnkDOWNLOAD_COMPLAINT_Click(object sender, EventArgs e)
        {
            funcPrintToExcel("COMPLAINT");
        }

        protected void lnkDOWNLOAD_IAC_Click(object sender, EventArgs e)
        {
            funcPrintToExcel("IAC");
        }

        protected void lnkDOWNLOAD_VIGILANCE_Click(object sender, EventArgs e)
        {
            funcPrintToExcel("VIGILANCE");
        }
    }
}
