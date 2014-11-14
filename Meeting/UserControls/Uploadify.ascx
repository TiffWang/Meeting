<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Uploadify.ascx.cs" Inherits="Site.UserControls.Uploadify" %>
<script type="text/javascript">
    var myUploadify = new Object();
    myUploadify.fileId = "file_uploadify";
    myUploadify.upImgId = "div_img_show"; //显示上传后
    myUploadify.upFileId = "file_doc_show";
    myUploadify.uploadFileName = "uploadify_fileName";
    myUploadify.onDeleteUpFile = null;
    myUploadify.fileTypes = []; //文件类型
    myUploadify.isImgUploadFile = true; //上传文件是否是图片类型。 默认为是
    myUploadify.settings = {
        multi: false, //是否开启一次多文件上传
        isComplete: true  //是否开启上传成功后执行自定义的onComplete事件
    };
    myUploadify.Init = function (options) {
        //已初始化
        if (options.multi) myUploadify.settings.multi = options.multi;
        if (options.isComplete === false) myUploadify.settings.isComplete = options.isComplete;

        if ($.isFunction(options.onComplete) && myUploadify.settings.isComplete) {
            var _onComplete = options.onComplete;
            options.onComplete = function (event, queueId, fileObj, response, data) {
                var resultData = myUploadify.formatXmlToObj(response);
                myUploadify.onComplete(event, queueId, fileObj, resultData, data);
                _onComplete(event, queueId, fileObj, resultData, data);
            }
        } else if (myUploadify.settings.isComplete) {
            options.onComplete = myUploadify.onComplete;
        }

        if ($.isFunction(options.onSelect)) {
            var _onSelect = options.onSelect;
            options.onSelect = function (event, queueID, fileObj) {
                myUploadify.onSelect(event, queueID, fileObj);
                _onSelect(event, queueID, fileObj);
            }
        } else {
            options.onSelect = myUploadify.onSelect;
        }


        if (typeof (options.scriptData) == "object") {
            options.scriptData.fileType = options.fileExt || "*";
        } else {
            options.scriptData = { fileType: options.fileExt || "*" };
        }

        if (options.fileExt) {
            myUploadify.fileTypes = options.fileExt.split(";");
        }

        if (options.auto) {//为自动时
            $("#div_file_button").hide();
        }
        $("#" + myUploadify.fileId).uploadify(options);

        myUploadify.onDeleteUpFile = options.onDeleteUpFile;
    }

    myUploadify.uploadifyUpload = function () {
        $('#' + myUploadify.fileId).uploadifyUpload();
    }
    myUploadify.uploadifyClearQueue = function () {
        $('#' + myUploadify.fileId).uploadifyClearQueue();
    }

    //检查文件类型
    myUploadify.checkFileType = function (type) {
        var pass = false;
        type = "*" + type;
        var ft = myUploadify.fileTypes;
        if (ft) {
            for (var i = 0; i < ft.length; i++) {
                if (ft[i] == type) pass = true;
            }
        } else { pass = true; }
        return pass;
    }

    //如果界面没有初始化这将进行初始化
    $(window).load(function () {
        var _settings = $("#" + myUploadify.fileId).data("settings");
        if (!_settings) {
            $("#" + myUploadify.fileId).uploadify({
                // fileDesc: "请选择bmp jpg png gif jpeg文件",
                //fileExt: "*.jpg;*.png;*.gif;",
                onComplete: myUploadify.onComplete
            });
        }

    });

    //选择事件
    myUploadify.onSelect = function (event, queueID, fileObj) {
        if (!myUploadify.checkFileType(fileObj.type.toLowerCase())) {
            alert($("#" + myUploadify.fileId).data("settings").fileDesc);
            $("#" + myUploadify.fileId).uploadifyCancel(myUploadify.fileId + queueId);
        }
        myUploadify.isImgUploadFile = comm.ImgFileType.search(fileObj.type) >= 0;

    }

    //上传完一个文件后事件
    myUploadify.onComplete = function (event, queueId, fileObj, response, data) {

        //resultData 等于 Object {fielName: "14300000851315128540725803378.jpg", path: "/Uploadfile/201311/27/20131127033324669.jpg", fileType: "img"}

        var resultData = new Object();
        if (typeof (response) == "object") {
            resultData = response;
        } else
            resultData = myUploadify.formatXmlToObj(response);

        if (resultData.fileType == "img") {
            var img = myUploadify.createImg("src_" + queueId, ".." + resultData.path, resultData.fielName);
            comm.AutoResizeImage(100, 100, img);
            //$("#div_img_show").append(img);
            var imgItme = myUploadify.createImgItme(resultData.path).append(myUploadify.createAForImg(img));
            $("#div_img_show").append(imgItme);
        } else {
            //做文档处理
            $("#file_doc_show").append(myUploadify.createHtmlbyDoc(resultData.fielName, resultData.path));
        }

        if (!myUploadify.settings.multi) {
            myUploadify.deleteUploadify();
        }
    }

    myUploadify.formatXmlToObj = function (xml) {
        var xml = $($.parseXML(xml));
        //        , fielName = xml.find("fileName").text()
        //        , path = xml.find("path").text()
        //        , fileType = xml.find("fileType").text();
        return {
            fielName: xml.find("fileName").text(),
            path: xml.find("path").text(),
            fileType: xml.find("fileType").text()
        }
    }

    myUploadify.createImg = function (id, src, alt) {
        //return $("<img>").attr({ src: src, id: id });
        var img = document.createElement("img");
        img.src = src;
        img.id = id;
        img.alt = alt;
        img.width = "100";
        img.height = "100";
        //img.onerror = function () {}
        return img;
    }
    myUploadify.createAForImg = function (img) {
        var _a = $("<a>").attr({ href: img.src, title: img.alt, "target": "_blank" }).append(img);
        if ($.isFunction(_a.lightBox)) {
            _a.lightBox({ fixedNavigation: true });
        }
        return _a;
    }
    myUploadify.createImgItme = function (path) {
        var _hid = $("<input type='hidden' name='uploadify_fileName' />").val(path);
        return $("<div>").attr("class", "file_imgitem")
        .append("<span onclick=\"myUploadify.deleteUpImg(this);\" path=\"" + path + "\"  title='删除图片'>删除</span>").append(_hid);
    }

    myUploadify.createHtmlbyDoc = function (filename, path) {
        var hidHtml = $("<input type='hidden' name='uploadify_fileName' />").val(path);
        return $("<div>").text(filename).attr({ title: filename, "class": "file_docitem" })
            .append("&nbsp;<span class='docDelete' onclick=\"myUploadify.deleteUpImg(this);\" path=\"" + path + "\"  title='删除文档'>删除</span>")
            .append(hidHtml);
    }

    //删除上传控件的浏览
    myUploadify.deleteUploadify = function () {
        //$("#" + myUploadify.fileId).uploadifySettings("");
        $("#" + myUploadify.fileId).parent().find("object").hide();
        $("#div_file_button").hide();
    }
    //恢复上传控件的浏览
    myUploadify.recoveryUploadify = function () {
        var _fileUp = $("#" + myUploadify.fileId);
        _fileUp.parent().find("object").show();
        //        var _obj = _fileUp.parent().find("object");
        //        if (_obj < 1) {
        //            _fileUp.uploadify(_fileUp.data("settings"));
        //        }
        $("#div_file_button").show();
    }
    //删除已上的文件
    myUploadify.deleteUpImg = function (obj) {
        Boxy.confirm('确定删除此文件！', function () {

            var path = $(obj).attr("path");
            $.AjaxExecute({
                url: "../UserControls/Upload.ashx?action=del",
                data: { path: path },
                success: function (xml) {
                    //xml = $($.parseXML(xml));
                    xml = $(xml);
                    var status = xml.find("status").text();
                    if (status == "0") {
                        $(".file_imgitem span[path='" + path + "']").parent().remove();
                        $(".file_docitem span[path='" + path + "']").parent().remove();
                        if (!myUploadify.settings.multi) {
                            myUploadify.recoveryUploadify();
                        }
                        if (myUploadify.onDeleteUpFile) {
                            myUploadify.onDeleteUpFile(xml);
                        }
                        //$(obj).parent().remove();
                        Boxy.alert('删除成功！', null, { title: '提示' });
                    } else {
                        Boxy.alert('删除失败！', null, { title: '提示' });
                    }
                }
            });

        }, { title: '提示' });

    }
</script>
<div class="file_upload">
    <div id="file_uploadifyQueue">
    </div>
    <div class="inputfile">
        <input type="file" name="file_uploadify" id="file_uploadify" />
    </div>
    <div class="file_imgshow" id="div_img_show" style="margin: 10px;">
    </div>
    <div class="file_docshow" id="file_doc_show" style="margin: 10px;">
    </div>
    <p class="filebutton" id="div_file_button">
        <a href="javascript:void(0);" class="save" onclick=" myUploadify.uploadifyUpload();">
            上传</a> <a href="javascript:void(0);" class="del" style="display: none;" onclick="myUploadify.uploadifyClearQueue();">
                取消上传</a>
    </p>
</div>
<%--
<div class="file_imgitem" style="position: relative; float: left; width: 110px;">
    <a href="http://localhost:20195/Uploadfile/201308/30/20130830105157807.jpg" title="Win7_第5辑_06.jpg"
        style="">
        <img src="../Uploadfile/201308/30/20130830105157807.jpg" id="src_YDTGJR" alt="Win7_第5辑_06.jpg"
            style="max-width: 100px;"></a> 
            <span  style="left: 0px; position: absolute; background: black; color: white; cursor: pointer;">删除</span>
</div>
--%>