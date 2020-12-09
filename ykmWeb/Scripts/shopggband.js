var shop_spec_do = function () {
    var i = 0;
    var pid = 0;
    var mytr = new Array()
    var getval = function () {
        var r = "";
        $(mytr[i]).children("td").children("input").each(function () {
            r += "&" + $(this).attr("name") + "=" + $(this).val();
        });
        r += "&i=" + i + "&pid=" + pid;
    
        return r;
    }
    var send = function () {
        var text = "正在提交第" + (i + 1) + "条规格信息，一共有" + mytr.length + "条规格信息";
     var lay=   layer.msg(text, {
            icon: 16
            , shade: 0.01
        });
 
     var dataval = getval();

        $.ajax({
            type: "POST",
            url: "/management/shopgg/doup",
            data: dataval ,
            contentType: "application/x-www-form-urlencoded",
            success: function (rdata) {
                if (rdata == "y") {
                    if ((i+1)  == mytr.length) {

                        window.location.reload();
                    } else {
                        i++;
                        send();
                    }
                }
                else {
                    layer.alert(rdata);
                }
              
            },
            error: function (r) {
                alert("错误");
            }
        });
    }
    this.start = function (o) {
        pid = $("#pid").val();
        $("." + o + " table tbody tr").each(function () {
      
            mytr.push($(this));
        });
        send();
    }
}
