<%@ Page Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="frmChangePassword.aspx.cs"
    Inherits="VMISP.frmChangePassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/ssMain.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="../Js/JS_CommonValidation.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F;
        color: White; font-weight: 700;">
        CHANGE PASSWORD
    </div>
    <asp:Panel ID="pnlMain" runat="server" Width="100%">
        <div style="height: 350px; width: 100%;" align="center">
            <div style="float: left; width: 35%; height: 350px; background-image: url('http://localhost:65180/images/ChangePassword.jpg');">
            </div>
            <div style="float: left; width: 30%; height: 350px;">
                <fieldset style="height: 310px; width: 92%;">
                    <legend>Instructions </legend>
                    <asp:ChangePassword ID="ChangePassword1" runat="server" OnChangedPassword="ChangePassword1_ChangedPassword"
                        OnContinueButtonClick="ChangePassword1_ContinueButtonClick" OnCancelButtonClick="ChangePassword1_CancelButtonClick"
                        OnChangingPassword="ChangePassword1_ChangingPassword" ChangePasswordFailureText="">
                        <%-- <CancelButtonStyle BackColor="White" BorderColor="#CC9966" BorderStyle="Solid" BorderWidth="1px"
                    Font-Names="Verdana" Font-Size="0.8em" />--%>
                        <PasswordHintStyle Font-Italic="True" />
                        <%--    <ContinueButtonStyle BackColor="White" BorderColor="#CC9966" BorderStyle="Solid"
                    BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" />--%>
                        <%--  <ChangePasswordButtonStyle BackColor="White" BorderColor="#CC9966" BorderStyle="Solid"
                    BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" />--%>
                        <%--   <TitleTextStyle BackColor="#990000" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />--%>
                        <ChangePasswordTemplate>
                            <table border="0" cellpadding="4" cellspacing="0" style="border-collapse: collapse;">
                                <tr>
                                    <td align="left">
                                        <ul>
                                            <li><span style="color: Red; font-weight: bold;">Password should contain minimum '1'
                                                alphabet</span></li>
                                            <li><span style="color: Blue; font-weight: bold;">Password should contain minimum '1'
                                                Spl.Char</span></li>
                                            <li><span style="color: Green; font-weight: bold;">Password should contain minimum '1'
                                                number</span></li>
                                            <li><span style="color: Gray; font-weight: bold;">Password Length Should be minimum
                                                '6'</span></li>
                                            <li><span style="color: Maroon; font-weight: bold;">Password can't contain User-ID</span></li>
                                            <li><span style="color: Orange; font-weight: bold;">Password can't be same as Last '5'
                                                passwords</span></li>
                                        </ul>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" cellpadding="0">
                                <tr>
                                    <td align="right" class="style1">
                                        <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword"
                                            class="lblCaption">Current Password :</asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password" CssClass="txtDefault"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdTextReight">
                                        <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword"
                                            class="lblCaption">New Password :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="NewPassword" runat="server" TextMode="Password" CssClass="txtDefault"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                            ErrorMessage="New Password is required." ToolTip="New Password is required."
                                            ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdTextReight">
                                        <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword"
                                            class="lblCaption">Confirm New Password :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password" CssClass="txtDefault"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                            ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
                                            ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                                            ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
                                            ValidationGroup="ChangePassword1" ForeColor="Red" Font-Size="Medium" Font-Bold="True"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3" style="color: red; font-size: small; font-bold: True;"
                                        class="lblCaption">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                        <%--<asp:Label ID="FailureText2" runat="server" EnableViewState="False" CssClass="lblCaption"></asp:Label>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="ChangePasswordPushButton" runat="server" CommandName="ChangePassword"
                                            CssClass="btnDefault" Text="Change Password" ValidationGroup="ChangePassword1" />
                                    </td>
                                    <td>
                                        <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                            CssClass="btnDefault" Text="Cancel" />
                                    </td>
                                </tr>
                            </table>
                        </ChangePasswordTemplate>
                        <SuccessTemplate>
                            <table border="0" cellpadding="4" cellspacing="0" style="border-collapse: collapse;">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0">
                                            <tr>
                                                <td align="center" colspan="2" style="color: White; background-color: #990000; font-size: 0.9em;
                                                    font-weight: bold;">
                                                    Change Password Complete
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="lblCaption" style="color: Red">Your password has been changed!</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="2">
                                                    <asp:Button ID="ContinuePushButton" runat="server" CausesValidation="False" CommandName="Continue"
                                                        Text="Continue" CssClass="btnDefault" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </SuccessTemplate>
                        <TextBoxStyle Font-Size="0.8em" />
                        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                    </asp:ChangePassword>
                </fieldset>
            </div>
            <div style="width: 35%; height: 350px; float: left; background-image: url('http://localhost:65180/images/ChangePassword.jpg');">
            </div>
        </div>
    </asp:Panel>
</asp:Content>
