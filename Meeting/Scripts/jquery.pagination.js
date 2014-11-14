/**
* This jQuery plugin displays pagination links inside the selected elements.
*
* @author Gabriel Birke (birke *at* d-scribe *dot* de)
* @version 1.2
* @param {int} maxentries Number of entries to paginate
* @param {Object} opts Several options (see README for documentation)
* @return {Object} jQuery Object
*/
jQuery.fn.pagination = function (maxentries, opts) {
    opts = jQuery.extend({
        items_per_page: 10,
        num_display_entries: 10,
        current_page: 0,
        num_edge_entries: 0,
        link_to: "#",
        prev_text: "上一页",
        next_text: "下一页",
        go_text: "Go",
        ellipse_text: "...",
        prev_show_always: true,
        next_show_always: true,
        callback: function () { return false; }
    }, opts || {});

    return this.each(function () {
        /**
        * 计算最大分页显示数目
        */
        function numPages() {
            return Math.ceil(maxentries / opts.items_per_page);
        }
        /**
        * 极端分页的起始和结束点，这取决于current_page 和 num_display_entries.
        * @返回 {数组(Array)}
        */
        function getInterval() {
            var ne_half = Math.ceil(opts.num_display_entries / 2);
            var np = numPages();
            var upper_limit = np - opts.num_display_entries;
            var start = current_page > ne_half ? Math.max(Math.min(current_page - ne_half, upper_limit), 0) : 0;
            var end = current_page > ne_half ? Math.min(current_page + ne_half, np) : Math.min(opts.num_display_entries, np);
            return [start, end];
        }

        /**
        * 分页链接事件处理函数
        * @参数 {int} page_id 为新页码
        */
        function pageSelected(page_id, evt) {

            if (isNaN(page_id)) {
                Boxy.alert('非法页数', null, { title: '提示', closeable: true });
                return;
            }
            var np = numPages();
            if (page_id < 0 || page_id > np - 1) {
                Boxy.alert('非法页数', null, { title: '提示', closeable: true });
                return;
            }
            current_page = page_id;
            drawLinks();
            var continuePropagation = opts.callback(page_id, panel);
            if (!continuePropagation) {
                if (evt.stopPropagation) {
                    evt.stopPropagation();
                }
                else {
                    evt.cancelBubble = true;
                }
            }
            return continuePropagation;
        }

        /**
        * 此函数将分页链接插入到容器元素中
        */
        function drawLinks() {
            panel.empty();
            var interval = getInterval();
            var np = numPages();
            if (np <= 0) {
                var span = jQuery("<span style='color:red' >暂无数据......</span>");
                panel.append(span);
                return;
            }
            // 这个辅助函数返回一个处理函数调用有着正确page_id的pageSelected。

            var getClickHandler = function (page_id) {

                return function (evt) { return pageSelected(page_id, evt); }
            }

            var getGoHandler = function () {

                return function (evt) { return pageSelected(jQuery("#txtPageNum").val() - 1, evt); }
            }
            //辅助函数用来产生一个单链接(如果不是当前页则产生span标签)
            var appendItem = function (page_id, appendopts) {
                page_id = page_id < 0 ? 0 : (page_id < np ? page_id : np - 1); // 规范page id值
                appendopts = jQuery.extend({ text: page_id + 1, classes: "" }, appendopts || {});
                if (page_id == current_page) {
                    var lnk = jQuery("<span>" + (appendopts.text) + "</span>");
                } else {
                    var lnk = jQuery("<a>" + (appendopts.text) + "</a>")
						.bind("click", getClickHandler(page_id))
						.attr('href', opts.link_to.replace(/__id__/, page_id));
                }

                if (appendopts.classes) {
                    lnk.addClass(appendopts.classes);
                } else {
                    if (page_id == current_page)
                        lnk.addClass("cur");
                    else {
                        lnk.addClass("");
                    }

                }
                panel.append(lnk);
            }

            var desc = jQuery("<span class='txt'>共<font color='#FF6600'>" + maxentries + "</font>条 第" + (current_page + 1) + "页/共" + np + "页</span>");
            panel.append(desc);

            // 产生"Previous"-链接

            if (opts.prev_text && (current_page > 0 || opts.prev_show_always)) {
                appendItem(current_page - 1, { text: opts.prev_text, classes: "pav" });
            }
            // 产生起始点
            if (interval[0] > 0 && opts.num_edge_entries > 0) {
                var end = Math.min(opts.num_edge_entries, interval[0]);
                for (var i = 0; i < end; i++) {
                    appendItem(i);
                }
                if (opts.num_edge_entries < interval[0] && opts.ellipse_text) {
                    jQuery("<span class='m'>" + opts.ellipse_text + "</span>").appendTo(panel);
                }
            }
            // 产生内部的些链接
            for (var i = interval[0]; i < interval[1]; i++) {
                appendItem(i);
            }
            // 产生结束点
            if (interval[1] < np && opts.num_edge_entries > 0) {
                if (np - opts.num_edge_entries > interval[1] && opts.ellipse_text) {
                    jQuery("<span class='m'>" + opts.ellipse_text + "</span>").appendTo(panel);
                }
                var begin = Math.max(np - opts.num_edge_entries, interval[1]);
                for (var i = begin; i < np; i++) {
                    appendItem(i);
                }

            }
            // 产生 "Next"-链接
            if (opts.next_text && (current_page < np - 1 || opts.next_show_always)) {
                appendItem(current_page + 1, { text: opts.next_text, classes: "next" });
            }

            var more = jQuery("<a href='javascript:void(0);' class='more' onclick='showJump();'></a>");
            panel.append(more);

            var goDiv = jQuery("<div class='jump' id='divJump' style='display:none;'></div>");


            var goInput = jQuery("<input onfocus=\"setEnterEvent('linkGoPage');\"  type='text' class='text' id='txtPageNum'  name='txtPageNum' />");


            goDiv.append(goInput);

            var pageIndex = current_page - 0 + 1;
            //jQuery("#txtPageNum").val(pageIndex);



            var goLnk = jQuery("<a herf='javascript:void(0);'style='cursor: pointer;'  class='jump-sub' id='linkGoPage'>" + opts.go_text + "</a>")
						.bind("click", getGoHandler());
            goDiv.append(goLnk);

            panel.append(goDiv);
        }

        //从选项中提取current_page
        var current_page = opts.current_page;
        //创建一个显示条数和每页显示条数值
        maxentries = (!maxentries || maxentries < 0) ? 1 : maxentries;
        opts.items_per_page = (!opts.items_per_page || opts.items_per_page < 0) ? 1 : opts.items_per_page;
        //存储DOM元素，以方便从所有的内部结构中获取
        var panel = jQuery(this);
        // 获得附加功能的元素
        this.selectPage = function (page_id) { pageSelected(page_id); }
        this.prevPage = function () {
            if (current_page > 0) {
                pageSelected(current_page - 1);
                return true;
            }
            else {
                return false;
            }
        }
        this.nextPage = function () {
            if (current_page < numPages() - 1) {
                pageSelected(current_page + 1);
                return true;
            }
            else {
                return false;
            }
        }
        // 所有初始化完成，绘制链接
        drawLinks();
        // 回调函数

    });
}

