//前台错误信息提示中英文
var prompt = new Object();
prompt.GetStr = function (nID) {

    //top.strLang = "English";
    if (!prompt._data.g_aryChinese[nID] && !prompt._data.g_aryChinese[nID]) {
        alert("错误: 没有找到资源编号 {" + nID + "}");
        return "";
    }
    return (top.strLang == "English") ? prompt._data.g_aryChinese[nID] : prompt._data.g_aryChinese[nID];
}

prompt._data = (function () {
    var _msgData = {};
    _msgData.g_aryChinese = new Array();
    _msgData.g_aryEnglish = new Array();

    //------------------------- 系统 -------------------------//
    _msgData.g_aryChinese[0] = "请输入正确的用户名！";
    _msgData.g_aryEnglish[0] = "";

    _msgData.g_aryChinese[1] = "添加成功！";
    _msgData.g_aryEnglish[1] = "";

    _msgData.g_aryChinese[2] = "添加失败！";
    _msgData.g_aryEnglish[2] = "";

    //------------------------- 商机 -------------------------//
    _msgData.g_aryChinese[101] = "线索添加成功！";
    _msgData.g_aryEnglish[101] = "";

    _msgData.g_aryChinese[102] = "线索添加失败！";
    _msgData.g_aryEnglish[102] = "";

    _msgData.g_aryChinese[103] = "处理成功！";
    _msgData.g_aryEnglish[103] = "";

    _msgData.g_aryChinese[104] = "处理失败！";
    _msgData.g_aryEnglish[104] = "";

    _msgData.g_aryChinese[105] = "没找到对应的线索！";
    _msgData.g_aryEnglish[105] = "";

    _msgData.g_aryChinese[106] = "线索还没有审核！";
    _msgData.g_aryEnglish[106] = "";

    _msgData.g_aryChinese[107] = "请选择分配清洗人员！";
    _msgData.g_aryEnglish[107] = "";

    _msgData.g_aryChinese[108] = "线索分配清洗成功！";
    _msgData.g_aryEnglish[108] = "";

    _msgData.g_aryChinese[109] = "线索分配清洗失败！";
    _msgData.g_aryEnglish[109] = "";

    _msgData.g_aryChinese[110] = "线索还没有被分配！";
    _msgData.g_aryEnglish[110] = "";

    _msgData.g_aryChinese[111] = "没找到对应的商机！";
    _msgData.g_aryEnglish[111] = "";

    _msgData.g_aryChinese[112] = "商机还没有审核！";
    _msgData.g_aryEnglish[112] = "";

    _msgData.g_aryChinese[113] = "放弃线索时不能写入黑名单！<br> 是否放弃此线索?";
    _msgData.g_aryEnglish[113] = "";

    _msgData.g_aryChinese[114] = "是否放弃此线索?";
    _msgData.g_aryEnglish[114] = "";

    _msgData.g_aryChinese[115] = "是否关闭此线索,同时加入黑名单?";
    _msgData.g_aryEnglish[115] = "";

    _msgData.g_aryChinese[116] = "是否关闭此线索?";
    _msgData.g_aryEnglish[116] = "";

    _msgData.g_aryChinese[117] = "是否提交为商机?";
    _msgData.g_aryEnglish[117] = "";

    _msgData.g_aryChinese[118] = "提交商机时不能写入黑名单！<br> 是否提交为商机?";
    _msgData.g_aryEnglish[118] = "";

    //------------------------- 产品 -------------------------//



    //------------------------- 订单 -------------------------//



    //------------------------- 员工管理 -------------------------//


    return _msgData;
})();

 