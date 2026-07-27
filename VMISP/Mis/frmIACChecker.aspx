<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/SiteMaster.Master"
    CodeBehind="frmIACChecker.aspx.cs"
    Inherits="VMISP.Mis.frmIACChecker" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">

    <title>IAC Checker</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />

    <style>

        #custom-heading { color: white; }

        .page-header h3, .page-header h3 * { color:#fff !important; }

        .table td .btn-primary, .table td .btn-primary * { color:#fff !important; }

        body{
            background:#f5f6fa;
        }

        :root{
            --primary:#9F2925;
            --primary-dark:#7d1d1a;
        }

        .page-header{
            background:linear-gradient(90deg,var(--primary),#b73d38);
            color:#fff;
            padding:22px;
            border-radius:12px;
            margin-bottom:25px;
        }

        .page-header h3{
            margin:0;
            font-weight:600;
        }

        .card-custom{
            border:none;
            border-radius:12px;
            box-shadow:0 8px 20px rgba(0,0,0,.08);
        }

        .table thead{
            background:var(--primary);
            color:white;
        }

        .table-hover tbody tr:hover{
            background:#fff7f6;
        }

        .btn-primary{
            background:var(--primary);
            border-color:var(--primary);
        }

        .btn-primary:hover{
            background:var(--primary-dark);
            border-color:var(--primary-dark);
        }

        .badge-pending{
            background:#ffc107;
            color:#000;
        }

        .badge-progress{
            background:#0d6efd;
        }

        .badge-closed{
            background:#198754;
        }

        .search-box{
            max-width:350px;
        }

        .table td{
            vertical-align:middle;
        }

        .table tbody tr{
            transition:.2s;
        }

        .table tbody tr:hover{
            transform:scale(1.003);
        }

    </style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">

<div class="container-fluid p-4">

    <div class="page-header">

        <div class="d-flex justify-content-between align-items-center">

            <div>

                <h3><i id="custom-heading" style="color:white" class="bi bi-shield-check text-white"></i> IAC Checker</h3>

                <small>Review and verify IAC records assigned to you</small>

            </div>

            <div>

                <span class="badge bg-light text-dark fs-6">

                    Total :
                    <asp:Label ID="lblTotal" runat="server" Text="0"></asp:Label>

                </span>

            </div>

        </div>

    </div>

    <div class="card card-custom">

        <div class="card-body">

            <asp:Label ID="lblMsg" runat="server" CssClass="d-block mb-3 fw-semibold text-danger" EnableViewState="false" />

            <div class="row mb-3">

                <div class="col-md-4">

                    <input type="text"
                        id="txtSearch"
                        class="form-control search-box"
                        placeholder="Search IAC..." />

                </div>

            </div>

            <div class="table-responsive">

                <asp:GridView ID="gvIAC"
                    runat="server"
                    CssClass="table table-hover align-middle"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    EmptyDataText="No IAC records assigned to you.">

                    <Columns>

                        <asp:BoundField HeaderText="IAC No" DataField="RecordRef" />

                        <asp:BoundField HeaderText="Zone" DataField="ZoneSolID" />

                        <asp:BoundField HeaderText="Submitted By" DataField="MakerUser" />

                        <asp:BoundField HeaderText="Submitted On"
                            DataField="MakerDate"
                            DataFormatString="{0:dd-MMM-yyyy}" />

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='badge <%# GetStatusClass(Eval("ApprovalStatus").ToString()) %>'>
                                    <%# GetStatusText(Eval("ApprovalStatus").ToString()) %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <a class="btn btn-primary btn-sm"
                                   href='frmIACCheckerView.aspx?id=<%# Eval("RecordCode") %>'>
                                    <i class="bi bi-eye"></i> View
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

        </div>

    </div>

</div>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/js/bootstrap.bundle.min.js"></script>

<script>

document.getElementById('txtSearch').addEventListener('keyup', function () {

    var value = this.value.toLowerCase();

    document.querySelectorAll("#<%=gvIAC.ClientID%> tbody tr").forEach(function (row) {

        row.style.display =
            row.innerText.toLowerCase().indexOf(value) > -1 ? "" : "none";

    });

});

</script>

</asp:Content>
