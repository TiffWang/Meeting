(function (a) {
    //固定table的表头
    a.CreateFixedTableThead = function (useTable) {
        //var newObject = $(useTable).find("thead");
        useTable = $(useTable);
        var newObject, newTheadDiv, useTableParent;
        newObject = useTable.clone();
        newObject.find("tbody").remove();
        useTableParent = useTable.parent();
        if (!useTableParent) { return; }

        newTheadDiv = $("<div style='position:fixed;z-index:10;'></div>");
        newTheadDiv.css("width", (useTable.width()) + "px")
                        .css("top", useTableParent.position().top + "px");
        newTheadDiv.append(newObject);
        useTableParent.append(newTheadDiv);
        //设置父级div的position 值
        useTableParent.css("position", "relative");

        var tdArr = $(newObject).css("width", "100%").attr("id", "").find("tr:first td,th");
        useTable.find("tr:first").find("th,td").each(function (index, obj) {
            obj = $(obj);
            var bWidth = parseInt(obj.css("border-left-width") || 0);
            bWidth = obj.width() + bWidth;
            $(tdArr[index]).css("width", bWidth + "px");
        });

        $(window).resize(function () {
            newTheadDiv.css("width", (useTable.width()) + "px");
            var tdArr = $(newObject).find("tr:first td,th");
            useTable.find("tr:first").find("th,td").each(function (index, obj) {
                var bWidth = parseInt($(obj).css("border-left-width") || 0);
                bWidth = $(obj).width() + bWidth;
                $(tdArr[index]).css("width", bWidth + "px");
            });
        });

    };

})(jQuery);