<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmPenaltyType.aspx.cs" Inherits="VMISP.Master.frmPenaltyType" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/ssMain.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" lang="javascript" src="../Js/JS_CommonFunction.js"></script>
    <script type="text/javascript" lang="javascript" src="../Js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" lang="javascript" src="../Js/JS_CommonValidation.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F;
        color: White; font-weight: 700;">
        Penalty Type Entry
    </div>
    <asp:Panel ID="pnlMain" runat="server" Width="100%">
        <div>
            <div style="float: right">
                <asp:Panel ID="pnlHeader" runat="server" Visible="false">
                    <span class="lblCaptionHead" style="font-size: small; font-weight: bold">Entry By :</span>
                    <asp:Label ID="lblEntryBy" runat="server" Width="50px" ForeColor="#FF3300" Font-Size="small"
                        Font-Bold="True"></asp:Label>&nbsp<span class="lblCaptionHead" style="font-size: small;
                            font-weight: bold">Entry Date :</span>
                    <asp:Label ID="lblEntryDate" runat="server" Width="75px" ForeColor="#FF3300" Font-Size="small"
                        Font-Bold="True"></asp:Label>&nbsp<span class="lblCaptionHead" style="font-size: small;
                            font-weight: bold">Modify By :</span>
                    <asp:Label ID="lblModifyBy" runat="server" Width="50px" ForeColor="#FF3300" Font-Size="small"
                        Font-Bold="True"></asp:Label>&nbsp<span class="lblCaptionHead" style="font-size: small;
                            font-weight: bold">Modify Date :</span>
                    <asp:Label ID="lblModifyDate" runat="server" Width="50px" ForeColor="#FF3300" Font-Size="small"
                        Font-Bold="True"></asp:Label>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                </asp:Panel>
            </div>
        </div>
        <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="100%">
            <act:TabPanel ID="tabEntry" runat="server">
                <HeaderTemplate>
                    <asp:Label ID="lblEntryHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                        Font-Size="Small" Text="Entry" ToolTip="Penalty Type Entry"></asp:Label>
                </HeaderTemplate>
                <ContentTemplate>
                    <table width="60%">
                        <tr>
                            <td class="tdTextReight">
                                <asp:Label ID="lblCodeRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                                    class="lblCaption">Code :</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode" runat="server" Width="85px" CssClass="txtDefault"></asp:TextBox>
                                &nbsp;<asp:ImageButton ID="imgGet" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                    Width="25px" Height="30px" OnClick="btnGet_Click" ToolTip="Nature of Punishment DA Search" />
                            </td>
                        </tr>
                        <tr>
                            <tr>
                                <td class="tdTextReight">
                                    <asp:Label ID="lblScaleRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                                        class="lblCaption">Name :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" Width="400px" CssClass="txtDefault"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Remarks :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" runat="server" Width="400px" CssClass="txtDefault"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Active :</span>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkActive" runat="server" />
                                </td>
                            </tr>
                    </table>
                    <table width="50%">
                        <tr>
                            <td style="width: 10%">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 90%; text-align: center;">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btnDefault" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btnDefault" OnClick="btnUpdate_Click"
                                    Visible="false" />&nbsp;&nbsp; &nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server"
                                        Text="Cancel" CssClass="btnDefault" OnClick="btnCancel_Click" />&nbsp;&nbsp;<asp:Label
                                            ID="lblMsg" runat="server" CssClass="lblMsg"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tabList" runat="server" HeaderText="List">
                <HeaderTemplate>
                    <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                        Font-Size="Small" Text="List" ToolTip="List of Penalty Type Master Entry"></asp:Label>
                </HeaderTemplate>
                <ContentTemplate>
                    <table width="50%">
                        <tr>
                            <td class="tdTextReight">
                                <span class="lblCaption">Code :</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode_LIST" runat="server" Width="100px" CssClass="txtDefault"></asp:TextBox>
                                &nbsp;<asp:ImageButton ID="imgSearch_List" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                    Width="25px" Height="30px" OnClick="imgSearch_LIST_Click" ToolTip="Penalty Type Search" />
                                &nbsp;&nbsp;<asp:Label ID="lblList" runat="server" CssClass="lblMsg"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlList" runat="server" ScrollBars="Both" Height="350px" Width="100%">
                        <table width="100%">
                            <tr>
                                <td style="width: 100%">
                                    <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" DataKeyNames="CODE"
                                        CellPadding="3" GridLines="None" ViewStateMode="Enabled" Style="margin-top: 0px"
                                        BackColor="White" BorderColor="White" BorderWidth="2px" CellSpacing="1" Width="100%"
                                        OnRowCommand="gvMain_RowCommand" AllowPaging="True" AllowSorting="True" Font-Size="Small"
                                        OnPageIndexChanging="gvMain_PageIndexChanging" PageSize="15" OnSorting="gvMain_Sorting"
                                        OnRowDataBound="gvMain_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText">
                                                <HeaderTemplate>
                                                    Select
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnView" runat="server" CausesValidation="false" CommandName="View"
                                                        ToolTip='<%# Eval("ID") %>' ImageUrl="~/images/selg_16.png" Height="20px" Width="18px"
                                                        CommandArgument='<%# Eval("ID")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CODE" HeaderText="Code" SortExpression="CODE" HeaderStyle-CssClass="gridText"
                                                ItemStyle-CssClass="gridText" />
                                            <asp:BoundField DataField="NAME" HeaderText="Scale" SortExpression="NAME" HeaderStyle-CssClass="gridText"
                                                ItemStyle-CssClass="gridText" />
                                            <asp:BoundField DataField="REMARKS" HeaderText="Remarks" SortExpression="REMARKS"
                                                HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                            <asp:BoundField DataField="ACTIVE" HeaderText="Active" SortExpression="ACTIVE" HeaderStyle-CssClass="gridText"
                                                ItemStyle-CssClass="gridText" />
                                            <asp:BoundField DataField="ENTRYBY" HeaderText="Entry User" SortExpression="ENTRYBY"
                                                HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                            <asp:BoundField DataField="ENTRYDATE" HeaderText="Entry Date" SortExpression="ENTRYDATE"
                                                HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                            <asp:BoundField DataField="MODIFYBY" HeaderText="Modify User" SortExpression="MODIFYBY"
                                                HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                            <asp:BoundField DataField="MODIFYDATE" HeaderText="Modify Date" SortExpression="MODIFYDATE"
                                                HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                        </Columns>
                                        <PagerSettings PageButtonCount="10" />
                                        <PagerStyle Font-Bold="True" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </act:TabPanel>
        </act:TabContainer>
    </asp:Panel>
</asp:Content>
