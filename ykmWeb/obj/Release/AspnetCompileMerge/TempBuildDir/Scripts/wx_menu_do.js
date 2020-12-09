var wx_menu = function () {
    set_form_v=function(obj,savetype){
        $("#mename").val(obj.name);
        $("#ctype").val(obj.ctype);
        $("#ck_value").val(obj.ck_value);
        $("#sorts").val(obj.sorts);
        $("#pid").val(obj.pid);
        $("#id").val(obj.id);
        $("#savetype").val(savetype);
    }
  get_form_data = function (id) {
      $.post("/management/wxapp/wx_menu_data?t=get_data", { id:id }, function (data) {
          var obj = jQuery.parseJSON(data);
          set_form_v(obj,"edit");
      });
  }
  this.del_data = function () {
      var index = layer.msg('正在提交，请稍后！', { icon: 16, time: 0 });
      var data = $("#form1").serialize();
      $.post("/management/wxapp/wx_menu_data?t=del_data", data, function (rdata) {
          if (rdata == "1") {
              window.location.reload();
          }
          else {
              alert(rdata);
              layer.close(index);
          }
      });
  }
  this.post_data = function () {
   
      var bret = Validator.Validate($('#form1')[0], 1);
      if (bret) {
          var data = $("#form1").serialize();
          var index = layer.msg('正在提交，请稍后！', { icon: 16, time: 0 });
        
          $.post("/management/wxapp/wx_menu_data?t=do_data", data, function (rdata) {
              if (rdata == "1") {
                  window.location.reload();
              }
              else {
                  alert("提交失败")
                  window.location.reload();
              }
          });
      }
  }
    this.onload = function () {
        $(".mdo").each(function () {
            $(this).click(function () {

                $("#savetype").val($(this).data("ctype"));
                if ($(this).data("ctype") == "add") {
                    set_form_v(jQuery.parseJSON("{\"id\":\"\",\"name\":\"\",\"ctype\":\"\",\"ck_value\":\"\",\"sorts\":\"1\",\"pid\":\"" + $(this).data("pid") + "\"}"), "add");
                }
                else if ($(this).data("ctype") == "edit") {
                    $("#id").val($(this).data("id"));
                    get_form_data($(this).data("id"));
                }
    
            });
        })
    }
    this.submitaccton = function () {
        var index = layer.msg('正在提交，请稍后！', { icon: 16, time: 0 });
        $.post("/management/wxapp/wx_menu_data?t=tb", function (rdata) {
            if (rdata == "1") {
                layer.close(index);
                layer.alert("同步完毕");
            }
            else {
                alert(rdata);
                layer.close(index);
            }
        });
    }
}