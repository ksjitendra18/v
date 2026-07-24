<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="changePwd.aspx.cs" EnableTheming="false" Inherits="VMISP.changePwd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title></title>
    <style type="text/css">
        #vertical1
        {
            position:absolute;
            top:50%;
            left: 0;
            width:100%;
            margin-top:-100px;
            text-align:center;
        }
        #horizontal
        {
            position:relative;
            width:400px;
            height:300px;
            margin: 0 auto;
        }
        .style1
        {
            height: 23px;
        }
    </style>

    
</head>
<body  style="margin: 0 auto">
    <form id="form1" runat="server">
    <div id="vertical1"> 
        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/PNBlogo_hindi_eng.jpg" />

    <div id="horizontal">
        <asp:ChangePassword ID="ChangePassword1" runat="server"  BackColor="#A12434" ForeColor="FloralWhite"  
            BorderColor="#FFDFAD" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" 
            Font-Names="Verdana" Font-Size="0.8em" 
            onchangedpassword="ChangePassword1_ChangedPassword" 
            oncontinuebuttonclick="ChangePassword1_ContinueButtonClick" 
            oncancelbuttonclick="ChangePassword1_CancelButtonClick" 
            onchangingpassword="ChangePassword1_ChangingPassword" 
            ChangePasswordFailureText="">
            <CancelButtonStyle BackColor="White" BorderColor="#CC9966" BorderStyle="Solid" 
                BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#990000" />
            <PasswordHintStyle Font-Italic="True" ForeColor="#888888" />
            <ContinueButtonStyle BackColor="White" BorderColor="#CC9966" 
                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" 
                ForeColor="#990000" />
            <ChangePasswordButtonStyle BackColor="White" BorderColor="#CC9966" 
                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" 
                ForeColor="#990000" />
            <TitleTextStyle BackColor="#990000" Font-Bold="True" Font-Size="0.9em" 
                ForeColor="White" />
            <ChangePasswordTemplate>
                <table border="0" cellpadding="4" cellspacing="0" 
                    style="border-collapse:collapse;">
                    <tr>
                    <td>Instructions:</td>
                    </tr>
                    <tr>
                                <td  align="left">
                                <ul >
                                <li>Password should contain minimum 1 alphabet</li>
                                <li>Password should contain minimum 1 Spl.Char</li>
                                <li>Password should contain minimum 1 number</li>
                                <li>Password Length Should be minimum 6</li>
                                <li>Password can't contain User-ID</li>
                                <li>Password can't be same as Last 5 passwords</li>
                                </ul>
                    </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="0" cellpadding="0">
                                <tr>
                                    <td align="center" colspan="2" 
                                        style="color:White;font-size:0.9em;font-weight:bold;">
                                        Change Your Password</td>
                                </tr>
                                <tr>
                                    <td align="right" class="style1">
                                        <asp:Label ID="CurrentPasswordLabel" runat="server" 
                                            AssociatedControlID="CurrentPassword">Current Password:</asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="CurrentPassword" runat="server" Font-Size="0.8em" 
                                            TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" 
                                            ControlToValidate="CurrentPassword" ErrorMessage="Password is required." 
                                            ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="NewPasswordLabel" runat="server" 
                                            AssociatedControlID="NewPassword">New Password:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="NewPassword" runat="server" Font-Size="0.8em" 
                                            TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" 
                                            ControlToValidate="NewPassword" ErrorMessage="New Password is required." 
                                            ToolTip="New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ConfirmNewPasswordLabel" runat="server" 
                                            AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="ConfirmNewPassword" runat="server" Font-Size="0.8em" 
                                            TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" 
                                            ControlToValidate="ConfirmNewPassword" 
                                            ErrorMessage="Confirm New Password is required." 
                                            ToolTip="Confirm New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:CompareValidator ID="NewPasswordCompare" runat="server" 
                                            ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" 
                                            Display="Dynamic" 
                                            ErrorMessage="The Confirm New Password must match the New Password entry." 
                                            ValidationGroup="ChangePassword1" ForeColor="White"></asp:CompareValidator>
<%--                                            <asp:CompareValidator ID="CompareValidator10" runat="server" 
                                            ControlToCompare="NewPassword" ControlToValidate="CurrentPassword" 
                                            Display="Dynamic" 
                                            ErrorMessage="New Password must be different from Old Password." 
                                            ValidationGroup="ChangePassword1" Operator="NotEqual" ForeColor="White"></asp:CompareValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color:White;">
                                        <asp:Literal ID="FailureText" runat="server"  EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="ChangePasswordPushButton" runat="server" BackColor="White" 
                                            BorderColor="#CC9966" BorderStyle="Solid" BorderWidth="1px" 
                                            CommandName="ChangePassword" Font-Names="Verdana" Font-Size="0.8em" 
                                            ForeColor="#990000" Text="Change Password" 
                                            ValidationGroup="ChangePassword1"  />
                                    </td>
                                    <td>
                                        <asp:Button ID="CancelPushButton" runat="server" BackColor="White" 
                                            BorderColor="#CC9966" BorderStyle="Solid" BorderWidth="1px" 
                                            CausesValidation="False" CommandName="Cancel" Font-Names="Verdana" 
                                            Font-Size="0.8em" ForeColor="#990000" Text="Cancel" />
                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                </table>
            </ChangePasswordTemplate>
            <SuccessTemplate>
                <table border="0" cellpadding="4" cellspacing="0" 
                    style="border-collapse:collapse;">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0">
                                <tr>
                                    <td align="center" colspan="2" 
                                        style="color:White;background-color:#990000;font-size:0.9em;font-weight:bold;">
                                        Change Password Complete</td>
                                </tr>
                                <tr>
                                    <td>
                                        Your password has been changed!</td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:Button ID="ContinuePushButton" runat="server" BackColor="White" 
                                            BorderColor="#CC9966" BorderStyle="Solid" BorderWidth="1px" 
                                            CausesValidation="False" CommandName="Continue" Font-Names="Verdana" 
                                            Font-Size="0.8em" ForeColor="#990000" Text="Continue" />
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
    </div>
    </div>
    </form>
</body>
</html>
