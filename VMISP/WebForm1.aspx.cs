using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using VIGILANCE;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Collections;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Security;
using System;

namespace VMISP
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
                       // Create a new XML document.
              XmlDocument xmlDoc = new XmlDocument();
              // Load an XML file into the XmlDocument object.
              xmlDoc.PreserveWhitespace = true;
              xmlDoc.Load("E:\\paymentfiles\\030DSCPAYREQ010820133.xml");
              string filename = "E:\\paymentfiles\\030DSCPAYREQ010820133.xml";
              // Verify the signature of the signed XML.
              Console.WriteLine("Verifying signature...");
              SignedXml signedXml = new SignedXml(xmlDoc);
              // Find the "Signature" node and create a new XmlNodeList object.
              XmlNodeList nodeList = xmlDoc.GetElementsByTagName("Signature");
              XmlElement ex = (XmlElement)nodeList[0];

              // Throw an exception if no signature was found.
              if (nodeList.Count <= 0)
              {
                  throw new CryptographicException("Verification failed: No Signature was found in the document.");
              }
              if (nodeList.Count > 2)
              {
                  throw new CryptographicException("Verification failed: More that one signature was found for the document.");
              }
              // Load the first <signature> node. 
              signedXml.LoadXml((XmlElement)nodeList[0]);
              X509Certificate2 certtoverify = new X509Certificate2();
              IEnumerator enumerator = signedXml.KeyInfo.GetEnumerator();
              while (enumerator.MoveNext())
              {
                  if (enumerator.Current is KeyInfoX509Data)
                  {
                      var current = (KeyInfoX509Data)enumerator.Current;
                      if (current.Certificates.Count != 0)
                      {
                          var certificate = (System.Security.Cryptography.X509Certificates.X509Certificate)current.Certificates[0];
                          certtoverify = new X509Certificate2(certificate);
                          string test1 = certtoverify.Issuer + certtoverify.IssuerName + certtoverify.Subject + certtoverify.SubjectName + certtoverify.Thumbprint;
                          Byte[] key = certtoverify.GetPublicKey();
                          string test = key.ToString();
                         // SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCE"].ConnectionString);
                         // cn.Open();
                         // SqlCommand cmd = new SqlCommand();
                         // cmd.Connection = cn;
                         // cmd.CommandText = "insert into [VigilanceDB].[dbo].[Enrolldata](EnrollmentFilename,[SubjectName],[Issuername] ,[serialname],[thumbprint]) values (@EnrollmentFilename,@SubjectName,@Issuername  ,@serialname ,@thumbprint)";
                         // cmd.Parameters.AddWithValue("EnrollmentFilename", filename);
                         //cmd.Parameters.AddWithValue("SubjectName", certtoverify.SubjectName.ToString());
                         // cmd.Parameters.AddWithValue("Issuername", certtoverify.IssuerName.ToString());
                         // cmd.Parameters.AddWithValue("serialname", certtoverify.SerialNumber.ToString());
                         // cmd.Parameters.AddWithValue("thumbprint", certtoverify.Thumbprint.ToString());
                         
                         // cmd.ExecuteNonQuery();
                          SqlConnection cn1 = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCE"].ConnectionString);
                          cn1.Open();
                          SqlCommand cmd1 = new SqlCommand();
                          cmd1.Connection = cn1;
                          cmd1.CommandText = "SELECT *  FROM [VigilanceDB].[dbo].[Enrolldata]";

                          SqlDataReader sdr = cmd1.ExecuteReader();
                          if (sdr.Read())
                          {
                              string SubjectName = sdr["SubjectName"].ToString();
                              string serialname = sdr["serialname"].ToString();
                              string thumbprint = sdr["thumbprint"].ToString();





                              if (SubjectName == certtoverify.SubjectName.ToString() && serialname == certtoverify.SerialNumber && thumbprint == certtoverify.Thumbprint)
                              {
                                  Label1.Text = "success";
                              }
                              else Label1.Text = "failure";
                          }
                          continue;
                      }
                  }
              }

              
              if (signedXml.CheckSignature(certtoverify, true))
              {

                  string test = certtoverify.Thumbprint;
              }
                   
              else
              {

              }
       

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}