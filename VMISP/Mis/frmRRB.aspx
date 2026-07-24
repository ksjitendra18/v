<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="frmRRB.aspx.cs" Inherits="VMISP.Mis.frmRRB" %>

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
                RRB
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
                                Font-Size="Small" Text="Entry" ToolTip="RRB Entry"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="pnlControls" runat="server" ScrollBars="Both" Height="350px">
                                <table width="100%" style="background-color: #FFE4E1">
                                    <tr>
                                        <td class="tdTextReight">
                                            <asp:Label ID="lblRNumber" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">R No. :</span>
                                        </td>
                                        <td style="width: 110px">
                                            <asp:TextBox ID="txtRNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                            <asp:ImageButton ID="imgGet" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                                Width="25px" Height="30px" OnClick="btnGet_Click" ToolTip="RRB Search" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">RNO 1 :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRNo1" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight" style="width: 130px">
                                            <span class="lblCaption">Name Particulars :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNameOfParticulars" runat="server" CssClass="txtDefault"></asp:TextBox>&nbsp;<asp:Button
                                                ID="btnShowNameOfParticulars_MODAL" runat="server" OnClick="btnShowNameOfParticulars_MODAL_Click"
                                                CssClass="button" Text="+" />
                                            <act:ModalPopupExtender ID="modalPopUp_NameOfParticulars" runat="server" PopupControlID="pnlModal_NameOfParticulars"
                                                TargetControlID="hidNameOfParticulars_MODAL" BackgroundCssClass="Background" DropShadow="true"
                                                ViewStateMode="Enabled">
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
                                            <span class="lblCaption">Name :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Date Ch :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChargeDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeChargeDate" runat="server" TargetControlID="txtChargeDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
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
                                            <asp:TextBox ID="txtNatCHSheet" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Status Code :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatusCode" runat="server" CssClass="ddlDefault1">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblStatusCodeMIS" runat="server"></asp:Label>
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
                                            <span class="lblCaption">Final :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlFinal" runat="server" CssClass="txtNO">
                                                <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Scale :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlScale" runat="server" CssClass="ddlDefault1">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">PF No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPFNo" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Dt RNo :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRNoDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeRNoDate" runat="server" TargetControlID="txtRNoDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceRNoDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtRNoDate" PopupButtonID="imgRNoDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgRNoDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight" style="width: 90px;">
                                            <span class="lblCaption">Retirement Dt :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRetirementDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeRetirementDate" runat="server" TargetControlID="txtRetirementDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceRetirementDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtRetirementDate" PopupButtonID="imgRetirementDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgRetirementDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">DT ORD DA :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDAOrdDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeDAOrdDate" runat="server" TargetControlID="txtDAOrdDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceDAOrdDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtDAOrdDate" PopupButtonID="imgDAOrdDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgDAOrdDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Na Pun DA :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNaPunDa" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Branch :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <asp:Label ID="lblCircleOffice" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">Circle Office :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCircleOffice" runat="server" CssClass="ddlDefault1">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdTextReight" style="width: 90px;">
                                            <span class="lblCaption">Disp Authority :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDispAuthority" runat="server" CssClass="ddlDefault1">
                                                <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                                <asp:ListItem Text="Chairman" Value="Chairman"></asp:ListItem>
                                                <asp:ListItem Text="General Manager" Value="General Manager"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Dis Authority Zone :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDisAuthorityZone" runat="server" CssClass="ddlDefault1">
                                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="HIMANCHAL GRAMIN BANK" Value="HGB"></asp:ListItem>
                                                <asp:ListItem Text="MADHYA BIHAR GRAMIN BANK" Value="MBGB"></asp:ListItem>
                                                <asp:ListItem Text="PUNJAB GRAMIN BANK" Value="PGB"></asp:ListItem>
                                                <asp:ListItem Text="SARVA HARYANA GRAMIN BANK" Value="SHGB"></asp:ListItem>
                                                <asp:ListItem Text="SARVA UP GRAMIN BANK" Value="SUGB"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Lapse Nature :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLapseNature" runat="server" CssClass="txtDefault"></asp:TextBox>&nbsp;<asp:Button
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
                                                                <asp:TextBox ID="txtLapseNature_MODAL" runat="server" Width="500" Height="250px"
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
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVC OM No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVCOMNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">2nd CVC :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVCAdbiceII" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight" style="width: 106px;">
                                            <span class="lblCaption">CVC Proposed Action :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProposedActiontoCVC" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVC 2 Ref :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVC2Ref" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeCVC2Ref" runat="server" TargetControlID="txtCVC2Ref"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceCVC2Ref" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtCVC2Ref" PopupButtonID="imgCVC2Ref" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgCVC2Ref" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Ist DA Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIstDaDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeIstDaDate" runat="server" TargetControlID="txtIstDaDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceIstDaDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtIstDaDate" PopupButtonID="imgIstDaDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgIstDaDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">2nd DA Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt2ndDADate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="mee2ndDADate" runat="server" TargetControlID="txt2ndDADate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ce2ndDADate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txt2ndDADate" PopupButtonID="img2ndDADate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="img2ndDADate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">DA Proposal :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDAProposal" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight" style="width: 100px">
                                            <span class="lblCaption">2DA Proposal :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt2DAProposal" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight" style="width: 105px">
                                            <span class="lblCaption">CVO 1st Adv Dt :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVOAdviceDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeCVOAdviceDate" runat="server" TargetControlID="txtCVOAdviceDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceCVOAdviceDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtCVOAdviceDate" PopupButtonID="imgCVOAdviceDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgCVOAdviceDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVO 1st Adv :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVOAdvice" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVO 2 Adv Dt :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVO2AdviceDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeCVO2AdviceDate" runat="server" TargetControlID="txtCVO2AdviceDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceCVO2AdviceDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtCVO2AdviceDate" PopupButtonID="imgCVO2AdviceDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgCVO2AdviceDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight" style="width: 90px">
                                            <span class="lblCaption">CVO 2 Advice :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVO2Advice" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">App PO Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAppPODate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeAppPODate" runat="server" TargetControlID="txtAppPODate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
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
                                            <asp:TextBox ID="txtPOName" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">App EO Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAppEODate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeAppEODate" runat="server" TargetControlID="txtAppEODate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceAppEODate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtAppEODate" PopupButtonID="imgAppEODate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgAppEODate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">EO Name :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEOName" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
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
                                            <span class="lblCaption">CBI RC NO1 :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCbiRcNo1" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">RC1 Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRC1Date" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeRC1Date" runat="server" TargetControlID="txtRC1Date"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceRC1Date" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtRC1Date" PopupButtonID="imgRC1Date" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgRC1Date" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight" style="display:none;">
                                            <span class="lblCaption">Refer To CVC :</span>
                                        </td>
                                        <td style="display:none;">
                                            <asp:TextBox ID="txtReferToCVCDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeReferToCVCDate" runat="server" TargetControlID="txtReferToCVCDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceReferToCVCDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtReferToCVCDate" PopupButtonID="imgReferToCVCDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgReferToCVCDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                    </tr>
                                    <tr>
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
                                            <act:MaskedEditExtender ID="meeRC2Date" runat="server" TargetControlID="txtRC2Date"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceRC2Date" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtRC2Date" PopupButtonID="imgRC2Date" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgRC2Date" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight" style="width: 85px; display: none;">
                                            <span class="lblCaption">OM CVC Dt :</span>
                                        </td>
                                        <td style="display: none;">
                                            <asp:TextBox ID="txtOMCVCDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeOMCVCDate" runat="server" TargetControlID="txtOMCVCDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceOMCVCDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtOMCVCDate" PopupButtonID="imgOMCVCDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgOMCVCDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Complaint Date :</span>
                                        </td>
                                        <td style="width: 100px;">
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
                                            <span class="lblCaption">Present Posting :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPresentPosting" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Zone :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlZone" runat="server" CssClass="ddlDefault1" DataSourceID="sdsZone"
                                                DataTextField="ZONENAME" DataValueField="ZONEID">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="sdsZone" runat="server" ConnectionString="<%$ ConnectionStrings:dbVIGILANCEMIS %>"
                                                SelectCommand="((SELECT '0' AS [ZONEID],'-Select' AS [ZONENAME]) UNION (SELECT DISTINCT [FGMCODE] AS ZONEID, [FGMNAME] AS ZONENAME FROM [Fgm_Master])) ORDER BY ZONEID"></asp:SqlDataSource>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">EO CDI :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEoCdi" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Designation :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDesignation" runat="server" CssClass="txtDefault"></asp:TextBox>
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
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Amount :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="txtNO" Style="text-align: right"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Ist Pending :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIstPending" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">PD/Susp No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDRefNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Suspension :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSuspensionDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeSuspensionDate" runat="server" TargetControlID="txtSuspensionDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceSuspensionDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtSuspensionDate" PopupButtonID="imgSuspensionDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgSuspensionDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Source :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSource" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">DA Ref No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDARefNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
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
                                        <td class="tdTextReight" style="width: 114px">
                                            <span class="lblCaption">Reasons Inclusion :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReasonsforInclusion" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Investig :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInvestig" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVC Recom :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRecommofCVC" runat="server" CssClass="txtNO"></asp:TextBox>
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
                                            <asp:TextBox ID="txtInvOfficerName" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">U/S :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUS" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">IR-CBI Pending :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIRCBIPending" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Pol Fir No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPolFirNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
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
                                            <span class="lblCaption">RC Source :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRCSource" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">PO CBI :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPOCBI" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVC 2 Proposed :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVC2Proposed" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">DTS Hear :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDTSHear" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight" style="width: 100px">
                                            <span class="lblCaption">Account Nature :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNatureofAccount" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">2nd Pending :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt2ndPending" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
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
                                        <td class="tdTextReight">
                                            <span class="lblCaption">ADV 1 Awaited :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtADV1Awaited" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">NO Award S :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNoAwardS" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Final Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFinalDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeFinalDate" runat="server" TargetControlID="txtFinalDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceFinalDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtFinalDate" PopupButtonID="imgFinalDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgFinalDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Re Comp :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReComp" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Field 1 :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtField1" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Con Enq Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtConEnqDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeConEnqDate" runat="server" TargetControlID="txtConEnqDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceConEnqDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtConEnqDate" PopupButtonID="imgConEnqDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgConEnqDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Reg Invok :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRegInvok" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Basic Pay :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBasicPay" runat="server" CssClass="txtNO"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Basic Pay Date :</span>
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
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CDI Name :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCDIName" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">AIC CVC :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAICCVC" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeAICCVC" runat="server" TargetControlID="txtAICCVC"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceAICCVC" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtAICCVC" PopupButtonID="imgAICCVC" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgAICCVC" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                    </tr>
                                    <tr style="display: none">
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
                                            <span class="lblCaption">Nature Case :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlNature" runat="server" CssClass="ddlDefault1" DataSourceID="sdsNature"
                                                DataValueField="NATURECODE" DataTextField="NATURECASE">
                                            </asp:DropDownList>
                                            <asp:Panel ID="pnlNatureMIS" runat="server" Visible="false">
                                                <asp:Label ID="lblNatureMIS" runat="server"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Lodi New :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLodiNew" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">AIE CVC :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAIECVC" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeAIECVC" runat="server" TargetControlID="txtAIECVC"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceAIECVC" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtAIECVC" PopupButtonID="imgAIECVC" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgAIECVC" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Lodi Case :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLodiCase" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Rec CVC 2 :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRecCVC2" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeRecCVC2" runat="server" TargetControlID="txtRecCVC2"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceRecCVC2" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtRecCVC2" PopupButtonID="imgRecCVC2" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgRecCVC2" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
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
                                        <td class="tdTextReight">
                                            <span class="lblCaption">State :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtState" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
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
                                        <td>
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
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Commitment :</span>
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
                                            <asp:TextBox ID="txtAdv2Awt" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Penalty :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPenalty" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
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
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Status in Brief :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStatusinBrief" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Add Mod :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAddMod" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">No CAT A :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCatNoA" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Revocation Date :</span>
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
                                            <span class="lblCaption">No CAT B :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCatNoB" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Review Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReviewDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeReviewDate" runat="server" TargetControlID="txtReviewDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceReviewDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtReviewDate" PopupButtonID="imgReviewDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgReviewDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CBI Recom :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCBIRecom" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CBI Zone :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCBIZone" runat="server" CssClass="ddlDefault1" DataSourceID="sdsZone"
                                                DataTextField="ZONENAME" DataValueField="ZONEID">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Bank IR Awaited :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankIRAwaited" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Lodi No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLodiNo" runat="server" CssClass="txtDefault"></asp:TextBox>
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
                                            <span class="lblCaption">CVO 1 Diff :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVO1Diff" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVO 2 Diff :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVO2Diff" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">ERCO Date :</span>
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
                                            <span class="lblCaption">Written Brief PO :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtWrittenBriefPO" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeWrittenBriefPO" runat="server" TargetControlID="txtWrittenBriefPO"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceWrittenBriefPO" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtWrittenBriefPO" PopupButtonID="imgWrittenBriefPO" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgWrittenBriefPO" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">No Of Emp :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNoOfEmp" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">A2 CVC :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtA2CVC" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeA2CVC" runat="server" TargetControlID="txtA2CVC" Mask="99/99/9999"
                                                MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceA2CVC" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtA2CVC" PopupButtonID="imgA2CVC" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgA2CVC" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                    </tr>
                                    <tr style="display: none">
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
                                            <span class="lblCaption">Sanction Order :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSanctionOrderDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeSanctionOrderDate" runat="server" TargetControlID="txtSanctionOrderDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceSanctionOrderDate" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtSanctionOrderDate" PopupButtonID="imgSanctionOrderDate"
                                                CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgSanctionOrderDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVC 1 Diff :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVC1Diff" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">CVC 2 Diff :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCVC2Diff" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Enter Reg-1 :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEnterReg1Date" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeEnterReg1Date" runat="server" TargetControlID="txtEnterReg1Date"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceEnterReg1Date" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtEnterReg1Date" PopupButtonID="imgEnterReg1Date" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgEnterReg1Date" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Enter Reg-2 :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEnterReg2Date" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeEnterReg2Date" runat="server" TargetControlID="txtEnterReg2Date"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceEnterReg2Date" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtEnterReg2Date" PopupButtonID="imgEnterReg2Date" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgEnterReg2Date" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Enter Reg-3 :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEnterReg3Date" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeEnterRegDate" runat="server" TargetControlID="txtEnterReg3Date"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceEnterReg3Date" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtEnterReg3Date" PopupButtonID="imgEnterReg3Date" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgEnterReg3Date" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">IAC Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIACDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeIACDate" runat="server" TargetControlID="txtIACDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceIACDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtIACDate" PopupButtonID="imgIACDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgIACDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdTextReight" style="display: none">
                                            <span class="lblCaption">Complaint Date :</span>
                                        </td>
                                        <td style="display: none">
                                            <asp:TextBox ID="txtComplaintDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeComplaintDate" runat="server" TargetControlID="txtComplaintDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceComplaintDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtComplaintDate" PopupButtonID="imgComplaintDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgComplaintDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Sub Rep Date :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSubRepDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeSubRepDate" runat="server" TargetControlID="txtSubRepDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="ceSubRepDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                TargetControlID="txtSubRepDate" PopupButtonID="imgSubRepDate" CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgSubRepDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">ZO Commitment :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtZOCommitment" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" style="display: none">
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Authority Circle :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDisAuthoritysCircle" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Case of Non-Consultation :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCaseofNonConsultation" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Whether CVO Differs :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtWhetherCVODiffers" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Deletion Reasons :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDeletionReasons" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">DA Proposed Punishment :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPunishmentProposedbyDA" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Prev Case/Punishments :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPrevCasePunishment" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Connected/Vig Case :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtConnectedVigCase" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Police Report Awaited :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPoliceReportAwaited" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Place in Present Scale :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPlaceinPresentScaleDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meePlaceinPresentScaleDate" runat="server" TargetControlID="txtPlaceinPresentScaleDate"
                                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                            </act:MaskedEditExtender>
                                            <act:CalendarExtender ID="cePlaceinPresentScaleDate" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtPlaceinPresentScaleDate" PopupButtonID="imgPlaceinPresentScaleDate"
                                                CssClass="cal_Theme1">
                                            </act:CalendarExtender>
                                            <asp:ImageButton ID="imgPlaceinPresentScaleDate" runat="server" AlternateText="Please Select date!!"
                                                ImageUrl="~/images/calendar.png" />
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Adjournment Reasons :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAdjournmentReasons" runat="server" CssClass="txtDefault"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" style="background-color: #FFE4E1">
                                    <tr>
                                        <td class="tdTextReight" style="width: 10.5%">
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
                                                <asp:TextBox ID="txtHOStatus" runat="server" CssClass="txtDefault" Width="95%" Height="35px"></asp:TextBox>
                                            </td>
                                        </asp:Panel>
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
                            </asp:Panel>
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="tabList" runat="server" HeaderText="List">
                        <HeaderTemplate>
                            <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                Font-Size="Small" Text="List" ToolTip="List of RRB Entry"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="65%" style="margin-top: -10px; border-bottom: 1px solid;">
                                <tr>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">R No. :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRNo_LIST" runat="server" Width="85px" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Name :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName_LIST" runat="server" Width="125px" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Zone :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZone_LIST" runat="server" Width="125px" CssClass="txtDefault"></asp:TextBox>
                                        &nbsp;<asp:ImageButton ID="imgSearch_List" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                            Width="25px" Height="30px" OnClick="imgSearch_LIST_Click" ToolTip="RRB Search" />
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
                                                    <asp:BoundField DataField="RNO" HeaderText="R No." SortExpression="RNO" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RNO1" HeaderText="R NO 1" SortExpression="RNO1" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NAMEOFPARTICULARS" HeaderText="Name Particulars" SortExpression="NAMEOFPARTICULARS"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NAME" HeaderText="Name" SortExpression="NAME" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NATCHSHEET" HeaderText="Nat CH Sheet" SortExpression="NATCHSHEET"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CHARGEDATE" HeaderText="Date Ch" SortExpression="CHARGEDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" SortExpression="STATUSCODE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="REGISTER" HeaderText="Register" SortExpression="REGISTER"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="FINAL" HeaderText="Final" SortExpression="FINAL" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SCALE" HeaderText="Scale" SortExpression="SCALE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="PFNO" HeaderText="PF Number" SortExpression="PFNO" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RNODATE" HeaderText="R No Date" SortExpression="RNODATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RETIREMENTDATE" HeaderText="Retirement Dt" SortExpression="RETIREMENTDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DAORDDATE" HeaderText="DT ORD DA" SortExpression="DAORDDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NAPUNDA" HeaderText="Na Pun DA" SortExpression="NAPUNDA"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="BRCOMPLAINT" HeaderText="Branch" SortExpression="BRCOMPLAINT"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" SortExpression="CIRCLEOFFICE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DISPAUTHORITY" HeaderText="Disp Authority" SortExpression="DISPAUTHORITY"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DISAUTHORITYSZONE" HeaderText="Dis Authority Zone" SortExpression="DISAUTHORITYSZONE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="OMCVCDATE" HeaderText="OM CVC Dt" SortExpression="OMCVCDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVCOMNO" HeaderText="CVC OM No" SortExpression="CVCOMNO"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVCADVICEII" HeaderText="2nd CVC" SortExpression="CVCADVICEII"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="PROPOSEDACTIONTOCVC" HeaderText="CVC Proposed Action"
                                                        SortExpression="PROPOSEDACTIONTOCVC" HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVC2REF" HeaderText="CVC 2 Ref Date" SortExpression="CVC2REF"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="ISTDADATE" HeaderText="Ist DA Date" SortExpression="ISTDADATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DA2NDDATE" HeaderText="2nd DA Date" SortExpression="DA2NDDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DAPROPOSAL" HeaderText="DA Proposal" SortExpression="DAPROPOSAL"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DAPROPOSAL2" HeaderText="2DA Proposal" SortExpression="DAPROPOSAL2"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVOADVICEDATE" HeaderText="CVO 1st Adv Dt" SortExpression="CVOADVICEDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVOADVICE" HeaderText="CVO 1st Adv" SortExpression="CVOADVICE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVO2ADVICEDATE" HeaderText="CVO 2 Adv Dt" SortExpression="CVO2ADVICEDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CVO2ADVICE" HeaderText="CVO 2 Advice" SortExpression="CVO2ADVICE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="APPPODATE" HeaderText="App PO Date" SortExpression="APPPODATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="PONAME" HeaderText="PO Name" SortExpression="PONAME" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="APPEODATE" HeaderText="App EO Date" SortExpression="APPEODATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="EONAME" HeaderText="EO Name" SortExpression="EONAME" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="REFERTOCVCDATE" HeaderText="Refer to CVC Date" SortExpression="REFERTOCVCDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="LAPSENATURE" HeaderText="Lapse Nature" SortExpression="LAPSENATURE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CBIRCNO1" HeaderText="CBI RC No 1" SortExpression="CBIRCNO1"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RC1DATE" HeaderText="RC 1 Date" SortExpression="RC1DATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CBIRCNO2" HeaderText="CBI RC No 2" SortExpression="CBIRCNO2"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RC2DATE" HeaderText="RC 2 Date" SortExpression="RC2DATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CLOSUREDATE" HeaderText="RC 2 Date" SortExpression="CLOSUREDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="IACDATE" HeaderText="Regu Enq Date" SortExpression="IACDATE"
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
                <asp:HiddenField ID="hidNatureCase" runat="server" />
                <asp:HiddenField ID="hidScale" runat="server" />
                <asp:HiddenField ID="hidDisAuthorityZone" runat="server" />
                <asp:HiddenField ID="hidCBIZone" runat="server" />
                <asp:HiddenField ID="hidDispAuthority" runat="server" />
                <asp:HiddenField ID="hidFinal" runat="server" />
                <asp:SqlDataSource ID="sdsNature" runat="server" ConnectionString="<%$ ConnectionStrings:dbVIGILANCEMIS %>"
                    SelectCommand="((SELECT '0' AS [NATURECODE],'-Select' AS [NATURECASE]) UNION (SELECT [CODE] AS NATURECODE, [NATURECASE] AS NATURECASE FROM [NATURECASE] WHERE ACTIVE='Y' AND FORTABLE='RRB')) ORDER BY NATURECODE"></asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
