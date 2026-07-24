<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmCircleHead.aspx.cs" Inherits="VMISP.Admin.frmCircleHead" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" lang="javascript" src="../Js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" lang="javascript" src="../Js/JS_CommonValidation.js"></script>
    <link href="../css/ssMain.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F; color: White; font-weight: 700;">
        Branch Master Maintenance
    </div>
    <table style="width: 80%;">
        <tr>
            <td class="tdTextReight">
                <asp:Label ID="lblCircleOfficeRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">Circle Office :</span>
            </td>
            <td>
                <asp:DropDownList ID="ddlCircleOffice" runat="server" AutoPostBack="True" DataSourceID="sdsCO"
                    DataTextField="Branch_name" DataValueField="SOLID" CssClass="ddlDefault" Width="250px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="sdsCO" runat="server" ConnectionString="<%$ ConnectionStrings:dbVIGILANCEMIS %>"
                    SelectCommand="((Select '' as [SOLID],'---Select Circle Office---' as [Branch_name]) union (SELECT [SOLID], [Branch_name] FROM [BRANCH_MASTER] where br_type IN('CO','HO') AND ACTIVE='Y')) ORDER BY BRANCH_NAME"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td class="tdTextReight">
                <asp:Label ID="lblNameRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">Name :</span>
            </td>
            <td>
                <asp:TextBox ID="txtCircleHeadName" runat="server" ValidationGroup="Save" Width="500px" CssClass="txtDefault"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdTextReight"></td>
            <td>
                <asp:Button ID="btnSave" runat="server" Font-Bold="True" OnClick="btnSave_Click"
                    Text="Save" ValidationGroup="Save" CssClass="btnDefault" Visible="false" />&nbsp
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btnDefault" OnClick="btnUpdate_Click"
                                Visible="false" />&nbsp
                           <asp:Button ID="btnCancel" runat="server" Font-Bold="True" OnClick="btnCancel_Click"
                               Text="Cancel" ValidationGroup="Save" CssClass="btnDefault" />
            </td>
        </tr>
        <tr>
            <td class="tdTextReight"></td>
            <td>
                <asp:Label ID="lblMsg" runat="server" CssClass="lblMsg"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="tdTextReight"></td>
            <td>
                <asp:Panel ID="pnlGrid" runat="server" GroupingText="Show Circle Head Details">
                    <asp:GridView ID="gvMain" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px"
                        CellPadding="4" CellSpacing="2" DataSourceID="sdsDetails" ForeColor="Black" Width="100%"
                        DataKeyNames="CODE" OnRowCommand="gvMain_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText">
                                <HeaderTemplate>
                                    Select
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnView" runat="server" CausesValidation="false" CommandName="View"
                                        ToolTip='<%# Eval("CIRCLECODE") %>' ImageUrl="~/images/selg_16.png" Height="20px"
                                        Width="18px" CommandArgument='<%#Eval("CODE")+"~"+ Eval("NAME")+"~"+ Eval("CIRCLECODE")+"~"+ Eval("CIRCLEHEAD")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CODE" HeaderText="SolID" />
                            <asp:BoundField DataField="NAME" HeaderText="Circle Name" />
                            <asp:BoundField DataField="CIRCLEHEAD" HeaderText="Circle Head" />
                            <asp:BoundField DataField="MODUSER" HeaderText="Entry By" />
                            <asp:BoundField DataField="MODDATE" HeaderText="Entry Date" />
                        </Columns>
                        <EmptyDataTemplate>
                            No Circle Head updated in this Circle Office!!
                        </EmptyDataTemplate>
                        <FooterStyle BackColor="#CCCCCC" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                        <RowStyle BackColor="White" />
                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#808080" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#383838" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="sdsDetails" runat="server" ConnectionString="<%$ ConnectionStrings:dbVIGILANCEMIS %>"
                        SelectCommand="SELECT SOLID AS CODE,Branch_name AS NAME,DNO AS CIRCLECODE,CONTACT_PERSON AS CIRCLEHEAD, CONTACTPERSON_MODUSER AS MODUSER,CONVERT(VARCHAR(50),CONTACTPERSON_MODDATE,103) AS MODDATE FROM [BRANCH_MASTER] WHERE (SOLID = @SOLID)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlCircleOffice" Name="SOLID" PropertyName="SelectedValue"
                                Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
