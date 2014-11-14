var setting = {
    check: {
        enable: true,
        chkboxType:{ "Y" : "p", "N" : "p" }
    },
    data: {
        simpleData: {
            enable: true
        }
    }
};

function setCheck() {
    var zTree = $.fn.zTree.getZTreeObj("treeFunc"),
			py = "p",
			sy = "s",
			pn = "p",
			sn = "s",
			type = { "Y": "p", "N": "p" };
    zTree.setting.check.chkboxType = type;

    var zTree1 = $.fn.zTree.getZTreeObj("treeEnty"),
			py = "p",
			sy = "s",
			pn = "p",
			sn = "s",
			type = { "Y": py + sy, "N": pn + sn };
    zTree1.setting.check.chkboxType = type;

}

function GetItemList() {
    var zTreeFunc = $.fn.zTree.getZTreeObj("treeFunc");

    var funcList = "";
    var nodeFuncChecked = zTreeFunc.getCheckedNodes(true);
    for (var i = 0, l = nodeFuncChecked.length; i < l; i++) {
        funcList += nodeFuncChecked[i].id + ",Y,0|";
    }
    var nodeFuncUnChecked = zTreeFunc.getCheckedNodes(false);
    for (var i = 0, l = nodeFuncUnChecked.length; i < l; i++) {
        funcList += nodeFuncUnChecked[i].id + ",N,0|";
    }


    var zTreeEnty = $.fn.zTree.getZTreeObj("treeEnty");

    var entyList = "";
    var nodeEntyChecked = zTreeEnty.getCheckedNodes(true);
    for (var i = 0, l = nodeEntyChecked.length; i < l; i++) {
        if (nodeEntyChecked[i].include == "1")
            entyList += nodeEntyChecked[i].id + ",Y,0|";

    }
    var nodeEntyUnChecked = zTreeEnty.getCheckedNodes(false);
    for (var i = 0, l = nodeEntyUnChecked.length; i < l; i++) {
        if (nodeEntyUnChecked[i].include == "1")
            entyList += nodeEntyUnChecked[i].id + ",N,0|";

    }

    $("#" + hfFunccListId).val(funcList);
    $("#" + hfEntypListId).val(entyList);
}