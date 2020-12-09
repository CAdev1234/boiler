function turnPage(ym, wh) {
    $.ajax({
        type: "POST",
        url: "/ajax/trunPage",
        data: "pageid=" + ym + "&" + wh,
        success: function (msg) {
            if (msg != "false") {
                var arr = msg.split("{+-*/}");
                $("#MainList").html(arr[0]);
                $("#PageFoot").html(arr[1]);
            }
            else {
                alert("已经到最后了！");
            }
        }
    });
}

function turnPageH5(ym, wh) {
    $.ajax({
        type: "POST",
        url: "/ajax/trunPageH5",
        data: "pageid=" + ym + "&" + wh,
        success: function (msg) {
            if (msg != "false") {
                var arr = msg.split("{+-*/}");
                $("#MainList").html(arr[0]);
                $("#PageFoot").html(arr[1]);
            }
            else {
                alert("已经到最后了！");
            }
        }
    });
}