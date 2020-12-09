function searchthis_sj() {
    //window.location.href = '/Search/Index?k=' + escape(document.getElementById('key').value);
    //var t = document.getElementById("type").value;  t=' + escape(t) + '&
    var k = document.getElementById("key_sj").value;
    window.location.href = '/search?k=' + escape(k);
}

function searchpage(pagurl) {
    var k = document.getElementById("str").value;
    window.location.href = pagurl + '&k=' + escape(k);
}



//document.onkeydown = keyDownSearch;
//function keyDownSearch(e) {
//    // 兼容FF和IE和Opera  
//    var theEvent = e || window.event;
//    var code = theEvent.keyCode || theEvent.which || theEvent.charCode;
//    if (code == 13) {
//        searchthis();//具体处理函数  
//        return false;
//    }
//    return true;
//}




//设为首页
function SetHome(obj, url) {
    try {
        obj.style.behavior = 'url(#default#homepage)';
        obj.setHomePage(url);
    } catch (e) {
        if (window.netscape) {
            try {
                netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
            } catch (e) {
                alert("抱歉，此操作被浏览器拒绝！\n\n请在浏览器地址栏输入“about:config”并回车然后将[signed.applets.codebase_principal_support]设置为'true'");
            }
        } else {
            alert("抱歉，您所使用的浏览器无法完成此操作。\n\n您需要手动将【" + url + "】设置为首页。");
        }
    }
}
//收藏本站
function AddFavorite(title, url) {
    try {
        window.external.addFavorite(url, title);
    }
    catch (e) {
        try {
            window.sidebar.addPanel(title, url, "");
        }
        catch (e) {
            alert("抱歉，您所使用的浏览器无法完成此操作。\n\n加入收藏失败，请使用Ctrl+D进行添加");
        }
    }
}
//保存到桌面
function toDesktop(sUrl, sName) {
    try {
        var WshShell = new ActiveXObject("WScript.Shell");
        var oUrlLink = WshShell.CreateShortcut(WshShell.SpecialFolders("Desktop") + "\\" + sName + ".url");
        oUrlLink.TargetPath = sUrl;
        oUrlLink.Save();
    }
    catch (e) {
        alert("当前IE安全级别不允许操作！");
    }
}