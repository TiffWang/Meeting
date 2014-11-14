var orderNameVal = {
    pack: {
        Id: "selectedProdPackId",
        count: "packCount",
        discount: "packDiscount",
        jsonData: "packJsonData"
    },
    prod: {
        Id: "selectedProdId",
        name: "prodName",
        count: "prodCount",
        discount: "prodDiscount"
    },
    model: {
        Id: "modelId",
        name: "modelName",
        count: "modelCount",
        prices: "modelPrices",
        remark: "modelRemark",
        prodId: "modelProdId"
    }
};


var setProdHmtl = function (nodes, isProdModel) {
    $("#tbdproditem").html(createProdHtml(nodes, isProdModel)).parent().show();
    $("#tbitem1").treetable({ reset: true }).treetable("expandAll");
    msieChange();
}

var setModelHtml = function (models) {
    var mHtml = createModelHtml(models);
    $("#tbdselectedModel").html(mHtml);

    $("#tbTreetable").treetable({ expandable: true, reset: true }).treetable("expandAll");
    $("#div_model").show();
}

var createProdHtml = function (nodes, isProdModel) {
    var sb = new StringBuilder(), node = {};
    if (CurProdType == 5) {
        CurEditProdJson = nodes;
    }
    for (var i = 0, len = nodes.length; i < len; i++) {
        node = nodes[i];
        node.name = node.name || "";
        if (CurProdType == 3 || CurProdType == 5) {//组合产品

            if (node.prodType == 3) {
                sb = createPackTrHtml(node, sb);
            } else {
                sb = createProdViewTrHtml(node, sb);
            }
        } else {
            sb = createProdTrHtml(node, sb);
        }
    }
    return sb.toString();
    //$("#selectedProdId").val(node.id);
}


var priceChange = function () {
    var pricesum = 0;
    $("#tbdproditem tr td[name='price']").text(function () {
        pricesum = pricesum + (parseFloat($(this).text()) || 0);
    });
    pricesum = pricesum || 0;
    pricesum = pricesum.toFixed(2);
    $("#spProdSum").text(pricesum);
    $("#tbdproditem td[name='selPrice']").text(pricesum)
    .next().find("input:text").attr("price", pricesum).change();
    $("#txtinvoiceMoney").val(pricesum);
}

var modelItemCount_Control = function (obj) {
    var txtItem = $(obj), countVal = parseInt(txtItem.val()), price = parseFloat(txtItem.attr("price"));
    txtItem.val(countVal);
    if (countVal <= 0) {
        delmodelItem(txtItem);
        txtItem.val(1);
        countVal = 1;
    }
    if (txtItem.attr("name").indexOf(orderNameVal.model.count) > -1) {
        txtItem.parents("td").next().text(((price * countVal) || price).toFixed(2));
        priceChange();
    } else {
        var pTd = txtItem.parents("td"),
                 disCount = (parseFloat(pTd.next().find("input:text").val()) || 100);
        price = (price * countVal * (disCount / 100));
        price = price.toFixed(2);
        txtItem.parents("tr").find("span[name='prodPrice']").text(price);
        $("#txtinvoiceMoney").val(price);
    }
}
//折扣改变事件
var disCountChange = function (obj) {
    if (obj) {
        obj = $(obj).parents("td").prev().find("input:text");
        modelItemCount_Control(obj);
    }
}

//发票改变事件
var invoiceChange = function (obj) {
    $("#form1").validationEngine("hide");
    var eleVal = $(obj).val();
    if (eleVal == 0) {//不需要发票
        //$(obj).parents("table").find("[class*='validate']").attr("disabled", true);
        // $("#tbOrderDetail4").hide();
    } else {
        // $(obj).parents("table").find("[class*='validate']").removeAttr("disabled");
        // $("#tbOrderDetail4").show();
    }
    if (eleVal == "Z") {
        // var _head = $("#txtHead");
        // _head.val(_head.attr("IdName"));
        // _head.attr("readonly", true);
    } else {
        //$("#txtHead").removeAttr("readonly") 

    }
}


//根据产品包ID获取 包含了模块信息
var getProdsByPackId = function (getId) {
    $.AjaxGetJson({
        url: '../Ajax/Product/ProdInfoAjax.ashx?Action=GetProdsByPackId&getID=' + getId,
        resultNode: "resultData",
        success: function (data) {
            if (data) {
                setProdHmtl(data);
            }
        }
    });
}

