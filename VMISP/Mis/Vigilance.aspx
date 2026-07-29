<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="Vigilance.aspx.cs" Inherits="VMISP.Mis.Vigilance" %>

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
                            Vigilance Entry 
                        </div>
                        <br />
                        <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="100%" OnActiveTabChanged="tabMain_ActiveTabChanged"
                            AutoPostBack="true">
                            <act:TabPanel ID="tabEntry" runat="server">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEntryHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="Entry" ToolTip="Vigilance Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-2">
                                            <label for="txtRNo"><span style="color: #FF0000">*</span>R Number</label>
                                            <asp:TextBox ID="txtRNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-1" style="padding-top: 23px;">
                                            <asp:Button ID="btnGet" runat="server" OnClick="btnGet_Click" ToolTip="Vigilance Search" CssClass="btn btn-sm btn-info" Text="Search"></asp:Button>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtRNo1"><span style="color: #FF0000">*</span>R Number 1</label>
                                            <asp:TextBox ID="txtRNo1" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtNameOfParticulars"><span style="color: #FF0000">*</span>Name & Particulars</label>
                                            <asp:TextBox ID="txtNameOfParticulars" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtName">Name</label>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtChargeDate"><span style="color: #FF0000">*</span>Charge Date</label>
                                            <asp:TextBox ID="txtChargeDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlNatCHSheet">Nature of Charge Sheet</label>
                                            <asp:DropDownList ID="ddlNatCHSheet" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Gross" Value="GROSS"></asp:ListItem>
                                                <asp:ListItem Text="Major" Value="MAJOR"></asp:ListItem>
                                                <asp:ListItem Text="Minor" Value="MINOR"></asp:ListItem>
                                                <asp:ListItem Text="Null" Value="NULL"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlZone">Zone</label>
                                            <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control input-sm">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlStatusCode">Status Code</label>
                                            <asp:DropDownList ID="ddlStatusCode" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            <asp:Label ID="lblStatusCodeMIS" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="ddlRegister"><span style="color: #FF0000">*</span>Register</label>
                                            <asp:DropDownList ID="ddlRegister" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            <asp:Label ID="lblRegister" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlCircleOffice">Circle</label>
                                            <asp:DropDownList ID="ddlCircleOffice" runat="server" CssClass="form-control input-sm">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlFinal">Final</label>
                                            <asp:DropDownList ID="ddlFinal" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlScale">Scale</label>
                                            <asp:DropDownList ID="ddlScale" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="chkRNoDate">R No Date</label>
                                            <asp:CheckBox ID="chkRNoDate" runat="server" Checked="false" />
                                            <asp:Label ID="lblRNoDate" runat="server" CssClass="lblCaption"></asp:Label>
                                            <asp:Panel ID="pnlRNoDate" runat="server" Visible="false">
                                                <asp:TextBox ID="txtRNoDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtPFNo">PF Number</label>
                                            <asp:TextBox ID="txtPFNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtRetirementDate">Retirement Date</label>
                                            <asp:TextBox ID="txtRetirementDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtDAOrdDate">DA Order Date</label>
                                            <asp:TextBox ID="txtDAOrdDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtNAPUNDA">Nature of Punishment of DA</label>
                                            <asp:TextBox ID="txtNAPUNDA" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlPenaltyType">Penalty Type</label>
                                            <asp:DropDownList ID="ddlPenaltyType" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlDisAuthoritysCircle">DA_CO/ZO/HO</label>
                                            <asp:DropDownList ID="ddlDisAuthoritysCircle" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtDispAuthority">Disp Authority</label>
                                            <asp:TextBox ID="txtDispAuthority" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtIstDaDate">Ist DA Date</label>
                                            <asp:TextBox ID="txtIstDaDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtDAProposal">DA Proposal</label>
                                            <asp:TextBox ID="txtDAProposal" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtFinalDate">Final Date</label>
                                            <asp:TextBox ID="txtFinalDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtCVOAdvice">CVO Advice</label>
                                            <asp:TextBox ID="txtCVOAdvice" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtCVOAdviceDate">CVO Advice Date</label>
                                            <asp:TextBox ID="txtCVOAdviceDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txt2ndDADate">2nd DA Date</label>
                                            <asp:TextBox ID="txt2ndDADate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txt2DAProposal">2DA Proposal</label>
                                            <asp:TextBox ID="txt2DAProposal" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtCVO2Advice">CVO 2 Advice</label>
                                            <asp:TextBox ID="txtCVO2Advice" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtCVO2AdviceDate">CVO 2 Advice Date</label>
                                            <asp:TextBox ID="txtCVO2AdviceDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtAccountName">Account Name</label>
                                            <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtSource">Source</label>
                                            <asp:TextBox ID="txtSource" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlState">State</label>
                                            <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtPlaceinPresentScaleDate">Place in Present Scale From Date</label>
                                            <asp:TextBox ID="txtPlaceinPresentScaleDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtSanctionRefusedDate">Refused Date</label>
                                            <asp:TextBox ID="txtSanctionRefusedDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtDesignation">Designation</label>
                                            <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtPunishmentProposedbyDA">DA Proposed Punishment</label>
                                            <asp:TextBox ID="txtPunishmentProposedbyDA" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtCompRecDate">Supplementary C/S Date</label>
                                            <asp:TextBox ID="txtCompRecDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtStatusinBrief">Supplementary C/S Status</label>
                                            <asp:TextBox ID="txtStatusinBrief" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlFinal">Branch</label>
                                            <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtPenalty">Penalty</label>
                                            <asp:TextBox ID="txtPenalty" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtAmount">Amount</label>
                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control input-sm" Style="text-align: right"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtCSOREPDate">Date of CSO REP.</label>
                                            <asp:TextBox ID="txtCSOREPDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtConEnqDate">Conduct Enquiry Date</label>
                                            <asp:TextBox ID="txtConEnqDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtSuspensionDate">Suspension Date</label>
                                            <asp:TextBox ID="txtSuspensionDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtCbiRcNo1">CBI RC NO1</label>
                                            <asp:TextBox ID="txtCbiRcNo1" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtRC1Date">RC1 Date</label>
                                            <asp:TextBox ID="txtRC1Date" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtCBIRCNo2">CBI RC No2</label>
                                            <asp:TextBox ID="txtCBIRCNo2" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtRC2Date">RC 2 Date</label>
                                            <asp:TextBox ID="txtRC2Date" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtCVCOMNo">CVC OM Number</label>
                                            <asp:TextBox ID="txtCVCOMNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtOMCVCDate">OM CVC Date</label>
                                            <asp:TextBox ID="txtOMCVCDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtRCSource">RC Source</label>
                                            <asp:TextBox ID="txtRCSource" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtInvestig">Investig</label>
                                            <asp:TextBox ID="txtInvestig" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtAppEODate">Appointment EO Date</label>
                                            <asp:TextBox ID="txtAppEODate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtEOName">EO Name</label>
                                            <asp:TextBox ID="txtEOName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtAppPODate">Appointment PO Date</label>
                                            <asp:TextBox ID="txtAppPODate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtPOName">PO Name</label>
                                            <asp:TextBox ID="txtPOName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtCBIRecom">CBI Recommendation</label>
                                            <asp:TextBox ID="txtCBIRecom" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtField1">Field 1</label>
                                            <asp:TextBox ID="txtField1" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtPrevCasePunishment">Prev Case/Punishments</label>
                                            <asp:TextBox ID="txtPrevCasePunishment" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtNatureofAccount">Nature of Account</label>
                                            <asp:TextBox ID="txtNatureofAccount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtSanctionOrderDate">Sanction Order Date</label>
                                            <asp:TextBox ID="txtSanctionOrderDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtRecCVC2">Received CVC 2 Date</label>
                                            <asp:TextBox ID="txtRecCVC2" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtProposedActiontoCVC">CVC Proposed Action</label>
                                            <asp:TextBox ID="txtProposedActiontoCVC" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtCVC2Proposed">2nd Stage CVC</label>
                                            <asp:TextBox ID="txtCVC2Proposed" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtCVC2Ref">CVC 2 Reference Date</label>
                                            <asp:TextBox ID="txtCVC2Ref" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtReviewDate">Review Date</label>
                                            <asp:TextBox ID="txtReviewDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtRegInvok">Reg Invok</label>
                                            <asp:TextBox ID="txtRegInvok" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlNature">Nature Case</label>
                                            <asp:DropDownList ID="ddlNature" runat="server" CssClass="form-control input-sm">
                                            </asp:DropDownList>
                                            <asp:Panel ID="pnlNatureMIS" runat="server" Visible="False">
                                                <asp:Label ID="lblNatureMIS" runat="server"></asp:Label>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtReferToCVCDate">Refer To CVC Date</label>
                                            <asp:TextBox ID="txtReferToCVCDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtRecommofCVC">Recommendation of CVC</label>
                                            <asp:TextBox ID="txtRecommofCVC" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtCVCAdbiceII">CVC's Advice II</label>
                                            <asp:TextBox ID="txtCVCAdbiceII" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtBasicPay">Basic Pay</label>
                                            <asp:TextBox ID="txtBasicPay" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="ddlLodiCase">Lodi Case</label>
                                            <asp:DropDownList ID="ddlLodiCase" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtLodiNo">Lodi Number</label>
                                            <asp:TextBox ID="txtLodiNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="chkClosureDate">Closure Date</label>
                                            <asp:CheckBox ID="chkClosureDate" runat="server" Checked="false" />
                                            <asp:Label ID="lblClosureDate" runat="server" CssClass="lblCaption"></asp:Label>
                                            <asp:Panel ID="pnlClosureDate" runat="server" Visible="false">
                                                <asp:TextBox ID="txtClosureDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtLapseNature">Lapse Nature</label>
                                            <asp:TextBox ID="txtLapseNature" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtA1CSCVC">A1C C/S CVC Date</label>
                                            <asp:TextBox ID="txtA1CSCVC" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtA1EOPOCVC">A1E EO/PO CVC Date</label>
                                            <asp:TextBox ID="txtA1EOPOCVC" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtA2FOCVC">A2 F/O CVC Date</label>
                                            <asp:TextBox ID="txtA2FOCVC" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtCDIName">CDI Name</label>
                                            <asp:TextBox ID="txtCDIName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtAppCDIDate">Appointment CDI Date</label>
                                            <asp:TextBox ID="txtAppCDIDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlFinal">Penalty Proceedings</label>
                                            <asp:DropDownList ID="ddlPenaltyProceedings" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtLodiInclusionReason">Lodi Inclusion Reason</label>
                                            <asp:TextBox ID="txtLodiInclusionReason" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtLodiDeletionReason">Lodi Deletion Reason</label>
                                            <asp:TextBox ID="txtLodiDeletionReason" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="txtLodiCode">Lodi Code</label>
                                            <asp:TextBox ID="txtLodiCode" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlBankName">Bank Name</label>
                                            <asp:DropDownList ID="ddlBankName" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                <asp:ListItem Value="PNB" Text="PNB"></asp:ListItem>
                                                <asp:ListItem Value="OBC" Text="OBC"></asp:ListItem>
                                                <asp:ListItem Value="UBI" Text="UBI"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlFinal">TMSAC Ref Number</label>
                                            <asp:TextBox ID="txtTMSACRefNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlFinal">Letter Sent Date</label>
                                            <asp:TextBox ID="txtLetterSentDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-3">
                                            <label for="ddlLetterSentTo">Letter Sent To</label>
                                            <asp:DropDownList ID="ddlLetterSentTo" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtReminderDate">Reminder Date</label>
                                            <asp:TextBox ID="txtReminderDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="txtReplyReceivedDate">Reply Received Date</label>
                                            <asp:TextBox ID="txtReplyReceivedDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="ddlFinal"></label>
                                        </div>
                                    </div>

                                    <div class="form-group row" style="padding-right: 5px;">
                                        <div class="col-sm-6">
                                            <label for="ddlZoneNew">New Zone</label>
                                             <asp:DropDownList ID="ddlZoneNew" runat="server" CssClass="form-control  input-sm" OnSelectedIndexChanged="ddlZoneNew_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-6">
                                            <label for="ddlCircleNew">New Circle</label>
                                            <asp:DropDownList ID="ddlCircleNew" runat="server" CssClass="form-control  input-sm"></asp:DropDownList></td>
                                        </div>
                                        
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-12">
                                            <label for="txtStatus"><span style="color: #FF0000">*</span>Status</label>
                                            <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-12">
                                            <asp:Panel ID="pnlHOStatus" runat="server" Visible="False">
                                                <span class="lblCaption">HO Status :</span>
                                                <asp:TextBox ID="txtHOStatus" runat="server" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <div class="form-group row">
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
                            <act:TabPanel ID="tabList" runat="server" HeaderText="Vigilance Entry Details">
                                <HeaderTemplate>
                                    <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="Vigilance Entry Details" ToolTip="List of Vigilance Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row">
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtRNo_LIST" runat="server" CssClass="form-control input-sm" placeholder="R Number"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtAccountName_LIST" runat="server" CssClass="form-control input-sm" placeholder="Account Name"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtName_LIST" runat="server" CssClass="form-control input-sm" placeholder="Name"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtPFNumber_LIST" runat="server" CssClass="form-control input-sm" placeholder="PF Number"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtBranch_LIST" runat="server" CssClass="form-control input-sm" placeholder="Branch"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtCircle_LIST" runat="server" CssClass="form-control input-sm" placeholder="Circle"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtCBIRCNO_LIST" runat="server" CssClass="form-control input-sm" placeholder="CBI RC Number 1"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtCVCOMNO_LIST" runat="server" CssClass="form-control input-sm" placeholder="CVC OM Number"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-6">
                                                <asp:TextBox ID="txtStatus_LIST" runat="server" CssClass="form-control input-sm" placeholder="Status"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1" style="padding-top: 0px;">
                                                <asp:Button ID="btnSearch_List" runat="server" OnClick="btnSearch_List_Click" ToolTip="Complaint Search" Text="Search" CssClass="btn btn-sm btn-info" />
                                            </div>
                                            <div class="col-sm-5">
                                                <asp:Label ID="lblList" runat="server" CssClass="label label-danger"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-12">
                                            <asp:GridView ID="gvMain" runat="server" OnRowCommand="gvMain_RowCommand" OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="false" CssClass="table input-sm table-bordered table-condensed">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Select
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnView" runat="server" CausesValidation="false" CommandName="View" ToolTip='<%# Eval("CODE") %>' CommandArgument='<%# Eval("CODE")%>' CssClass="btn btn-sm btn-danger" Text="Edit" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="RNO" HeaderText="R No." SortExpression="RNO" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RNO1" HeaderText="R NO 1" SortExpression="RNO1" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NAMEOFPARTICULARS" HeaderText="Name & Particulars" />
                                                    <asp:BoundField DataField="NAME" HeaderText="Name" />
                                                    <asp:BoundField DataField="CHARGEDATE" HeaderText="Charge Date" />
                                                    <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" />
                                                    <asp:BoundField DataField="REGISTER" HeaderText="Register" />
                                                    <asp:BoundField DataField="SCALE" HeaderText="Scale" />
                                                    <asp:BoundField DataField="PFNO" HeaderText="PF Number" />
                                                    <asp:BoundField DataField="ISTDADATE" HeaderText="Ist DA Date" />
                                                    <asp:BoundField DataField="AMOUNT" HeaderText="Amount" />
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
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </act:TabPanel>
                        </act:TabContainer>
                        <table style="background-color: #FFE4E1">
                            <tr style="display: none">
                                <td class="tdTextReight">
                                    <span class="lblCaption">Present Posting :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPresentPosting" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">EO CDI :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEoCdi" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">D Ref No :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDRefNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Reasons for Inclusion :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReasonsforInclusion" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdTextReight">
                                    <span class="lblCaption">DA Ref No :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDARefNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Rec Report :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRecReportDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeRecReportDate" runat="server" TargetControlID="txtRecReportDate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                        CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceRecReportDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtRecReportDate" PopupButtonID="imgRecReportDate" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgRecReportDate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Deletion Reasons :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDeletionReasons" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">U/S :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUS" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdTextReight">
                                    <span class="lblCaption">Occur Date :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOccurDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeOccurDate" runat="server" TargetControlID="txtOccurDate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                        CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceOccurDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtOccurDate" PopupButtonID="imgOccurDate" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgOccurDate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Inv Officer Name :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInvOfficerName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">IR-CBI Pending :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIRCBIPending" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Pol Fir No :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPolFirNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdTextReight">
                                    <span class="lblCaption">FIR Date :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFIRDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeFIRDate" runat="server" TargetControlID="txtFIRDate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                        CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceFIRDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtFIRDate" PopupButtonID="imgFIRDate" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgFIRDate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">ADV 1 Awaited :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtADV1Awaited" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">PO CBI :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPOCBI" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Last RH Date :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLastRHDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeLastRHDate" runat="server" TargetControlID="txtLastRHDate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                        CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceLastRHDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtLastRHDate" PopupButtonID="imgLastRHDate" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgLastRHDate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdTextReight">
                                    <span class="lblCaption">DTS Hear :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDTSHear" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">NO Award S :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoAwardS" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Written Brief PO :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtWrittenBriefPO" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeWrittenBriefPO" runat="server" TargetControlID="txtWrittenBriefPO"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                        CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceWrittenBriefPO" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtWrittenBriefPO" PopupButtonID="imgWrittenBriefPO" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgWrittenBriefPO" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdTextReight">
                                    <span class="lblCaption">Agency Inv Date :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBasicPayDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeBasicPayDate" runat="server" TargetControlID="txtBasicPayDate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                        CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceBasicPayDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtBasicPayDate" PopupButtonID="imgBasicPayDate" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgBasicPayDate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>

                            </tr>
                            <tr style="display: none">
                                <td class="tdTextReight">
                                    <span class="lblCaption">Connected/Vig Case :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConnectedVigCase" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">CH-Sheet Filed :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCHSheetFiledDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeCHSheetFiledDate" runat="server" TargetControlID="txtCHSheetFiledDate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceCHSheetFiledDate" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtCHSheetFiledDate" PopupButtonID="imgCHSheetFiledDate"
                                        CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgCHSheetFiledDate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">2nd Pending :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt2ndPending" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">DA Sent Advice :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdviceSentToDADate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeAdviceSentToDADate" runat="server" TargetControlID="txtAdviceSentToDADate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                        CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceAdviceSentToDADate" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtAdviceSentToDADate" PopupButtonID="imgAdviceSentToDADate"
                                        CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgAdviceSentToDADate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdTextReight">
                                    <span class="lblCaption">Re Comp :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReComp" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Appeal :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppeal" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeAppeal" runat="server" TargetControlID="txtAppeal"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                        CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceAppeal" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtAppeal" PopupButtonID="imgAppeal" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgAppeal" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                                <td class="tdTextReight" style="width: 105px">
                                    <span class="lblCaption">Written Brief CO :</span>
                                </td>
                                <td style="width: 105px">
                                    <asp:TextBox ID="txtWrittenBriefCODate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeWrittenBriefCODate" runat="server" TargetControlID="txtWrittenBriefCODate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                        CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceWrittenBriefCODate" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtWrittenBriefCODate" PopupButtonID="imgWrittenBriefCODate"
                                        CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgWrittenBriefCODate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" Style="margin-left: -2px" />
                                </td>

                            </tr>
                            <tr style="display: none">
                                <td class="tdTextReight">
                                    <span class="lblCaption">Prelim Enq :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPrelimEnq" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meePrelimEnq" runat="server" TargetControlID="txtPrelimEnq"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                        CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="cePrelimEnq" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtPrelimEnq" PopupButtonID="imgPrelimEnq" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgPrelimEnq" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Adv 2 Awt :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdv2Awt" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Regu Enq :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReguEnq" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeReguEnq" runat="server" TargetControlID="txtReguEnq"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                        CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceReguEnq" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtReguEnq" PopupButtonID="imgReguEnq" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgReguEnq" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Regulat Date :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRegulatDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeRegulatDate" runat="server" TargetControlID="txtRegulatDate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                        CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceRegulatDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtRegulatDate" PopupButtonID="imgRegulatDate" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgRegulatDate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdTextReight">
                                    <span class="lblCaption">Lodi New :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLodiNew" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Ist Pending :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIstPending" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Revocation :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRevocationDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeRevocationDate" runat="server" TargetControlID="txtRevocationDate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceRevocationDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtRevocationDate" PopupButtonID="imgRevocationDate" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgRevocationDate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Commitment Date :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCommitmentDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeCommitmentDate" runat="server" TargetControlID="txtCommitmentDate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceCommitmentDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtCommitmentDate" PopupButtonID="imgCommitmentDate" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgCommitmentDate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdTextReight">
                                    <span class="lblCaption">CO Reply Date :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCOReplyDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeCOReplyDate" runat="server" TargetControlID="txtCOReplyDate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceCOReplyDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtCOReplyDate" PopupButtonID="imgCOReplyDate" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgCOReplyDate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Target Date :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTargetDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeTargetDate" runat="server" TargetControlID="txtTargetDate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceTargetDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtTargetDate" PopupButtonID="imgTargetDate" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgTargetDate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">ER CO Date :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtERCODate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeERCODate" runat="server" TargetControlID="txtERCODate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceERCODate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                        TargetControlID="txtERCODate" PopupButtonID="imgERCODate" CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgERCODate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Sanction Recv Date :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSanctionRecvDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    <act:MaskedEditExtender ID="meeSanctionRecvDate" runat="server" TargetControlID="txtSanctionRecvDate"
                                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                        CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                    </act:MaskedEditExtender>
                                    <act:CalendarExtender ID="ceSanctionRecvDate" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtSanctionRecvDate" PopupButtonID="imgSanctionRecvDate"
                                        CssClass="cal_Theme1">
                                    </act:CalendarExtender>
                                    <asp:ImageButton ID="imgSanctionRecvDate" runat="server" AlternateText="Please Select date!!"
                                        ImageUrl="~/images/calendar.png" />
                                </td>
                            </tr>
                        </table>
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
