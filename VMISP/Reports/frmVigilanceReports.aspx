<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="frmVigilanceReports.aspx.cs" Inherits="VMISP.Reports.frmVigilanceReports" %>

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
            Vigilance Reports
        </div>
        <div id="divMain">
            <div style="border: 1px solid #000000; width: 22%; float: left; height: 400px; margin-top: 3px; text-align: left; background-color: #C0C0C0;">
                <span class="lblCaption">1) </span>
                <asp:LinkButton ID="lnkVIGILANCEOUTSTANDING" runat="server" OnClick="lnkVIGILANCEOUTSTANDING_Click"
                    CssClass="lblCaption">Vigilance Outstanding</asp:LinkButton>
                <br />
                <span class="lblCaption">2) </span>
                <asp:LinkButton ID="lnkVIGILANCESTATUS" runat="server" OnClick="lnkVIGILANCESTATUS_Click"
                    CssClass="lblCaption">Vigilance Status</asp:LinkButton>
                <br />
                <span class="lblCaption">3) </span>
                <asp:LinkButton ID="lnkVigilanceRetirement" runat="server" OnClick="lnkVigilanceRetirement_Click"
                    CssClass="lblCaption">Vigilance Retirement</asp:LinkButton>
                <br />
                <span class="lblCaption">4) </span>
                <a href="~/Reports/VigilanceMonitoringReports.aspx" id="VigilanceMonitoringReports" runat="server"><b>Vigilance Monitoring</b></a>
                <br />
                <span class="lblCaption" style="display: none;">3) </span>
                <asp:LinkButton ID="lnkFIRSTSTAGEPENDING" runat="server" OnClick="lnkFIRSTSTAGEPENDING_Click"
                    CssClass="lblCaption" Visible="false">First Stage Pending</asp:LinkButton>
                <br />
                <span class="lblCaption" style="display: none;">4) </span>
                <asp:LinkButton ID="lnkFIRSTSTAGEPENDINGATDESK" runat="server" OnClick="lnkFIRSTSTAGEPENDINGATDESK_Click"
                    CssClass="lblCaption" Visible="false">First Stage Pending at Desk</asp:LinkButton>
                <br />
                <span class="lblCaption" style="display: none;">5) </span>
                <asp:LinkButton ID="lnkSECONDSTAGEPENDING" runat="server" OnClick="lnkSECONDSTAGEPENDING_Click"
                    CssClass="lblCaption" Visible="false">Second Stage Pending with Desk</asp:LinkButton>
                <br />
                <span class="lblCaption" style="display: none;">6) </span>
                <asp:LinkButton ID="lnkSECONDSTAGEPENDINGATDA" runat="server" OnClick="lnkSECONDSTAGEPENDINGATDA_Click"
                    CssClass="lblCaption" Visible="false">Second Stage Pending at DA</asp:LinkButton>
                <br />
                <span class="lblCaption" style="display: none;">7) </span>
                <asp:LinkButton ID="lnkCHARGESHEETNOTSERVED" runat="server" OnClick="lnkCHARGESHEETNOTSERVED_Click"
                    CssClass="lblCaption" Visible="false">Charge Sheet not Served</asp:LinkButton>
                <br />
                <span class="lblCaption" style="display: none;">8) </span>
                <asp:LinkButton ID="lnkEOPONOTAPPOINTED" runat="server" OnClick="lnkEOPONOTAPPOINTED_Click"
                    CssClass="lblCaption" Visible="false">EO/PO not Appointed</asp:LinkButton>
                <br />
                <span class="lblCaption" style="display: none;">9) </span>
                <asp:LinkButton ID="lnkRECONSIDERVIEWAWIATEDFROMDA" runat="server" OnClick="lnkRECONSIDERVIEWAWIATEDFROMDA_Click"
                    CssClass="lblCaption" Visible="false">Reconsider view Awaited from DA</asp:LinkButton>
                <br />
                <span class="lblCaption" style="display: none;">10) </span>
                <asp:LinkButton ID="lnkENQUIRYISINPROGRESS" runat="server" OnClick="lnkENQUIRYISINPROGRESS_Click"
                    CssClass="lblCaption" Visible="false">Enquiry Is In Progress</asp:LinkButton>
                <br />
                <span class="lblCaption" style="display: none;">12) </span>
                <asp:LinkButton ID="lnkFinalOrderAwaited" runat="server" OnClick="lnkFinalOrderAwaited_Click"
                    CssClass="lblCaption" Visible="false">Final Order Awaited</asp:LinkButton>
                <br />
                <span class="lblCaption" style="display: none;">13) </span>
                <asp:LinkButton ID="lnkMinorChargeSheet" runat="server" OnClick="lnkMinorChargeSheet_Click"
                    CssClass="lblCaption" Visible="false">Minor Charge Sheet</asp:LinkButton>
                <br />
            </div>
            <div style="border: 1px solid #000000; width: 77%; float: right; margin-top: 3px;">
                <asp:Panel ID="pnlReport" runat="server" Width="100%" ScrollBars="Both" Height="420px">
                    <rsweb:ReportViewer ID="rvMain" EnableTheming="False" ShowPrintButton="true" runat="server"
                        Font-Names="Verdana" ProcessingMode="Remote" ShowBackButton="True"
                        ShowCredentialPrompts="False" DocumentMapCollapsed="True" LinkActiveHoverColor="Wheat"
                        ShowDocumentMapButton="False" EnableViewState="True" LinkActiveColor="Red" BorderWidth="0px"
                        BackColor="#3366CC" Width="99%" Height="400px">
                    </rsweb:ReportViewer>
                </asp:Panel>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
