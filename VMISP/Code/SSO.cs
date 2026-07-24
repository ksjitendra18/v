using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace VMISP.SSO
{   
    public static class SSOLayer
    {
        public static string GETSSOData(string reqdata)
        {
            string result = "";
            string UserID = ConfigurationManager.AppSettings["CBSServiceUserID"];
            string Password = ConfigurationManager.AppSettings["CBSServicePassword"];
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings["SSO_TokenAPI_URL"]);
                    message.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(UserID + ":" + Password)));
                    message.Content = new StringContent(reqdata, System.Text.Encoding.UTF8, "application/json");
                    var response = client.SendAsync(message).Result;
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public static bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }

    public class SSOResponse
    {
        public string empName { get; set; }
        public string Host { get; set; }
        public string returnURL { get; set; }
        public string SolId { get; set; }
        public string Flag { get; set; }
        public string Username { get; set; }
    }

}