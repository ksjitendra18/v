using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;


namespace VMISP.VMISP_COMM_ERROR_TRACK
{

    public class VMISP_Error_Log
    {
        public static void HandleException(Exception ex)
        {
            HttpContext ctxObject = HttpContext.Current;
            string strLogConnString = WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString.ToString();
            string logDateTime = DateTime.Now.ToString("g");
            string strReqURL = (ctxObject.Request.Url != null) ? ctxObject.Request.Url.ToString() : String.Empty;
            string strReqQS = (ctxObject.Request.QueryString != null) ? ctxObject.Request.QueryString.ToString() : String.Empty;
            string strServerName = String.Empty;
            if (ctxObject.Request.ServerVariables["HTTP_REFERER"] != null)
            {
                strServerName = ctxObject.Request.ServerVariables["HTTP_REFERER"].ToString();
            }
            string strUserAgent = (ctxObject.Request.UserAgent != null) ? ctxObject.Request.UserAgent : String.Empty;
            string strUserIP = (ctxObject.Request.UserHostAddress != null) ? ctxObject.Request.UserHostAddress : String.Empty;
            string strUserAuthen = (ctxObject.User.Identity.IsAuthenticated.ToString() != null) ? ctxObject.User.Identity.IsAuthenticated.ToString() : String.Empty;
            string strUserName = (ctxObject.User.Identity.Name != null) ? ctxObject.User.Identity.Name : String.Empty;
            string strMessage = string.Empty, strSource = string.Empty, strTargetSite = string.Empty, strStackTrace = string.Empty;
            while (ex != null)
            {
                strMessage = ex.Message;
                strSource = ex.Source;
                strTargetSite = (ex.TargetSite == null) ? null : ex.TargetSite.ToString();
                strStackTrace = ex.StackTrace;
                ex = ex.InnerException;
            }
            if (strLogConnString.Length > 0)
            {
                SqlConnection sqlConn = new SqlConnection(strLogConnString);
                SqlCommand strSqlCmd = new SqlCommand();
                strSqlCmd.Parameters.Clear();
                strSqlCmd.CommandType = CommandType.StoredProcedure;
                strSqlCmd.CommandText = "[dbo].[spErrorLog_Update]";
                strSqlCmd.Connection = sqlConn;
                sqlConn.Open();
                try
                {
                    strSqlCmd.Parameters.AddWithValue("@p_SOURCE", strSource);
                    strSqlCmd.Parameters.AddWithValue("@p_LOGDATETIME", logDateTime);
                    strSqlCmd.Parameters.AddWithValue("@p_MESSAGE", strMessage);
                    strSqlCmd.Parameters.AddWithValue("@p_QUERYSTRING", strReqQS);
                    strSqlCmd.Parameters.AddWithValue("@p_TARGETSITE", strTargetSite);
                    strSqlCmd.Parameters.AddWithValue("@p_STACKTRACE", strStackTrace);
                    strSqlCmd.Parameters.AddWithValue("@p_SERVERNAME", strServerName);
                    strSqlCmd.Parameters.AddWithValue("@p_REQUESTURL", strReqURL);
                    strSqlCmd.Parameters.AddWithValue("@p_USERAGENT", strUserAgent);
                    strSqlCmd.Parameters.AddWithValue("@p_USERIP", strUserIP);
                    strSqlCmd.Parameters.AddWithValue("@p_USERAUTHENTICATION", strUserAuthen);
                    strSqlCmd.Parameters.AddWithValue("@p_USERNAME", strUserName);

                    //if (strMessage != "Thread was being aborted.")
                    //{
                    //    strSqlCmd.ExecuteNonQuery();
                    //}
                }
                catch (Exception exc)
                {
                    throw exc;
                }
                finally
                {
                    strSqlCmd.Dispose();
                    sqlConn.Close();
                }
            }
        }
    }
}