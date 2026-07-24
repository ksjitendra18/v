<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmCaseRegister.aspx.cs" Inherits="VMISP.Reports.frmCaseRegister" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
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
    <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F; color: White; font-weight: 700;">
        Case Register Report
    </div>
    <asp:Panel ID="pnlMain" runat="server" Width="100%">
        <div>
            <table style="width: 100%; border: 1px:solid;">
                <tr>

                    <td class="tdTextReight" style="width: 7%">
                        <span class="lblCaption">From Date :</span>
                    </td>
                    <td style="width: 8%">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtDate"></asp:TextBox>
                        <act:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True">
                        </act:MaskedEditExtender>
                        <act:CalendarExtender ID="ceFromDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                            TargetControlID="txtFromDate" PopupButtonID="imgFromDate" CssClass="cal_Theme1">
                        </act:CalendarExtender>
                        <asp:ImageButton ID="imgFromDate" runat="server" AlternateText="Please Select date!!"
                            ImageUrl="~/images/calendar.png" />
                    </td>
                    <td class="tdTextReight" style="width: 5%">
                        <span class="lblCaption">To Date :</span>
                    </td>
                    <td style="width: 9%">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="txtDate"></asp:TextBox>
                        <act:MaskedEditExtender ID="meToDate" runat="server" TargetControlID="txtToDate"
                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True">
                        </act:MaskedEditExtender>
                        <act:CalendarExtender ID="ceToDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                            TargetControlID="txtToDate" PopupButtonID="imgToDate" CssClass="cal_Theme1">
                        </act:CalendarExtender>
                        <asp:ImageButton ID="imgToDate" runat="server" AlternateText="Please Select date!!"
                            ImageUrl="~/images/calendar.png" />
                    </td>
                    <td style="width: 26%">
                        <asp:Button ID="btnComplaint" runat="server" Text="Complaint" CssClass="btnDefault" OnClick="btnComplaintSubmit_Click" />&nbsp;&nbsp;<asp:Button ID="btnIAC" runat="server" Text="IAC" Visible="true" CssClass="btnDefault" OnClick="btnIACSubmit_Click" />&nbsp;&nbsp;<asp:Button ID="btnVigilance" runat="server" Text="Vigilance" Visible="true" CssClass="btnDefault" OnClick="btnVigilanceSubmit_Click" />&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnDefault" OnClick="btnCancel_Click" />&nbsp;&nbsp;
                    </td>
                    <td style="width: 5%; border-right-style: solid; border-right-width: 1px;">
                        <asp:LinkButton ID="lnkDOWNLOAD_COMPLAINT" runat="server" Text="Complaint" OnClick="lnkDOWNLOAD_COMPLAINT_Click" Visible="false" CssClass="lblCaptionHead" ToolTip="Download Excel of Complaint Details"></asp:LinkButton></td>
                    <td style="width: 3%; border-right-style: solid; border-right-width: 1px;">
                        <asp:LinkButton ID="lnkDOWNLOAD_IAC" runat="server" Text="IAC" OnClick="lnkDOWNLOAD_IAC_Click" Visible="false" CssClass="lblCaptionHead" ToolTip="Download Excel of IAC Details"></asp:LinkButton></td>
                    <td style="width: 5%; border-right-style: solid; border-right-width: 1px;">
                        <asp:LinkButton ID="lnkDOWNLOAD_VIGILANCE" runat="server" Text="Vigilance" OnClick="lnkDOWNLOAD_VIGILANCE_Click" Visible="false" CssClass="lblCaptionHead" ToolTip="Download Excel of Vigilance Details"></asp:LinkButton></td>
                    <td>
                        <asp:Label ID="lblMsg" runat="server" CssClass="lblMsg"></asp:Label>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td>
                        <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="100%" Height="350px">
                            <act:TabPanel ID="tabComplaint" runat="server" TabIndex="0">
                                <HeaderTemplate>
                                    <asp:Label ID="lblComplaintHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="Complaint" ToolTip="Complaint Data"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 100%">
                                                <asp:GridView ID="gvComplaint" runat="server" AutoGenerateColumns="False" DataKeyNames="RNO"
                                                    CellPadding="3" GridLines="None" ViewStateMode="Enabled" Style="margin-top: 0px"
                                                    BackColor="White" BorderColor="White" BorderWidth="2px" CellSpacing="1" Width="100%"
                                                    AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvComplaint_PageIndexChanging"
                                                    OnSorting="gvComplaint_Sorting" OnRowDataBound="gvComplaint_RowDataBound" PagerSettings-PageButtonCount="15">
                                                    <Columns>
                                                        <asp:BoundField DataField="ROWNO" HeaderText="S No" SortExpression="ROWNO" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="RNO" HeaderText="Complaint No" SortExpression="RNO" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="CASENO" HeaderText="IAC No" SortExpression="CASENO" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="RECDATECOMP" HeaderText="Complaint Date" SortExpression="RECDATECOMP" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="NAME" HeaderText="Name" SortExpression="NAME"
                                                            HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" SortExpression="STATUSCODE" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="STATUSNAME" HeaderText="Status Name" SortExpression="STATUSNAME" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="REGISTER" HeaderText="Register" SortExpression="REGISTER" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" SortExpression="CIRCLEOFFICE" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="BRCOMPLAINT" HeaderText="Branch" SortExpression="BRCOMPLAINT" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="STATUS" HeaderText="Status" SortExpression="STATUS" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </act:TabPanel>
                            <act:TabPanel ID="tabIAC" runat="server" TabIndex="1">
                                <HeaderTemplate>
                                    <asp:Label ID="lblIACHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="IAC" ToolTip="IAC Data"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 100%">
                                                <asp:GridView ID="gvIAC" runat="server" AutoGenerateColumns="False" DataKeyNames="RNO"
                                                    CellPadding="3" GridLines="None" ViewStateMode="Enabled" Style="margin-top: 0px"
                                                    BackColor="White" BorderColor="White" BorderWidth="2px" CellSpacing="1" Width="100%"
                                                    AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvIAC_PageIndexChanging"
                                                    OnSorting="gvIAC_Sorting" OnRowDataBound="gvComplaint_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="ROWNO" HeaderText="S No" SortExpression="ROWNO" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="COMPLAINTNO" HeaderText="Complaint No" SortExpression="COMPLAINTNO" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="RNO" HeaderText="IAC No" SortExpression="RNO" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="CASENO" HeaderText="Vigilance No" SortExpression="CASENO" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="DTIAC" HeaderText="IAC Date" SortExpression="DTIAC" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="NAME" HeaderText="Name" SortExpression="NAME"
                                                            HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" SortExpression="STATUSCODE" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="STATUSNAME" HeaderText="Status Name" SortExpression="STATUSNAME" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="PFNUMBER" HeaderText="PF Number" SortExpression="PFNUMBER" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" SortExpression="CIRCLEOFFICE" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="BRANCH" HeaderText="Branch" SortExpression="BRANCH" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="STATUS" HeaderText="Status" SortExpression="STATUS" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </act:TabPanel>
                            <act:TabPanel ID="tabVigilance" runat="server" TabIndex="2">
                                <HeaderTemplate>
                                    <asp:Label ID="lblVigilanceHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="Vigilance" ToolTip="Vigilance Data"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 100%">
                                                <asp:GridView ID="gvVigilance" runat="server" AutoGenerateColumns="False" DataKeyNames="RNO"
                                                    CellPadding="3" GridLines="None" ViewStateMode="Enabled" Style="margin-top: 0px"
                                                    BackColor="White" BorderColor="White" BorderWidth="2px" CellSpacing="1" Width="100%"
                                                    AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvVigilance_PageIndexChanging"
                                                    OnSorting="gvVigilance_Sorting" OnRowDataBound="gvComplaint_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="ROWNO" HeaderText="S No" SortExpression="ROWNO" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="COMPLAINTNO" HeaderText="Complaint No" SortExpression="COMPLAINTNO" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="IACVIGNO" HeaderText="IAC VIG No" SortExpression="IACVIGNO" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="RNO" HeaderText="Vigilance No" SortExpression="RNO" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="VIGILANCEDATE" HeaderText="Vigilance Date" SortExpression="VIGILANCEDATE" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="NAME" HeaderText="Name" SortExpression="NAME"
                                                            HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" SortExpression="STATUSCODE" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="STATUSNAME" HeaderText="Status Name" SortExpression="STATUSNAME" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="PFNO" HeaderText="PF Number" SortExpression="PFNO" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" SortExpression="CIRCLEOFFICE" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="BRCOMPLAINT" HeaderText="Branch" SortExpression="BRCOMPLAINT" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="STATUS" HeaderText="Status" SortExpression="STATUS" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </act:TabPanel>
                        </act:TabContainer>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
