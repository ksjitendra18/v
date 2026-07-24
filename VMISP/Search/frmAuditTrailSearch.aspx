<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmAuditTrailSearch.aspx.cs" Inherits="VMISP.Search.frmAuditTrailSearch" %>

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
        Audit Trail Search
    </div>
    <asp:Panel ID="pnlMain" runat="server" Width="98%">
        <fieldset style="height: 15px; width: 97%; margin-top: 5px;">
            <table width="98%" style="margin-top: -10px">
                <tr>
                    <td class="tdTextReight">
                        <span class="lblCaption">Form :</span>
                    </td>
                    <td class="tdTextLeft">
                        <asp:DropDownList ID="ddlTableName" runat="server" CssClass="ddlDefault1" Width="135px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlTableName_SelectedIndexChanged">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Complaint" Value="COMPLAINT_HISTORY"></asp:ListItem>
                            <asp:ListItem Text="IAC" Value="IAC_HISTORY"></asp:ListItem>
                            <asp:ListItem Text="MISC" Value="MISC_HISTORY"></asp:ListItem>
                            <asp:ListItem Text="NOC" Value="NOC_HISTORY"></asp:ListItem>
                            <asp:ListItem Text="Operational Ref" Value="OPERATIONALREF_HISTORY"></asp:ListItem>
                            <asp:ListItem Text="RRB" Value="RRB_HISTORY"></asp:ListItem>
                            <asp:ListItem Text="RTI" Value="RTI_HISTORY"></asp:ListItem>
                            <asp:ListItem Text="SR" Value="SR_HISTORY"></asp:ListItem>
                            <asp:ListItem Text="Vigilance" Value="VIGILANCE_HISTORY"></asp:ListItem>
                            <asp:ListItem Text="Whistle Blower" Value="WB_HISTORY"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="tdTextReight">
                        <span class="lblCaption">Show all Data :</span>
                    </td>
                    <td class="tdTextLeft">
                        <asp:CheckBox ID="chkShowAllData" runat="server" />
                    </td>
                    <td class="tdTextReight">
                        <span class="lblCaption">Fields :</span>
                    </td>
                    <td class="tdTextLeft">
                        <asp:DropDownList ID="ddlColumnName" runat="server" CssClass="ddlDefault" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td class="tdTextReight">
                        <asp:Label ID="lblValueCaption" runat="server"></asp:Label>
                    </td>
                    <td id="tdText" runat="server" style="display: none">
                        <asp:Panel ID="pnlText" runat="server">
                            <asp:TextBox ID="txtEnterValue" runat="server" CssClass="txtDefault"></asp:TextBox>
                        </asp:Panel>
                    </td>
                    <td id="tdDate" runat="server" style="display: none">
                        <asp:Panel ID="pnlDate" runat="server">
                            <asp:TextBox ID="txtEnterDate" runat="server" CssClass="txtDate"></asp:TextBox>
                            <act:MaskedEditExtender ID="meeEnterDate" runat="server" TargetControlID="txtEnterDate"
                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                            </act:MaskedEditExtender>
                            <act:CalendarExtender ID="ceEnterDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                TargetControlID="txtEnterDate" PopupButtonID="imgEnterDate" CssClass="cal_Theme1">
                            </act:CalendarExtender>
                            <asp:Image ID="imgEnterDate" runat="server" AlternateText="Please Select date!!"
                                ImageUrl="~/images/calendar.png" />
                        </asp:Panel>
                    </td>
                    <td class="tdTextLeft">
                        <asp:Button ID="btnGet" runat="server" OnClick="btnGet_Click" Text="Search" CssClass="btnSearch" />&nbsp
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExel_Click" Text="Excel" CssClass="btnSearch"
                            Visible="false" />&nbsp
                        <asp:Button ID="btnPDF" runat="server" OnClick="btnPdf_Click" Text="PDF" CssClass="btnSearch"
                            Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMsg" runat="server" CssClass="lblMsg"></asp:Label>
                        <asp:HiddenField ID="hidColumnDataType" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset style="height: 350px; width: 97%;">
            <asp:Panel ID="pnlList" runat="server" ScrollBars="Both" Height="355px" Width="100%">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                Style="margin-top: 0px" BackColor="White" BorderColor="White" BorderWidth="2px"
                                CellSpacing="1" Width="100%" AllowPaging="True" OnPageIndexChanging="gvMain_PageIndexChanging"
                                EmptyDataText="No Record Found..!" ShowHeaderWhenEmpty="True" PageSize="20" OnRowDataBound="gvMain_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ROWNO" HeaderText="S No." HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="RNO" HeaderText="Number" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="NAME" HeaderText="Name" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="COMPRECDATE" HeaderText="Date" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="BRANCH" HeaderText="Branch" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="ADDUSER" HeaderText="Entry User" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="ADDDATE" HeaderText="Entry Date" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="MODUSER" HeaderText="Modify User" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="MODDATE" HeaderText="Modify Date" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                </Columns>
                                <PagerSettings PageButtonCount="20" />
                                <PagerStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </fieldset>
    </asp:Panel>
</asp:Content>
