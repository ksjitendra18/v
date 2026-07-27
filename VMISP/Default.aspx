<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="VMISP.Default" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <script src="/Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="/Js/JS_CommonValidation.js" type="text/javascript"></script>
    <script src="/Js/jquery-3.3.1.min.js" type="text/javascript"></script>
    <script src="/Js/jquery.min.js"></script>

    <script src="/Js/jquery-1.9.1.js"></script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <link href="/Js/jquery-ui.css" rel="stylesheet" />
    <link href="/css/bootstrap.css" rel="stylesheet" />
    <script src="/Js/bootstrap.min.js" type="text/javascript"></script>

    <!-- plugins:css -->
    <%-- <script src="/Scripts/highchart/jquery-1.11.1.min.js"></script>--%>
    <%-- <script src="/Scripts/highchart/jquery-ui.min.js"></script>--%>
    <%--<script src="/Scripts/highchart/jquery.min.js"></script>--%>
    <script src="/Scripts/highchart/highcharts.js"></script>
    <script src="/Scripts/highchart/data.js"></script>
    <script src="/Scripts/highchart/encoder.js"></script>
    <script src="/Scripts/highchart/exporting.js"></script>
    <div class="form-group row">
        <div class="panel panel-primary">
            <div class="panel-heading" style="font-size: medium; font-weight: bold;">Vigilance Matters Follow - up Dashboard</div>
            <div class="form-group">
                <div class="col-sm-12 alert alert-dark">
                    <div class="form-group row">
                        <div class="col-sm-2">
                            <div class="form-group row">
                                <div class="col-sm-12" style="align-items: center;">
                                    <asp:Button ID="btnOutstanding" runat="server" Text="OUTSTANDING" CssClass="btn btn-warning" Width="209px" Font-Bold="True" Font-Size="Medium" OnClick="btnOutstanding_Click" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12" style="align-items: center;">
                                    <asp:Button ID="btnComplaintOutstanding" runat="server" Text="COMPLAINT" CssClass="btn btn-danger" Width="209px" Font-Bold="True" Font-Size="Medium" OnClick="btnComplaintOutstanding_Click" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12" style="align-items: center;">
                                    <asp:Button ID="btnIACOutstanding" runat="server" Text="IAC" CssClass="btn btn-primary" Width="209px" Font-Bold="True" Font-Size="Medium" OnClick="btnIACOutstanding_Click" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12" style="align-items: center;">
                                    <asp:Button ID="btnVigilanceOutstanding" runat="server" Text="VIGILANCE" CssClass="btn btn-info" Width="209px" Font-Bold="True" Font-Size="Medium" OnClick="btnVigilanceOutstanding_Click" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12" style="align-items: center;">
                                    <asp:Button ID="btnNPAOutstanding" runat="server" Text="NPA" CssClass="btn btn-success" Width="209px" Font-Bold="True" Font-Size="Medium" OnClick="btnNPAOutstanding_Click" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12" style="align-items: center;">
                                    <asp:Button ID="btnABBFF" runat="server" Text="ABBFF" CssClass="btn btn-warning" Width="209px" Font-Bold="True" Font-Size="Medium" OnClick="btnABBFF_Click" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12" style="align-items: center;">
                                    <img src="images/VigilanceLogo.PNG" alt="Vigilance Logo" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-10">
                            <asp:Panel ID="pnlOutstanding" runat="server" Visible="false">
                                <div class="form-group">
                                    <div class="col-sm-12 panel-body bg-dark">
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblOutstandingComplaintsGraph" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblOutstandingIACReceivedGraph" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="col-sm-1" style="padding-top: 27px;margin-right: 160px">
                                            <asp:Button ID="btnPWOD" runat="server" CssClass="btn btn-sm btn-success" Text="Pending with other Department" OnClick="btnPWOD_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="padding-top: 27px;margin-right: 75px">
                                            <asp:Button ID="btnPAD" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at Desk" OnClick="btnPAD_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="padding-top: 27px;">
                                            <asp:Button ID="btnDWPR" runat="server" CssClass="btn btn-sm btn-success" Text="Dealt With-Pending for Other Reference" OnClick="btnDWPR_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="padding-top: 27px;">
                                        <asp:Label ID="lblCOD" runat="server" CssClass="label label-danger"></asp:Label>
                                    </div>
                                    </div>
                                        <div class="col-sm-6">
                                            <div class="col-sm-1" style="padding-top: 27px;margin-right: 78px">
                                            <asp:Button ID="btnPDC" runat="server" CssClass="btn btn-sm btn-success" Text="Pending DA Con." OnClick="btnPDC_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="padding-top: 27px;margin-right: 71px">
                                            <asp:Button ID="btnPDI" runat="server" CssClass="btn btn-sm btn-success" Text="Pending DA Inf." OnClick="btnPDI_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="padding-top: 27px;margin-right: 94px">
                                            <asp:Button ID="btnCS" runat="server" CssClass="btn btn-sm btn-success" Text="Clarification sought" OnClick="btnCS_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="padding-top: 27px;margin-right: 91px">
                                            <asp:Button ID="btnPD" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at Desk" OnClick="btnPD_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="padding-top: 27px;margin-right: 75px">
                                            <asp:Button ID="btnDWP" runat="server" CssClass="btn btn-sm btn-success" Text="Dealt With-Pending for Other Reference" OnClick="btnDWP_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="padding-top: 27px;">
                                        <asp:Label ID="Label1" runat="server" CssClass="label label-danger"></asp:Label>
                                    </div>
                                    </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="Label2" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-12 panel-body bg-dark">
                                        <div class="col-sm-12">
                                            <asp:Label ID="lblOutstandingVigilanceReceivedGraph" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                        <div >
                                            <div class="col-sm-1" style="margin-left: 128px;margin-right: 75px">
                                            <asp:Button ID="btnPDA" runat="server" CssClass="btn btn-sm btn-success" Text="Pending with DA" OnClick="btnPDA_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 115px">
                                            <asp:Button ID="btnPWIO" runat="server" CssClass="btn btn-sm btn-success" Text="Pending with IO" OnClick="btnPWIO_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 49px">
                                            <asp:Button ID="btnCourt" runat="server" CssClass="btn btn-sm btn-success" Text="Court" OnClick="btnCourt_Click"></asp:Button>
                                                </div>
                            <div class="col-sm-1" style="margin-right: 92px">
                                            <asp:Button ID="btnPDesk" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at Desk" OnClick="btnPDesk_Click"></asp:Button>
                                                </div>
                            <div class="col-sm-1" style="margin-right: 75px">
                                            <asp:Button ID="btnCVC" runat="server" CssClass="btn btn-sm btn-success" Text="Sent to CVC" OnClick="btnCVC_Click"></asp:Button>
                                                </div>
                            <div class="col-sm-1" style="margin-right: 75px">
                                            <asp:Button ID="btnFOI" runat="server" CssClass="btn btn-sm btn-success" Text="Final order issued" OnClick="btnFOI_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="padding-top: 27px;">
                                        <asp:Label ID="Label3" runat="server" CssClass="label label-danger"></asp:Label>
                                    </div>
                                    </div>
                        <br/>
                                <div class="form-group panel-body bg-dark">
                                    <div class="col-sm-3">
                                        <label for="txtfromDate" style="padding-top: 27px"><span style="color: #FF0000">*</span>From Date</label>
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <label for="txtToDate" style="padding-top: 27px"><span style="color: #FF0000">*</span>To Date</label>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1" style="padding-top: 51px;">
                                        <asp:Button ID="btnGetDetails" runat="server" CssClass="btn btn-sm btn-success" Text="Get Details" OnClick="btnGetDetails_Click"></asp:Button>
                                    </div>
                                    <div class="col-sm-5" style="padding-top: 27px;">
                                        <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-12 panel-body bg-dark">
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblOutstandingIACViewVigilancePie" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblOutstandingIACViewNonVigilancePie" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                        <div class="form-group" id="divPie" runat="server">
                                    <div class="col-sm-1"  style="margin-right: 497px;margin-left: 241px">
                                            <asp:Button ID="btnVigPie" runat="server" CssClass="btn btn-sm btn-success" Text="IAC Vigilance Cases" OnClick="btnVigPie_Click"></asp:Button>
                                                </div>
                            <div class="col-sm-1"  style="margin-right: 75px">
                                            <asp:Button ID="btnNonVigPie" runat="server" CssClass="btn btn-sm btn-success" Text="IAC Non Vigilance Cases" OnClick="btnNonVigPie_Click"></asp:Button>
                                                </div>
                                </div>

                            </asp:Panel>
                            <asp:Panel ID="pnlComplaintOutstanding" runat="server" Visible="false">
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <label for="ddlDealingCMComplaint"><span style="color: #FF0000">*</span>Complaint Dealing CM</label>
                                        <asp:DropDownList ID="ddlDealingCMComplaint" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlDealingCMComplaint_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlComplaintOutstandingDetails" runat="server" Visible="false">
                                    <div class="form-group">
                                        <div class="col-sm-12 panel-body bg-dark">
                                            <asp:Label ID="lblComplaintOutstandingPendingAtDeskDayWise" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="margin-bottom:60px;margin-top:-15px">
                                            <div class="col-sm-1" style="margin-left: 164px;margin-right: 316px">
                                            <asp:Button ID="btnCPWOD" runat="server" CssClass="btn btn-sm btn-success" Text="Pending with other Department" OnClick="btnCPWOD_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 213px">
                                            <asp:Button ID="btnCPAD" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at Desk" OnClick="btnCPAD_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 49px">
                                            <asp:Button ID="btnCDWPOR" runat="server" CssClass="btn btn-sm btn-success" Text="Dealt With-Pending for Other Reference" OnClick="btnCDWPOR_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="padding-top: 27px;">
                                        <asp:Label ID="Label4" runat="server" CssClass="label label-danger"></asp:Label>
                                    </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-12 panel-body bg-dark">
                                            <asp:Label ID="lblComplaintOutstandingPendingAtDesk" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="margin-bottom:60px;margin-top:-15px">
                                            <div class="col-sm-1" style="margin-left: 42px;margin-right: 42px">
                                            <asp:Button ID="btnCPHM" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at HO (Misc)" OnClick="btnCPHM_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 42px">
                                            <asp:Button ID="btnCPHF" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at HO FRMD" OnClick="btnCPHF_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 49px">
                                            <asp:Button ID="btnCHH" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at HO HRD" OnClick="btnCHH_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="margin-left: -7px;margin-right: 27px">
                                            <asp:Button ID="btnCHIAD" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at HO IAD" OnClick="btnCHIAD_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 56px">
                                            <asp:Button ID="btnCHS" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at HO SASTRA" OnClick="btnCHS_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 25px">
                                            <asp:Button ID="btnCHZ" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at HO ZO" OnClick="btnCHZ_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="margin-right: 23px">
                                            <asp:Button ID="btnCPO" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at Other" OnClick="btnCPO_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" >
                                            <asp:Button ID="btnCVO" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at VO" OnClick="btnCVO_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="padding-top: 27px;">
                                        <asp:Label ID="Label5" runat="server" CssClass="label label-danger"></asp:Label>
                                    </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-12 panel-body bg-dark">
                                            <asp:Label ID="lblComplaintOutstandingSourceRef" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                     <div style="margin-bottom:60px;margin-top:-15px">
                                          <%--<div class="col-sm-1" style="margin-left: 48px;margin-right: 28px">
                                            <asp:Button ID="btnCAnno" runat="server" CssClass="btn btn-sm btn-success" Text="ANNONYMOUS" OnClick="btnCAnno_Click"></asp:Button>
                                           </div>--%>
                                         <div class="col-sm-1" style="margin-left:120px;margin-right: 62px">
                                            <asp:Button ID="btnCCBI" runat="server" CssClass="btn btn-sm btn-success" Text="CBI" OnClick="btnCCBI_Click"></asp:Button>
                                                </div>
                                         <div class="col-sm-1" style="margin-right: 56px">
                                            <asp:Button ID="btnCCVC" runat="server" CssClass="btn btn-sm btn-success" Text="CVC" OnClick="btnCCVC_Click"></asp:Button>
                                                </div>
                                        <%-- <div class="col-sm-1" style="margin-right: 20px">
                                            <asp:Button ID="btnCCVCP" runat="server" CssClass="btn btn-sm btn-success" Text="CVC PORTAL" OnClick="btnCCVCP_Click"></asp:Button>
                                                </div>--%>
                                          <%--<div class="col-sm-1" style="margin-right: -15px">
                                            <asp:Button ID="btnCFRMD" runat="server" CssClass="btn btn-sm btn-success" Text="FRMD" OnClick="btnCFRMD_Click"></asp:Button>
                                                </div>--%>
                                          <%--<div class="col-sm-1"  style="margin-right: -5px" >
                                            <asp:Button ID="btnCIADHO" runat="server" CssClass="btn btn-sm btn-success" Text="IAD HO" OnClick="btnCIADHO_Click"></asp:Button>
                                                </div>--%>
                                       <%--  <div class="col-sm-1" style="margin-right: -2px">
                                            <asp:Button ID="btnCMARD" runat="server" CssClass="btn btn-sm btn-success" Text="MARD" OnClick="btnCMARD_Click"></asp:Button>
                                                </div>--%>
                                         <div class="col-sm-1" style="margin-right: 63px">
                                            <asp:Button ID="btnCEOW" runat="server" CssClass="btn btn-sm btn-success" Text="EOW" OnClick="btnCEOW_Click"></asp:Button>
                                                </div>
                                         <div class="col-sm-1" style="margin-right: 53px">
                                            <asp:Button ID="btnCMOF" runat="server" CssClass="btn btn-sm btn-success" Text="MOF" OnClick="btnCMOF_Click"></asp:Button>
                                                </div>
                                          <%--<div class="col-sm-1" style="margin-right: -5px">
                                            <asp:Button ID="btnCMATR" runat="server" CssClass="btn btn-sm btn-success" Text="Mof-ATR" OnClick="btnCMATR_Click"></asp:Button>
                                                </div>--%>
                                         <div class="col-sm-1" style="margin-right: 58px">
                                            <asp:Button ID="btnCOther" runat="server" CssClass="btn btn-sm btn-success" Text="Others" OnClick="btnCOther_Click"></asp:Button>
                                                </div>
                                            <%--<div class="col-sm-1" style="margin-right: -9px">
                                            <asp:Button ID="btnCPMRef" runat="server" CssClass="btn btn-sm btn-success" Text="PMO REF" OnClick="btnCPMRef_Click"></asp:Button>
                                                </div>--%>
                                            <div class="col-sm-1"  style="margin-right: 75px;">
                                            <asp:Button ID="btnCPolice" runat="server" CssClass="btn btn-sm btn-success" Text="POLICE" OnClick="btnCPolice_Click"></asp:Button>
                                                </div>
                                          <div class="col-sm-1" >
                                            <asp:Button ID="btnCRBI" runat="server" CssClass="btn btn-sm btn-success" Text="RBI" OnClick="btnCRBI_Click"></asp:Button>
                                                </div>
                                         <div class="col-sm-1" style="padding-top: 27px;">
                                        <asp:Label ID="Label11" runat="server" CssClass="label label-danger"></asp:Label>
                                    </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-12 panel-body bg-dark">
                                            <asp:Label ID="lblComplaintOutstandingPieChart" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="pnlIACOutstanding" runat="server" Visible="false">
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <label for="ddlDealingCMIAC"><span style="color: #FF0000">*</span>IAC Dealing CM</label>
                                        <asp:DropDownList ID="ddlDealingCMIAC" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlDealingCMIAC_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlIACOutstandingDetails" runat="server" Visible="false">
                                    <div class="form-group">
                                        <div class="col-sm-12 panel-body bg-dark">
                                            <div class="col-sm-12">
                                                <asp:Label ID="lblIACOutstandingPieChart" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="margin-bottom:60px;margin-top:-15px">
                                            <div class="col-sm-1" style="margin-left: 506px;">
                                            <asp:Button ID="btnICDW" runat="server" CssClass="btn btn-sm btn-success" Text="Concluded/Dealt with" OnClick="btnICDW_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="padding-top: 27px;">
                                        <asp:Label ID="Label6" runat="server" CssClass="label label-danger"></asp:Label>
                                    </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-6 panel-body bg-dark">
                                            <asp:Label ID="lblIACOutstandingPendingAtDesk" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-6 panel-body bg-dark">
                                            <asp:Label ID="lblIACOutstandingPendingAtDeskDayWise" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    </div>
                                    <div style="margin-bottom:60px;margin-top:-15px">
                                            <div class="col-sm-1" style="margin-left: 77px;margin-right: 86px">
                                            <asp:Button ID="btnICS" runat="server" CssClass="btn btn-sm btn-success" Text="Clarafication sought" OnClick="btnICS_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 19px">
                                            <asp:Button ID="btnIPAD" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at Desk" OnClick="btnIPAD_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 100px">
                                            <asp:Button ID="btnIPDI" runat="server" CssClass="btn btn-sm btn-success" Text="Pending with DA for information" OnClick="btnIPDI_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="margin-left: 164px;margin-right: 153px">
                                            <asp:Button ID="btnIL15" runat="server" CssClass="btn btn-sm btn-success" Text="Less than 15 Days" OnClick="btnIL15_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" >
                                            <asp:Button ID="btnIG15" runat="server" CssClass="btn btn-sm btn-success" Text="Greater than 15 Days" OnClick="btnIG15_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="padding-top: 27px;">
                                        <asp:Label ID="Label7" runat="server" CssClass="label label-danger"></asp:Label>
                                    </div>
                                    </div>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="pnlVigilanceOutstanding" runat="server" Visible="false">
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <label for="ddlDealingCMVigilance"><span style="color: #FF0000">*</span>Vigilance Dealing CM</label>
                                        <asp:DropDownList ID="ddlDealingCMVigilance" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlDealingCMVigilance_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlVigilanceOutstandingDetails" runat="server" Visible="false">
                                    <div class="form-group">
                                        <div class="col-sm-12 panel-body bg-dark">
                                            <div class="col-sm-12">
                                                <asp:Label ID="lblVigilanceOutstandingPendingAtDsk" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="margin-bottom:60px;margin-top:-15px">
                                            <div class="col-sm-1" style="margin-left: 113px;margin-right: 56px">
                                            <asp:Button ID="btnVPWD" runat="server" CssClass="btn btn-sm btn-success" Text="Pending with DA" OnClick="btnVPWD_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 78px">
                                            <asp:Button ID="btnVPWI" runat="server" CssClass="btn btn-sm btn-success" Text="Pending with IO" OnClick="btnVPWI_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 29px">
                                            <asp:Button ID="btnVC" runat="server" CssClass="btn btn-sm btn-success" Text="Court" OnClick="btnVC_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="margin-right: 66px">
                                            <asp:Button ID="btnVPAD" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at Desk" OnClick="btnVPAD_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" >
                                            <asp:Button ID="btnVCVC" runat="server" CssClass="btn btn-sm btn-success" Text="Sent to CVC" OnClick="btnVCVC_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="margin-left: 36px;margin-right: 77px">
                                            <asp:Button ID="btnVFOI" runat="server" CssClass="btn btn-sm btn-success" Text="Final Order Issued" OnClick="btnVFOI_Click"></asp:Button>
                                                </div>
                                            <%--<div class="col-sm-1" >
                                            <asp:Button ID="btnV2S" runat="server" CssClass="btn btn-sm btn-success" Text="2nd Stage" OnClick="btnV2S_Click"></asp:Button>
                                                </div>--%>
                                        <div class="col-sm-1" style="padding-top: 27px;">
                                        <asp:Label ID="Label8" runat="server" CssClass="label label-danger"></asp:Label>
                                    </div>
                                    </div>

                                    <div class="form-group" style="display: block;">
                                        <div class="col-sm-12 panel-body bg-dark">
                                            <div class="col-sm-12">
                                                <asp:Label ID="lblVigilanceOutstandingChargeSheetGraph" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                     <div style="margin-bottom:60px;margin-top:-15px">
                                            <div class="col-sm-1" style="margin-left: 113px;margin-right: 56px">
                                            <asp:Button ID="btnVDEP" runat="server" CssClass="btn btn-sm btn-success" Text="Dept. Enq. in Prog." OnClick="btnVDEP_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 53px">
                                            <asp:Button ID="btnVDEC" runat="server" CssClass="btn btn-sm btn-success" Text="Dept. Enq. Conc." OnClick="btnVDEC_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 82px">
                                            <asp:Button ID="btnVSBC" runat="server" CssClass="btn btn-sm btn-success" Text="Stayed by Court" OnClick="btnVSBC_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="margin-right: 8px">
                                            <asp:Button ID="btnVM" runat="server" CssClass="btn btn-sm btn-success" Text="Minor" OnClick="btnVM_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 24px" >
                                            <asp:Button ID="btnVCPFO" runat="server" CssClass="btn btn-sm btn-success" Text="Closed, Pen. for other" OnClick="btnVCPFO_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="margin-left: 36px;margin-right: 77px">
                                            <asp:Button ID="btnVFOA" runat="server" CssClass="btn btn-sm btn-success" Text="Final Order Awaited" OnClick="btnVFOA_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" >
                                            <asp:Button ID="btnVSSA" runat="server" CssClass="btn btn-sm btn-success" Text="SSA Awaited" OnClick="btnVSSA_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="padding-top: 27px;">
                                        <asp:Label ID="Label9" runat="server" CssClass="label label-danger"></asp:Label>
                                    </div>
                                    </div>
                                    <div class="form-group" style="display: block;">
                                        <div class="col-sm-12 panel-body bg-dark">
                                            <div class="col-sm-6">
                                                <asp:Label ID="lblVigilanceOutstandingNatureChargeSheetPieChart" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-sm-6">
                                                <asp:Label ID="lblVigilanceOutstandingChargeSheetPieChart" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                     <div class="col-sm-1" style="margin-left: 233px;margin-right: 478px">
                                            <asp:Button ID="btnVNCS" runat="server" CssClass="btn btn-sm btn-success" Text="Nature of Charge Sheet" OnClick="btnVNCS_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" >
                                            <asp:Button ID="btnVCSS" runat="server" CssClass="btn btn-sm btn-success" Text="Charge Sheet yet to be Served" OnClick="btnVCSS_Click"></asp:Button>
                                                </div>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="pnlNPAOutstanding" runat="server" Visible="false">
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <label for="ddlDealingCMNPA"><span style="color: #FF0000">*</span>NPA Dealing CM</label>
                                        <asp:DropDownList ID="ddlDealingCMNPA" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlDealingCMNPA_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlNPAOutstandingDetails" runat="server" Visible="false">
                                    <div class="form-group">
                                        <div class="col-sm-12 panel-body bg-dark">
                                            <div class="col-sm-12">
                                                <asp:Label ID="lblNPAOutstandingPieChart" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-1" style="margin-left: 550px;margin-bottom:26px">
                                          <asp:Button ID="btnNNO" runat="server" CssClass="btn btn-sm btn-success" Text="NPA Outstanding" OnClick="btnNNO_Click"></asp:Button>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-12 panel-body bg-dark">
                                            <asp:Label ID="lblNPAOutstandingPendingAtDesk" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="margin-bottom:60px;margin-top:-15px">
                                            <div class="col-sm-1" style="margin-left: 71px;margin-right: 29px">
                                            <asp:Button ID="btnNPAH" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at HO" OnClick="btnNPAH_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 53px">
                                            <asp:Button ID="btnNPHF" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at HO FRMD" OnClick="btnNPHF_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 36px">
                                            <asp:Button ID="btnNPHH" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at HO HRD" OnClick="btnNPHH_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="margin-right: 29px">
                                            <asp:Button ID="btnNPHI" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at HO IAD" OnClick="btnNPHI_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 24px" >
                                            <asp:Button ID="btnNHS" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at HO SASTRA" OnClick="btnNHS_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="margin-left: 36px;margin-right: 39px">
                                            <asp:Button ID="btnNPHZ" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at HO ZO" OnClick="btnNPHZ_Click"></asp:Button>
                                                </div>
                                            <div class="col-sm-1" style="margin-right: 40px">
                                            <asp:Button ID="btnNPO" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at Other" OnClick="btnNPO_Click"></asp:Button>
                                                </div>
                                         <div class="col-sm-1" >
                                            <asp:Button ID="btnNPZAO" runat="server" CssClass="btn btn-sm btn-success" Text="Pending at ZAO" OnClick="btnNPZAO_Click"></asp:Button>
                                                </div>
                                        <div class="col-sm-1" style="padding-top: 27px;">
                                        <asp:Label ID="Label10" runat="server" CssClass="label label-danger"></asp:Label>
                                    </div>
                                    </div>
                                </asp:Panel>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
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



    <div id="pendingApprovalsModal" class="modal fade" tabindex="-1" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content">

            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Pending Approvals</h4>
            </div>

            <div class="modal-body">
                <table class="table table-bordered table-condensed">
                    <thead>
                        <tr>
                            <th>Activity</th>
                            <th class="text-center">Pending</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="phPendingApprovals" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>

            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">
                    Later
                </button>
            </div>

        </div>
    </div>
</div>


</asp:Content>
