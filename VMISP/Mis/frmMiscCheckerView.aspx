<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmMiscCheckerView.aspx.cs" Inherits="VMISP.Mis.frmMiscCheckerView" ValidateRequest="false" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style>
        /* Read-only fields keep the entry form's shape but read as "not editable". */
        .form-control[readonly] {
            background-color: #f5f5f5;
            cursor: default;
        }

        .chkDecision {
            background-color: #fcf8e3;
        }

        /* Action bar pinned to the bottom of the viewport, so Accept / Push Back / Reject stay
           reachable however far down the record the checker has scrolled. Hand-rolled rather
           than Bootstrap 5's .sticky-bottom, because this page runs on Bootstrap 3 to match the
           entry form and loading both would break the grid. */
        .checker-action-bar {
            position: fixed;
            left: 0;
            right: 0;
            bottom: 0;
            background: #fff;
            border-top: 1px solid #ddd;
            box-shadow: 0 -2px 10px rgba(0,0,0,.15);
            padding: 10px 20px;
            z-index: 1000;
        }

            .checker-action-bar .btn {
                min-width: 110px;
            }

        /* Keep the last fields clear of the fixed bar. */
        .checker-page {
            padding-bottom: 80px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <script src="/Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="/Js/jquery-3.3.1.min.js" type="text/javascript"></script>
    <link href="/css/bootstrap.css" rel="stylesheet" />

    <div class="col-lg-12 checker-page">
        <div class="form-group row">
            <div class="panel panel-primary">
                <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                    MISC Verification
                </div>
                <br />
                <div class="col-sm-12 alert alert-dark">

                    <%-- Record header: which record, where it stands, who sent it --%>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label>R Number</label>
                            <asp:Label ID="lblRNo" runat="server" CssClass="form-control input-sm" Text="-"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <label>Checker Status</label>
                            <div>
                                <span id="spanStatus" runat="server" class="label label-warning" style="display: inline-block; padding: 6px 12px; font-size: 13px;">
                                    <asp:Label ID="lblStatus" runat="server" Text="Pending"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <label>Submitted By</label>
                            <asp:Label ID="lblMaker" runat="server" CssClass="form-control input-sm" Text="-"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <label>Submitted On</label>
                            <asp:Label ID="lblMakerDate" runat="server" CssClass="form-control input-sm" Text="-"></asp:Label>
                        </div>
                    </div>

                    <hr />

                    <%-- Fields below follow the MISC Entry form's order and labels exactly, so a
                         checker reads the record in the same shape the maker keyed it in. --%>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtRNo">R No</label>
                            <asp:TextBox ID="txtRNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtCompRecDate">Complaint Receive Date</label>
                            <asp:TextBox ID="txtCompRecDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <label for="txtCircleOffice">Circle</label>
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
                            <label for="txtCompNo">Comp Number</label>
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
                            <label for="txtAmount">Amount</label>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control input-sm" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtNPADate">NPA Date</label>
                            <asp:TextBox ID="txtNPADate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtFinalAction">Final Action</label>
                            <asp:TextBox ID="txtFinalAction" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtSource">Source</label>
                            <asp:TextBox ID="txtSource" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtZone">Zone</label>
                            <asp:TextBox ID="txtZone" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
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
                            <label for="txtAccountName">Account Name</label>
                            <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtClose">Close</label>
                            <asp:TextBox ID="txtClose" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtDateForINVReport">Date for Inv Report</label>
                            <asp:TextBox ID="txtDateForINVReport" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtDesignation">Designation</label>
                            <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtNatureComp">Nature Comp</label>
                            <asp:TextBox ID="txtNatureComp" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtInvestigationDate">Investigation Date</label>
                            <asp:TextBox ID="txtInvestigationDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtType">Type</label>
                            <asp:TextBox ID="txtType" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtStatusCode">Status Code</label>
                            <asp:TextBox ID="txtStatusCode" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtNature">Nature</label>
                            <asp:TextBox ID="txtNature" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-12">
                            <label for="txtRessonsForClosure">Closure Ressons</label>
                            <asp:TextBox ID="txtRessonsForClosure" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtLetterSentDate">Letter Sent Date</label>
                            <asp:TextBox ID="txtLetterSentDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
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
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtBankName">Bank Name</label>
                            <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtPFNumber">Accused PF Number</label>
                            <asp:TextBox ID="txtPFNumber" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtZoneNew">New Zone</label>
                            <asp:TextBox ID="txtZoneNew" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtCircleNew">New Circle</label>
                            <asp:TextBox ID="txtCircleNew" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtZoneType">Type</label>
                            <asp:TextBox ID="txtZoneType" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-9">
                            <label for="txtZOCM">Chief Manager Name</label>
                            <asp:TextBox ID="txtZOCM" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-12">
                            <label for="txtStatus">Status</label>
                            <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-12">
                            <label for="txtDeskUserRemarks">Dealing Officer Remarks</label>
                            <asp:TextBox ID="txtDeskUserRemarks" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>

                    <%-- Checker Decision --%>
                    <asp:Panel ID="pnlDecision" runat="server">
                        <hr />
                        <asp:Panel ID="pnlVerifyNote" runat="server">
                            <div class="form-group row" style="padding-right: 5px;">
                                <div class="col-sm-12">
                                    <div class="alert alert-warning" style="margin-bottom: 10px;">
                                        Please verify the MISC record carefully before taking any action.
                                        All actions performed here will be recorded in the audit trail.
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-group row" style="padding-right: 5px;">
                            <div class="col-sm-12">
                                <label for="txtCheckerRemarks"><span style="color: #FF0000">*</span>Checker Remarks</label>
                                <asp:TextBox ID="txtCheckerRemarks" runat="server" CssClass="form-control input-sm chkDecision"
                                    TextMode="MultiLine" Rows="4" placeholder="Enter your remarks...."></asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>

                </div>
            </div>
        </div>
    </div>

    <%-- Pinned action bar. Stays visible while the checker scrolls the record.
         lblMsg sits outside pnlActions so outcomes and errors still show once the
         record has been actioned and the buttons are gone. --%>
    <div class="checker-action-bar">
        <div class="row">
            <div class="col-sm-6">
                <asp:HyperLink ID="lnkBack" runat="server" CssClass="btn btn-default btn-sm"
                    NavigateUrl="~/Mis/frmMiscChecker.aspx">&#8592; Back to Inbox</asp:HyperLink>
                <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
            </div>
            <div class="col-sm-6 text-right">
                <asp:Panel ID="pnlActions" runat="server" style="display: inline;">
                    <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-danger"
                        OnClick="btnReject_Click" OnClientClick="return validateCheckerAction('Reject this MISC record?');" />
                    <asp:Button ID="btnPushBack" runat="server" Text="Push Back" CssClass="btn btn-warning"
                        OnClick="btnPushBack_Click" OnClientClick="return validateCheckerAction('Push back this MISC record for correction?');" />
                    <asp:Button ID="btnAccept" runat="server" Text="Accept" CssClass="btn btn-success"
                        OnClick="btnAccept_Click" OnClientClick="return validateCheckerAction('Approve this MISC record?');" />
                </asp:Panel>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function validateCheckerAction(confirmMsg) {
            var remarksBox = document.getElementById('<%= txtCheckerRemarks.ClientID %>');
            if (!remarksBox || remarksBox.value.replace(/^\s+|\s+$/g, '') === '') {
                alert('Checker Remarks are mandatory before taking any action.');
                if (remarksBox) { remarksBox.focus(); }
                return false;
            }
            return confirm(confirmMsg);
        }
    </script>
</asp:Content>
