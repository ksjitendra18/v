<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmWBStructure.aspx.cs" Inherits="VMISP.Mis.frmWBStructure" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/ssMain.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="../Js/JS_CommonValidation.js" type="text/javascript"></script>
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
                Whistle Blower (WB)
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
                                Font-Size="Small" Text="Entry" ToolTip="Whistle Blower Structure Entry"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="background-color: #FFE4E1">
                                <tr>
                                    <td class="tdTextReight">
                                        <asp:Label ID="lblRNoRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">R No. :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        <asp:ImageButton ID="imgGet" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                            Width="25px" Height="30px" OnClick="btnGet_Click" ToolTip="Whistle Blower Search" />
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Comp Rec Date :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCompRecDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                        <act:MaskedEditExtender ID="meeCompRecDate" runat="server" TargetControlID="txtCompRecDate"
                                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="" Enabled="True">
                                        </act:MaskedEditExtender>
                                        <act:CalendarExtender ID="ceCompRecDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                            TargetControlID="txtCompRecDate" PopupButtonID="imgCompRecDate" CssClass="cal_Theme1">
                                        </act:CalendarExtender>
                                        <asp:ImageButton ID="imgCompRecDate" runat="server" AlternateText="Please Select date!!"
                                            ImageUrl="~/images/calendar.png" />
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">BR Complaint :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        <asp:Button ID="btnShowBranch_MODAL" runat="server" OnClick="btnShowBranch_MODAL_Click"
                                            CssClass="button" Text="+" />
                                        <act:ModalPopupExtender ID="modalPopUp_Branch" runat="server" PopupControlID="pnlModal_Branch"
                                            TargetControlID="hidBranch_MODAL" BackgroundCssClass="Background" DropShadow="true"
                                            ViewStateMode="Enabled">
                                        </act:ModalPopupExtender>
                                        <asp:HiddenField ID="hidBranch_MODAL" runat="server" />
                                        <asp:Panel ID="pnlModal_Branch" runat="server" CssClass="Popup" align="center" Style="display: none">
                                            <div id="divBranch_MODAL" runat="server" style="width: 100%">
                                                <table width="100%" style="border: 1px solid #000000; background-color: #808080">
                                                    <tr>
                                                        <td class="tdTextReight">
                                                            <span class="lblCaption">Branch :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBranch_MODAL" runat="server" Width="500" Height="250px" TextMode="MultiLine"
                                                                CssClass="txtDefault"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:Button ID="btnCloseBranch_MODAL" runat="server" Text="Close" CssClass="btnDefault"
                                                    OnClick="btnCloseBranch_MODAL_Click" />
                                            </div>
                                        </asp:Panel>
                                    </td>
                                    <td class="tdTextReight">
                                        <asp:Label ID="lblCircleOffice" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">Circle Office :</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCircleOffice" runat="server" CssClass="ddlDefault1">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Comp No :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCompNo" runat="server" CssClass="txtNO"></asp:TextBox>
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
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceClosureDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtClosureDate" PopupButtonID="imgClosureDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgClosureDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </asp:Panel>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Accused :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAccused" runat="server" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Allegations :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAllegations" runat="server" CssClass="txtDefault"></asp:TextBox>&nbsp;<asp:Button
                                            ID="btnShowAllegations_MODAL" runat="server" OnClick="btnShowAllegations_MODAL_Click"
                                            CssClass="button" Text="+" />
                                        <act:ModalPopupExtender ID="modalPopUp_Allegations" runat="server" PopupControlID="pnlModal_Allegations"
                                            TargetControlID="hidAllegations_MODAL" BackgroundCssClass="Background" DropShadow="true"
                                            ViewStateMode="Enabled">
                                        </act:ModalPopupExtender>
                                        <asp:HiddenField ID="hidAllegations_MODAL" runat="server" />
                                        <asp:Panel ID="pnlModal_Allegations" runat="server" CssClass="Popup" align="center"
                                            Style="display: none">
                                            <div id="divAllegations_MODAL" runat="server" style="width: 100%">
                                                <table width="100%" style="border: 1px solid #000000; background-color: #808080">
                                                    <tr>
                                                        <td class="tdTextReight">
                                                            <span class="lblCaption">Allegations :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAllegations_MODAL" runat="server" Width="500" Height="250px"
                                                                TextMode="MultiLine" CssClass="txtDefault"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:Button ID="btnCloseMODAL_Allegations" runat="server" Text="Close" CssClass="btnDefault"
                                                    OnClick="btnCloseMODAL_Allegations_Click" />
                                            </div>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Case No :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCaseNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">RY Sent :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRYSent" runat="server" CssClass="txtDate"></asp:TextBox>
                                        <act:MaskedEditExtender ID="meeRYSent" runat="server" TargetControlID="txtRYSent"
                                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                        </act:MaskedEditExtender>
                                        <act:CalendarExtender ID="ceRYSent" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                            TargetControlID="txtRYSent" PopupButtonID="imgRYSent" CssClass="cal_Theme1">
                                        </act:CalendarExtender>
                                        <asp:ImageButton ID="imgRYSent" runat="server" AlternateText="Please Select date!!"
                                            ImageUrl="~/images/calendar.png" />
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Present Posting :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPresentPosting" runat="server" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Zone :</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlZone" runat="server" CssClass="ddlDefault1">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Source :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSource" runat="server" CssClass="txtNO"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Source Date :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSourceDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                        <act:MaskedEditExtender ID="meeSourceDate" runat="server" TargetControlID="txtSourceDate"
                                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="" Enabled="True">
                                        </act:MaskedEditExtender>
                                        <act:CalendarExtender ID="ceSourceDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                            TargetControlID="txtSourceDate" PopupButtonID="imgSourceDate" CssClass="cal_Theme1">
                                        </act:CalendarExtender>
                                        <asp:ImageButton ID="imgSourceDate" runat="server" AlternateText="Please Select date!!"
                                            ImageUrl="~/images/calendar.png" />
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Source Ref :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSourceReference" runat="server" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">A/c Name :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAccountName" runat="server" CssClass="txtDefault"></asp:TextBox>&nbsp;<asp:Button
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
                                        <span class="lblCaption">Sent for Inv Date :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSentForInvDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                        <act:MaskedEditExtender ID="meeSentForInvDate" runat="server" TargetControlID="txtSentForInvDate"
                                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="" Enabled="True">
                                        </act:MaskedEditExtender>
                                        <act:CalendarExtender ID="ceSentForInvDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                            TargetControlID="txtSentForInvDate" PopupButtonID="imgSentForInvDate" CssClass="cal_Theme1">
                                        </act:CalendarExtender>
                                        <asp:ImageButton ID="imgSentForInvDate" runat="server" AlternateText="Please Select date!!"
                                            ImageUrl="~/images/calendar.png" />
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Sent To :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSentTo" runat="server" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Region :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRegion" runat="server" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Close :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClose" runat="server" CssClass="txtNO"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Date for Inv Report :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateForINVReport" runat="server" CssClass="txtDate"></asp:TextBox>
                                        <act:MaskedEditExtender ID="meeDateForINVReport" runat="server" TargetControlID="txtDateForINVReport"
                                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="" Enabled="True">
                                        </act:MaskedEditExtender>
                                        <act:CalendarExtender ID="ceDateForINVReport" runat="server" Format="dd/MM/yyyy"
                                            Enabled="True" TargetControlID="txtDateForINVReport" PopupButtonID="imgDateForINVReport"
                                            CssClass="cal_Theme1">
                                        </act:CalendarExtender>
                                        <asp:ImageButton ID="imgDateForINVReport" runat="server" AlternateText="Please Select date!!"
                                            ImageUrl="~/images/calendar.png" />
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Designation :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDesignation" runat="server" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Register :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRegister" runat="server" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Status Code :</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatusCode" runat="server" CssClass="ddlDefaultStatusCode">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblStatusCodeMIS" runat="server"></asp:Label>
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
                                    <td class="tdTextReight" style="width: 9%">
                                        <span class="lblCaption">Status :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStatus" runat="server" CssClass="txtDefault" Width="95%" TextMode="MultiLine"
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
                                    <asp:Panel ID="pnlHOStatus" runat="server" Visible="false">
                                        <td class="tdTextReight" style="width: 8.6%">
                                            <span class="lblCaption">HO Status :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHOStatus" runat="server" CssClass="txtDefault" Width="95%" TextMode="MultiLine"
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
                                            Visible="false" />&nbsp;&nbsp;
                                        <asp:Button ID="btnDelete" runat="server" CssClass="btnDefault" Text="Delete" OnClick="btnDelete_Click"
                                            Visible="false" />
                                        &nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnDefault"
                                            OnClick="btnCancel_Click" />&nbsp;&nbsp;<asp:Label ID="lblMsg" runat="server" CssClass="lblMsg"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="tabList" runat="server" HeaderText="List">
                        <HeaderTemplate>
                            <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                Font-Size="Small" Text="List" ToolTip="List of Whistle Blower Entry"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">R No. :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRNo_LIST" runat="server" Width="100px" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Branch :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBranch_LIST" runat="server" Width="200px" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Circle :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCircle_LIST" runat="server" Width="200px" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">A/c Name :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAccountName_LIST" runat="server" Width="200px" CssClass="txtDefault"></asp:TextBox>
                                        &nbsp;<asp:ImageButton ID="imgSearch_List" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                            Width="25px" Height="30px" OnClick="imgSearch_LIST_Click" ToolTip="Whistle Blower Search" />
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
                                                BackColor="White" BorderColor="White" BorderWidth="1px" CellSpacing="1" Width="100%"
                                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvMain_PageIndexChanging"
                                                OnSorting="gvMain_Sorting" OnRowCommand="gvMain_RowCommand" OnRowDataBound="gvMain_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText">
                                                        <HeaderTemplate>
                                                            Select
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnView" runat="server" CausesValidation="false" CommandName="View"
                                                                ToolTip='<%# Eval("CODE") %>' ImageUrl="~/images/selg_16.png" Height="20px" Width="18px"
                                                                CommandArgument='<%# Eval("CODE")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="RNO" HeaderText="R No." SortExpression="RNO" ItemStyle-CssClass="gridText"
                                                        HeaderStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="COMPRECDATE" HeaderText="Comp Rec Date" SortExpression="COMPRECDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="BRCOMPLAINT" HeaderText="BR Complaint" SortExpression="BRCOMPLAINT"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" SortExpression="CIRCLEOFFICE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="COMPNO" HeaderText="Comp No" SortExpression="COMPNO" ItemStyle-CssClass="gridText"
                                                        HeaderStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CLOSUREDATE" HeaderText="Closure Date" SortExpression="CLOSUREDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="ACCUSED" HeaderText="Accused" SortExpression="ACCUSED"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="ALLEGATIONS" HeaderText="Allegations" SortExpression="ALLEGATIONS"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CASENO" HeaderText="Case No" SortExpression="CASENO" ItemStyle-CssClass="gridText"
                                                        HeaderStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RYSENTDATE" HeaderText="RY Sent" SortExpression="RYSENTDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="PRESENTPOSTING" HeaderText=" Present Posting" SortExpression="PRESENTPOSTING"
                                                        ItemStyle-CssClass="gridText" HeaderStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="ZONE" HeaderText="Zone" SortExpression="ZONE" ItemStyle-CssClass="gridText"
                                                        HeaderStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SOURCE" HeaderText="Source" SortExpression="SOURCE" ItemStyle-CssClass="gridText"
                                                        HeaderStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SOURCEDATE" HeaderText="Source Date" SortExpression="SOURCEDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SOURCEREF" HeaderText="Source Ref" SortExpression="SOURCEREF"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="ACCOUNTNAME" HeaderText="Account Name" SortExpression="ACCOUNTNAME"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="AMOUNT" HeaderText="Amount" SortExpression="AMOUNT" ItemStyle-CssClass="gridText"
                                                        HeaderStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SENTFORINVDATE" HeaderText="Sent for Inv Date" SortExpression="SENTFORINVDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SENTTO" HeaderText="Sent To" SortExpression="SENTTO" ItemStyle-CssClass="gridText"
                                                        HeaderStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="REGION" HeaderText="Region" SortExpression="REGION" ItemStyle-CssClass="gridText"
                                                        HeaderStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CASECLOSE" HeaderText="Close" SortExpression="CASECLOSE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="INVREPORTDATE" HeaderText="Date for Inv Report" SortExpression="INVREPORTDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DESIGNATION" HeaderText="Designation" SortExpression="DESIGNATION"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="REGISTER" HeaderText="Register" SortExpression="REGISTER"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" SortExpression="STATUSCODE"
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
                <asp:HiddenField ID="hidZone" runat="server" />
                <asp:HiddenField ID="hidStatusCode" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
