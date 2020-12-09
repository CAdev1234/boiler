function submit() {
    var data = $("#form1").serialize()
    $.post("/ajax/savebook", data, function (res) {
        var arr_d = res.split('|');
        if (arr_d[0] == '1') {
            layer.alert(arr_d[1], function () {
                window.location.href = "/"
            });
        }
        else {
            layer.alert(arr_d[1]);
        }
    });
}

function reset() {
    $("#name").val("");
    $("#tel").val("");
    $("#sex").val("");
    $("#email").val("");
    $("#cont").val("");
}

function changeimg() {
    $('#yzm').attr('src', '/management/getCheckCode?t=' + Math.random());
}