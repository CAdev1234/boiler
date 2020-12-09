var del_state_js = function () {
    var url = "/management/del_list/add"
   var band = function () {
       $(".delsh").each(function () {
           var id = $(this).data("val");
           var t = $(this).data("t");
           $(this).click(function () {
               var dataval = { id: id, t: t };
               $.ajax({
                   type: "POST",
                   url: url,
                   data: dataval,
                   contentType: "application/x-www-form-urlencoded",
                   success: function (rdata) {
                       if (rdata == "1") {
                           alert("申请成功!");
                           window.location.reload();
                       }
                       else {
                           alert(rdata);
                       }
                   },
                   error: function (r) {
                       alert("错误");
                   }
               });
           })
       })
   }
}