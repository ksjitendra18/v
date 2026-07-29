<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmVigilanceCheckerView.aspx.cs" Inherits="VMISP.Mis.frmVigilanceCheckerView" ValidateRequest="false" %>

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
           reachable however far down the record the checker has scrolled. A Vigilance record is
           the longest form in the application, so this matters more here than anywhere else.
           Hand-rolled rather than Bootstrap 5's .sticky-bottom, because this page runs on
           Bootstrap 3 to match the entry form and loading both would break the grid. */
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
                    Vigilance Verification
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

                    <%-- Fields below follow the Vigilance Entry form's order and labels exactly, so
                         a checker reads the record in the same shape the maker keyed it in. --%>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtRNo">R Number</label>
                            <asp:TextBox ID="txtRNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtRNo1">R Number 1</label>
                            <asp:TextBox ID="txtRNo1" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtNameOfParticulars">Name &amp; Particulars</label>
                            <asp:TextBox ID="txtNameOfParticulars" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtName">Name</label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtChargeDate">Charge Date</label>
                            <asp:TextBox ID="txtChargeDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtNatCHSheet">Nature of Charge Sheet</label>
                            <asp:TextBox ID="txtNatCHSheet" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtZone">Zone</label>
                            <asp:TextBox ID="txtZone" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtStatusCode">Status Code</label>
                            <asp:TextBox ID="txtStatusCode" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtRegister">Register</label>
                            <asp:TextBox ID="txtRegister" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtCircleOffice">Circle</label>
                            <asp:TextBox ID="txtCircleOffice" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtFinal">Final</label>
                            <asp:TextBox ID="txtFinal" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtScale">Scale</label>
                            <asp:TextBox ID="txtScale" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtRNoDate">R No Date</label>
                            <asp:TextBox ID="txtRNoDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtPFNo">PF Number</label>
                            <asp:TextBox ID="txtPFNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtRetirementDate">Retirement Date</label>
                            <asp:TextBox ID="txtRetirementDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtDAOrdDate">DA Order Date</label>
                            <asp:TextBox ID="txtDAOrdDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtNAPUNDA">Nature of Punishment of DA</label>
                            <asp:TextBox ID="txtNAPUNDA" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtPenaltyType">Penalty Type</label>
                            <asp:TextBox ID="txtPenaltyType" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtDisAuthoritysCircle">DA_CO/ZO/HO</label>
                            <asp:TextBox ID="txtDisAuthoritysCircle" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtDispAuthority">Disp Authority</label>
                            <asp:TextBox ID="txtDispAuthority" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtIstDaDate">Ist DA Date</label>
                            <asp:TextBox ID="txtIstDaDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtDAProposal">DA Proposal</label>
                            <asp:TextBox ID="txtDAProposal" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtFinalDate">Final Date</label>
                            <asp:TextBox ID="txtFinalDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtCVOAdvice">CVO Advice</label>
                            <asp:TextBox ID="txtCVOAdvice" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtCVOAdviceDate">CVO Advice Date</label>
                            <asp:TextBox ID="txtCVOAdviceDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txt2ndDADate">2nd DA Date</label>
                            <asp:TextBox ID="txt2ndDADate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txt2DAProposal">2DA Proposal</label>
                            <asp:TextBox ID="txt2DAProposal" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtCVO2Advice">CVO 2 Advice</label>
                            <asp:TextBox ID="txtCVO2Advice" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtCVO2AdviceDate">CVO 2 Advice Date</label>
                            <asp:TextBox ID="txtCVO2AdviceDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtAccountName">Account Name</label>
                            <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtSource">Source</label>
                            <asp:TextBox ID="txtSource" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtState">State</label>
                            <asp:TextBox ID="txtState" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtPlaceinPresentScaleDate">Place in Present Scale From Date</label>
                            <asp:TextBox ID="txtPlaceinPresentScaleDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtSanctionRefusedDate">Refused Date</label>
                            <asp:TextBox ID="txtSanctionRefusedDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtDesignation">Designation</label>
                            <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtPunishmentProposedbyDA">DA Proposed Punishment</label>
                            <asp:TextBox ID="txtPunishmentProposedbyDA" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtCompRecDate">Supplementary C/S Date</label>
                            <asp:TextBox ID="txtCompRecDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtStatusinBrief">Supplementary C/S Status</label>
                            <asp:TextBox ID="txtStatusinBrief" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtBRComplaint">Branch</label>
                            <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtPenalty">Penalty</label>
                            <asp:TextBox ID="txtPenalty" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtAmount">Amount</label>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtCSOREPDate">Date of CSO REP.</label>
                            <asp:TextBox ID="txtCSOREPDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtConEnqDate">Conduct Enquiry Date</label>
                            <asp:TextBox ID="txtConEnqDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtSuspensionDate">Suspension Date</label>
                            <asp:TextBox ID="txtSuspensionDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtCbiRcNo1">CBI RC NO1</label>
                            <asp:TextBox ID="txtCbiRcNo1" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtRC1Date">RC1 Date</label>
                            <asp:TextBox ID="txtRC1Date" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtCBIRCNo2">CBI RC No2</label>
                            <asp:TextBox ID="txtCBIRCNo2" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtRC2Date">RC 2 Date</label>
                            <asp:TextBox ID="txtRC2Date" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtCVCOMNo">CVC OM Number</label>
                            <asp:TextBox ID="txtCVCOMNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtOMCVCDate">OM CVC Date</label>
                            <asp:TextBox ID="txtOMCVCDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtRCSource">RC Source</label>
                            <asp:TextBox ID="txtRCSource" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtInvestig">Investig</label>
                            <asp:TextBox ID="txtInvestig" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtAppEODate">Appointment EO Date</label>
                            <asp:TextBox ID="txtAppEODate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtEOName">EO Name</label>
                            <asp:TextBox ID="txtEOName" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtAppPODate">Appointment PO Date</label>
                            <asp:TextBox ID="txtAppPODate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtPOName">PO Name</label>
                            <asp:TextBox ID="txtPOName" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtCBIRecom">CBI Recommendation</label>
                            <asp:TextBox ID="txtCBIRecom" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtField1">Field 1</label>
                            <asp:TextBox ID="txtField1" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtPrevCasePunishment">Prev Case/Punishments</label>
                            <asp:TextBox ID="txtPrevCasePunishment" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtNatureofAccount">Nature of Account</label>
                            <asp:TextBox ID="txtNatureofAccount" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtSanctionOrderDate">Sanction Order Date</label>
                            <asp:TextBox ID="txtSanctionOrderDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtRecCVC2">Received CVC 2 Date</label>
                            <asp:TextBox ID="txtRecCVC2" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtProposedActiontoCVC">CVC Proposed Action</label>
                            <asp:TextBox ID="txtProposedActiontoCVC" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtCVC2Proposed">2nd Stage CVC</label>
                            <asp:TextBox ID="txtCVC2Proposed" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtCVC2Ref">CVC 2 Reference Date</label>
                            <asp:TextBox ID="txtCVC2Ref" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtReviewDate">Review Date</label>
                            <asp:TextBox ID="txtReviewDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtRegInvok">Reg Invok</label>
                            <asp:TextBox ID="txtRegInvok" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtNature">Nature Case</label>
                            <asp:TextBox ID="txtNature" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtReferToCVCDate">Refer To CVC Date</label>
                            <asp:TextBox ID="txtReferToCVCDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtRecommofCVC">Recommendation of CVC</label>
                            <asp:TextBox ID="txtRecommofCVC" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtCVCAdbiceII">CVC's Advice II</label>
                            <asp:TextBox ID="txtCVCAdbiceII" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtBasicPay">Basic Pay</label>
                            <asp:TextBox ID="txtBasicPay" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtLodiCase">Lodi Case</label>
                            <asp:TextBox ID="txtLodiCase" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtLodiNo">Lodi Number</label>
                            <asp:TextBox ID="txtLodiNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtClosureDate">Closure Date</label>
                            <asp:TextBox ID="txtClosureDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtLapseNature">Lapse Nature</label>
                            <asp:TextBox ID="txtLapseNature" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtA1CSCVC">A1C C/S CVC Date</label>
                            <asp:TextBox ID="txtA1CSCVC" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtA1EOPOCVC">A1E EO/PO CVC Date</label>
                            <asp:TextBox ID="txtA1EOPOCVC" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtA2FOCVC">A2 F/O CVC Date</label>
                            <asp:TextBox ID="txtA2FOCVC" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtCDIName">CDI Name</label>
                            <asp:TextBox ID="txtCDIName" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtAppCDIDate">Appointment CDI Date</label>
                            <asp:TextBox ID="txtAppCDIDate" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtPenaltyProceedings">Penalty Proceedings</label>
                            <asp:TextBox ID="txtPenaltyProceedings" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtLodiInclusionReason">Lodi Inclusion Reason</label>
                            <asp:TextBox ID="txtLodiInclusionReason" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtLodiDeletionReason">Lodi Deletion Reason</label>
                            <asp:TextBox ID="txtLodiDeletionReason" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-3">
                            <label for="txtLodiCode">Lodi Code</label>
                            <asp:TextBox ID="txtLodiCode" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtBankName">Bank Name</label>
                            <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label for="txtTMSACRefNo">TMSAC Ref Number</label>
                            <asp:TextBox ID="txtTMSACRefNo" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
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
                            <label for="txtPresentPosting">Present Posting</label>
                            <asp:TextBox ID="txtPresentPosting" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-6">
                            <label for="txtNewZone">New Zone</label>
                            <asp:TextBox ID="txtNewZone" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <label for="txtNewCircle">New Circle</label>
                            <asp:TextBox ID="txtNewCircle" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
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
                            <label for="txtDealingOfficerRemarks">Dealing Officer Remarks</label>
                            <asp:TextBox ID="txtDealingOfficerRemarks" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>

                    <%-- Checker Decision --%>
                    <asp:Panel ID="pnlDecision" runat="server">
                        <hr />
                        <asp:Panel ID="pnlVerifyNote" runat="server">
                            <div class="form-group row" style="padding-right: 5px;">
                                <div class="col-sm-12">
                                    <div class="alert alert-warning" style="margin-bottom: 10px;">
                                        Please verify the Vigilance record carefully before taking any action.
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
                    NavigateUrl="~/Mis/frmVigilanceChecker.aspx">&#8592; Back to Inbox</asp:HyperLink>
                <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
            </div>
            <div class="col-sm-6 text-right">
                <asp:Panel ID="pnlActions" runat="server" style="display: inline;">
                    <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-danger"
                        OnClick="btnReject_Click" OnClientClick="return validateCheckerAction('Reject this Vigilance record?');" />
                    <asp:Button ID="btnPushBack" runat="server" Text="Push Back" CssClass="btn btn-warning"
                        OnClick="btnPushBack_Click" OnClientClick="return validateCheckerAction('Push back this Vigilance record for correction?');" />
                    <asp:Button ID="btnAccept" runat="server" Text="Accept" CssClass="btn btn-success"
                        OnClick="btnAccept_Click" OnClientClick="return validateCheckerAction('Approve this Vigilance record?');" />
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
