<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmAccessUpload.aspx.cs" Inherits="VMISP.Upload.frmAccessUpload" %>

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
        Access Upload
    </div>
    <asp:Panel ID="pnlMain" runat="server" Width="98%">
        <fieldset style="height: 40px; width: 97%; margin-top: 5px;">
            <table width="98%" style="margin-top: -10px">
                <tr>
                    <td>
                        <span class="lblCaptionHead">Your Access File Path: - "Source=c:\\NEWAL.MDB"</span>
                    </td>
                    <td>
                        <span class="lblCaption">Please Upload One File at the one Time</span>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnAccessUpload" runat="server" OnClick="btnAccessUpload_Click" Text="Access  File Upload"
                            CssClass="btnSearch" />
                    </td>
                    <td>
                        <asp:Button ID="btnCOMPLAINTS" runat="server" OnClick="btnCOMPLAINTS_Click" Text="Complaints"
                            CssClass="btnSearch" ToolTip="Upload Complaints Access File" Visible="false" />
                    </td>
                    <td>
                        <asp:Button ID="btnMISC" runat="server" OnClick="btnMISC_Click" Text="MISC" CssClass="btnSearch"
                            ToolTip="Upload MISC Access File" Visible="false" />
                    </td>
                    <td>
                        <asp:Button ID="btnOPERATIONALREFERENCE" runat="server" OnClick="btnOPERATIONALREFERENCE_Click"
                            Text="Operation Reference" CssClass="btnSearch" ToolTip="Upload Operation Reference Access File"
                            Visible="false" />
                    </td>
                    <td>
                        <asp:Button ID="btnRRB" runat="server" OnClick="btnRRB_Click" Text="RRB" CssClass="btnSearch"
                            ToolTip="Upload RRB Access File" Visible="false" />
                    </td>
                    <td>
                        <asp:Button ID="btnSR" runat="server" OnClick="btnSR_Click" Text="SR" CssClass="btnSearch"
                            ToolTip="Upload SR Access File" Visible="false" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 50%">
                        <asp:Label ID="lblMsg" runat="server" CssClass="lblMsg"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
</asp:Content>
