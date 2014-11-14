function GetExtData(prodClassId) {
    $.ajax({
        url: '../Ajax/Product/ProdInfoAjax.ashx?Action=GetExtData&prodClassId=' + prodClassId + '&TimeStamp=' + Date.parse(new Date()),
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
                    return;
                }

                var extData = $(this).find("ExtData").text();
                extData = XmlFormat(extData);
               
                $("#tdExtData").html(extData);
            });
        }
    });
}

function GetInfoData(prodClassId) {
    $.ajax({
        url: '../Ajax/Product/ProdInfoAjax.ashx?Action=GetExtData&prodClassId=' + prodClassId + '&TimeStamp=' + Date.parse(new Date()),
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
                    return;
                }

                var extData = $(this).find("ExtData").text();
                extData = XmlFormat(extData);

                $("#tdExtData").html(extData);
            });
        }
    });
}