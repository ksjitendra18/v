<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmComplaintRpt.aspx.cs" Inherits="VMISP.Reports.frmComplaintRpt" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/ssMain.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="../Js/JS_CommonValidation.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F;
        color: White; font-weight: 700;">
        Complaint Entry Report
    </div>
    <asp:Panel ID="pnlReport" runat="server" Width="100%" Style="margin-top: 10px;">
        <div style="width: 95%; border: 1px:solid; height: 20px; text-align: center; color: White;
            font-weight: 700; border-bottom: 1px;">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkComplaintsReports" runat="server" OnClick="lnkComplaintsReports_Click"
                            Visible="">MONTHLY REPORT OF THE CVO</asp:LinkButton>&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkPenaltyProcedings" runat="server" OnClick="lnkPenaltyProcedings_Click">PENALTY PROCEDINGS</asp:LinkButton>&nbsp;&nbsp;
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                </tr>
            </table>
        </div>
        <table width="100%">
            <tr>
                <td>
                    <rsweb:ReportViewer ID="rvMain" EnableTheming="False" ShowPrintButton="true" runat="server"
                        Font-Names="Verdana" ProcessingMode="Remote" BorderColor="#90C0EA" BorderStyle="Solid"
                        Width="600pt" ShowBackButton="True" ShowCredentialPrompts="False" DocumentMapCollapsed="True"
                        LinkActiveHoverColor="Wheat" ShowDocumentMapButton="False" EnableViewState="True"
                        LinkActiveColor="Red" BorderWidth="0px" BackColor="#3366CC">
                    </rsweb:ReportViewer>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
