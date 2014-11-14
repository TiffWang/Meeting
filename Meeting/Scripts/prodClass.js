var isAdd = 1;
var extAction = 1;

function submit() {
    if (isAdd == 0)
        CreateClass();
    else
        UpdateClass();
}

function extSubmit() {
    if (extAction > 0) {
        UpdateExt(extAction);
        //GetExtData($("#hfProdClassId").val());
    }
    else {
        CreateExt();
    }

}

function closeExtInfo() {
    $("#divExtList").hide();
    $("#div_prodedit").hide();
}

function GetData(prodClassId) {
    maskTop('divClass');
    showMask();
    $.ajax({
        url: '../Ajax/Product/ProdClassAjax.ashx?Action=GetData&prodClassId=' + prodClassId + '&TimeStamp=' + Date.parse(new Date()),
        type: 'GET',
        DataType: 'xml',
        timeout: 600000,
        error: function (xml, err) {

        },
        success: function (xml) {
            $(xml).find('xmlRoot').each(function () {
                var result = $(this).find("Result").text();
                if (result != '0') {
                    Boxy.alert('获取失败.', null, { title: '提示' });
                    hideMask();
                    return;
                }
                var thisNode = $(this);


                $("input[realname=canAddProd][value=" + thisNode.find("canAddProd").text() + "]").attr("checked", true);
                $("#radCanAddProd1").attr("disabled", "disabled");
                $("#radCanAddProd2").attr("disabled", "disabled");

                $("#txtClassName").val(thisNode.find("txtProdClassName").text());

                $("#lblId").text(thisNode.find("txtProdClassId").text());
                $("#lblId1").text(thisNode.find("txtProdClassId").text());

                $("#txtProdClassName").val(thisNode.find("txtProdClassName").text());
                $("#lblCanAddProd").text(thisNode.find("txtCanAddProd").text());
                $("#txtValidPeriods").val(thisNode.find("txtValidPeriods").text());
                $("#txtValidPeriode").val(thisNode.find("txtValidPeriode").text());
                $("#txtMaxDiscount").val(thisNode.find("txtMaxDiscount").text());
                $("#txtBonus").val(thisNode.find("txtBonus").text());
                $("#txtBonusPercent").val(thisNode.find("txtBonusPercent").text());
                $("#txtMinFirstPayPercent").val(thisNode.find("txtMinFirstPayPercent").text());
                $("#txtMaxRefundPercent").val(thisNode.find("txtMaxRefundPercent").text());

                $("input[realname=quotationAudit][value=" + thisNode.find("txtQuotationAudit").text() + "]").attr("checked", true);
                $("input[realname=isReleased][value=" + thisNode.find("txtIsReleased").text() + "]").attr("checked", true);
                $("input[realname=canBePromoted][value=" + thisNode.find("txtCanBePromoted").text() + "]").attr("checked", true);
                $("input[realname=isFree][value=" + thisNode.find("txtIsFree").text() + "]").attr("checked", true);
                $("input[realname=countPerformance][value=" + thisNode.find("txtCountPerformance").text() + "]").attr("checked", true);

                $("#drpServiceType").val(thisNode.find("serviceType").text());
                $("#drpContract").val(thisNode.find("contractId").text());

                $("#txtDescription").val(thisNode.find("txtDescription").text());

                isAdd = 1;
                $("#hfProdClassId").val(thisNode.find("txtProdClassId").text());
                $("#hfParentClassId").val(thisNode.find("txtParentClassId").text());
                closeExtInfo();
                hideMask();
            });
        }
    });
}

