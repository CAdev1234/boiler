
Validator = {
    Require: /.+/,
    Email: /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/,
    Phone: /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/,
    Mobile: /^((\(\d{2,3}\))|(\d{3}\-))?13\d{9}$/,
    Url: /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/,
    IdCard: "this.IsIdCard(value)", Currency: /^\d+(\.\d+)?$/,
    Number: /^\d+$/, Zip: /^[1-9]\d{5}$/, QQ: /^[1-9]\d{4,8}$/,
    Integer: /^[-\+]?\d+$/, Double: /^[-\+]?\d+(\.\d+)?$/,
    English: /^[A-Za-z]+$/, Chinese: /^[\u0391-\uFFE5]+$/,
    Username: /^(?!_)(?!.*?_$)[a-zA-Z0-9_\u4e00-\u9fa5]{4,20}$/,
    UnSafe: /^(([A-Z]*|[a-z]*|\d*|[-_\~!@#\$%\^&\*\.\(\)\[\]\{\}<>\?\\\/\'\"]*)|.{0,5})$|\s/,
    Date: /^\d{4}-(0?[1-9]|1[0-2])-(0?[1-9]|[12]\d|3[12])(   (0?\d|1\d|2[0-4]):(0?\d|[1-5]\d):(0?\d|[1-5]\d))?$/,
    Title: /^.{1,60}$/,
    IsSafe: function (str) { return !this.UnSafe.test(str); },
    SafeString: "this.IsSafe(value)",
    Filter: "this.DoFilter(value, getAttribute('accept'))",
    Limit: "this.limit(value.length,getAttribute('min'),  getAttribute('max'))",
    LimitB: "this.limit(this.LenB(value), getAttribute('min'), getAttribute('max'))",
    Date: "this.IsDate(value, getAttribute('min'), getAttribute('format'))",
    Repeat: "value == document.getElementsByName(getAttribute('to'))[0].value",
    Range: "getAttribute('min') < (value|0) && (value|0) < getAttribute('max')",
    Compare: "this.compare(value,getAttribute('operator'),getAttribute('to'))",
    Custom: "this.Exec(value, getAttribute('regexp'))",
    Group: "this.MustChecked(getAttribute('name'), getAttribute('min'), getAttribute('max'))",
    ErrorItem: [document.forms[0]], ErrorMessage: ["以下原因导致提交失败：\t\t\t\t"],
    Validate: function (theForm, mode) {
        var obj = theForm || event.srcElement;
        var count = obj.elements.length;
        this.ErrorMessage.length = 1;
        this.ErrorItem.length = 1;
        this.ErrorItem[0] = obj;
        for (var i = 0; i < count; i++) {
            with (obj.elements[i]) {
                var _dataType = getAttribute("dataType");
                if (typeof (_dataType) == "object" || typeof (this[_dataType]) == "undefined") continue;
                this.ClearState(obj.elements[i]); if (getAttribute("require") == "false" && value == "") continue;
                switch (_dataType) {
                    case "IdCard":
                    case "Date":
                    case "Repeat":
                    case "Range":
                    case "Compare":
                    case "Custom":
                    case "Group":
                    case "Limit":
                    case "LimitB":
                    case "SafeString":
                    case "Filter":
                        if (!eval(this[_dataType])) {
                            this.AddError(i, getAttribute("msg"));
                        }
                        break;
                    default:
                        if (_dataType == "Title") {
                            var tempValue = value.replace(/[^\x00-\xff]/g, "**");
                            if (!this[_dataType].test(tempValue)) {
                                this.AddError(i, getAttribute("msg"));
                            }
                        }
                        else {
                            if (!this[_dataType].test(value)) {
                                this.AddError(i, getAttribute("msg"));
                            } else {

                            }

                        }
                        break;
                }
            }
        }
        if (this.ErrorMessage.length > 1) {
            mode = mode || 1;
            var errCount = this.ErrorItem.length;
            switch (mode) {
                case 2:
                    for (var i = 1; i < errCount; i++) this.ErrorItem[i].style.color = "red";
                case 1:
                    layer.tips(this.ErrorMessage[1].replace(/\d+:/, "*"), $(this.ErrorItem[1]), { tips: 3 });
                    this.ErrorItem[1].focus();
                    break; //alert(this.ErrorMessage.join("\n"));
                case 3:
                    for (var i = 1; i < errCount; i++) {
                        try {
                            layer.tips(this.ErrorMessage[i].replace(/\d+:/, "*"), $(this.ErrorItem[i]), { tips: 3 });

                        }
                        catch (e) {
                            //alert(e.description);
                        }
                    }
                    if (errCount > 1) {
                        //if (this.ErrorItem[errCount - 1].type != "hidden") 
                        //this.ErrorItem[errCount - 1].focus();
                    }
                    break;
                default:
                    //alert(this.ErrorMessage.join("\n"));
                    break;
            }
            return false;
        }
        return true;
    },
    limit: function (len, min, max) {
        min = min || 0; max = max || Number.MAX_VALUE; return min <= len && len <= max;
    },
    LenB: function (str) { return str.replace(/[^\x00-\xff]/g, "**").length; },
    ClearState: function (elem) { with (elem) { if (style.color == "red") style.color = ""; var lastNode = parentNode.childNodes[parentNode.childNodes.length - 1]; if (lastNode.id == "__ErrorMessagePanel") parentNode.removeChild(lastNode); } },
    AddError: function (index, str) { this.ErrorItem[this.ErrorItem.length] = this.ErrorItem[0].elements[index]; this.ErrorMessage[this.ErrorMessage.length] = this.ErrorMessage.length + ":" + str; },
    Exec: function (op, reg) { return new RegExp(reg, "g").test(op); },
    compare: function (op1, operator, op2) { switch (operator) { case "NotEqual": return (op1 != op2); case "GreaterThan": return (op1 > op2); case "GreaterThanEqual": return (op1 >= op2); case "LessThan": return (op1 < op2); case "LessThanEqual": return (op1 <= op2); default: return (op1 == op2); } },
    MustChecked: function (name, min, max) {
        var groups = document.getElementsByName(name);
        var hasChecked = 0;
        min = min || 1;
        max = max || groups.length;
        var qtv = "";
        for (var i = groups.length - 1; i >= 0; i--) {
            if (groups[i].checked)
            {
                hasChecked++;
                if (groups[i].value == "qt") {
                    qtv = "qt";
                }
            }
        }
        if (min <= hasChecked && hasChecked <= max) {
            if (qtv == "qt") {
                if ($("#" + groups[0].name + "_qt").val() == "") {
                    return false;
                } else {
                    return true;
                }
            } else {
                return true;
            }
        } else {
            return false;
        }
       
    },
    DoFilter: function (input, filter) { return new RegExp("^.+\.(?=EXT)(EXT)$".replace(/EXT/g, filter.split(/\s*,\s*/).join("|")), "gi").test(input); },
    IsIdCard: function (number) { var date, Ai; var verify = "10x98765432"; var Wi = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2]; var area = ['', '', '', '', '', '', '', '', '', '', '', '北京', '天津', '河北', '山西', '内蒙古', '', '', '', '', '', '辽宁', '吉林', '黑龙江', '', '', '', '', '', '', '', '上海', '江苏', '浙江', '安微', '福建', '江西', '山东', '', '', '', '河南', '湖北', '湖南', '广东', '广西', '海南', '', '', '', '重庆', '四川', '贵州', '云南', '西藏', '', '', '', '', '', '', '陕西', '甘肃', '青海', '宁夏', '新疆', '', '', '', '', '', '台湾', '', '', '', '', '', '', '', '', '', '香港', '澳门', '', '', '', '', '', '', '', '', '国外']; var re = number.match(/^(\d{2})\d{4}(((\d{2})(\d{2})(\d{2})(\d{3}))|((\d{4})(\d{2})(\d{2})(\d{3}[x\d])))$/i); if (re == null) return false; if (re[1] >= area.length || area[re[1]] == "") return false; if (re[2].length == 12) { Ai = number.substr(0, 17); date = [re[9], re[10], re[11]].join("-"); } else { Ai = number.substr(0, 6) + "19" + number.substr(6); date = ["19" + re[4], re[5], re[6]].join("-"); } if (!this.IsDate(date, "ymd")) return false; var sum = 0; for (var i = 0; i <= 16; i++) { sum += Ai.charAt(i) * Wi[i]; } Ai += verify.charAt(sum % 11); return (number.length == 15 || number.length == 18 && number == Ai); },
    IsDate: function (op, formatString) { formatString = formatString || "ymd"; var m, year, month, day; switch (formatString) { case "ymd": m = op.match(new RegExp("^((\\d{4})|(\\d{2}))([-./])(\\d{1,2})\\4(\\d{1,2})$")); if (m == null) return false; day = m[6]; month = m[5] * 1; year = (m[2].length == 4) ? m[2] : GetFullYear(parseInt(m[3], 10)); break; case "dmy": m = op.match(new RegExp("^(\\d{1,2})([-./])(\\d{1,2})\\2((\\d{4})|(\\d{2}))$")); if (m == null) return false; day = m[1]; month = m[3] * 1; year = (m[5].length == 4) ? m[5] : GetFullYear(parseInt(m[6], 10)); break; default: break; } if (!parseInt(month)) return false; month = month == 0 ? 12 : month; var date = new Date(year, month - 1, day); return (typeof (date) == "object" && year == date.getFullYear() && month == (date.getMonth() + 1) && day == date.getDate()); function GetFullYear(y) { return ((y < 30 ? "20" : "19") + y) | 0; } }
}

var hidden_back=function(){
	this.band=function(o){
	    $("#" + o + " input").each(function () {

	        if ($(this).attr("type") == "text" ||  $(this).attr("type") == "textarea" || $(this).attr("type") == "password") {
	            $(this).keyup(function () { $(this).css("background", "#ffffff") });
	        }
	    });
	    $("#" + o + " textarea").each(function () {
	            $(this).keyup(function () { $(this).css("background", "#ffffff") });
	    });
	}
}


var re_post_bar = {
    postthis: function (t, data) {
        var index = layer.msg('正在提交，请稍后！', { icon: 16, time: 0 });
  
        $.post("/ajax/post?t=" + t, data, function (data) {//data 状态1or2|type1or2|hrefortext
            var arr_d = data.split('|');
            if (arr_d[0] == "1") {
                layer.close(index);
                if (arr_d[1] == "1") {
                    layer.alert("提交成功！", function () {
                        window.location.href = arr_d[2]
                    })

                }
                else if (arr_d[1] == "2") {
                
                    window.parent.location.reload();
                }
                else {
                    layer.close(index);
                    layer.alert(arr_d[2]);
                }
            }
            else {
                layer.close(index);
                layer.alert(arr_d[1]);
                re_post_bar.reset_code("coide", "/admincode");
            }
        });
    },
    submit: function (t, formid) {
        switch (t) {
            default:
                var bret = Validator.Validate($('#' + formid)[0], 1);
                if (bret) {
                    var data = $("#" + formid).serialize();
            
                    re_post_bar.postthis(t, data);
                }
                break;
            case "repass":
                var data = { id: formid };
                re_post_bar.postthis(t, data);
                break;

        }

    }, reset_code: function (id, src) {
        $("#" + id).attr("src",src+"?t=" + Math.random());
    },
    checkform: function (formid) {
        var bret = Validator.Validate($('#' + formid)[0], 1);
        if (bret) {
            return true;
        }
        else {
            return false;
        }
    }
}