Array.prototype.contains = function (needle) {
    for (i in this) {
        if (this[i] == needle) return true;
    }
    return false;
}
var ctrl_choose_cont = function () {
    var formid = "";
    var divid = "";
    var htmlTemp = "<li>[title]<span class=\"btn btn-default btn-sm\" data-val=\"[val]\" onclick=\"delval(this,'[t]')\" >删除</span></li>";
    var set_val = function (title, id) {
        if ($(formid).val() == ""){
            var htmlTemp2 = htmlTemp.replace("[title]", title).replace("[val]", id).replace("[t]", divid);
            $(divid).append(htmlTemp2)
            $(formid).val(id);
        }
        else
        {
            var o = $(formid).val().split(',');
            if (o.contains(id) == false) {
                o.push(id);
                var htmlTemp2 = htmlTemp.replace("[title]", title).replace("[val]", id).replace("[t]", divid);
                $(divid).append(htmlTemp2)
                $(formid).val(o.join());
            }
        }
    }

    var del_html = function (o) {
        $(o).parent().remove();
    }



    var delval = function (id) {
        var ary = $(formid).val().split(',');
        ary.splice($.inArray(id, ary), 1);
        $(formid).val(ary.join());
    }

    this.init = function (_formid,_divid) {
        formid = _formid;
        divid = _divid;
    }

    this.add = function (id,title) {
        set_val(title,id);
    }

    this.del = function (obj) {
        var id = $(obj).data("val");
        delval(id);
        del_html(obj);
    }


}


