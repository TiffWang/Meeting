var AutocompleteIndustry = new Object();
AutocompleteIndustry.hidIndustryId = "hidIndustry";
AutocompleteIndustry.spIndustryId = "spIndustry";
AutocompleteIndustry.txtIndustryId = "txtIndustry";
AutocompleteIndustry.init = function (hidIndustryId, spIndustryId, txtIndustryId) {
    AutocompleteIndustry.hidIndustryId = hidIndustryId;
    AutocompleteIndustry.spIndustryId = spIndustryId;
    AutocompleteIndustry.txtIndustryId = txtIndustryId;
}

AutocompleteIndustry.result = function (event, data, formatted) {
    if (data) {
        $("#" + AutocompleteIndustry.hidIndustryId).val(data.Path);
        $("#" + AutocompleteIndustry.spIndustryId).text(data.PathName);
        //data.Code.length =级别
        var level = 1;
        switch (data.Level) {
            case "一级": level = 1; break;
            case "二级": level = 2; break;
            case "三级": level = 3; break;
            case "四级": level = 4; break;
            default: break;
        }
        if ($("#" + AutocompleteIndustry.txtIndustryId).data("path") != data.Path) {
            $("#selIndustry2,#selIndustry3,#selIndustry4").remove();
            $(".levelName").remove();
            if (level < 4) {
                AutocompleteIndustry.CreateIndustrySelect(AutocompleteIndustry.txtIndustryId, level, data.CodeId);
            }
            $("#" + AutocompleteIndustry.txtIndustryId).data("path", data.Path);
        }
    } else {
        $("#" + AutocompleteIndustry.hidIndustryId).val("");
        $("#" + AutocompleteIndustry.spIndustryId).text("");
    }
};

AutocompleteIndustry.CreateIndustrySelect = function (appendId, level, codeId) {
    var count = 4 - parseInt(level);
    var beforeSelId = "";
    var levelName = "二级";
    for (var i = 0; i < count; i++) {
        selId = "selIndustry" + (level + i + 1);
        if (!document.getElementById(selId)) {
            var sel = document.createElement("select");
            sel.id = selId;
            $(sel).attr("level", level + i + 1).attr("codeId", codeId);
            $(sel).html("<option value=''>-请选择" + (level + i + 1) + "级行业-</option>");
            $(sel).addClass("select validate[required]");
            $(sel).change(AutocompleteIndustry.selIndustryChange);
            switch ((level + i + 1)) {
                case 2: levelName = "<span class='levelName'>二级:</span>"; break;
                case 3: levelName = "<span class='levelName'>三级:</span>"; break;
                case 4: levelName = "<span class='levelName'>四级:</span>"; break;
            }
            if (i == 0) {
                $("#" + appendId).after(sel);
                $("#" + appendId).after(levelName);
                AutocompleteIndustry.getAjaxIndustryByParent(codeId, (level + i + 1));
            } else {
                $("#" + beforeSelId).after(sel);
                $("#" + beforeSelId).after(levelName);
            }
            beforeSelId = selId;
        }
    }
};
AutocompleteIndustry.selIndustryChange = function () {
    var level = parseInt($(this).attr("level")), codeId = $(this).find("option:selected").attr("codeId");
    if (level < 4) {
        if (codeId) {
            AutocompleteIndustry.getAjaxIndustryByParent(codeId, level + 1);
        }
    } else {
        var code2 = $("#selIndustry2").val();
        var code3 = $("#selIndustry3").val();
        var codePath = $("#" + AutocompleteIndustry.hidIndustryId).val();
        var oldPath = codePath.split(",");
        var newPath = [];
        newPath[0] = oldPath[0];
        if (code2) {
            newPath[1] = code2;
        } else {
            if (oldPath[1])
                newPath[1] = oldPath[1];
        }
        if (code3) {
            newPath[2] = code3;
        } else {
            if (oldPath[2])
                newPath[2] = oldPath[2];
        }
        if ($(this).val()) {
            newPath[3] = $(this).val();
        } else {
            if (oldPath[3])
                newPath[3] = oldPath[3];
        }
        codePath = newPath.join(",");
        $("#" + AutocompleteIndustry.hidIndustryId).val(codePath);
        $("#" + AutocompleteIndustry.hidIndustryId).val();
        $("#" + AutocompleteIndustry.hidIndustryId).val(codePath);
    }
};
AutocompleteIndustry.getAjaxIndustryByParent = function (codeId, level) {
    var json = JSONDATA_VocationDataArr[codeId];
    if (json) {
        var data = $.parseJSON(json);
        if (data) {
            var sb = new StringBuilder();
            var optionhtml = "<option codeId=\"{0}\" value=\"{1}\" >{2}</option>";
            sb.append("<option codeId=\"\" value=\"\" >-请选择" + level + "级行业-</option>");
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                sb.append(optionhtml.format(item["CodeId"], item["Code"], item["Name"]));
            }
            $("#selIndustry" + level).html(sb.toString());
        }
    }
    //    $.AjaxGetXml({
    //        url: '../ajax/Industry.ashx?Action=GetIndustryBy&industryId=' + codeId + "&level=" + level,
    //        async: false,
    //        isFill: false,
    //        popupID: false,
    //        showMask: false,
    //        success: function (xml) {
    //            var options = $(xml).find("resultData").text();
    //            $("#selIndustry" + level).html(options);
    //        }
    //    });
};

