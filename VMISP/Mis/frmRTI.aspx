<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmRTI.aspx.cs" Inherits="VMISP.Mis.frmRTI" ValidateRequest="false" %>

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
                            RTI Entry 
                        </div>
                        <br />
                        <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="100%" OnActiveTabChanged="tabMain_ActiveTabChanged"
                            AutoPostBack="true">
                            <act:TabPanel ID="tabEntry" runat="server">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEntryHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300" Font-Size="Small" Text="Entry" ToolTip="RTI Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-2">
                                                <label for="txtRTINo"><span style="color: #FF0000">*</span>RTI Number</label>
                                                <asp:TextBox ID="txtRTINo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1" style="padding-top: 23px;">
                                                <asp:Button ID="btnGet" runat="server" OnClick="btnGet_Click" ToolTip="NOC Search" CssClass="btn btn-sm btn-info" Text="Search"></asp:Button>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtRTIRecDate"><span style="color: #FF0000">*</span>RTI Received Date</label>
                                                <asp:TextBox ID="txtRTIRecDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtSourceDate">Source Date</label>
                                                <asp:TextBox ID="txtSourceDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDesignation">Designation</label>
                                                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtBRComplaint">Branch Complaint</label>
                                                <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlCircleOffice">Circle</label>
                                                <asp:DropDownList ID="ddlCircleOffice" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtSource">Source</label>
                                                <asp:TextBox ID="txtSource" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDesignation">Account Name</label>
                                                <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtAllegations">Allegations</label>
                                                <asp:TextBox ID="txtAllegations" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlStatusCode">Status Code</label>
                                                <asp:DropDownList ID="ddlStatusCode" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                <asp:Label ID="lblStatusCodeMIS" runat="server"></asp:Label>
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
                                                <label for="txtDesignation">Source Reference</label>
                                                <asp:DropDownList ID="ddlSourceRef" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtAmount">Amount</label>
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control input-sm" Style="text-align: right"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtAccused">Accused</label>
                                                <asp:TextBox ID="txtAccused" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDesignation">Designation</label>
                                                <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDesignation">Present Posting</label>
                                                <asp:TextBox ID="txtPresentPosting" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="ddlZone">Zone</label>
                                                <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtSentTo">Sent To</label>
                                                <asp:TextBox ID="txtSentTo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtSentForInvDate">Sent for Investigation Date</label>
                                                <asp:TextBox ID="txtSentForInvDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtCatANo">CAT A Number</label>
                                                <asp:TextBox ID="txtCatANo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtCatBNo">CAT B Number</label>
                                                <asp:TextBox ID="txtCatBNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtASNo">AS Number</label>
                                                <asp:TextBox ID="txtASNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtIACDate">IAC Date</label>
                                                <asp:TextBox ID="txtIACDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtPendingWith">Pending With</label>
                                                <asp:TextBox ID="txtPendingWith" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtNameINVOfficial">Name INV Official</label>
                                                <asp:TextBox ID="txtNameINVOfficial" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtForINVReport">For Inv Report</label>
                                                <asp:TextBox ID="txtDateForINVReport" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtClose">Close</label>
                                                <asp:TextBox ID="txtClose" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtRNO">R Number</label>
                                                <asp:TextBox ID="txtRNO" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtRYSent">RY Sent</label>
                                                <asp:TextBox ID="txtRYSent" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtAPlan">A Plan</label>
                                                <asp:TextBox ID="txtAPlan" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtRegister">Register</label>
                                                <asp:TextBox ID="txtRegister" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlNature">Nature</label>
                                                <asp:DropDownList ID="ddlNature" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                <asp:Panel ID="pnlNatureMIS" runat="server" Visible="False">
                                                    <asp:Label ID="lblNatureMIS" runat="server"></asp:Label>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtReasonsForClosure">Reasons for Closure</label>
                                                <asp:TextBox ID="txtReasonsForClosure" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
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
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtReplyReceivedDate">Reply Received Date</label>
                                                <asp:TextBox ID="txtReplyReceivedDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlBankName">Bank Name</label>
                                                <asp:DropDownList ID="ddlBankName" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                    <asp:ListItem Value="PNB" Text="PNB"></asp:ListItem>
                                                    <asp:ListItem Value="OBC" Text="OBC"></asp:ListItem>
                                                    <asp:ListItem Value="UBI" Text="UBI"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlLetterSentTo">New Zone</label>
                                                <asp:DropDownList ID="ddlZoneNew" Width="100%" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlZoneNew_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtReminderDate">New Circle</label>
                                                <asp:DropDownList ID="ddlCircleNew" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
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
                                    </div>
                                </ContentTemplate>
                            </act:TabPanel>
                            <act:TabPanel ID="tabList" runat="server" HeaderText="List">
                                <HeaderTemplate>
                                    <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300" Font-Size="Small" Text="RTI Entry Details" ToolTip="List of RTI Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row">
                                            <div class="col-sm-2">
                                                <label for="txtRTINo_LIST">RTI Number</label>
                                                <asp:TextBox ID="txtRTINo_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <label for="txtBranch_LIST">Branch</label>
                                                <asp:TextBox ID="txtBranch_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <label for="txtCircle_LIST">Circle</label>
                                                <asp:TextBox ID="txtCircle_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtStatus_LIST">Status</label>
                                                <asp:TextBox ID="txtStatus_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1" style="padding-top: 22px;">
                                                <asp:Button ID="btnSearch_List" runat="server" OnClick="btnSearch_List_Click" ToolTip="RTI Search" Text="Search" CssClass="btn btn-sm btn-info" />
                                            </div>
                                            <div class="col-sm-2" style="padding-top: 22px;">
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
                                                                <asp:Button ID="btnView" runat="server" CausesValidation="false" CommandName="View" ToolTip='<%# Eval("CODE") %>' CommandArgument='<%# Eval("CODE")%>' CssClass="btn btn-sm btn-danger" Text="Edit" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                RTI No.
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRTINO_GV" runat="server" Text='<%# Bind("RTINO") %>' ToolTip='<%# Eval("SHORTSTATUS") %>'></asp:Label>
                                                            </ItemTemplate>

                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="RECRTIDATE" HeaderText="RTI Rec Date" />
                                                        <asp:BoundField DataField="SOURCEDATE" HeaderText="Source Date" />
                                                        <asp:BoundField DataField="ACCOUNTNAME" HeaderText="Account Name" />
                                                        <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" />
                                                        <asp:BoundField DataField="AMOUNT" HeaderText="Amount" />
                                                        <asp:BoundField DataField="ACCUSED" HeaderText="Accused" />
                                                        <asp:BoundField DataField="IACDATE" HeaderText="IAC Date" />
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
