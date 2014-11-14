(function ($) {


    var findLinkUrlAppend = function () {
        var fData = $("#form1").serializeArray();
        var queryString = $.map(fData, function (val, i) { return val.name + "|" + val.value });
        queryString = encodeURI(queryString);
        $("#divList .sub a").each(function () {
            var href = $(this).attr("href");
            $(this).attr("href", href + "&qsr=" + queryString);
        });
    }

    //url 、elementID 必填
    $.AjaxGetXml = function (_options) {
        //var options = { url: "", error: null, success: null, htmlID: null, popupID: "divDetail", resultNode: "resultData", data: null };
        var options = $.extend({
            url: "",
            data: null,
            error: null,
            success: null,
            htmlID: null,      //要填充 HTML 的标签ID
            isShowLoad: false, //当 “htmlID ”的值有效的时候且此值true才生效。 是否显示小的加载标签
            popupID: "divDetail", //要弹出的标签ID
            resultNode: "resultData", //服务器返回的Xml 节点名称
            showMask: false, //弹出加载存
            isFill: false//此属性当定义了success事件时才有效。当有success事件时是否将HTML填充到属性“htmlID” 的标签里
        }, _options || {});

        if (options.showMask) {
            if (!options.htmlID)
                showMask();
            else
                showMask(options.htmlID);
        } else if ($("#" + options.popupID).css("display") == "none")
            showPopup(options.popupID, 0);

        //是否显示小的加载标签
        if (options.htmlID && options.isShowLoad) {
            $("#" + options.htmlID).showLoad();
        }
        $.ajax({
            url: options.url, type: 'POST', DataType: 'xml', timeout: 600000, data: options.data,
            error: function (obj, err) {
                if ($.isFunction(options.error)) {
                    options.error(obj);
                } else {
                    hidePopup(options.popupID);
                    //状态为0时说明是强制中断ajax，这种情况下很有可能是在加载数据时用户去点另外的按钮发生的，所以不做提示。
                    if (obj.status != 0)
                        Boxy.alert('请求错误！', null, { title: '提示', closeable: true });
                }
            },
            success: function (xml) {
                $(xml).find('xmlRoot').each(function () {
                    var result = $(this).find("Result").text();
                    if (result != '0') {
                        //给处理请求成功，但有返回失败的情况给与相应的提示
                        successResult(result, $(this).find("Description").text());

                        hidePopup(options.popupID);
                        if (options.showMask)
                            hideMask();
                        return;
                    }

                    if ($.isFunction(options.success)) {
                        if (options.isFill) {
                            var rowHtml = $(this).find(options.resultNode).text();
                            rowHtml = XmlFormat(rowHtml);
                            if (options.showMask)
                                hideMask();
                            setTimeout(function () {
                                $("#" + options.htmlID).html($.trim(rowHtml));
                                options.success(xml);
                            }, 100);
                        } else { options.success(xml); }
                    } else {
                        var rowHtml = $(this).find(options.resultNode).text();
                        rowHtml = XmlFormat(rowHtml);
                        if (options.showMask)
                            hideMask();
                        setTimeout(function () {
                            $("#" + options.htmlID).html($.trim(rowHtml));
                        }, 500);
                    }
                });
            }
        });
    };

    $.AjaxGetJson = function (_options) {
        var options = $.extend({
            url: "",
            error: null,
            success: null,
            resultNode: "resultData",
            data: null
        }, _options);

        $.ajax({
            url: options.url, type: 'POST', DataType: 'xml', timeout: 600000, data: options.data,
            error: function (xml, err) {
                if ($.isFunction(options.error)) {
                    options.error(xml);
                } else { }
            },
            success: function (xml) {
                var result = $(xml).find("Result").text();
                if (result != '0') {

                    successResult(result, $(this).find("Description").text());

                    if (options.showMask)
                        hideMask();
                    return;
                }
                if ($.isFunction(options.success)) {
                    try {
                        var json = $.parseJSON($(xml).find(options.resultNode).text());
                    } catch (e) {
                    }
                    if (options.showMask)
                        hideMask();
                    setTimeout(function () {
                        options.success(json);
                    }, 10);

                }

            }
        });
    }
    //执行事件时用
    $.AjaxExecute = function (_options) {
        var options = $.extend({
            url: "", error: null, success: null, data: null, DataType: "xml", showMask: false
        }, _options);
        if (options.showMask) {
            showMask();
        }
        $.ajax({
            url: options.url, type: 'POST', DataType: options.DataType, timeout: 600000, data: options.data,
            error: function (result, err) {
                if ($.isFunction(options.error)) {
                    options.error(xml);
                    if (options.showMask)
                        hideMask();
                } else {
                    Boxy.alert('请求异常', null, { title: '提示', closeable: true });
                    if (options.showMask)
                        hideMask();
                }
            },
            success: function (resultData) {
                if ($.isFunction(options.success)) {
                    if (options.showMask)
                        hideMask();
                    setTimeout(function () {
                        options.success(resultData);
                    }, 10);

                }
            }
        });
    }

    var successResult = function (result, errMsg) {
        switch (result) {
            case "998": //未登录
                Boxy.alert('未登录。', function () { logOut(); }, { title: '提示', closeable: true });
                break;
            default:
                Boxy.alert('获取失败。' + errMsg, null, { title: '提示', closeable: true });
                break;
        }
    }

})(jQuery);
