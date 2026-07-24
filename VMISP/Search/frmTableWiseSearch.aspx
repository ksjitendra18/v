<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmTableWiseSearch.aspx.cs" Inherits="VMISP.Search.frmTableWiseSearch" %>

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
                    Form Wise Search
                </div>
                <br />
                <div class="col-sm-12 alert alert-dark">
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="ddlTableName"><span style="color: #FF0000">*</span>Form Name </label>
                            <asp:DropDownList ID="ddlTableName" runat="server" CssClass="form-control input-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlTableName_SelectedIndexChanged">
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Complaint" Value="COMPLAINT"></asp:ListItem>
                                <asp:ListItem Text="Complaint EO Details" Value="COMPLAINT_EO_DETAILS"></asp:ListItem>
                                <asp:ListItem Text="IAC Entry" Value="IAC"></asp:ListItem>
                                <asp:ListItem Text="Lodi Entry" Value="LODI"></asp:ListItem>
                                <asp:ListItem Text="MISC" Value="MISC_EO_DETAILS"></asp:ListItem>
                                <asp:ListItem Text="MISC EO Details" Value="MISC_EO_DETAILS"></asp:ListItem>
                                <asp:ListItem Text="NOC" Value="NOC"></asp:ListItem>
                                <asp:ListItem Text="Operational Ref" Value="OPERATIONALREF"></asp:ListItem>
                                <asp:ListItem Text="RRB" Value="RRB"></asp:ListItem>
                                <asp:ListItem Text="RTI" Value="RTI"></asp:ListItem>
                                <asp:ListItem Text="SR" Value="SR"></asp:ListItem>
                                <asp:ListItem Text="SANCTION" Value="SANCTION"></asp:ListItem>
                                <asp:ListItem Text="Sanction for Investigation" Value="SANCTION_FOR_INVESTIGATION"></asp:ListItem>
                                <asp:ListItem Text="Sanction for Prosecution" Value="SANCTION_FOR_PROSECUTION"></asp:ListItem>
                                <asp:ListItem Text="Vigilance" Value="VIGILANCE"></asp:ListItem>
                                <asp:ListItem Text="Whistle Blower" Value="WB"></asp:ListItem>
                                <asp:ListItem Text="Vigilance Monitoring" Value="VIGILANCEMIS"></asp:ListItem>
                                 <asp:ListItem Text="ABBFF" Value="ABBFF"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-2">
                            <label for="chkShowAllData">Show all Data </label>
                            <br />
                            <asp:CheckBox ID="chkShowAllData" runat="server" />
                        </div>
                        <div class="col-sm-2">
                            <label for="ddlColumnName">Form Fields </label>
                            <asp:DropDownList ID="ddlColumnName" runat="server" CssClass="form-control input-sm">
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-2" style="display: none;" id="divText" runat="server">
                            <label for="txtEnterValue">Enter Value</label>
                            <asp:TextBox ID="txtEnterValue" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                        </div>
                        <div class="col-sm-2" style="display: none;" id="divDate" runat="server">
                            <label for="txtEnterDate">Enter Date</label>
                            <asp:TextBox ID="txtEnterDate" runat="server" CssClass="form-control input-sm date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3"></div>
                        <div class="col-sm-1">
                            <asp:Button ID="btnGet" runat="server" OnClick="btnGet_Click" Text="Search" CssClass="btn btn-info btn-sm" />
                        </div>
                        <div class="col-sm-1">
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExel_Click" Text="Excel" CssClass="btn btn-primary btn-sm" Visible="false" />
                        </div>
                        <div class="col-sm-1">
                            <asp:Button ID="btnPDF" runat="server" OnClick="btnPdf_Click" Text="PDF" CssClass="btn btn-success btn-sm" Visible="false" />
                        </div>
                        <div class="col-sm-6">
                            <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
                            <asp:HiddenField ID="hidColumnDataType" runat="server" />
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvMain" runat="server" OnRowDataBound="gvMain_RowDataBound" CssClass="table input-sm table-bordered table-condensed">
                                <Columns>
                                    <asp:BoundField DataField="ROWNO" HeaderText="S No." SortExpression="ROWNO" />
                                    <asp:BoundField DataField="RNO" HeaderText="Number" SortExpression="RNO" />
                                    <asp:BoundField DataField="NAME" HeaderText="Name" SortExpression="NAME" />
                                    <asp:BoundField DataField="COMPRECDATE" HeaderText="Date" SortExpression="COMPRECDATE" />
                                    <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" SortExpression="CIRCLEOFFICE" />
                                    <asp:BoundField DataField="BRANCH" HeaderText="Branch" SortExpression="BRANCH" />
                                    <asp:BoundField DataField="STATUS" HeaderText="Status" SortExpression="STATUS" />
                                </Columns>
                            </asp:GridView>
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
                maxDate: new Date(),
            }
            );
        });
    </script>
</asp:Content>
