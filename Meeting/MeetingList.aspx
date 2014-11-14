<%@ Page Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true"
    CodeBehind="MeetingList.aspx.cs" Inherits="Meeting.MeetingList" %>

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
        var pageSize = 20; //每页显示数量

        jQuery(document).ready(function () {
            getCluesList(1, true); //列表页初始化
        });

        function getCluesList(pageIndex, isInit) {
            var url = "";
            url += "&pageIndex=" + pageIndex;
            url += "&pageSize=" + pageSize;

            $.AjaxGetXml({
                url: '../Ajax/List.ashx?Action=GetBaiduList' + url,
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
