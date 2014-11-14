var currentRow;
var isEdit = 0;
function CancelEntya() {
    SetShowDiv(false);
    jQuery("#divEntya").fadeOut();
    jQuery("#txtTag").val("");
    jQuery("#txtEntyaName").val("");
    jQuery("#txtFiledName").val("");
    jQuery("#txtEntyaNo").val("");
}

function ShowAddEntya(isNew, obj) {
    jQuery("#divEntya").hide();
    if (isNew == "0") {
        isEdit = 0;
        showPopup("divEntya", 0);
        SetShowDiv(true);

        jQuery("#txtTag").val("");
        jQuery("#txtEntyaName").val("");
        jQuery("#txtFiledName").val("");
        jQuery("#txtEntyaNo").val("");
        jQuery("#btnAddEntya").show();
        jQuery("#btnEditEntya").hide();
        jQuery("#divEntya").show();


    }
    else {
        isEdit = 1;
        SetShowDiv(true);
        var tr = $(obj).parents("tr");
        currentRow = tr;
        jQuery("#txtTag").val($(tr).find('td').eq(0).text());
        jQuery("#txtEntyaName").val($(tr).find('td').eq(1).text());
        jQuery("#txtFiledName").val($(tr).find('td').eq(2).text());
        jQuery("#txtEntyaNo").val($(tr).find('td').eq(3).text());
        jQuery("#btnAddEntya").hide();
        jQuery("#btnEditEntya").show();
        jQuery("#divEntya").show();
    }
}

function submitEntya() {
    if (isEdit == 0)
        AddEntya();
    else
        EditEntya();
}
function DeleteEntya(obj) {
    var tr = jQuery(obj).parents("tr");
    jQuery(tr).fadeOut();
}
function hide(id) {
    jQuery("#" + id).fadeOut();
}
function AddEntya() {

    var rowHtml = "<tr align='center'>";
    rowHtml += "<td>" + jQuery("#txtTag").val() + "</td>";
    rowHtml += "<td>" + jQuery("#txtEntyaName").val() + "</td>";
    rowHtml += "<td>" + jQuery("#txtFiledName").val() + "</td>";
    rowHtml += "<td>" + jQuery("#txtEntyaNo").val() + "</td>";
    rowHtml += "<td colspan='2'><span  style=\"cursor:pointer;\" onclick='ShowAddEntya(1,this)' >编辑</span> ";
    rowHtml += " <span  style=\"cursor:pointer;\" onclick='DeleteEntya(this)' >删除</span> </td>";
    // rowHtml += "<td   style='cursor:pointer;'>  删除</td>";
    rowHtml += "</tr>";
    jQuery("#tblEntyaList").append(rowHtml);
    jQuery("#divEntya").fadeOut();
    SetShowDiv(false);
}


function EditEntya() {
    $(currentRow).find('td').eq(0).text(jQuery("#txtTag").val());
    $(currentRow).find('td').eq(1).text(jQuery("#txtEntyaName").val());
    $(currentRow).find('td').eq(2).text(jQuery("#txtFiledName").val());
    $(currentRow).find('td').eq(3).text(jQuery("#txtEntyaNo").val());
    jQuery("#divEntya").fadeOut();
    SetShowDiv(false);
}

function GetEntya() {
    var entya = "";
    jQuery("#tblEntyaList").find("tr").each(function (i) {
        if (i > 0) {
            if (i > 1)
                entya += "|";
            jQuery(this).find("td").each(function (l) {
                if (l < 4) {
                    if (l > 0)
                        entya += ",";

                    entya += $(this).text();
                }
            });
        }
    });
    jQuery("#" + hfEntyaListId).val(entya);
}

var SetShowDiv = function (isShow) {
    if ($mask) {
        isShow ? $($mask).show() : $($mask).hide();
    }
}
