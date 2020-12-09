//初始化fileinput
var FileInput = function () {
    var oFile = new Object();
    //初始化fileinput控件（第一次初始化）
    oFile.Init = function (ctrlName, fname) {
        var control = $('#' + ctrlName);
        //初始化上传控件的样式
        control.fileinput({
            language: 'zh', //设置语言
            uploadUrl: "/management/uploadfiles/Index", //上传的地址
            allowedFileExtensions: ['jpg', 'gif', 'png','rar'],//接收的文件后缀
            showUpload: false, //是否显示上传按钮
            dropZoneEnabled: false,
            showRemove: false,
            showCancel: false,
            showClose:false,
            uploadAsync: true,
            showCaption: false,//是否显示标题
            showPreview: false,
            browseClass: "btn btn-info", //按钮样式     
            dropZoneEnabled: false,//是否显示拖拽区域
            //minImageWidth: 50, //图片的最小宽度
            //minImageHeight: 50,//图片的最小高度
            //maxImageWidth: 1000,//图片的最大宽度
            //maxImageHeight: 1000,//图片的最大高度
            maxFileSize: 0,//单位为kb，如果为0表示不限制文件大小
            //minFileCount: 0,
            maxFileCount: 10, //表示允许同时上传的最大文件个数
            enctype: 'multipart/form-data',
            validateInitialCount: true,
            previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
            msgFilesTooMany: "选择上传的文件数量({n}) 超过允许的最大数值{m}！"
        }).on("filebatchselected", function (event, files) {
            $(this).fileinput("upload");
        }).on('fileuploaded', function (event, data, previewId, index) {
            // var form = data.form, files = data.files, extra = data.extra, response = data.response, reader = data.reader;
            var fna = data.response.result.split('|');
            if (fna[0] == "ok") {
                fname(fna[1]);

            }
            else {
                alert(fna[1]);
            }
         
          
           
        });
    }

    return oFile;
};

