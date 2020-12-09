function submit() {
    var data = $("#form1").serialize();
    var nowqua_value = [];
    var planqua_value = [];
    $('input[name="nowqua"]:checked').each(function () {
        nowqua_value.push($(this).val());
    }); 
    $('input[name="planqua"]:checked').each(function () {
        planqua_value.push($(this).val());
    }); 
    data.nowqua = nowqua_value;
    data.planqua = planqua_value;
    $.post("/ajax/regform", data, function (res) {
        var arr_d = res.split('|');
        if (arr_d[0] == '1') {
            layer.open({
                content: arr_d[1]
                , btn: '我知道了'
            });
        }
        else {
            layer.open({
                content: arr_d[1]
                , btn: '我知道了'
            });
        }
    });
}

function reset() {
    $("#company").val("");
    $("#content").val("");
    $("#lisgfile").html("");
    $("#downloadfile").val("");
}