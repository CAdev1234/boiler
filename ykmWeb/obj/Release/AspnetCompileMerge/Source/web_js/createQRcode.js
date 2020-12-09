$(function(){
    var qrstr = "https://www.xxx.com/xxx/xxx";
    $('#qrid').qrcode(qrstr);//不指定二维码大写
    $('#qrid').qrcode({width: 280,height: 280,text: qrstr});//指定二维码大小
});