//GetProdModelJsonByProdId
//产品模块json信息
var getProdModelJsonByProdId = function (getId) {
    $.AjaxGetJson({
        url: '../Ajax/Product/ProdInfoAjax.ashx?Action=GetProdModelJsonByProdId&getID=' + getId,
        resultNode: "returnData",
        success: function (data) {
            if (!data) return;

            var html = createModelViewHtml(data); //createModelHtml(data);
            if (html.length > 0) {
                html = createModelHead() + html;
                $(html).appendTo("#tbdproditem");
                $("#tbitem1").treetable({ reset: true }).treetable("expandAll");
                msieChange();
            }
        }
    });
}

var delmodelItem = function (obj) {
    if (!obj.attr) obj = $(obj);
    if (obj.attr("isOptional") == "true") {
        Boxy.alert("这是必选模块，不可删除！", null, { title: "提示" });
        return;
    }
    Boxy.confirm("确定删除?", function () { $(obj).parents("tr").remove(); }, "");
}

var msieChange = function () {
    if ($.browser.msie) {
        $("input:text[onchange='modelItemCount_Control(this);']").blur(function () { modelItemCount_Control(this); });
        $("input:text[onchange='disCountChange(this);']").blur(function () { disCountChange(this); });
    }
}

var createModelHead = function (prodName) {
    return "<tr><td colspan='6'>上行产品所有模块</td></tr><tr class='tablelist-head'><td>模块名称</td><td>定价</td><td>数量</td><td>金额</td><td>备注</td><td>操作</td></tr>";
}

var createModelHtml = function (models) {
    var sb = new StringBuilder(), trId = "tr_";
    var _fun_appendPId = function (pid) {
        if (pid) { return "data-tt-parent-id='" + pid + "' "; }
        return "";
    }
    for (var i = 0; i < models.length; i++) {
        var node = models[i], children = node.children ? true : false
                , isOptional = node.open; //是否必填. node.open这里意思是是否必填
        //if (node.open) children = node.open;
        //node.pId = null;
        sb.appendFormat("<tr id='tr_{0}' data-tt-id='{0}' {1}  class='{2}' style=\"{3}\" >", node.id
                                 , _fun_appendPId(node.pId)
                                 , (children ? "branch" : "leaf")
                                 , (isOptional ? "background-color:whitesmoke;" : ""));
        sb.append("<td class='tdfirst'>");
        if (!children) {
            sb.appendFormat("<input type='hidden' name='" + orderNameVal.model.Id + node.pId + "' value='{0}' />", node.id);
            sb.appendFormat("<input type='hidden' name='" + orderNameVal.model.prices + node.pId + "' value='{0}' />", node.price);
            sb.appendFormat("<input type='hidden' name='" + orderNameVal.model.prodId + node.pId + "' value='{0}' />", node.pId);
            sb.appendFormat("<input type='hidden' name='" + orderNameVal.model.name + node.pId + "' value='{0}' />", node.name.htmlEncode());
        }

        sb.appendFormat("<span>{0}</span>", node.name);
        sb.append("</td>");

        if (!children) {
            sb.appendFormat("<td>{0}</td> <td>", node.price);
            sb.append("<a class='decrease' href='javascript:void(0);' onclick='count_computing(this);' ><img src='../images/btn-decrease.gif'></a>");
            sb.appendFormat("<input type='text' class='text  validate[required,custom[onlyNumberSp]]' onchange='modelItemCount_Control(this);' value='{2}' onkeyup='integer(this)'  name='" + orderNameVal.model.count + node.pId + "'  price='{0}' isOptional='{1}' />"
                                    , node.price, isOptional, (node.quantity || 1));
            sb.append("<a class='increase' href='javascript:void(0);' onclick='count_computing(this);' ><img src='../images/btn-increase.gif'></a>");
            sb.appendFormat("</td> <td name='price' >{0}</td>", node.quantity ? (node.quantity * node.price) : node.price);
            //sb.append("<td><input type='text' class='text'  name='modelItemDesc' /></td>");
            sb.appendFormat("<td><input type='text' class='text validate[maxSize[500]]' value='{0}'  name='" + orderNameVal.model.remark + node.pId + "' /></td>", (node.desc || ""));
            sb.appendFormat("<td>{0}</td>", isOptional ? "" : "<a href='javascript:void(0);' onclick='delmodelItem(this);'>删除</a>");
        } else {
            sb.append("<td></td><td></td><td></td><td></td><td></td>"); //<td></td>
        }
        sb.append("</tr>");
    }
    return sb.toString();
}


