<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmComplaintUpdate.aspx.cs" Inherits="VMISP.Mis.frmComplaintUpdate"
    ValidateRequest="false" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <script src="/Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="/Js/JS_CommonValidation.js" type="text/javascript"></script>
    <script src="/Js/jquery-3.3.1.min.js" type="text/javascript"></script>
    <script src="/Js/jquery.min.js"></script>

    <script src="/Js/jquery-1.9.1.js"></script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <link href="/Js/jquery-ui.css" rel="stylesheet" />
    <link href="/css/bootstrap.css" rel="stylesheet" />

    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="col-lg-12">
                <div class="form-group row">
                    <div class="panel panel-primary">
                        <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                            Compalint Details Update 
                        </div>
                        <br />
                        <div class="col-sm-12 alert alert-dark">
                            <div class="form-group row" style="padding-right: 5px;">
                                <div class="col-sm-3">
                                    <label for="txtRNo"><span style="color: #FF0000">*</span>Complaint No</label>
                                    <asp:TextBox ID="txtRNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                                <div class="col-sm-3">
                                    <label for="ddlField"><span style="color: #FF0000">*</span>Field</label>
                                    <asp:DropDownList ID="ddlField" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlField_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="Select" Value="SELECT"></asp:ListItem>
                                        <asp:ListItem Text="Circle Office" Value="CIRCLE"></asp:ListItem>
                                        <asp:ListItem Text="Sent To" Value="SENTTO"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-3">
                                    <asp:Label ID="lblFieldName" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><asp:Label ID="lblValueCaption" runat="server" class="lblCaption"></asp:Label>
                                    <asp:HiddenField ID="hidColumnDataType" runat="server" />
                                    <asp:DropDownList ID="ddlCircleOffice" runat="server" CssClass="form-control input-sm" Visible="false">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtSentTo" runat="server" CssClass="form-control date input-sm" Visible="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row" style="padding-right: 5px;">
                                <div class="col-sm-3">
                                </div>
                                <div class="col-sm-9">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClick="btnSubmit_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-warning btn-sm" OnClick="btnCancel_Click" />&nbsp;&nbsp;
                                    <asp:Label ID="lblMsg" runat="server" CssClass="lblMsg"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        jQuery.browser = {};
        (function () {
            jQuery.browser.msie = false;
            jQuery.browser.version = 0;
            if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
                jQuery.browser.msie = true;
                jQuery.browser.version = RegExp.$1;
            }
        })();
        $(function () {
            $('.date').datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                yearRange: '2000:2100',
                buttonImageOnly: true,
                maxDate: new Date(),
            }
            );
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $('.date').datepicker({
                    dateFormat: 'dd/mm/yy',
                    changeMonth: true,
                    changeYear: true,
                    yearRange: '2000:2100',
                    buttonImageOnly: true,
                    maxDate: new Date(),
                })
            }
        });
    </script>
</asp:Content>
