var shop_class_nclass =function() {
    this.viewpagename = "";
    var _viewp = "";
    var tablename = "";
    var language = "";
    var tableType = "";
    var ischoose = false;
    var modelsHtml =("<div class=\"modal fade\"id=\"myModal\"tabindex=\"-1\"role=\"dialog\"aria-labelledby=\"myModalLabel\"aria-hidden=\"true\"><div class=\"modal-dialog\"><div class=\"modal-content\"><div class=\"modal-body\"><iframe src=\"\"width=\"100%\"height=\"600\"scrolling=\"auto\"frameborder=\"0\"id=\"ctrlbody\"name=\"ctrlbody\"></iframe></div><div class=\"modal-footer\"><button type=\"button\"class=\"btn btn-default\" id=\"closemodal\">关闭</button></div></div></div></div>");
    var showdioag = function (src) {
        if ($("#myModal").length <= 0) {
            $("body").append(modelsHtml);
            $("#closemodal").click(function () {
                deldioag();
            });
        }
  
        $("#ctrlbody").attr("src", src);
            $('#myModal').modal({
                show: true
            });
    }
    var deldioag = function () {
        if ($("#myModal").length > 0) {   
            $('#myModal').modal('hide');
        }
    }
    var get_tabname = function () {
        tablename = $("#tablename").val();
    }
    var showchild = function (pid, o) {
        if (ischoose == true) {
            $.get("/management/nclass/showshopclasschild?tn=" + tablename + "&pid=" + pid + "&lang=" + language + "&tableType=" + tableType + "&choo=1&r=" + Math.random(), function (r) {
                $(o).html(r);
                band_eval(o);
            });
        } else {
            $.get("/management/nclass/showshopclasschild?tn=" + tablename + "&pid=" + pid + "&lang=" + language + "&r=" + Math.random(), function (r) {
                $(o).html(r);
                band_eval(o);
            });
        }
  
    }
    //var getLanguAge = function () {

    //    if (language == "") {
    //        language = $("#lang").val();
    //    }

    //    if (tableType == "") {
    //        tableType = $("#tableType").val();
    //    }

    //    $(document).ready(function () {
    //        $("#myTab").children("li").each(function () {
    //            $(this).click(function () {
    //                language = $(this).children("a").data("val");
    //                $("#lang").val(language);
    //                showchild("0", $("#p0"));
                   
    //            });
    //        });
    //    });
    //};

   

    var band_eval = function (o) {
        get_tabname();
        $(o).children("div.nclass").each(function () {
            var pid = $(this).data("pid");
            var cid = $(this).data("cid");
            $(this).children("div.titleclass").click(function () {//显示子分类部分
                var o1 = $("#p" + cid);
               
                if ($(o1).children().length==0) {
               
                    showchild(cid,o1);
                    $("#ico" + cid).removeClass("glyphicon-plus").addClass("glyphicon-minus");
                }
                else {
                    $(o1).children("div.nclass").remove();
                    $("#ico" + cid).removeClass("glyphicon-minus").addClass("glyphicon-plus");
                }
            });
            if (ischoose) {
                $(this).children("div.ctrlclass").children("button.choose").click(function () {
                    var catalogname = $(this).data("catalogname");
                    var ptypelink = $("#ptype").val();
                    if (ptypelink == "setlink" || ptypelink == "setH5Link") {
                        set_class(cid, catalogname);
                    }
                    else {
                        parent.set_class(cid, catalogname, $("#ptype").val());
                    }
                });
            }
            else {
                //添加
                $(this).children("div.ctrlclass").children("a.addc").click(function () {
                    showdioag("/management/" + _viewp + "/addchild?pid=" + cid + "&lang=" + language + "&a=a");
                });
                //修改
                $(this).children("div.ctrlclass").children("a.setc").click(function () {
                    showdioag("/management/" + _viewp+"/addchild?cid=" + cid + "&a=e");
                });
                //上移
                $(this).children("div.ctrlclass").children("a.movet").click(function () {
                    
                    $.ajax({
                        type: "POST",
                        url: "/management/nclass/Dosort",
                        data: { pid: pid, tn: tablename, cid: cid, s: "t" },
                        contentType: "application/x-www-form-urlencoded",
                        success: function (r) {

                            $("#p" + pid).children("div.nclass").remove();
                            showchild(pid, $("#p" + pid));
                        },
                        error: function (r) {
                            alert("错误");
                        }
                    });
                });
                //下移
                $(this).children("div.ctrlclass").children("a.moved").click(function () {
                    $.ajax({
                        type: "POST",
                        url: "/management/nclass/Dosort",
                        data: { pid: pid, tn: tablename, cid: cid, s: "d" },
                        contentType: "application/x-www-form-urlencoded",
                        success: function (r) {

                            $("#p" + pid).children("div.nclass").remove();
                            showchild(pid, $("#p" + pid));
                        },
                        error: function (r) {
                            alert("错误");

                        }
                    });
                });
                //删除
                $(this).children("div.ctrlclass").children("a.delc").click(function () {
                    if (confirm("确定要删除分类吗？操作不可恢复!!")) {
                        $.get("/management/nclass/del?tn=" + tablename + "&cid=" + cid + "&pid=" + pid, function (r) {
                            if (r == "1") {
                                $("#p" + pid).children("div.nclass").remove();
                                showchild(pid, $("#p" + pid));
                            } else {

                                alert("请先删除当前分类下的子类");
                            }
                        });
                    }
                });
                //设置连接
                $(this).children("div.ctrlclass").children("button.linkc").click(function () {
                    showdioag("/management/" + _viewp + "/setLink?cid=" + cid + "&a=e");
                });
                //设置连接
                $(this).children("div.ctrlclass").children("button.linkh").click(function () {
                    showdioag("/management/" + _viewp + "/setH5Link?cid=" + cid + "&a=e");
                });
            }
        });

    }

    this.info_list_class = function (rq,_tableType) {
        _viewp = this.viewpagename;
        tableType = _tableType;
        showdioag("/management/" + _viewp + "/nclass_view_list?ptype=" + rq + "&tt=" + tableType);
    }

    //ykm-2019.6.4-选择活动内项目
    this.choose_list = function () {
        _viewp = this.viewpagename;
        showdioag("/management/" + _viewp + "/choose");
    }

    this.info_hide_class = function () {
        deldioag();
    }

    this.start = function (o) {
        _viewp = this.viewpagename;
        get_tabname();
        //getLanguAge();
        showchild("0", o);
    }

    this.showchoose = function (o) {
        ischoose = true;
        get_tabname();
        //getLanguAge();
        showchild("0", o);
    }
    this.updatehtml=function(pid)
    {
        showchild(pid, $("#p" + pid));
        deldioag();
    }
    this.add_one_class = function () {
        showdioag("/management/" + _viewp + "/addchild?pid=0&lang=" + language + "&a=a");
    }
}


