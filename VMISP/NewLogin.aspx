<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewLogin.aspx.cs" Inherits="VMISP.NewLogin" %>

<!DOCTYPE html>

<html lang="en">
<!--<![endif]-->
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0,maximum-scale=1.0, user-scalable=no">
    <title>EDI</title>

    <link rel="shortcut icon" href="/images/favicon.ico" type="image/x-icon">
    <link href="/css/Login.css" rel="stylesheet">
    <link href="/css/Login1.css" rel="stylesheet">
</head>
<body class="pace-top bg-white pace-done">
    <div class="pace  pace-inactive">
        <div class="pace-progress" data-progress-text="100%" data-progress="99" style="width: 100%;">
            <div class="pace-progress-inner"></div>
        </div>
        <div class="pace-activity"></div>
    </div>
    <!-- begin #page-loader -->
    <div id="page-loader" class="fade in hide"><span class="spinner"></span></div>
    <!-- end #page-loader -->
    <!-- begin #page-container -->
    <div id="page-container" class="fade in">



        <!-- begin login -->
        <div class="login login-with-news-feed">
            <!-- begin news-feed -->
            <div class="news-feed">
                <div class="news-image">
                    <img src="/images/Cover.png" data-id="login-cover-image" alt="">
                </div>
                <div class="news-caption">
                    <div class="brand">
                        <div class="pull-left">
                            <img style="max-width: 5em; margin-top: -1em;" src="/images/pnbLogo.jpg">
                        </div>
                        <div>
                            <h4 class="caption-title"><i class="text-success"></i>&nbsp; Punjab National Bank</h4>
                            <p>
                                &nbsp;...the name you can Bank upon !
                   
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <!-- end news-feed -->
            <!-- begin right-content -->
            <div class="right-content">
                <!-- begin login-header -->
                <div class="login-header">
                    <div class="brand">
                        <div class="pull-left">
                            <img style="max-width: 2em; margin-top: -.4em;" src="/images/pnbLogo.jpg">
                        </div>
                        <div>
                            &nbsp; VMIS- Portal
               
                        </div>
                    </div>
                    <div class="icon">
                        <i class="fa fa-sign-in"></i>
                    </div>
                </div>
                <!-- end login-header -->
                <!-- begin login-content -->
                <div class="login-content">
                    <form action="/EDI/Account/Login" class="margin-bottom-0" enctype="multipart/form-data" id="frmLogin" method="post" novalidate="novalidate">
                        <div class="form-group m-b-15">
                            <input value="" autocomplete="off" class="form-control" data-val="true" data-val-number="The field PFNumber must be a number." data-val-regex="Please enter valid PF Number" data-val-regex-pattern="^[0-9]+$" data-val-required="Please Enter the PF Number." id="txtPFNo" name="PFNumber" placeholder="PF Number" required="" type="text">
                        </div>
                        <div class="form-group m-b-15">
                            <input class="form-control" data-val="true" data-val-required="Password is required." id="txtPassword" name="Password" placeholder="Password" required="" type="password">
                        </div>
                        <div class="form-group">
                            <h5>
                                <label id="lblLoginError" class="text-danger"></label>
                                <span class="field-validation-valid label label-danger" data-valmsg-for="PFNumber" data-valmsg-replace="true"></span>
                                <span class="field-validation-valid label label-danger" data-valmsg-for="Password" data-valmsg-replace="true"></span>
                            </h5>
                        </div>
                        <div class="row row-space-10">
                            <div class="col-md-6 m-b-15">
                                <div class="login-buttons">
                                    <button type="submit" class="btn btn-info btn-block" id="btnSubmit" onclick="return encryptPassword();">SignIn &gt;&gt;</button>
                                </div>
                            </div>
                            <div class="col-md-6 m-b-15">
                                <div class="login-buttons">
                                    <button type="reset" class="btn btn-warning btn-block">Reset &gt;&gt;</button>
                                </div>
                            </div>

                        </div>
                        <div class="m-t-20 m-b-40 p-b-40 text-inverse">
                            Forget/Reset Password ? Click <a href="/EDI/Account/ForgetPassword" class="text-danger">here</a>

                        </div>
                        <div class="text-danger">
                            <div class="validation-summary-valid" data-valmsg-summary="true">
                                <ul>
                                    <li style="display: none"></li>
                                </ul>
                            </div>
                        </div>
                        <hr>
                        <p class="text-center">
                            © Punjab National Bank All Right Reserved 2022
               
                        </p>
                        <input id="HDRandomSeed" name="HDRandomSeed" type="hidden" value="BF135D1A6C0CAAFD89BE1CE6757FCE31">
                    </form>
                </div>
                <!-- end login-content -->
            </div>
            <!-- end right-container -->
        </div>
        <!-- end login -->


    </div>
    <!-- end page container -->
    <script src="/EDI/bundles/jquery?v=JL596WEzEYSLK79KRL4It4N63VXpRlW4A824KHlhVLc1"></script>

    <script src="/EDI/bundles/bootstrap?v=xlqPrbAYYPiJhyG6y4CfYRcrNxVPrNYmpta99TjjMnM1"></script>

    <script src="/EDI/bundles/jqueryval?v=UCY_KgmM5Jly7K9-xbZnr7FyGohRYMKV-OAi_bPc32I1"></script>


    <script src="/EDI/Scripts/aes.js"></script>
    <script type="text/javascript">
        function encryptPassword() {
            var orignalUsername = $("#txtPFNo").val().trim();
            var orignalPassword = $("#txtPassword").val();
            if (orignalUsername === '') {
                alert('Username is required');
                return false;
            }
            if (orignalPassword === '') {
                alert('Password is required');
                return false;
            }
            if ($.isNumeric(orignalUsername)) {
                var key = CryptoJS.enc.Utf8.parse('€-v£d@A+Pnb~!');
                var iv = CryptoJS.enc.Utf8.parse('8786858483828180');
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
                $("#txtPassword").val(password);
                return true;
            }
            else {
                alert('PF number should be a numeric value');
                $("#txtPFNo").val('');
                return false;
            }
            return false;
        }
    </script>


    <script>
        $(document).ready(function () {
            App.init();

            $(this).bind("contextmenu", function (e) {
                e.preventDefault();
            });
        });
    </script>


</body>
</html>
