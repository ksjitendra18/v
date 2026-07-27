<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmIACCheckerView.aspx.cs" Inherits="VMISP.Mis.frmIACCheckerView" ValidateRequest="false" %>

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
                    IAC Verification
                </div>
                <br />
                <div class="col-sm-12 alert alert-dark">

                    <%-- Record header: which record, where it stands, who sent it --%>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label>IAC Number</label>
                            <asp:Label ID="lblIACNo" runat="server" CssClass="form-control input-sm" Text="-"></asp:Label>
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

                    <%-- Fields below follow the IAC Entry form's order and labels exactly, so a
                         checker reads the record in the same shape the maker keyed it in. --%>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtIACNo">IAC No</label>
                            <asp:TextBox ID="txtIACNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtRecDate">Received Date</label>
                            <asp:TextBox ID="txtRecDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtCircleOffice">Circle Name</label>
                            <asp:TextBox ID="txtCircleOffice" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtBranch">Branch Name</label>
                            <asp:TextBox ID="txtBranch" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtVIGNo">Vigilance Number</label>
                            <asp:TextBox ID="txtVIGNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtClosureDate">Closure Date</label>
                            <asp:TextBox ID="txtClosureDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtAccused">Accused</label>
                            <asp:TextBox ID="txtAccused" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtDAView">DA View</label>
                            <asp:TextBox ID="txtDAView" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtMeetNo">Meet Number</label>
                            <asp:TextBox ID="txtMeetNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtRetDate">Ret Date</label>
                            <asp:TextBox ID="txtRetDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtIACView">IAC View</label>
                            <asp:TextBox ID="txtIACView" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtZone">Zone</label>
                            <asp:TextBox ID="txtZone" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtSource">Source</label>
                            <asp:TextBox ID="txtSource" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtDA">DA</label>
                            <asp:TextBox ID="txtDA" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtCVOView">CVO View</label>
                            <asp:TextBox ID="txtCVOView" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtAccountName">Account Name</label>
                            <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtAmount">Amount</label>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtIACNo1">IAC No-1</label>
                            <asp:TextBox ID="txtIACNo1" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtPFNumber">PF Number</label>
                            <asp:TextBox ID="txtPFNumber" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtStatusCode">Status Code</label>
                            <asp:TextBox ID="txtStatusCode" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtNature">Nature Case</label>
                            <asp:TextBox ID="txtNature" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtBank">Bank Name</label>
                            <asp:TextBox ID="txtBank" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtLetterSentToDADate">Letter Sent to DA Date</label>
                            <asp:TextBox ID="txtLetterSentToDADate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtLetterSentDate">Letter Sent Date</label>
                            <asp:TextBox ID="txtLetterSentDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
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
                            <label for="txtTMSACRefNo">TMSAC Reference Number</label>
                            <asp:TextBox ID="txtTMSACRefNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtDesignation">Designation</label>
                            <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtScale">Scale</label>
                            <asp:TextBox ID="txtScale" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtNewZone">New Zone</label>
                            <asp:TextBox ID="txtNewZone" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtNewCircle">New Circle</label>
                            <asp:TextBox ID="txtNewCircle" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtABBFFCase">ABBFF Case</label>
                            <asp:TextBox ID="txtABBFFCase" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtABBFFCaseSubmissionDate">ABBFF Case Submission date</label>
                            <asp:TextBox ID="txtABBFFCaseSubmissionDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtABBFFReplyDate">ABBFF Reply Date</label>
                            <asp:TextBox ID="txtABBFFReplyDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtABBFFRefNo">ABBFF Reference Number</label>
                            <asp:TextBox ID="txtABBFFRefNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtABBFFAdviceReceiveDate">ABBFF Advice Receive Date</label>
                            <asp:TextBox ID="txtABBFFAdviceReceiveDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-9">
                            <label for="txtABBFFAdviceDetail">ABBFF Advice Detail</label>
                            <asp:TextBox ID="txtABBFFAdviceDetail" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
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
                                        Please verify the IAC record carefully before taking any action.
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
                    NavigateUrl="~/Mis/frmIACChecker.aspx">&#8592; Back to Inbox</asp:HyperLink>
                <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
            </div>
            <div class="col-sm-6 text-right">
                <asp:Panel ID="pnlActions" runat="server" style="display: inline;">
                    <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-danger"
                        OnClick="btnReject_Click" OnClientClick="return validateCheckerAction('Reject this IAC record?');" />
                    <asp:Button ID="btnPushBack" runat="server" Text="Push Back" CssClass="btn btn-warning"
                        OnClick="btnPushBack_Click" OnClientClick="return validateCheckerAction('Push back this IAC record for correction?');" />
                    <asp:Button ID="btnAccept" runat="server" Text="Accept" CssClass="btn btn-success"
                        OnClick="btnAccept_Click" OnClientClick="return validateCheckerAction('Approve this IAC record?');" />
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
