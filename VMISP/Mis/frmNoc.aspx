<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmNoc.aspx.cs" Inherits="VMISP.Mis.frmNoc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/ssMain.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="../Js/JS_CommonValidation.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F; color: White; font-weight: 700;">
                NOC
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
                    <act:TabPanel ID="tabEntry" runat="server" TabIndex="1">
                        <HeaderTemplate>
                            <asp:Label ID="lblEntryHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                Font-Size="Small" Text="Entry" ToolTip="NOC Structure Entry"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="background-color: #FFE4E1">
                                <tr>
                                    <td class="tdTextReight" style="width: 8.6%">
                                        <asp:Label ID="lblSNoRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><span class="lblCaption">S No. :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                        <asp:ImageButton ID="imgGet" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                            Width="25px" Height="30px" OnClick="btnGet_Click" ToolTip="NOC Search" />
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
                                        <span class="lblCaption">Branch Name :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBRComplaint" runat="server" CssClass="txtDefault"></asp:TextBox>
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
                                    <td class="tdTextReight" style="width: 8.6%">
                                        <span class="lblCaption">PF No :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPFNo" runat="server" CssClass="txtNO"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Clearance Date :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearanceDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                        <act:MaskedEditExtender ID="meeClearanceDate" runat="server" TargetControlID="txtClearanceDate"
                                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="" Enabled="True">
                                        </act:MaskedEditExtender>
                                        <act:CalendarExtender ID="ceClearanceDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                            TargetControlID="txtClearanceDate" PopupButtonID="imgClearanceDate" CssClass="cal_Theme1">
                                        </act:CalendarExtender>
                                        <asp:ImageButton ID="imgClearanceDate" runat="server" AlternateText="Please Select date!!"
                                            ImageUrl="~/images/calendar.png" />
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Name :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Designation :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDesignation" runat="server" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdTextReight" style="width: 8.6%">
                                        <span class="lblCaption">State :</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlState" runat="server" CssClass="ddlDefault1">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Scale :</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlScale" runat="server" CssClass="ddlDefault" Width="84px">
                                        </asp:DropDownList>
                                        &nbsp;
                                        <asp:Panel ID="pnlNatureMIS" runat="server" Visible="False">
                                            <asp:Label ID="lblNatureMIS" runat="server"></asp:Label>
                                        </asp:Panel>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Bank Name :</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBankName" runat="server" CssClass="ddlDefaultVig">
                                            <asp:ListItem Value="PNB" Text="PNB"></asp:ListItem>
                                              <asp:ListItem Value="eOBC" Text="eOBC"></asp:ListItem>
                                            <asp:ListItem Value="eUNI" Text="eUNI"></asp:ListItem>
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
                                    <td class="tdTextReight" style="width: 8.6%">
                                        <span class="lblCaption">Remarks :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="txtDefault" Width="98.1%" TextMode="MultiLine"
                                            Height="35px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <asp:Panel ID="pnlHOStatus" runat="server" Visible="False">
                                        <td class="tdTextReight" style="width: 8.6%">
                                            <span class="lblCaption">HO Remarks :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHORemarks" runat="server" CssClass="txtDefault" Width="98.1%"></asp:TextBox>
                                        </td>
                                    </asp:Panel>
                                </tr>
                                <tr>
                                    <td class="tdTextReight" style="width: 15%">
                                        <span class="lblCaption">Dealing Officer Remarks :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDealingOfficerRemarks" runat="server" Width="98.1%" placeholder="Enter Dealing Officer Remarks, If Any...." TextMode="MultiLine" onkeypress="return blockSpecialChar(event)" Enabled="false"></asp:TextBox>
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
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="tabList" runat="server" HeaderText="List" TabIndex="2">
                        <HeaderTemplate>
                            <asp:Label ID="lblListHeaderText" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                Font-Size="Small" Text="List" ToolTip="List of NOC Entry"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="65%" style="margin-top: -10px; border-bottom: 1px solid;">
                                <tr>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">S No. :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRNo_LIST" runat="server" Width="100px" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">PF No :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPFNumber_LIST" runat="server" Width="100px" CssClass="txtDefault"></asp:TextBox>
                                    </td>
                                    <td class="tdTextReight">
                                        <span class="lblCaption">Name :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName_LIST" runat="server" Width="100px" CssClass="txtDefault"></asp:TextBox>
                                        &nbsp;<asp:ImageButton ID="imgSearch_List" runat="server" ImageAlign="Middle" ImageUrl="~/images/Search.jpg"
                                            Width="25px" Height="30px" OnClick="imgSearch_LIST_Click" ToolTip="NOC Search" />
                                        &nbsp;&nbsp;<asp:Label ID="lblList" runat="server" CssClass="lblMsg"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Both" Height="350px" Width="100%">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100%">
                                            <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" DataKeyNames="SNO"
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
                                                    <asp:BoundField DataField="SNO" HeaderText="S No." SortExpression="SNO" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="RECDATE" HeaderText="Rec Date" SortExpression="RECDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="BRCOMPLAINT" HeaderText="Branch Name" SortExpression="BRCOMPLAINT"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" SortExpression="CIRCLEOFFICE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="PFNO" HeaderText="PF No" SortExpression="PFNO" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="CLOSUREDATE" HeaderText="Clearance Date" SortExpression="CLOSUREDATE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="NAME" HeaderText="Name" SortExpression="NAME" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="DESIGNATION" HeaderText="Designation" SortExpression="DESIGNATION"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="STATE" HeaderText="State" SortExpression="STATE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SCLAECODE" HeaderText="Scale Code" SortExpression="SCLAECODE"
                                                        HeaderStyle-CssClass="gridText" ItemStyle-CssClass="gridText" />
                                                    <asp:BoundField DataField="SCALE" HeaderText="Scale" SortExpression="SCALE" HeaderStyle-CssClass="gridText"
                                                        ItemStyle-CssClass="gridText" />
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Remarks
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
                <asp:HiddenField ID="hidScale" runat="server" />
                <asp:HiddenField ID="hidState" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
