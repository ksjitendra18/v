<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginSSO.aspx.cs" Inherits="VMISP.LoginSSO" ValidateRequest="false" ViewStateEncryptionMode="Always" EnableViewState="true" ViewStateMode="Enabled"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="css/normalize.css" rel="stylesheet" type="text/css" />
    <link href="css/styles.css" rel="stylesheet" type="text/css" />
    <title>Welcome to Vigilance Management Information System</title>
    <style type="text/css">
        /* NOTE: The styles were added inline because Prefixfree needs access to your styles and they must be inlined if they are on local disk! */

        .btn {
            display: inline-block;
            *display: inline;
            padding: 4px 10px 4px;
            margin-bottom: 0;
            font-size: 13px;
            line-height: 18px;
            color: #333333;
            text-align: center;
            vertical-align: middle;
            background-color: #f5f5f5;
            background-image: -moz-linear-gradient(top, #ffffff, #e6e6e6);
            background-image: -ms-linear-gradient(top, #ffffff, #e6e6e6);
            background-image: -webkit-gradient(linear, 0 0, 0 100%, from(#ffffff), to(#e6e6e6));
            background-image: -webkit-linear-gradient(top, #ffffff, #e6e6e6);
            background-image: -o-linear-gradient(top, #ffffff, #e6e6e6);
            background-image: linear-gradient(top, #ffffff, #e6e6e6);
            background-repeat: repeat-x;
            filter: progid:dximagetransform.microsoft.gradient(startColorstr=#ffffff, endColorstr=#e6e6e6, GradientType=0);
            border-color: #e6e6e6 #e6e6e6 #e6e6e6;
            border-color: rgba(0, 0, 0, 0.1) rgba(0, 0, 0, 0.1) rgba(0, 0, 0, 0.25);
            border: 1px solid #e6e6e6;
            -webkit-border-radius: 4px;
            -moz-border-radius: 4px;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.2), 0 1px 2px rgba(0, 0, 0, 0.05);
            -moz-box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.2), 0 1px 2px rgba(0, 0, 0, 0.05);
            box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.2), 0 1px 2px rgba(0, 0, 0, 0.05);
            cursor: pointer;
            *margin-left: .3em;
        }

            .btn:hover, .btn:active, .btn.active, .btn.disabled, .btn[disabled] {
                background-color: #e6e6e6;
            }

        .btn-large {
            padding: 9px 14px;
            font-size: 15px;
            line-height: normal;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
        }

        .btn:hover {
            color: #333333;
            text-decoration: none;
            background-color: #e6e6e6;
            background-position: 0 -15px;
            -webkit-transition: background-position 0.1s linear;
            -moz-transition: background-position 0.1s linear;
            -ms-transition: background-position 0.1s linear;
            -o-transition: background-position 0.1s linear;
            transition: background-position 0.1s linear;
        }

        .btn-primary, .btn-primary:hover {
            color: #ffffff;
        }

            .btn-primary.active {
                color: rgba(255, 255, 255, 0.75);
            }

        .btn-primary {
            background-color: #4a77d4;
            background-image: -moz-linear-gradient(top, #6eb6de, #4a77d4);
            background-image: -ms-linear-gradient(top, #6eb6de, #4a77d4);
            background-image: -webkit-gradient(linear, 0 0, 0 100%, from(#6eb6de), to(#4a77d4));
            background-image: -webkit-linear-gradient(top, #6eb6de, #4a77d4);
            background-image: -o-linear-gradient(top, #6eb6de, #4a77d4);
            background-image: linear-gradient(top, #6eb6de, #4a77d4);
            background-repeat: repeat-x;
            filter: progid:dximagetransform.microsoft.gradient(startColorstr=#6eb6de, endColorstr=#4a77d4, GradientType=0);
            border: 1px solid #3762bc;
            text-shadow: 1px 1px 1px rgba(0,0,0,0.4);
            box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.2), 0 1px 2px rgba(0, 0, 0, 0.5);
        }

            .btn-primary:hover, .btn-primary:active, .btn-primary.active, .btn-primary.disabled, .btn-primary[disabled] {
                background-color: #4a77d4;
            }

        .btn-block {
            width: 100%;
            display: block;
        }

        * {
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -o-box-sizing: border-box;
        }

        html {
            width: 100%;
            height: 100%;
            overflow: hidden;
        }

        .body {
            width: 100%;
            height: 100%;
            font-family: 'Open Sans', sans-serif;
            background: #092756;
            background: -moz-radial-gradient(0% 100%, ellipse cover, rgba(104,128,138,.4) 10%,rgba(138,114,76,0) 40%),-moz-linear-gradient(top, rgba(57,173,219,.25) 0%, rgba(42,60,87,.4) 100%), -moz-linear-gradient(-45deg, #670d10 0%, #092756 100%);
            background: -webkit-radial-gradient(0% 100%, ellipse cover, rgba(104,128,138,.4) 10%,rgba(138,114,76,0) 40%), -webkit-linear-gradient(top, rgba(57,173,219,.25) 0%,rgba(42,60,87,.4) 100%), -webkit-linear-gradient(-45deg, #670d10 0%,#092756 100%);
            background: -o-radial-gradient(0% 100%, ellipse cover, rgba(104,128,138,.4) 10%,rgba(138,114,76,0) 40%), -o-linear-gradient(top, rgba(57,173,219,.25) 0%,rgba(42,60,87,.4) 100%), -o-linear-gradient(-45deg, #670d10 0%,#092756 100%);
            background: -ms-radial-gradient(0% 100%, ellipse cover, rgba(104,128,138,.4) 10%,rgba(138,114,76,0) 40%), -ms-linear-gradient(top, rgba(57,173,219,.25) 0%,rgba(42,60,87,.4) 100%), -ms-linear-gradient(-45deg, #670d10 0%,#092756 100%);
            background: -webkit-radial-gradient(0% 100%, ellipse cover, rgba(104,128,138,.4) 10%,rgba(138,114,76,0) 40%), linear-gradient(to bottom, rgba(57,173,219,.25) 0%,rgba(42,60,87,.4) 100%), linear-gradient(135deg, #670d10 0%,#092756 100%);
        }

        .login {
            position: absolute;
            top: 30%;
            left: 30%;
            margin: -150px 0 0 -150px;
            width: 100%;
            height: 300px;
        }

        .h1 {
            letter-spacing: 1px;
            text-align: center;
            color: Silver;
            font-style: normal;
            font: bold;
            font-size: x-large;
        }

        .TextBox {
            width: 100%;
            margin-bottom: 10px;
            border: none;
            outline: none;
            padding: 10px;
            font-size: 13px;
            border: 1px solid rgba(0,0,0,0.3);
            -webkit-transition: box-shadow .5s ease;
            -moz-transition: box-shadow .5s ease;
            -o-transition: box-shadow .5s ease;
            -ms-transition: box-shadow .5s ease;
        }
    </style>
    <script type="text/javascript">
        function blockSpecialChar(e) {
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
        }
    </script>
    <script type="text/javascript">
            if (document.layers) {
                //Capture the MouseDown event.
                document.captureEvents(Event.MOUSEDOWN);

                //Disable the OnMouseDown event handler.
                document.onmousedown = function () {
                    return false;
                };
            }
            else {
                //Disable the OnMouseUp event handler.
                document.onmouseup = function (e) {
                    if (e != null && e.type == "mouseup") {
                        //Check the Mouse Button which is clicked.
                        if (e.which == 2 || e.which == 3) {
                            //If the Button is middle or right then disable.
                            return false;
                        }
                    }
                };
            }

            //Disable the Context Menu event.
            document.oncontextmenu = function () {
                return false;
            };
    </script>
    <script type="text/javascript">
            window.onload = function () {
                noBack();
            }
            function noBack() {
                window.history.forward();
            }
    </script>
    <script type="text/javascript">
            function preventBack() { window.history.forward(); }
            setTimeout("preventBack()", 0);
            window.onunload = function () { null };
    </script>
    <script type="text/javascript">
            function UniqueID() {
                function s4() {
                    return Math.floor((1 + Math.random()) * 0x10000)
                        .toString(16)
                        .substring(1);
                }
                return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
                    s4() + '-' + s4() + s4() + s4();
            }

            function Encrypt() {
                var txtPassword = document.getElementById("Login1_Password");
                var hidUniqueID = document.getElementById("hidUniqueID");
                var hidPassword = document.getElementById("hidPassword");

                var text = createRandomString(4);
                hidUniqueID.value = UniqueID();

                for (i = 0; i < txtPassword.value.length; i++) {
                    var t1 = txtPassword.value.slice(i, (i + 1));
                    var t2 = createRandomString(2);
                    text = text + t1 + t2;
                }

                text = text + hidUniqueID.value;
                hidPassword.value = text;

                document.getElementById("Login1_Password").value = text;
            }

            function Success(result) {
                alert(result);
            }

            function Failure(error) {
                alert(error);
            }

            function createRandomString(length) {
                var str = "";
                for (; str.length < length; str += Math.random().toString(36).substr(2));
                return str.substr(0, length);
            }
    </script>
</head>
<body class="body">
    <form id="form1" runat="server">
        <div class="login">
            <table>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/images/PNB_LOGO.png" />
                        <asp:Label ID="Label2" runat="server" Font-Size="XX-Large" ForeColor="#A12434" Text="Vigilance Management Information System Portal"
                            Style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="font-weight: 700; text-align: center; font-size: xx-large;"
                        class="style4">
                        <em style="font-size: medium; color: Green;">Version V1.1.000817 </em>
                    </td>
                </tr>
                <tr align="center">
                    <td rowspan="2">
                        <asp:Login ID="Login1" runat="server" Font-Size="Small" OnLoggedIn="Login1_LoggedIn" OnAuthenticate="aspLogin_Authenticate"
                            OnLoggingIn="Login1_LoggingIn" Width="350">
                            <LayoutTemplate>
                                <fieldset>
                                    <table border="0" cellpadding="5" cellspacing="0" style="border-collapse: collapse;">
                                        <tr>
                                            <h1 class="h1">Login</h1>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="UserName" runat="server" AutoCompleteType="Disabled" placeholder="User ID" MaxLength="6"
                                                                CssClass="TextBox" ToolTip="Enter User ID" Width="250px" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                                ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="Password" runat="server" TextMode="Password" placeholder="Password" 
                                                                CssClass="TextBox" ToolTip="Enter User Password" Width="250px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                                ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td align="center" colspan="2" style="color: Red;">
                                                <asp:Label ID="resetlabel" runat="server" ForeColor="red" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="color: Red;">
                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblLocked" runat="server" ForeColor="red" Visible="false" Text="User Locked, please contact to Vigilance Administrator"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--<table>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Let me in."
                                                                ValidationGroup="Login1" OnClick="LoginButton_Click" class="btn btn-primary btn-block btn-large"
                                                                Width="100px" ToolTip="if you have User id and Password please Login" OnClientClick="Encrypt()" />
                                                        </td>
                                                        &nbsp;&nbsp;
                                                    <td>
                                                        <asp:Button ID="ResetButton" runat="server" OnClientClick="return confirm('Are you sure?');"
                                                            CommandName="ResetPwd" ValidationGroup="ResetButton1" class="btn btn-primary btn-block btn-large"
                                                            Width="135px" Text="Reset Password" OnClick="ResetButton_Click" ToolTip="Reset Password of Locked User and Password sent to user Emial id" />
                                                    </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>--%>
                                    <table>
                                        <tr align="center">
                                            <td align="center" style="padding-left: 30px;">
                                                <asp:HyperLink ID="hlDownloadUserManual" runat="server" NavigateUrl="../VigilanceMIS/VMIS_User_Manual.pdf"
                                                    Style="text-align: center" ToolTip="User Manual of Vigilance Management Information System"
                                                    ForeColor="Red" Font-Size="Small" Font-Bold="True" Target="_blank">Click here to download the User Manual</asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </LayoutTemplate>
                        </asp:Login>
                    </td>
                </tr>
                <tr style="display: none;">
                    <%--<td colspan="2" class="style3">
                    <span class="style4"><strong><em style="font-size: large; color: #A12434">
                        <marquee>"Without Fear, Without Favour"</marquee>
                    </em></strong></span>
                </td>--%>

                    <td align="center" style="font-family: Verdana; font-size: small; font-weight: bold; color: greenyellow">
                        <br />
                        <br />
                        If case of any query/difficulty please contact:<br />
                        <br />
                        Contact Detail of Vigilance Management HO Division for Operational query:<br />
                        Narendra Singh Chouhan :
                        <br />
                        <br />
                        Contact Detail of Information Technology Division for Technical query:<br />
                        Gaurav Kumar/Ravi Agrawal : +91-11-23356506
                   <br />
                        Email Id:&nbsp; gaurav.kumar11@pnb.co.in,raviagrawal@pnb.co.in
                    <br />
                        <%-- <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/images/a.pdf">Download User Manual</asp:HyperLink>--%>

                    </td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hiddenforcelogin" runat="server" Value="0" />
        <asp:HiddenField ID="hidUniqueID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hidPassword" runat="server" ClientIDMode="Static" />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </form>
</body>
</html>
