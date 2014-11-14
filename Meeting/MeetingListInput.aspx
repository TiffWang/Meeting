<%@ Page Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="MeetingListInput.aspx.cs" Inherits="Meeting.MeetingListInput" %>

<%@ Register Src="~/UserControls/Uploadify.ascx" TagName="Uploadify" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Scripts/Validation/css/validationEngine.jquery.css" rel="stylesheet"
        type="text/css" />
    <link href="../Scripts/Validation/css/template.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/Validation/languages/jquery.validationEngine-zh_CN.js" type="text/javascript"></script>
    <script src="../Scripts/Validation/jquery.validationEngine.js" type="text/javascript"></script>
    <link href="../UserControls/Uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.lightbox.css" rel="stylesheet" type="text/css" />
    <script src="../UserControls/Uploadify/swfobject.js" type="text/javascript"></script>
    <script src="../UserControls/Uploadify/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.lightbox.5.js" type="text/javascript"></script>
    <link href="css/pagination.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/MyAjax.js" type="text/javascript"></script>
    <script src="Scripts/jquery.pagination.js" type="text/javascript"></script>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery("#form1").validationEngine();
        });

        $(document).ready(function () {
            $("#readPath").hide();

            myUploadify.Init({
                fileDesc: "请选择excel文件",
                fileExt: "*.xls;*.xlsx;",
                scriptData: { 'Type': 'OnlineData', 'Inventory': 'N' },
                multi: false,
                //auto: true,
                // hideButton: true,
                onComplete: function (event, queueId, fileObj, resultData, data) {
                    if (resultData.path != "") {
                        $("#txtPath").val(resultData.path);
                        $("#readPath").attr("href", resultData.path);
                        $("#readPath").show();
                    }
                }
            });

            if ($("#txtPath").val() != "") {
                $("#readPath").attr("href", $("#txtPath").val());
                $("#readPath").show();
            }
        });

        var tabsToggle = function (showId) {
            TabsContentShow("ul_upfiles", "div_box_upfiles", showId);
        }



        var pageSize = 10; //每页显示数量

        jQuery(document).ready(function () {
            getCluesList(1, true); //列表页初始化
        });

        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值
        }

        function getCluesList(pageIndex, isInit) {
            var url = "";
            url += "&pageIndex=" + pageIndex;
            url += "&pageSize=" + pageSize;
            url += "&activityId=" + getUrlParam('ActivityId');

            $.AjaxGetXml({
                url: '../Ajax/List.ashx?Action=GetCluesList' + url,
                data: $("#form1").serialize(),
                htmlID: "divList",
                resultNode: "divListHtml",
                isFill: true,
                popupID: false,
                showMask: true,
                success: function (xml) {
                    var totalCount = $(xml).find("totalCount").text();

                    if (isInit) {
                        setPagination('Pagination', totalCount, pageSize, pageselectCallback);
                    }

                    //初始化排序操作
                    $("#form1").sortByAjax().Init(function () {
                        getCluesList(pageIndex, false);
                    });

                    if (parseInt(totalCount) <= 0) {
                        $('#Pagination').html("<span style=\"color:red\">暂无数据......</span>");
                        //
                    }
                    hideMask();
                }
            });
        }

        //翻页事件，不需要初始化分页
        function pageselectCallback(page_index, jq) {
            getCluesList(page_index + 1, false);
            return false;
        }

        function Delete(id) {
            Boxy.confirm("是否删除?", function () { window.location.href = "MeetingListInput.aspx?Action=del&InviteID=" + id; }, "");
        }
    </script>
    <style type="text/css">
        .file_imgitem
        {
            position: relative;
            float: left;
            width: 110px;
            padding-bottom: 10px;
        }
        .file_imgitem span
        {
            left: 0px;
            position: absolute;
            background: black;
            color: white;
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tc_box" id="div_box_upfiles">
        <table class="aui_border">
            <tbody>
                <tr>
                    <td class="aui_nw">
                    </td>
                    <td class="aui_n">
                        <div class="tc_title">
                            <ul id="ul_upfiles">
                                <li class="selected" onclick="tabsToggle('div_con_upfiles');" for="div_con_upfiles">
                                    文件上传</li>
                            </ul>
                            <a href="#" class="colse" onclick="hidePopup('div_box_upfiles');"></a>
                        </div>
                    </td>
                    <td class="aui_ne">
                    </td>
                </tr>
                <tr>
                    <td class="aui_w">
                    </td>
                    <td class="aui_c">
                        <!--- tc_content --->
                        <div class="tc_content">
                            <div id="div_con_upfiles">
                                <uc1:Uploadify runat="server" ID="Uploadify1" />
                            </div>
                        </div>
                        <!--- tc_content --->
                    </td>
                    <td class="aui_e">
                    </td>
                </tr>
                <tr>
                    <td class="aui_sw">
                    </td>
                    <td class="aui_s">
                    </td>
                    <td class="aui_se">
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="form">
        <ul class="form-header">
            <li><a href="ActivityList.aspx">活动列表</a></li>
            <li class="selected">数据导入</li>
        </ul>
        <form id="form1" style="position: relative" runat="server">
        <br />
        <input type="hidden" runat="server" id="hidId" />
        <table class="form-content">
            <tr>
                <th width="10%">
                    活动：
                </th>
                <td width="90%">
                    <asp:DropDownList ID="drpActivity" Enabled="false" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th width="10%">
                </th>
                <td width="90%">
                    <input type="button" name="name" onclick="tabsToggle('div_con_upfiles');" class="save"
                        value="上传" />
                </td>
            </tr>
            <tr>
                <th>
                    路径：
                </th>
                <td>
                    <input type="text" id="txtPath" readonly="readonly" value="" class="text validate[required]"
                        style="width: 400px" runat="server" />
                    <a id="readPath" href="" class="" target="_blank" runat="server">查看</a>
                </td>
            </tr>
        </table>
        <div class="form-button">
            <asp:Button Text="导入" ID="btnSubmit" class="save" runat="server" OnClick="btnSubmit_Click" />
        </div>
        </form>
    </div>
    <div class="table">
        <ul class="form-header">
            <li class="selected">数据列表</li>
        </ul>
        <%--   <form id="form2" action="" onsubmit="return false">
        <div class="search">
            客户名称：<input id="txtCustomerName" name="customerName" type="text" class="text" />&nbsp;&nbsp;&nbsp;&nbsp;
            联系人：<input id="txtcontact" name="contact" type="text" class="text" />
            电话号码：<input id="txtphoneNo" name="phoneNo" type="text" class="text" />
            线索状态：
            <select runat="server" id="drpClueStatus" class="select">
            </select>
            产品需求：
            <select class="select" name="ProductRequire" id="drpProductRequire" runat="server">
            </select>
            <a href="javascript:void(0)" id="linkSearch" class="submit" onclick="getCluesList(1, true);">
                搜索</a></div>
        </form>--%>
        <div id="divList" style="max-width: 100%; overflow-y: auto;">
        </div>
        <div class="operate">
            <div id="Pagination" class="pager">
            </div>
        </div>
    </div>
</asp:Content>
