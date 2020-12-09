var buycar = function () {
    var btnid = "";
    var band = function () {
        $("#" + btnid).click(function () {
            var pid = $("#pid").val();
            var sl = $("#sl").val();
            var selluid = $("#selluid").val();
            var gg = $("#gg").val();
            var yhid = $("#yhid").val();
            var dataval = { pid: pid, sl: sl, selluid: selluid, gg: gg, yhid: yhid };
            $.ajax({
                type: "POST",
                url: "/shopcar/add",
                data: dataval,
                contentType: "application/x-www-form-urlencoded",
                success: function (rdata) {
                    $("#loddiv").load("/shopcar/list?t=" + Math.random());
                },
                error: function (r) {
                    alert("错误");
                }
            });
        });
    }
    this.start = function (_btnid) {
        btnid = _btnid;
        band();
    }
}