<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="frmEOSearch.aspx.cs" Inherits="VMISP.Search.frmEOSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <script src="/Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="/Js/JS_CommonValidation.js" type="text/javascript"></script>
    <script src="/Js/jquery-3.3.1.min.js" type="text/javascript"></script>
    <script src="/Js/jquery.min.js"></script>

    <script src="/Js/jquery-1.9.1.js"></script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <link href="/Js/jquery-ui.css" rel="stylesheet" />
    <link href="/css/bootstrap.css" rel="stylesheet" />
    <div class="form-group row">
        <div class="panel panel-primary">
            <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                Accussed/ EO Search
            </div>
            <br />
            <div class="col-sm-12 alert alert-dark">
                <div class="form-group row" style="padding-right: 5px;">
                    <div class="col-sm-3">
                        <label for="ddlTableName"><span style="color: #FF0000">*</span>Form Name </label>
                        <asp:DropDownList ID="ddlTableName" runat="server" CssClass="form-control input-sm">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Complaint" Value="COMPLAINT"></asp:ListItem>
                            <asp:ListItem Text="MISC" Value="MISC"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <label for="txtRNO"><span style="color: #FF0000">*</span>R Number </label>
                        <asp:TextBox ID="txtRNO" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <label for="txtEOPFNumber"><span style="color: #FF0000">*</span>EO PF Number </label>
                        <asp:TextBox ID="txtEOPFNumber" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-1" style="padding-top: 24px;">
                        <asp:Button ID="btnGet" runat="server" OnClick="btnGet_Click" Text="Search" CssClass="btn btn-info btn-sm" />
                    </div>
                    <div class="col-sm-1" style="padding-top: 24px;">
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Excel" CssClass="btn btn-primary btn-sm" Visible="false" />
                    </div>
                    <div class="col-sm-1" style="padding-top: 24px;">
                        <asp:Button ID="btnPDF" runat="server" OnClick="btnPDF_Click" Text="PDF" CssClass="btn btn-success btn-sm" Visible="false" />
                    </div>
                    <div class="col-sm-2" style="padding-top: 24px;">
                        <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
                    </div>
                </div>
                <div class="form-group row" style="padding-right: 5px;">
                    <div class="col-sm-12">
                        <asp:GridView ID="gvEODetails" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered input-sm table-condensed">
                            <Columns>
                                <asp:BoundField DataField="ROWNO" HeaderText="S. No." />
                                <asp:BoundField DataField="RNO" HeaderText="R. No." />
                                <asp:BoundField DataField="RNO_DATE" HeaderText="RNo Date" />
                                <asp:BoundField DataField="TABLE_NAME" HeaderText="Table Name" />
                                <asp:BoundField DataField="TYPE" HeaderText="Accused or EO" />
                                <asp:BoundField DataField="EO_NAME" HeaderText="EO Name" />
                                <asp:BoundField DataField="EO_PF" HeaderText="EO PF Number" />
                                <asp:BoundField DataField="EO_DOR" HeaderText="EO Retirement Date" />
                                <asp:BoundField DataField="STATUS" HeaderText="Status" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status Code" />
                                <asp:BoundField DataField="CIRCLE_SOLID" HeaderText="Circle" />
                                <asp:BoundField DataField="ZONE_SOLID" HeaderText="Zone" />
                            </Columns>
                            <HeaderStyle BackColor="darkseagreen" />
                            <RowStyle BackColor="White" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
