<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmWBReports.aspx.cs" Inherits="VMISP.Reports.frmWBReports" %>

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
        <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F;
            color: White; font-weight: 700;">
            Whistle Blower (WB) Outstanding/Status Reports
        </div>
        <div id="divMain">
            <div style="border: 1px solid #000000; width: 12%; float: left; height: 400px; margin-top: 3px;
                text-align: left; background-color: #C0C0C0;">
                <span class="lblCaption">1) </span><asp:LinkButton ID="lnkWBOUTSTANDING" runat="server" OnClick="lnkWBOUTSTANDING_Click"
                    CssClass="lblCaption">WB Outstanding</asp:LinkButton><br />
                <span class="lblCaption">2) </span><asp:LinkButton ID="lnkWBSTATUS" runat="server" OnClick="lnkWBSTATUS_Click" CssClass="lblCaption">WB Status</asp:LinkButton>
                <br />
                <span class="lblCaption" style="display: none;">3) </span><asp:LinkButton ID="lnkWBREPORTTOMD" runat="server" OnClick="lnkWBREPORTTOMD_Click" CssClass="lblCaption" Visible="false">WB Report To MD</asp:LinkButton>
            </div>
            <div style="border: 1px solid #000000; width: 87%; float: right; margin-top: 3px;">
                <asp:Panel ID="pnlReport" runat="server" Width="100%" ScrollBars="Both" Height="395px">
                    <rsweb:ReportViewer ID="rvMain" EnableTheming="False" ShowPrintButton="true" runat="server"
                        Font-Names="Verdana" ProcessingMode="Remote" Width="99%" ShowBackButton="True"
                        ShowCredentialPrompts="False" DocumentMapCollapsed="True" LinkActiveHoverColor="Wheat"
                        ShowDocumentMapButton="False" EnableViewState="True" LinkActiveColor="Red" BorderWidth="0px"
                        BackColor="#3366CC">
                    </rsweb:ReportViewer>
                </asp:Panel>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
