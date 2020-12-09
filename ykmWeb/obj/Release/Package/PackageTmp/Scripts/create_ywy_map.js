var createmap = function () {

    var userdata = null;//传递数据
    var htmlmobile = "<div class='showsmjj'><h4>[title]</h4><ol><li>联系人：[lxr]</li><li>联系电话：[mobile]</li><li>地址：[address]</li></ol></div>"//覆盖物html代码
    var map = new qq.maps.Map(document.getElementById("container"), {
        zoom: 13                                                // 地图的中心地理坐标。
    });

    var size = new qq.maps.Size(24, 24), origin = new qq.maps.Point(0, 0),
        iconhome = new qq.maps.MarkerImage('/images/center.gif', size, origin);


    var marker = new qq.maps.Marker({
        icon: iconhome,
    });


    //获取城市列表接口设置中心点
    citylocation = new qq.maps.CityService({
        complete: function (results) {
            map.setCenter(results.detail.latLng);

            if (marker != null) {

                marker.position = results.detail.latLng;
                chooseY = results.detail.latLng.getLat();
                chooseX = results.detail.latLng.getLng();
                marker.setMap(map);
            }
        }
    });
    //调用searchLocalCity();方法    根据用户IP查询城市信息。
    citylocation.searchCityByAreaCode("0414");



    var size = new qq.maps.Size(32, 32), origin = new qq.maps.Point(0, 0),//初始化地图标记小图标
        icon = new qq.maps.MarkerImage('/images/sethome.png', size, origin);


    var create_bj = function (i) {//创建商家在地图上的标点位置，i为当前json对象中的数据
        var marker2 = new qq.maps.Marker({ //初始化标记覆盖物
            icon: icon,
        });

        var info = new qq.maps.InfoWindow({//初始化点击标记弹出框覆盖物
            map: map
        });
      
        var userhtml = htmlmobile.replace("[title]", userdata[i].companyname).replace("[lxr]", userdata[i].lxr).replace("[mobile]", userdata[i].mobile).replace("[address]", userdata[i].address);//初始化覆盖物内html代码内容

        var myLatlng = new qq.maps.LatLng(userdata[i].lat, userdata[i].lng);//初始化覆盖物坐标
        marker2.position = myLatlng;//确定覆盖物位置
        marker2.setTitle = userdata[i].companyname;//设置标题
        marker2.setClickable(true);//设置可点击
        marker2.setMap(map);//显示覆盖物

        qq.maps.event.addListener(marker2, 'click', function (e) {//监听覆盖物点击事件
            info.open();//显示信息html内容
            info.setContent(userhtml);
            info.setPosition(marker2.getPosition());

        });
    }

    this.start = function (jsonstr) {

        if (jsonstr != "") {
            userdata = $.parseJSON(jsonstr);
            for (var i = 0; i < userdata.length; i++) {
                create_bj(i);
            }
        }

    }
}