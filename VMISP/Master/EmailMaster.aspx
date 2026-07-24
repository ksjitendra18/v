<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="EmailMaster.aspx.cs" Inherits="VMISP.Master.EmailMaster" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script src="/Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="/Js/JS_CommonValidation.js" type="text/javascript"></script>
    <script src="/Js/jquery-3.3.1.min.js" type="text/javascript"></script>
    <script src="/Js/jquery.min.js"></script>

    <script src="/Js/jquery-1.9.1.js"></script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <link href="/Js/jquery-ui.css" rel="stylesheet" />
    <link href="/css/bootstrap.css" rel="stylesheet" />
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="form-group">
                <br />
                <div class="panel panel-primary" style="overflow: hidden;">
                    <div class="panel-heading bg-info" style="font-weight: bold; color: white; font-size: 15px;">Email Master</div>
                    <div class="col-sm-12 alert alert-dark">
                        <div class="form-group">
                            <div class="col-sm-3">
                                <label for="ddlAuthority" class="control-label"><span style="color: #FF0000">*</span>Authority</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="fa fa-arrow-circle-o-down"></i></span>
                                    <asp:DropDownList ID="ddlAuthority" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlAuthority_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Circle Office" Value="Circle"></asp:ListItem>
                                        <asp:ListItem Text="Zonal Office" Value="Zone"></asp:ListItem>
                                        <asp:ListItem Text="Head Office" Value="HO"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <label for="ddlAuthorityDetail" class="control-label"><span style="color: #FF0000">*</span>Authority Detail</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="fa fa-arrow-circle-o-down"></i></span>
                                    <asp:DropDownList ID="ddlAuthorityDetail" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlAuthorityDetail_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label for="txtEmailID" class="control-label"><span style="color: #FF0000">*</span>Email ID</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="fa fa-pencil"></i></span>
                                    <asp:TextBox ID="txtEmailID" runat="server" placeholder="Email ID" CssClass="form-control input-sm" onblur="checkEmail(this.value)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" style="padding-top: 50px;">
                            <div class="col-sm-3">
                            </div>
                            <div class="col-sm-9">
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success btn-sm" Text="Submit" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnReset" runat="server" CssClass="btn btn-warning btn-sm" Text="Reset" OnClick="btnReset_Click" />
                                <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger" Font-Size="Medium"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
