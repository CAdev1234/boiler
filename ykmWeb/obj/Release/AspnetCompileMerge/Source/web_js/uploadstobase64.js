function uploadFile() {
    $("#file").trigger("click");
}
function showPic(t) {
    if (!/\.(gif|jpg|jpeg|png|GIF|JPG|PNG)$/.test(t.value)) {
        alert("图片类型必须是.gif,jpeg,jpg,png中的一种");
        return false;
    } 
    var file = t.files[0];
    if ((file.size / 1024 / 1024).toFixed(0) < 10) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function (e) {
            var imgBase64Data = e.target.result;
            var head = imgBase64Data.indexOf("4") + 2;
            var imgUrl = imgBase64Data.substring(head, imgBase64Data.length - head);
            $("#" + t.id + "_img").attr("src", imgBase64Data);
            //$("#frontcover").val(imgUrl);
            $("#frontcover").val(imgBase64Data);
            fileClear(t);
        }
    }
    else {
        alert("上传的图片超过1.5M")
    }
}

function showHeadImg(t) {
    if (!/\.(gif|jpg|jpeg|png|GIF|JPG|PNG)$/.test(t.value)) {
        alert("图片类型必须是.gif,jpeg,jpg,png中的一种");
        return false;
    }
    var file = t.files[0];
    if ((file.size / 1024 / 1024).toFixed(0) < 1.5) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function (e) {
            var imgBase64Data = e.target.result;
            var head = imgBase64Data.indexOf("4") + 2;
            var imgUrl = imgBase64Data.substring(head, imgBase64Data.length - head);
            $("#" + t.id + "_img").attr("src", imgBase64Data);
            //$("#frontcover").val(imgUrl);
            $("#headimg").val(imgBase64Data);
            fileClear(t);
        }
    }
    else {
        alert("上传的图片超过1.5M")
    }
}

function showBackGroundImg(t,str) {
    if (!/\.(gif|jpg|jpeg|png|GIF|JPG|PNG)$/.test(t.value)) {
        alert("图片类型必须是.gif,jpeg,jpg,png中的一种");
        return false;
    }
    var file = t.files[0];
    if ((file.size / 1024 / 1024).toFixed(0) < 1.5) {
        var files = $('#file').prop('files');
        var data = new FormData();
        data.append('file', files[0]);
        $.ajax({
            url: "/getuploadfiles/uploads?t=" + str,
            type: 'POST',
            data: data,
            processData: false,// ⑧告诉jQuery不要去处理发送的数据
            contentType: false, // ⑨告诉jQuery不要去设置Content-Type请求头
            beforeSend: function () {
                //⑩发送之前的动作
                //alert("我还没开始发送呢");
            },
            success: function (responseStr) {
                //11成功后的动作
                $("#file_img").css("background-images", responseStr);
                alert("成功啦");
                fileClear(t);
            }
            ,
            error: function (responseStr) {
                console.log(responseStr)
                //12出错后的动作
                //alert("出错啦");
                fileClear(t);
            }
        });
    }
    else {
        alert("上传的图片超过1.5M")
    }
}


function uploadGallery() {
    return $("#gallery").click();
}
function showImg(t) {
    var length = $("#" + t.id + "_list li").length;
    var addlength = t.files.length;
    if ((length + addlength) > 21) {
        layer.msg("仅限上传20张图片")
    }
    else {
        for (var i = 0; i < t.files.length; i++) {
            (function (i) {
                setTimeout(function () {
                    var file = t.files[i];
                    if ((file.size / 1024 / 1024).toFixed(0) < 1.5) {
                        var reader = new FileReader();
                        reader.readAsDataURL(file);
                        reader.onload = function (e) {
                            var imgBase64Data = e.target.result;
                            $("#" + t.id + "_list").append(getImgHtml(imgBase64Data));
                            fileClear(t);
                        }
                    }
                    else {
                        //console.log("上传的图片超过1.5M")
                        layer.msg("上传的图片超过1.5M")
                    }
                }, (i + 1) * 500);
            })(i);
        }
    }


    //if (!/\.(gif|jpg|jpeg|png|GIF|JPG|PNG)$/.test(t.value)) {
    //    alert("图片类型必须是.gif,jpeg,jpg,png中的一种");
    //    return false;
    //} 
    //var file = t.files[0];
    //if ((file.size / 1024 / 1024).toFixed(0) < 1.5) {
    //    var reader = new FileReader();
    //    reader.readAsDataURL(file);
    //    reader.onload = function (e) {
    //        var imgBase64Data = e.target.result;
    //        $("#" + t.id + "_list").append(getImgHtml(imgBase64Data));
    //        fileClear(t);
    //    }
    //}
    //else {
    //    alert("上传的图片超过1.5M")
    //}
}
function getImgHtml(imgUrl) {
    //var head = imgUrl.indexOf("4") + 2;
    //var imgBase64Data = imgUrl.substring(head, imgUrl.length - head);
    var imgBase64Data = imgUrl;
    var length = $("#gallery_list li").length;
    var css = (length % 4 == 3) ? " class=\"last\"" : "";
    return "<li" + css + "><input type=\"hidden\" name=\"galleryid\" value=\"0\" /><div class=\"hide\"><div class=\"arrow\"><img src=\"/web_images/upbg.png\" alt=\"\" /></div><div class=\"txt\"><textarea name=\"notes\" maxlength=\"1000\"></textarea><span>字数限制 1000</span></div><a href=\"javascript:;\" onclick=\"hideNotes(this)\">保存</a></div><div class=\"img\"><input type=\"hidden\" name=\"photourl\" value=\"" + imgBase64Data + "\" /><img src=\"" + imgUrl + "\" alt=\"\" /></div><div class=\"del\" onclick=\"delImgHtml(this)\">删除</div><div class=\"add\" onclick=\"showNotes(this)\">添加注释</div></li>";
}
function delImgHtml(t) {
    $(t).parent().remove();
}
function fileClear(t) {
    if (t.outerHTML) {
        t.outerHTML = t.outerHTML;
    } else {
        t.value = "";
    }
}
function showNotes(t) {
    $(t).siblings(".hide").show().parents("li").siblings().children(".hide").hide();
}
function hideNotes(t) {
    $(t).parent().hide();
}

function get_arr_val() {
    var id = $("#id").val();
    console.log(id)
    var arr = [];
    $("input[name='galleryid']").each(function (i) {
        var photourl = $("input[name='photourl']").eq(i);
        var notes = $("textarea[name='notes']").eq(i);
        arr.push({ "id": $(this).val(), "infoid": id, "photourl": photourl.val(), "notes": notes.val(),"infodate":"" });
    });
    console.log(arr)
    //return arr;
    $("#galleryarr").val(JSON.stringify(arr));
}




//验证文件的格式
function checkfile(filenames) {
    filename = document.getElementById(filenames).value;

    var re = /(.[.]bmp)$|(.[.]gif)$|(.[.]jpg)$|(.[.]png)$|(.[.]jpeg)$/i;
    if (!re.test(filename)) {
        errorinfo = "只支持bmp,gif,jpg,png,jpeg格式文件，请重新上传";
        alert(errorinfo);
        flag = false;
        return false;
    }

    //验证文件的大小
    if (document.getElementById(filenames).size > 15000) {
        errorinfo = "文件必须小于1.5M,图片太大 size:" + document.getElementById(filenames).size;
        alert(errorinfo);
        return false;
    }
}
