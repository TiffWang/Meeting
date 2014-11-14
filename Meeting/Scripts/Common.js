/**
* 公用方法

**/

function logOut() {
    top.location.href = '/signin.aspx';
}

function XmlFormat(xml) {
    var reg = new RegExp("&lt;", "g");
    xml = xml.replace(reg, "<");

    reg = new RegExp("&gt;", "g");
    xml = xml.replace(reg, ">");

    reg = new RegExp("&amp;", "g");
    xml = xml.replace(reg, "&");

    reg = new RegExp("&quot;", "g");
    xml = xml.replace(reg, "\"");

    reg = new RegExp("&apos;", "g");
    xml = xml.replace(reg, "'");
    return xml;
}

$('#tabs li a').click(function () {
    $('#tabs li a').removeClass('current');
    $(this).addClass('current');
});

function shrinkObj(link, objid) {

    if ($('#' + objid).is(":hidden")) {
        $('#' + objid).fadeIn();
        $('#' + link).html("收起");
    }
    else {
        $('#' + objid).fadeOut();
        $('#' + link).html("展开");
    }
}

function setEnterEvent(objid) {

    document.onkeydown = function (e) {
        var ev = document.all ? window.event : e;
        if (ev.keyCode == 13) {

            $('#' + objid).click();
        }
    }
}


//一些公用的方法。放这里面是为了方便。防止方法名冲突
var comm = new Object();
comm = {
    //动态加载JS
    loadJS: function (src) {
        var oHead = document.getElementsByTagName('HEAD').item(0)
    , oScript = document.createElement("script");
        oScript.type = "text/javascript";
        oScript.src = src;
        oHead.appendChild(oScript);
    },
    GetRequest: function () {
        var url = location.search; //获取url中"?"符后的字串
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = (strs[i].split("=")[1]);
            }
        }
        return theRequest;
    },
    //动态加载样式文件
    loadStyle: function (src) {
        var oHead = document.getElementsByTagName('HEAD').item(0)
    , oStyle = document.createElement("link");
        oStyle.type = "text/css";
        oStyle.src = src;
        oStyle.rel = "stylesheet";
        oHead.appendChild(oStyle);
    },

    //操作键
    isSpecialKey: function (key) {
        //8:backspace; 46:delete; 37-40:arrows; 36:home; 35:end; 9:tab; 13:enter
        return key == 8 || key == 46 || (key >= 37 && key <= 40) || key == 35 || key == 36 || key == 9 || key == 13;
    },

    //控件文本的长度处理操作键
    controlTextLen: function (valLen, maxSize, key) {
        var retVal = true;
        if (valLen > maxSize) {
            retVal = false;
            retVal = comm.isSpecialKey(key);
        }
        return retVal;
    }, GetCurDate: function () {
        var d = new Date();
        var vYear = d.getFullYear();
        var vMon = d.getMonth() + 1;
        var vDay = d.getDate();
        return vYear + "-" + vMon + "-" + vDay;
    },
    //显示文本的长度事件
    showTextLenHandle: function (event) {
        var obj = $(this), maxLen = parseInt(obj.attr("maxlength"));
        var len = obj.val().length, showObj = obj.next("label");
        showObj.text(maxLen + "/" + len);
        if (showObj.length <= 0)
            obj.next().next().text(maxLen + "/" + len);
        obj = showObj = null;
        return comm.controlTextLen(len, maxLen, event.which);
    },

    //图片等比显示
    AutoResizeImage: function (maxWidth, maxHeight, objImg) {
        var img = new Image();
        img.onload = function () {
            var hRatio;
            var wRatio;
            var Ratio = 1;
            var w = this.width;
            var h = this.height;
            wRatio = maxWidth / w;
            hRatio = maxHeight / h;
            if (maxWidth == 0 && maxHeight == 0) {
                Ratio = 1;
            } else if (maxWidth == 0) {// 
                if (hRatio < 1) Ratio = hRatio;
            } else if (maxHeight == 0) {
                if (wRatio < 1) Ratio = wRatio;
            } else if (wRatio < 1 || hRatio < 1) {
                Ratio = (wRatio <= hRatio ? wRatio : hRatio);
            }
            if (Ratio < 1) {
                w = w * Ratio;
                h = h * Ratio;
            }
            objImg.height = h;
            objImg.width = w;
        }
        img.src = objImg.src;
    },
    /*
    my97日期控件
    WdatePickerBegin  开始时间 控件使用
    WdatePickerEnd    结束时间 控件使用
    */
    WdatePickerBegin: function (maxId, options) {
        var _max = $dp.$(maxId);
        options = options || {};
        options.onpicked = function () { _max.focus(); };
        options.maxDate = "#F{$dp.$D('" + maxId + "')}";
        WdatePicker(options);
    },
    WdatePickerEnd: function (minId, options) {
        var _min = $dp.$(minId);
        options = options || {};
        options.minDate = "#F{$dp.$D('" + minId + "')}";
        WdatePicker(options);
    }, DocumentsFileType: "*.docx;*.doc;*.xlsx;*.xls;*.ppt;*.pptx;*.rar;*.7z;*.zip;",
    ImgFileType: "*.bmp;*.jpg;*.png;*.gif;*jpeg;"
    , NoPassInRemark: function (obj, remarkId, isReverse) {
        var Result = "Y";
        if (isReverse === undefined) {
            isReverse = false;
        } else if (isReverse === false) {
            Result = "Y";
        } else if (isReverse === true) {
            Result = "N";
        }
        if ($(obj).val() == Result) {
            $("#" + remarkId).attr("disabled", true);
        } else $("#" + remarkId).removeAttr("disabled");
    }, //Select 左右的方法
    Tool_Select: function (selectSource, select2) {
        var _select = {};
        $("#" + selectSource).dblclick(function () {
            var opn = $(this);
            var option = opn.find(":selected");
            _select.AddOption(document.getElementById(select2), option.text(), opn.val());
            option.remove();
        });
        $("#" + select2).dblclick(function () {
            var opn = $(this);
            var option = opn.find(":selected");
            //option = opn.find("option[value=" + opn.val() + "]");
            _select.AddOption(document.getElementById(selectSource), option.text(), opn.val());
            option.remove();
        });

        _select.Sw = function (flg) {
            var source, target;
            switch (flg) {
                case 1:
                case 2:
                    source = document.getElementById(selectSource);
                    target = document.getElementById(select2);
                    break;
                case 3:
                case 4:
                    source = document.getElementById(select2);
                    target = document.getElementById(selectSource);
                    break;
            }
            for (var i = source.options.length - 1; i >= 0; i--) {
                var opn = source.options[i];
                if (flg == 1 || flg == 4 || opn.selected) {
                    _select.AddOption(target, $(opn).text(), opn.value);
                    opn.parentNode.removeChild(opn);
                }
            }
        }
        _select.AddOption = function (lst, txt, val) {
            var oOption = document.createElement("OPTION");
            var findOption = $(lst).find("option[value='" + val + "']");
            if (findOption.length > 0) {
                return null;
            }
            lst.options.add(oOption);
            $(oOption).text(txt);
            oOption.value = val;
            return oOption;
        }

        _select.GetItemValueString = function (sel) {
            var list = "", ids = [];
            var source = document.getElementById(sel);
            for (var i = source.options.length - 1; i >= 0; i--) {
                var opn = source.options[i];
                ids.push(opn.value);
            }
            return ids.join(",");
        }

        return _select;
    }

};

