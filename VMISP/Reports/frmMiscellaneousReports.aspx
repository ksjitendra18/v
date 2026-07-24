<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmMiscellaneousReports.aspx.cs" Inherits="VMISP.Reports.frmMiscellaneousReports" %>

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
    <asp:Panel ID="pnlMain" runat="server" Width="100%" Height="450px">
        <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F; color: White; font-weight: 700;">
            Miscellaneous Reports
        </div>
        <div id="divMain">
            <div style="border: 1px solid #000000; width: 16%; float: left; height: 420px; margin-top: 3px; text-align: left; padding-left: 5px; background-color: #C0C0C0;">
                <asp:LinkButton ID="lnkDFSReports" runat="server" OnClick="lnkDFSReports_Click" CssClass="lblCaption">DFS Summary Report</asp:LinkButton><br />
                <asp:LinkButton ID="lnkDFSDetails" runat="server" OnClick="lnkDFSDetailsReports_Click" CssClass="lblCaption">DFS Details Report</asp:LinkButton><br />
                <asp:LinkButton ID="lnkProgressOfRRBReports" runat="server" OnClick="lnkProgressOfRRBReports_Click"
                    CssClass="lblCaption">PROGRESS OF RRB</asp:LinkButton><br />
                <asp:LinkButton ID="lnkNatureProcedings" runat="server" OnClick="lnkNatureProcedings_Click"
                    CssClass="lblCaption">NATURE PROCEDINGS</asp:LinkButton><br />
                <asp:LinkButton ID="lnkDepartmentalEnquiries" runat="server" OnClick="lnkDepartmentalEnquiries_Click"
                    CssClass="lblCaption">DEPARTMENTAL ENQUIRIES</asp:LinkButton><br />
                <asp:LinkButton ID="lnkInvestigation" runat="server" OnClick="lnklnkInvestigation_Click"
                    CssClass="lblCaption">INVESTIGATION REPORTS</asp:LinkButton><br />
                <asp:LinkButton ID="lnkComplaints" runat="server" OnClick="lnkComplaints_Click" CssClass="lblCaption">COMPLAINTS REPORTS</asp:LinkButton><br />
            </div>
            <div style="border: 1px solid #000000; width: 83%; float: right; margin-top: 3px;">
                <asp:Panel ID="pnlReport" runat="server" Width="100%" ScrollBars="Both" Height="420px">
                    <rsweb:ReportViewer ID="rvMain" EnableTheming="False" ShowPrintButton="true" runat="server"
                        Font-Names="Verdana" ProcessingMode="Remote" ShowBackButton="True" ShowCredentialPrompts="False"
                        DocumentMapCollapsed="True" ShowDocumentMapButton="False" AsyncRendering="true"
                        SizeToReportContent="true" Width="98%" Height="400px" LinkActiveColor="Red" BorderWidth="0px"
                        BackColor="#3366CC">
                    </rsweb:ReportViewer>
                </asp:Panel>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
