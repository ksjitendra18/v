<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="frmExcelPF.aspx.cs" Inherits="VMISP.Upload.frmExcelPF" %>


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
                    PF Excel Upload
                </div>
                <br />
                <div class="col-sm-12 alert alert-dark">
                    <div class="form-group row">
                        <div class="col-sm-4">
                            <label for="ddlTableName"><span style="color: #FF0000">*</span>Excel Upload For</label>
                            <asp:DropDownList ID="ddlTableName" runat="server" CssClass="form-control input-sm">
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Complaint" Value="COMPLAINT"></asp:ListItem>
                                <asp:ListItem Text="LODI Entry" Value="LODI"></asp:ListItem>
                                <asp:ListItem Text="IAC Entry" Value="IAC"></asp:ListItem>
                                <asp:ListItem Text="MISC" Value="MISC"></asp:ListItem>
                                <asp:ListItem Text="Sanction for Investigation" Value="SANCTION_FOR_INVESTIGATION"></asp:ListItem>
                                <asp:ListItem Text="Sanction for Prosecution" Value="SANCTION_FOR_PROSECUTION"></asp:ListItem>
                                <asp:ListItem Text="Vigilance" Value="VIGILANCE"></asp:ListItem>
                                <asp:ListItem Text="Vigilance MISC" Value="VIGILANCEMIS"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-4">
                            <label for="fileUpload"><span style="color: #FF0000">*</span>Select File</label>
                            <asp:FileUpload ID="fileUpload" runat="server" onchange="return validateFileExtension(this)" CssClass="form-control input-sm" />
                        </div>
                        <div class="col-sm-4" style="padding-top: 27px;">
                            <asp:LinkButton ID="lnkExcel" runat="server" Text="Download Excel Format for PF Upload" OnClick="lnkExcel_Click" CssClass="btn btn-danger btn-sm"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-3">
                        </div>
                        <div class="col-sm-1">
                            <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" CssClass="btn btn-success btn-sm" />
                        </div>
                        <div class="col-sm-2">
                            <asp:LinkButton ID="lnkPrint_Excel" runat="server" Text="Excel Print" OnClick="lnkPrint_Excel_Click" Visible="false" CssClass="btn btn-info btn-sm"></asp:LinkButton>&nbsp;
                        </div>
                        <div class="col-sm-2">
                            <asp:LinkButton ID="lnkPrint_PDF" runat="server" Text="PDF Print" OnClick="lnkPrint_PDF_Click" Visible="false" CssClass="btn btn-primary btn-sm"></asp:LinkButton>
                        </div>
                        <div class="col-sm-4">
                            <asp:Label ID="lblMsg" runat="server" CssClass="label label-danger"></asp:Label>
                        </div>
                    </div>
                    <br />
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvMain" runat="server" CssClass="table input-sm table-bordered table-condensed">
                                <Columns>
                                    <asp:BoundField DataField="ROWNO" HeaderText="S No." />
                                    <asp:BoundField DataField="PFNUMBER" HeaderText="PF Number" />
                                    <asp:BoundField DataField="NAME" HeaderText="Name/Accused" />
                                    <asp:BoundField DataField="RNO" HeaderText="R No." SortExpression="RNO" />
                                    <asp:BoundField DataField="ACCOUNTNAME" HeaderText="Account Name" />
                                    <asp:BoundField DataField="COMPRECDATE" HeaderText="Comp Rec Date" />
                                    <asp:BoundField DataField="STATUSCODE" HeaderText="Status Code" />
                                    <asp:BoundField DataField="CIRCLEOFFICE" HeaderText="Circle Office" />
                                    <asp:BoundField DataField="AMOUNT" HeaderText="Amount" />
                                    <asp:BoundField DataField="STATUS" HeaderText="Status" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
