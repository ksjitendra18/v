<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="RRB.aspx.cs" Inherits="VMISP.Mis.RRB" %>

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
                            RRB Entry 
                        </div>
                        <br />
                        <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="100%" OnActiveTabChanged="tabMain_ActiveTabChanged"
                            AutoPostBack="true">
                            <act:TabPanel ID="tabEntry" runat="server">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEntryHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Font-Size="Small" Text="Entry" ToolTip="RRB Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-2">
                                                <label for="txtRNo"><span style="color: #FF0000">*</span>RRB No</label>
                                                <asp:TextBox ID="txtRNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1" style="padding-top: 23px;">
                                                <asp:Button ID="btnGet" runat="server" OnClick="btnGet_Click" ToolTip="Complaint Search" CssClass="btn btn-sm btn-info" Text="Search"></asp:Button>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtRNo1"><span style="color: #FF0000">*</span>RNO 1</label>
                                                <asp:TextBox ID="txtRNo1" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtNameOfParticulars"><span style="color: #FF0000">*</span>Particulars Name</label>
                                                <asp:TextBox ID="txtNameOfParticulars" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtName">Name</label>
                                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtChargeDate"><span style="color: #FF0000">*</span>Charge Sheet Date</label>
                                                <asp:TextBox ID="txtChargeDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtNatCHSheet">Nature of Charge Sheet</label>
                                                <asp:TextBox ID="txtNatCHSheet" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlStatusCode"><span style="color: #FF0000">*</span>Status Code</label>
                                                <asp:DropDownList ID="ddlStatusCode" runat="server" CssClass="form-control input-sm">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblStatusCodeMIS" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtRegister">Register</label>
                                                <asp:TextBox ID="txtRegister" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="ddlFinal">Final</label>
                                                <asp:DropDownList ID="ddlFinal" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                    <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlScale">Scale</label>
                                                <asp:DropDownList ID="ddlScale" runat="server" CssClass="form-control input-sm">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlStatusCode"><span style="color: #FF0000">*</span>PF Number</label>
                                                <asp:TextBox ID="txtPFNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtRegister">R Number Date</label>
                                                <asp:TextBox ID="txtRNoDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="ddlFinal">Retirement Date</label>
                                                <asp:TextBox ID="txtRetirementDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlScale">ORD DA Date</label>
                                                <asp:TextBox ID="txtDAOrdDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtNaPunDa">Nature of Punishment of DA</label>
                                                <asp:TextBox ID="txtNaPunDa" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtBRComplaint">Branch</label>
                                                <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="ddlCircleOffice">Circle</label>
                                                <asp:DropDownList ID="ddlCircleOffice" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlDispAuthority">Disp Authority</label>
                                                <asp:DropDownList ID="ddlDispAuthority" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Chairman" Value="Chairman"></asp:ListItem>
                                                    <asp:ListItem Text="General Manager" Value="General Manager"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlDisAuthorityZone">Dis Authority Zone</label>
                                                <asp:DropDownList ID="ddlDisAuthorityZone" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Assam Gramin Vikas Bank" Value="AGVK"></asp:ListItem>
                                                    <asp:ListItem Text="Bangiya Gramin Vikas Bank" Value="BGVK"></asp:ListItem>
                                                    <asp:ListItem Text="Himanchal Gramin Bank" Value="HGB"></asp:ListItem>
                                                    <asp:ListItem Text="Dakshin Bihar Gramin Bank" Value="MBGB"></asp:ListItem>
                                                    <asp:ListItem Text="Manipur Rural Bank" Value="MRB"></asp:ListItem>
                                                    <asp:ListItem Text="Punjab Gramin Bank" Value="PGB"></asp:ListItem>
                                                    <asp:ListItem Text="Sarva Haryana Gramin Bank" Value="SHGB"></asp:ListItem>
                                                    <asp:ListItem Text="Prathama UP Gramin Bank" Value="SUGB"></asp:ListItem>
                                                    <asp:ListItem Text="Tripura Gramin Bank" Value="TGB"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtLapseNature">Lapse Nature</label>
                                                <asp:TextBox ID="txtLapseNature" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtIstDaDate">Ist DA Date</label>
                                                <asp:TextBox ID="txtIstDaDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txt2ndDADate">2nd DA Date</label>
                                                <asp:TextBox ID="txt2ndDADate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtDAProposal">DA Proposal</label>
                                                <asp:TextBox ID="txtDAProposal" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txt2DAProposal">2DA Proposal</label>
                                                <asp:TextBox ID="txt2DAProposal" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtCVOAdviceDate">CVO 1st Advice Date</label>
                                                <asp:TextBox ID="txtCVOAdviceDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtCVOAdvice">CVO 1st Advice</label>
                                                <asp:TextBox ID="txtCVOAdvice" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtCVO2AdviceDate">CVO 2 Advice Date</label>
                                                <asp:TextBox ID="txtCVO2AdviceDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txt2DAProposal">CVO 2 Advice</label>
                                                <asp:TextBox ID="txtCVO2Advice" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtAppPODate">PO Appointment Date</label>
                                                <asp:TextBox ID="txtAppPODate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtPOName">PO Name</label>
                                                <asp:TextBox ID="txtPOName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtCVO2AdviceDate">PO Appointment Date</label>
                                                <asp:TextBox ID="txtAppEODate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtEOName">EO Name</label>
                                                <asp:TextBox ID="txtEOName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="chkClosureDate">Closure Date</label>
                                                <asp:CheckBox ID="chkClosureDate" runat="server" Checked="false" />
                                                <asp:Label ID="lblClosureDate" runat="server" CssClass="lblCaption"></asp:Label>
                                                <asp:Panel ID="pnlClosureDate" runat="server" Visible="false">
                                                    <asp:TextBox ID="txtClosureDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtCbiRcNo1">CBI RC NO1</label>
                                                <asp:TextBox ID="txtCbiRcNo1" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtRC1Date">RC1 Date</label>
                                                <asp:TextBox ID="txtRC1Date" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtEOName">CBI RC No2</label>
                                                <asp:TextBox ID="txtCBIRCNo2" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-3">
                                                <label for="txtRC2Date">RC 2 Date</label>
                                                <asp:TextBox ID="txtRC2Date" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="ddlBankName">Bank Name</label>
                                                <asp:DropDownList ID="ddlBankName" runat="server" CssClass="form-control date input-sm">
                                                    <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                    <asp:ListItem Value="PNB" Text="PNB"></asp:ListItem>
                                                    <asp:ListItem Value="OBC" Text="OBC"></asp:ListItem>
                                                    <asp:ListItem Value="UBI" Text="UBI"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtLetterSentDate">Letter Sent Date</label>
                                                <asp:TextBox ID="txtLetterSentDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtEOName">Letter Sent To</label>
                                                <asp:DropDownList ID="ddlLetterSentTo" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                <asp:HiddenField ID="hidLetterSentTo" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group row" style="padding-right: 5px;">
                                            <div class="col-sm-2">
                                                <label for="txtReminderDate">Reminder Date</label>
                                                <asp:TextBox ID="txtReminderDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <label for="txtReplyReceivedDate">Reply Received Date</label>
                                                <asp:TextBox ID="txtReplyReceivedDate" runat="server" CssClass="form-control date input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4">
                                                <label for="ddlZoneNew">New Zone</label>
                                                <asp:DropDownList ID="ddlZoneNew" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlZoneNew_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-4">
                                                <label for="txtReplyReceivedDate">New Circle</label>
                                                <asp:DropDownList ID="ddlCircleNew" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
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
                            <act:TabPanel ID="tabList" runat="server" HeaderText="RRB Entry Details">
                                <HeaderTemplate>
                                    <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300" Font-Size="Small" Text="RRB Entry Details" ToolTip="List of RRB Entry"></asp:Label>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-sm-12 alert alert-dark">
                                        <div class="form-group row">
                                            <div class="col-sm-3">
                                                <label for="txtRNo_LIST">R No</label>
                                                <asp:TextBox ID="txtRNo_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtName_LIST">Name</label>
                                                <asp:TextBox ID="txtName_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <label for="txtZone_LIST">Zone</label>
                                                <asp:TextBox ID="txtZone_LIST" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1" style="padding-top: 22px;">
                                                <asp:Button ID="btnSearch_List" runat="server" OnClick="btnSearch_List_Click" ToolTip="RRB Search" Text="Search" CssClass="btn btn-sm btn-info" />
                                            </div>
                                            <div class="col-sm-2" style="padding-top: 22px;">
                                                <asp:Label ID="lblList" runat="server" CssClass="label label-danger"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <asp:GridView ID="gvMain" runat="server" OnRowCommand="gvMain_RowCommand" AutoGenerateColumns="false" CssClass="table input-sm table-bordered table-condensed">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnView" runat="server" CausesValidation="false" CommandName="View" ToolTip='<%# Eval("CODE") %>' CommandArgument='<%# Eval("CODE")%>' CssClass="btn btn-sm btn-danger" Text="Edit" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="RNO" HeaderText="R No." />
                                                        <asp:BoundField DataField="NAMEOFPARTICULARS" HeaderText="Name Particulars" />
                                                        <asp:BoundField DataField="CHARGEDATE" HeaderText="Date of CH" />
                                                        <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" />
                                                        <asp:BoundField DataField="SCALE" HeaderText="Scale" />
                                                        <asp:BoundField DataField="PFNO" HeaderText="PF Number" />
                                                        <asp:BoundField DataField="RNODATE" HeaderText="R No Date" />
                                                        <asp:BoundField DataField="EONAME" HeaderText="EO Name" />
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
                                    </div>
                                </ContentTemplate>
                            </act:TabPanel>
                        </act:TabContainer>
                        <asp:Panel ID="pnlMain" runat="server" Width="100%">



                            <asp:SqlDataSource ID="sdsNature" runat="server" ConnectionString="<%$ ConnectionStrings:dbVIGILANCEMIS %>"
                                SelectCommand="((SELECT '0' AS [NATURECODE],'-Select' AS [NATURECASE]) UNION (SELECT [CODE] AS NATURECODE, [NATURECASE] AS NATURECASE FROM [NATURECASE] WHERE ACTIVE='Y' AND FORTABLE='RRB')) ORDER BY NATURECODE"></asp:SqlDataSource>
                        </asp:Panel>
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
