var currentRow;
var currentEntyaId;
var isEdit = 0;
function CancelEntya() {
    hidePopup('divEntya');
    jQuery("#txtTag").val("");
    jQuery("#txtEntyaName").val("");
    jQuery("#txtFiledName").val("");
    jQuery("#txtEntyaNo").val("");
}

function ShowAddEntya(isNew, obj, entyaId) {
    jQuery("#divEntya").hide();
    if (isNew == "0") {
        isEdit = 0;
        jQuery("#txtTag").val("");
        jQuery("#txtEntyaName").val("");
        jQuery("#txtFiledName").val("");
        jQuery("#txtEntyaNo").val("");
        jQuery("#btnAddEntya").show();
        jQuery("#btnEditEntya").hide();
        //jQuery("#divEntya").show();
        showPopup('divEntya', 0);


    }
    else {
        isEdit = 1;
        var tr = $(obj).parent();

        currentRow = tr;
        
        jQuery("#txtTag").val($(tr).find('td').eq(0).text());
        jQuery("#txtEntyaName").val($(tr).find('td').eq(1).text());
        jQuery("#txtFiledName").val($(tr).find('td').eq(2).text());
        jQuery("#txtEntyaNo").val($(tr).find('td').eq(3).text());
        jQuery("#btnAddEntya").hide();
        jQuery("#btnEditEntya").show();
        //jQuery("#divEntya").show();
        currentEntyaId = entyaId;
        showPopup('divEntya', 0);
    }
}

function hide(id) {
    jQuery("#" + id).fadeOut();
}

function submitEntya() {
    if (isEdit == 0)
        CreateEntyaAjax();
    else
        UpdateEntyaAjax();
}

function CreateEntyaAjax() { 
    var updateId = 0;
    var name = jQuery("#txtEntyaName").val();
    var entyId = jQuery("#" + hfEntyIdId).val();
    var tag = jQuery("#txtTag").val();
    var filedName = jQuery("#txtFiledName").val();
    var no = jQuery("#txtEntyaNo").val();

    var url = "";
    url += "&entyId=" + encodeURIComponent(entyId);
    url += "&updateId=" + encodeURIComponent(updateId);
    url += "&tag=" + encodeURIComponent(tag);
    url += "&name=" + encodeURIComponent(name);
    url += "&fieldName=" + encodeURIComponent(filedName);
    url += "&no=" + encodeURIComponent(no);
    url += "&TimeStamp=" + Date.parse(new Date());
    
    //url = encodeURIComponent(url);
    maskTop('divEntya');
    $.ajax({
        url: '../Ajax/Compt/ComptAjax.ashx?Action=CreateEntya' + url,
        type: 'GET',
        DataType: 'xml',
        timeout: 600000,
        error: function (xml, err) {
            //Ext.MessageBox.alert('', 'Internet error, please try later');
            //            $('#divMsg').html("<span class='redfont'>This message [" + chatContent + "] had be sent failed. Please resend.</span>");
            popupTop('divEntya');
        },

        success: function (xml) {
            $(xml).find('xmlRoot').each(function () {
                var result = $(this).find("Result").text();
                if (result != '0') {
                    Boxy.alert('创建失败.', null, { title: '提示' });
                    popupTop('divEntya');
                    return;
                }
                var entyaId = $(this).find("entyaId").text();
                hidePopup('divEntya', function () {



                   
                    var rowHtml = "<tr align='center'>";
                    rowHtml += "<td>" + tag + "</td>";
                    rowHtml += "<td>" + name + "</td>";
                    rowHtml += "<td>" + filedName + "</td>";
                    rowHtml += "<td>" + no + "</td>";
                    rowHtml += "<td onclick='ShowAddEntya(1,this," + entyaId + ");'  style='cursor:pointer;'>编辑</td>";
                    rowHtml += "<td onclick='DeleteEntyaAjax(this," + entyaId + ");'  style='cursor:pointer;'>  删除</td>";
                    rowHtml += "</tr>";


                    //alert(jQuery('#tblEntyaList').html());
                    jQuery('#tblEntyaList').append(rowHtml);



                    Boxy.alert('创建成功.', null, { title: '提示' });
                });



            });
        }
    });
}



function UpdateEntyaAjax() {
    var updateId = 0;
    var name = jQuery("#txtEntyaName").val();
    var entyId = jQuery("#" + hfEntyIdId).val();
    var tag = jQuery("#txtTag").val();
    var filedName = jQuery("#txtFiledName").val();
    var no = jQuery("#txtEntyaNo").val();
    var entyaId = currentEntyaId;
    maskTop('divEntya');
    $.ajax({
        url: '../Ajax/Compt/ComptAjax.ashx?Action=UpdateEntya&entyId=' + entyId + '&entyaId=' + entyaId + '&updateId=' + updateId + '&name=' + encodeURIComponent(name) + '&tag=' + encodeURIComponent(tag) + '&fieldName=' + encodeURIComponent(filedName) + '&no=' + no + '&TimeStamp=' + Date.parse(new Date()),
        type: 'GET',
        DataType: 'xml',
        timeout: 600000,
        error: function (xml, err) {
            //Ext.MessageBox.alert('', 'Internet error, please try later');
            //            $('#divMsg').html("<span class='redfont'>This message [" + chatContent + "] had be sent failed. Please resend.</span>");
            popupTop('divEntya');
        },
        
        success: function (xml) {
            $(xml).find('xmlRoot').each(function () {
                var result = $(this).find("Result").text();
                if (result != '0') {
                    popupTop('divEntya');
                    Boxy.alert('更新失败', null, { title: '提示' });
                    return false;
                }

                $(currentRow).find('td').eq(0).text(tag);
                $(currentRow).find('td').eq(1).text(name);
                $(currentRow).find('td').eq(2).text(filedName);
                $(currentRow).find('td').eq(3).text(no);
                hidePopup('divEntya');
                Boxy.alert('更新成功', null, { title: '提示' });
                return true;

            });
        }
    });
}


function DeleteEntyaAjax(obj, entyaId) {
    showMask();
    $.ajax({
        url: '../Ajax/Compt/ComptAjax.ashx?Action=DeleteEntya&entyaId=' + entyaId + '&TimeStamp=' + Date.parse(new Date()),
        type: 'GET',
        DataType: 'xml',
        timeout: 600000,
        error: function (xml, err) {
            //Ext.MessageBox.alert('', 'Internet error, please try later');
            //            $('#divMsg').html("<span class='redfont'>This message [" + chatContent + "] had be sent failed. Please resend.</span>");
            hideMask();
        },

        success: function (xml) {
            $(xml).find('xmlRoot').each(function () {
                var result = $(this).find("Result").text();
                if (result != '0') {
                    hideMask();
                    Boxy.alert('删除失败.', null, { title: '提示' });
                    return false;
                }
                Boxy.alert('删除成功.', null, { title: '提示' });
                var tr = $(obj).parent();
                hideMask();
                $(tr).fadeOut();
                
                return true;

            });
        }
    });
}