<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="SR.aspx.cs" Inherits="VMISP.Mis.SR" %>

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
    <script src="/Js/JS_CommonValidation.js" type="text/javascript"></script>
    <script src="/Js/jquery-3.3.1.min.js" type="text/javascript"></script>
    <script src="/Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="/Js/jquery.min.js"></script>
    <script src="/Js/jquery-1.9.1.js"></script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <link href="/Js/jquery-ui.css" rel="stylesheet" />
    <link href="/css/bootstrap.css" rel="stylesheet" />
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="col-lg-12">
                <div class="form-group row">
                    <div class="panel panel-primary">
                        <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                            SR Entry
                        </div>
                        <br />
                        <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="100%" OnActiveTabChanged="tabMain_ActiveTabChanged"
                            AutoPostBack="true">
                            <act:TabPanel ID="tabEntry" runat="server">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEntryHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="Entry" ToolTip="SR Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-2">
                                                <label for="txtSRNo"><span style="color: #FF0000">*</span>SR Number</label>
                                                <asp:TextBox ID="txtSRNo" runat="server" CssClass="form-control input-sm" placeHolder="SR Number" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1" style="padding-top: 23px;">
                                                <asp:Button ID="btnGet" runat="server" OnClick="btnGet_Click" ToolTip="SR Search" CssClass="btn btn-sm btn-info" Text="Search"></asp:Button>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtSRDate"><span style="color: #FF0000">*</span>SR Date</label>
                                                <asp:TextBox ID="txtSRDate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 input-group">
                                                <label for="txtBRComplaint"><span style="color: #FF0000">*</span>Branch</label>
                                                <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtReportDate">Circle Office</label>
                                                <asp:DropDownList ID="ddlCircleOffice" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtRNo">R Number</label>
                                                <asp:TextBox ID="txtRNo" runat="server" CssClass="form-control input-sm" placeHolder="R Number" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="chkClosureDate">Closure Date</label>
                                                <asp:CheckBox ID="chkClosureDate" runat="server" Checked="false" />
                                                <asp:Label ID="lblClosureDate" runat="server" CssClass="lblCaption"></asp:Label>
                                                <asp:Panel ID="pnlClosureDate" runat="server" Visible="false">
                                                    <asp:TextBox ID="txtClosureDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtAccused">Accused</label>
                                                <asp:TextBox ID="txtAccused" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtReportDate">Allegations</label>
                                                <asp:TextBox ID="txtAllegations" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtAmount">Amount</label>
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control input-sm" Style="text-align: right"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtCompRecDate">Received Date</label>
                                                <asp:TextBox ID="txtCompRecDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtFinalAction">Final Action</label>
                                                <asp:TextBox ID="txtFinalAction" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlZone">Zone</label>
                                                <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="ddlStatusCode">Status Code</label>
                                                <asp:DropDownList ID="ddlStatusCode" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                <asp:Label ID="lblStatusCodeMIS" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtRegion">Region</label>
                                                <asp:TextBox ID="txtRegion" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtPresentPosting">Present Posting</label>
                                                <asp:TextBox ID="txtPresentPosting" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtAccountName">Account Name</label>
                                                <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtClose">Close</label>
                                                <asp:TextBox ID="txtClose" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlNature">Nature</label>
                                                <asp:DropDownList ID="ddlNature" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                <asp:Panel ID="pnlNatureMIS" runat="server" Visible="false">
                                                    <asp:Label ID="lblNatureMIS" runat="server"></asp:Label>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDesignation">Designation</label>
                                                <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtInvestigation">Investigation</label>
                                                <asp:TextBox ID="txtInvestigation" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtLetterSentDate">Letter Sent Date</label>
                                                <asp:TextBox ID="txtLetterSentDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlLetterSentTo">Letter Sent To</label>
                                                <asp:DropDownList ID="ddlLetterSentTo" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtReminderDate">Reminder Date</label>
                                                <asp:TextBox ID="txtReminderDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtReplyReceivedDate">Reply Received Date</label>
                                                <asp:TextBox ID="txtReplyReceivedDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-2">
                                                <label for="ddlBankName">Bank Name</label>
                                                <asp:DropDownList ID="ddlBankName" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                    <asp:ListItem Value="PNB" Text="PNB"></asp:ListItem>
                                                    <asp:ListItem Value="OBC" Text="OBC"></asp:ListItem>
                                                    <asp:ListItem Value="UBI" Text="UBI"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-5">
                                                <label for="ddlZoneNew">New Zone</label>
                                                <asp:DropDownList ID="ddlZoneNew" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlZoneNew_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-5">
                                                <label for="txtReminderDate">New Circle</label>
                                                <asp:DropDownList ID="ddlCircleNew" runat="server" CssClass="form-control input-sm"></asp:DropDownList></td>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <label for="txtStatus"><span style="color: #FF0000">*</span>Status</label>
                                                <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <asp:Panel ID="pnlHOStatus" runat="server" Visible="False">
                                                    <span class="lblCaption">HO Status :</span>
                                                    <asp:TextBox ID="txtHOStatus" runat="server" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <label for="txtDealingOfficerRemarks">Dealing Officer Remarks</label>
                                                <asp:TextBox ID="txtDealingOfficerRemarks" runat="server" placeholder="Enter Dealing Officer Remarks, If Any...." TextMode="MultiLine" onkeypress="return blockSpecialChar(event)" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                        </div>
                                        <div class="col-sm-9">
                                            <asp:Button ID="btnSubmit" runat="server" Text="Final Submit" CssClass="btn btn-success btn-sm" OnClick="btnSubmit_Click" />
                                            <asp:Button ID="btnUpdate" runat="server" Text="Final Update" CssClass="btn btn-success btn-sm" OnClick="btnUpdate_Click" Visible="False" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-warning btn-sm" OnClick="btnCancel_Click" />
                                            <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </act:TabPanel>
                            <act:TabPanel ID="tabList" runat="server" HeaderText="SR Entry Details">
                                <HeaderTemplate>
                                    <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300" Font-Size="Small" Text="SR Entry Details" ToolTip="List of SR Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row">
                                            <div class="col-sm-3">
                                                <label for="txtRNo_LIST">SR No</label>
                                                <asp:TextBox ID="txtRNo_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtCircleOffice_LIST">Circle Office</label>
                                                <asp:TextBox ID="txtCircleOffice_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1" style="padding-top: 22px;">
                                                <asp:Button ID="btnSearch_List" runat="server" OnClick="btnSearch_List_Click" ToolTip="SR Search" Text="Search" CssClass="btn btn-sm btn-info" />
                                            </div>
                                            <div class="col-sm-5" style="padding-top: 22px;">
                                                <asp:Label ID="lblList" runat="server" CssClass="label label-danger"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <asp:GridView ID="gvMain" runat="server" OnRowCommand="gvMain_RowCommand" AutoGenerateColumns="false" CssClass="table input-sm table-bordered table-condensed">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnView" runat="server" CausesValidation="false" Text="Edit" CssClass="btn btn-sm btn-danger" CommandName="View" ToolTip='<%# Eval("CODE") %>' CommandArgument='<%# Eval("CODE")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="SRNO" HeaderText="SR No." />
                                                        <asp:BoundField DataField="SRDATE" HeaderText="SR Date" />
                                                        <asp:BoundField DataField="BRCOMPLAINT" HeaderText="BR Complaint" />
                                                        <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" />
                                                        <asp:BoundField DataField="RNO" HeaderText="R No" />
                                                        <asp:BoundField DataField="CLOSUREDATE" HeaderText="Closure Date" />
                                                        <asp:BoundField DataField="ACCUSED" HeaderText="Accused" />
                                                        <asp:BoundField DataField="ALLEGATIONS" HeaderText="Allegations" />
                                                        <asp:BoundField DataField="AMOUNT" HeaderText="Amount" />
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Status
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSTATUS_GV" runat="server" Text='<%# Bind("SHORTSTATUS") %>' ToolTip='<%# Eval("STATUS") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </act:TabPanel>
                        </act:TabContainer>
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