/*
* 赋值查询参数
* 2014/05/06
* 
* 使用：
*  $("from1").setQueryString();
* 
*/
jQuery.fn.setQueryString = function (htmlId) {
    var form = $(this);
    var req = comm.GetRequest();
    var qsr = req["qsr"];
    qsr = decodeURI(qsr);
    qsr = unescape(qsr);
    qsr = qsr.split(",");
    for (var i = 0; i < qsr.length; i++) {
        var item = qsr[i];
        var arr = item.split("|");
        var val = arr[1] === undefined ? "" : arr[1] === NaN ? "" : arr[1];
        form.find("[name=" + arr[0] + "]").val(val);
    }

    form.findLinkAppendQueryString(htmlId, form.attr("id"));
}
/*
* 把查询条件加到URL后 
* 参数名：qsr=...
* 2014/05/06
* 使用：
*/
jQuery.fn.findLinkAppendQueryString = function (htmlId, formId) {
    $("#" + htmlId).on("click", ".sub a", function () {
        var fData = $("#" + formId).serializeArray();
        var queryString = $.map(fData, function (val, i) { return val.name + "|" + $.trim(val.value) });
        queryString = encodeURI(queryString);
        var href = $(this).attr("href");
        $(this).attr("href", href + "&qsr=" + queryString);
    })
    //    $("#" + htmlId + " .sub a").each(function () {
    //     
    //    });
}
/*
* 表头标签排序
* 2013/09/11
* 
* 使用：
*  $("from1").sortByAjax().Init(options);
*  listTableId 是数据table ID
*  如果一个界面有多个列表时最好加上 listTableId 参数
*  如只有一个也可以加上，速度更快
*/
jQuery.fn.sortByAjax = function (listTableId) {
    var _sort = {}, _form = $(this), _listTableId = listTableId || "";
    _listTableId = typeof (_listTableId) === "object" ? $(_listTableId).attr("id") : _listTableId;

    _sort.Init = function (options) {
        if ($.isFunction(options)) {
            tableId: null,
            options.callback = options;
        } else {
            options = jQuery.extend({
                callback: null
            }, options || {});
        }

        if ($("#sort_orderField,#sort_orderType").length <= 0) {
            _form.append("<input type='hidden' name='orderField' id='sort_orderField' />");
            _form.append("<input type='hidden' name='orderType' id='sort_orderType' />");
        }

        _listTableId = _listTableId || "";
        var listTb = $("#" + _listTableId);
        if (listTb.length > 0) {
            listTb = listTb.find(".table-headerCell a[orderField]");
        } else {
            listTb = $(".table-headerCell a[orderField]");
        }
        listTb.click(function () { _sort.setSort(this, options); });
    }
    //排序赋值事件
    _sort.setSort = function (obj, options) {
        obj = $(obj);
        var _ofVal = obj.attr("orderField")
            , _otVal = obj.attr("orderType")
            , _sof = $("#sort_orderField")
            , _sot = $("#sort_orderType");

        if (!_ofVal) return;
        var stVal = $.trim(_sot.val()), sfVal = $.trim(_sof.val());

        if (!stVal && !sfVal) {
            //读取可能存在的默认排序字段和排序类型
            var otA = obj.parents("tr").find("a[orderType]");
            if (otA) {
                stVal = otA.attr("orderType");
                sfVal = otA.attr("orderField");
            }
        }

        _sot.val(sfVal == $.trim(_ofVal) ? (stVal == "asc" ? "desc" : "asc") : "asc");
        _sof.val(_ofVal || "");

        if (options) {
            if ($.isFunction(options.callback)) {
                options.callback(); //parseInt(obj.attr("pageIndex")) 这可不用传pageIndex
            }
        }

    }
    return _sort;
}


var orderField = "";
var orderType = "";

function sequenceSet(field, type) {
    orderField = field;
    orderType = type;
}

function setPagination(objId, totalCount, pageSize, callBack) {

    $("#" + objId).pagination(totalCount, {
        num_edge_entries: 1, //边缘页数
        num_display_entries: 3, //主体页数
        callback: callBack,
        items_per_page: pageSize, //每页显示项
        prev_text: "上一页",
        next_text: "下一页",
        go_text: "Go"
    });
}




