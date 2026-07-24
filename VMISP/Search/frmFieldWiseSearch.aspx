<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmFieldWiseSearch.aspx.cs" Inherits="VMISP.Search.frmFieldWiseSearch" %>

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
        Table Wise Search
    </div>
    <asp:Panel ID="pnlMain" runat="server" Width="98%">
        <fieldset style="height: 40px; width: 97%; margin-top: 5px;">
            <table width="98%" style="margin-top: -10px">
                <tr>
                    <td class="tdTextReight" style="width: 100px">
                        <span class="lblCaption">Table Name :</span>
                    </td>
                    <td style="width: 21%">
                        <asp:DropDownList ID="ddlTableName" runat="server" CssClass="ddlDefault1" Width="175px">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Complaint Entry" Value="COMPLAINT"></asp:ListItem>
                            <asp:ListItem Text="IAC Entry" Value="IAC"></asp:ListItem>
                            <asp:ListItem Text="MISC Structure" Value="MISC"></asp:ListItem>
                            <asp:ListItem Text="Operational Ref" Value="OPERATIONALREF"></asp:ListItem>
                            <asp:ListItem Text="RRB" Value="RRB"></asp:ListItem>
                            <asp:ListItem Text="SR Structure" Value="SR"></asp:ListItem>
                            <asp:ListItem Text="Vigilance" Value="VIGILANCE"></asp:ListItem>
                            <asp:ListItem Text="Whistle Blower Structure" Value="WB"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:HiddenField ID="hidTableName" runat="server" />
                    </td>
                    <td class="tdTextReight" style="width: 47px">
                        <span class="lblCaption">Name :</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" CssClass="txtDefault" Width="170px"></asp:TextBox>
                    </td>
                    <td id="tdCaseCaption" runat="server" class="tdTextReight" style="display: none; width:205px;">
                        <span class="lblCaption">Case No. :</span> &nbsp;
                        <asp:TextBox ID="txtCaseNo" runat="server" CssClass="txtDefault"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExel_Click" Text="Excel" CssClass="btnSearch"
                            Visible="false" />
                        <asp:Button ID="btnPDF" runat="server" OnClick="btnPdf_Click" Text="PDF" CssClass="btnSearch"
                            Visible="false" />
                    </td>
                </tr>
            </table>
            <table width="98%">
                <tr>
                    <td class="tdTextReight" style="width: 100px">
                        <span class="lblCaption">Status :</span>
                    </td>
                    <td style="width: 400px">
                        <asp:TextBox ID="txtStatus" runat="server" CssClass="txtDefault" Width="400px"></asp:TextBox>
                    </td>
                    <td id="tdPFCaption" runat="server" class="tdTextReight" style="display: none; margin-left: -90px;">
                        <span class="lblCaption">PF No. :</span>&nbsp;
                        <asp:TextBox ID="txtPFNo" runat="server" CssClass="txtDefault"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnGet" runat="server" OnClick="btnGet_Click" Text="Search" CssClass="btnSearch" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMsg" runat="server" CssClass="lblMsg"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset style="height: 350px; width: 97%;">
            <table width="98%">
                <tr>
                    <td style="width: 98%">
                        <asp:Panel ID="pnlList" runat="server" ScrollBars="Both" Height="355px" Width="880px">
                            <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                GridLines="None" ViewStateMode="Enabled" Style="margin-top: 0px" BackColor="White"
                                BorderColor="White" BorderWidth="2px" CellSpacing="1" Width="98%" AllowPaging="True"
                                AllowSorting="True" OnPageIndexChanging="gvMain_PageIndexChanging" OnSorting="gvMain_Sorting"
                                EmptyDataText="No Record Found..!" ShowHeaderWhenEmpty="True" PageSize="20">
                                <Columns>
                                    <asp:BoundField DataField="ROWNO" HeaderText="S No." SortExpression="ROWNO" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="RNO" HeaderText="R No." SortExpression="RNO" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="COMPRECDATE" HeaderText="Comp Rec Date" SortExpression="COMPRECDATE"
                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="ZONE" HeaderText="Zone" SortExpression="ZONE" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" SortExpression="STATUSCODE"
                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" SortExpression="CIRCLEOFFICE"
                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="AMOUNT" HeaderText="Amount" SortExpression="AMOUNT" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                    <asp:BoundField DataField="STATUS" HeaderText="Status" SortExpression="STATUS" HeaderStyle-CssClass="gridText"
                                        ItemStyle-CssClass="gridText" />
                                </Columns>
                                <PagerSettings PageButtonCount="20" />
                                <PagerStyle Font-Bold="True" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
</asp:Content>
