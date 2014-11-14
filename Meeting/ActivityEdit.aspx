<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="ActivityEdit.aspx.cs" Inherits="Meeting.ActivityEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Scripts/Validation/css/validationEngine.jquery.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/Validation/languages/jquery.validationEngine-zh_CN.js" type="text/javascript"></script>
    <script src="../Scripts/Validation/jquery.validationEngine.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/MyAjax.js"></script>
    <script src="../Scripts/threeSelect.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="../Scripts/VocationData.js" type="text/javascript"></script>
    <script src="../Scripts/autocompleteIndustry.js" type="text/javascript"></script>
    <script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery("#form1").validationEngine();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form">
        <ul class="form-header">
            <li><a href="ActivityList.aspx">活动列表</a></li>
            <li class="selected">添加活动</li>
        </ul>
        <form id="form1" style="position: relative" runat="server">
        <table cellpadding="0" cellspacing="0" class="form-content">
            <tr>
                <th width="10%">
                    活动分类：
                </th>
                <td width="20%">
                    <asp:DropDownList ID="drpType" runat="server">
                    </asp:DropDownList>
                </td>
                <th width="10%">
                </th>
                <td width="70%">
                </td>
            </tr>
            <tr>
                <th width="10%">
                    活动主题：
                </th>
                <td width="20%">
                    <asp:TextBox ID="txtName" Width="300" CssClass="text validate[required]" runat="server" />
                </td>
                <th width="10%">
                </th>
                <td width="70%">
                </td>
            </tr>
            <tr>
                <th width="10%">
                    起始时间：
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtStartTime" CssClass="text Wdate" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                        runat="server" />-<asp:TextBox ID="txtEndTime" CssClass="text Wdate" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                            runat="server" />
                </td>
            </tr>
            <tr>
                <th width="10%">
                    城市及地点：
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtCity" CssClass="text" runat="server" />
                    <asp:TextBox ID="txtAddress" CssClass="text" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="10%">
                    目标客户行业：
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtIndustDes" CssClass="text" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="10%">
                    预计参会企业数：
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtAttendTotal" CssClass="text" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="10%">
                    预计签单企业数：
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtSignedTotal" CssClass="text" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="10%">
                    预计签单金额：
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtSignedAmount" CssClass="text" runat="server" />
                </td>
            </tr>
        </table>
        <div class="form-button">
            <asp:Button Text="添加" ID="btnSubmit" CssClass="save" runat="server" OnClick="btnSubmit_Click" />
        </div>
        </form>
    </div>
</asp:Content>