var createModelViewHtml = function (models) {
    var sb = new StringBuilder(), trId = "tr_";
    var _fun_appendPId = function (pid) {
        if (pid) { return "data-tt-parent-id='" + pid + "' "; }
        return "";
    }
    for (var i = 0; i < models.length; i++) {
        var node = models[i], children = node.children ? true : false
                , isOptional = node.open; //是否必填. node.open这里意思是是否必填
        //if (node.open) children = node.open;
        //node.pId = null;
        sb.appendFormat("<tr id='tr_{0}' data-tt-id='{0}' {1}  class='{2}' style=\"{3}\" >", node.id
                                 , _fun_appendPId(node.pId)
                                 , (children ? "branch" : "leaf")
                                 , (isOptional ? "background-color:whitesmoke;" : ""));
        sb.append("<td class='tdfirst'>");
        if (!children) {
            sb.appendFormat("<input type='hidden' name='" + orderNameVal.model.Id + node.pId + "' value='{0}' />", node.id);
            sb.appendFormat("<input type='hidden' name='" + orderNameVal.model.prices + node.pId + "' value='{0}' />", node.price);
            sb.appendFormat("<input type='hidden' name='" + orderNameVal.model.prodId + node.pId + "' value='{0}' />", node.pId);
            sb.appendFormat("<input type='hidden' name='" + orderNameVal.model.name + node.pId + "' value='{0}' />", node.name.htmlEncode());
            sb.append("<input type='hidden' name='" + orderNameVal.model.count + node.pId + "' value='1' />");
        }

        sb.appendFormat("<span>{0}</span>", node.name);
        sb.append("</td>");

        if (!children) {
            sb.appendFormat("<td>{0}</td> <td>", node.price);
            sb.appendFormat("1</td> <td name='price' >{0}</td>", node.quantity ? (node.quantity * node.price) : node.price);
            //sb.append("<td><input type='text' class='text'  name='modelItemDesc' /></td>");
            sb.appendFormat("<td><input type='text' class='text validate[maxSize[500]]' value='{0}'  name='" + orderNameVal.model.remark + node.pId + "' /></td>", (node.desc || ""));
            sb.appendFormat("<td>{0}</td>", isOptional ? "" : "<a href='javascript:void(0);' onclick='delmodelItem(this);'>删除</a>");
        } else {
            sb.append("<td></td><td></td><td></td><td></td><td></td>"); //<td></td>
        }
        sb.append("</tr>");
    }
    return sb.toString();
}



//标准产品tr生成
var createProdTrHtml = function (node, sb) {

    sb.appendFormat("<tr data-tt-id='{0}' >", node.id);
    sb.appendFormat("<td class='tdfirst' ><a href='javascript:void(0);' onclick='ProdByType.lookSelected(\"{0}\");' >{1}</a></td>", node.id, node.name);
    sb.appendFormat("<td name='selPrice' >{0}</td> <td>", node.price);

    sb.appendFormat("<input type='hidden' name='{1}' value='{0}' />", node.name.htmlEncode(), orderNameVal.prod.name + node.id);
    sb.appendFormat("<input type='hidden' name='{1}' value='{0}' />", node.id, orderNameVal.prod.Id);

    sb.append("<a class='decrease' href='javascript:void(0);'  onclick='count_computing(this);' ><img src='../images/btn-decrease.gif'></a>");
    sb.appendFormat("<input type='text' value='{2}' class='text validate[required,custom[onlyNumberSp]]' onchange='modelItemCount_Control(this);'  onkeyup='integer(this)'  name='{0}' price='{1}' />"
                                , orderNameVal.prod.count + node.id, node.price, node.quantity || 1);
    sb.append("<a class='increase' href='javascript:void(0);'  onclick='count_computing(this);' ><img src='../images/btn-increase.gif'></a>");
    sb.appendFormat("</td> <td><input type='text' value='{1}' class='text validate[number,min[1],max[100]]' name='{0}' onchange='disCountChange(this);' onkeyup='integer(this)' placeholder='1-100 7.5折填75' title='值为1-100 如：7.5折请填75' />%</td>"
                            , orderNameVal.prod.discount + node.id, (node.discount || ""));
    sb.appendFormat("<td name='price'><span name='prodPrice'> {0}</span></td>", node.quantity ? (node.quantity * node.price * (node.discount / 100)) : node.price);
    sb.appendFormat(" <td><a href='javascript:void(0);' onclick='ProdByType.lookSelected(\"{0}\");' >查看</a></td>", node.id);
    sb.append("</tr>");
    if (node.Models && node.Models.length > 0) {
        sb.append(createModelHead());
        //sb.append(createModelHtml(node.Models));
        sb.append(createModelViewHtml(node.Models));
    }

    return sb;
}

