<%@ Page  Title="" Language="C#" AutoEventWireup="true" CodeBehind="frmABBFF.aspx.cs" MasterPageFile="~/SiteMaster.Master"
    Inherits="VMISP.Mis.frmABBFF" ValidateRequest="false" %>

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
                            ABBFF Entry 
                        </div>
                        <br />
                        <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="100%" OnActiveTabChanged="tabMain_ActiveTabChanged" AutoPostBack="true">
                            <act:TabPanel ID="tabEntry" runat="server">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEntryHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300" Font-Size="Small" Text="Entry" ToolTip="ABBFF Structure Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-2">
                                                <label for="txtRNo"><span style="color: #FF0000">*</span>R No</label>
                                                <asp:TextBox ID="txtRNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvRNo" runat="server" ControlToValidate="txtRNo" ErrorMessage="Please enter R No" ForeColor="Red" ValidationGroup="btnSubmit"></asp:RequiredFieldValidator> 
                                            </div>
                                            <div class="col-sm-1" style="padding-top: 23px;">
                                                <asp:Button ID="btnGet" runat="server" OnClick="btnGet_Click" ToolTip="ABBFF Search" CssClass="btn btn-sm btn-info" Text="Search"></asp:Button>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtABBFFRecDate"><span style="color: #FF0000">*</span>ABBFF Receive Date</label>
                                                <asp:TextBox ID="txtABBFFRecDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlSourceRef">SOURCE REF</label>
                                                <asp:DropDownList ID="ddlSourceRef" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="IAD" Value="IAD"></asp:ListItem>
                                                    <asp:ListItem Text="HRDD" Value="HRDD"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtSourceDate"><span style="color: #FF0000"></span>Source Date</label>
                                                <asp:TextBox ID="txtSourceDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtDtofOccurance"><span style="color: #FF0000"></span>Date of Occurance</label>
                                                <asp:TextBox ID="txtDtofOccurance" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDtofDetection"><span style="color: #FF0000"></span>Date of Detection</label>
                                                <asp:TextBox ID="txtDtofDetection" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDtofReporttoRBI"><span style="color: #FF0000"></span>Date of Reporting to RBI</label>
                                                <asp:TextBox ID="txtDtofReporttoRBI" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlFraudCommitedby">FRAUD Committed By</label>
                                                <asp:DropDownList ID="ddlFraudCommitedby" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Staff" Value="STAFF"></asp:ListItem>
                                                    <asp:ListItem Text="Customer" Value="CUSTOMER"></asp:ListItem>
                                                    <asp:ListItem Text="OutSider" Value="OUTSIDER"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtFMRNo"><span style="color: #FF0000">*</span>FMR Number</label>
                                                <asp:TextBox ID="txtFMRNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvFMR" runat="server" ControlToValidate="txtFMRNo" ErrorMessage="Please enter FMR Number name" ForeColor="Red" ValidationGroup="btnSubmit"></asp:RequiredFieldValidator> 
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlFIR">FIR</label>
                                                <asp:DropDownList ID="ddlFIR" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="txtFIRDate">FIR Date</label>
                                                <asp:TextBox ID="txtFIRDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtDtofNPA"><span style="color: #FF0000">*</span>Date of NPA</label>
                                                <asp:TextBox ID="txtDtofNPA" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvDtofNPA" runat="server" ControlToValidate="txtDtofNPA" ErrorMessage="Please enter Date of NPA" ForeColor="Red" ValidationGroup="btnSubmit"></asp:RequiredFieldValidator> 
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtAccName">Account Name</label>
                                                <asp:TextBox ID="txtAccName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtAmt">Total Exposure</label>
                                                <asp:TextBox ID="txtAmt" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtBRComplaint">Complaint with CBI</label>
                                                <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <%--  <div class="col-sm-3">
                                                <label for="txtZone">Zone</label>
                                                <asp:TextBox ID="txtZone" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtCircleOffice">Circle Office</label>
                                                <asp:TextBox ID="txtCircleOffice" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>--%>
                                            <div class="col-sm-3">
                                                <label for="txtBranchOffice">Branch Office</label>
                                                <asp:TextBox ID="txtBranchOffice" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="txtHOSACDate"><span style="color: #FF0000"></span>HOSAC Date</label>
                                                <asp:TextBox ID="txtHOSACDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <label for="txtModusOperandi"><span style="color: #FF0000"></span>Nature of Fraud</label>
                                                <asp:TextBox ID="txtModusOperandi" runat="server" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtCaseSubmissionDate"><span style="color: #FF0000"></span>ABBFF Case  Submission Date</label>
                                                <asp:TextBox ID="txtCaseSubmissionDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <%--<label for="txtABBFFReplydt"><span style="color: #FF0000"></span>ABBFF Reply Date</label>--%>
                                                <label for="txtABBFFReplydt"><span style="color: #FF0000"></span>ABBFF Observations</label>
                                                <asp:TextBox ID="txtABBFFReplydt" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtReplySenttoABBFFDt"><span style="color: #FF0000"></span>Reply Sent to ABBFF Date</label>
                                                <asp:TextBox ID="txtReplySenttoABBFFDt" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtABBFFRefNo"><span style="color: #FF0000"></span>ABBFF Reference No</label>
                                                <asp:TextBox ID="txtABBFFRefNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtABBFFAdviceRecDate"><span style="color: #FF0000"></span>ABBFF Advice Received Date</label>
                                                <asp:TextBox ID="txtABBFFAdviceRecDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtABBFFAdviceDetails">ABBFF Advice Details</label>
                                                <asp:TextBox ID="txtABBFFAdviceDetails" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlNewZone"><span style="color: #FF0000">*</span>New Zone</label>
                                                <asp:DropDownList ID="ddlNewZone" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlNewZone_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvNewZone" runat="server" ControlToValidate="ddlNewZone" ErrorMessage="Please enter New Zone" ForeColor="Red" ValidationGroup="btnSubmit"></asp:RequiredFieldValidator> 
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlNewCircle"><span style="color: #FF0000">*</span>New Circle</label>
                                                <asp:DropDownList ID="ddlNewCircle" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                 <asp:RequiredFieldValidator ID="rfvNewCircle" runat="server" ControlToValidate="ddlNewCircle" ErrorMessage="Please enter New Circle" ForeColor="Red" ValidationGroup="btnSubmit"></asp:RequiredFieldValidator> 
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtConnectSOPNumber">Connected SOP/SI Number</label>
                                                <asp:TextBox ID="txtConnectSOPNumber" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlCaseCloseStatus">SOP/SI Case Close</label>
                                                <asp:DropDownList ID="ddlCaseCloseStatus" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="chkClosureDate"><span style="color: #FF0000">*</span>Closure Date</label>
                                                <asp:CheckBox ID="chkClosureDate" runat="server" OnCheckedChanged="chkClosureDate_CheckedChanged" AutoPostBack="true" Checked="false" />    
                                                 <asp:CustomValidator  ID="custchkClosureDate" runat="server"  OnServerValidate="custchkClosureDate_ServerValidate"  ErrorMessage="Please check Closure Date" ForeColor="Red" ValidationGroup="btnSubmit"></asp:CustomValidator> 
                                                <asp:TextBox ID="txtClosureDt" Enabled="false"  runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <div class="panel panel-success">
                                                    <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                                                        EO/ Accused Entry Details
                                                    </div>
                                                    <div class="form-group row">
                                                        <div class="col-sm-1">
                                                            <label for="ddlType">Type</label>
                                                            <asp:DropDownList ID="ddlType_D" runat="server" CssClass="form-control input-sm">
                                                                <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                                                <asp:ListItem Text="Accused" Value="Accused"></asp:ListItem>
                                                                <asp:ListItem Text="EO" Value="EO"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label for="txtEOPFNumber">EMP ID</label>
                                                            <asp:TextBox ID="txtPFNumber_D" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <label for="txtEOName">Name</label>
                                                            <asp:TextBox ID="txtName_D" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label for="txtDesignationEO">Designation</label>
                                                            <asp:TextBox ID="txtDesignation_D" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
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
                                                        <div class="col-sm-1">
                                                            <label for="txtNPANo">Related NPA No</label>
                                                            <asp:TextBox ID="txtNPANo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label for="txtIACNo">Related Case No IAC</label>
                                                            <asp:TextBox ID="txtIACNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label for="txtVigNo">Related Case No Vig.</label>
                                                            <asp:TextBox ID="txtVigNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
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
                                                                            <asp:Button ID="btnEdit_D" runat="server" CssClass="btn btn-sm btn-info" Text="Edit" CommandName="VIEW" ToolTip='<%# Eval("EO_UNIQUEID") %>' CommandArgument='<%#Eval("EO_UNIQUEID") + "~" + Eval("TYPE") + "~" + Eval("PFNO") + "~" + Eval("NAME") + "~" + Eval("DESIGNATION") + "~" + Eval("DOR") + "~" + Eval("DEALTHWITH")  + "~" + Eval("RELATEDNPANO")  + "~" + Eval("RELATEDCASENOIAC")  + "~" + Eval("RELATEDCASENOVIG")  %>' />
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
                                                                    <asp:BoundField DataField="RELATEDNPANO" HeaderText="Related NPA No" />
                                                                    <asp:BoundField DataField="RELATEDCASENOIAC" HeaderText="Related CASE NO IAC" />
                                                                    <asp:BoundField DataField="RELATEDCASENOVIG" HeaderText="Related CASE NO IAC" />
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
                                            <div class="col-sm-9">
                                                <label for="txtStatus">Status</label>
                                                <asp:TextBox ID="txtStatus" runat="server" TextMode="MultiLine" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlStatusCode">Status Code</label>
                                                <asp:DropDownList ID="ddlStatusCode" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="2IAR-FMRRec.ABBFF Ref from IAD Pending" Value="2IAR"></asp:ListItem>
                                                    <asp:ListItem Text="15-Closed" Value="15"></asp:ListItem>
                                                    <asp:ListItem Text="15A-Case Closed Pending for other Ref" Value="15A"></asp:ListItem>
                                                    <asp:ListItem Text="17-Pending at Desk" Value="17"></asp:ListItem>
                                                    <asp:ListItem Text="2C-Pending at CO" Value="2C"></asp:ListItem>
                                                    <asp:ListItem Text="2F-Pending at FRMD" Value="2F"></asp:ListItem>
                                                    <asp:ListItem Text="2H-Pending at HRD" Value="2H"></asp:ListItem>
                                                    <asp:ListItem Text="2I-Pending at IAD" Value="2I"></asp:ListItem>
                                                    <asp:ListItem Text="2IT-Pending at ITD" Value="2IT"></asp:ListItem>
                                                    <asp:ListItem Text="2M-Pending at HO(MISC)" Value="2M"></asp:ListItem>
                                                    <asp:ListItem Text="2O-Pending for Other Ref" Value="2O"></asp:ListItem>
                                                    <asp:ListItem Text="2S-Pending at SASTRA" Value="2S"></asp:ListItem>
                                                    <asp:ListItem Text="2SU-Pending at SUBS" Value="2SU"></asp:ListItem>
                                                    <asp:ListItem Text="2Z-Pending at ZO" Value="2Z"></asp:ListItem>
                                                    <asp:ListItem Text="14-Sent to ABBFF" Value="14"></asp:ListItem>
                                                    <asp:ListItem Text="14D-ABBFF Recommendation Received" Value="14D"></asp:ListItem>
                                                    <asp:ListItem Text="37-Info Recvd Under Process at Desk" Value="37"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-12">
                                                <label for="txtDeskUserRrmks"><span style="color: #FF0000"></span>Desk User Remarks</label>
                                                <asp:TextBox ID="txtDeskUserRrmks" runat="server" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                            </div>
                                            <div class="col-sm-9">
                                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" ValidationGroup="btnSubmit" OnClick="btnSubmit_Click" />
                                                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-success btn-sm" ValidationGroup="btnSubmit" OnClick="btnUpdate_Click" Visible="False" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-warning btn-sm" OnClick="btnCancel_Click" />
                                                <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </act:TabPanel>

                            <act:TabPanel ID="tabList" runat="server" HeaderText="ABBFF Entry Details">
                                <HeaderTemplate>
                                    <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="ABBFF Entry Details" ToolTip="ABBFF Entry Details"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row">
                                            <div class="col-sm-2">
                                                <label for="txtR_No_LIST">R No</label>
                                                <asp:TextBox ID="txtR_No_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <label for="txt_FMR_NO"><span style="color: #FF0000">*</span>FMR No</label>
                                                <asp:TextBox ID="txt_FMR_NO" runat="server" CssClass="form-control input-sm" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <label for="ddlZone_List">New Zone</label>
                                                <asp:DropDownList ID="ddlZone_List" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlZone_List_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlCircle_List">New Circle</label>
                                                <asp:DropDownList ID="ddlCircle_List" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-1">
                                                <label for="btnSearch">&nbsp;&nbsp;</label>
                                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-info btn-sm" Text="Search" OnClick="btnSearch_Click" />
                                            </div>
                                            <div class="col-sm-2" style="padding-top: 27px;">
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
                                                                CommandArgument='<%#Eval("RNO")%>' Text="Edit" />
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="col-sm-1" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="RNO" HeaderText="R No" />
                                                    <asp:BoundField DataField="FMR_NO" HeaderText="FMR No" />
                                                    <asp:BoundField DataField="NEW_ZONE" HeaderText="NEW ZONE" />
                                                    <asp:BoundField DataField="NEW_CIRCLE" HeaderText="NEW CIRCLE" />
                                                    <asp:BoundField DataField="ADDUSER" HeaderText="ENTERED BY" />
                                                    <asp:BoundField DataField="ADDDATE" HeaderText="ENTERED TIME" />
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
