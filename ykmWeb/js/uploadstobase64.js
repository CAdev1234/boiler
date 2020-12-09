function uploadFile() {
    return $("#file").click();
}
function uploadGallery() {
    return $("#gallery").click();
}
function showImg(t) {
    //var file = document.getElementById("file").files[0];
    var file = t.files[0];
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function (e) {
        var imgBase64Data = e.target.result;
        //alert(imgBase64Data)
        //console.info(imgBase64Data);
        //$("img").rem6 .ove();
        //$("#show").append("<img src=\"" + imgBase64Data + "\"/>");
        //var head = imgBase64Data.indexOf("4") + 2;
        //imgBase64Data = imgBase64Data.substring(head, imgBase64Data.length - head);
        //console.info(imgBase64Data);
        //$("#imgData").attr("value", imgBase64Data);
        $("#" + t.id + "_list").append(getImgHtml(imgBase64Data));
        fileClear(t);
    }
}

function getImgHtml(imgUrl) {
    var head = imgUrl.indexOf("4") + 2;
    var imgBase64Data = imgUrl.substring(head, imgUrl.length - head);
    return "<div class=\"picbox\"><input type=\"hidden\" name=\"img\" value=\"" + imgBase64Data + "\" /><img class=\"col-sm-2\" src=\"" + imgUrl +"\" /><textarea class=\"col-sm-10\" name=\"txt\" maxlength=\"1000\" placeholder=\"图片注释（字数限制1000）\"></textarea><a class=\"btn btn-primary\" onclick=\"delImgHtml(this)\">删除</a></div>";
}
function delImgHtml(t) {
    t.parent().remove();
}
function fileClear(t) {
    if (t.outerHTML) {
        t.outerHTML = t.outerHTML;
    } else {
        t.value = "";
    }
}


//function add_gg() {
//    var html = "";
//    html += "<li class=\"flex m-b\">";
//    html += "    <input name=\"ggid\" type=\"hidden\" value=\"0\">";
//    html += "    <div class=\"w30\">";
//    html += "        <select name=\"gg\" class=\"form-control\">";
//    html += "@Html.Raw(ViewData["gg_class_list"])";
//    html += "        </select>";
//    html += "    </div>";
//    html += "    <div class=\"w10 m-l\">";
//    html += "        <label for=\"kc\">单价</label>";
//    html += "    </div>";
//    html += "    <div class=\"w30\">";
//    html += "        <input type=\"text\" class=\"form-control\" name=\"ggprice\" placeholder=\"\" value=\"\" required data-bv-notempty-message=\"规格单价不能为空！\">";
//    html += "    </div>";
//    html += "    <div class=\"w10 m-l\">元</div>";
//    html += "    <div class=\"w10 m-l\"><a href=\"javascript:;\" onclick=\"$(this).parent().parent().remove();\">删除</a></div>";
//    html += "</li>";
//    $("#gg_list").append(html);
//}

function get_arr_val() {
    var id = $("#id").val();
    var arr = [];
    $("input[name='ggid']").each(function (i) {
        var gg = $("select[name='gg']").eq(i);
        var price = $("input[name='ggprice']").eq(i);
        arr.push({ "id": $(this).val(), "pro_id": id, "gg_id": gg.val(), "price": price.val() });
    });
    //return arr;
    $("#ggarr").val(JSON.stringify(arr));
}