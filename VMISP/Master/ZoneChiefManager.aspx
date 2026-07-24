<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="ZoneChiefManager.aspx.cs" Inherits="VMISP.Master.ZoneChiefManager" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
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
                    <div class="panel-heading bg-info" style="font-weight: bold; color: white; font-size: 15px;">Zone Chief Manager Master</div>
                    <div class="col-sm-12 alert alert-dark">
                        <div class="form-group row">
                            <div class="col-sm-6">
                                <label for="ddlZone" class="control-label"><span style="color: #FF0000">*</span>Zone</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-pencil"></i></span>
                                    <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label for="ddlType" class="control-label"><span style="color: #FF0000">*</span>Type</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-pencil"></i></span>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control input-sm">
                                        <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                        <asp:ListItem Text="Preventive" Value="Preventive"></asp:ListItem>
                                        <asp:ListItem Text="Punitive" Value="Punitive"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <label for="txtCMName" class="control-label"><span style="color: #FF0000">*</span>Chief Manager Name</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-pencil"></i></span>
                                    <asp:TextBox ID="txtCMName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <label for="txtRemarks" class="control-label">Remarks</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="fa fa-pencil"></i></span>
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3">
                            </div>
                            <div class="col-sm-9">
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-success" Visible="true" OnClick="btnSubmit_Click" Text="Submit"></asp:Button>
                                <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-sm btn-success" Visible="False" OnClick="btnUpdate_Click" Text="Update"></asp:Button>
                                <asp:Button ID="btnReset" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnReset_Click" Text="Reset"></asp:Button>
                                <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger" Font-Size="Medium"></asp:Label>
                            </div>
                        </div>
                        <ul class="bottom_notes">
                            <li><span style="color: #FF0000">*</span> marked fields are mandatory</li>
                        </ul>
                        <div class="clearfix"></div>
                        <br />
                        <div class="panel panel-primary" style="overflow: hidden;">
                            <div class="panel-heading bg-primary" style="font-weight: bold; color: white; font-size: 15px;">
                                Zone Chief Manager Master Details
                        <p style="float: right;">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-danger" OnClick="btnSearch_Click" Text="Search"></asp:Button>
                        </p>
                                <p style="float: right;">
                                    <asp:TextBox ID="txtSolidSearch" runat="server" class="form-control input-sm" placeHolder="Enter Zone SolID"></asp:TextBox>
                                </p>
                            </div>
                            <div class="form-three widget-shadow panel-body bg-light">
                                <div class="panel-body-inputin">
                                    <div class="form-group">
                                        <div class="col-sm-12">
                                            <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered input-sm table-condensed" OnRowCommand="gvMain_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>S No</HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>Select</HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-sm btn-danger" CommandArgument='<%#Eval("REFNO") + "~" + Eval("ZONE_SOLID") + "~"+ Eval("ZONETYPE")+"~"+Eval("NAME")+"~"+Eval("REMARKS")%>' CommandName="SELECT" Text="Edit"></asp:Button>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ZONE" HeaderText="Zone" />
                                                    <asp:BoundField DataField="ZONETYPE" HeaderText="Type" />
                                                    <asp:BoundField DataField="NAME" HeaderText="Chief Manager Name" />
                                                    <asp:BoundField DataField="ADDUSER" HeaderText="Entry By" />
                                                    <asp:BoundField DataField="ADDDATE" HeaderText="Entry Date" />
                                                    <asp:BoundField DataField="MODUSER" HeaderText="Modify By" />
                                                    <asp:BoundField DataField="MODDATE" HeaderText="Modify Date" />
                                                </Columns>
                                                <HeaderStyle BackColor="burlywood" />
                                                <RowStyle BackColor="White" />
                                            </asp:GridView>
                                            <span class="input-sm" style="color: maroon;" id="lastUpdated" runat="server" visible="false">Last Updated &nbsp; <%: DateTime.Now %></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
