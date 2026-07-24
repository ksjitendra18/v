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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using ClosedXML.Excel;
using System.Configuration;
using System.Data.OleDb;
using System.Text;
using iTextSharp.text.pdf;

namespace VMISP.Upload
{
    public partial class frmAccessUpload : System.Web.UI.Page
    {
        #region ** declare Variable **
        int intErrCode = 0;
        string strMsg = string.Empty;
        string strErrMsg = string.Empty;
        string strUser = string.Empty;
        string strUserRole = string.Empty;

        StringBuilder strScript = new StringBuilder();
        CommonFunction objCommonFunction = new CommonFunction();

        DataSet dsMain = new DataSet();
        DataTable dtCOMPLAINTS = new DataTable();
        DataTable dtMISC = new DataTable();
        DataTable dtOPERATIONALREFERENCEETC = new DataTable();
        DataTable dtRRB = new DataTable();
        DataTable dtSR = new DataTable();

        string strSRNO = string.Empty;
        string strBRCOMPLAINT = string.Empty;
        string strCIRCLEOFFICE = string.Empty;
        string strRNO = string.Empty;
        string strACCUSED = string.Empty;
        string strALLEGATIONS = string.Empty;
        decimal decAMOUNT = 0;
        string strFINALACTION = string.Empty;
        string strZONE = string.Empty;
        string strSTATUSCODE = string.Empty;
        string strREGION = string.Empty;
        string strPRESENTPOSTING = string.Empty;
        string strACCOUNTNAME = string.Empty;
        string strCASECLOSE = string.Empty;
        string strNATURE = string.Empty;
        string strDESIGNATION = string.Empty;
        string strINVESTIGATION = string.Empty;
        string strSTATUS = string.Empty;
        string strHOSTATUS = string.Empty;
        string strVIEW = string.Empty;
        string strCLOSURE = string.Empty;
        string strREMINDERS = string.Empty;
        string strPENDINGWITH = string.Empty;
        string strZMVIEW = string.Empty;
        string strICVIEW = string.Empty;
        string strCLOSE = string.Empty;
        string strAPLAN = string.Empty;
        string strADDUSER = string.Empty;

