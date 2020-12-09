// 打开新页面
// 打开自定义新选项卡：<a href="#" οnclick="openTabPage('url', '自定义标题')">test</a>
// 打开系统选项卡：<a href="#" οnclick="openTabPage('url')">查看提现记录</a>
function openTabPage(url, title) {
    var wpd = $(window.parent.document);
    var mainContent = wpd.find('.J_mainContent');
    var thisIframe = mainContent.find("iframe[data-id='" + url + "']");
    var pageTabs = wpd.find('.J_menuTabs .page-tabs-content ')
    pageTabs.find(".J_menuTab.active").removeClass("active");
    mainContent.find("iframe").css("display", "none");
    if (thisIframe.length > 0) {	// 选项卡已打开
        thisIframe.css("display", "inline");
        pageTabs.find(".J_menuTab[data-id='" + url + "']").addClass("active");
    } else {
        var menuItem = wpd.find("a.J_menuItem[href='" + url + "']");
        var dataIndex = title == undefined ? menuItem.attr("data-index") : '9999';
        var _title = title == undefined ? menuItem.find('.nav-label').text() : title;
        var iframe = '<iframe class="J_iframe" name="iframe' + dataIndex + '" width="100%" height="100%" src="' + url + '" frameborder="0" data-id="' + url
            + '" seamless="" style="display: inline;"></iframe>';
        pageTabs.append(
            ' <a href="javascript:;" class="J_menuTab active" data-id="' + url + '">' + _title + ' <i class="fa fa-times-circle"></i></a>');
        mainContent.append(iframe);
        //显示loading提示
        var loading = top.layer.load();
        mainContent.find('iframe:visible').load(function () {
            //iframe加载完成后隐藏loading提示
            top.layer.close(loading);
        });
    }
}