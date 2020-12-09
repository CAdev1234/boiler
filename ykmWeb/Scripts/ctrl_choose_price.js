var ctrl_choose_price = function () {

    var dwstr = "";
    var varstr = "";

    var divid = "";
    var i = 0;
    var htmlTemp = "<div class=\"row content\"><div class=\"col-md-3\"><div class=\"form-group\"><input type=\"text\"class=\"form-control checkform\"name=\"maxnum\" value=\"[maxval]\"  /></div></div><div class=\"col-md-3\"><div class=\"form-group\"><select name=\"dw\" class=\"form-control checkform\"><option value=\"\">请选择</option>[optionlist]</select></div></div><div class=\"col-md-3 \"><div class=\"form-group\"><input type=\"text\" class=\"form-control checkform\" name=\"price\" value=\"[priceval]\"/></div></div><div class=\"col-md-3\"><button type=\"button\"class=\"btn btn-danger btn-sm\" onclick=\"delprice(this)\">删除价格</button><input type=\"hidden\"name=\"price_savetype\" value=\"[savetype]\"/><input type=\"hidden\" name=\"priceid\" value=\"[priceid]\"/></div></div>";
    var init = function () {
        if (varstr != "") {
            var obj = JSON.parse(varstr);
            if (obj != null) {
                for (var o in obj) {
                    var tempstr = htmlTemp.replace("[maxval]", obj[o].maxbuynum).replace("[optionlist]", create_option_str(obj[o].dw)).replace("[priceval]", obj[o].price).replace("[priceid]", obj[o].id).replace("[savetype]", "e");
                
                    $(divid).append(tempstr);
                    i++;
                }
            }
        }
    }

    this.start = function (_dwstr, _varstr, _divid) {
        dwstr = _dwstr;
        varstr = _varstr;
        divid = _divid;
        init();
        band_input_nr();
    }

    this.add = function () {
     
        var tempstr = htmlTemp.replace("[maxval]", "").replace("[optionlist]", create_option_str("")).replace("[priceval]", "").replace("[priceid]", i).replace("[savetype]", "s");
        console.log(tempstr);
        $(divid).append(tempstr);
        i++;
        band_input_nr();
    }

    this.del = function (btnObj) {
        $(btnObj).parent().parent().remove();
    }

    var create_option_str = function (focusVal) {
        var optionstr = "";
        if (dwstr != "") {
            var obj = JSON.parse(dwstr); 
            if (obj == null) {
                alert("字符串转换错误");
            }
            else {
                for (var p in obj) {//遍历json数组时，这么写p为索引，0,1
                    if (focusVal == obj[p].mn) {
                        optionstr += "<option value=\"" + obj[p].mn + "\" selected >" + obj[p].mn + "</option>";
                    }
                    else {
                        optionstr += "<option value=\"" + obj[p].mn + "\">" + obj[p].mn + "</option>";
                    }
                }
            }
        }
        return optionstr;
    }

    var checknum = function (val) {
      
     
            if (val === "" || val == null) {
                return false;
        }

            if (!isNaN(val)) {
                return true;
            } else {
                return false;
            }
    }
    var checkkong = function (val) {
        if (val === "" || val == null) {
            return false;
        }
    }

    var band_input_nr = function () {
        $(".checkform").each(function () {
                $(this).unbind()
                $(this).bind("blur", function () {
                if ($(this).attr("name") == "dw") {
                    if (checkkong($(this).val()) == false) {
                        layer.tips('不能为空', this, {
                            tips: 3
                        });
                    }
                }
                else if ($(this).attr("type") == "text") {
                    if (checknum($(this).val()) == false) {
                        layer.tips('请输入数字', this, {
                            tips: 3
                        });
                    }
                }
             });
        })
    }
    
}