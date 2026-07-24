<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmVigilanceUpdate.aspx.cs" Inherits="VMISP.Mis.frmVigilanceUpdate"
    ValidateRequest="false" %>

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
                <div class="panel panel-primary">
                    <div class="panel-heading" style="font-size: medium; font-weight: bold;">
                        Vigilance Update
                    </div>
                    <br />
                    <div class="col-sm-12 alert alert-dark">
                        <div class="form-group row" style="padding-right: 5px;">
                            <div class="col-sm-2">
                                <label for="txtRNo"><span style="color: #FF0000">*</span>R Number</label>
                                <asp:TextBox ID="txtRNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <label for="ddlField"><span style="color: #FF0000">*</span>Field</label>
                                <asp:DropDownList ID="ddlField" runat="server" CssClass="form-control input-sm">
                                    <asp:ListItem Text="Select" Value="SELECT"></asp:ListItem>
                                    <asp:ListItem Text="Basic Pay" Value="BASICPAY"></asp:ListItem>
                                    <asp:ListItem Text="DA_CO/ZO/HO" Value="DA_CO_ZO_HO"></asp:ListItem>
                                    <asp:ListItem Text="Register" Value="REGISTER"></asp:ListItem>
                                    <asp:ListItem Text="Penalty Proceeding" Value="PENALTYPROCEEDING"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-1">
                                <label for="ddlField"><span style="color: #FF0000">*</span><asp:Label ID="lblValueCaption" runat="server" class="lblCaption"></asp:Label></label>
                                <asp:HiddenField ID="hidColumnDataType" runat="server" />
                            </div>
                            <div class="col-sm-2" id="divBASICPAY" runat="server" style="display: none;">
                                <asp:TextBox ID="txtBASICPAY" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-2" id="divDACOZOHO" runat="server" style="display: none">
                                <asp:DropDownList ID="ddlDACOZOHO" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-2" id="divREGISTER" runat="server" style="display: none">
                                <asp:DropDownList ID="ddlRegister" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-2" id="divPENALTYPROCEEDING" runat="server" style="display: none">
                                <asp:DropDownList ID="ddlPenaltyProceeding" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row" style="padding-right: 5px;">
                            <div class="col-sm-3">
                            </div>
                            <div class="col-sm-9">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-sm btn-success" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-sm btn-warning" OnClick="btnCancel_Click" />&nbsp;&nbsp;
                                <asp:Label ID="lblMsg" runat="server" CssClass="label label-success"></asp:Label>
                            </div>
                        </div>
                    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