        DateTime? dtSRDATE = null;
        DateTime? dtCLOSUREDT = null;
        DateTime? dtRECDATECOMP = null;
        DateTime? dtIACDT = null;
        DateTime? dtICDT = null;
        DateTime? dtCMD = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["USERNAME"] = Session["userid"].ToString();
                ViewState["USERROLE"] = Session["role"].ToString();

            }
            strUser = ViewState["USERNAME"].ToString();
            lblMsg.Text = string.Empty;
        }

        protected void btnAccessUpload_Click(object sender, EventArgs e)
        {
            string strDSN = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\\NEWAL.MDB";

            string strSQLCOMPLAINTS = "SELECT * FROM COMPLAINTS";
            string strSQLMISC = "SELECT * FROM MISC";
            string strSQLOPERATIONALREFERENCEETC = "SELECT * FROM OPERATIONALREF";
            string strSQLRRB = "SELECT * FROM RRB";
            string strSQLSR = "SELECT * FROM SR";
            string strSQLIAC = "SELECT * FROM IAC";
            string strSQLNOC = "SELECT * FROM NOC";
            string strSQLVIGILANCE = "SELECT * FROM VIGILANCE";
            string strSQLWB = "SELECT * FROM WB";

            OleDbConnection myConn = new OleDbConnection(strDSN);

            OleDbDataAdapter myCmdCOMPLAINTS = new OleDbDataAdapter(strSQLCOMPLAINTS, myConn);
            OleDbDataAdapter myCmdMISC = new OleDbDataAdapter(strSQLMISC, myConn);
            OleDbDataAdapter myCmdOPERATIONALREFERENCEETC = new OleDbDataAdapter(strSQLOPERATIONALREFERENCEETC, myConn);
            OleDbDataAdapter myCmdRRB = new OleDbDataAdapter(strSQLRRB, myConn);
            OleDbDataAdapter myCmdSR = new OleDbDataAdapter(strSQLSR, myConn);
            OleDbDataAdapter myCmdIAC = new OleDbDataAdapter(strSQLIAC, myConn);
            OleDbDataAdapter myCmdNOC = new OleDbDataAdapter(strSQLNOC, myConn);
            OleDbDataAdapter myCmdVIGILANCE = new OleDbDataAdapter(strSQLVIGILANCE, myConn);
            OleDbDataAdapter myCmdWB = new OleDbDataAdapter(strSQLWB, myConn);

            myConn.Open();

            myCmdCOMPLAINTS.Fill(dsMain, "COMPLAINTS");
            myCmdMISC.Fill(dsMain, "MISC");
            myCmdOPERATIONALREFERENCEETC.Fill(dsMain, "OPERATIONALREF");
            myCmdRRB.Fill(dsMain, "RRB");
            myCmdSR.Fill(dsMain, "SR");
            myCmdIAC.Fill(dsMain, "IAC");
            myCmdNOC.Fill(dsMain, "NOC");
            myCmdVIGILANCE.Fill(dsMain, "VIGILANCE");
            myCmdWB.Fill(dsMain, "WB");

            ViewState["COMPLAINTS"] = dsMain.Tables[0];
            ViewState["MISC"] = dsMain.Tables[1];
            ViewState["OPERATIONALREF"] = dsMain.Tables[2];
            ViewState["RRB"] = dsMain.Tables[3];
            ViewState["SR"] = dsMain.Tables[4];
            ViewState["IAC"] = dsMain.Tables[5];
            ViewState["NOC"] = dsMain.Tables[6];
            ViewState["VIGILANCE"] = dsMain.Tables[7];
            ViewState["WB"] = dsMain.Tables[8];

            if (dsMain.Tables[0].Rows.Count > 0)
            {
                btnCOMPLAINTS.Visible = true;
            }

            if (dsMain.Tables[1].Rows.Count > 0)
            {
                btnMISC.Visible = true;
            }

            if (dsMain.Tables[2].Rows.Count > 0)
            {
                btnOPERATIONALREFERENCE.Visible = true;
            }

            if (dsMain.Tables[3].Rows.Count > 0)
            {
                btnRRB.Visible = true;
            }

            if (dsMain.Tables[4].Rows.Count > 0)
            {
                btnSR.Visible = true;
            }

            foreach (DataRow dtRow in dtCOMPLAINTS.Rows)
            {
                lblMsg.Text = dtRow["COMPNO"].ToString();
                lblMsg.Text = dtRow["ZONE"].ToString();
            }
        }

        protected void btnCOMPLAINTS_Click(object sender, EventArgs e)
        {
        }

        protected void btnMISC_Click(object sender, EventArgs e)
        {
        }

        protected void btnOPERATIONALREFERENCE_Click(object sender, EventArgs e)
        {
        }

        protected void btnRRB_Click(object sender, EventArgs e)
        {
            dtRRB = ((DataTable)ViewState["RRB"]);
            DataTable distinctRRB = dtRRB.DefaultView.ToTable(true, "RNO");
            if (distinctRRB.Rows.Count == dtRRB.Rows.Count)
            {
                //there are no duplicates
            }
            else
            {
                lblMsg.Text = "Upload Failed ! Duplicate Record in RRB Table, please check";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        protected void btnSR_Click(object sender, EventArgs e)
        {
            dtSR = ((DataTable)ViewState["SR"]);
            DataTable distinct = dtSR.DefaultView.ToTable(true, "SR NO");
            if (distinct.Rows.Count == dtSR.Rows.Count)
            {
                funcSave_SR(dtSR);
            }
            else
            {
                lblMsg.Text = "Upload Failed ! Duplicate Record in SR Table, please check";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        protected void funcSave_SR(DataTable dtSR)
        {
            foreach (DataRow row in dtSR.Rows)
            {
                try
                {
                    strSRNO = row["SR NO"].ToString();
                }
                catch (Exception)
                {
                    lblMsg.Text = "Upload Failed ! Please check your Access File.....!";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (strSRNO != "")
                {
                    strACCUSED = row["ACCUSED"].ToString();
                    strDESIGNATION = row["DESIGNATION"].ToString();
                    strPRESENTPOSTING = row["PRESENT POSTING"].ToString();
                    strBRCOMPLAINT = row["BRANCH"].ToString();
                    strZONE = row["ZONE"].ToString();
                    strCIRCLEOFFICE = row["CIRCLE OFFICE"].ToString();
                    strREGION = row["REGION"].ToString();
                    strINVESTIGATION = row["INVESTIGATION"].ToString();
                    strNATURE = row["NATURE SR"].ToString();
                    strALLEGATIONS = row["ALLEGATIONS"].ToString();
                    strREMINDERS = row["REMINDERS"].ToString();
                    strSTATUS = row["STATUS"].ToString();
                    strPENDINGWITH = row["PENDING WITH"].ToString();
                    strACCOUNTNAME = row["ACCOUNT"].ToString();
                    strZMVIEW = row["ZM VIEW"].ToString();
                    strICVIEW = row["IC VIEW"].ToString();
                    strFINALACTION = row["FINAL ACTION"].ToString();
                    strCLOSE = row["CLOSE"].ToString();
                    strRNO = row["RNO"].ToString();
                    strAPLAN = row["A PLAN"].ToString();
                    strSTATUSCODE = row["STATUS CODE"].ToString();
                    strADDUSER = ViewState["USERNAME"].ToString();

                    TextBox txtAmount = new TextBox();
                    txtAmount.Text = row["AMOUNT"].ToString();
                    decAMOUNT = objCommonFunction.convertToDecimal(txtAmount);

                    #region ** convert Date **
                    string strSRDATE = row["DATE SR"].ToString();
                    if (!string.IsNullOrEmpty(strSRDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strSRDATE, out date))
                            dtSRDATE = date;
                    }

                    string strCLOSUREDT = row["CLOSURE DT"].ToString();
                    if (!string.IsNullOrEmpty(strCLOSUREDT))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strCLOSUREDT, out date))
                            dtCLOSUREDT = date;
                    }

                    string strRECDATECOMP = row["REC DT SR"].ToString();
                    if (!string.IsNullOrEmpty(strRECDATECOMP))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strRECDATECOMP, out date))
                            dtRECDATECOMP = date;
                    }

                    string strIACDATE = row["DT-IAC"].ToString();
                    if (!string.IsNullOrEmpty(strIACDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strIACDATE, out date))
                            dtIACDT = date;
                    }

                    string strICDATE = row["IC DT"].ToString();
                    if (!string.IsNullOrEmpty(strICDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strICDATE, out date))
                            dtICDT = date;
                    }

                    string strCMDDATE = row["CMD"].ToString();
                    if (!string.IsNullOrEmpty(strCMDDATE))
                    {
                        DateTime date;
                        if (DateTime.TryParse(strCMDDATE, out date))
                            dtCMD = date;
                    }
                    #endregion
                }

                try
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spACCESSSR_Import]";

                    SqlParameter sqlErrMsgOutput = new SqlParameter("@o_EERMSG", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter sqlErrCodeOutput = new SqlParameter("@o_ERRCODE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(sqlErrMsgOutput);
                    cmd.Parameters.Add(sqlErrCodeOutput);

                    cmd.Parameters.AddWithValue("@p_SRNO", strSRNO);
                    cmd.Parameters.AddWithValue("@p_ACCUSED", strACCUSED);
                    cmd.Parameters.AddWithValue("@p_DESIGNATION", strDESIGNATION);
                    cmd.Parameters.AddWithValue("@p_PRESENTPOSTING", strDESIGNATION);
                    cmd.Parameters.AddWithValue("@p_BRCOMPLAINT", strDESIGNATION);
                    cmd.Parameters.AddWithValue("@p_ZONE", strZONE);
                    cmd.Parameters.AddWithValue("@p_CIRCLEOFFICE", strZONE);
                    cmd.Parameters.AddWithValue("@p_REGION", strZONE);
                    cmd.Parameters.AddWithValue("@p_INVESTIGATION", strINVESTIGATION);
                    cmd.Parameters.AddWithValue("@p_NATURE", strNATURE);
                    cmd.Parameters.AddWithValue("@p_ALLEGATIONS", strALLEGATIONS);
                    cmd.Parameters.AddWithValue("@p_REMINDERS", strREMINDERS);
                    cmd.Parameters.AddWithValue("@p_STATUS", strSTATUS);
                    cmd.Parameters.AddWithValue("@p_PENDINGWITH", strPENDINGWITH);
                    cmd.Parameters.AddWithValue("@p_ACCOUNTNAME", strACCOUNTNAME);
                    cmd.Parameters.AddWithValue("@p_ZMVIEW", strZMVIEW);
                    cmd.Parameters.AddWithValue("@p_ICVIEW", strICVIEW);
                    cmd.Parameters.AddWithValue("@p_FINALACTION", strFINALACTION);
                    cmd.Parameters.AddWithValue("@p_CLOSE", strCLOSE);
                    cmd.Parameters.AddWithValue("@p_RNO", strRNO);
                    cmd.Parameters.AddWithValue("@p_APLAN", strAPLAN);
                    cmd.Parameters.AddWithValue("@p_STATUSCODE", strSTATUSCODE);

                    cmd.Parameters.AddWithValue("@p_AMOUNT", decAMOUNT);

                    cmd.Parameters.AddWithValue("@p_SRDATE", dtSRDATE);
                    cmd.Parameters.AddWithValue("@p_CLOSUREDT", dtCLOSUREDT);
                    cmd.Parameters.AddWithValue("@p_RECDATECOMP", dtRECDATECOMP);
                    cmd.Parameters.AddWithValue("@p_IACDT", dtIACDT);
                    cmd.Parameters.AddWithValue("@p_ICDT", dtICDT);
                    cmd.Parameters.AddWithValue("@p_CMD", dtCMD);

                    cmd.Parameters.AddWithValue("@p_USER", strADDUSER);

                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 0;

                    strErrMsg = sqlErrMsgOutput.Value.ToString();
                    intErrCode = Convert.ToInt32(sqlErrCodeOutput.Value);

                    if (intErrCode == -1)
                    {
                        strScript.Append("<script language=JavaScript>");
                        strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
                        lblMsg.Text = strErrMsg.ToString();
                        return;
                    }

                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            strScript.Append("<script language=JavaScript>");
            strScript.Append("document.body.onload=function(){alert('" + strErrMsg + "')}</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Pop", strScript.ToString());
            lblMsg.Text = strErrMsg.ToString();
        }

        public static bool IsDateTime(string txtDate)
        {
            DateTime tempDate;
            return DateTime.TryParse(txtDate, out tempDate) ? true : false;
        }
    }
}