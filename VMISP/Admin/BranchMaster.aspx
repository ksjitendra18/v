<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="BranchMaster.aspx.cs" Inherits="VMISP.Admin.BranchMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" lang="javascript" src="../Js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" lang="javascript" src="../Js/JS_CommonValidation.js"></script>
    <link href="../css/ssMain.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F;
        color: White; font-weight: 700;">
        Branch Master Maintenance
    </div>
    <table width="50%" align="left">
        <tr>
            <td>
                <table width="100%">
                    <tr style="display: none">
                        <td class="tdTextReight">
                            <span class="lblCaption">Type :</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="ddlDefault" Width="100px">
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                <asp:ListItem Text="BO" Value="BO"></asp:ListItem>
                                <asp:ListItem Text="CO" Value="CO"></asp:ListItem>
                                <asp:ListItem Text="ZO" Value="ZO"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTextReight">
                            <asp:Label ID="lblCircleOfficeRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">Circle Office :</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCircleOffice" runat="server" AutoPostBack="True" DataSourceID="sdsCO"
                                DataTextField="Branch_name" DataValueField="SOLID" CssClass="ddlDefault" Width="250px">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="sdsCO" runat="server" ConnectionString="<%$ ConnectionStrings:dbVIGILANCEMIS %>"
                                SelectCommand="((Select '' as [SOLID],'---Select Circle Office---' as [Branch_name]) union (SELECT [SOLID], [Branch_name] FROM [BRANCH_MASTER] where br_type='CO')) ORDER BY BRANCH_NAME">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdTextReight">
                            <span class="lblCaption">Branch Type :</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranchType" runat="server" DataSourceID="sdsBranchType"
                                DataTextField="BRNNAME" DataValueField="BRNVALUE" CssClass="ddlDefault" Width="100px">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="sdsBranchType" runat="server" ConnectionString="<%$ ConnectionStrings:dbVIGILANCEMIS %>"
                                SelectCommand="((SELECT '0' AS BRNVALUE,'Select' AS BRNNAME) UNION (SELECT DISTINCT BR_TYPE AS BRNVALUE,BR_TYPE AS BRNNAME FROM BRANCH_MASTER)) ORDER BY BRNVALUE">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTextReight">
                            <asp:Label ID="lblSolIDRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">Sol ID of the Branch (6 digits) :</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSolID" runat="server" MaxLength="6" ValidationGroup="Save" CssClass="txtDefault"
                                Width="96px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTextReight">
                            <asp:Label ID="lblNameRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">Name :</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBranchName" runat="server" TextMode="MultiLine" ValidationGroup="Save"
                                Width="250px" CssClass="txtDefault"></asp:TextBox>
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
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Font-Bold="True" OnClick="btnSave_Click"
                                Text="Save" ValidationGroup="Save" CssClass="btnDefault" />&nbsp
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btnDefault" OnClick="btnUpdate_Click"
                                Visible="false" />&nbsp
                            <asp:Button ID="btnDelete" runat="server" Font-Bold="True" OnClick="btnDelete_Click"
                                Text="Delete" ValidationGroup="Save" CssClass="btnDefault" Visible="false" />&nbsp
                            <asp:Button ID="btnCancel" runat="server" Font-Bold="True" OnClick="btnCancel_Click"
                                Text="Cancel" ValidationGroup="Save" CssClass="btnDefault" />
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
            </td>
        </tr>
    </table>
    <table width="40%" align="left">
        <tr>
            <td colspan="1">
                <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Vertical" Height="395px">
                    <asp:GridView ID="gvBranch" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px"
                        CellPadding="4" CellSpacing="2" DataSourceID="sdsBO" ForeColor="Black" Width="100%"
                        DataKeyNames="CODE" OnRowCommand="gvBranch_RowCommand" OnRowDataBound="gvBranch_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText">
                                <HeaderTemplate>
                                    Select
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnView" runat="server" CausesValidation="false" CommandName="View"
                                        ToolTip='<%# Eval("CIRCLECODE") %>' ImageUrl="~/images/selg_16.png" Height="20px"
                                        Width="18px" CommandArgument='<%#Eval("CODE")+"~"+ Eval("NAME")+"~"+ Eval("CIRCLECODE")+"~"+ Eval("ACTIVE")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CODE" HeaderText="SolID" SortExpression="CODE" HeaderStyle-CssClass="gridText"
                                ItemStyle-CssClass="gridText" />
                            <asp:BoundField DataField="NAME" HeaderText="Branch Name" SortExpression="NAME" HeaderStyle-CssClass="gridText"
                                ItemStyle-CssClass="gridText" />
                            <asp:BoundField DataField="STATUS" HeaderText="Branch Status" SortExpression="STATUS" HeaderStyle-CssClass="gridText"
                                ItemStyle-CssClass="gridText" />
                        </Columns>
                        <EmptyDataTemplate>
                            No Branch in this Circle Office!!
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
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="sdsBO" runat="server" ConnectionString="<%$ ConnectionStrings:dbVIGILANCEMIS %>"
        SelectCommand="SELECT SOLID AS CODE,Branch_name AS NAME,BR_PARENT_CODE2 AS CIRCLECODE,ACTIVE,(CASE WHEN(ACTIVE='N') THEN 'InActive' ELSE 'Active' END) AS STATUS FROM [BRANCH_MASTER] WHERE ([BR_PARENT_CODE2] = @BR_PARENT_CODE2)">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlCircleOffice" Name="BR_PARENT_CODE2" PropertyName="SelectedValue"
                Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