//整数
function integer(obj) {
    obj.value = obj.value.replace(/[^\d]/g, "");
}

//两位小数
function decimal(obj) {
    //先把非数字的都替换掉，除了数字和小数点
    obj.value = obj.value.replace(/[^\d.]/g, "");
    //必须保证第一个为数字而不是小数点	
    obj.value = obj.value.replace(/^\./g, "");
    //保证只有出现一个小数点而没有多个.
    obj.value = obj.value.replace(/\.{2,}/g, ".");
    //保证小数点只出现一次，而不能出现两次以上
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
    var strs = "";
    var midd = "";
    var count = 0;
    for (var i = 0; i < obj.value.length; i++) {
        if (obj.value.charAt(i) == ".") {
            midd = "start";
        }
        if (midd == "start") {
            count++;
        }
        if (count == 4) {
            break;
        }
        strs += obj.value.charAt(i);
    }
    obj.value = strs;
}

//编码
String.prototype.htmlEncode = function () { return String(this).replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/'/g, '&#39;').replace(/</g, '&lt;').replace(/>/g, '&gt;'); }
//解码
String.prototype.htmlDecode = function () { return String(this).replace(/&quot;/g, '"').replace(/&#39;/g, "'").replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&amp;/g, '&'); }

//仿C# String.Format 用法：string.format(arg);
String.prototype.format = function () { var args = arguments; if (!this.replace) return ""; return this.replace(/\{(\d+)\}/g, function (m, i) { return args[i]; }); };

//仿C#中StringBuilder，用法一样。
var StringBuilder = function (val) {
    this.strings = new Array();
    this.append = function (val) { if (val) { this.strings.push(val); } }
    this.toString = function () { return this.strings.join(""); }
    this.clear = function () { this.strings.length = 1; }
    this.appendFormat = function (val) { if (val) { var args = arguments; var str = val.replace(/\{(\d+)\}/g, function (m, i) { return args[parseInt(i) + 1]; }); this.append(str); } };
}

function showJump() {
    $('#divJump').show();
}