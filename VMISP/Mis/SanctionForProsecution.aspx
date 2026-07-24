<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="SanctionForProsecution.aspx.cs" Inherits="VMISP.Mis.SanctionForProsecution" %>

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
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="col-lg-12">
                <div class="form-group row">
                    <div class="panel panel-primary">
                        <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                            Sanction For Prosecution
                        </div>
                        <br />
                        <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="100%">
                            <act:TabPanel ID="tabEntry" runat="server">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEntryHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300" Font-Size="Small" Text="Sanction For Prosecution Entry" ToolTip="Sanction For Prosecution Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtSPNumber"><span style="color: #FF0000">*</span>SP Number</label>
                                                <asp:TextBox ID="txtSPNumber" runat="server" CssClass="form-control input-sm" placeHolder="SP Number" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtRCNumber"><span style="color: #FF0000">*</span>RC Number</label>
                                                <asp:TextBox ID="txtRCNumber" runat="server" CssClass="form-control input-sm" placeHolder="RC Number" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 input-group">
                                                <label for="txtRCDate"><span style="color: #FF0000">*</span>RC Date</label>
                                                <asp:TextBox ID="txtRCDate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtReportDate"><span style="color: #FF0000">*</span>Date of Report Received</label>
                                                <asp:TextBox ID="txtReportDate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtPFNumber"><span style="color: #FF0000">*</span>PF Number</label>
                                                <asp:TextBox ID="txtPFNumber" runat="server" CssClass="form-control input-sm" placeHolder="PF Number" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="txtName"><span style="color: #FF0000">*</span>Name</label>
                                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control input-sm" placeHolder="Name" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDOR"><span style="color: #FF0000">*</span>Date of Retirement</label>
                                                <asp:TextBox ID="txtDOR" runat="server" CssClass="form-control input-sm date" placeHolder="click here"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtDesignation"><span style="color: #FF0000">*</span>Designation</label>
                                                <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control input-sm" placeHolder="Designation" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlCircle"><span style="color: #FF0000">*</span>Circle</label>
                                                <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlBranch"><span style="color: #FF0000">*</span>Branch</label>
                                                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDA"><span style="color: #FF0000">*</span>DA</label>
                                                <asp:TextBox ID="txtDA" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="ddlDAView">DA View</label>
                                                <asp:DropDownList ID="ddlDAView" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Denial" Value="Denial"></asp:ListItem>
                                                    <asp:ListItem Text="Sanction" Value="Sanction"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtLetterToCBIDate">Letter to CBI, After Sanction</label>
                                                <asp:TextBox ID="txtLetterToCBIDate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtLetterToCVCDate">Letter to CVC, After denial</label>
                                                <asp:TextBox ID="txtLetterToCVCDate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlCVCView">CVC View</label>
                                                <asp:DropDownList ID="ddlCVCView" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Denial" Value="Denial"></asp:ListItem>
                                                    <asp:ListItem Text="Sanction" Value="Sanction"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtLetterToDADate">Letter to DA, After CVC Advice</label>
                                                <asp:TextBox ID="txtLetterToDADate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDAOrderToCBIDate">DA's order to CBI</label>
                                                <asp:TextBox ID="txtDAOrderToCBIDate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="ddlStatus">Status</label>
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtLetterSentDate">Letter Sent Date :</label>
                                                <asp:TextBox ID="txtLetterSentDate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlLetterSentTo">Letter Sent To :</label>
                                                <asp:DropDownList ID="ddlLetterSentTo" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtReminderDate">Reminder Date :</label>
                                                <asp:TextBox ID="txtReminderDate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtReplyReceivedDate">Reply Received Date :</label>
                                                <asp:TextBox ID="txtReplyReceivedDate" runat="server" CssClass="form-control date input-sm" placeHolder="click Here"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-6">
                                                <label for="txtAccountName"><span style="color: #FF0000">*</span>Account Name</label>
                                                <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control input-sm" placeholder="Account Name" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                             <div class="col-sm-3">
                                                <label for="ddlCBIEOW"><span style="color: #FF0000">*</span>CBI/EOW Request for</label>
                                                <asp:DropDownList ID="ddlCBIEOW" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                    <asp:ListItem Value="Only SoP" Text="Only SoP"></asp:ListItem>
                                                    <asp:ListItem Value="Only RDA" Text="Only RDA"></asp:ListItem>
                                                    <asp:ListItem Value="Both SoP & RDA" Text="Both SoP & RDA"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlBankName"><span style="color: #FF0000">*</span>Bank Name</label>
                                                <asp:DropDownList ID="ddlBankName" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Value="PNB" Text="PNB"></asp:ListItem>
                                                    <asp:ListItem Value="OBC" Text="OBC"></asp:ListItem>
                                                    <asp:ListItem Value="UBI" Text="UBI"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <label for="txtRemarks"><span style="color: #FF0000">*</span>Remarks</label>
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control input-sm" placeholder="Enter Remarks, If Any...." TextMode="MultiLine" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <label for="txtDealingOfficerRemarks"><span style="color: #FF0000">*</span>Dealing Officer Remarks</label>
                                                <asp:TextBox ID="txtDealingOfficerRemarks" runat="server" CssClass="form-control input-sm" placeholder="Enter Dealing Officer Remarks, If Any...." TextMode="MultiLine" onkeypress="return blockSpecialChar(event)" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                            </div>
                                            <div class="col-sm-9">
                                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success btn-sm" Text="Submit" OnClick="btnSubmit_Click" />
                                                <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-success btn-sm" Text="Update" Visible="false" OnClick="btnUpdate_Click" />
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
                            <act:TabPanel ID="tabList" runat="server" HeaderText="Sanction For Prosecution Entry Details">
                                <HeaderTemplate>
                                    <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="Sanction For Prosecution Entry Details" ToolTip="Sanction For Prosecution Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row">
                                            <div class="col-sm-2">
                                                <label for="txtSPNO_LIST">SP Number</label>
                                                <asp:TextBox ID="txtSPNO_LIST" runat="server" CssClass="form-control input-sm" placeholder="SP Number" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <label for="txtRCNO_LIST">RC Number</label>
                                                <asp:TextBox ID="txtRCNO_LIST" runat="server" CssClass="form-control input-sm" placeholder="RC Number" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1">
                                                <label for="btnSearch">&nbsp;&nbsp;</label>
                                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-info btn-sm" Text="Search" OnClick="btnSearch_Click" />
                                            </div>
                                            <div class="col-sm-7">
                                                <label for="lblMsgSearch">&nbsp;&nbsp;</label>
                                                <asp:Label ID="lblMsgSearch" runat="server" CssClass="label label-danger"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="margin-right: 0px;">
                                            <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="false" OnRowCommand="gvDetails_RowCommand" CssClass="table input-sm table-bordered table-condensed">
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
                                                    <asp:BoundField DataField="SPNO" HeaderText="SP No" />
                                                    <asp:BoundField DataField="RCNO" HeaderText="RC No" />
                                                    <asp:BoundField DataField="RCDATE" HeaderText="RC Date" />
                                                    <asp:BoundField DataField="REPORTDATE" HeaderText="Report Date" />
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
                </div>
            </div>
            <asp:HiddenField ID="hidUniqueID" runat="server" />
            <asp:HiddenField ID="hidUserRole" runat="server" />
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
