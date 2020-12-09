function showTime() {

    var weekString = "日一二三四五六";
    var time = new Date();
    var year = time.getFullYear();
    var month = time.getMonth();
    var date = time.getDate();
    var day = time.getDay();
    var hour = time.getHours();
    var minutes = time.getMinutes();
    var second = time.getSeconds();
    month < 10 ? month = '0' + month : month;
    month = parseInt( month) + 1;
    hour < 10 ? hour = '0' + hour : hour;
    minutes < 10 ? minutes = '0' + minutes : minutes;
    second < 10 ? second = '0' + second : second;
    var now_time = '今天是 ' + year + '年' + month + '月' + date +'日'+' '+'周' + weekString.charAt(time.getDay());
    document.getElementById('showtime').innerHTML = now_time;

}
showTime();