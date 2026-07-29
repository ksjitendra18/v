<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmVigilance.aspx.cs" Inherits="VMISP.Mis.frmVigilance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <link href="../css/ssMain.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="../Js/JS_CommonValidation.js" type="text/javascript"></script>
    <script src="../Js/MaskedEditFix.js" type="text/javascript"></script>
    <style type="text/css">
        .button {
            padding: 2px 5px;
            font-size: 10px;
            text-align: center;
            cursor: pointer;
            outline: none;
            color: #fff;
            background-color: #4CAF50;
            border: none;
            border-radius: 15px;
            box-shadow: 0 9px #999;
        }

            .button:hover {
                background-color: #3e8e41;
            }

            .button:active {
                background-color: #3e8e41;
                box-shadow: 0 5px #666;
                transform: translateY(4px);
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F; color: White; font-weight: 700;">
                VIGILANCE
            </div>
            <asp:Panel ID="pnlMain" runat="server" Width="100%">
                <div>
                    <div style="float: right">
                        <asp:Panel ID="pnlHeader" runat="server" Visible="false">
                            <span class="lblCaptionHead" style="font-size: small; font-weight: bold">Entry By :</span>
                            <asp:Label ID="lblEntryBy" runat="server" Width="50px" ForeColor="#FF3300" Font-Size="small"
                                Font-Bold="True"></asp:Label>&nbsp<span class="lblCaptionHead" style="font-size: small; font-weight: bold">Entry Date :</span>
                            <asp:Label ID="lblEntryDate" runat="server" Width="75px" ForeColor="#FF3300" Font-Size="small"
                                Font-Bold="True"></asp:Label>&nbsp<span class="lblCaptionHead" style="font-size: small; font-weight: bold">Modify By :</span>
                            <asp:Label ID="lblModifyBy" runat="server" Width="50px" ForeColor="#FF3300" Font-Size="small"
                                Font-Bold="True"></asp:Label>&nbsp<span class="lblCaptionHead" style="font-size: small; font-weight: bold">Modify Date :</span>
                            <asp:Label ID="lblModifyDate" runat="server" Width="50px" ForeColor="#FF3300" Font-Size="small"
                                Font-Bold="True"></asp:Label>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                        </asp:Panel>
                    </div>
                </div>
                <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="1" Width="100%" OnActiveTabChanged="tabMain_ActiveTabChanged"
                    AutoPostBack="true">
                    <act:TabPanel ID="tabEntry" runat="server">
                        <HeaderTemplate>
                            <asp:Label ID="lblEntryHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                Font-Size="Small" Text="Entry" ToolTip="Vigilance Entry"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="pnlControls" runat="server">
                                <table width="100%" style="background-color: #FFE4E1">
                                    <tr>
                                        <td class="tdTextReight">
                                            <asp:Label ID="lblRNoRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                                                class="lblCaption">R No. :</span>
                                        </td>
                                        <td style="width: 110px">
                                            <asp:TextBox ID="txtRNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                            <asp:ImageButton ID="imgGet" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                                Width="25px" Height="30px" OnClick="btnGet_Click" ToolTip="Vigilance Search" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">R No-1 :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRNo1" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Name & Particulars :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNameOfParticulars" runat="server" CssClass="txtDefaultVig"></asp:TextBox>&nbsp;<asp:Button
                                                ID="btnShowNameOfParticulars_MODAL" runat="server" OnClick="btnShowNameOfParticulars_MODAL_Click"
                                                CssClass="button" Text="+" />
                                            <act:ModalPopupExtender ID="modalPopUp_NameOfParticulars" runat="server" PopupControlID="pnlModal_NameOfParticulars"
                                                TargetControlID="hidNameOfParticulars_MODAL" BackgroundCssClass="Background"
                                                DropShadow="true" ViewStateMode="Enabled">
                                            </act:ModalPopupExtender>
                                            <asp:HiddenField ID="hidNameOfParticulars_MODAL" runat="server" />
                                            <asp:Panel ID="pnlModal_NameOfParticulars" runat="server" CssClass="Popup" align="center"
                                                Style="display: none">
                                                <div id="divNameOfParticulars_MODAL" runat="server" style="width: 100%">
                                                    <table width="100%" style="border: 1px solid #000000; background-color: #808080">
                                                        <tr>
                                                            <td class="tdTextReight">
                                                                <span class="lblCaption">Name Particulars :</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtNameOfParticulars_MODAL" runat="server" Width="500" Height="250px"
                                                                    TextMode="MultiLine" CssClass="txtDefault"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <asp:Button ID="btnCloseNameOfParticulars_MODAL" runat="server" Text="Close" CssClass="btnDefault"
                                                        OnClick="btnCloseNameOfParticulars_MODAL_Click" />
                                                </div>
                                            </asp:Panel>
                                        </td>
                                        <td class="tdTextReight">
                                            <asp:Label ID="lblNameRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                                                class="lblCaption">Name :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Charge Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChargeDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceChargeDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtChargeDate" PopupButtonID="imgChargeDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgChargeDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Nat CH Sheet :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlNatCHSheet" runat="server" CssClass="ddlDefaultVig">
                                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Gross" Value="GROSS"></asp:ListItem>
                                                <asp:ListItem Text="Major" Value="MAJOR"></asp:ListItem>
                                                <asp:ListItem Text="Minor" Value="MINOR"></asp:ListItem>
                                                <asp:ListItem Text="Null" Value="NULL"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Zone :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlZone" runat="server" CssClass="ddlDefaultVig">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdTextReight">
                                            <asp:Label ID="lblStatusCodeRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                                                class="lblCaption">Status Code :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatusCode" runat="server" CssClass="ddlDefaultVig">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblStatusCodeMIS" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <asp:Label ID="lblRegisterRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">Register :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRegister" runat="server" CssClass="ddlDefaultVig"></asp:DropDownList>
                                            <asp:Label ID="lblRegister" runat="server"></asp:Label>
                                            <asp:HiddenField ID="hidRegister" runat="server" />
                                        </td>
                                        <td class="tdTextReight">
                                            <asp:Label ID="lblCircleOffice" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">Circle Office :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCircleOffice" runat="server" CssClass="ddlDefaultVig">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Final :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlFinal" runat="server" CssClass="txtNO">
                                                <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdTextReight">
                                            <asp:Label ID="lblScaleRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                                                class="lblCaption">Scale :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlScale" runat="server" CssClass="ddlDefaultVig">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <asp:Label ID="lblRNoDateRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                                                class="lblCaption">R No Date :</span>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkRNoDate" runat="server" Checked="false" />
                                            <asp:Label ID="lblRNoDate" runat="server" CssClass="lblCaption"></asp:Label>
                                            <asp:Panel ID="pnlRNoDate" runat="server" Visible="false">
                                                <asp:TextBox ID="txtRNoDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                                <act:CalendarExtender ID="ceRNoDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                    TargetControlID="txtRNoDate" PopupButtonID="imgRNoDate" CssClass="cal_Theme1">
                                                </act:CalendarExtender>
                                                <asp:ImageButton ID="imgRNoDate" runat="server" AlternateText="Please Select date!!"
                                                    ImageUrl="~/images/calendar.png" />
                                            </asp:Panel>
                                        </td>
                                        <td class="tdTextReight">
                                            <asp:Label ID="lblPFNoRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                                                class="lblCaption">PF No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPFNo" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Retirement :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRetirementDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceRetirementDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtRetirementDate" PopupButtonID="imgRetirementDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgRetirementDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">DA Ord Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDAOrdDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceDAOrdDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtDAOrdDate" PopupButtonID="imgDAOrdDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgDAOrdDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Na Pun DA :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNAPUNDA" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Penalty Type :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPenaltyType" runat="server" CssClass="ddlDefaultVig">
                                            </asp:DropDownList>
                                            <asp:HiddenField runat="server" ID="hidPenaltyType" />
                                        </td>
                                        <td class="tdTextReight">
                                            <asp:Label ID="lblDisAuthoritysCircleRequired" runat="server" Text="*" Font-Bold="True"
                                                ForeColor="Red"></asp:Label><span class="lblCaption">DA_CO/ZO/HO :</span>
                                        </td>
                                        <td>
                                            <%--<asp:TextBox ID="txtDisAuthoritysCircle" runat="server" CssClass="txtDefaultVig"></asp:TextBox>--%>
                                            <asp:DropDownList ID="ddlDisAuthoritysCircle" runat="server" CssClass="ddlDefaultVig"></asp:DropDownList>
                                            <asp:HiddenField ID="hidDisAuthoritysCircle" runat="server" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Disp Authority :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDispAuthority" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Ist DA Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIstDaDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceIstDaDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtIstDaDate" PopupButtonID="imgIstDaDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgIstDaDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">DA Proposal :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDAProposal" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Final Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFinalDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceFinalDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtFinalDate" PopupButtonID="imgFinalDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgFinalDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVO Advice :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVOAdvice" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVO Advice :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVOAdviceDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceCVOAdviceDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtCVOAdviceDate" PopupButtonID="imgCVOAdviceDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgCVOAdviceDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">2nd DA Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt2ndDADate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ce2ndDADate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txt2ndDADate" PopupButtonID="img2ndDADate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="img2ndDADate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">2DA Proposal :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt2DAProposal" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVO 2 Advice :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVO2Advice" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVO 2 Advice :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVO2AdviceDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceCVO2AdviceDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtCVO2AdviceDate" PopupButtonID="imgCVO2AdviceDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgCVO2AdviceDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Account Name :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAccountName" runat="server" CssClass="txtDefaultVig"></asp:TextBox>&nbsp;<asp:Button
                                                ID="btnShowAccountName_MODAL" runat="server" OnClick="btnShowAccountName_MODAL_Click"
                                                CssClass="button" Text="+" />
                                            <act:ModalPopupExtender ID="modalPopUp_AccountName" runat="server" PopupControlID="pnlModal_AccountName"
                                                TargetControlID="hidAccountName_MODAL" BackgroundCssClass="Background" DropShadow="true"
                                                ViewStateMode="Enabled">
                                            </act:ModalPopupExtender>
                                            <asp:HiddenField ID="hidAccountName_MODAL" runat="server" />
                                            <asp:Panel ID="pnlModal_AccountName" runat="server" CssClass="Popup" align="center"
                                                Style="display: none">
                                                <div id="divpnlModalSR_AccountName" runat="server" style="width: 100%">
                                                    <table width="100%" style="border: 1px solid #000000; background-color: #808080">
                                                        <tr>
                                                            <td class="tdTextReight">
                                                                <span class="lblCaption">A/c Name :</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAccountName_MODAL" runat="server" Width="500" Height="250px"
                                                                    TextMode="MultiLine" CssClass="txtDefault"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <asp:Button ID="btnCloseMODAL_AccountName" runat="server" Text="Close" CssClass="btnDefault"
                                                        OnClick="btnCloseMODAL_AccountName_Click" />
                                                </div>
                                            </asp:Panel>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Source :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSource" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">State :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlState" runat="server" CssClass="ddlDefault1">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Place in Present Scale :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPlaceinPresentScaleDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="cePlaceinPresentScaleDate" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtPlaceinPresentScaleDate" PopupButtonID="imgPlaceinPresentScaleDate"
                                                CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgPlaceinPresentScaleDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Refused Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSanctionRefusedDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceSanctionRefusedDate" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtSanctionRefusedDate" PopupButtonID="imgSanctionRefusedDate"
                                                CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgSanctionRefusedDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Designation :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDesignation" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">DA Proposed Punishment :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPunishmentProposedbyDA" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Supplementary C/S Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCompRecDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceCompRecDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtCompRecDate" PopupButtonID="imgCompRecDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgCompRecDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Supplementary C/S Status :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStatusinBrief" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Branch :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Penalty :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPenalty" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Amount :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="txtNO" Style="text-align: right"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Dt CSO REP. :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCSOREPDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceCSOREPDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtCSOREPDate" PopupButtonID="imgCSOREPDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgCSOREPDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Con Enq Dt :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtConEnqDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceConEnqDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtConEnqDate" PopupButtonID="imgConEnqDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgConEnqDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Suspension :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSuspensionDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceSuspensionDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtSuspensionDate" PopupButtonID="imgSuspensionDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgSuspensionDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CBI RC NO1 :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCbiRcNo1" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">RC1 Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRC1Date" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceRC1Date" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtRC1Date" PopupButtonID="imgRC1Date" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgRC1Date" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CBI RC No2 :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCBIRCNo2" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">RC 2 Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRC2Date" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceRC2Date" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtRC2Date" PopupButtonID="imgRC2Date" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgRC2Date" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVC OM No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVCOMNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">OM CVC Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOMCVCDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceOMCVCDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtOMCVCDate" PopupButtonID="imgOMCVCDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgOMCVCDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">RC Source :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRCSource" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Investig :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInvestig" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">App EO Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAppEODate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceAppEODate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtAppEODate" PopupButtonID="imgAppEODate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgAppEODate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">App PO Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAppPODate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceAppPODate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtAppPODate" PopupButtonID="imgAppPODate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgAppPODate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">PO Name :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPOName" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">2nd Stage CVC :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVC2Proposed" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CBI Recom :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCBIRecom" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Field 1 :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtField1" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Prev Case/Punishments :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPrevCasePunishment" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">AC Nature :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNatureofAccount" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Sanction Order :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSanctionOrderDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceSanctionOrderDate" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtSanctionOrderDate" PopupButtonID="imgSanctionOrderDate"
                                                CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgSanctionOrderDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Rec CVC 2 :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRecCVC2" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceRecCVC2" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtRecCVC2" PopupButtonID="imgRecCVC2" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgRecCVC2" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVC Proposed Action :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProposedActiontoCVC" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">EO Name :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEOName" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVC 2 Ref :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVC2Ref" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceCVC2Ref" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtCVC2Ref" PopupButtonID="imgCVC2Ref" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgCVC2Ref" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Review Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReviewDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceReviewDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtReviewDate" PopupButtonID="imgReviewDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgReviewDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Reg Invok :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRegInvok" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Nature Case :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlNature" runat="server" CssClass="ddlDefaultVig">
                                            </asp:DropDownList>
                                            <asp:Panel ID="pnlNatureMIS" runat="server" Visible="False">
                                                <asp:Label ID="lblNatureMIS" runat="server"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Refer To CVC :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReferToCVCDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceReferToCVCDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtReferToCVCDate" PopupButtonID="imgReferToCVCDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgReferToCVCDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Recomm of CVC :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRecommofCVC" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVC's Advice II :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVCAdbiceII" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Basic Pay :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBasicPay" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Lodi Case :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlLodiCase" runat="server" CssClass="txtNO">
                                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hidLodiCase" runat="server" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Lodi No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLodiNo" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Closure Date :</span>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkClosureDate" runat="server" Checked="false" />
                                            <asp:Label ID="lblClosureDate" runat="server" CssClass="lblCaption"></asp:Label>
                                            <asp:Panel ID="pnlClosureDate" runat="server" Visible="false">
                                                <asp:TextBox ID="txtClosureDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                                <act:MaskedEditExtender ID="meeClosureDate" runat="server" TargetControlID="txtClosureDate"
                                                    Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                    CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                                </act:MaskedEditExtender>
                                                <act:CalendarExtender ID="ceClosureDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                    TargetControlID="txtClosureDate" PopupButtonID="imgClosureDate" CssClass="cal_Theme1">
                                                </act:CalendarExtender>
                                                <asp:ImageButton ID="imgClosureDate" runat="server" AlternateText="Please Select date!!"
                                                    ImageUrl="~/images/calendar.png" />
                                            </asp:Panel>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Lapse Nature :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLapseNature" runat="server" CssClass="txtDefaultVig"></asp:TextBox>&nbsp;<asp:Button
                                                ID="btnShowLapseNature_MODAL" runat="server" OnClick="btnShowLapseNature_MODAL_Click"
                                                CssClass="button" Text="+" />
                                            <act:ModalPopupExtender ID="modalPopUp_LapseNature" runat="server" PopupControlID="pnlModal_LapseNature"
                                                TargetControlID="hidLapseNature_MODAL" BackgroundCssClass="Background" DropShadow="true"
                                                ViewStateMode="Enabled">
                                            </act:ModalPopupExtender>
                                            <asp:HiddenField ID="hidLapseNature_MODAL" runat="server" />
                                            <asp:Panel ID="pnlModal_LapseNature" runat="server" CssClass="Popup" align="center"
                                                Style="display: none">
                                                <div id="divLapseNature_MODAL" runat="server" style="width: 100%">
                                                    <table width="100%" style="border: 1px solid #000000; background-color: #808080">
                                                        <tr>
                                                            <td class="tdTextReight">
                                                                <span class="lblCaption">Lapse Nature :</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLapseNature_MODAL" runat="server" Width="495" Height="250px"
                                                                    TextMode="MultiLine" CssClass="txtDefault"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <asp:Button ID="btnCloseMODAL_LapseNature" runat="server" Text="Close" CssClass="btnDefault"
                                                        OnClick="btnCloseMODAL_LapseNature_Click" />
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">A1C C/S CVC :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtA1CSCVC" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meA1CSCVC" runat="server" TargetControlID="txtA1CSCVC"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceA1CSCVC" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtA1CSCVC" PopupButtonID="imgA1CSCVC" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgA1CSCVC" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">A1E EO/PO CVC :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtA1EOPOCVC" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meA1EOPOCVC" runat="server" TargetControlID="txtA1EOPOCVC"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceA1EOPOCVC" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtA1EOPOCVC" PopupButtonID="imgA1EOPOCVC" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgA1EOPOCVC" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">A2 F/O CVC :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtA2FOCVC" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meA2FOCVC" runat="server" TargetControlID="txtA2FOCVC"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceA2FOCVC" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtA2FOCVC" PopupButtonID="imgA2FOCVC" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgA2FOCVC" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CDI Name :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCDIName" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">App CDI Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAppCDIDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeAppCDIDate" runat="server" TargetControlID="txtAppCDIDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceAppCDIDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtAppCDIDate" PopupButtonID="imgAppCDIDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgAppCDIDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Penalty Proceedings :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPenaltyProceedings" runat="server" CssClass="ddlDefaultVig">
                                            </asp:DropDownList>
                                            <asp:HiddenField runat="server" ID="hidPenaltyProceedings" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Lodi Inclusion Reason :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLodiInclusionReason" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Lodi Deletion Reason :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLodiDeletionReason" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Lodi Code :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLodiCode" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Bank Name :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBankName" runat="server" CssClass="ddlDefaultVig">
                                                <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                <asp:ListItem Value="PNB" Text="PNB"></asp:ListItem>
                                                <asp:ListItem Value="OBC" Text="OBC"></asp:ListItem>
                                                <asp:ListItem Value="UBI" Text="UBI"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">TMSAC Ref No. :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTMSACRefNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Letter Sent Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLetterSentDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeLetterSentDate" runat="server" TargetControlID="txtLetterSentDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceLetterSentDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtLetterSentDate" PopupButtonID="imgLetterSentDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgLetterSentDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" /></td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Letter Sent To :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlLetterSentTo" runat="server" CssClass="ddlDefault1"></asp:DropDownList>
                                            <asp:HiddenField ID="hidLetterSentTo" runat="server" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Reminder Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReminderDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeReminderDate" runat="server" TargetControlID="txtReminderDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceReminderDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtReminderDate" PopupButtonID="imgReminderDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgReminderDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" /></td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Reply Received Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReplyReceivedDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeReplyReceivedDate" runat="server" TargetControlID="txtReplyReceivedDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceReplyReceivedDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtReplyReceivedDate" PopupButtonID="imgReplyReceivedDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgReplyReceivedDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" /></td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Present Posting :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPresentPosting" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">EO CDI :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEoCdi" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">D Ref No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDRefNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Reasons for Inclusion :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReasonsforInclusion" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">DA Ref No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDARefNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Rec Report :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRecReportDate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtDeletionReasons" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">U/S :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUS" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Occur Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOccurDate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtInvOfficerName" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">IR-CBI Pending :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIRCBIPending" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Pol Fir No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPolFirNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">FIR Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFIRDate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtADV1Awaited" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">PO CBI :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPOCBI" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Last RH Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLastRHDate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtDTSHear" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">NO Award S :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNoAwardS" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Written Brief PO :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtWrittenBriefPO" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtBasicPayDate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtConnectedVigCase" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CH-Sheet Filed :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCHSheetFiledDate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txt2ndPending" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">DA Sent Advice :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAdviceSentToDADate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtReComp" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Appeal :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAppeal" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtWrittenBriefCODate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtPrelimEnq" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtAdv2Awt" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Regu Enq :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReguEnq" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtRegulatDate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtLodiNew" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Ist Pending :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIstPending" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Revocation :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRevocationDate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtCommitmentDate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtCOReplyDate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtTargetDate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtERCODate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                            <asp:TextBox ID="txtSanctionRecvDate" runat="server" CssClass="txtDate"></asp:TextBox>
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
                                <table style="width: 100%; background-color: #FFE4E1">
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">New Zone :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlZoneNew" Width="100%" runat="server" CssClass="ddlDefault" OnSelectedIndexChanged="ddlZoneNew_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">New Circle :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCircleNew" Width="100%" runat="server" CssClass="ddlDefault"></asp:DropDownList></td>
                                    </tr>
                                </table>
                                <table width="100%" style="background-color: #FFE4E1">
                                    <tr>
                                        <td class="tdTextReight" style="width: 14.5%">
                                            <span class="lblCaption">Status :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStatus" runat="server" CssClass="txtDefaultVig" Width="95%" TextMode="MultiLine"
                                                Height="35px"></asp:TextBox>&nbsp;
                                            <asp:Button ID="btnShowStatus_MODAL" runat="server" CssClass="button" OnClick="btnShowStatus_MODAL_Click"
                                                Text="+" />
                                            <act:ModalPopupExtender ID="modalPopUp_Status" runat="server" PopupControlID="pnlModal_Status"
                                                TargetControlID="hidStatus_MODAL" BackgroundCssClass="Background" DropShadow="true"
                                                ViewStateMode="Enabled">
                                            </act:ModalPopupExtender>
                                            <asp:HiddenField ID="hidStatus_MODAL" runat="server" />
                                            <asp:Panel ID="pnlModal_Status" runat="server" CssClass="Popup" align="center" Style="display: none">
                                                <div id="divModal_Status" runat="server" style="width: 100%">
                                                    <table width="100%" style="border: 1px solid #000000; background-color: #808080">
                                                        <tr>
                                                            <td class="tdTextReight">
                                                                <span class="lblCaption">Status :</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtStatus_MODAL" runat="server" Width="500" Height="250px" TextMode="MultiLine"
                                                                    CssClass="txtDefault"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <asp:Button ID="btnCloseMODAL_Status" runat="server" Text="Close" CssClass="btnDefault"
                                                        OnClick="btnCloseMODAL_Status_Click" />
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <asp:Panel ID="pnlHOStatus" runat="server" Visible="False">
                                            <td class="tdTextReight" style="width: 8.6%">
                                                <span class="lblCaption">HO Status :</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHOStatus" runat="server" CssClass="txtDefaultVig" Width="95%"
                                                    Height="35px"></asp:TextBox>
                                            </td>
                                        </asp:Panel>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight" style="width: 15%">
                                            <span class="lblCaption">Dealing Officer Remarks :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDealingOfficerRemarks" runat="server" Width="95%" placeholder="Enter Dealing Officer Remarks, If Any...." TextMode="MultiLine" onkeypress="return blockSpecialChar(event)" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" style="background-color: #FFE4E1">
                                    <tr>
                                        <td style="width: 10%">&nbsp;&nbsp;
                                        </td>
                                        <td style="width: 90%; text-align: center;">
                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btnDefault" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btnDefault" OnClick="btnUpdate_Click"
                                                Visible="False" />&nbsp;&nbsp;
                                            <asp:Button ID="btnDelete" runat="server" CssClass="btnDefault" Text="Delete" OnClick="btnDelete_Click"
                                                Visible="False" />
                                            &nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnDefault"
                                                OnClick="btnCancel_Click" />&nbsp;&nbsp;<asp:Label ID="lblMsg" runat="server" CssClass="lblMsg"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="tabList" runat="server" HeaderText="List">
                        <HeaderTemplate>
                            <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                Font-Size="Small" Text="List" ToolTip="List of Vigilance Entry"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="margin-top: -10px; border-bottom: 1px solid;">
                                <tr>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">R No. :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRNo_LIST" runat="server" Width="150px" CssClass="txtDefaultVig"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">A/c Name :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAccountName_LIST" runat="server" Width="150px" CssClass="txtDefaultVig"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Name :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName_LIST" runat="server" Width="150px" CssClass="txtDefaultVig"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">PF Number :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPFNumber_LIST" runat="server" Width="150px" CssClass="txtDefaultVig"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Branch :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBranch_LIST" runat="server" Width="150px" CssClass="txtDefaultVig"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Circle :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCircle_LIST" runat="server" Width="150px" CssClass="txtDefaultVig"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">CBI Rc No 1 :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCBIRCNO_LIST" runat="server" Width="150px" CssClass="txtDefaultVig"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">CVC OM No. :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCVCOMNO_LIST" runat="server" Width="150px" CssClass="txtDefaultVig"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Status :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStatus_LIST" runat="server" Width="150px" CssClass="txtDefaultVig"></asp:TextBox>
                                        &nbsp;<asp:ImageButton ID="imgSearch_List" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                            Width="25px" Height="30px" OnClick="imgSearch_LIST_Click" ToolTip="Vigilance Search" />
                                        &nbsp;&nbsp;<asp:Label ID="lblList" runat="server" CssClass="lblMsg"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Both" Height="350px" Width="100%">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100%">
                                            <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" DataKeyNames="RNO"
                                                CellPadding="3" GridLines="None" ViewStateMode="Enabled" Style="margin-top: 0px"
                                                BackColor="White" BorderColor="White" BorderWidth="2px" CellSpacing="1" Width="100%"
                                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvMain_PageIndexChanging"
                                                OnSorting="gvMain_Sorting" OnRowCommand="gvMain_RowCommand" OnRowDataBound="gvMain_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Select
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnView" runat="server" CausesValidation="false" CommandName="View"
                                                                ToolTip='<%# Eval("CODE") %>' ImageUrl="~/images/selg_16.png" Height="20px" Width="18px"
                                                                CommandArgument='<%# Eval("CODE")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="RNO" HeaderText="R No." SortExpression="RNO" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RNO1" HeaderText="R NO 1" SortExpression="RNO1" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NAMEOFPARTICULARS" HeaderText="Name & Particulars" SortExpression="NAMEOFPARTICULARS"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NAME" HeaderText="Name" SortExpression="NAME" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CHARGEDATE" HeaderText="Charge Date" SortExpression="CHARGEDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NATCHSHEET" HeaderText="Nat CH Sheet" SortExpression="NATCHSHEET"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="ZONE" HeaderText="Zone" SortExpression="ZONE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" SortExpression="STATUSCODE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="REGISTER" HeaderText="Register" SortExpression="REGISTER"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" SortExpression="CIRCLEOFFICE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="FINAL" HeaderText="Final" SortExpression="FINAL" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SCALE" HeaderText="Scale" SortExpression="SCALE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RNODATE" HeaderText="R No Date" SortExpression="RNODATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="PFNO" HeaderText="PF Number" SortExpression="PFNO" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RETIREMENTDATE" HeaderText="Retirement Date" SortExpression="RETIREMENTDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DAORDDATE" HeaderText="DA ORD Date" SortExpression="DAORDDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NAPUNDA" HeaderText="Na Pun DA" SortExpression="NAPUNDA"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DISAUTHORITYCIRCLE" HeaderText="DA_CO/ZO/HO" SortExpression="DISAUTHORITYCIRCLE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DISPAUTHORITY" HeaderText="Disp Authority" SortExpression="DISPAUTHORITY"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="BRCOMPLAINT" HeaderText="Branch" SortExpression="BRCOMPLAINT"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="ISTDADATE" HeaderText="Ist DA Date" SortExpression="ISTDADATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DAPROPOSAL" HeaderText="DA Proposal" SortExpression="DAPROPOSAL"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="FINALDATE" HeaderText="Final Date" SortExpression="FINALDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVOADVICE" HeaderText="CVO Advice" SortExpression="CVOADVICE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVOADVICEDATE" HeaderText="CVO Advice Date" SortExpression="CVOADVICEDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DA2NDDATE" HeaderText="DA 2nd Date" SortExpression="DA2NDDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DAPROPOSAL2" HeaderText="CVO 2 Advice" SortExpression="DAPROPOSAL2"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVO2ADVICEDATE" HeaderText="CVO 2 Advice Date" SortExpression="CVO2ADVICEDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="ACCOUNTNAME" HeaderText="Account Name" SortExpression="ACCOUNTNAME"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SOURCE" HeaderText="Source" SortExpression="SOURCE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="STATE" HeaderText="State" SortExpression="STATE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="PLACEINPRESENTSCALEDATE" HeaderText="Place in Present Scale Date"
                                                        SortExpression="PLACEINPRESENTSCALEDATE" HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SANCTIONREFUSED" HeaderText="Refused Date" SortExpression="SANCTIONREFUSED"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DESIGNATION" HeaderText="Designation" SortExpression="DESIGNATION"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="PUNISHMENTPROPOSEDBYDA" HeaderText="DA Proposed Punishment"
                                                        SortExpression="PUNISHMENTPROPOSEDBYDA" HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RECDATECOMP" HeaderText="Comp Rec" SortExpression="RECDATECOMP"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="STATUSINBRIEF" HeaderText="Status in Brief" SortExpression="STATUSINBRIEF"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CLOSUREDATE" HeaderText="Closure Date" SortExpression="CLOSUREDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="PENALTY" HeaderText="Penalty" SortExpression="PENALTY"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="AMOUNT" HeaderText="Amount" SortExpression="AMOUNT" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RC1DATE" HeaderText="RC 1 Date" SortExpression="RC1DATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SUSPENSION" HeaderText="Suspension Date" SortExpression="SUSPENSION"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="LAPSENATURE" HeaderText="Lapse Nature" SortExpression="LAPSENATURE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CBIRCNO1" HeaderText="CBI RC No 1" SortExpression="CBIRCNO1"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CONENQDATE" HeaderText="CON Enq Date" SortExpression="CONENQDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CBIRECOM" HeaderText="CBI ReCom" SortExpression="CBIRECOM"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RECOMMOFCVC" HeaderText="Recmm Of CVC" SortExpression="RECOMMOFCVC"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVCOMNO" HeaderText="CVC OM No" SortExpression="CVCOMNO"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="OMCVCDATE" HeaderText="OM CVC Date" SortExpression="OMCVCDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RCSOURCE" HeaderText="RC Source" SortExpression="RCSOURCE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="INVESTIG" HeaderText="Investig" SortExpression="INVESTIG"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="APPEODATE" HeaderText="App EO Date" SortExpression="APPEODATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="APPPODATE" HeaderText="App PO Date" SortExpression="APPPODATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="PONAME" HeaderText="PO Name" SortExpression="PONAME" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVC2PROPOSED" HeaderText="2nd Stage" SortExpression="CVC2PROPOSED"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CBIRCNO2" HeaderText="CBI RC No2" SortExpression="CBIRCNO2"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="FIELD1" HeaderText="Field 1" SortExpression="FIELD1" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="PREVCASEPUNISHMENT" HeaderText="Prev Case/Punishments"
                                                        SortExpression="PREVCASEPUNISHMENT" HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NATUREOFACCOUNT" HeaderText="AC Nature" SortExpression="NATUREOFACCOUNT"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SANCTIONORDERDATE" HeaderText="Sanction Order Date" SortExpression="SANCTIONORDERDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RECCVC2" HeaderText="Rec CVC 2 Date" SortExpression="RECCVC2"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="PROPOSEDACTIONTOCVC" HeaderText="CVC Proposed Action"
                                                        SortExpression="PROPOSEDACTIONTOCVC" HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="EONAME" HeaderText="EO Name" SortExpression="EONAME" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVC2REF" HeaderText="CVC 2 Ref Date" SortExpression="CVC2REF"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="REVIEWDATE" HeaderText="Review Date" SortExpression="REVIEWDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="REGINVOK" HeaderText="Reg Invok" SortExpression="REGINVOK"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NATURE" HeaderText="Nature Case" SortExpression="NATURE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="REFERTOCVCDATE" HeaderText="Refer To CVC" SortExpression="REFERTOCVCDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RC2DATE" HeaderText="RC 2 Date" SortExpression="RC2DATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVCADVICEII" HeaderText="CVC's Advice II" SortExpression="CVCADVICEII"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="BASICPAY" HeaderText="Basic Pay" SortExpression="BASICPAY"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="LODICASE" HeaderText="Lodi Case" SortExpression="LODICASE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="LODINO" HeaderText="Lodi No" SortExpression="LODINO" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CDINAME" HeaderText="CDI Name" SortExpression="CDINAME" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="APPCDIDATE" HeaderText="CDI App Date" SortExpression="APPCDIDATE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="LODIINCLUSIONREASON" HeaderText="Lodi Inclusion Reason" SortExpression="LODIINCLUSIONREASON" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="LODIDELETIONREASON" HeaderText="Lodi Deletion Reason" SortExpression="LODIDELETIONREASON" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="LODICODE" HeaderText="Lodi Code" SortExpression="LODICODE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="APPROVALSTATUSTEXT" HeaderText="Checker Status" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CHECKERREMARKS" HeaderText="Checker Remarks" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:TemplateField HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText">
                                                        <HeaderTemplate>
                                                            Status
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSTATUS_GV" runat="server" Text='<%# Bind("SHORTSTATUS") %>' ToolTip='<%# Eval("STATUS") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="250px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerSettings PageButtonCount="10" />
                                                <PagerStyle Font-Bold="True" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </act:TabPanel>
                </act:TabContainer>
                <asp:HiddenField ID="hidCircleOffice" runat="server" />
                <asp:HiddenField ID="hidZone" runat="server" />
                <asp:HiddenField ID="hidStatusCode" runat="server" />
                <asp:HiddenField ID="hidNatureCase" runat="server" />
                <asp:HiddenField ID="hidScale" runat="server" />
                <asp:HiddenField ID="hidNatCHSheet" runat="server" />
                <asp:HiddenField ID="hidFinal" runat="server" />
                <asp:HiddenField ID="hidState" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
