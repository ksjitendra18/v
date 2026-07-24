<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmCustomizeReports.aspx.cs" Inherits="VMISP.Search.frmCustomizeReports" %>

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
    <div style="width: 100%; border: 1px:solid; height: 20px; text-align: center; background-color: #99232F;
        color: White; font-weight: 700;">
        Form Wise Search
    </div>
    <asp:Panel ID="pnlMain" runat="server" Width="100%">
        <table width="44%" align="left" style="border: 1px solid #000000; height: 400px;
            margin-top: 2px;">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td class="tdTextReight">
                                <span class="lblCaption">Form :</span>
                            </td>
                            <td class="tdTextLeft">
                                <asp:DropDownList ID="ddlTableName" runat="server" CssClass="ddlDefault1" Width="115px"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlTableName_SelectedIndexChanged">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Complaint" Value="COMPLAINT"></asp:ListItem>
                                    <asp:ListItem Text="IAC Entry" Value="IAC"></asp:ListItem>
                                    <asp:ListItem Text="MISC" Value="MISC"></asp:ListItem>
                                    <asp:ListItem Text="NOC" Value="NOC"></asp:ListItem>
                                    <asp:ListItem Text="Operational Ref" Value="OPERATIONALREF"></asp:ListItem>
                                    <asp:ListItem Text="RRB" Value="RRB"></asp:ListItem>
                                    <asp:ListItem Text="RTI" Value="RTI"></asp:ListItem>
                                    <asp:ListItem Text="SR" Value="SR"></asp:ListItem>
                                    <asp:ListItem Text="Vigilance" Value="VIGILANCE"></asp:ListItem>
                                    <asp:ListItem Text="Whistle Blower" Value="WB"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="tdTextLeft">
                                <asp:Button ID="btnGet" runat="server" OnClick="btnGet_Click" Text="Search" CssClass="btnSearch" />&nbsp
                                <asp:Button ID="btnExcel" runat="server" OnClick="btnExel_Click" Text="Excel" CssClass="btnSearch"
                                    Visible="false" />&nbsp
                                <asp:Button ID="btnPDF" runat="server" OnClick="btnPdf_Click" Text="PDF" CssClass="btnSearch"
                                    Visible="false" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" style="border: 1px solid #000000; height: 369px;">
                        <tr>
                            <td colspan="1">
                                <asp:Panel ID="pnlNoRecords" runat="server" Visible="false" Width="99%">
                                    <asp:Image ID="Image1" ImageUrl="~/images/NoDataFound_3.jpg" ImageAlign="AbsMiddle"
                                        runat="server" />
                                </asp:Panel>
                                <asp:Panel ID="pnlGridDetails" runat="server" Visible="false">
                                    <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" DataKeyNames="RNO"
                                        CellPadding="3" GridLines="None" ViewStateMode="Enabled" Style="margin-top: 0px"
                                        BackColor="White" BorderColor="White" BorderWidth="2px" CellSpacing="1" Width="100%"
                                        AllowPaging="True" AllowSorting="True">
                                        <Columns>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="55%" align="left" style="margin-left: 2px; border: 1px solid #000000;
            height: 395px; margin-top: 2px;">
            <tr>
                <td colspan="1">
                    <fieldset style="margin-top: -3px; border: 1px solid #000000; width: 96%; height: 253px;">
                        <legend>
                            <asp:Label ID="lblColumnHeader" runat="server" CssClass="lblCaption" ForeColor="#009900"></asp:Label></legend>
                        <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Vertical" Style="height: 240px;
                            margin-top: -15px;">
                            <table style="width: 105%;">
                                <tr>
                                    <td style="width: 100%">
                                        <asp:CheckBoxList ID="chkColumnName" runat="server" RepeatColumns="3" Style="font-size: 11px;
                                            font-style: normal; font-weight: bold; font-family: Verdana, Geneva, Tahoma, sans-serif;"
                                            ForeColor="#FF0066" RepeatDirection="Vertical">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </fieldset>
                    <fieldset style="margin-top: -18px; border: 1px solid #000000; height: 83px;">
                        <legend><span class="lblCaption">Condition</span></legend>
                        <table width="100%" style="margin-top: -25px">
                            <tr>
                                <td class="tdTextReight">
                                    <span class="lblCaption">Column :</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="ddlDefault" Width="65px">
                                        <asp:ListItem Text="Select" Value="SELECT"></asp:ListItem>
                                        <asp:ListItem Text="TEXT" Value="TEXT"></asp:ListItem>
                                        <asp:ListItem Text="DATE" Value="DATE"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtColumnName_WHERE" runat="server" CssClass="txtDefault" Width="200"
                                        placeholder="Enter Column Name"></asp:TextBox>
                                    <asp:DropDownList ID="ddlCondition_WHERE" runat="server" CssClass="ddlDefault" Width="100px">
                                        <asp:ListItem Text="=" Value="="></asp:ListItem>
                                        <asp:ListItem Text="<" Value="<"></asp:ListItem>
                                        <asp:ListItem Text=">" Value=">"></asp:ListItem>
                                        <asp:ListItem Text="=>" Value="=>"></asp:ListItem>
                                        <asp:ListItem Text="=<" Value="=<"></asp:ListItem>
                                        <asp:ListItem Text="IN" Value="IN"></asp:ListItem>
                                        <asp:ListItem Text="NOT IN" Value="NOT IN"></asp:ListItem>
                                        <asp:ListItem Text="IS NULL" Value="IS NULL"></asp:ListItem>
                                        <asp:ListItem Text="IS NOT NULL" Value="IS NOT NULL"></asp:ListItem>
                                        <asp:ListItem Text="BETWEEN" Value="BETWEEN"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdTextReight">
                                    <asp:Label ID="lblComplaintNoRequired" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label><asp:Label
                                        ID="lblValueCaption" runat="server" class="lblCaption"></asp:Label>
                                    <asp:HiddenField ID="hidColumnDataType" runat="server" />
                                </td>
                                <td id="tdText" runat="server" style="display: none">
                                    <asp:Panel ID="pnlText" runat="server">
                                        <asp:TextBox ID="txtConditionValue_WHERE" runat="server" CssClass="txtDefault" Width="268"
                                            placeholder="Enter Column Value"></asp:TextBox>
                                    </asp:Panel>
                                </td>
                                <td id="tdDate" runat="server" style="display: none">
                                    <asp:Panel ID="pnlDate" runat="server">
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                        <act:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                        </act:MaskedEditExtender>
                                        <act:CalendarExtender ID="ceFromDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                            TargetControlID="txtFromDate" PopupButtonID="imgFromDate" CssClass="cal_Theme1">
                                        </act:CalendarExtender>
                                        <asp:ImageButton ID="imgFromDate" runat="server" AlternateText="Please Select date!!"
                                            ImageUrl="~/images/calendar.png" />&nbsp;
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                        <act:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
                                            Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="dd/MM/yyyy" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                                        </act:MaskedEditExtender>
                                        <act:CalendarExtender ID="ceToDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                            TargetControlID="txtToDate" PopupButtonID="imgToDate" CssClass="cal_Theme1">
                                        </act:CalendarExtender>
                                        <asp:ImageButton ID="imgToDate" runat="server" AlternateText="Please Select date!!"
                                            ImageUrl="~/images/calendar.png" />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table width="100%" style="margin-top: -5px;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNaote" runat="server" Text="Note : " Font-Bold="True" ForeColor="Red"
                                                    Font-Names="Verdana" Font-Size="X-Small"></asp:Label><asp:Label ID="lblNoteDetails"
                                                        runat="server" Text="Copy and paste column name in textbox, if column datatype date then select "
                                                        Font-Names="Verdana" Font-Size="X-Small"></asp:Label><asp:Label ID="lblNoteDetailsA"
                                                            runat="server" Text="'DATE' " Font-Bold="True" ForeColor="#000099" Font-Size="X-Small"
                                                            Font-Names="Verdana"></asp:Label><asp:Label ID="lblNoteDetailsB" runat="server" Text="from "
                                                                Font-Names="Verdana" Font-Size="X-Small"></asp:Label><asp:Label ID="lblNoteDetailsC"
                                                                    runat="server" Text="'COLUMN' " Font-Bold="True" ForeColor="#000099" Font-Size="X-Small"
                                                                    Font-Names="Verdana"></asp:Label><asp:Label ID="lblNoteDetailsD" runat="server" Text="dropdown list else "
                                                                        Font-Names="Verdana" Font-Size="X-Small"></asp:Label><asp:Label ID="lblNoteDetailsE"
                                                                            runat="server" Text="'TEXT'" Font-Bold="True" ForeColor="#000099" Font-Size="X-Small"
                                                                            Font-Names="Verdana"></asp:Label><asp:Label ID="lblNoteDetailsF" runat="server" Text=", if select "
                                                                                Font-Names="Verdana" Font-Size="X-Small"></asp:Label><asp:Label ID="lblNoteDetailsG"
                                                                                    runat="server" Text="'IN'" Font-Bold="True" ForeColor="#000099" Font-Size="X-Small"
                                                                                    Font-Names="Verdana"></asp:Label><asp:Label ID="lblNoteDetailsH" runat="server" Text=" and"
                                                                                        Font-Names="Verdana" Font-Size="X-Small"></asp:Label><asp:Label ID="lblNoteDetailsI"
                                                                                            runat="server" Text=" 'NOT IN'" Font-Bold="True" ForeColor="#000099" Font-Size="X-Small"
                                                                                            Font-Names="Verdana"></asp:Label><asp:Label ID="lblNoteDetailsJ" runat="server" Text=" condition from dropdown then text formatted as (|a|,|b|)"
                                                                                                Font-Names="Verdana" Font-Size="X-Small"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
