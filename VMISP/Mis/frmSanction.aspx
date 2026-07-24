<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmSanction.aspx.cs" Inherits="VMISP.Mis.frmSanction" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/ssMain.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="../Js/JS_CommonValidation.js" type="text/javascript"></script>
    <style type="text/css">
        .button
        {
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
        
        .button:hover
        {
            background-color: #3e8e41;
        }
        
        .button:active
        {
            background-color: #3e8e41;
            box-shadow: 0 5px #666;
            transform: translateY(4px);
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F;
                color: White; font-weight: 700;">
                Sanction for Prosecution
            </div>
            <asp:Panel ID="pnlMain" runat="server" Width="100%">
                <div>
                    <div style="float: right; background-color: #FAEBD7;">
                        <asp:Panel ID="pnlHeader" runat="server" Visible="false">
                            <span class="lblCaptionHead" style="font-size: small; font-weight: bold">Entry By :</span>
                            <asp:Label ID="lblEntryBy" runat="server" Width="50px" ForeColor="#FF3300" Font-Size="small"
                                Font-Bold="True"></asp:Label>&nbsp<span class="lblCaptionHead" style="font-size: small;
                                    font-weight: bold">Entry Date :</span>
                            <asp:Label ID="lblEntryDate" runat="server" Width="75px" ForeColor="#FF3300" Font-Size="small"
                                Font-Bold="True"></asp:Label>&nbsp<span class="lblCaptionHead" style="font-size: small;
                                    font-weight: bold">Modify By :</span>
                            <asp:Label ID="lblModifyBy" runat="server" Width="50px" ForeColor="#FF3300" Font-Size="small"
                                Font-Bold="True"></asp:Label>&nbsp<span class="lblCaptionHead" style="font-size: small;
                                    font-weight: bold">Modify Date :</span>
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
                                Font-Size="Small" Text="Entry" ToolTip="Sanction for Prosecution Entry"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="background-color: #FFE4E1">
                                <tr>
                                    <td class="tdTextReight">
                                        <asp:Label ID="lblRCNoRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span
                                            class="lblCaption">RC No :</span>
                                    </td>
                                    <td style="width: 110px">
                                        <asp:TextBox ID="txtRCNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        <asp:ImageButton ID="imgGet" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                            Width="25px" Height="30px" ToolTip="Sanction for Prosecution Search" OnClick="btnGet_Click" />
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">RC Date :</span>
                                    </td>
                                    <td style="width: 105px">
                                        <asp:TextBox ID="txtRCDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                        <act:MaskedEditExtender ID="meeRCDate" runat="server" TargetControlID="txtRCDate"
                                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="" Enabled="True">
                                        </act:MaskedEditExtender>
                                        <act:CalendarExtender ID="ceRCDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                            TargetControlID="txtRCDate" PopupButtonID="imgRCDate" CssClass="cal_Theme1">
                                        </act:CalendarExtender>
                                        <asp:ImageButton ID="imgRCDate" runat="server" AlternateText="Please Select date!!"
                                            ImageUrl="~/images/calendar.png" />
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Name :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="txtDefault" Width="250"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">PF Number :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPFNumber" runat="server" CssClass="txtDefault" Width="115px"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Dt of Report Recv. :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRecvDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                        <act:MaskedEditExtender ID="meeRecvDate" runat="server" TargetControlID="txtRecvDate"
                                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="" Enabled="True">
                                        </act:MaskedEditExtender>
                                        <act:CalendarExtender ID="ceRecvDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                            TargetControlID="txtRecvDate" PopupButtonID="imgRecvDate" CssClass="cal_Theme1">
                                        </act:CalendarExtender>
                                        <asp:ImageButton ID="imgRecvDate" runat="server" AlternateText="Please Select date!!"
                                            ImageUrl="~/images/calendar.png" />
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Designation :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDesignation" runat="server" CssClass="txtDefault" width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Sanction for Prosecution :</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSanctionforProsecution" runat="server" CssClass="ddlDefault1">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="YES"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="NO"></asp:ListItem>
                                            <asp:ListItem Text="Under Process" Value="UNDERPROCESS"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Sanction/Refused Date :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSanctionRefusedDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                        <act:MaskedEditExtender ID="meeSanctionRefusedDate" runat="server" TargetControlID="txtSanctionRefusedDate"
                                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="" Enabled="True">
                                        </act:MaskedEditExtender>
                                        <act:CalendarExtender ID="ceSanctionRefusedDate" runat="server" Format="dd/MM/yyyy"
                                            Enabled="True" TargetControlID="txtSanctionRefusedDate" PopupButtonID="imgSanctionRefusedDate"
                                            CssClass="cal_Theme1">
                                        </act:CalendarExtender>
                                        <asp:ImageButton ID="imgSanctionRefusedDate" runat="server" AlternateText="Please Select date!!"
                                            ImageUrl="~/images/calendar.png" />
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">CVC Date :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCVCDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                        <act:MaskedEditExtender ID="meeCVCDate" runat="server" TargetControlID="txtCVCDate"
                                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="" Enabled="True">
                                        </act:MaskedEditExtender>
                                        <act:CalendarExtender ID="ceCVCDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                            TargetControlID="txtCVCDate" PopupButtonID="imgCVCDate" CssClass="cal_Theme1">
                                        </act:CalendarExtender>
                                        <asp:ImageButton ID="imgCVCDate" runat="server" AlternateText="Please Select date!!"
                                            ImageUrl="~/images/calendar.png" />
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" style="background-color: #FFE4E1">
                                <tr>
                                    <td class="tdTextReight" style="width: 11.5%">
                                        <span class="lblCaption">Status :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStatus" runat="server" CssClass="txtDefault" Width="95%" TextMode="MultiLine"
                                            Height="35px"></asp:TextBox>
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
                                            <asp:TextBox ID="txtHOStatus" runat="server" CssClass="txtDefault" Width="95%" TextMode="MultiLine"
                                                Height="35px"></asp:TextBox>
                                        </td>
                                    </asp:Panel>
                                </tr>
                            </table>
                            <table width="100%" style="background-color: #FFE4E1">
                                <tr>
                                    <td style="width: 10%">
                                        &nbsp;&nbsp;
                                    </td>
                                    <td style="width: 90%; text-align: center;">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btnDefault" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btnDefault" Visible="False"
                                            OnClick="btnUpdate_Click" />&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnDefault" OnClick="btnCancel_Click" />&nbsp;&nbsp;<asp:Label
                                            ID="lblMsg" runat="server" CssClass="lblMsg"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="tabList" runat="server" HeaderText="List">
                        <HeaderTemplate>
                            <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                Font-Size="Small" Text="List" ToolTip="List of RTI Entry"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="margin-top: -10px;">
                                <table width="50%" style="margin-top: 0px;">
                                    <tr>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">RTI No :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRCNo_LIST" runat="server" CssClass="txtDefault" Width="90px"></asp:TextBox>
                                        </td>
                                        <td class="tdTextReight">
                                            <span class="lblCaption">Status :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStatus_LIST" runat="server" CssClass="txtDefault" Width="250px"></asp:TextBox>
                                            &nbsp;<asp:ImageButton ID="imgSearch_List" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                                Width="25px" Height="30px" OnClick="imgSearch_LIST_Click" ToolTip="Sanction Search" />
                                            &nbsp;&nbsp;
                                        </td>
                                </table>
                                <table width="100%" style="border-bottom: 1px solid;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblList" runat="server" CssClass="lblMsg"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="pnlList" runat="server" ScrollBars="Both" Height="350px" Width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 100%">
                                                <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" DataKeyNames="RCNO"
                                                    CellPadding="3" GridLines="None" ViewStateMode="Enabled" Style="margin-top: 0px"
                                                    BackColor="White" BorderColor="White" BorderWidth="2px" CellSpacing="1" AllowPaging="True"
                                                    AllowSorting="True" OnPageIndexChanging="gvMain_PageIndexChanging" OnSorting="gvMain_Sorting"
                                                    OnRowCommand="gvMain_RowCommand" OnRowDataBound="gvMain_RowDataBound">
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
                                                        <asp:TemplateField HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText">
                                                            <HeaderTemplate>
                                                                RC No.
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRCNO_GV" runat="server" Text='<%# Bind("RCNO") %>' ToolTip='<%# Eval("SHORTSTATUS") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="RCDATE" HeaderText="Recv Date" SortExpression="RCDATE"
                                                            HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="NAME" HeaderText="Name" SortExpression="NAME" HeaderStyle-CssClass="gridText"
                                                            ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="PFNUMBER" HeaderText="PF Number" SortExpression="PFNUMBER"
                                                            HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="RECVDATE" HeaderText="Recv Date" SortExpression="RECVDATE"
                                                            HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="DESIGNATION" HeaderText="Designation" SortExpression="DESIGNATION"
                                                            HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="SANCTIONPROSECUTION" HeaderText="Sanction for Prosecution"
                                                            SortExpression="SANCTIONPROSECUTION" HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="REFUSEDDATE" HeaderText="Refused Date" SortExpression="REFUSEDDATE"
                                                            HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="CVCDATE" HeaderText="CVC Date" SortExpression="CVCDATE"
                                                            HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="ENTRYBY" HeaderText="Entry By" SortExpression="ENTRYBY"
                                                            HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="ENTRYDATE" HeaderText="Entry Date" SortExpression="ENTRYDATE"
                                                            HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="MODIFYBY" HeaderText="Modify By" SortExpression="MODIFYBY"
                                                            HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                        <asp:BoundField DataField="MODIFYDATE" HeaderText="Modify Date" SortExpression="MODIFYDATE"
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
                <asp:HiddenField ID="hidSanctionforProsecution" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