function CreateClass() {
    var prodClassId = $("#hfProdClassId").val();
    var parentClassId = $("#hfParentClassId").val();

    var prodClassName = $("#txtProdClassName").val();
    if (prodClassName == "")
        prodClassName = $("#txtClassName").val();

    var canAddProd = $('input:radio[realname="canAddProd"]:checked').val();

    var validPeriods = $("#txtValidPeriods").val();
    var validPeriode = $("#txtValidPeriode").val();
    var maxDiscount = $("#txtMaxDiscount").val();
    var contractId = $("#drpContract").val();
    var bonus = $("#txtBonus").val();
    var bonusPercent = $("#txtBonusPercent").val();
    var minFirstPayPercent = $("#txtMinFirstPayPercent").val();
    var maxRefundPercent = $("#txtMaxRefundPercent").val();

    var quotationAudit = $('input:radio[realname="quotationAudit"]:checked').val();
    var isReleased = $('input:radio[realname="isReleased"]:checked').val();
    var canBePromoted = $('input:radio[realname="canBePromoted"]:checked').val();
    var isFree = $('input:radio[realname="isFree"]:checked').val();
    var countPerformance = $('input:radio[realname="countPerformance"]:checked').val();

    var description = $("#txtDescription").val();

    var url = "&prodClassId=" + prodClassId;
    url += "&parentClassId=" + parentClassId;
    url += "&prodClassName=" + encodeURIComponent(prodClassName);
    url += "&canAddProd=" + canAddProd;
    url += "&validPeriods=" + validPeriods;
    url += "&validPeriode=" + validPeriode;
    url += "&maxDiscount=" + maxDiscount;
    url += "&contractId=" + contractId;
    url += "&bonus=" + bonus;
    url += "&bonusPercent=" + bonusPercent;
    url += "&minFirstPayPercent=" + minFirstPayPercent;
    url += "&maxRefundPercent=" + maxRefundPercent;
    url += "&quotationAudit=" + quotationAudit;
    url += "&isReleased=" + isReleased;
    url += "&canBePromoted=" + canBePromoted;
    url += "&isFree=" + isFree;
    url += "&countPerformance=" + countPerformance;
    url += "&description=" + encodeURIComponent(description);

    $.ajax({
        url: '../Ajax/Product/ProdClassAjax.ashx?Action=CreateClass',
        data: url,
        type: 'Post',
        DataType: 'xml',
        timeout: 600000,
        error: function (xml, err) {
        },
        success: function (xml) {
            $(xml).find('xmlRoot').each(function () {
                var result = $(this).find("Result").text();
                if (result != '0') {
                    Boxy.alert('创建失败.', null, { title: '提示' });
                    return;
                } else {
                    Boxy.alert('创建成功.', function () {
                        var index = window.location.href.indexOf("ProdClassId");
                        if (index > 0) {
                            window.location.href = window.location.href.substring(0, index - 1);
                        } else window.location.href = window.location.href;
                    }, { title: '提示' });
                }
            });
        }
    });
}


function UpdateClass() {

    var prodClassId = $("#hfProdClassId").val();
    var parentClassId = $("#hfParentClassId").val();

    var prodClassName = $("#txtProdClassName").val();
    var canAddProd = $('input:radio[realname="canAddProd"]:checked').val();
    if (canAddProd == "N")
        prodClassName = $("#txtClassName").val();

    var validPeriods = $("#txtValidPeriods").val();
    var validPeriode = $("#txtValidPeriode").val();
    var maxDiscount = $("#txtMaxDiscount").val();
    var contractId = $("#drpContract").val();
    var bonus = $("#txtBonus").val();
    var bonusPercent = $("#txtBonusPercent").val();
    var minFirstPayPercent = $("#txtMinFirstPayPercent").val();
    var maxRefundPercent = $("#txtMaxRefundPercent").val();
    var serviceType = $("#drpServiceType").val();

    var quotationAudit = $('input:radio[realname="quotationAudit"]:checked').val();
    var isReleased = $('input:radio[realname="isReleased"]:checked').val();
    var canBePromoted = $('input:radio[realname="canBePromoted"]:checked').val();
    var isFree = $('input:radio[realname="isFree"]:checked').val();
    var countPerformance = $('input:radio[realname="countPerformance"]:checked').val();

    var description = $("#txtDescription").val();

    var url = "&prodClassId=" + prodClassId;
    url += "&parentClassId=" + parentClassId;
    url += "&prodClassName=" + encodeURIComponent(prodClassName);
    url += "&canAddProd=" + canAddProd;
    url += "&validPeriods=" + validPeriods;
    url += "&validPeriode=" + validPeriode;
    url += "&maxDiscount=" + maxDiscount;
    url += "&contractId=" + contractId;
    url += "&bonus=" + bonus;
    url += "&bonusPercent=" + bonusPercent;
    url += "&minFirstPayPercent=" + minFirstPayPercent;
    url += "&maxRefundPercent=" + maxRefundPercent;
    url += "&quotationAudit=" + quotationAudit;
    url += "&isReleased=" + isReleased;
    url += "&canBePromoted=" + canBePromoted;
    url += "&isFree=" + isFree;
    url += "&countPerformance=" + countPerformance;
    url += "&description=" + encodeURIComponent(description);
    url += "&serviceType=" + serviceType;

    $.ajax({
        url: '../Ajax/Product/ProdClassAjax.ashx?Action=UpdateClass',
        data: url,
        type: 'post',
        DataType: 'xml',
        timeout: 600000,
        error: function (xml, err) {
            Boxy.alert('请求错误！', null, { title: '提示' });
        },
        success: function (xml) {
            $(xml).find('xmlRoot').each(function () {
                var result = $(this).find("Result").text();
                if (result != '0') {
                    Boxy.alert('修改失败.', null, { title: '提示' });
                    return;
                } else {
                    Boxy.alert('修改成功.', function () {
                        var index = window.location.href.indexOf("ProdClassId");
                        if (index > 0) {
                            window.location.href = window.location.href.substring(0, index) + "ProdClassId=" + prodClassId
                        } else window.location.href = window.location.href;
                    }, { title: '提示' });
                }
            });
        }
    });
}

