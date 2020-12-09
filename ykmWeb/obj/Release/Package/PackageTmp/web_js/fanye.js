//function fanye(pn, t, u) {
//    var ym = parseInt($("#fy").val());
//    if (pn > ym) {
//        $.ajax({
//            type: "POST",
//            url: "/ajax/fanye",
//            data: "p=" + ym + "&t=" + t + "&u=" + u,
//            success: function (msg) {
//                ym = ym + 1;
//                $("#fy").val(ym);
//                if (msg != "false") {
//                    $("#addlist").append(msg);
//                }
//                else {
//                    alert("已经到最后了！");
//                }
//            }
//        });
//    }
//    else {
//        alert("已经到最后了！");
//    }
//}

function fanye(cid, pn) {
    var ym = parseInt($("#fy").val());
    if (pn > ym) {
        $.ajax({
            type: "POST",
            url: "/ajax/fanye",
            data: "pageid=" + ym + "&cid=" + cid,
            success: function (msg) {
                ym = ym + 1;
                $("#fy").val(ym);
                if (msg != "false") {
                    $("#addlist").append(msg);
                }
                else {
                    //alert("没有更多了！");
                    $(".more").text("没有更多了！");
                }
            }
        });
    }
    else {
        $(".more").text("没有更多了！");
    }
}


//重新加载
function reload_fy() {
    if (document.all.fy.value != "1") {
        document.all.fy.value = "1";
    }
}