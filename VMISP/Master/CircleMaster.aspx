<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="CircleMaster.aspx.cs" Inherits="VMISP.Master.CircleMaster" %>

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
            <div class="form-group">
                <br />
                <div class="panel panel-danger" style="overflow: hidden;">
                    <div class="panel-heading bg-info" style="font-weight: bold; color: white; font-size: 15px;">Circle Master</div>
                    <div class="col-sm-12 panel-body bg-form">
                        <div class="form-group">
                            <div class="col-sm-9">
                                <label for="ddlZone" class="control-label"><span style="color: #FF0000">*</span>Zone</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="fa fa-arrow-circle-o-down"></i></span>
                                    <asp:DropDownList ID="ddlZone" runat="server" class="form-control input-sm" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <label for="txtSolID" class="control-label"><span style="color: #FF0000">*</span>Circle Solid</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="fa fa-pencil"></i></span>
                                    <asp:TextBox ID="txtSolID" runat="server" placeholder="Sol ID" MaxLength="6" class="form-control input-sm" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-6">
                                <label for="txtCircleName" class="control-label"><span style="color: #FF0000">*</span>Circle Name</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="fa fa-pencil"></i></span>
                                    <asp:TextBox ID="txtCircleName" runat="server" class="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label for="txtCircleAddress" class="control-label"><span style="color: #FF0000">*</span>Circle Address</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="fa fa-pencil"></i></span>
                                    <asp:TextBox ID="txtCircleAddress" runat="server" class="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-2">
                                <label for="ddlState" class="control-label"><span style="color: #FF0000">*</span>Active</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="fa fa-pencil"></i></span>
                                    <asp:DropDownList ID="ddlActive" runat="server" class="form-control input-sm">
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <label for="ddlState" class="control-label"><span style="color: #FF0000">*</span>Circle State</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="fa fa-pencil"></i></span>
                                    <asp:DropDownList ID="ddlState" runat="server" class="form-control input-sm"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label for="txtEmailID" class="control-label"><span style="color: #FF0000">*</span>Circle Email-ID</label>
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon"><i class="fa fa-inbox"></i></span>
                                    <asp:TextBox ID="txtEmailID" runat="server" class="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-3">
                            </div>
                            <div class="col-sm-9">
                                <asp:LinkButton ID="lnkSubmit" runat="server" CssClass="btn btn-sm btn-dark" Visible="true" OnClick="lnkSubmit_Click"><i class="fa fa-save">&nbsp;Submit</i></asp:LinkButton>
                                <asp:LinkButton ID="lnkUpdate" runat="server" CssClass="btn btn-sm btn-dark" Visible="False" OnClick="lnkUpdate_Click"><i class="fa fa-save">&nbsp;Update</i></asp:LinkButton>
                                <asp:LinkButton ID="lnkReset" runat="server" CssClass="btn btn-sm btn-warning" OnClick="lnkReset_Click"><i class="fa fa-archive">&nbsp;Reset</i></asp:LinkButton>
                                <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger" Font-Size="Medium"></asp:Label>
                            </div>
                        </div>
                        <ul class="bottom_notes">
                            <li><span style="color: #FF0000">*</span> marked fields are mandatory</li>
                        </ul>
                        <div class="clearfix"></div>
                        <br />
                        <div class="panel panel-success" style="overflow: hidden;">
                            <div class="panel-heading bg-primary" style="font-weight: bold; color: white; font-size: 15px;">
                                Circle Master Details
                        <p style="float: right;">
                            <asp:LinkButton ID="lnkSearch" runat="server" CssClass="btn btn-sm btn-danger" OnClick="lnkSearch_Click"><i class="fa fa-search">&nbsp;Search</i></asp:LinkButton>
                        </p>
                                <p style="float: right;">
                                    <asp:TextBox ID="txtSolidSearch" runat="server" class="form-control input-sm" placeHolder="Enter Branch SolID"></asp:TextBox>
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
                                                            <asp:LinkButton ID="lnkEdit" runat="server" CssClass="btn btn-sm btn-danger" CommandArgument='<%#Eval("BRN_ID") + "~" + Eval("ZONE") + "~"+ Eval("BRN_SOLID")+"~"+Eval("BRN_NAME")+"~"+Eval("BRN_ADDRESS")+"~"+Eval("BRN_STATE")+"~"+Eval("BRN_ACTIVE")+"~"+Eval("BRN_EMAILID")%>' CommandName="SELECT"><i class="fa fa-edit">&nbsp;Edit</i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="BRN_SOLID" HeaderText="Sol ID" />
                                                    <asp:BoundField DataField="BRN_NAME" HeaderText="Circle Name" />
                                                    <asp:BoundField DataField="BRN_EMAILID" HeaderText="Circle Email" />
                                                    <asp:BoundField DataField="ZONE" HeaderText="Circle Zone" />
                                                    <asp:BoundField DataField="ACTIVE_STATUS" HeaderText="Active" />
                                                    <asp:BoundField DataField="ENTRYBY" HeaderText="Entry By" />
                                                    <asp:BoundField DataField="ENTRYDATE" HeaderText="Entry Date" />
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
