/**
Update:

添加 showPopup弹出层绑定Esc退出
作者：yangxiao 2013/09/09

修改TabsContentShow 方法，兼容新的样式
2013/11/10


2014/05/23修改 弹出框的坐标问题
*/
(function ($) {
    //在弹出层切换的显示加载小图片
    $.fn.showLoad = function () {

        if (this.html) { this.html(createLoadHtml()); }
    }
    var createLoadHtml = function () {
        //<div style="text-align: center;"><img src="../images/loading.gif" alt="加载中" height="16" width="16" /></div>
        return $("<div>").css({ "text-align": "center", "width": "100%", id: "_div_showLoad" }).addClass("showLoad_mini")
        .append("<img src='../images/loading.gif' alt='加载中' height='25' width='25' />");
    }
    $.fn.setEnterEvent = function (option) {
        if ($(this).submit) {
            $(this).submit(function () { return false; });
        }
        //event.which
        $(this).keydown(function (event) {
            if (event.which == 13) {
                arguments;
                var type = typeof (option);
                if (type === typeof ("")) {
                    $("#" + option).click();
                } else if (type === "function") {
                    option();
                } else if (type === "object") {
                    $(option).click();
                }
            }
        });
    }

    $.fn.ToggleTabsByUL = function (tabsDivId, defShowTableId) {
        var ul = $(this), tabs = $("#" + tabsDivId);
        tabs.find("table").hide();
        $("#" + defShowTableId).show();
        ul.find("li").click(function () {
            ul.find("li").removeClass('selected');

            $(this).addClass('selected');
            var tabId = $(this).attr("for");
            tabs.find("table").hide();
            $("#" + tabId).show();
        });

    }

})(jQuery);


var $mask = null;
function showPopup(id, args, callback) {

    var body = $(document.body);
    if (!$mask) {
        $mask = $('<div>')
                .addClass("mask")
                .css('position', 'absolute')
                .css('width', $(document).width())
                .css('height', $(document).height())
                .css('background-color', 'black')
                .css('-moz-opacity', '0.35')
                .css('filter', 'alpha(opacity=65)')
                .css('opacity', '0.35')
                .css('left', '0px')
                .css('top', '0px')
                .css('zIndex', '1000')
                .css('display', 'none');
        $mask.appendTo(body);
    } else {
        $mask.css("background-image", "");
    }
    //绑定Esc按钮
    body.bind('keyup.mask', function (evt) {
        //防止和弹出框冲突
        if ($(".boxy-wrapper").length > 0) return;

        var key = evt.which || evt.keyCode;
        //alert(key);
        if (key == 27) {
            hidePopup(id, callback);
            //解除Esc绑定
            body.unbind('keyup.mask');
        }
    });

    var popup = $('#' + id);
    var _top = ((body.innerHeight() - popup.outerHeight()) / 2);
    if (_top > 200)
        _top = 200;
    var topCount = Math.max(document.body.scrollTop, document.documentElement.scrollTop);
    if (topCount > 0) {
        _top = _top + topCount;
    }

    _top = Math.abs(_top);
    popup.css('left', (body.scrollLeft() + (body.innerWidth() - popup.outerWidth()) / 2) + 'px')
        .css('top', _top + 'px')
        .css('zIndex', (parseInt($mask.css('zIndex'), 10) + 1).toString());

    //body.css('overflow', 'hidden');
    //popup.attr('args', args);
    $mask.fadeIn();
    if (callback)
        popup.fadeIn('normal', callback);
    else
        popup.fadeIn();
}
function maskTop(id) {
    var popup = $('#' + id);
    if ($mask) {
        $mask.css('zIndex', (parseInt(popup.css('zIndex'), 10) + 1).toString());
    }
}
function popupTop(id) {
    var popup = $('#' + id);
    if ($mask) {
        popup.css('zIndex', (parseInt($mask.css('zIndex'), 10) + 1).toString());
    }
}
var needLoading = false;
var $loading = null;

function showMask(objId) {
    needLoading = true;
    var body;

    if (objId) {
        $loading = $('<div>')
                .css('position', 'relative')
                .css('width', "100%")
                .css('background-color', '#C7C4BF')
                .css('background-image', 'url(../images/loading.gif)')
                .css('background-repeat', 'no-repeat')
                .css('background-position', 'center center')
                .css('background-size', '72px 72px')
                .css('-moz-opacity', '0.45')
                .css('filter', 'alpha(opacity=45)')
                .css('opacity', '0.45')
                .css('left', '0px')
                .css('top', '0px')
                .css('zIndex', '1000')
                .css('display', 'none');
        if ($('#' + objId).height() && $('#' + objId).height() > 100)
            $loading.height($('#' + objId).height() + 'px');
        else
            $loading.height('150px');


        if ($('#' + objId).is('tbody')) {
            var table = $('#' + objId).parent()[0];
            $(table).after($loading);

        } else
            $('#' + objId).html($loading);


        setTimeout(function () {
            if (needLoading)
                $loading.fadeIn("normal");
        }, 500);

    }
    else {
        body = $(document.body);

        $loading = $('<div>')
                .css('position', 'absolute')
                .css('width', '100%')
                .css('height', '100%')
                .css('background-color', '#C7C4BF')
                .css('background-image', 'url(../images/loading.gif)')
                .css('background-repeat', 'no-repeat')
                .css('background-position', 'center center')
                .css('background-size', '72px 72px')
                .css('-moz-opacity', '0.45')
                .css('filter', 'alpha(opacity=45)')
                .css('opacity', '0.45')
                .css('left', '0px')
                .css('top', '0px')
                .css('zIndex', '1000')
                .css('display', 'none');
        $loading.appendTo(body);
        setTimeout(function () {
            if (needLoading)
                $loading.fadeIn("normal");
        }, 600);
    }
}

function hideMask() {
    needLoading = false;
    $loading.fadeOut();
}
function hidePopup(id, callback) {
    var popup = $('#' + id);
    popup.fadeOut();

    if (!$mask) {
        return;
    }
    if (callback)
        $mask.fadeOut('normal', callback);
    else
        $mask.fadeOut();
}


var SetMaskDisplay = function (isShow) {
    if ($mask) {
        isShow ? $($mask).show() : $($mask).hide();
    }
}



//弹出信息框 tabs 切换显示事件
var TabsContentShow = function (ulId, showPopupId, showId, exceptionId) {
    var _tb = $("#" + ulId);
    $("#" + showPopupId + " .tc_title ul").hide();
    _tb.show();
    if ($("#" + showPopupId).css("display") == "none")
        showPopup(showPopupId, 0);
    var con = $("#" + showPopupId + " .tc_content");
    //con.find("table").hide();
    //con.find("div").hide();
    con.children().hide();
    if (exceptionId)
        con.find("#" + exceptionId).show();
    $("#" + showId).show().find("table,div").show();
    _tb.find("li").removeClass('selected');
    _tb.find("li[for='" + showId + "']").addClass('selected');

}