//创建简单的查看行
var createProdViewTrHtml = function (node, sb) {
    sb.appendFormat("<tr data-tt-id='{0}' data-tt-parent-id='{1}' class='branch' >", node.id, node.pId);
    sb.appendFormat("<td class='tdfirst' ><a href='javascript:void(0);' onclick='ProdByType.lookSelected(\"{0}\");' >{1}</a></td>", node.id, node.name);
    sb.appendFormat("<td>{0}</td> <td>{1}</td> <td></td><td><span></span></td>", node.price, node.quantity || "");
    sb.appendFormat(" <td><a href='javascript:void(0);' onclick='ProdByType.lookSelected(\"{0}\");' >查看</a></td>", node.id);
    sb.append("</tr>");
    if (node.Models.length > 0) {
        sb.append(createModelHead());
        //sb.append(createModelHtml(node.Models));
        sb.append(createModelViewHtml(node.Models));
    }
    return sb;
}

//创建产品包行
var createPackTrHtml = function (node, sb) {
    sb.appendFormat("<tr id='pack' data-tt-id='{0}' class='leaf' style=\"background-color:whitesmoke;\">", node.id);
    sb.appendFormat("<td class='tdfirst' ><a href='javascript:void(0);' onclick='ProdByType.lookSelected(\"{0}\");' >{1}</a></td>", node.id, node.name);
    sb.appendFormat("<td name='selPrice'>{0}</td> <td>", node.price);

    sb.appendFormat("<input type='hidden' name='{1}' value='{0}' />", node.id, orderNameVal.pack.Id);
    if (CurProdType == 5) {
        sb.appendFormat("<input type='hidden' name='{1}' value='{0}' />", $.toJSON(CurEditProdJson), orderNameVal.pack.jsonData + node.id);
    } else {
        sb.appendFormat("<input type='hidden' name='{1}' value='{0}' />", $.toJSON(CurEditProdJson || ProdByType_ztreeJson), orderNameVal.pack.jsonData + node.id);
    }

    sb.append("<a class='decrease' href='javascript:void(0);'  onclick='count_computing(this);' ><img src='../images/btn-decrease.gif'></a>");
    sb.appendFormat("<input type='text' value='{3}' class='text validate[required,custom[onlyNumberSp]]' onchange='modelItemCount_Control(this);'  onkeyup='integer(this)'  name='{2}' price='{1}' />"
                        , node.name, node.price, orderNameVal.pack.count + node.id, (node.quantity || 1));
    sb.append("<a class='increase' href='javascript:void(0);'  onclick='count_computing(this);' ><img src='../images/btn-increase.gif'></a>");
    sb.appendFormat("</td> <td><input type='text' value='{1}' class='text validate[number,min[1],max[100]]' name='{0}'  onchange='disCountChange(this);' onkeyup='integer(this)' />%</td>"
                        , orderNameVal.pack.discount + node.id, (node.discount || ""));
    sb.appendFormat("<td name='price'><span name='prodPrice'> {0}</span></td>", node.discount ? (node.quantity * node.price * (node.discount / 100)) : node.price);
    sb.appendFormat(" <td><a href='javascript:void(0);' onclick='ProdByType.lookSelected(\"{0}\");' >查看</a></td>", node.id);
    sb.append("</tr>");
    return sb;
}
