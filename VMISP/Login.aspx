<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VMISP.Login" ValidateRequest="false" ViewStateEncryptionMode="Always" EnableViewState="true" ViewStateMode="Enabled" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<!DOCTYPE html>

<html>
<head runat="server">
    <title>Vigilance MIS Portal</title>
    <link rel="shortcut icon" href="/images/favicon.ico" type="image/x-icon" />
    <script type="application/x-javascript">
        addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false); function hideURLbar(){ window.scrollTo(0,1); }
    </script>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="/fonts/font-awesome-4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="/fonts/iconic/css/material-design-iconic-font.min.css" />
    <link rel="stylesheet" type="text/css" href="/css/util.css" />
    <link rel="stylesheet" type="text/css" href="/css/main.css" />
    <script src="/js/PVC.js"></script>
    <link rel="stylesheet" type='text/css' href="/css/bootstrap.css" />

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
    <script>
        function lettersOnly() {
            var charCode = event.keyCode;
            if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 8)

                return true;
            else
                return false;
        }
    </script>

    <style>
        .txtbox {
            border-top-left-radius: 20px;
            border-top-right-radius: 20px;
            border-bottom-left-radius: 20px;
            border-bottom-right-radius: 20px;
        }
    </style>


    <style>
        .header {
            z-index: 999;
            width: 100%;
            height:60px;
            float: left;
            background-color: #A30D3B;
            position: fixed;
            top: 0;
            left: 0;
        }

        .limiter {
            float: left;
            background-color: #FBBC09 !important;
        }

        .container-login100 {
            margin: 59px 0 0;
        }

        .custom-container {
            display: block;
            margin: 0 auto;
            width: 100%;
            max-width: 1140px;
            overflow: hidden;
        }

        .logo {
            margin: 0px auto;
            width: 100%;
            max-width: 450px;
        }

        img {
            max-width: 100%;
            height: 60px;
        }

        .blink {
            width: 100%;
            text-align: center;
            float: left;
        }

        /*span {
            font-size: 25px;
            font-family: arial;
            text-align: center;
            color: red;
            animation: blink 1s linear infinite;
        }*/

        @keyframes blink {
            0% {
                opacity: 0;
            }

            50% {
                opacity: .5;
            }

            100% {
                opacity: 1;
            }
        }

        .banner {
            width: 60%;
            float: left;
            padding: 20px 40px;
            margin: 0;
        }

            .banner h1 {
                text-align: center;
                font-size: 18px;
                font-weight: bold;
            }

            .banner img {
                width: 500px;
                margin: 0px auto;
                display: block;
            }

        .brand-block {
            width: 60%;
            float: left;
            padding: 20px 40px;
            margin: 0;
        }

            .brand-block h1 {
                text-align: center;
                font-size: 18px;
                font-weight: bold;
            }

            .brand-block img {
                width: 100px;
                margin: 0px auto;
                display: block;
            }

            .brand-block DisplayImage {
                width: 100px;
                margin: 0px auto;
                display: block;
            }

        .login-block {
            float: left;
            width: 40%;
            margin: 0;
            padding: 20px 40px;
            border-left: 10px solid #ddd;
        }

            .login-block h2 {
                text-align: center;
                font-size: 18px;
                font-weight: bold;
            }

            .login-block img {
                width: 180px;
                margin: 0px auto;
                display: block;
            }

        .footer {
            background-color: #A30D3B;
            color: #fff;
            text-align: center;
            position: fixed;
            overflow: hidden;
            left: 0;
            bottom: 0;
            width: 100%;
        }

            .footer a {
                color: #fff !important;
                font-size: 15px;
            }

            .footer small {
                font-size: 15px;
                margin: 2px 0;
                display: block;
            }

        #slider {
            position: relative;
            overflow: hidden;
            margin: 20px auto;
            border-radius: 4px;
        }

            #slider ul {
                position: relative;
                margin: 0;
                padding: 0;
                height: 275px;
                list-style: none;
            }

                #slider ul li {
                    position: relative;
                    display: block;
                    float: left;
                    margin: 0;
                    padding: 0;
                    width: 650px;
                    height: 270px;
                    background: #ccc;
                    text-align: center;
                    line-height: 300px;
                }

        a.control_prev, a.control_next {
            position: absolute;
            top: 40%;
            z-index: 999;
            display: block;
            padding: 4% 3%;
            width: auto;
            height: auto;
            background: #2a2a2a;
            color: #fff;
            text-decoration: none;
            font-weight: 600;
            font-size: 18px;
            opacity: 0.8;
            cursor: pointer;
        }

            a.control_prev:hover, a.control_next:hover {
                opacity: 1;
                -webkit-transition: all 0.2s ease;
            }

        a.control_prev {
            border-radius: 0 2px 2px 0;
        }

        a.control_next {
            right: 0;
            border-radius: 2px 0 0 2px;
        }

        .slider_option {
            position: relative;
            margin: 10px auto;
            width: 160px;
            font-size: 18px;
        }

        .brand-block #slider img {
            width: 820px;
            height: 400px;
            margin: 0;
        }

        .link {
            font-family: Poppins-Regular;
            font-size: 14px;
            line-height: 1.7;
            color: #666666;
            margin: 10px;
            display: block;
            text-decoration: underline;
            cursor: pointer;
        }

        /*ribbon css*/
        #ribbon {
            position: fixed;
            left: 0;
            top: 0;
            background: rgba(0,0,0, 0.8);
            z-index: 99999999;
            min-height: 1000px;
            cursor: none;
            width: 100%;
            display: none;
        }

        .cursor {
            background-image: url(../images/scissor-final.png);
            background-position: 0 0;
            background-repeat: no-repeat;
            background-size: 550px;
            height: 1000px;
            display: none;
        }

        .center {
            display: block;
            margin-left: auto;
            margin-right: auto;
            width: 50%;
        }

        #ribbon .ribbon-left {
            width: 50%;
            float: left;
            margin-top: 96px;
            transition: all 3s;
            left: 0;
        }

        #ribbon .ribbon-right {
            width: 50%;
            float: right;
            margin-top: 100px;
            transition: all 3s;
            right: 0;
        }

        .siteinfo {
            text-align: center;
            width: 100% !important;
            color: #ebb358 !important;
            overflow: hidden;
            padding: 0 !important;
            font-weight: 600;
            margin: 0;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server" autocomplete="off">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div class="header">
            <div class="custom-container">
                <div class="logo">
                    <img src="/images/PnbLogoRed.png" alt="pnb logo">
                </div>
            </div>
        </div>
        <div class="limiter">
            <div class="container-login100">
                <div class="brand-block">
                    <div class="form-group" style="width: 100%; float: left;">
                        <div style="width: 30%; float: left;">
                            <img src="/images/pnbLogoLogin.jpeg" alt="pnb logo" style="display: inline-block; width: 140px;">
                        </div>
                        <div id="lblHeader" style="font-size: 40px; display: inline-block; font-weight: bold; text-align: center; width: 40%;">VIGILANCE MIS</div>
                        <div style="width: 30%; float: right;">
                        </div>
                    </div>
                    <%--<div style="font-size: 20px; text-align: center; color: #A30D3B; font-weight: bold;">“भ्रष्टाचार मुक्त भारत- विकसित भारत”- “Corruption free India for a developed Nation” &nbsp;</div>--%>
                    <div style="font-size: 20px; text-align: center; color: #A30D3B; font-weight: bold;">“भ्रष्टाचार का विरोध करें: राष्ट्र के प्रति समर्पित रहें”  “Say no to corruption; commit to the Nation” &nbsp;</div>
                    <div id="slider" style="width: 850px;">
                        <li>
                            <img src="/images/VAW_2023.JPG" style="align-self: center" /></li>                        
                    </div>
                    <div class="blink"><span style="font-size: 20px; font-family: arial; text-align: center; font-weight: bold; color: red; animation: blink 1s linear infinite;">HO: Vigilance Department, New Delhi</span></div>
                </div>
                <br />
                <br />
                <div class="login-block">
                    <asp:Login ID="Login1" runat="server" Font-Size="Small" OnLoggedIn="Login1_LoggedIn" OnAuthenticate="aspLogin_Authenticate"
                        OnLoggingIn="Login1_LoggingIn" Width="350">
                        <LayoutTemplate>
                            <div id="lblHeader" style="font-size: 25px; font-weight: bold; text-align: center; width: 100%;">LOGIN DETAILS</div>
                            <br />
                            <br />
                            <br />
                            <div class="wrap-input100 validate-input" data-validate="User ID is required">
                                <asp:TextBox ID="UserName" runat="server" AutoCompleteType="Disabled" placeholder="Please enter User ID" MaxLength="7"
                                    CssClass="txtbox" ToolTip="Enter User ID" Width="100%" Height="50px" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                <span class="focus-input100"></span>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                    ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="wrap-input100 validate-input" data-validate="Password is required">
                                <asp:TextBox ID="Password" runat="server" TextMode="Password" placeholder="Please Enter Password"
                                    CssClass="txtbox" ToolTip="Enter User Password" Width="100%" Height="50px"></asp:TextBox>
                                <span class="focus-input100"></span>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                    ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <asp:Label ID="resetlabel" runat="server" ForeColor="red" EnableViewState="False"></asp:Label>
                            </div>
                            <div>
                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                            </div>
                            <div>
                                <asp:Label ID="lblLocked" runat="server" ForeColor="red" Visible="false" Text="User Locked, please contact to Vigilance Administrator"></asp:Label>
                            </div>

                            <div class="container-login100-form-btn">
                                <div class="wrap-login100-form-btn">
                                    <div class="login100-form-bgbtn"></div>
                                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Let me in."
                                        ValidationGroup="Login1" OnClick="LoginButton_Click" class="btn btn-primary btn-block btn-large"
                                        Width="100px" ToolTip="if you have User id and Password please Login" OnClientClick="Encrypt()" />
                                </div>
                                <div class="wrap-login100-form-btn">
                                    <div class="login100-form-bgbtn"></div>
                                    <asp:Button ID="ResetButton" runat="server" OnClientClick="return confirm('Are you sure?');"
                                        CommandName="ResetPwd" ValidationGroup="ResetButton1" class="btn btn-primary btn-block btn-large"
                                        Width="135px" Text="Reset Password" OnClick="ResetButton_Click" ToolTip="Reset Password of Locked User and Password sent to user Emial id" />
                                </div>
                                <%--      <div class="dis-block txt3 hov1 p-r-30 p-t-10 p-b-10 p-l-30">
                                    <a href="javascript:void(0)" data-toggle="modal" onclick="Showpopup()" title="Password sent to Registered Email-ID"><i class="fa fa-key"></i>&nbsp;Forgot Password</a>
                                </div>--%>
                            </div>
                            <div class="container-login100-form-btn">
                                <div class="dis-block txt10 hov1 p-r-30 p-t-10 p-b-10 p-l-30">
                                    <a href="/images/manual.pdf" rel="noopener noreferrer" target="_blank" title="Download User Manual"><i class="fa fa-circle"></i>&nbsp;Download User Manual</a>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-lg-12 text-center">
                                    <asp:Label ID="lblStatus" runat="server" Style="text-align: center; color: darkred; font-size: medium;"></asp:Label>
                                    <asp:HiddenField ID="hdnKeyaes" runat="server" />
                                    <asp:HiddenField ID="hdnSalt" runat="server" />
                                </div>
                            </div>
                        </LayoutTemplate>
                    </asp:Login>
                </div>
                <div>
                    <div class="blink"><span style="font-size: 18px; font-family: arial; text-align: center; font-weight: bold; color: green; animation: blink 1s linear infinite;">PNB Celebrates: Vigilance Awareness Week – 2022(31st Oct 2022 – 06th Nov 2022) “सतर्क भारत, समृद्ध भारत” “Satark Bharat, Samriddh Bharat” “Vigilant India, Prosperous India”</span></div>
                    <marquee direction="left"><strong style="font-size: 18px; font-family: arial; text-align: center; font-weight: bold; color: darkblue;">PNB Celebrates: Vigilance Awareness Week – 2022 (31st Oct 2022 – 06th Nov 2022) “सतर्क भारत, समृद्ध भारत” “Satark Bharat, Samriddh Bharat” “Vigilant India, Prosperous India”</strong></marquee>
                </div>
            </div>
        </div>
        <footer class="navbar footer navbar-fixed-bottom visible-on-desktop">
            <small style="font-weight: bold; color:white">&copy; <%: DateTime.Now.Year %> - Designed & Developed By <a href="https://www.pnbindia.in/" style="color: #e8519e;" rel="noopener noreferrer" target="_blank">Punjab National Bank, Software Deptt., HO ITD</a></small>
            <p class="siteinfo">This Site is best viewed in <a href="Upload/IE.exe">IE9+</a> , <a href="Upload/ChromeStandaloneSetup64.exe">Chrome 30+</a>, <a href="Upload/Firefox Setup 57.0b14.exe">Mozilla 27+</a></p>
        </footer>
        <!-- Rest Password Modal -->
        <asp:Panel ID="pnlRestPasswordModal" runat="server">
            <div class="modal-dialog" role="document" id="divModal" runat="server">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="panel panel-danger" style="overflow: hidden;">
                                <div class="panel-heading bg-danger" style="font-weight: bold; color: white; font-size: 15px; background-color: #A30D3B;">Reset Password</div>
                                <div class="col-sm-12 panel-body" style="background-image: url('/images/passwordreset.jpg'); background-repeat: no-repeat;">
                                    <div class="form-group">
                                    </div>
                                    <br />
                                    <div class="form-group" style="height: 200px; padding-top: 135px;">
                                        <div class="col-sm-2">
                                        </div>
                                        <div class="col-sm-2">
                                        </div>
                                        <div class="col-sm-2">
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="input-group input-icon right">
                                                <span class="input-group-addon"><i class="fa fa-pencil"></i></span>
                                                <asp:TextBox ID="txtUserID_RESET" runat="server" placeholder="Enter user id" MaxLength="6" CssClass="form-control" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Label ID="lblMsgResetPassword" runat="server" CssClass="label label-primary"></asp:Label>
                        <button type="button" id="closeResetPassword" class="btn btn-warning" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!-- Vigilance Week Modal -->
        <asp:Panel ID="pnlVigilanceWeekModal" runat="server">
            <div class="modal-dialog" role="document" style="width: 90%;">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="form-group">
                            <img src="/images/Vig week.jpeg" alt="Vigilance week" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="closeVigilanceWeek" class="btn btn-warning" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <!-- End Rest Password Modal-->
        <act:ModalPopupExtender ID="modalRestPassword" runat="server" BehaviorID="modalRestPassword" DynamicServicePath="" PopupControlID="pnlRestPasswordModal" CancelControlID="closeResetPassword" TargetControlID="btn" Enabled="true">
        </act:ModalPopupExtender>
        <!-- End Rest Password Modal-->
        <act:ModalPopupExtender ID="modalVigilanceWeek" runat="server" BehaviorID="modalVigilanceWeek" DynamicServicePath="" PopupControlID="pnlVigilanceWeekModal" CancelControlID="closeVigilanceWeek" TargetControlID="btn" Enabled="true">
        </act:ModalPopupExtender>
        <div id="ribbon">
            <div class="ribbon-left">
                <img src="images/ribbon-left.png" alt="left Ribbon" />
            </div>
            <div class="ribbon-right">
                <img src="images/ribbon-right.png" alt="Right Ribbon" />
            </div>
        </div>
        <div style="display: none">
            <asp:Button ID="btn2" runat="server" Text="Button" />
            <asp:Button ID="btn" runat="server" Text="Button" />
        </div>
        <asp:HiddenField ID="hiddenforcelogin" runat="server" Value="0" />
        <asp:HiddenField ID="hidUniqueID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hidPassword" runat="server" ClientIDMode="Static" />
        <!--===============================================================================================-->
        <script src="/js/jquery-3.4.1.min.js"></script>
        <script src="/vendor/bootstrap/js/popper.js"></script>
        <script src="/vendor/bootstrap/js/bootstrap.min.js"></script>
        <script src="/js/main.js"></script>
    </form>
    <script src="/js/aes.js"></script>
    <script type="text/javascript">
        function Showpopup() {
            document.getElementById("txtUserID_RESET").value = "";
            document.getElementById("lblMsgResetPassword").innerHTML = "";

            $find("modalRestPassword").show();
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $('#divModal').modal('hide');
        });
    </script>
    <script type="text/javascript">
        function blockSpecialChar(e) {
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
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

    <script type="text/javascript">
        function EncryptInputs() {
            var orignalUsername = $("#txtUserID").val().trim();
            var orignalPassword = $("#txtPassword").val();
            var validatestatus = 'Y';

            if (orignalUsername === '') {
                alert('Username is required');
                validatestatus = 'N';
                return false;
            }
            if (orignalPassword === '') {
                alert('Password is required');
                validatestatus = 'N';
                return false;
            }

            if (validatestatus = 'Y') {
                if ($.isNumeric(orignalUsername)) {
                    var key = CryptoJS.enc.Utf8.parse($("#hdnKeyaes").val());
                    var iv = CryptoJS.enc.Utf8.parse($("#hdnSalt").val());
                    var username = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(orignalUsername), key,
                        {
                            keySize: 128 / 8,
                            iv: iv,
                            mode: CryptoJS.mode.CBC,
                            padding: CryptoJS.pad.Pkcs7
                        });
                    var password = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(orignalPassword), key,
                        {
                            keySize: 128 / 8,
                            iv: iv,
                            mode: CryptoJS.mode.CBC,
                            padding: CryptoJS.pad.Pkcs7
                        });

                    //alert(username);

                    $("#txtUserID").val(username);
                    $("#txtPassword").val(password);
                }
                else {
                    $("#txtUserID").val('');
                    $("#txtPassword").val('');
                }
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function ($) {

            $('#checkbox').change(function () {
                setInterval(function () {
                    moveRight();
                }, 3000);
            });

            var slideCount = $('#slider ul li').length;
            var slideWidth = $('#slider ul li').width();
            var slideHeight = $('#slider ul li').height();
            var sliderUlWidth = slideCount * slideWidth;

            $('#slider').css({ width: slideWidth, height: slideHeight });

            $('#slider ul').css({ width: sliderUlWidth, marginLeft: - slideWidth });

            $('#slider ul li:last-child').prependTo('#slider ul');

            function moveLeft() {
                $('#slider ul').animate({
                    left: + slideWidth
                }, 200, function () {
                    $('#slider ul li:last-child').prependTo('#slider ul');
                    $('#slider ul').css('left', '');
                });
            };

            function moveRight() {
                $('#slider ul').animate({
                    left: - slideWidth
                }, 200, function () {
                    $('#slider ul li:first-child').appendTo('#slider ul');
                    $('#slider ul').css('left', '');
                });
            };

            $('a.control_prev').click(function () {
                moveLeft();
            });

            $('a.control_next').click(function () {
                moveRight();
            });

        });
    </script>

    <script type="text/javascript">
        // Ribbon JS Start
        $('#ribbon').click(function () {
            $(".ribbon-left").animate({ 'margin-left': '-50%' }, 500);
            $(".ribbon-right").animate({ 'margin-right': '-50%' }, 500);
            $(this).css('cursor', 'inherit');
            $(".cursor").css('display', 'none');
            setTimeout(function () {
                $(this).css('background', 'rgba(0,0,0, 0)');
                $('#ribbon').fadeOut();
            }, 3500);
        });
        $(document).ready(function (event) {
            $("#ribbon").append("<div class='cursor'></div>");

            var mouseX = event.pageX;
            var mouseY = event.pageY;
            var windowWidth = $(window).width();
            var windowHeight = $(window).height();

            $(this).on("mousemove", function (event) {

                speed = 30;

                mouseX = event.pageX;
                mouseY = event.pageY;

                percentX = ((mouseX / windowWidth) * speed) - (speed / 0.75);
                percentY = ((mouseY / windowHeight) * speed) - (speed / 0.6);
                stringX = (0 - percentX - speed) + "%";
                stringY = (0 - percentY - speed) + "%";

                percentCX = ((mouseX / windowWidth) * speed) - (speed / 30);
                percentCY = ((mouseY / windowHeight) * speed) - (speed / 30);
                stringCX = (0 - percentCX - speed) + "%";
                stringCY = (0 - percentCY - speed) + "%";

                $(".cursor").css({
                    "-webkit-transform": "translateX(" + mouseX + "px) translateY(" + mouseY + "px)",
                    "-moz-transform": "translateX(" + mouseX + "px) translateY(" + mouseY + "px)",
                    "transform": "translateX(" + mouseX + "px) translateY(" + mouseY + "px)",
                });
            });
        });
        // Ribbon JS End
    </script>
</body>
</html>
