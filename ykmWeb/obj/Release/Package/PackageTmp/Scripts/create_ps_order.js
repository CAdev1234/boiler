var markersArray = [];
var arrinfo = [];

var createOrder = function () {
  
    var userdata = null;//传递数据


    var lng = $("#lng").val();//当前车辆经度
    var lat = $("#lat").val();//当前车辆纬度

    var htmlmobile ="<div class='showsmjj'><h4>[title]</h4><ol><li>订单编号：[ordercode]</li><li>订购日期：[orderdate]</li><li>订购总价：[sum]</li><li>距离：约[jllist]公里</li></ol><div class='clickok'><button class='btn btn-default btn-sm ' data-val='[ichoosnum]' id='[btnichoosnum]'  >选择</button></div></div>"//覆盖物html代码
    var map = new qq.maps.Map(document.getElementById("container"), { //初始化地图
        zoom: 10                                               // 地图的中心地理坐标。
    });

    var size = new qq.maps.Size(32, 32), origin = new qq.maps.Point(0, 0),//初始化地图标记小图标
        icon = new qq.maps.MarkerImage('/images/sethome.png', size, origin), iconok = new qq.maps.MarkerImage('/images/sethomeok.png', size, origin);

 

    var homesize = new qq.maps.Size(24, 24), homeorigin = new qq.maps.Point(0, 0),//初始化中心点小图标
        homeicon = new qq.maps.MarkerImage('/images/center.gif', homesize, homeorigin);
   
    var myLatlng = new qq.maps.LatLng(lat, lng);//设置地图中心点为车辆所在位置
    map.setCenter(myLatlng);
    var marker = new qq.maps.Marker({
        icon: homeicon
    });

    marker.position = myLatlng;
    marker.setMap(map);//显示中心点标记


    var create_bj = function (i) {//创建商家在地图上的标点位置，i为当前json对象中的数据
        var marker2 = new qq.maps.Marker({ //初始化标记覆盖物
            icon: icon,
        });

        if (check_is_chooseed(i)) {
            marker2.setIcon(iconok);
        }
        var mandInfo = { marker: marker2, orid: userdata[i].orid };
        markersArray.push(mandInfo);

        var info = new qq.maps.InfoWindow({//初始化点击标记弹出框覆盖物
            map: map
        });
        arrinfo.push(info);
        var userhtml = htmlmobile.replace("[title]", userdata[i].companyname).replace("[ordercode]", userdata[i].p_code).replace("[orderdate]", userdata[i].infodate).replace("[sum]", userdata[i].sum).replace("[ichoosnum]", i).replace("[btnichoosnum]", "btn" + i).replace("[jllist]", userdata[i].sorts.toFixed(2));//初始化覆盖物内html代码内容

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
        qq.maps.event.addListener(info, 'domready', function (e) {//监听当html内容加入到网页dom时，监听按钮点击事件
          
            $("#btn" + i).click(function () {
                setChooseData(i, marker2);            
            })
        });
       
    }
    var setChooseData = function (i,makerobj) {//把选择好的内容加入到集合中

        var isadd = true;
        if (userchoosedate.length > 0) {
            for (a in userchoosedate) {
                if (userchoosedate[a].orid == userdata[i].orid) {
                    isadd = false;
                    break;
                }
            }
        }


        if (!isadd) {
            layer.alert("已经选择过了");
        }
        else {
            userchoosedate.push(userdata[i]);
            makerobj.setIcon(iconok);
           
        }
        
    }

    var check_is_chooseed = function (i) {
        var ischoose = false;
        if (userchoosedate.length > 0) {
            for (a in userchoosedate) {
                if (userchoosedate[a].orid == userdata[i].orid) {
                    ischoose = true;
                    break;
                }
            } 
        }
        return ischoose;
    }



    this.delchoose = function (orid) {
     
      
     
        var objMarke = null;
            for (var i = 0; i < userchoosedate.length; i++) {
                if (userchoosedate[i].orid == orid) {
                    userchoosedate.splice(i, 1);
                    break;
                }
        }

        for (var i = 0; i < markersArray.length; i++) {

            if (markersArray[i].orid == orid) {
                markersArray[i].marker.setIcon(icon);
                break;
            }
        }

     
        
    }


    this.postData = function () {//初始化js方法显示地图信息

         getPageData();
         
    }
    
    var delMakerAndInfo = function () {

        if (markersArray.length > 0) {
         
            for (var i = 0; i < markersArray.length;i++) {
               // console.log(i);
                markersArray[i].marker.setMap(null);
            }
            markersArray.length = 0;
        }

        if (arrinfo.length > 0) {
            for (var j = 0; j < arrinfo.length;j++) {
                arrinfo[j].setMap(null);
            }
            arrinfo.length = 0;
        }

    }
    var getPageData = function () {
   
        var sdate = $("#sdate").val();
        var edate = $("#edate").val();
        var carid = $("#carid").val();
        var yw_id = $("#ywyid").val();
    
        var data = { s: sdate, e: edate, lng: lng, lat: lat, pageindex: pageindex, ywid: yw_id };

        $.post("/management/psryorder/getData", data, function (rdata) {

           
            if (rdata != null && rdata.length > 0) {

                delMakerAndInfo();
                listdata.splice(0, listdata.length)
                userdata = rdata;

                for (var i = 0; i < userdata.length; i++) {

                    listdata.push(userdata[i]);
                    create_bj(i);

                }
            }
            else {
                pageindex--;
                layer.alert("没有订单信息");

            }
        });
    }

   
}

function submitok(dotype) {
   
    if (userchoosedate.length > 0) {

        var postdata = [];
        var newdata = { carid: $("#carid").val(), carlng: $("#lng").val(), carlat: $("#lat").val(), orderlist: userchoosedate }
        var url = "/management/psryorder/saveData";

        if (dotype == "update") {
            url = "/management/psryorder/updata?orid=" + $("#orid").val();
        }

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(newdata),
            dataType: "json",
            success: function (message) {
                if (message == "1") {
                    layer.alert("提交成功", { closeBtn: 0 }, function () {
                        window.location.href ='/management/psryorder/Index'
                    });

                }
            },
            error: function (message) {
                $("#request-process-patent").html("提交数据失败！");
            }
        });
    }
    else {
        layer.alert("您没有选择任何订单!");
    }
}


