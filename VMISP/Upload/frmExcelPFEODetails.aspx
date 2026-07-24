<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="frmExcelPFEODetails.aspx.cs" Inherits="VMISP.Upload.frmExcelPFEODetails" %>

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
                PF Excel Upload
            </div>
            <br />
            <div class="col-sm-12 alert alert-dark">
                <div class="form-group row" style="padding-right: 5px;">
                    <div class="col-sm-2">
                        <label for="ddlTableName"><span style="color: #FF0000">*</span>Excel Upload For </label>
                        <asp:DropDownList ID="ddlTableName" runat="server" CssClass="form-control input-sm">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Complaint" Value="COMPLAINT"></asp:ListItem>
                            <asp:ListItem Text="MISC" Value="MISC"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <label for="fileUpload"><span style="color: #FF0000">*</span>File </label>
                        <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-control input-sm" />
                    </div>
                    <div class="col-sm-1" style="padding-top: 24px;">
                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" CssClass="btn btn-sm btn-info" />
                    </div>
                    <div class="col-sm-2" style="padding-top: 24px;">
                        <asp:LinkButton ID="lnkExcel" runat="server" Text="Download Format" OnClick="lnkExcel_Click" CssClass="btn btn-danger btn-sm"></asp:LinkButton>
                    </div>
                    <div class="col-sm-1" style="padding-top: 24px;">
                        <asp:LinkButton ID="lnkPrint_Excel" runat="server" Text="Excel Download" OnClick="lnkPrint_Excel_Click" Visible="false" CssClass="btn btn-sm btn-warning"></asp:LinkButton>
                    </div>
                    <div class="col-sm-1" style="padding-top: 24px;">
                        <asp:LinkButton ID="lnkPrint_PDF" runat="server" Text="PDF Download" OnClick="lnkPrint_PDF_Click" Visible="false" CssClass="btn btn-sm btn-danger"></asp:LinkButton>
                    </div>
                    <div class="col-sm-3" style="padding-top: 24px;">
                        <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
                    </div>
                </div>
                <div class="form-group row" style="padding-right: 5px;">
                    <div class="col-sm-12">
                        <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="false" CssClass="table input-sm table-bordered table-condensed">
                            <Columns>
                                <asp:BoundField DataField="ROWNO" HeaderText="S No." />
                                <asp:BoundField DataField="PFNUMBER" HeaderText="PF Number" />
                                <asp:BoundField DataField="EO_PFNUMBER" HeaderText="EO PF Number" />
                                <asp:BoundField DataField="RNO" HeaderText="R No." />
                                <asp:BoundField DataField="ACCOUNTNAME" HeaderText="Account Name" />
                                <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" />
                                <asp:BoundField DataField="AMOUNT" HeaderText="Amount" />
                                <asp:BoundField DataField="STATUS" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
