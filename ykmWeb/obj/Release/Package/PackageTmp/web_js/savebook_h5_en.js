function submit() {
    var data = $("#form1").serialize()
    $.post("/ajax/savebook_en", data, function (res) {
        var arr_d = res.split('|');
        if (arr_d[0] == '1') {
            layer.open({
                content: arr_d[1]
                , btn: 'I got it!'
                , yes: function (index) {
                    window.location.href = "/h5/"
                    layer.close(index);
                }
            });
        }
        else {
            layer.open({
                content: arr_d[1]
                , btn: 'I got it!'
            });
        }
    });
}

function reset() {
    $("#name").val("");
    $("#tel").val("");
    $("#content").val("");
}