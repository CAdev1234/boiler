var batch_ctrl = function () {
    var selectall = function (checkallbox) {//全选
        var ck = document.getElementsByName("ck");
        for (var i = 0; i < ck.length; i++) {
            ck[i].checked = checkallbox.checked;
        }
    }
    var getvalue = function () { //获取选择的内容
        var str = "";
        var ck = document.getElementsByName("ck");
        for (var i = 0; i < ck.length; i++) {
            if (ck[i].checked == true) {
                str += ck[i].value + ",";
            }
        }
        if (str != "") {
            str = str.substring(0, str.length - 1);
        }
        return str;
    }
    var postdata = function (url, d) {
        var index = layer.msg('加载中', {
            icon: 16,time:0
            , shade: 0.4
        });
        $.ajax({
            type: "POST",
            url: url,
            data: d,
            contentType: "application/x-www-form-urlencoded",
            success: function (rdata) {
                if (rdata == "") {
                    layer.alert('更新成功', {
                        closeBtn: 0
                    }, function () {
                        window.location.reload();
                    });
                }
                else {
                    layer.alert(rdata);
                }
            }
        });
    }
    $(document).ready(function () {
        $(".batch_ckall").click(function () {
            selectall(this);
        });
        $(".batch-delall").click(function () {
            var url = $(this).data("batchurl");

            var val = getvalue();
      
            if (val == "") {
                layer.alert("没有选择到任何内容")
                return;
            }
            if (confirm("确定要删除吗，操作不可恢复!") == true) {
                postdata(url, { id: val });
            }
        });
        $(".batch-changeclass").click(function () {
            var url = $(this).data("batchurl");
            var val = getvalue();
      
            var classid = $("#changecid").val();
            if (classid == "") {
                layer.alert("请选择要更换的分类")
                return;
            }
            if (val == "") {
                layer.alert("没有选择到任何内容")
                return;
            }
            postdata(url, { id: val, cid: classid});
        });

        $(".batch-changestate").click(function () {
            var url = $(this).data("batchurl");
            var state = $("#cstate").val();
            if (state=="") {
                layer.alert("没有选择更改状态")
                return;
            }
            var val = getvalue();

            if (val == "") {
                layer.alert("没有选择到任何内容")
                return;
            }
            postdata(url, { id: val, state: state });
        });

        $("#setchoose").click(function () {
            var val = getvalue();
            if (val == "") {
                layer.alert("没有选择到任何内容")
                return;
            }
            parent.setChooseall(val);
            
        })
        
      
    });
}
batch_ctrl();
