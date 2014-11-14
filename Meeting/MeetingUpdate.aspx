<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="MeetingUpdate.aspx.cs" Inherits="Meeting.MeetingUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Scripts/Validation/css/validationEngine.jquery.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/Validation/languages/jquery.validationEngine-zh_CN.js" type="text/javascript"></script>
    <script src="../Scripts/Validation/jquery.validationEngine.js" type="text/javascript"></script>
    <script src="../DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/MyAjax.js"></script>
    <script src="../Scripts/threeSelect.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="../Scripts/VocationData.js" type="text/javascript"></script>
    <script src="../Scripts/autocompleteIndustry.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form">
        <ul class="form-header">
            <li class="selected">修改记录</li>
        </ul>
        <form id="form1" style="position: relative" runat="server">
        <table cellpadding="0" cellspacing="0" class="form-content">
            <tr>
                <th width="10%">
                    客户ID：
                </th>
                <td width="20%">
                    <asp:TextBox ID="txtCustomerId" CssClass="text" runat="server" />
                </td>
                <th width="10%">
                    客户名称：
                </th>
                <td width="70%">
                    <asp:TextBox ID="txtCustomerName" Width="300" CssClass="text" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="10%">
                    行业：
                </th>
                <td width="20%">
                    <asp:TextBox ID="txtIndustryDes" CssClass="text" runat="server" />
                </td>
                <th width="10%">
                    地域：
                </th>
                <td width="70%">
                    <asp:TextBox ID="txtAreaDes" CssClass="text" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="10%">
                    联系人：
                </th>
                <td width="20%">
                    <asp:TextBox ID="txtContactName" CssClass="text" runat="server" />
                </td>
                <th width="10%">
                    联系方式：
                </th>
                <td width="70%">
                    <asp:TextBox ID="txtContact" CssClass="text" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="10%">
                    参会情况：
                </th>
                <td width="20%">
                    <asp:RadioButtonList ID="radAttend" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="是" Value="1" />
                        <asp:ListItem Text="否" Value="0" />
                    </asp:RadioButtonList>
                </td>
                <th width="10%">
                    参会时间：
                </th>
                <td width="70%">
                    <asp:TextBox ID="txtAttendTime" CssClass="text" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="10%">
                    是否库外线索：
                </th>
                <td width="20%">
                    <asp:RadioButtonList ID="radIsExternal" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="是" Value="1" />
                        <asp:ListItem Text="否" Value="0" />
                    </asp:RadioButtonList>
                </td>
                <th width="10%">
                    状态：
                </th>
                <td width="70%">
                    <asp:TextBox ID="txtStatus" CssClass="text" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="10%">
                    签单产品：
                </th>
                <td width="20%">
                    <asp:TextBox ID="txtProductName" CssClass="text" runat="server" />
                </td>
                <th width="10%">
                    签单金额：
                </th>
                <td width="70%">
                    <asp:TextBox ID="txtProductAmount" CssClass="text" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="10%">
                    签单时间：
                </th>
                <td width="20%">
                    <asp:TextBox ID="txtSignedTime" CssClass="text" runat="server" />
                </td>
                <th width="10%">
                </th>
                <td width="70%">
                </td>
            </tr>
        </table>
        <div class="form-button">
            <asp:Button Text="修改" ID="btnSubmit" CssClass="save" runat="server" OnClick="btnSubmit_Click" />
        </div>
        </form>
    </div>
</asp:Content>
