<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmIACUpdate.aspx.cs" Inherits="VMISP.Mis.frmIACUpdate" ValidateRequest="false" %>

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
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <div class="col-lg-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                            IAC Update
                        </div>
                        <br />
                        <div class="col-sm-12 alert alert-dark">
                            <div class="form-group row" style="padding-right: 5px;">
                                <div class="col-sm-3">
                                    <label for="txtIACNo"><span style="color: #FF0000">*</span>IAC Number</label>
                                    <asp:TextBox ID="txtIACNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                                <div class="col-sm-3">
                                    <label for="txtDA"><span style="color: #FF0000">*</span>DA</label>
                                    <asp:TextBox ID="txtDA" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                                <div class="col-sm-1" style="padding-top: 23px;">
                                    <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-sm btn-primary" Text="Submit"></asp:Button>
                                </div>
                                <div class="col-sm-1" style="padding-top: 23px;">
                                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn btn-sm btn-warning" Text="Cancel"></asp:Button>
                                </div>
                                <div class="col-sm-4" style="padding-top: 23px;">
                                    <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
