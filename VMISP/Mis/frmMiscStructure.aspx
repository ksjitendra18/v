<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmMiscStructure.aspx.cs" Inherits="VMISP.Mis.frmMiscStructure" %>

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
                            MISC Entry 
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
                                                <label for="txtRNo"><span style="color: #FF0000">*</span>R No</label>
                                                <asp:TextBox ID="txtRNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1" style="padding-top: 23px;">
                                                <asp:Button ID="btnGet" runat="server" OnClick="btnGet_Click" ToolTip="Complaint Search" CssClass="btn btn-sm btn-info" Text="Search"></asp:Button>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtCompRecDate"><span style="color: #FF0000">*</span>Complaint Receive Date</label>
                                                <asp:TextBox ID="txtCompRecDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="ddlCircleOffice">Circle</label>
                                                <asp:DropDownList ID="ddlCircleOffice" runat="server" CssClass="form-control input-sm">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <label for="txtBRComplaint"><span style="color: #FF0000">*</span>Branch Complaint</label>
                                                <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtCompNo">Comp Number</label>
                                                <asp:TextBox ID="txtCompNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="chkClosureDate">Closure Date</label>
                                                <asp:CheckBox ID="chkClosureDate" runat="server" Checked="false" />
                                                <asp:Label ID="lblClosureDate" runat="server" CssClass="lblCaption"></asp:Label>
                                                <asp:Panel ID="pnlClosureDate" runat="server" Visible="false">
                                                    <asp:TextBox ID="txtClosureDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="txtAccused">Accused</label>
                                                <asp:TextBox ID="txtAccused" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <label for="txtCompNo">Allegations</label>
                                                <asp:TextBox ID="txtAllegations" runat="server" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtAmount">Amount</label>
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control input-sm" Style="text-align: right"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtNPADate">NPA Date</label>
                                                <asp:TextBox ID="txtNPADate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtFinalAction">Final Action</label>
                                                <asp:TextBox ID="txtFinalAction" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtSource">Source</label>
                                                <asp:TextBox ID="txtSource" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-6">
                                                <label for="ddlZone">Zone</label>
                                                <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control input-sm">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtSourceDate">Source Date</label>
                                                <asp:TextBox ID="txtSourceDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlSourceRef">Source Ref</label>
                                                <asp:DropDownList ID="ddlSourceRef" runat="server" CssClass="form-control input-sm">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <label for="txtAccountName">Account Name</label>
                                                <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtClose">Close</label>
                                                <asp:TextBox ID="txtClose" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDateForINVReport">Date for Inv Report</label>
                                                <asp:TextBox ID="txtDateForINVReport" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDesignation">Designation</label>
                                                <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtNatureComp">Nature Comp</label>
                                                <asp:TextBox ID="txtNatureComp" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtInvestigationDate">Investigation Date</label>
                                                <asp:TextBox ID="txtInvestigationDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtType">Type</label>
                                                <asp:TextBox ID="txtType" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="ddlStatusCode">Status Code</label>
                                                <asp:DropDownList ID="ddlStatusCode" runat="server" CssClass="form-control input-sm">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblStatusCodeMIS" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <label for="txtRessonsForClosure">Closure Ressons</label>
                                                <asp:TextBox ID="txtRessonsForClosure" runat="server" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
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
                                                <label for="txtReminderDate">Reminder Date</label>
                                                <asp:TextBox ID="txtReminderDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtReplyReceivedDate">Reply Received Date</label>
                                                <asp:TextBox ID="txtReplyReceivedDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="ddlBankName">Bank Name</label>
                                                <asp:DropDownList ID="ddlBankName" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Value="PNB" Text="PNB"></asp:ListItem>
                                                    <asp:ListItem Value="eOBC" Text="eOBC"></asp:ListItem>
                                                    <asp:ListItem Value="eUNI" Text="eUNI"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtPFNumber">Accused PF Number</label>
                                                <asp:TextBox ID="txtPFNumber" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="ddlNature">Nature</label>
                                                <asp:DropDownList ID="ddlNature" runat="server" CssClass="form-control input-sm">
                                                </asp:DropDownList>
                                                <asp:Panel ID="pnlNatureMIS" runat="server" Visible="false">
                                                    <asp:Label ID="lblNatureMIS" runat="server"></asp:Label>
                                                </asp:Panel>
                                            </div>

                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="ddlZoneNew"><span style="color: #FF0000">*</span>New Zone</label>
                                                <asp:DropDownList ID="ddlZoneNew" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlZoneNew_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlZoneType"><span style="color: #FF0000">*</span>Type</label>
                                                <asp:DropDownList ID="ddlZoneType" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlZoneType_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Preventive" Value="Preventive"></asp:ListItem>
                                                    <asp:ListItem Text="Punitive" Value="Punitive"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtZOCM"><span style="color: #FF0000">*</span>Chief Manager Name</label>
                                                <asp:TextBox ID="txtZOCM" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlBankName"><span style="color: #FF0000">*</span>New Circle</label>
                                                <asp:DropDownList ID="ddlCircleNew" runat="server" CssClass="form-control input-sm"></asp:DropDownList></td>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <div class="panel panel-success">
                                                    <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                                                        EO/ Accused Entry Details
                                                    </div>
                                                    <div class="form-group row">
                                                        <div class="col-sm-2">
                                                            <label for="ddlType">Type</label>
                                                            <asp:DropDownList ID="ddlType_D" runat="server" CssClass="form-control input-sm">
                                                                <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                                                <asp:ListItem Text="Accused" Value="Accused"></asp:ListItem>
                                                                <asp:ListItem Text="EO" Value="EO"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label for="txtEOPFNumber">PF No</label>
                                                            <asp:TextBox ID="txtPFNumber_D" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <label for="txtEOName">Name</label>
                                                            <asp:TextBox ID="txtName_D" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <label for="txtDesignationEO">Designation</label>
                                                            <asp:TextBox ID="txtDesignation_D" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <label for="txtEORetirementDate">Retirement Date</label>
                                                            <asp:TextBox ID="txtRetirementDate_D" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <label for="ddlDealtWith_D">Dealt With</label>
                                                            <asp:DropDownList ID="ddlDealtWith_D" runat="server" CssClass="form-control input-sm">
                                                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-sm-1" style="padding-top: 23px;">
                                                            <asp:Button ID="btnAddEO" runat="server" CssClass="btn btn-sm btn-success" Text="Add" OnClick="btnAddEO_Click" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group row">
                                                        <div class="col-sm-12">
                                                            <asp:GridView ID="gvEODetails" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered input-sm table-condensed" OnRowCommand="gvEODetails_RowCommand">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText">
                                                                        <HeaderTemplate>
                                                                            Edit
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btnEdit_D" runat="server" CssClass="btn btn-sm btn-info" Text="Edit" CommandName="VIEW" ToolTip='<%# Eval("EO_UNIQUEID") %>' CommandArgument='<%#Eval("EO_UNIQUEID") + "~" + Eval("TYPE") + "~" + Eval("PFNO") + "~" + Eval("NAME") + "~" + Eval("DESIGNATION") + "~" + Eval("DOR") + "~" + Eval("DEALTHWITH") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>S No</HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="TYPE" HeaderText="Type" />
                                                                    <asp:BoundField DataField="PFNO" HeaderText="PF Number" />
                                                                    <asp:BoundField DataField="NAME" HeaderText="Name" />
                                                                    <asp:BoundField DataField="DESIGNATION" HeaderText="Designation" />
                                                                    <asp:BoundField DataField="DOR" HeaderText="Retirement Date" />
                                                                    <asp:BoundField DataField="DEALTHWITH" HeaderText="Dealt With" />
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            Delete
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="lnkDelete" runat="server" CausesValidation="false" CommandName="DELETE" CssClass="btn btn-sm btn-danger" CommandArgument='<%#Eval("EO_UNIQUEID") + "~" + Eval("UNIQUEID") %>' Text="Delete"></asp:Button>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle BackColor="darkseagreen" />
                                                                <RowStyle BackColor="White" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
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
                            <act:TabPanel ID="tabList" runat="server" HeaderText="MISC Entry Details">
                                <HeaderTemplate>
                                    <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="MISC Entry Details" ToolTip="List of Misc Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row">
                                            <div class="col-sm-3">
                                                <label for="txtRNo_LIST">R No</label>
                                                <asp:TextBox ID="txtRNo_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtBranch_LIST">Branch</label>
                                                <asp:TextBox ID="txtBranch_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtSource_LIST">Source</label>
                                                <asp:TextBox ID="txtSource_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtSourceRef_LIST">Source Ref</label>
                                                <asp:TextBox ID="txtSourceRef_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-3">
                                                <label for="txtCompNo_LIST">Comp No</label>
                                                <asp:TextBox ID="txtCompNo_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtCircle_LIST">Circle</label>
                                                <asp:TextBox ID="txtCircle_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtAccountName_LIST">Account Name</label>
                                                <asp:TextBox ID="txtAccountName_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtStatus_LIST">Status</label>
                                                <asp:TextBox ID="txtStatus_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-3">
                                            </div>
                                            <div class="col-sm-9">
                                                <asp:Button ID="btnSearch_List" runat="server" OnClick="btnSearch_List_Click" ToolTip="Misc Search" Text="Search" CssClass="btn btn-sm btn-info" />
                                                <asp:Label ID="lblList" runat="server" CssClass="label label-danger"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <asp:Panel ID="pnlList" runat="server" ScrollBars="Both" Width="100%">
                                                    <asp:GridView ID="gvMain" runat="server" OnRowCommand="gvMain_RowCommand" OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="false" CssClass="table input-sm table-bordered table-condensed">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText">
                                                                <HeaderTemplate>
                                                                    Select
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnView" runat="server" CausesValidation="false" CommandName="View" ToolTip='<%# Eval("CODE") %>' CommandArgument='<%# Eval("CODE")%>' CssClass="btn btn-sm btn-danger" Text="Edit" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="RNO" HeaderText="R No." />
                                                            <asp:BoundField DataField="COMPRECDATE" HeaderText="Comp Rec Date" />
                                                            <asp:BoundField DataField="BRCOMPLAINT" HeaderText="BR Complaint" />
                                                            <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" />
                                                            <asp:BoundField DataField="COMPNO" HeaderText="Comp No" />
                                                            <asp:BoundField DataField="CLOSUREDATE" HeaderText="Closure Date" />
                                                            <asp:BoundField DataField="ACCUSED" HeaderText="Accused" />
                                                            <asp:BoundField DataField="ALLEGATIONS" HeaderText="Allegations" />
                                                            <asp:BoundField DataField="AMOUNT" HeaderText="Amount" />
                                                            <asp:BoundField DataField="ACCOUNTNAME" HeaderText="Account Name" />
                                                            <asp:BoundField DataField="CASECLOSE" HeaderText="Close" />
                                                            <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" />
                                                            <asp:BoundField DataField="APPROVALSTATUSTEXT" HeaderText="Checker Status" />
                                                            <asp:BoundField DataField="CHECKERREMARKS" HeaderText="Checker Remarks" />
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
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </act:TabPanel>
                        </act:TabContainer>
                        <asp:HiddenField ID="hidCircleOffice" runat="server" />
                        <asp:HiddenField ID="hidZone" runat="server" />
                        <asp:HiddenField ID="hidStatusCode" runat="server" />
                        <asp:HiddenField ID="hidNature" runat="server" />
                        <asp:HiddenField ID="hidSourceRef" runat="server" />
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
