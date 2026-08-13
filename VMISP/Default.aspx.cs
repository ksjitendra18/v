using ClosedXML.Excel;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using VMISP.Code;

namespace VMISP
{
    public partial class Default : System.Web.UI.Page
    {
        CommonFunction objCommonFunction = new CommonFunction();
        Logger logger = LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Info("Page_Load :Default ");

            if (!IsPostBack)
            {
                if (objCommonFunction.funcCheckUserRights("DASHBOARD") == false)
                {
                    Response.Redirect("~/Logout.aspx");
                }
                logger.Info("line 23");
                funcbindDropdown();

                string userPF = Session["userid"].ToString();

                List<CheckerModulePending> pendingRows = new List<CheckerModulePending>();

                int pendingComplaintCount = GetPendingComplaintCount(userPF);
                if (pendingComplaintCount > 0)
                {
                    pendingRows.Add(new CheckerModulePending
                    {
                        ModuleName = "Complaint",
                        Count = pendingComplaintCount,
                        InboxUrl = "~/ComplaintApproval.aspx"
                    });
                }

                pendingRows.AddRange(GetOtherPendingCheckerCounts(userPF));

                if (pendingRows.Count > 0)
                {
                    StringBuilder rowsHtml = new StringBuilder();
                    foreach (CheckerModulePending m in pendingRows)
                    {
                        rowsHtml.Append("<tr>");
                        rowsHtml.Append("<td>" + HttpUtility.HtmlEncode(m.ModuleName) + "</td>");
                        rowsHtml.Append("<td class=\"text-center\"><span class=\"label label-danger\">" + m.Count + "</span></td>");
                        rowsHtml.Append("<td><a href=\"" + ResolveUrl(m.InboxUrl) + "\" class=\"btn btn-primary btn-sm\">Review Now</a></td>");
                        rowsHtml.Append("</tr>");
                    }
                    phPendingApprovals.Controls.Add(new Literal { Text = rowsHtml.ToString() });

                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "PendingPopup",
                        "window.onload = function () { $('#pendingApprovalsModal').modal('show'); };",
                        true);
                }
                logger.Info("line 24");
            }
        }

        private sealed class CheckerModulePending
        {
            public string ModuleCode;
            public string ModuleName;
            public int Count;
            public string InboxUrl;
        }

        private List<CheckerModulePending> GetOtherPendingCheckerCounts(string userPf)
        {
            // Modules on the central CASE_APPROVAL / WORKFLOW_MODULE workflow (see
            // Docs/VMIS_IAC_MakerChecker_Implementation.md). Complaint is not here - it still
            // uses its own inline-columns mechanism, handled by GetPendingComplaintCount above.
            // WORKFLOW_MODULE.ViewPage is the checker's record-detail page, not the inbox list,
            // so the inbox page for each module is mapped here; add a line as each module is rolled out.
            Dictionary<string, string> checkerInboxPages = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "IAC", "~/Mis/frmIACChecker.aspx" },
                { "VIGILANCE", "~/Mis/frmVigilanceChecker.aspx" },
                { "MISC", "~/Mis/frmMiscChecker.aspx" }
            };

            List<CheckerModulePending> list = new List<CheckerModulePending>();

            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                // Scoped by fnCheckerScope so the badges show only modules this checker is
                // actually granted -- a Vigilance-only checker gets no Complaint or MISC count.
                string query = @"
            SELECT WM.ModuleCode, WM.ModuleName, COUNT(*) AS PendingCount
            FROM CASE_APPROVAL CA
            INNER JOIN WORKFLOW_MODULE WM
                ON WM.ModuleCode = CA.ModuleCode
                AND WM.IsActive = 1
            INNER JOIN dbo.fnCheckerScope(@UserPF) S
                ON S.ModuleCode = CA.ModuleCode
                AND S.ZoneSolID = CA.ZoneSolID
            WHERE CA.ApprovalStatus = 'P'
            GROUP BY WM.ModuleCode, WM.ModuleName";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserPF", userPf);

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string moduleCode = Convert.ToString(dr["ModuleCode"]);
                        string inboxUrl;

                        if (string.IsNullOrEmpty(moduleCode) || !checkerInboxPages.TryGetValue(moduleCode, out inboxUrl))
                        {
                            continue; // inbox page not registered yet for this module
                        }

                        list.Add(new CheckerModulePending
                        {
                            ModuleCode = moduleCode,
                            ModuleName = Convert.ToString(dr["ModuleName"]),
                            Count = Convert.ToInt32(dr["PendingCount"]),
                            InboxUrl = inboxUrl
                        });
                    }
                }
            }

            return list;
        }

        private int GetPendingComplaintCount(string userPf)
        {
            int count = 0;

            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString))
            {
                // Complaint is still on its own inline-columns mechanism, but the checker's
                // scope is resolved the same way as every other module.
                string query = @"
            SELECT COUNT(*)
            FROM COMPLAINT C
            INNER JOIN dbo.fnCheckerScope(@UserPF) S
                ON S.ModuleCode = 'COMPLAINT'
               AND S.ZoneSolID  = C.NEWZONE
            WHERE C.APPROVALSTATUS = 'P'";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserPF", userPf);

                con.Open();

                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return count;
        }

        public void funcbindDropdown()
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashbaord_Ddl]";
                cmd.CommandTimeout = 0;
                sda.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    objCommonFunction.bindDropdownList(ddlDealingCMIAC, ds.Tables[0]);
                    objCommonFunction.bindDropdownList(ddlDealingCMVigilance, ds.Tables[0]);
                    objCommonFunction.bindDropdownList(ddlDealingCMNPA, ds.Tables[0]);
                    objCommonFunction.bindDropdownList(ddlDealingCMComplaint, ds.Tables[1]);
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

        private void funcHideUnhide(string VIEW)
        {
            if (VIEW.Equals("OUTSTANDING"))
            {
                pnlOutstanding.Visible = true;
                pnlComplaintOutstanding.Visible = false;
                pnlIACOutstanding.Visible = false;
                pnlVigilanceOutstanding.Visible = false;
                pnlNPAOutstanding.Visible = false;
            }

            else if (VIEW.Equals("COMPLAINT_OUTSTANDING"))
            {
                pnlOutstanding.Visible = false;
                pnlComplaintOutstanding.Visible = true;
                pnlIACOutstanding.Visible = false;
                pnlVigilanceOutstanding.Visible = false;
                pnlNPAOutstanding.Visible = false;
            }

            else if (VIEW.Equals("IAC_OUTSTANDING"))
            {
                pnlOutstanding.Visible = false;
                pnlComplaintOutstanding.Visible = false;
                pnlIACOutstanding.Visible = true;
                pnlVigilanceOutstanding.Visible = false;
                pnlNPAOutstanding.Visible = false;
            }

            else if (VIEW.Equals("VIGILANCE_OUTSTANDING"))
            {
                pnlOutstanding.Visible = false;
                pnlComplaintOutstanding.Visible = false;
                pnlIACOutstanding.Visible = false;
                pnlVigilanceOutstanding.Visible = true;
                pnlNPAOutstanding.Visible = false;
            }

            else if (VIEW.Equals("NPA_OUTSTANDING"))
            {
                pnlOutstanding.Visible = false;
                pnlComplaintOutstanding.Visible = false;
                pnlIACOutstanding.Visible = false;
                pnlVigilanceOutstanding.Visible = false;
                pnlNPAOutstanding.Visible = true;
            }
            else if (VIEW.Equals("ABBFF"))
            {
                pnlOutstanding.Visible = false;
                pnlComplaintOutstanding.Visible = false;
                pnlIACOutstanding.Visible = false;
                pnlVigilanceOutstanding.Visible = false;
                pnlNPAOutstanding.Visible = false;

            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //UpdatePanel panel = (UpdatePanel)Master.FindControl("upMaster"); ;
            //panel.UpdateMode = UpdatePanelUpdateMode.Conditional;
            //panel.ChildrenAsTriggers = true;

            ScriptManager sm = (ScriptManager)Master.FindControl("ScriptManager1");
            sm.EnablePartialRendering = false;
        }

        #region **  Call Outstanding Store Proc    **
        public List<CompalintsChartData> getOutstandingComplaintsWiseData(string TYPE)
        {
            List<CompalintsChartData> p = new List<CompalintsChartData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboard_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    CompalintsChartData cpData = new CompalintsChartData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    p.Add(cpData);
                }
            }

            return p;
        }

        public List<IACChartData> getOutstandingIACWiseData(string TYPE)
        {
            List<IACChartData> p = new List<IACChartData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboard_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    IACChartData cpData = new IACChartData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    //cpData.Code = Convert.ToString(dr["CODE"]);
                    p.Add(cpData);
                }
            }

            return p;
        }

        public List<VigilanceChartData> getOutstandingVigilanceWiseData(string TYPE)
        {
            List<VigilanceChartData> p = new List<VigilanceChartData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboard_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    VigilanceChartData cpData = new VigilanceChartData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    //cpData.Code = Convert.ToString(dr["CODE"]);
                    p.Add(cpData);
                }
            }

            return p;
        }

        public List<IACVigilancePieData> getOutstandingIACViewWiseData(string TYPE)
        {
            List<IACVigilancePieData> p = new List<IACVigilancePieData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardVigNonVig_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_FROMDATE", txtFromDate.Text.Trim());
            cmd.Parameters.AddWithValue("@p_TODATE", txtToDate.Text.Trim());
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    IACVigilancePieData cpData = new IACVigilancePieData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    cpData.Percentage = Convert.ToString(dr["PERCENTAGE"]);
                    p.Add(cpData);
                }
            }

            return p;
        }
        #endregion

        #region **  Call Compalint Outstanding Store Proc    **
        public List<ComplaintPieData> getCompalintOutstanding(string TYPE)
        {
            List<ComplaintPieData> p = new List<ComplaintPieData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardCompalint_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMIAC));
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ComplaintPieData cpData = new ComplaintPieData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    cpData.Percentage = Convert.ToString(dr["PERCENTAGE"]);
                    p.Add(cpData);
                }
            }

            return p;
        }

        public List<ComplaintPieData> getComplaintOutstandingPendingatDesk(string TYPE)
        {
            List<ComplaintPieData> p = new List<ComplaintPieData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardComplaintPendingatDesk_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ComplaintPieData cpData = new ComplaintPieData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    p.Add(cpData);
                }
            }

            return p;
        }

        public List<ComplaintPieData> getComplaintOutstandingSourceRef(string TYPE)
        {
            List<ComplaintPieData> p = new List<ComplaintPieData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardComplaintSourceRef_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ComplaintPieData cpData = new ComplaintPieData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    p.Add(cpData);
                }
            }

            return p;
        }

        public List<ComplaintPieData> getCompalintOutstandingPendingatDeskDayWise(string TYPE)
        {
            List<ComplaintPieData> p = new List<ComplaintPieData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardCompalintDayWise_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ComplaintPieData cpData = new ComplaintPieData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    p.Add(cpData);
                }
            }

            return p;
        }
        #endregion

        #region **  Call IAC Outstanding Store Proc    **
        public List<IACPieData> getIACOutstanding(string TYPE)
        {
            List<IACPieData> p = new List<IACPieData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardIAC_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMIAC));
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    IACPieData cpData = new IACPieData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    cpData.Percentage = Convert.ToString(dr["PERCENTAGE"]);
                    p.Add(cpData);
                }
            }

            return p;
        }

        public List<IACPieData> getIACOutstandingPendingatDesk(string TYPE)
        {
            List<IACPieData> p = new List<IACPieData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardIACPendingatDesk_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMIAC));
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    IACPieData cpData = new IACPieData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    p.Add(cpData);
                }
            }

            return p;
        }

        public List<IACPieData> getIACOutstandingPendingatDeskDayWise(string TYPE)
        {
            List<IACPieData> p = new List<IACPieData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardIACDayWise_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMIAC));
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    IACPieData cpData = new IACPieData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    p.Add(cpData);
                }
            }

            return p;
        }
        #endregion

        #region **  Call Vigilance Store Proc    **
        public List<VigilanceChartData> getVigilanceOutstandingComplaintsWiseData(string TYPE)
        {
            List<VigilanceChartData> p = new List<VigilanceChartData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardVigilance_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    VigilanceChartData cpData = new VigilanceChartData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    p.Add(cpData);
                }
            }

            return p;
        }

        public List<VigilanceChartData> getVigilanceOutstandingIACWiseData(string TYPE)
        {
            List<VigilanceChartData> p = new List<VigilanceChartData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardVigilance_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    VigilanceChartData cpData = new VigilanceChartData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    //cpData.Code = Convert.ToString(dr["CODE"]);
                    p.Add(cpData);
                }
            }

            return p;
        }

        public List<VigilancePieData> getVigilanceOutstandingVigilanceWiseData(string TYPE)
        {
            List<VigilancePieData> p = new List<VigilancePieData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardVigilance_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    VigilancePieData cpData = new VigilancePieData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    cpData.Percentage = Convert.ToString(dr["PERCENTAGE"]);
                    //cpData.Code = Convert.ToString(dr["CODE"]);
                    p.Add(cpData);
                }
            }

            return p;
        }

        public List<VigilancePieData> getVigilanceOutstandingIACViewWiseData(string TYPE)
        {
            List<VigilancePieData> p = new List<VigilancePieData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardVigilance_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    VigilancePieData cpData = new VigilancePieData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    cpData.Percentage = Convert.ToString(dr["PERCENTAGE"]);
                    //cpData.Percentage = Convert.ToString(dr["PERCENTAGE"]);
                    p.Add(cpData);
                }
            }

            return p;
        }
        #endregion

        #region **  Call NPA Outstanding Store Proc    **
        public List<NPAPieData> getNPAOutstanding(string TYPE)
        {
            List<NPAPieData> p = new List<NPAPieData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardNPA_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMNPA));
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    NPAPieData cpData = new NPAPieData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    cpData.Percentage = Convert.ToString(dr["PERCENTAGE"]);
                    p.Add(cpData);
                }
            }

            return p;
        }

        public List<NPAPieData> getNPAOutstandingPendingatDeskDayWise(string TYPE)
        {
            List<NPAPieData> p = new List<NPAPieData>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDashboardNPADayWise_Outstanding]";

            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
            cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMNPA));
            cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
            cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    NPAPieData cpData = new NPAPieData();
                    cpData.NoofCount = Convert.ToString(dr["NOOFCOUNT"]);
                    cpData.Name = Convert.ToString(dr["NAME"]);
                    p.Add(cpData);
                }
            }

            return p;
        }
        #endregion

        #region **  Start Outstanding Graph    **
        protected void funcOutstandingComplaintsGraphCharts(string TYPE)
        {
            string xData = string.Empty;
            string yData = string.Empty;

            List<CompalintsChartData> obj = getOutstandingComplaintsWiseData(TYPE);
            foreach (var items in obj)
            {
                CompalintsChartData s = (CompalintsChartData)items;
                xData += s.Name + ",";
                yData += s.NoofCount + ",";
            }

            xData = objCommonFunction.removeStringLastComma(xData);
            yData = objCommonFunction.removeStringLastComma(yData);
            if ((!string.IsNullOrEmpty(xData)) && (!string.IsNullOrEmpty(yData)))
            {
                Highcharts graphChart = new Highcharts("Complaints")
                    .SetXAxis(new XAxis { Categories = xData.Split(',') })
                    .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Complaints Received" } })
                    .SetSeries(new Series { Data = new Data(new object[] { yData }), Name = "Complaints", Type = ChartTypes.Column })
                    .SetYAxis(new YAxis { StackLabels = new YAxisStackLabels { Enabled = true } })
                    .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { DataLabels = new PlotOptionsColumnDataLabels { Enabled = true } } })
                    .SetTitle(new Title { Text = "Complaints Outstanding Data" });
                lblOutstandingComplaintsGraph.Text = graphChart.ToHtmlString();
            }
        }

        protected void funcOutstandingIACReceivedGraphCharts(string TYPE)
        {
            string xData = string.Empty;
            string yData = string.Empty;

            List<IACChartData> obj = getOutstandingIACWiseData(TYPE);
            foreach (var items in obj)
            {
                IACChartData s = (IACChartData)items;
                xData += s.Name + ",";
                yData += s.NoofCount + ",";
            }

            xData = objCommonFunction.removeStringLastComma(xData);
            yData = objCommonFunction.removeStringLastComma(yData);
            if ((!string.IsNullOrEmpty(xData)) && (!string.IsNullOrEmpty(yData)))
            {
                Highcharts graphChart = new Highcharts("IAC")
                   .SetXAxis(new XAxis { Categories = xData.Split(',') })
                    .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "IAC Received" } })
                    .SetSeries(new Series { Data = new Data(new object[] { yData }), Name = "IAC" })
                    .SetYAxis(new YAxis { StackLabels = new YAxisStackLabels { Enabled = true } })
                    .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { DataLabels = new PlotOptionsColumnDataLabels { Enabled = true } } })
                    .SetTitle(new Title { Text = "IAC Wise Data" })
                    .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Column });
                lblOutstandingIACReceivedGraph.Text = graphChart.ToHtmlString();
            }
        }

        protected void funcOutstandingVigilanceReceivedGraphCharts(string TYPE)
        {
            string xData = string.Empty;
            string yData = string.Empty;

            List<VigilanceChartData> obj = getOutstandingVigilanceWiseData(TYPE);
            foreach (var items in obj)
            {
                VigilanceChartData s = (VigilanceChartData)items;
                xData += s.Name + ",";
                yData += s.NoofCount + ",";
            }

            xData = objCommonFunction.removeStringLastComma(xData);
            yData = objCommonFunction.removeStringLastComma(yData);
            if ((!string.IsNullOrEmpty(xData)) && (!string.IsNullOrEmpty(yData)))
            {
                Highcharts graphChart = new Highcharts("Vigilance")
                   .SetXAxis(new XAxis { Categories = xData.Split(',') })
                    .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Vigilance Received" } })
                    .SetSeries(new Series { Data = new Data(new object[] { yData }), Name = "Vigilance" })
                    .SetYAxis(new YAxis { StackLabels = new YAxisStackLabels { Enabled = true } })
                    .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { DataLabels = new PlotOptionsColumnDataLabels { Enabled = true } } })
                    .SetTitle(new Title { Text = "Outstanding Vigilance Cases" })
                    .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Column });
                lblOutstandingVigilanceReceivedGraph.Text = graphChart.ToHtmlString();
            }
        }

        protected void funcOutstandingIACVigilancePieCharts(string TYPE)
        {
            string Data = string.Empty;
            List<IACVigilancePieData> obj = getOutstandingIACViewWiseData(TYPE);
            foreach (var items in obj)
            {
                IACVigilancePieData s = (IACVigilancePieData)items;
                Data += s.Name + ": " + s.NoofCount + ": " + s.Percentage + ",";
            }

            Data = objCommonFunction.removeStringLastComma(Data);
            if (!string.IsNullOrEmpty(Data))
            {
                DotNet.Highcharts.Highcharts DAPieChart = new DotNet.Highcharts.Highcharts("VigilanceViewPieChart")
                .SetTitle(new Title { Text = "IAC Vigilance Cases" })
                .SetSubtitle(new Subtitle { Text = "From " + txtFromDate.Text + " to " + txtToDate.Text })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "IAC Vigilance Wise Data",
                    Data = new Data(new object[]{
                //piedata
                new object[] { (obj[0].Name + ", " + obj[0].NoofCount + ", " + obj[0].Percentage), obj[0].NoofCount },
                new object[] { (obj[1].Name + ", " + obj[1].NoofCount + ", " + obj[1].Percentage), obj[1].NoofCount },
                new object[] { (obj[2].Name + ", " + obj[2].NoofCount + ", " + obj[2].Percentage), obj[2].NoofCount },})
                });

                lblOutstandingIACViewVigilancePie.Text = DAPieChart.ToHtmlString();
            }
        }

        protected void funcOutstandingNonVigilancePieCharts(string TYPE)
        {
            List<IACVigilancePieData> obj = getOutstandingIACViewWiseData(TYPE);
            DotNet.Highcharts.Highcharts DAPieChart = new DotNet.Highcharts.Highcharts("NonVigilanceViewPieChart")
            .SetTitle(new Title { Text = "IAC Non Vigilance Cases" })
            .SetSubtitle(new Subtitle { Text = "From " + txtFromDate.Text + " to " + txtToDate.Text })

            .SetSeries(new Series
            {
                Type = ChartTypes.Pie,
                Name = "IAC Non-Vigilance Wise Data",
                Data = new Data(new object[]{
                //piedata
                new object[] { (obj[0].Name + ", " + obj[0].NoofCount + ", " + obj[0].Percentage), obj[0].NoofCount},
                new object[] { (obj[1].Name + ", " + obj[1].NoofCount + ", " + obj[1].Percentage), obj[1].NoofCount},
                new object[] { (obj[2].Name + ", " + obj[2].NoofCount + ", " + obj[2].Percentage), obj[2].NoofCount},})
            });

            lblOutstandingIACViewNonVigilancePie.Text = DAPieChart.ToHtmlString();
        }
        #endregion

        #region **  Start Complaint Graph    **
        protected void funcOutstandingComplaintPieCharts(string TYPE)
        {
            string Data = string.Empty;
            List<ComplaintPieData> obj = getCompalintOutstanding(TYPE);
            foreach (var items in obj)
            {
                ComplaintPieData s = (ComplaintPieData)items;
                Data += s.Name + ": " + s.NoofCount + ": " + s.Percentage + ",";
            }

            Data = objCommonFunction.removeStringLastComma(Data);
            if (!string.IsNullOrEmpty(Data))
            {
                DotNet.Highcharts.Highcharts DAPieChart = new DotNet.Highcharts.Highcharts("ComplaintViewPieChart")
                .SetTitle(new Title { Text = "Concluded/ Dealt With" })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "NPA Outstanding",
                    Data = new Data(new object[]{
                //piedata
                new object[] { (obj[0].Name + ", " + obj[0].NoofCount + ", " + obj[0].Percentage), obj[0].NoofCount },
                new object[] { (obj[1].Name + ", " + obj[1].NoofCount + ", " + obj[1].Percentage), obj[1].NoofCount },})
                });

                lblComplaintOutstandingPieChart.Text = DAPieChart.ToHtmlString();
            }
        }

        protected void funcOutstandingComplaintPendingatDeskCharts(string TYPE)
        {
            string xData = string.Empty;
            string yData = string.Empty;

            List<ComplaintPieData> obj = getComplaintOutstandingPendingatDesk(TYPE);
            foreach (var items in obj)
            {
                ComplaintPieData s = (ComplaintPieData)items;
                xData += s.Name + ",";
                yData += s.NoofCount + ",";
            }

            xData = objCommonFunction.removeStringLastComma(xData);
            yData = objCommonFunction.removeStringLastComma(yData);

            if ((!string.IsNullOrEmpty(xData)) && (!string.IsNullOrEmpty(yData)))
            {

                Highcharts graphChart = new Highcharts("ComplaintPendingatDesk")
                       .SetXAxis(new XAxis { Categories = xData.Split(',') })
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Pending at Desk" } })
                        .SetSeries(new Series { Data = new Data(new object[] { yData }), Name = "Complaint - Pending at Desk" })
                        .SetYAxis(new YAxis { StackLabels = new YAxisStackLabels { Enabled = true } })
                        .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { DataLabels = new PlotOptionsColumnDataLabels { Enabled = true } } })
                        .SetTitle(new Title { Text = "Compalint Pending with other Department" })
                        .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Column });

                lblComplaintOutstandingPendingAtDesk.Text = graphChart.ToHtmlString();
            }
        }

        protected void funcOutstandingComplaintSourceRefCharts(string TYPE)
        {
            string xData = string.Empty;
            string yData = string.Empty;

            List<ComplaintPieData> obj = getComplaintOutstandingSourceRef(TYPE);
            foreach (var items in obj)
            {
                ComplaintPieData s = (ComplaintPieData)items;
                xData += s.Name + ",";
                yData += s.NoofCount + ",";
            }

            xData = objCommonFunction.removeStringLastComma(xData);
            yData = objCommonFunction.removeStringLastComma(yData);

            if ((!string.IsNullOrEmpty(xData)) && (!string.IsNullOrEmpty(yData)))
            {

                Highcharts graphChart = new Highcharts("ComplaintSourceRef")
                       .SetXAxis(new XAxis { Categories = xData.Split(',') })
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Source Reference" } })
                        .SetSeries(new Series { Data = new Data(new object[] { yData }), Name = "Complaint - Source Reference" })
                        .SetYAxis(new YAxis { StackLabels = new YAxisStackLabels { Enabled = true } })
                        .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { DataLabels = new PlotOptionsColumnDataLabels { Enabled = true } } })
                        .SetTitle(new Title { Text = "Compalint Pending As Per Source Reference" })
                        .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Column });

                lblComplaintOutstandingSourceRef.Text = graphChart.ToHtmlString();
            }
        }

        protected void funcOutstandingComplaintPendingatDeskDayWise(string TYPE)
        {
            string xData = string.Empty;
            string yData = string.Empty;

            List<ComplaintPieData> obj = getCompalintOutstandingPendingatDeskDayWise(TYPE);
            foreach (var items in obj)
            {
                ComplaintPieData s = (ComplaintPieData)items;
                xData += s.Name + ",";
                yData += s.NoofCount + ",";
            }

            xData = objCommonFunction.removeStringLastComma(xData);
            yData = objCommonFunction.removeStringLastComma(yData);
            if ((!string.IsNullOrEmpty(xData)) && (!string.IsNullOrEmpty(yData)))
            {
                Highcharts graphChart = new Highcharts("ComplaintPendingatDeskDayWise")
                   .SetXAxis(new XAxis { Categories = xData.Split(',') })
                    .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Pending at Desk" } })
                    .SetSeries(new Series { Data = new Data(new object[] { yData }), Name = "Complaint - Pending at Desk - Day wise" })
                    .SetYAxis(new YAxis { StackLabels = new YAxisStackLabels { Enabled = true } })
                    .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { DataLabels = new PlotOptionsColumnDataLabels { Enabled = true } } })
                    .SetTitle(new Title { Text = "Complaints Outstanding" })
                    .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Column });

                lblComplaintOutstandingPendingAtDeskDayWise.Text = graphChart.ToHtmlString();
            }
        }
        #endregion

        #region **  Start IAC Graph    **
        protected void funcOutstandingIACPieCharts(string TYPE)
        {
            string Data = string.Empty;
            List<IACPieData> obj = getIACOutstanding(TYPE);
            foreach (var items in obj)
            {
                IACPieData s = (IACPieData)items;
                Data += s.Name + ": " + s.NoofCount + ": " + s.Percentage + ",";
            }

            Data = objCommonFunction.removeStringLastComma(Data);
            if (!string.IsNullOrEmpty(Data))
            {
                DotNet.Highcharts.Highcharts DAPieChart = new DotNet.Highcharts.Highcharts("IACViewPieChart")
                .SetTitle(new Title { Text = "Concluded/ Dealt With" })
                //.SetSubtitle(new Subtitle { Text = "From 01-04-2022 to " + DateTime.Now.ToString("dd-MM-yyyy") })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "Concluded/ Dealt With",
                    Data = new Data(new object[]{
                //piedata
                new object[] { (obj[0].Name + ", " + obj[0].NoofCount + ", " + obj[0].Percentage), obj[0].NoofCount },
                new object[] { (obj[1].Name + ", " + obj[1].NoofCount + ", " + obj[1].Percentage), obj[1].NoofCount },})
                });

                lblIACOutstandingPieChart.Text = DAPieChart.ToHtmlString();
            }
        }

        protected void funcOutstandingIACPendingatDeskCharts(string TYPE)
        {
            string xData = string.Empty;
            string yData = string.Empty;

            List<IACPieData> obj = getIACOutstandingPendingatDesk(TYPE);
            foreach (var items in obj)
            {
                IACPieData s = (IACPieData)items;
                xData += s.Name + ",";
                yData += s.NoofCount + ",";
            }

            xData = objCommonFunction.removeStringLastComma(xData);
            yData = objCommonFunction.removeStringLastComma(yData);

            if ((!string.IsNullOrEmpty(xData)) && (!string.IsNullOrEmpty(yData)))
            {

                Highcharts graphChart = new Highcharts("IACPendingatDesk")
                       .SetXAxis(new XAxis { Categories = xData.Split(',') })
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Pending at Desk" } })
                        .SetSeries(new Series { Data = new Data(new object[] { yData }), Name = "IAC - Pending at Desk" })
                        .SetYAxis(new YAxis { StackLabels = new YAxisStackLabels { Enabled = true } })
                        .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { DataLabels = new PlotOptionsColumnDataLabels { Enabled = true } } })
                        .SetTitle(new Title { Text = "Pending at Desk" })
                        .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Column });

                lblIACOutstandingPendingAtDesk.Text = graphChart.ToHtmlString();
            }
        }

        protected void funcOutstandingIACPendingatDeskDayWise(string TYPE)
        {
            string xData = string.Empty;
            string yData = string.Empty;

            List<IACPieData> obj = getIACOutstandingPendingatDeskDayWise(TYPE);
            foreach (var items in obj)
            {
                IACPieData s = (IACPieData)items;
                xData += s.Name + ",";
                yData += s.NoofCount + ",";
            }

            xData = objCommonFunction.removeStringLastComma(xData);
            yData = objCommonFunction.removeStringLastComma(yData);
            if ((!string.IsNullOrEmpty(xData)) && (!string.IsNullOrEmpty(yData)))
            {
                Highcharts graphChart = new Highcharts("IACPendingatDeskDayWise")
                   .SetXAxis(new XAxis { Categories = xData.Split(',') })
                    .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Pending at Desk" } })
                    .SetSeries(new Series { Data = new Data(new object[] { yData }), Name = "IAC - Pending at Desk - Day wise" })
                    .SetYAxis(new YAxis { StackLabels = new YAxisStackLabels { Enabled = true } })
                    .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { DataLabels = new PlotOptionsColumnDataLabels { Enabled = true } } })
                    .SetTitle(new Title { Text = "Pending at Desk - Day Wise" })
                    .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Column });

                lblIACOutstandingPendingAtDeskDayWise.Text = graphChart.ToHtmlString();
            }
        }
        #endregion

        #region **  Start Vigilance Graph    **
        protected void funcOutstandingVigilanceGraphCharts(string TYPE)
        {
            string xData = string.Empty;
            string yData = string.Empty;

            List<VigilanceChartData> obj = getVigilanceOutstandingComplaintsWiseData(TYPE);
            foreach (var items in obj)
            {
                VigilanceChartData s = (VigilanceChartData)items;
                xData += s.Name + ",";
                yData += s.NoofCount + ",";
            }

            xData = objCommonFunction.removeStringLastComma(xData);
            yData = objCommonFunction.removeStringLastComma(yData);
            if ((!string.IsNullOrEmpty(xData)) && (!string.IsNullOrEmpty(yData)))
            {
                Highcharts graphChart = new Highcharts("VigilanceOutstanding")
                    .SetXAxis(new XAxis { Categories = xData.Split(',') })
                    .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Vigilance Received" } })
                    .SetSeries(new Series { Data = new Data(new object[] { yData }), Name = "Complaints", Type = ChartTypes.Column })
                    .SetYAxis(new YAxis { StackLabels = new YAxisStackLabels { Enabled = true } })
                    .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { DataLabels = new PlotOptionsColumnDataLabels { Enabled = true } } })
                    .SetTitle(new Title { Text = "Outstanding Vigilance Cases" });
                lblVigilanceOutstandingPendingAtDsk.Text = graphChart.ToHtmlString();
            }
        }

        protected void funcVigilanceOutstandingGraphCharts(string TYPE)
        {
            string xData = string.Empty;
            string yData = string.Empty;

            List<VigilanceChartData> obj = getVigilanceOutstandingIACWiseData(TYPE);
            foreach (var items in obj)
            {
                VigilanceChartData s = (VigilanceChartData)items;
                xData += s.Name + "|";
                yData += s.NoofCount + ",";
            }

            xData = objCommonFunction.removeStringLastPipe(xData);
            yData = objCommonFunction.removeStringLastComma(yData);

            if ((!string.IsNullOrEmpty(xData)) && (!string.IsNullOrEmpty(yData)))
            {
                Highcharts graphChart = new Highcharts("VigilanceScale")
                   .SetXAxis(new XAxis { Categories = xData.Split('|') })
                    .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Vigilance Received" } })
                    .SetSeries(new Series { Data = new Data(new object[] { yData }), Name = "Vigilance" })
                    .SetYAxis(new YAxis { StackLabels = new YAxisStackLabels { Enabled = true } })
                    .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { DataLabels = new PlotOptionsColumnDataLabels { Enabled = true } } })
                    .SetTitle(new Title { Text = "Charge Sheet Served Cases" })
                    .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Column });
                lblVigilanceOutstandingChargeSheetGraph.Text = graphChart.ToHtmlString();
            }
        }

        protected void funcOutstandingVigilanceIACPieCharts(string TYPE)
        {
            string Data = string.Empty;
            List<VigilancePieData> obj = getVigilanceOutstandingVigilanceWiseData(TYPE);
            foreach (var items in obj)
            {
                VigilancePieData s = (VigilancePieData)items;
                Data += s.Name + ": " + s.NoofCount + ": " + s.Percentage + ",";
            }

            Data = objCommonFunction.removeStringLastComma(Data);
            if (!string.IsNullOrEmpty(Data))
            {
                DotNet.Highcharts.Highcharts DAPieChart = new DotNet.Highcharts.Highcharts("VigilancePendingatdesk")
                .SetTitle(new Title { Text = "Nature of Charge Sheet" })
                //.SetSubtitle(new Subtitle { Text = "From 01-04-2022 to " + DateTime.Now.ToString("dd-MM-yyyy") })
                //.SetSeries(new Series { Data = new Data(new object[] { Data }), Name = "IAC Vigilance", Type = ChartTypes.Pie });
                //.SetSeries(new Series
                //{
                //    Type = ChartTypes.Pie,
                //    Name = "IAC Vigilance Wise Data",
                //    Data = new Data(new object[]{Data})
                //});

                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "IAC Vigilance Wise Data",
                    Data = new Data(new object[]{
                //piedata
                new object[] { (obj[0].Name + ", " + obj[0].NoofCount + ", " + obj[0].Percentage), obj[0].NoofCount },
                new object[] { (obj[1].Name + ", " + obj[1].NoofCount + ", " + obj[1].Percentage), obj[1].NoofCount },})
                });

                lblVigilanceOutstandingNatureChargeSheetPieChart.Text = DAPieChart.ToHtmlString();
            }
        }

        protected void funcOutstandingVigilanceNonVigilancePieCharts(string TYPE)
        {
            List<VigilancePieData> obj = getVigilanceOutstandingIACViewWiseData(TYPE);
            DotNet.Highcharts.Highcharts DAPieChart = new DotNet.Highcharts.Highcharts("VigilanceChargeSheet")
            .SetTitle(new Title { Text = "Charge Sheet yet to be Served" })
            //.SetSubtitle(new Subtitle { Text = "From 01-04-2022 to " + DateTime.Now.ToString("dd-MM-yyyy") })

            .SetSeries(new Series
            {
                Type = ChartTypes.Pie,
                Name = "IAC Non-Vigilance Wise Data",
                Data = new Data(new object[]{
                //piedata
                new object[] { (obj[0].Name + ", " + obj[0].NoofCount + ", " + obj[0].Percentage), obj[0].NoofCount},
                new object[] { (obj[1].Name + ", " + obj[1].NoofCount + ", " + obj[1].Percentage), obj[1].NoofCount},})
            });

            lblVigilanceOutstandingChargeSheetPieChart.Text = DAPieChart.ToHtmlString();
        }
        #endregion

        #region **  Start NPA Graph    **
        protected void funcOutstandingNPAPieCharts(string TYPE)
        {
            string Data = string.Empty;
            List<NPAPieData> obj = getNPAOutstanding(TYPE);
            foreach (var items in obj)
            {
                NPAPieData s = (NPAPieData)items;
                Data += s.Name + ": " + s.NoofCount + ": " + s.Percentage + ",";
            }

            Data = objCommonFunction.removeStringLastComma(Data);
            if (!string.IsNullOrEmpty(Data))
            {
                DotNet.Highcharts.Highcharts DAPieChart = new DotNet.Highcharts.Highcharts("NPAViewPieChart")
                .SetTitle(new Title { Text = "NPA Outstanding" })
                //.SetSubtitle(new Subtitle { Text = "From 01-04-2022 to " + DateTime.Now.ToString("dd-MM-yyyy") })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "NPA Outstanding",
                    Data = new Data(new object[]{
                //piedata
                new object[] { (obj[0].Name + ", " + obj[0].NoofCount + ", " + obj[0].Percentage), obj[0].NoofCount },
                new object[] { (obj[1].Name + ", " + obj[1].NoofCount + ", " + obj[1].Percentage), obj[1].NoofCount },
                new object[] { (obj[2].Name + ", " + obj[2].NoofCount + ", " + obj[2].Percentage), obj[2].NoofCount },})

                });

                lblNPAOutstandingPieChart.Text = DAPieChart.ToHtmlString();
            }
        }

        protected void funcOutstandingNPAPendingatDeskDayWise(string TYPE)
        {
            string xData = string.Empty;
            string yData = string.Empty;

            List<NPAPieData> obj = getNPAOutstandingPendingatDeskDayWise(TYPE);
            foreach (var items in obj)
            {
                NPAPieData s = (NPAPieData)items;
                xData += s.Name + ",";
                yData += s.NoofCount + ",";
            }

            xData = objCommonFunction.removeStringLastComma(xData);
            yData = objCommonFunction.removeStringLastComma(yData);
            if ((!string.IsNullOrEmpty(xData)) && (!string.IsNullOrEmpty(yData)))
            {
                Highcharts graphChart = new Highcharts("NPAPendingatDeskDayWise")
                   .SetXAxis(new XAxis { Categories = xData.Split(',') })
                    .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Pending at Desk" } })
                    .SetSeries(new Series { Data = new Data(new object[] { yData }), Name = "NPA" })
                    .SetYAxis(new YAxis { StackLabels = new YAxisStackLabels { Enabled = true } })
                    .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { DataLabels = new PlotOptionsColumnDataLabels { Enabled = true } } })
                    .SetTitle(new Title { Text = "NPA Pendency with other Department" })
                    .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Column });

                lblNPAOutstandingPendingAtDesk.Text = graphChart.ToHtmlString();
            }
        }
        #endregion

        #region **  Start Dealing CM    **
        protected void ddlDealingCMIAC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(objCommonFunction.ddlSelectedValue(ddlDealingCMIAC)))
            {
                funcOutstandingIACPieCharts("DETAILS");
                funcOutstandingIACPendingatDeskCharts("DETAILS");
                funcOutstandingIACPendingatDeskDayWise("DETAILS");
                pnlIACOutstandingDetails.Visible = true;
            }
        }

        protected void ddlDealingCMComplaint_SelectedIndexChanged(object sender, EventArgs e)
        {
            funcOutstandingComplaintPieCharts("DETAILS");
            funcOutstandingComplaintPendingatDeskCharts("DETAILS");
            funcOutstandingComplaintSourceRefCharts("DETAILS");
            funcOutstandingComplaintPendingatDeskDayWise("DETAILS");
            pnlComplaintOutstandingDetails.Visible = true;
            lblComplaintOutstandingSourceRef.Visible = true;
        }

        protected void ddlDealingCMVigilance_SelectedIndexChanged(object sender, EventArgs e)
        {
            funcOutstandingVigilanceGraphCharts("COMPLAINTS");
            funcVigilanceOutstandingGraphCharts("VIGILANCE");
            funcOutstandingVigilanceIACPieCharts("VIGILANCE_PIECHART");
            funcOutstandingVigilanceNonVigilancePieCharts("NONVIGILANCE");
            pnlVigilanceOutstandingDetails.Visible = true;
        }

        protected void ddlDealingCMNPA_SelectedIndexChanged(object sender, EventArgs e)
        {
            funcOutstandingNPAPieCharts("DETAILS");
            funcOutstandingNPAPendingatDeskDayWise("DETAILS");
            pnlNPAOutstandingDetails.Visible = true;
        }
        #endregion

        #region **  Button Click Event    **
        protected void btnOutstanding_Click(object sender, EventArgs e)
        {
            funcHideUnhide("OUTSTANDING");
            funcOutstandingComplaintsGraphCharts("COMPLAINTS");
            funcOutstandingIACReceivedGraphCharts("IAC");
            funcOutstandingVigilanceReceivedGraphCharts("VIGILANCE");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "clientscript", "document.getElementById('divPie').style.visibility = 'visible';", true);
            //divPie.Visible = true;
            //btnVigPie.Visible = true;
            //btnNonVigPie.Visible = true;
        }

        protected void btnIACOutstanding_Click(object sender, EventArgs e)
        {
            funcHideUnhide("IAC_OUTSTANDING");
        }

        protected void btnComplaintOutstanding_Click(object sender, EventArgs e)
        {
            funcHideUnhide("COMPLAINT_OUTSTANDING");
        }

        protected void btnVigilanceOutstanding_Click(object sender, EventArgs e)
        {
            funcHideUnhide("VIGILANCE_OUTSTANDING");
        }

        protected void btnNPAOutstanding_Click(object sender, EventArgs e)
        {
            funcHideUnhide("NPA_OUTSTANDING");
        }
        #endregion

        protected void btnGetDetails_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (string.IsNullOrEmpty(txtFromDate.Text.Trim()))
            {
                lblMsg.Text = "Please select From Date";
                return;
            }

            if (string.IsNullOrEmpty(txtToDate.Text.Trim()))
            {
                lblMsg.Text = "Please select To Date";
                return;
            }

            funcOutstandingIACVigilancePieCharts("VIGILANCE");
            funcOutstandingNonVigilancePieCharts("NONVIGILANCE");
        }

        protected void btnPWOD_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingWithOtherDept_" + DateTime.Now.ToString("ddMMyyyy");

            try
            {
                string codType = "Pending with other Department";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINTS");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnPAD_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "Pending at Desk_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at Desk";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINTS");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnDWPR_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "DealtWithPendingOther_ " + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Dealt With-Pending for Other Reference";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINTS");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnPDC_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingWithDACon_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending with DA for Concurrence";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "IAC");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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


        protected void btnPDI_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingWithDAInfo_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending with DA for Information";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "IAC");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCS_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "ClarificationReturnIAD_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Clarification Sought/ Returned to IAD";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "IAC");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnPD_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingAtDesk_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at Desk";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "IAC");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnDWP_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "DealtOtherRef_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Dealt With-Pending for Other Reference";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "IAC");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnPDA_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "Pending with DA_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending with DA";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnPWIO_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "Pending with IO_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending with IO";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCourt_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "Court_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Court";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnPDesk_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "Pending at Desk_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at Desk";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCVC_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "Sent to CVC_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Sent to CVC";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnFOI_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "Final order issued_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Final order issued";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVigPie_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "IAC Vigilance Cases_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "IAC Vigilance Cases";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "PIECHART");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_FROMDATE", txtFromDate.Text.Trim());
                cmd.Parameters.AddWithValue("@p_TODATE", txtToDate.Text.Trim());

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnNonVigPie_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "IACNonVigilanceCases_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "IAC Non Vigilance Cases";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboard_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "PIECHART");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_FROMDATE", txtFromDate.Text.Trim());
                cmd.Parameters.AddWithValue("@p_TODATE", txtToDate.Text.Trim());

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCPWOD_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendotherDept_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending with other Department";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCPAD_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingAtDesk_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at Desk";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCDWPOR_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "DealtWithPending_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Dealt With-Pending for Other Reference";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCPHM_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingHOMisc_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at HO (Misc)";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCPHF_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingHOFRMD_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at HO FRMD";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCHH_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingatHOHRD_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at HO HRD";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCHIAD_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingatHOIAD_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at HO IAD";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCHS_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingatHOSASTRA_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at HO SASTRA";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCHZ_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingatHOZO_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at HO ZO";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCPO_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingAtOther_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at Other";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));
                cmd.Parameters.AddWithValue("@p_CODType", codType);

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCVO_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingAtVO_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at VO";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        //protected void btnCPMRef_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    string fileName = "PMOREF_" + DateTime.Now.ToString("ddMMyyyy");
        //    try
        //    {
        //        string codType = "PMO REF";
        //        con.Open();
        //        cmd.Connection = con;
        //        cmd.Parameters.Clear();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

        //        cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
        //        cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
        //        cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
        //        cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
        //        cmd.Parameters.AddWithValue("@p_CODType", codType);
        //        cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

        //        cmd.CommandTimeout = 0;
        //        sda.Fill(dt);
        //        funcConvertToExcelCOD(dt, fileName);
        //    }
        //    catch (Exception es)
        //    {
        //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
        //    }

        //    finally
        //    {
        //        con.Close();
        //        sda.Dispose();
        //        con.Dispose();
        //    }
        //}

        //protected void btnCCVCP_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    string fileName = "CVCPORTAL_" + DateTime.Now.ToString("ddMMyyyy");
        //    try
        //    {
        //        string codType = "CVC PORTAL";
        //        con.Open();
        //        cmd.Connection = con;
        //        cmd.Parameters.Clear();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

        //        cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
        //        cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
        //        cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
        //        cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
        //        cmd.Parameters.AddWithValue("@p_CODType", codType);
        //        cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

        //        cmd.CommandTimeout = 0;
        //        sda.Fill(dt);
        //        funcConvertToExcelCOD(dt, fileName);
        //    }
        //    catch (Exception es)
        //    {
        //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
        //    }

        //    finally
        //    {
        //        con.Close();
        //        sda.Dispose();
        //        con.Dispose();
        //    }
        //}

        protected void btnCPolice_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "POLICE_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "POLICE";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCRBI_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "RBI_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "RBI";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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


        //protected void btnCFRMD_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    string fileName = "FRMD_" + DateTime.Now.ToString("ddMMyyyy");
        //    try
        //    {
        //        string codType = "FRMD";
        //        con.Open();
        //        cmd.Connection = con;
        //        cmd.Parameters.Clear();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

        //        cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
        //        cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
        //        cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
        //        cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
        //        cmd.Parameters.AddWithValue("@p_CODType", codType);
        //        cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

        //        cmd.CommandTimeout = 0;
        //        sda.Fill(dt);
        //        funcConvertToExcelCOD(dt, fileName);
        //    }
        //    catch (Exception es)
        //    {
        //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
        //    }

        //    finally
        //    {
        //        con.Close();
        //        sda.Dispose();
        //        con.Dispose();
        //    }
        //}

        protected void btnCCBI_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "CBI_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "CBI";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        //protected void btnCMATR_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    string fileName = "MofATR_" + DateTime.Now.ToString("ddMMyyyy");
        //    try
        //    {
        //        string codType = "MofATR";
        //        con.Open();
        //        cmd.Connection = con;
        //        cmd.Parameters.Clear();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

        //        cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
        //        cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
        //        cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
        //        cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
        //        cmd.Parameters.AddWithValue("@p_CODType", codType);
        //        cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

        //        cmd.CommandTimeout = 0;
        //        sda.Fill(dt);
        //        funcConvertToExcelCOD(dt, fileName);
        //    }
        //    catch (Exception es)
        //    {
        //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
        //    }

        //    finally
        //    {
        //        con.Close();
        //        sda.Dispose();
        //        con.Dispose();
        //    }
        //}

        //protected void btnCMARD_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    string fileName = "MARD_" + DateTime.Now.ToString("ddMMyyyy");
        //    try
        //    {
        //        string codType = "MARD";
        //        con.Open();
        //        cmd.Connection = con;
        //        cmd.Parameters.Clear();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

        //        cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
        //        cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
        //        cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
        //        cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
        //        cmd.Parameters.AddWithValue("@p_CODType", codType);
        //        cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

        //        cmd.CommandTimeout = 0;
        //        sda.Fill(dt);
        //        funcConvertToExcelCOD(dt, fileName);
        //    }
        //    catch (Exception es)
        //    {
        //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
        //    }

        //    finally
        //    {
        //        con.Close();
        //        sda.Dispose();
        //        con.Dispose();
        //    }
        //}

        //protected void btnCIADHO_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    string fileName = "IADHO_" + DateTime.Now.ToString("ddMMyyyy");
        //    try
        //    {
        //        string codType = "IAD HO";
        //        con.Open();
        //        cmd.Connection = con;
        //        cmd.Parameters.Clear();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

        //        cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
        //        cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
        //        cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
        //        cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
        //        cmd.Parameters.AddWithValue("@p_CODType", codType);
        //        cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

        //        cmd.CommandTimeout = 0;
        //        sda.Fill(dt);
        //        funcConvertToExcelCOD(dt, fileName);
        //    }
        //    catch (Exception es)
        //    {
        //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
        //    }

        //    finally
        //    {
        //        con.Close();
        //        sda.Dispose();
        //        con.Dispose();
        //    }
        //}

        protected void btnCCVC_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "CVC_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "CVC";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        //protected void btnCAnno_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    string fileName = "ANNONYMOUS_" + DateTime.Now.ToString("ddMMyyyy");
        //    try
        //    {
        //        string codType = "ANNONYMOUS";
        //        con.Open();
        //        cmd.Connection = con;
        //        cmd.Parameters.Clear();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

        //        cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
        //        cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
        //        cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
        //        cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
        //        cmd.Parameters.AddWithValue("@p_CODType", codType);
        //        cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

        //        cmd.CommandTimeout = 0;
        //        sda.Fill(dt);
        //        funcConvertToExcelCOD(dt, fileName);
        //    }
        //    catch (Exception es)
        //    {
        //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
        //    }

        //    finally
        //    {
        //        con.Close();
        //        sda.Dispose();
        //        con.Dispose();
        //    }
        //}

        protected void btnCEOW_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "EOW_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "EOW";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnCMOF_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "MOF_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "MOF";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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
        protected void btnCOther_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "Others_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Others";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "COMPLAINT");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMComplaint));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnICDW_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendotherDept_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Concluded with";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "IAC");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMIAC));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnICS_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "ClaraficSought_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Clarafication sought";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "IAC");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMIAC));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnIPAD_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingAtDesk_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at Desk";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "IAC");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMIAC));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnIPDI_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingDAInform_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending with DA for information";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "IAC");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMIAC));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnIL15_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "Lessthan15 Days_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Less than 15 Days";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "IAC");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMIAC));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnIG15_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "Greater15Days_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Greater than 15 Days";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "IAC");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMIAC));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVPWD_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingWithDA_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending with DA";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVPWI_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingwithIO_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending with IO";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVC_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "Court_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Court";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVPAD_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingatDesk_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at Desk";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVCVC_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "SenttoCVC_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Sent to CVC";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVFOI_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "FinalOrderIssued_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Final Order Issued";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        //protected void btnV2S_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    string fileName = "_2ndStage_" + DateTime.Now.ToString("ddMMyyyy");
        //    try
        //    {
        //        string codType = "_2nd Stage";
        //        con.Open();
        //        cmd.Connection = con;
        //        cmd.Parameters.Clear();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

        //        cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
        //        cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
        //        cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
        //        cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
        //        cmd.Parameters.AddWithValue("@p_CODType", codType);
        //        cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

        //        cmd.CommandTimeout = 0;
        //        sda.Fill(dt);
        //        funcConvertToExcelCOD(dt, fileName);
        //    }
        //    catch (Exception es)
        //    {
        //        VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
        //    }

        //    finally
        //    {
        //        con.Close();
        //        sda.Dispose();
        //        con.Dispose();
        //    }
        //}

        protected void btnVDEP_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "DeptEnqInProg_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Dept Enq in Prog";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVDEC_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "DeptEnqConc_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Dept Enq Conc";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVSBC_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "StayedbyCourt_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Stayed by Court";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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
        protected void btnVM_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "Minor_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Minor";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVCPFO_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "ClosedPenOther_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Closed, Pen. for other";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVFOA_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "FinalOrderAwaited_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Final Order Awaited";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVSSA_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "SSA Awaited_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "SSA Awaited";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVNCS_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "NatureChgeSheet_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Nature of Charge Sheet";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnVCSS_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "ChargeSheetYet_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Charge Sheet yet to be Served";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "VIGILANCE");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMVigilance));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnNNO_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "NPAOutstanding_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "NPA Outstanding";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "NPA");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMNPA));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnNPAH_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingatHO_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at HO";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "NPA");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMNPA));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnNPHF_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingHOFRMD_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at HO FRMD";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "NPA");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMNPA));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnNPHH_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingatHOHRD_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at HO HRD";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "NPA");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMNPA));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnNPHI_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingatHOIAD_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at HO IAD";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "NPA");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMNPA));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnNHS_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingSASTRA_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at HO SASTRA";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "NPA");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMNPA));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnNPHZ_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingAtHOZO_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at HO ZO";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "NPA");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMNPA));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnNPO_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingAtOther_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending At Other";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "NPA");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMNPA));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void btnNPZAO_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            string fileName = "PendingAtZAO_" + DateTime.Now.ToString("ddMMyyyy");
            try
            {
                string codType = "Pending at ZAO";
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spDashboardCompalintIACVigilanceNPA_OutstandingData]";

                cmd.Parameters.AddWithValue("@p_TYPE", "NPA");
                cmd.Parameters.AddWithValue("@p_VIEW", "DETAILS");
                cmd.Parameters.AddWithValue("@p_ROLE", Convert.ToString(Session["ROLE"]));
                cmd.Parameters.AddWithValue("@p_SOLID", Convert.ToString(Session["SOLID"]));
                cmd.Parameters.AddWithValue("@p_CODType", codType);
                cmd.Parameters.AddWithValue("@p_DEALINGCM", objCommonFunction.ddlSelectedValue(ddlDealingCMNPA));

                cmd.CommandTimeout = 0;
                sda.Fill(dt);
                funcConvertToExcelCOD(dt, fileName);
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

        protected void funcConvertToExcelCOD(DataTable dt, string fileName)
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, fileName);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("", "");
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xlsx");
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

        protected void btnABBFF_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Mis/frmABBFF.aspx");

        }
    }
}
