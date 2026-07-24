<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="RetirementCases.aspx.cs" Inherits="VMISP.Search.RetirementCases" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <script src="/Js/jquery-3.3.1.min.js" type="text/javascript"></script>
    <script src="/Js/jquery.min.js"></script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <link href="/Js/jquery-ui.css" rel="stylesheet" />
    <link href="/css/bootstrap.css" rel="stylesheet" />
    <div class="col-lg-12">
        <div class="form-group row">
            <div class="panel panel-primary">
                <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                    Search Retirement Cases
                </div>
                <br />
                <div class="col-sm-12 alert alert-dark">
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-2">
                            <label for="txtFromDate"><span style="color: #FF0000">*</span>From Date</label>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <label for="txtToDate"><span style="color: #FF0000">*</span>To Date</label>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                        </div>
                        <div class="col-sm-2" style="margin-top: 25px;">
                            <asp:Button ID="btnGetDetails" runat="server" OnClick="btnGetDetails_Click" CssClass="btn btn-sm btn-info" Text="Get Details" />
                        </div>
                        <div class="col-sm-6" style="margin-top: 25px;">
                            <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <asp:Panel ID="pnlDetails" runat="server" GroupingText="Retirement Cases Details">
                            <div class="col-sm-12">
                                <asp:GridView ID="gvDetails" runat="server" CssClass="table input-sm table-bordered table-condensed" AutoGenerateColumns="true">
                                    <Columns>
                                        <asp:BoundField DataField="Row No" HeaderText="Row No" />
                                        <asp:BoundField DataField="R No" HeaderText="S/R No" />
                                        <asp:BoundField DataField="Date" HeaderText="Date" />
                                        <asp:BoundField DataField="Retirement Date" HeaderText="Retirement Date" />
                                        <asp:BoundField DataField="Zone" HeaderText="Zone" />
                                        <asp:BoundField DataField="Circle" HeaderText="Circle" />
                                        <asp:BoundField DataField="Name" HeaderText="Name" />
                                        <asp:BoundField DataField="PF Number" HeaderText="PF Number" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:BoundField DataField="Status Code" HeaderText="Status Code" />
                                        <asp:BoundField DataField="Desk User Remarks" HeaderText="Desk User Remarks" />
                                        <asp:BoundField DataField="Table Name" HeaderText="Table Name" />
                                    </Columns>
                                    <HeaderStyle BackColor="#00bcd4" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-10"></div>
                        <div class="col-sm-2">
                            <asp:Button ID="btnExcelDownload" ToolTip="Download Retirement Cases Details" runat="server" CssClass="btn btn-sm btn-warning" Visible="false" Text="Download" OnClick="btnExcelDownload_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
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
            });
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
                })
            }
        });
    </script>
</asp:Content>
