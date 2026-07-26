<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/SiteMaster.Master"
    CodeBehind="frmComplaintCheckerView.aspx.cs"
    Inherits="VMISP.Mis.frmComplaintCheckerView" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <title>Complaint Checker</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />

    <style>
        :root {
            --primary: #9F2925;
            --primary-dark: #7d1d1a;
            --light: #f8f9fa;
        }

        body {
            background: #f5f6fa;
        }

        .page-header {
            background: linear-gradient(90deg,var(--primary),#b73d38);
            color: #fff;
            padding: 22px;
            border-radius: 12px;
            margin-bottom: 25px;
        }

        .page-header h2, .page-header h2 *, .page-header h4, .page-header h4 * {
            color: #fff !important;
        }

        .card-custom {
            border: none;
            border-radius: 12px;
            box-shadow: 0 8px 20px rgba(0,0,0,.08);
            margin-bottom: 25px;
        }

            .card-custom .card-header {
                background: #fff;
                font-weight: 600;
                color: var(--primary);
                border-bottom: 2px solid #f1f1f1;
            }

        .btn-primary {
            background: var(--primary);
            border-color: var(--primary);
        }

        .btn-primary:hover {
            background: var(--primary-dark);
            border-color: var(--primary-dark);
        }

        .badge-pending {
            background: #ffc107;
            color: #000;
        }

        .badge-progress {
            background: #0d6efd;
        }

        .badge-closed {
            background: #198754;
        }

        .form-control,
        .form-select {
            background: #fafafa;
            min-height: 42px;
        }

        textarea.form-control {
            min-height: 100px;
        }

        label {
            font-weight: 600;
            color: #555;
        }

        .readonly {
            background: #f8f9fa !important;
        }

        .badge-status {
            font-size: .9rem;
            padding: 8px 15px;
        }


        .sticky-bottom{

    z-index:999;

}

.btn-success{

    background:#198754;
    border:none;

}

.btn-danger{

    border:none;

}

.btn-warning{

    color:#fff;

}

.form-control:read-only{

    background:#f8f9fa;

}
    </style>

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <div class="container-fluid py-4">

        <div class="page-header">

            <div class="row">

                <div class="col-md-8">

                    <h2 class="mb-2">

                        <i class="bi bi-shield-check"></i>

                        Complaint Verification

                    </h2>

                    <p class="mb-0">
                        Review complaint before taking an action.

                    </p>

                </div>

                <div class="col-md-4 text-end">

                    <h4>Complaint #

                        <asp:Label ID="lblComplaintNo"
                            runat="server"
                            Text="-"></asp:Label>

                    </h4>

                    <span id="spanStatus" runat="server" class="badge badge-status">

                        <asp:Label ID="lblStatus"
                            runat="server"
                            Text="Pending"></asp:Label>

                    </span>

                </div>

            </div>

        </div>

        <div class="card card-custom">

            <div class="card-header">

                <i class="bi bi-info-circle"></i>

                Complaint Summary

            </div>

            <div class="card-body">

                <div class="row g-3">

                    <div class="col-md-3">

                        <label>Complaint Date</label>

                        <asp:TextBox
                            ID="txtComplaintDate"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-3">

                        <label>Circle Office</label>

                        <asp:TextBox
                            ID="txtCircleOffice"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-3">

                        <label>Internal Ref No</label>

                        <asp:TextBox
                            ID="txtInternalRef"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-3">

                        <label>Closure Date</label>

                        <asp:TextBox
                            ID="txtClosureDate"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                </div>

            </div>

        </div>


        <div class="card card-custom">

            <div class="card-header">

                <i class="bi bi-file-earmark-text"></i>

                Complaint Details

            </div>

            <div class="card-body">

                <div class="row g-3">

                    <div class="col-md-6">

                        <label>Branch Complaint</label>

                        <asp:TextBox
                            ID="txtBranchComplaint"
                            runat="server"
                            TextMode="MultiLine"
                            CssClass="form-control readonly"
                            Rows="4"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-6">

                        <label>Accused</label>

                        <asp:TextBox
                            ID="txtAccused"
                            runat="server"
                            TextMode="MultiLine"
                            Rows="4"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-12">

                        <label>Allegations</label>

                        <asp:TextBox
                            ID="txtAllegations"
                            runat="server"
                            TextMode="MultiLine"
                            Rows="5"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                </div>

            </div>

        </div>



        <div class="card card-custom">

            <div class="card-header">

                <i class="bi bi-search"></i>

                Investigation Details

            </div>

            <div class="card-body">

                <div class="row g-3">

                    <div class="col-md-3">

                        <label>Case / IAC No</label>

                        <asp:TextBox
                            ID="txtCaseNo"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-3">

                        <label>IAC Date</label>

                        <asp:TextBox
                            ID="txtIACDate"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-3">

                        <label>Present Posting</label>

                        <asp:TextBox
                            ID="txtPresentPosting"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-3">

                        <label>State</label>

                        <asp:TextBox
                            ID="txtState"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-3">

                        <label>Sent To</label>

                        <asp:TextBox
                            ID="txtSentTo"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-3">

                        <label>Source Date</label>

                        <asp:TextBox
                            ID="txtSourceDate"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-3">

                        <label>Source Reference</label>

                        <asp:TextBox
                            ID="txtSourceReference"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-3">

                        <label>Amount</label>

                        <asp:TextBox
                            ID="txtAmount"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-12">

                        <label>Account Name</label>

                        <asp:TextBox
                            ID="txtAccountName"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-4">

                        <label>External Source</label>

                        <asp:TextBox
                            ID="txtExternalSource"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-4">

                        <label>Region</label>

                        <asp:TextBox
                            ID="txtRegion"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-4">

                        <label>Close</label>

                        <asp:TextBox
                            ID="txtClose"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />

                    </div>

                </div>

            </div>

        </div>

        <div class="card card-custom">

            <div class="card-header">
                <i class="bi bi-person-badge"></i>
                Official Details
            </div>

            <div class="card-body">

                <div class="row g-3">

                    <div class="col-md-3">
                        <label>Investigation Report Date</label>
                        <asp:TextBox ID="txtINVReportDate"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />
                    </div>

                    <div class="col-md-3">
                        <label>Designation</label>
                        <asp:TextBox ID="txtDesignation"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />
                    </div>

                    <div class="col-md-3">
                        <label>Investigation Officer</label>
                        <asp:TextBox ID="txtINVOfficer"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />
                    </div>

                    <div class="col-md-3">
                        <label>RY Sent Date</label>
                        <asp:TextBox ID="txtRYSent"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />
                    </div>

                    <div class="col-md-6">
                        <label>Status Code</label>
                        <asp:TextBox ID="txtStatusCode"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />
                    </div>

                    <div class="col-md-3">
                        <label>Accused PF Number</label>
                        <asp:TextBox ID="txtPFNumber"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />
                    </div>

                    <div class="col-md-3">
                        <label>Letter Sent Date</label>
                        <asp:TextBox ID="txtLetterSentDate"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />
                    </div>

                    <div class="col-md-12">
                        <label>Closure Reason</label>
                        <asp:TextBox ID="txtClosureReason"
                            runat="server"
                            CssClass="form-control readonly"
                            TextMode="MultiLine"
                            Rows="3"
                            ReadOnly="true" />
                    </div>

                </div>

            </div>

        </div>

        <div class="card card-custom">

            <div class="card-header">
                <i class="bi bi-envelope-paper"></i>
                Communication Details
            </div>

            <div class="card-body">

                <div class="row g-3">

                    <div class="col-md-3">
                        <label>Letter Sent To</label>
                        <asp:TextBox ID="txtLetterSentTo"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />
                    </div>

                    <div class="col-md-3">
                        <label>Reminder Date</label>
                        <asp:TextBox ID="txtReminderDate"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />
                    </div>

                    <div class="col-md-3">
                        <label>Reply Received Date</label>
                        <asp:TextBox ID="txtReplyReceivedDate"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />
                    </div>

                    <div class="col-md-3">
                        <label>Bank</label>
                        <asp:TextBox ID="txtBank"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />
                    </div>

                    <div class="col-md-6">
                        <label>New Zone</label>
                        <asp:TextBox ID="txtNewZone"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />
                    </div>

                    <div class="col-md-6">
                        <label>New Circle</label>
                        <asp:TextBox ID="txtNewCircle"
                            runat="server"
                            CssClass="form-control readonly"
                            ReadOnly="true" />
                    </div>

                </div>

            </div>

        </div>

        <div class="card card-custom">

            <div class="card-header">

                <i class="bi bi-people"></i>

                EO / Accused Details

            </div>

            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView
                        ID="gvEODetails"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-bordered table-hover align-middle">

                        <Columns>

                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField
                                HeaderText="Type"
                                DataField="TYPE" />

                            <asp:BoundField
                                HeaderText="PF Number"
                                DataField="PFNO" />

                            <asp:BoundField
                                HeaderText="Name"
                                DataField="NAME" />

                            <asp:BoundField
                                HeaderText="Designation"
                                DataField="DESIGNATION" />

                            <asp:BoundField
                                HeaderText="Retirement Date"
                                DataField="DOR" />

                            <asp:BoundField
                                HeaderText="Dealt With"
                                DataField="DEALTHWITH" />

                        </Columns>

                    </asp:GridView>

                </div>

            </div>

        </div>

        <div class="card card-custom">

            <div class="card-header">

                <i class="bi bi-clipboard-check"></i>

                Complaint Status

            </div>

            <div class="card-body">

                <div class="row g-3">

                    <div class="col-md-12">

                        <label>Status</label>

                        <asp:TextBox
                            ID="txtStatus"
                            runat="server"
                            CssClass="form-control readonly"
                            TextMode="MultiLine"
                            Rows="4"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-12">

                        <label>HO Status</label>

                        <asp:TextBox
                            ID="txtHOStatus"
                            runat="server"
                            CssClass="form-control readonly"
                            TextMode="MultiLine"
                            Rows="3"
                            ReadOnly="true" />

                    </div>

                    <div class="col-md-12">

                        <label>Dealing Officer Remarks</label>

                        <asp:TextBox
                            ID="txtDealingOfficerRemarks"
                            runat="server"
                            CssClass="form-control readonly"
                            TextMode="MultiLine"
                            Rows="5"
                            ReadOnly="true" />

                    </div>

                </div>

            </div>

        </div>


        <div class="card card-custom mb-5">

            <div class="card-header bg-white">

                <i class="bi bi-pencil-square text-danger"></i>

                <strong>Checker Decision</strong>

            </div>

            <div class="card-body">

                <asp:Label ID="lblMsg" runat="server" CssClass="d-block mb-3 fw-semibold" EnableViewState="false" />

                <asp:Panel ID="pnlDecision" runat="server">

                <div class="alert alert-warning">

                    <i class="bi bi-exclamation-triangle-fill"></i>

                    Please verify the complaint carefully before taking any action.
            All actions performed here will be recorded in the audit trail.

                </div>

                <div class="row">

                    <div class="col-md-12">

                        <label class="form-label fw-semibold">
                            Checker Remarks
                    <span class="text-danger">*</span>

                        </label>

                        <asp:TextBox
                            ID="txtCheckerRemarks"
                            runat="server"
                            TextMode="MultiLine"
                            Rows="6"
                            CssClass="form-control"
                            placeholder="Enter your remarks..." />

                    </div>

                </div>

                </asp:Panel>

            </div>

        </div>

        <asp:Panel ID="pnlActions" runat="server" CssClass="sticky-bottom bg-white border-top shadow-lg p-3">

            <div class="container-fluid">

                <div class="d-flex justify-content-end gap-2">

                    <asp:Button
                        ID="btnReject"
                        runat="server"
                        CssClass="btn btn-danger btn-lg px-4"
                        Text="Reject"
                        OnClick="btnReject_Click"
                        OnClientClick="return validateCheckerAction('Reject this complaint?');" />

                    <asp:Button
                        ID="btnPushBack"
                        runat="server"
                        CssClass="btn btn-warning btn-lg px-4"
                        Text="Push Back"
                        OnClick="btnPushBack_Click"
                        OnClientClick="return validateCheckerAction('Push back this complaint for correction?');" />

                    <asp:Button
                        ID="btnAccept"
                        runat="server"
                        CssClass="btn btn-success btn-lg px-5"
                        Text="Accept"
                        OnClick="btnAccept_Click"
                        OnClientClick="return validateCheckerAction('Approve this complaint?');" />

                </div>

            </div>

        </asp:Panel>

        <script>
            function validateCheckerAction(confirmMsg) {
                var remarksBox = document.getElementById('<%= txtCheckerRemarks.ClientID %>');
                if (!remarksBox || remarksBox.value.trim() === '') {
                    alert('Checker Remarks are mandatory before taking any action.');
                    if (remarksBox) { remarksBox.focus(); }
                    return false;
                }
                return confirm(confirmMsg);
            }
        </script>
</asp:Content>
