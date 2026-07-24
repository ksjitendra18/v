<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="SanctionDataUpload.aspx.cs" Inherits="VMISP.Mis.SanctionDataUpload" %>

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
                    Sanction Investigation/ Prosecution Data Upload
                </div>
                <br />
                <div class="col-sm-12 alert alert-dark">
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-2">
                            <label for="ddlDataUploadFor"><span style="color: #FF0000">*</span>Data Upload for</label>
                            <asp:DropDownList ID="ddlDataUploadFor" runat="server" CssClass="form-control input-sm">
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Investigation" Value="INVESTIGATION"></asp:ListItem>
                                <asp:ListItem Text="Prosecution" Value="PROSECUTION"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-3">
                            <label for="fileUpload"><span style="color: #FF0000">*</span>Select File</label>
                            <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-control input-sm" onchange="return validateFileExtension(this)" />
                        </div>
                        <div class="col-sm-1" style="padding-top: 24px;">
                            <label for="fileUpload">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                            <asp:Button ID="btnVerify" runat="server" Text="Verify" CssClass="btn btn-info input-sm" OnClick="btnVerify_Click" />
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" Visible="false" CssClass="btn btn-primary input-sm" OnClick="btnSubmit_Click" />
                        </div>
                        <div class="col-sm-2" style="padding-right: 24px;">
                            <label for="btndownload">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                            <asp:Button ID="btndownload" runat="server" Text="Download Excel Format" CssClass="btn btn-danger input-sm" OnClick="btndownload_Click" />
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-11">
                            <asp:Label ID="lblMsg" runat="server" CssClass="label label-success"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row" style="padding-right: 5px;">
                        <ul class="bottom_notes">
                            <li><span style="color: #FF0000">*</span> marked fields are mandatory</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
