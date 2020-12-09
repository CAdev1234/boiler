var showfile = function () {
    var temp1 = "<div class=\"upimg\"><div class=\"img-thumbnail\"><img src=\"/images/icon-file.png\" data-src=\"{imgtitle}\" class=\"priveimg\"/></div><div class=\"imgtitle\">{imgtitle}</div><div class=\"imgdel\">删除</div></div>";
    var def = "";
    var upf = "";
    var htmlbox = "";
    var set_upfiles = function (f) {
        var i = $("#" + upf).val();
        if (i == "") {
            $("#" + upf).val(f);
        }
        else {
            $("#" + upf).val(i + "|" + f);
        }
    }

    var set_def = function (f) {
        $("#" + def).val(f);
    }
    var del_img = function (f) {
        var defimg = $("#" + def).val();
        if (defimg == f) {
            $("#" + def).val("");
        }
        var up = $("#" + upf).val();
        if (up != "") {
            var arr = up.split('|');
            $("#" + upf).val("");

            for (var i = 0; i < arr.length; i++) {
                if (arr[i] != f) {
                    set_upfiles(arr[i]);
                }
            }
        }
        load_del();
    }
    var showhtml = function () {
        var up = $("#" + upf).val();
        //var up = $("#" + def).val();
        console.log(def)
        if (up != "") {
            var arr = up.split('|');
            var _str = "";
            for (var i = 0; i < arr.length; i++) {
                //_str += temp1.replace("{imgurl}", arr[i]);
                _str += temp1.myReplace("{imgtitle}", arr[i]);
            }
            $("#" + htmlbox).html(_str);
        }
    }
    var load_del = function () {
        $("#" + htmlbox + " div.upimg").each(function () {
            var o = this;
            $(o).children("div.imgdel").unbind("click");
            $(o).children("div.imgdel").click(function () {
                //del_img($(o).children().children("img").attr("src"));
                del_img($(o).children().children("img").attr("data-src"));
                $(o).remove();
            });
        });
        $("#" + htmlbox + " img.priveimg").each(function () {
            $(this).unbind("click");
            $(this).click(function () {
                set_def($(this).attr("src"));
            });
        })
    }

    this.init = function (_def, _upf, _htmlbox) {
        def = _def;
        upf = _upf;
        htmlbox = _htmlbox;
    }

    this.show_img = function (files) {
        if (files == "") {
            showhtml();
            load_del();
        } else {
            set_def(files);
            set_upfiles(files);
            showhtml();
            load_del();
        }

    }



}

String.prototype.myReplace = function (f, e) {//吧f替换成e
    var reg = new RegExp(f, "g"); //创建正则RegExp对象   
    return this.replace(reg, e);
}
