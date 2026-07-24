<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="UserCreation.aspx.cs" Inherits="VMISP.UserCreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/ssMain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2 {
            width: 196px;
        }
    </style>
    <script type="text/javascript" lang="javascript" src="../Js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" lang="javascript" src="../Js/JS_CommonValidation.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F; color: White; font-weight: 700;">
        User Creation
    </div>
    <table style="width: 100%; padding-top: 5px;">
        <tr>
            <td class="tdTextReight">
                <asp:Label ID="lblPFNoRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                    class="lblCaption">PF Number :</span>
            </td>
            <td>
                <asp:TextBox ID="TxtPF" runat="server" CssClass="txtDefault" Width="150px"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="Search_group"
                    OnClick="btnSearch_Click" />
            </td>
        </tr>
        <tr>
            <td class="tdTextReight">
                <asp:Label ID="lblNameRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                    class="lblCaption">Name of Employee :</span>
            </td>
            <td>
                <asp:TextBox ID="TxtName" runat="server" CssClass="txtDefault" Width="245px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdTextReight">
                <asp:Label ID="lblEmailRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                    class="lblCaption">Email -ID :</span>
            </td>
            <td>
                <asp:TextBox ID="TxtEmail" runat="server" CssClass="txtDefault" Width="245px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdTextReight">
                <asp:Label ID="lblPlaceofPostingRequired" runat="server" Text="*" Font-Bold="True"
                    ForeColor="Red"></asp:Label><span class="lblCaption">Placed of Posting :</span>
            </td>
            <td>
                <asp:DropDownList ID="DDPOP" runat="server" DataTextField="item_desc" DataValueField="item_value"
                    Width="250px">
                    <asp:ListItem Value=" ">Select </asp:ListItem>
                    <asp:ListItem Value="5135"> Vigilance HO </asp:ListItem>
                </asp:DropDownList>
            </td>
            <td></td>
        </tr>
     <%--   <tr>
            <td class="tdTextReight">
                <asp:Label ID="lblLocationRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                    class="lblCaption">Location/Role :</span>
            </td>
            <td>
                <asp:DropDownList ID="DDLocation" runat="server" Width="250px" DataTextField="SETIDDESC"
                    DataValueField="SETID" DataSourceID="sdsLocation">
                </asp:DropDownList>
                <asp:SqlDataSource ID="sdsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:dbVIGILANCEMIS %>"
                    SelectCommand="((Select 'Select Office' AS SETIDDESC,'0' AS SETID) UNION (SELECT ITEM_DESC AS SETIDDESC, ITEM_VALUE AS setid FROM [tbl_UserCreationDD] WHERE [ROLE] =@ROLE)) ORDER BY SETID">
                    <SelectParameters>
                        <asp:SessionParameter SessionField="role" Name="ROLE" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td></td>
        </tr>
        <tr id="trZone" runat="server" style="display: none;">
            <td class="tdTextReight">
                <asp:Label ID="lblZoneRequired" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <span class="lblCaption">Zone :</span>
            </td>
            <td>
                <asp:DropDownList ID="ddlZone" runat="server" Width="250px">
                </asp:DropDownList>
            </td>
        </tr>--%>

        <tr>
    <td class="tdTextReight">
        <asp:Label ID="lblLocationRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label>
        <span class="lblCaption">Location/Role :</span>
    </td>
    <td>
        <asp:DropDownList ID="DDLocation"
            runat="server"
            Width="250px"
            DataTextField="SETIDDESC"
            DataValueField="SETID"
            DataSourceID="sdsLocation"
            AutoPostBack="true"
            OnSelectedIndexChanged="DDLocation_SelectedIndexChanged">
        </asp:DropDownList>

        <asp:SqlDataSource ID="sdsLocation"
            runat="server"
            ConnectionString="<%$ ConnectionStrings:dbVIGILANCEMIS %>"
            SelectCommand="
                ((SELECT 'Select Office' AS SETIDDESC, '0' AS SETID)
                 UNION
                 (SELECT ITEM_DESC AS SETIDDESC,
                         ITEM_VALUE AS SETID
                  FROM tbl_UserCreationDD
                  WHERE ROLE = @ROLE)
                 UNION
                 (SELECT 'CHECKER' AS SETIDDESC,
                         'VMIS_CHECKER' AS SETID))
                ORDER BY SETID">

            <SelectParameters>
                <asp:SessionParameter SessionField="role" Name="ROLE" />
            </SelectParameters>

        </asp:SqlDataSource>
    </td>
    <td></td>
</tr>

<%--<tr id="trZone" runat="server" visible="false">
    <td class="tdTextReight">
        <asp:Label ID="lblZoneRequired" runat="server" Text="*" ForeColor="Red"></asp:Label>
        <span class="lblCaption">Zone :</span>
    </td>
    <td>
        <asp:DropDownList ID="ddlZone" runat="server" Width="250px">
            <asp:ListItem Value="">Select Zone</asp:ListItem>
        </asp:DropDownList>
    </td>
    <td></td>
</tr>--%>

        <asp:CheckBoxList
    ID="chkZones"
    runat="server"
    RepeatDirection="Vertical">
</asp:CheckBoxList>



        <tr style="display: none">
            <td class="style1">
                <asp:Label ID="lblLandLine" runat="server" Text="LandLine*"></asp:Label>
            </td>
            <td class="style2">
                <asp:TextBox ID="TxtLL" runat="server" MaxLength="11"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none">
            <td class="style1">
                <asp:Label ID="lblMobile" runat="server" Text="Mobile No."></asp:Label>
            </td>
            <td class="style2">
                <asp:TextBox ID="TxtMobile" runat="server" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="BtnSubmit" runat="server" OnClick="BtnSubmit_Click" Text="Update"
                    ValidationGroup="Update_Group" />
                <asp:Button ID="btnResetPassword" runat="server" OnClick="btnResetPassword_Click"
                    Text="Reset Password" />
                <asp:Button ID="btnRemove" runat="server" OnClick="btnRemove_Click" Text="Remove"
                    OnClientClick="return confirm('Are you sure you want to delete this user?');"
                    ValidationGroup="Update_Group" />
            </td>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
            <td colspan="2">
                <asp:Label ID="LblResponse" runat="server" EnableViewState="False"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
