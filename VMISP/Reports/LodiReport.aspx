<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="LodiReport.aspx.cs" Inherits="VMISP.Reports.LodiReport" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <script src="/Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="/Js/JS_CommonValidation.js" type="text/javascript"></script>
    <script src="/Js/jquery-3.3.1.min.js" type="text/javascript"></script>
    <script src="/Js/jquery.min.js"></script>

    <script src="/Js/jquery-1.9.1.js"></script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <link href="/Js/jquery-ui.css" rel="stylesheet" />
    <link href="/css/bootstrap.css" rel="stylesheet" />
    <div class="col-lg-12">
        <div class="form-group row">
            <div class="panel panel-primary">
                <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                    Lodi Details Enquiry
                </div>
                <br />
                <div class="col-sm-12 alert alert-dark">
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-2">
                            <label for="txtPFNumber">PF Number</label>
                            <asp:TextBox ID="txtPFNumber" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <label for="txtLodiNumber">Lodi Number</label>
                            <asp:TextBox ID="txtLodiNumber" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <label for="txtVigCaseNo">Vig Case No</label>
                            <asp:TextBox ID="txtVigCaseNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <label for="txtFromDate">From Date</label>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <label for="txtToDate"><span style="color: #FF0000">*</span>To Date</label>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                        </div>
                        <div class="col-sm-1">
                            <asp:Button ID="btnGetDetails" runat="server" CssClass="btn btn-info btn-sm" Text="Get >>" OnClick="btnGetDetails_Click" />
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
                        </div>

                    </div>
                    <br />
                    <asp:Panel ID="pnlDetails" runat="server" GroupingText="Lodi Enquiry Details">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvDetails" runat="server" CssClass="table input-sm table-bordered table-condensed" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="Row No" HeaderText="Row No" />
                                    <asp:BoundField DataField="Lodi No" HeaderText="Lodi No" />
                                    <asp:BoundField DataField="Lodi As on Date" HeaderText="Lodi As on Date" />
                                    <asp:BoundField DataField="Vig Case No" HeaderText="Vig Case No" />
                                    <asp:BoundField DataField="PF Number" HeaderText="PF Number" />
                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:BoundField DataField="Entry Date" HeaderText="Entry Date" />
                                </Columns>
                            </asp:GridView>
                            <span class="input-sm" style="color: maroon;" id="lastUpdated" runat="server" visible="false">Last Updated &nbsp; <%: DateTime.Now %></span>
                            <div class="form-group">
                                <div class="col-sm-10">
                                </div>
                                <div class="col-sm-2">
                                    <asp:Button ID="btnExcelDownload" ToolTip="Download Lodi Enquiry Details" runat="server" CssClass="btn btn-sm btn-warning" Visible="false" Text="Download" OnClick="btnExcelDownload_Click" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
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
                maxDate: new Date(),
            }
            );
        });
    </script>
</asp:Content>
