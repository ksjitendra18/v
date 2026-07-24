<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmExcelUpload.aspx.cs" Inherits="VMISP.Upload.frmExcelUpload" %>

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
    <div class="col-lg-12">
        <div class="form-group row">
            <div class="panel panel-primary">
                <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                    Excel Upload Form
                </div>
                <br />
                <div class="col-sm-12 alert alert-dark">
                    <div class="form-group row">
                        <div class="col-sm-4">
                            <label for="ddlTableName"><span style="color: #FF0000">*</span>Excel Upload For</label>
                            <asp:DropDownList ID="ddlTableName" runat="server" CssClass="form-control input-sm">
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                <asp:ListItem Text="COMPLAINT" Value="COMPLAINT"></asp:ListItem>
                                <asp:ListItem Text="IAC Entry" Value="IAC"></asp:ListItem>
                                <asp:ListItem Text="Lodi Entry" Value="LODI"></asp:ListItem>
                                <asp:ListItem Text="MISC" Value="MISC"></asp:ListItem>
                                <asp:ListItem Text="NOC" Value="NOC"></asp:ListItem>
                                <asp:ListItem Text="RTI" Value="RTI"></asp:ListItem>
                                <asp:ListItem Text="RRB" Value="RRB"></asp:ListItem>
                                <asp:ListItem Text="SR" Value="SR"></asp:ListItem>
                                <asp:ListItem Text="SANCTION_FOR_INVESTIGATION" Value="SANCTION_FOR_INVESTIGATION"></asp:ListItem>
                                <asp:ListItem Text="SANCTION_FOR_PROSECUTION" Value="SANCTION_FOR_PROSECUTION"></asp:ListItem>
                                <asp:ListItem Text="VIGILANCE" Value="VIGILANCE"></asp:ListItem>
                                <asp:ListItem Text="VIGILANCEMIS" Value="VIGILANCEMIS"></asp:ListItem>
                                <asp:ListItem Text="WB" Value="WB"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-4">
                            <label for="fileUpload"><span style="color: #FF0000">*</span>Select File</label>
                            <asp:FileUpload ID="fileUpload" runat="server" Width="200px" onchange="return validateFileExtension(this)" />
                        </div>
                        <div class="col-sm-4">
                            <label for="fileUpload"><span style="color: #FF0000">*</span>Download Excel Format of</label>
                            <asp:DropDownList ID="ddlDownloadFormat" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlDownloadFormat_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Complaint" Value="COMPLAINT"></asp:ListItem>
                                <asp:ListItem Text="IAC" Value="IAC"></asp:ListItem>
                                <asp:ListItem Text="Lodi Entry" Value="LODI"></asp:ListItem>
                                <asp:ListItem Text="MISC" Value="MISC"></asp:ListItem>
                                <asp:ListItem Text="NOC" Value="NOC"></asp:ListItem>
                                <asp:ListItem Text="RTI" Value="RTI"></asp:ListItem>
                                <asp:ListItem Text="RRB" Value="RRB"></asp:ListItem>
                                <asp:ListItem Text="SR" Value="SR"></asp:ListItem>
                                <asp:ListItem Text="Sanction For Investigation" Value="SANCTION_FOR_INVESTIGATION"></asp:ListItem>
                                <asp:ListItem Text="Sanction For Prosecution" Value="SANCTION_FOR_PROSECUTION"></asp:ListItem>
                                <asp:ListItem Text="VIGILANCE" Value="VIGILANCE"></asp:ListItem>
                                <asp:ListItem Text="Vigilance MIS" Value="VIGILANCEMIS"></asp:ListItem>
                                <asp:ListItem Text="WB" Value="WB"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-3">
                        </div>
                        <div class="col-sm-1">
                            <asp:Button ID="btnVerify" runat="server" CssClass="btn btn-sm btn-success" Text="Verify" OnClick="btnVerify_Click"></asp:Button>
                        </div>
                        <div class="col-sm-1">
                            <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-sm btn-primary" Visible="false" Text="Upload" OnClick="btnUpload_Click"></asp:Button>
                        </div>
                        <div class="col-sm-7">
                            <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
