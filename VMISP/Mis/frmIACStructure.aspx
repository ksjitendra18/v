<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmIACStructure.aspx.cs" Inherits="VMISP.Mis.frmIACStructure" ValidateRequest="false" %>

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
                            IAC Entry 
                        </div>
                        <br />
                        <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="100%" OnActiveTabChanged="tabMain_ActiveTabChanged"
                            AutoPostBack="true">
                            <act:TabPanel ID="tabEntry" runat="server">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEntryHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="Entry" ToolTip="IAC Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-2">
                                                <label for="txtIACNo"><span style="color: #FF0000">*</span>IAC No</label>
                                                <asp:TextBox ID="txtIACNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1" style="padding-top: 23px;">
                                                <asp:Button ID="btnGet" runat="server" OnClick="btnGet_Click" ToolTip="IAC Search" CssClass="btn btn-sm btn-info" Text="Search"></asp:Button>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtCompRecDate"><span style="color: #FF0000">*</span>Received Date</label>
                                                <asp:TextBox ID="txtCompRecDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlCircleOffice"><span style="color: #FF0000">*</span>Circle Name</label>
                                                <asp:DropDownList ID="ddlCircleOffice" runat="server" CssClass="form-control input-sm">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtBRComplaint"><span style="color: #FF0000">*</span>Branch Name</label>
                                                <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtVIGNo">Vigilance Number</label>
                                                <asp:TextBox ID="txtVIGNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="chkClosureDate"><span style="color: #FF0000">*</span>Closure Date</label>
                                                <asp:CheckBox ID="chkClosureDate" runat="server" Checked="false" />
                                                <asp:Label ID="lblClosureDate" runat="server" CssClass="form-control input-sm"></asp:Label>
                                                <asp:Panel ID="pnlClosureDate" runat="server" Visible="false">
                                                    <asp:TextBox ID="txtClosureDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtAccused">Accused</label>
                                                <asp:TextBox ID="txtAccused" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDAView">DA View</label>
                                                <asp:TextBox ID="txtDAView" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtMeetNo">Meet Number</label>
                                                <asp:TextBox ID="txtMeetNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtRetDate">Ret Date</label>
                                                <asp:TextBox ID="txtRetDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtIACView"><span style="color: #FF0000">*</span>IAC View</label>
                                                <asp:TextBox ID="txtIACView" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlZone">Zone</label>
                                                <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtSource">Source</label>
                                                <asp:TextBox ID="txtSource" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDA">DA</label>
                                                <asp:TextBox ID="txtDA" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtCVOView"><span style="color: #FF0000">*</span>CVO View</label>
                                                <asp:TextBox ID="txtCVOView" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtAccountName">Account Name</label>
                                                <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtAmount">Amount</label>
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtIACNo1">IAC No-1</label>
                                                <asp:TextBox ID="txtIACNo1" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtPFNumber">PF Number</label>
                                                <asp:TextBox ID="txtPFNumber" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlStatusCode"><span style="color: #FF0000">*</span>Status Code</label>
                                                <asp:DropDownList ID="ddlStatusCode" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                <asp:Label ID="lblStatusCodeMIS" runat="server" CssClass="form-control input-sm" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="ddlNature">Nature Case</label>
                                                <asp:DropDownList ID="ddlNature" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                <asp:Panel ID="pnlNatureMIS" runat="server" Visible="false">
                                                    <asp:Label ID="lblNatureMIS" runat="server"></asp:Label>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlBankName">Bank Name</label>
                                                <asp:DropDownList ID="ddlBankName" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Value="PNB" Text="PNB"></asp:ListItem>
                                                    <asp:ListItem Value="eOBC" Text="eOBC"></asp:ListItem>
                                                    <asp:ListItem Value="eUNI" Text="eUNI"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtLetterSentToDADate">Letter Sent to DA Date</label>
                                                <asp:TextBox ID="txtLetterSentToDADate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtLetterSentDate">Letter Sent Date</label>
                                                <asp:TextBox ID="txtLetterSentDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
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
                                            <div class="col-sm-3">
                                                <label for="txtTMSACRefNo">TMSAC Reference Number</label>
                                                <asp:TextBox ID="txtTMSACRefNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtDesignation">Designation</label>
                                                <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlScale">Scale</label>
                                                <asp:DropDownList ID="ddlScale" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlZoneNew">New Zone</label>
                                                <asp:DropDownList ID="ddlZoneNew" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlZoneNew_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlCircleNew">New Circle</label>
                                                <asp:DropDownList ID="ddlCircleNew" runat="server" CssClass="form-control input-sm"></asp:DropDownList></td>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="ddlABBFFCase">ABBFF Case</label>
                                                <asp:DropDownList ID="ddlABBFFCase" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtABBFFCaseSubmissionDate">ABBFF Case Submission date</label>
                                                <asp:TextBox ID="txtABBFFCaseSubmissionDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtABBFFReplyDate">ABBFF Reply Date</label>
                                                <asp:TextBox ID="txtABBFFReplyDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtABBFFReferenceNumber">ABBFF Reference Number</label>
                                                <asp:TextBox ID="txtABBFFReferenceNumber" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtABBFFAdviceReceiveDate">ABBFF Advice Receive Date</label>
                                                <asp:TextBox ID="txtABBFFAdviceReceiveDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-9">
                                                 <label for="txtABBFFAdviceDetail">ABBFF Advice Detail</label>
                                                <asp:TextBox ID="txtABBFFAdviceDetail" runat="server" CssClass="form-control input-sm"></asp:TextBox></div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <label for="txtStatus">Status</label>
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
                            <act:TabPanel ID="tabList" runat="server" HeaderText="IAC Entry Details">
                                <HeaderTemplate>
                                    <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="IAC Entry Details" ToolTip="List of IAC Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row">
                                            <div class="col-sm-3">
                                                <label for="txtRNo_LIST">IAC Number</label>
                                                <asp:TextBox ID="txtRNo_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtPFNumber_LIST">PF Number</label>
                                                <asp:TextBox ID="txtPFNumber_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtAccountName_LIST">Account Name</label>
                                                <asp:TextBox ID="txtAccountName_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtAccused_LIST">Accused</label>
                                                <asp:TextBox ID="txtAccused_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-3">
                                                <label for="txtBranch_LIST">Branch</label>
                                                <asp:TextBox ID="txtBranch_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtCircle_LIST">Circle</label>
                                                <asp:TextBox ID="txtCircle_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtStatus_LIST">Status</label>
                                                <asp:TextBox ID="txtStatus_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1" style="padding-top: 22px;">
                                                <asp:Button ID="btnSearch_List" runat="server" OnClick="btnSearch_List_Click" ToolTip="IAC Search" Text="Search" CssClass="btn btn-sm btn-info" />
                                            </div>
                                            <div class="col-sm-2" style="padding-top: 22px;">
                                                <asp:Label ID="lblList" runat="server" CssClass="label label-danger"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <%--  <asp:Panel ID="pnlList" runat="server" ScrollBars="Both" Width="100%">--%>
                                                <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" OnRowCommand="gvMain_RowCommand" CssClass="table input-sm table-bordered table-condensed" PageSize="20">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnView" runat="server" CausesValidation="false" CommandName="View" CssClass="btn btn-sm btn-danger"
                                                                    ToolTip='<%# Eval("SNO") %>' CommandArgument='<%# Eval("SNO")%>' Text="Edit" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="IACNO" HeaderText="IAC No." />
                                                        <asp:BoundField DataField="RECDATE" HeaderText="Rec Date" />
                                                        <asp:BoundField DataField="BRCOMPLAINT" HeaderText="Branch Name" />
                                                        <asp:BoundField DataField="VIGNO" HeaderText="VIG No" />
                                                        <asp:BoundField DataField="CLOSUREDATE" HeaderText="Closure Date" />
                                                        <asp:BoundField DataField="ACCUSED" HeaderText="Accused" />
                                                        <asp:BoundField DataField="DAVIEW" HeaderText="DA View" />
                                                        <asp:BoundField DataField="RETDATE" HeaderText="Ret Date" />
                                                        <asp:BoundField DataField="IACVIEW" HeaderText="IAC View" />
                                                        <asp:BoundField DataField="ACCOUNTNAME" HeaderText="Account Name" />
                                                        <asp:BoundField DataField="AMOUNT" HeaderText="Amount" />
                                                        <asp:BoundField DataField="PFNUMBER" HeaderText="PF Number" />
                                                        <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" />
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
                                                <%--    </asp:Panel>--%>
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
