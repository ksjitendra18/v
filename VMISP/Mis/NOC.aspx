<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="NOC.aspx.cs" Inherits="VMISP.Mis.NOC" %>

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
                            NOC Entry 
                        </div>
                        <br />
                        <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="100%" OnActiveTabChanged="tabMain_ActiveTabChanged"
                            AutoPostBack="true">
                            <act:TabPanel ID="tabEntry" runat="server">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEntryHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="Entry" ToolTip="Misc Structure Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-2">
                                                <label for="txtSNo"><span style="color: #FF0000">*</span>S No</label>
                                                <asp:TextBox ID="txtSNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1" style="padding-top: 23px;">
                                                <asp:Button ID="btnGet" runat="server" OnClick="btnGet_Click" ToolTip="NOC Search" CssClass="btn btn-sm btn-info" Text="Search"></asp:Button>
                                            </div>
                                            <div class="col-sm-2">
                                                <label for="txtPFNo"><span style="color: #FF0000">*</span>PF Number</label>
                                                <asp:TextBox ID="txtPFNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1" style="padding-top: 23px;">
                                                <asp:Button ID="btnGetEmpDetails" runat="server" OnClick="btnGetEmpDetails_Click" ToolTip="Employee Details Search" CssClass="btn btn-sm btn-danger" Text="Get"></asp:Button>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtName">Name</label>
                                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDesignation">Designation</label>
                                                <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="ddlScale">Scale</label>
                                                <asp:DropDownList ID="ddlScale" runat="server" CssClass="form-control input-sm"></asp:DropDownList>&nbsp;
                                                <asp:Panel ID="pnlNatureMIS" runat="server" Visible="False">
                                                    <asp:Label ID="lblNatureMIS" runat="server" CssClass="form-control input-sm"></asp:Label>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-sm-2">
                                                <label for="txtDOR"><span style="color: #FF0000">*</span>Date of Retirement</label>
                                                <asp:TextBox ID="txtDOR" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtSolID"><span style="color: #FF0000">*</span>SolID</label>
                                                <asp:TextBox ID="txtSolID" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1">
                                                <label for="txtActiveStatus"><span style="color: #FF0000">*</span>Active</label>
                                                <asp:TextBox ID="txtActiveStatus" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlZoneNew">New Zone</label>
                                                <asp:DropDownList ID="ddlZoneNew" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlZoneNew_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 4px;">
                                            <div class="col-sm-3">
                                                <label for="ddlCircleNew">New Circle</label>
                                                <asp:DropDownList ID="ddlCircleNew" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtBRComplaint"><span style="color: #FF0000">*</span>Branch Name</label>
                                                <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtClearanceDate">Clearance Date</label>
                                                <asp:TextBox ID="txtClearanceDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlBankName">Bank Name</label>
                                                <asp:DropDownList ID="ddlBankName" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Value="PNB" Text="PNB"></asp:ListItem>
                                                    <asp:ListItem Value="eOBC" Text="eOBC"></asp:ListItem>
                                                    <asp:ListItem Value="eUNI" Text="eUNI"></asp:ListItem>
                                                </asp:DropDownList>
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
                                                <asp:HiddenField ID="hidLetterSentTo" runat="server" />
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtReplyReceivedDate">Reply Received Date</label>
                                                <asp:TextBox ID="txtReplyReceivedDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlReason">Reason</label>
                                                <asp:DropDownList ID="ddlReason" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Retirement" Value="Retirement"></asp:ListItem>
                                                    <asp:ListItem Text="VRS" Value="VRS"></asp:ListItem>
                                                    <asp:ListItem Text="Resignation" Value="Resignation"></asp:ListItem>
                                                    <asp:ListItem Text="Review of Service" Value="Review of Service"></asp:ListItem>
                                                    <asp:ListItem Text="Sabatical Leave" Value="Sabatical Leave"></asp:ListItem>
                                                    <asp:ListItem Text="Death" Value="Death"></asp:ListItem>
                                                    <asp:ListItem Text="Visit to Abroad" Value="Visit to Abroad"></asp:ListItem>
                                                    <asp:ListItem Text="Deputation" Value="Deputation"></asp:ListItem>
                                                    <asp:ListItem Text="Compulsory Retirement" Value="Compulsory Retirement"></asp:ListItem>
                                                    <asp:ListItem Text="Terminal Dues" Value="Terminal Dues"></asp:ListItem>
                                                    <asp:ListItem Text="Other – Mention in Remark" Value="Other – Mention in Remark"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <label for="txtRemarks">Remarks</label>
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <label for="txtDealingOfficerRemarks">Dealing Officer Remarks</label>
                                                <asp:TextBox ID="txtDealingOfficerRemarks" runat="server" placeholder="Enter Dealing Officer Remarks, If Any...." onkeypress="return blockSpecialChar(event)" Enabled="false" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <asp:Panel ID="pnlHOStatus" runat="server" Visible="False">
                                                    <label for="txtHORemarks">HO Remarks</label>
                                                    <asp:TextBox ID="txtHORemarks" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                </asp:Panel>
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
                            <act:TabPanel ID="tabList" runat="server" HeaderText="NOC Entry Details">
                                <HeaderTemplate>
                                    <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="NOC Entry Details" ToolTip="List of NOC Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row">
                                            <div class="form-group row" style="padding-right: 5px;">
                                                <div class="col-sm-2">
                                                    <label for="txtRNo_LIST">S No</label>
                                                    <asp:TextBox ID="txtRNo_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-2">
                                                    <label for="txtPFNumber_LIST">PF Number</label>
                                                    <asp:TextBox ID="txtPFNumber_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-3">
                                                    <label for="txtName_LIST">Name</label>
                                                    <asp:TextBox ID="txtName_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-1" style="margin-top: 25px;">
                                                    <asp:Button ID="btnSearch_List" runat="server" OnClick="btnSearch_List_Click" ToolTip="NOC Search" Text="Search" CssClass="btn btn-sm btn-info" />
                                                </div>
                                                <div class="col-sm-4" style="margin-top: 25px;">
                                                    <asp:Label ID="lblList" runat="server" CssClass="label label-danger"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <asp:Panel ID="pnlList" runat="server" ScrollBars="Both" Width="100%">
                                                    <asp:GridView ID="gvMain" runat="server" OnRowCommand="gvMain_RowCommand" AutoGenerateColumns="false" CssClass="table input-sm table-bordered table-condensed">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Select
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnView" runat="server" CausesValidation="false" CommandName="View" CssClass="btn btn-sm btn-danger" CommandArgument='<%#Eval("CODE")%>' ToolTip='<%# Eval("CODE") %>' Text="Edit" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="SNO" HeaderText="S No." />
                                                            <asp:BoundField DataField="RECDATE" HeaderText="Rec Date" />
                                                            <asp:BoundField DataField="BRCOMPLAINT" HeaderText="Branch Name" />
                                                            <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" />
                                                            <asp:BoundField DataField="PFNO" HeaderText="PF No" />
                                                            <asp:BoundField DataField="CLOSUREDATE" HeaderText="Clearance Date" />
                                                            <asp:BoundField DataField="NAME" HeaderText="Name" />
                                                            <asp:BoundField DataField="DESIGNATION" HeaderText="Designation" />
                                                            <asp:BoundField DataField="STATE" HeaderText="State" />
                                                            <asp:BoundField DataField="SCLAECODE" HeaderText="Scale Code" />
                                                            <asp:BoundField DataField="SCALE" HeaderText="Scale" />
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Remarks
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSTATUS_GV" runat="server" Text='<%# Bind("SHORTSTATUS") %>' ToolTip='<%# Eval("STATUS") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
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
