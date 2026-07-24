<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="PenaltyCharge.aspx.cs" Inherits="VMISP.Mis.PenaltyCharge" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style>
        .ajax__tab_xp .ajax__tab_tab {
            background: #00bcd4 !important;
            color: #fff !important;
            /*border-radius: 10px;*/
            font-size: 16px;
            font-weight: bold;
            height: 32px !important;
            padding: 5px;
        }

        .ajax__tab_xp .ajax__tab_active .ajax__tab_outer, .ajax__tab_xp .ajax__tab_inner, .ajax__tab_xp .ajax__tab_outer {
            background: none !important;
        }

        .ajax__tab_xp .ajax__tab_active .ajax__tab_tab {
            background: maroon !important;
        }

        .hideBranchScoreMarks {
            display: none;
        }
    </style>
</asp:Content>
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
                            Penalty not Commensurate with Charges Details
                        </div>
                        <br />
                        <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="100%">
                            <act:TabPanel ID="tabEntry" runat="server">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEntryHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="Entry" ToolTip="Lodi Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtPNCNo"><span style="color: #FF0000">*</span>PNC No</label>
                                                <asp:TextBox ID="txtPNCNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                <asp:HiddenField ID="hidUniqueID" runat="server" />
                                                <asp:HiddenField ID="hidUserRole" runat="server" />
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtVigCaseNo"><span style="color: #FF0000">*</span>Vig Case No</label>
                                                <asp:TextBox ID="txtVigCaseNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="txtEOName"><span style="color: #FF0000">*</span>Name of Erring Official</label>
                                                <asp:TextBox ID="txtEOName" runat="server" CssClass="form-control input-sm" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtDesignation"><span style="color: #FF0000">*</span>Designation of EO</label>
                                                <asp:TextBox ID="txtEODesignation" runat="server" CssClass="form-control input-sm" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtChargeSheetDate"><span style="color: #FF0000">*</span>Charge Sheet Date</label>
                                                <asp:TextBox ID="txtChargeSheetDate" runat="server" CssClass="form-control input-sm date"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="txtNatureOfChargeSheet"><span style="color: #FF0000">*</span>Nature of Charge Sheet</label>
                                                <asp:TextBox ID="txtNatureOfChargeSheet" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtLapsesCharges"><span style="color: #FF0000">*</span>Lapses/Charges</label>
                                                <asp:TextBox ID="txtLapsesCharges" runat="server" CssClass="form-control input-sm" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="txtAccountName">Account Name</label>
                                                <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control input-sm" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtFinalOrderDate">Date of Final Order</label>
                                                <asp:TextBox ID="txtFinalOrderDate" runat="server" CssClass="form-control input-sm date"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtPenaltyImposed">Penalty Imposed</label>
                                                <asp:TextBox ID="txtPenaltyImposed" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="txtNameOfDA">Name of DA</label>
                                                <asp:TextBox ID="txtNameOfDA" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDesignationOfDA">Designation of DA</label>
                                                <asp:TextBox ID="txtDesignationOfDA" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtPFNumberOfDA">PF Number of DA</label>
                                                <asp:TextBox ID="txtPFNumberOfDA" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtPostingOfDA">Place of Posting of DA</label>
                                                <asp:TextBox ID="txtPostingOfDA" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="ddlStatus">Status</label>
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-6">
                                                <label for="ddlZone"><span style="color: #FF0000">*</span>Zone</label>
                                                <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="ddlCircle"><span style="color: #FF0000">*</span>Circle</label>
                                                <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                            </div>
                                            <div class="col-sm-9">
                                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary btn-sm" Text="Submit" OnClick="btnSubmit_Click" />
                                                <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary btn-sm" Text="Update" Visible="false" OnClick="btnUpdate_Click" />
                                                <asp:Button ID="btnReset" runat="server" CssClass="btn btn-warning btn-sm" Text="Reset" OnClick="btnReset_Click" />
                                                <asp:Label ID="lblMsg" runat="server" CssClass="label label-success" Font-Size="Medium"></asp:Label>
                                            </div>
                                        </div>
                                        <ul class="bottom_notes">
                                            <li><span style="color: #FF0000">*</span> marked fields are mandatory</li>
                                        </ul>
                                    </div>
                                </ContentTemplate>
                            </act:TabPanel>
                            <act:TabPanel ID="tabList" runat="server" HeaderText="Entry Details">
                                <HeaderTemplate>
                                    <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="Entry Details" ToolTip="Penalty not Commensurate with Charges Entry Details"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row">
                                            <div class="col-sm-2">
                                                <label for="txtPNCNO_LIST">PNC Number</label>
                                                <asp:TextBox ID="txtPNCNO_LIST" runat="server" CssClass="form-control input-sm" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <label for="txtVigCaseNo_LIST">Vig Case No</label>
                                                <asp:TextBox ID="txtVigCaseNo_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <label for="txtPFNoOfDA_LIST">PF Number of DA</label>
                                                <asp:TextBox ID="txtPFNoOfDA_LIST" runat="server" CssClass="form-control input-sm" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1">
                                                <label for="btnSearch">&nbsp;&nbsp;</label>
                                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-info btn-sm" Text="Search" OnClick="btnSearch_Click" />
                                            </div>
                                            <div class="col-sm-5" style="padding-top: 27px;">
                                                <label for="lblMsgSearch">&nbsp;&nbsp;</label>
                                                <asp:Label ID="lblMsgSearch" runat="server" CssClass="label label-danger"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="margin-right: 0px;">
                                            <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="false" CssClass="table input-sm table-bordered table-condensed" OnRowCommand="gvDetails_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Select
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnView" runat="server" CausesValidation="false" CommandName="View" CssClass="btn btn-sm btn-danger"
                                                                CommandArgument='<%#Eval("UNIQUEID")%>' Text="Edit" />
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="col-sm-1" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PNCNO" HeaderText="PNC No" />
                                                    <asp:BoundField DataField="VIGCASENO" HeaderText="Vig Case No" />
                                                    <asp:BoundField DataField="PFNO" HeaderText="PF Number" />
                                                    <asp:BoundField DataField="NAME" HeaderText="Name" />
                                                    <asp:BoundField DataField="ADDDATE" HeaderText="Entry Date" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </act:TabPanel>
                        </act:TabContainer>
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
                })
            }
        });
    </script>
</asp:Content>
