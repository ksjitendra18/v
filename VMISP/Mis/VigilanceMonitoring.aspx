<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="VigilanceMonitoring.aspx.cs" Inherits="VMISP.Mis.VigilanceMonitoring" %>

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
                VIGILANCE MISC
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
                                Font-Size="Small" Text="Entry" ToolTip="Vigilance Temp Entry"></asp:Label>
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
                                                Width="25px" Height="30px" OnClick="btnGet_Click" ToolTip="Vigilance Temp Search" />
                                        </td>
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
                                            <asp:Label ID="lblNameRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                                                class="lblCaption">Name :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
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
                                    <tr style="display: none;">
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
                                            <span class="lblCaption">Final :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlFinal" runat="server" CssClass="txtNO">
                                                <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                            </asp:DropDownList>
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
                                            <span class="lblCaption">Register :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRegister" runat="server" CssClass="ddlDefaultVig"></asp:DropDownList>
                                            <asp:HiddenField ID="hidRegister" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
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
                                            <span class="lblCaption">Closure Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDAOrdDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:CalendarExtender ID="ceDAOrdDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtDAOrdDate" PopupButtonID="imgDAOrdDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgDAOrdDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <asp:Label ID="lblCircleOffice" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">Circle Office :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCircleOffice" runat="server" CssClass="ddlDefaultVig">
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
                                            <span class="lblCaption">Vig Case :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtConnectedVigCase" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
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
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Amount :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="txtNO" Style="text-align: right"></asp:TextBox>
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
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Designation :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDesignation" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
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
                                        <td class="tdTextReight">
                                            <asp:Label ID="lblPFNoRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                                                class="lblCaption">PF No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPFNo" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Branch :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
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
                                            <span class="lblCaption">DA Ref No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDARefNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">U/S :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlUS" runat="server" CssClass="txtNO">
                                                <asp:ListItem Text="Yes" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="NO"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hidUS" runat="server" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CBI RC NO1 :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCbiRcNo1" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">External Source :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtExternalSource" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight" style="display: none;">
                                            <span class="lblCaption">AC Nature :</span>
                                        </td>
                                        <td style="display: none;">
                                            <asp:DropDownList ID="ddlAccountNature" runat="server" CssClass="ddlDefaultVig">
                                            </asp:DropDownList>
                                            <asp:Panel ID="pnlNatureMIS" runat="server" Visible="False">
                                                <asp:Label ID="lblNatureMIS" runat="server"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">External Source Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtExternalSourceDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeExternalSourceDate" runat="server" TargetControlID="txtExternalSourceDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtExternalSourceDate" PopupButtonID="imgExternalSourceDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgExternalSourceDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Bank Name :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBankName" runat="server" CssClass="ddlDefaultVig">
                                                <asp:ListItem Text="select" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="PNB" Value="PNB"></asp:ListItem>
                                                <asp:ListItem Text="OBC" Value="OBC"></asp:ListItem>
                                                <asp:ListItem Text="UBI" Value="UBI"></asp:ListItem>
                                            </asp:DropDownList>
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
                                    <tr style="display: none;">
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
                                            <span class="lblCaption">PD Ref No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPDRefNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Na Pun DA :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNAPUNDA" runat="server" CssClass="txtDefaultVig"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Sanction order  :</span>
                                        </td>
                                        <td>
                                             <asp:TextBox ID="txtSanctionOrder" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meSanctionOrder" runat="server" TargetControlID="txtSanctionOrder"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceSanctionOrder" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtSanctionOrder" PopupButtonID="imgSanctionOrder" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgSanctionOrder" runat="server" AlternateText="Please Select date!!"
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
                                        <td class="tdTextReight" style="width: 10.5%">
                                            <span class="lblCaption">Status :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStatus" runat="server" CssClass="txtDefaultVig" Width="91%" TextMode="MultiLine"
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
                                Font-Size="Small" Text="List" ToolTip="List of Vigilance Temp Entry"></asp:Label>
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
                                        <span class="lblCaption">Status :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStatus_LIST" runat="server" Width="150px" CssClass="txtDefaultVig"></asp:TextBox>
                                        &nbsp;<asp:ImageButton ID="imgSearch_List" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                            Width="25px" Height="30px" OnClick="imgSearch_LIST_Click" ToolTip="Vigilance Temp Search" />
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
                                                    <asp:BoundField DataField="NAME" HeaderText="Name" SortExpression="NAME" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CHARGEDATE" HeaderText="Charge Date" SortExpression="CHARGEDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NATCHSHEET" HeaderText="Nat CH Sheet" SortExpression="NATCHSHEET"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
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
                                                    <asp:BoundField DataField="BRANCH" HeaderText="Branch" SortExpression="BRANCH"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="ACCOUNTNAME" HeaderText="Account Name" SortExpression="ACCOUNTNAME"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SOURCE" HeaderText="Source" SortExpression="SOURCE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="STATE" HeaderText="State" SortExpression="STATE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DESIGNATION" HeaderText="Designation" SortExpression="DESIGNATION"
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
                                                    <asp:BoundField DataField="ACCOUNTNATURE" HeaderText="AC Nature" SortExpression="ACCOUNTNATURE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
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
