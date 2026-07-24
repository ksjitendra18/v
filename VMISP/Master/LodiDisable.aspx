<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="LodiDisable.aspx.cs" Inherits="VMISP.Master.LodiDisable" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <script src="/Js/JS_CommonFunction.js" type="text/javascript"></script>
    <script src="/Js/JS_CommonValidation.js" type="text/javascript"></script>
    <script src="/Js/jquery-3.3.1.min.js" type="text/javascript"></script>
    <script src="/Js/jquery.min.js"></script>

    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <link href="/Js/jquery-ui.css" rel="stylesheet" />
    <link href="/css/bootstrap.css" rel="stylesheet" />
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="form-group">
                <br />
                <div class="panel panel-primary" style="overflow: hidden;">
                    <div class="panel-heading bg-info" style="font-weight: bold; color: white; font-size: 15px;">Lodi Existing Details</div>
                    <div class="col-sm-12 alert alert-dark">
                        <div class="form-group row">
                            <div class="col-sm-3">
                                <label for="ddlYear" class="control-label"><span style="color: #FF0000">*</span>Year</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-pencil"></i></span>
                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control input-sm">
                                        <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                        <asp:ListItem Text="2023" Value="2023"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-1" style="padding-top: 25px;">
                                <asp:Button ID="btnGetDetails" runat="server" CssClass="btn btn-sm btn-success" OnClick="btnGetDetails_Click" Text="Get Details"></asp:Button>
                            </div>
                            <div class="col-sm-8" style="padding-top: 25px;">
                                <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger" Font-Size="Medium"></asp:Label>
                            </div>
                        </div>
                        <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                            <div class="form-group row">
                                <div class="col-sm-4">
                                    <label for="lblTotalRecords" class="control-label"><span style="color: #FF0000">*</span>Total Records for Disable</label>
                                    <div class="input-group input-icon right">
                                        <span class="input-group-addon"><i class="glyphicon glyphicon-pencil"></i></span>
                                        <asp:Label ID="lblTotalRecords" runat="server" CssClass="form-control input-sm"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-sm-8">
                                    <label for="txtRemarks" class="control-label"><span style="color: #FF0000">*</span>Remarks</label>
                                    <div class="input-group input-icon right">
                                        <span class="input-group-addon"><i class="glyphicon glyphicon-pencil"></i></span>
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-2">
                                </div>
                                <div class="col-sm-2">
                                    <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnUpdate_Click" Text="Disable Lodi Details"></asp:Button>
                                </div>
                                <div class="col-sm-8">
                                    <asp:Label ID="lblUpdateMsg" runat="server" CssClass="label label-danger" Font-Size="Medium"></asp:Label>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
