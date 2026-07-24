<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmSRStructure.aspx.cs" Inherits="VMISP.Mis.frmSRStructure" %>

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
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F; color: White; font-weight: 700;">
                SR
            </div>
            <asp:Panel ID="pnlMain" runat="server" Width="100%" UpdateMode="Conditional" EnableViewState="true">
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
                                Font-Size="Small" Text="Entry" ToolTip="SR Entry"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="background-color: #FFE4E1">
                                <tr>
                                    <td class="tdTextReight">
                                        <asp:Label ID="lblSRNoRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">SR No. :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSRNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        &nbsp;<asp:ImageButton ID="imgGet" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                            Width="25px" Height="30px" OnClick="btnGet_Click" ToolTip="SR Search" />
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">SR Date :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSRDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                        <act:MaskedEditExtender ID="meeSRDate" runat="server" TargetControlID="txtSRDate"
                                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="" Enabled="True">
                                        </act:MaskedEditExtender>
                                        <act:CalendarExtender ID="ceSRDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                            TargetControlID="txtSRDate" PopupButtonID="imgSRDate" CssClass="cal_Theme1">
                                        </act:CalendarExtender>
                                        <asp:ImageButton ID="imgSRDate" runat="server" AlternateText="Please Select date!!"
                                            ImageUrl="~/images/calendar.png" />
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Branch :</span>
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
                                        <span class="lblCaption">R No :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRNo" runat="server" CssClass="txtNO"></asp:TextBox>
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
                                        <span class="lblCaption">Amount :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="txtNO" Style="text-align: right"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Rec Date :</span>
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
                                        <span class="lblCaption">Final Action :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFinalAction" runat="server" CssClass="txtDefault"></asp:TextBox>
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
                                        <span class="lblCaption">Status Code :</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatusCode" runat="server" CssClass="ddlDefaultStatusCode">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblStatusCodeMIS" runat="server"></asp:Label>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Region :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRegion" runat="server" CssClass="txtDate"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Present Posting :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPresentPosting" runat="server" CssClass="txtDefault"></asp:TextBox>
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
                                        <span class="lblCaption">Close :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClose" runat="server" CssClass="txtNO"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Nature :</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlNature" runat="server" CssClass="ddlDefault" Width="100px">
                                        </asp:DropDownList>
                                        &nbsp;
                                        <asp:Panel ID="pnlNatureMIS" runat="server" Visible="false">
                                            <asp:Label ID="lblNatureMIS" runat="server"></asp:Label>
                                        </asp:Panel>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Designation :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDesignation" runat="server" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Investigation :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInvestigation" runat="server" CssClass="txtDefault"></asp:TextBox>
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
                                <tr>
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
                            </table>
                            <table width="100%" style="background-color: #FFE4E1">
                                <tr>
                                    <td class="tdTextReight" style="width: 8.6%"></td>
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
                                    <td class="tdTextReight" style="width: 9.5%">
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
                                Font-Size="Small" Text="List" ToolTip="List of SR Entry"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="55%" style="margin-top: -10px; border-bottom: 1px solid;">
                                <tr>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">SR No. :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRNo_LIST" runat="server" Width="100px" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Circle Office :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCircleOffice_LIST" runat="server" Width="150px" CssClass="txtDefault"></asp:TextBox>
                                        &nbsp;<asp:ImageButton ID="imgSearch_List" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                            Width="25px" Height="30px" OnClick="imgSearch_LIST_Click" ToolTip="SR Search" />
                                        &nbsp;&nbsp;<asp:Label ID="lblList" runat="server" CssClass="lblMsg"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Both" Height="350px" Width="100%">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100%">
                                            <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" DataKeyNames="SRNO"
                                                CellPadding="3" GridLines="None" ViewStateMode="Enabled" Style="margin-top: 0px"
                                                BackColor="White" BorderColor="White" BorderWidth="2px" CellSpacing="1" Width="100%"
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
                                                    <asp:BoundField DataField="SRNO" HeaderText="SR No." SortExpression="SRNO" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SRDATE" HeaderText="SR Date" SortExpression="SRDATE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="BRCOMPLAINT" HeaderText="BR Complaint" SortExpression="BRCOMPLAINT"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" SortExpression="CIRCLEOFFICE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RNO" HeaderText="R No" SortExpression="RNO" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CLOSUREDATE" HeaderText="Closure Date" SortExpression="CLOSUREDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="ACCUSED" HeaderText="Accused" SortExpression="ACCUSED"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="ALLEGATIONS" HeaderText="Allegations" SortExpression="ALLEGATIONS"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="AMOUNT" HeaderText="Amount" SortExpression="AMOUNT" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="COMPRECDATE" HeaderText="Comp Rec Date" SortExpression="COMPRECDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="FINALACTION" HeaderText="Final Action" SortExpression="FINALACTION"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="ZONE" HeaderText="Zone" SortExpression="ZONE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" SortExpression="STATUSCODE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="REGION" HeaderText="Region" SortExpression="REGION" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="PRESENTPOSTING" HeaderText="Present Posting" SortExpression="PRESENTPOSTING"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="ACCOUNTNAME" HeaderText="Account Name" SortExpression="ACCOUNTNAME"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CASECLOSE" HeaderText="Close" SortExpression="CASECLOSE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NATURE" HeaderText="Nature" SortExpression="NATURE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NATURECODE" HeaderText="Nature Code" SortExpression="NATURECODE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DESIGNATION" HeaderText="Designation" SortExpression="DESIGNATION"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="INVESTIGATION" HeaderText="Investigation" SortExpression="INVESTIGATION"
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
                <asp:HiddenField ID="hidNature" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
