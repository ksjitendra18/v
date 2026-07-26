<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmComplaintView.aspx.cs" Inherits="VMISP.Mis.frmComplaintView" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style>
        .hideBranchScoreMarks {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <link href="/css/bootstrap.css" rel="stylesheet" />
    <div class="col-lg-12">
        <div class="form-group row">
            <div class="panel panel-primary">
                <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                    Complaint View (Read Only)
                </div>
                <br />
                <div class="col-sm-12 alert alert-dark">
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-2">
                            <label for="txtRNo">Complaint No</label>
                            <asp:TextBox ID="txtRNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtCompRecDate">Complaint Date</label>
                            <asp:TextBox ID="txtCompRecDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <label for="txtCircleOffice">Circle Office</label>
                            <asp:TextBox ID="txtCircleOffice" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-12">
                            <label for="txtBRComplaint">Branch Complaint</label>
                            <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtCompNo">Internal Ref No</label>
                            <asp:TextBox ID="txtCompNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtClosureDate">Closure Date</label>
                            <asp:TextBox ID="txtClosureDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <label for="txtAccused">Accused</label>
                            <asp:TextBox ID="txtAccused" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-12">
                            <label for="txtAllegations">Allegations</label>
                            <asp:TextBox ID="txtAllegations" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtCaseNo">Case/IAC No</label>
                            <asp:TextBox ID="txtCaseNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtIACDate">IAC Date</label>
                            <asp:TextBox ID="txtIACDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtPresentPosting">Present Posting</label>
                            <asp:TextBox ID="txtPresentPosting" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtZone">State</label>
                            <asp:TextBox ID="txtZone" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtSentTo">Sent To</label>
                            <asp:TextBox ID="txtSentTo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtSourceDate">Source Date</label>
                            <asp:TextBox ID="txtSourceDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtSourceRef">Source Ref</label>
                            <asp:TextBox ID="txtSourceRef" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtAmount">Amount</label>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control input-sm" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-12">
                            <label for="txtAccountName">Account Name</label>
                            <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtSentForInvDate">Sent for Inv</label>
                            <asp:TextBox ID="txtSentForInvDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtSource">External Source</label>
                            <asp:TextBox ID="txtSource" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtRegion">Region</label>
                            <asp:TextBox ID="txtRegion" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtClose">Close</label>
                            <asp:TextBox ID="txtClose" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtDateForINVReport">For Inv Report</label>
                            <asp:TextBox ID="txtDateForINVReport" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtDesignation">Designation</label>
                            <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtNameINVOfficial">Name INV Official</label>
                            <asp:TextBox ID="txtNameINVOfficial" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtRYSent">RY Sent</label>
                            <asp:TextBox ID="txtRYSent" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-6">
                            <label for="txtStatusCode">Status Code</label>
                            <asp:TextBox ID="txtStatusCode" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtPFNumber">Accused PF Number</label>
                            <asp:TextBox ID="txtPFNumber" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtLetterSentDate">Letter Sent Date</label>
                            <asp:TextBox ID="txtLetterSentDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-12">
                            <label for="txtRessonsForClosure">Closure Reasons</label>
                            <asp:TextBox ID="txtRessonsForClosure" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtLetterSentTo">Letter Sent To</label>
                            <asp:TextBox ID="txtLetterSentTo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtReminderDate">Reminder Date</label>
                            <asp:TextBox ID="txtReminderDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtReplyReceivedDate">Reply Received Date</label>
                            <asp:TextBox ID="txtReplyReceivedDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtBankName">Bank Name</label>
                            <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-6">
                            <label for="txtZoneNew">New Zone</label>
                            <asp:TextBox ID="txtZoneNew" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <label for="txtCircleNew">New Circle</label>
                            <asp:TextBox ID="txtCircleNew" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-12">
                            <div class="panel panel-success">
                                <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                                    EO/ Accused Entry Details
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <asp:GridView ID="gvEODetails" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered input-sm table-condensed">
                                            <Columns>
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
                                            </Columns>
                                            <HeaderStyle BackColor="darkseagreen" />
                                            <RowStyle BackColor="White" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <label for="txtStatus">Status</label>
                            <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <label for="txtDealingOfficerRemarks">Dealing Officer Remarks</label>
                            <asp:TextBox ID="txtDealingOfficerRemarks" runat="server" TextMode="MultiLine" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <label for="txtCheckerStatus">Checker Status</label>
                            <asp:TextBox ID="txtCheckerStatus" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <label for="txtCheckerRemarks">Checker Remarks</label>
                            <asp:TextBox ID="txtCheckerRemarks" runat="server" TextMode="MultiLine" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                        </div>
                        <div class="col-sm-9">
                            <asp:Button ID="btnBack" runat="server" Text="Back to List" CssClass="btn btn-warning btn-sm" OnClick="btnBack_Click" />
                            <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