function AddChildClass() {
    $("#txtClassName").val("");
    $("#txtProdClassName").val("");

    $("#hfParentClassId").val($("#hfProdClassId").val());

    $("#radCanAddProd1").removeAttr("checked", "");
    $("#radCanAddProd2").removeAttr("checked", "");

    $("#radCanAddProd1").removeAttr("disabled", "disabled");
    $("#radCanAddProd2").removeAttr("disabled", "disabled");
    isAdd = 0;
}

//--------------------------------------------- Ext --------------------------------------------------------

function GetExtData(prodClassId) {
    $.AjaxGetXml({
        url: '../Ajax/Product/ProdExtAjax.ashx?Action=GetExtData&prodClassId=' + prodClassId,
        htmlID: "tbExtList",
        resultNode: "resultData",
        showMask: true
    });
    hideMask();
}

function GetExtDataById(prodClassExtPropId) {
    $.AjaxGetXml({
        url: '../Ajax/Product/ProdExtAjax.ashx?Action=GetExtDataById&prodClassExtPropId=' + prodClassExtPropId,
        popupID: "div_box_ext",
        success: function (xml) {
            $("#txtExtPropName").val($(xml).find("txtExtPropName").text());
            $("#drpExtPropType").val($(xml).find("drpExtPropType").text());
            $("input[realname=isValid][value=" + $(xml).find("isValid").text() + "]").attr("checked", true);
        }
    });
}

function CreateExt() {
    var prodClassId = $("#hfProdClassId").val();
    var extPropName = $("#txtExtPropName").val();
    var extPropTypeId = $('#drpExtPropType').val();
    var isValid = $('input:radio[realname="isValid"]:checked').val();

    var url = "&prodClassId=" + encodeURIComponent(prodClassId);
    url += "&extPropName=" + encodeURIComponent(extPropName);
    url += "&extPropTypeId=" + encodeURIComponent(extPropTypeId);
    url += "&isValid=" + encodeURIComponent(isValid);
    url += "&TimeStamp=" + Date.parse(new Date());

    $.ajax({
        url: '../Ajax/Product/ProdExtAjax.ashx?Action=CreateExt' + url,
        type: 'GET',
        DataType: 'xml',
        timeout: 600000,
        error: function (xml, err) {

        },

        success: function (xml) {
            $(xml).find('xmlRoot').each(function () {
                var result = $(this).find("Result").text();
                if (result != '0') {
                    Boxy.alert('添加失败.', null, { title: '提示' });
                    return;
                } else {
                    hidePopup("div_box_ext");
                    GetExtData($("#hfProdClassId").val());
                    Boxy.alert('添加成功.', null, { title: '提示' });
                }
            });
        }
    });
}

function UpdateExt(prodClassExtPropId) {
    var prodClassId = $("#hfProdClassId").val();
    var extPropName = $("#txtExtPropName").val();
    var extPropTypeId = $('#drpExtPropType').val();
    var isValid = $('input:radio[realname="isValid"]:checked').val();

    var url = "&prodClassId=" + encodeURIComponent(prodClassId);
    url += "&prodClassExtPropId=" + encodeURIComponent(prodClassExtPropId);
    url += "&extPropName=" + encodeURIComponent(extPropName);
    url += "&extPropTypeId=" + encodeURIComponent(extPropTypeId);
    url += "&isValid=" + encodeURIComponent(isValid);
    url += "&TimeStamp=" + Date.parse(new Date());

    $.ajax({
        url: '../Ajax/Product/ProdExtAjax.ashx?Action=UpdateExt' + url,
        type: 'GET',
        DataType: 'xml',
        timeout: 600000,
        error: function (xml, err) {
        },
        success: function (xml) {
            $(xml).find('xmlRoot').each(function () {
                var result = $(this).find("Result").text();
                if (result != '0') {
                    Boxy.alert('修改失败.', null, { title: '提示' });
                    return;
                } else {
                    GetExtData($("#hfProdClassId").val());
                    hidePopup("div_box_ext");
                    Boxy.alert('修改成功.', null, { title: '提示' });
                }

            });
        }
    });
}