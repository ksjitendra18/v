<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmUserList.aspx.cs" Inherits="VMISP.Admin.frmUserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F;
        color: White; font-weight: 700;">
        User List
    </div>
    <table style="padding-top: 5px;" align="left">
        <tr>
            <td>
                &nbsp;&nbsp; &nbsp;
            </td>
            <td>
                <span class="lblCaption">List Of User :</span>
            </td>
            <td>
                <asp:DropDownList ID="DropDownList1" runat="server" Width="175" AutoPostBack="True"
                    OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                    <asp:ListItem Value="">Select</asp:ListItem>
                    <asp:ListItem Value="ALL">All</asp:ListItem>
                    <asp:ListItem Value="VMIS_ADMIN">Admin</asp:ListItem>
                    <asp:ListItem Value="VMIS_DESKUSER">Desk User</asp:ListItem>
                    <asp:ListItem Value="VMIS_MISUSER">MIS User</asp:ListItem>
                    <asp:ListItem Value="VMIS_SUPERUSER">Super User</asp:ListItem>
                    <asp:ListItem Value="VMIS_VIEWUSER">View User</asp:ListItem>
                    <asp:ListItem Value="VMIS_CHECKER">Checker User</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="70%" align="left">
        <tr>
            <td colspan="1">
                <asp:Panel ID="pnlMain" runat="server" ScrollBars="Vertical" Height="420px">
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" CellPadding="3" DataSourceID="SqlDataSource1" GridLines="None"
                        HorizontalAlign="Center" BackColor="White" BorderColor="White" BorderStyle="Ridge"
                        BorderWidth="2px" CellSpacing="1" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
                            <asp:BoundField DataField="RoleName" HeaderText="RoleName" SortExpression="RoleName" />
                            <asp:CheckBoxField DataField="IsLockedOut" HeaderText="IsLockedOut" SortExpression="IsLockedOut" />
                            <asp:BoundField DataField="PropertyValuesString" HeaderText="PropertyValuesString"
                                SortExpression="PropertyValuesString" />
                        </Columns>
                        <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#E7E7FF" />
                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                        <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                        <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#594B9C" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#33276A" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AuthDB %>"
                        SelectCommand="SELECT * FROM [dbo].[View_ListofVigilanceMISUsers] where  RoleName=@RoleName">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList1" Name="RoleName" PropertyName="SelectedValue"
                                Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
