var ctrl_form = function(){
    this.title = ""; this.widht = ""; this.height = "";
    var _title = "", _width = "", _height = "";
    var indexpage = null;
  var  alert_page = function (src) {

      indexpage= layer.open({
            type: 2,
            title: _title,
            shadeClose: true,
            shade: false,
            maxmin: true, //开启最大化最小化按钮
            area: [_width, _height],
            content: src
        });
    }
    this.closealert = function () {
        layer.close(indexpage);
    }

    this.band_ctrl = function () {
        _title=this.title;
        _width=this.width;
        _height = this.height;
        $(this.band_classname).each(function () {
            $(this).click(function () {
         
                alert_page($(this).data("href"));
            })
        })
    }

    this.openPage = function (src) {
        _title = this.title;
        _width = this.width;
        _height = this.height;
        alert_page(src);
    }
}