using ykmWeb.Bll;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;
using ykmWeb.Models;
using ykmWeb.sysHtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using ykmWeb.Areas.management.Controllers;
using ykmWeb.common;
using HtmlAgilityPack;

namespace ykmWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                siteSeo seo = new siteSeo(s, "cn");
                DalMenuClass dmc = new DalMenuClass(s);
                do_class_view dcv = new do_class_view();
                NavLIst nl = new NavLIst(s);
                createPageHtml cph = new createPageHtml();
                DalSiteSeo dss = new DalSiteSeo(s);
                DalInfo di = new DalInfo(s);

                var c = dmc.find(item => item.Caenname == "ljwm");
                var contactInfo = (di.find(item => item.classid == c.Catalogid).cont).ToString();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(contactInfo);
                var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//p");
                foreach (var node in htmlNodes)
                {
                    if (node.InnerText.IndexOf("联系人") != -1)
                    {
                        ViewBag.CONTNAME = node.InnerText.Substring(4);
                        ViewBag.CONTNAME = ViewBag.CONTNAME.Replace(" ", "");
                        continue;
                    }
                    if (node.InnerText.IndexOf("联系电话") != -1)
                    {
                        ViewBag.MOBILE = node.InnerText.Substring(5);
                        ViewBag.CONTNAME = ViewBag.MOBILE.Replace(" ", "");
                        continue;
                    }
                    if (node.InnerText.IndexOf("座机号码") != -1)
                    {
                        ViewBag.TELEPHONE = node.InnerText.Substring(5);
                        ViewBag.CONTNAME = ViewBag.TELEPHONE.Replace(" ", "");
                        continue;
                    }
                    if (node.InnerText.IndexOf("联系地址") != -1)
                    {
                        ViewBag.ADDRESS = node.InnerText.Substring(5);
                        ViewBag.CONTNAME = ViewBag.ADDRESS.Replace(" ", "");
                        continue;
                    }
                }

                //Get qrcode
                DalGgw dgw = new DalGgw(s);
                var qr_code = dgw.FindList(item => item.ggwposition == "botewm", 0, null).FirstOrDefault();
                ViewBag.QRCODE = qr_code.imgurl;

                ViewBag.seostr = seo.reSeo("index");
                ViewBag.lanmu = cph.index_lanmu();
                ViewBag.index_product = cph.index_product();//全部
                ViewBag.index_customercase = cph.index_customercase();

                ViewBag.ydjcsb = cph.ydjcsb();//硬度检测设备
                ViewBag.dzzxydj = cph.dzzxydj();//定制在线硬度计
                ViewBag.jxzysb = cph.jxzysb();//金相制样设备
                ViewBag.clsysb = cph.clsysb();//材料试验设备

                ViewBag.about_aolong = cph.index_about_aolong();
                ViewBag.index_news = cph.index_news();

                var hjzs = dmc.find(n => n.Caenname == "hjzs");
                ViewBag.hjzs = hjzs.Catalogname;
                ViewBag.hjzs_url = nl.getUrlLink(hjzs);

                var yxwl = dmc.find(n => n.Caenname == "yxwl");
                ViewBag.yxwl = yxwl.Catalogname;
                ViewBag.yxwl_url = nl.getUrlLink(yxwl);


                ViewBag.lxwm = cph.index_lxwm();
                ViewBag.link = cph.index_link();

                ViewBag.sj_lanmu = cph.index_sj_lanmu();
                ViewBag.sj_pro = cph.index_sj_pro();
                ViewBag.sj_aboutus = cph.index_sj_aboutus();
                ViewBag.sj_customercase = cph.index_sj_customercase();


                ViewBag.sj_index_news = cph.sj_index_news();
                ViewBag.sj_lxwm = cph.sj_lxwm();
                ViewBag.sjbanner = nl.getIndexBanner_sj();
                return View();
            }
        }

        public ActionResult list(int cid = 0, int pageid = 1)
        {
            if (pageid <= 0)
            {
                pageid = 1;
            }
            string pagurl = Request.Path + "?cid=" + cid;
            int rows = 1;
            createPageHtml cph = new createPageHtml();
            cph.pageUrl = pagurl;
            ViewBag.pageUrl = pagurl;
            ViewBag.cid = cid;
            Expression<Func<info, bool>> wherelba = PredicateExtensionses.True<info>();
            string searchstr = "";
            //if (!string.IsNullOrEmpty(k))
            //{
            //    searchstr = "&k=" + k;
            //    wherelba = wherelba.And(n => n.title.Contains(k));
            //}
            string pagestr = "&cid=" + cid + searchstr;
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                pageNewlist p = new pageNewlist();
                p.path= nl.classPath(cid);
                p.emptyStr = "<div class=\"nr\"><img src=\"images/noinfo.png\"style=\"margin:0 20%;width:60%\"></div>";
                DalSiteSeo dss = new DalSiteSeo(s);
                var ss = dss.FindList(n => n.lang == "cn", 1, null).SingleOrDefault();
                if (ss != null)
                {
                    ViewBag.tel = ss.tel;
                }
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                do_class_view dcv = new do_class_view();
                InfoTableList itl = new InfoTableList(s);
                siteSeo seo = new siteSeo(s, "cn");
                var ljwm = dmc.find(n => n.Caenname == "ljwm");
                ViewBag.ljwm_url = nl.getUrlLink(ljwm);
                var classObj = dmc.FindList(n => n.Catalogid == cid, 1, nl.getOrderList()).FirstOrDefault();
                if (classObj == null)
                {
                    classObj = new menuClass();
                }
                ViewBag.leftmenu = nl.leftMenu(cid);
                ViewBag.classtitle = classObj.Catalogname;
                ViewBag.list_type = classObj.pclisttype;
                p.mobi_child_str = nl.mobi_child_str(cid);
                //p.mobi_child_str1 = nl.mobi_child_str1(cid);
                int topnum = 20;
                List<info> ilist = new List<info>();
                p.classList = nl.getMenuClass(classObj);
                List<int> arrid = dcv.showallclassid(p.classList.Caenname);
                wherelba = wherelba.And(n => arrid.Contains(n.classid.Value));
                p.seostr = seo.reSeo("c");
                //topnum = 5;
                switch (classObj.pclisttype)
                {
                    case "pro-list":
                        topnum = 9;
                        break;
                    case "news-list":
                        topnum = 6;
                        break;
                    case "news-shipin-list":
                        topnum = 6;
                        break;
                    case "news-huanjing-list":
                        topnum = 9;
                        break;
                    default:
                        topnum = 10;
                        break;
                }
                bool isPage = true;
                var obje = di.FindListPage(pageid, topnum, out rows, wherelba, itl.getInfoOrder()).Select(itl.get_info_coloum(true)).ToList();
                if (obje.Count > 0)
                {
                    switch (classObj.pclisttype)
                    {
                        case "pro-list"://产品列表
                            p.listHtml = cph.pro_list(obje);
                            p.listHtml_sj = cph.sj_pro_list(obje);
                            p.PageFootHtml = cph.PageFoot(rows, topnum, pageid.ToString(), Request.Path, pagestr, true, true);
                            break;
                        case "news-huanjing-list"://环境列表
                            p.listHtml = cph.huanjing_list(obje);
                            p.listHtml_sj = cph.sj_huanjing_list(obje);
                            p.PageFootHtml = cph.PageFoot(rows, topnum, pageid.ToString(), Request.Path, pagestr, true, true);
                            break;
                        case "news-list"://新闻列表
                            p.listHtml = cph.news_list(obje);
                            p.listHtml_sj = cph.sj_news_list(obje);
                            p.PageFootHtml = cph.PageFoot(rows, topnum, pageid.ToString(), Request.Path, pagestr, true, true);
                            break;
                        case "news-shipin-list"://视频列表
                            p.listHtml = cph.shipin_list(obje);
                            p.listHtml_sj = cph.sj_shipin_list(obje);
                            p.PageFootHtml = cph.PageFoot(rows, topnum, pageid.ToString(), Request.Path, pagestr, true, true);
                            break;
                        case "cont"://内容
                            p.listHtml = cph.info_content(obje);
                            p.listHtml_sj = cph.info_content(obje);
                            isPage = false;
                            break;
                    }
                }
                else if (classObj.pclisttype == "msg")
                {
                    p.listHtml = cph.message_borad();
                    p.listHtml_sj = cph.message_borad();
                    isPage = false;
                }
                else
                {
                    p.listHtml = "<div class=\"nr\"><img src=\"images/noinfo.png\"style=\"margin:0 20%;width:60%\"></div>";
                }
                if (isPage)
                {
                    p.PageFootHtml = cph.PageFoot(rows, topnum, pageid.ToString(), Request.Path, pagestr, true, true);
                    //p.smallPageFootHtml = cph.PageFootNext(rows, topnum, pageid.ToString(), Request.Path, pagestr);
                }
                return View(p);
            }
        }

        public ActionResult cont(int cid = 0, int id = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalInfo di = new DalInfo(s);
                DalMenuClass dmc = new DalMenuClass(s);
                InfoTableList itl = new InfoTableList(s);
                NavLIst nl = new NavLIst(s);
                siteSeo seo = new siteSeo(s, "cn");
                info o = new info();
                createPageHtml cph = new createPageHtml();
                DalSiteSeo dss = new DalSiteSeo(s);
                var ss = dss.FindList(n => n.lang == "cn", 1, null).SingleOrDefault();
                if (ss != null)
                {
                    ViewBag.tel = ss.tel;
                }
                var ljwm = dmc.find(n => n.Caenname == "ljwm");
                ViewBag.ljwm_url = nl.getUrlLink(ljwm);
                if (id != 0)
                {
                    o = di.FindList(n => n.id == id, 1, null).SingleOrDefault();
                    if (o != null)
                    {
                        int classid = o.classid.Value;
                        ViewBag.path = nl.classPath(classid);
                        ViewBag.cid = o.classid;
                        ViewBag.leftmenu = nl.leftMenu(classid);
                        ViewBag.isTitle = true;
                        var classObj = dmc.FindList(n => n.Catalogid == classid, 1, nl.getOrderList()).FirstOrDefault();
                        if (classObj == null)
                        {
                            classObj = new menuClass();
                        }
                        ViewBag.listType = classObj.pclisttype;
                        ViewBag.backToList = nl.getUrlLink(classObj);
                        ViewBag.classtitle = classObj.Catalogname;
                        ViewBag.seostr = seo.reSeo("i");
                        ViewBag.infoHtml = cph.get_cont_page(o);
                        ViewBag.classtitle = classObj.Catalogname;
                        ViewBag.mobi_child_str = nl.mobi_child_str(classid);
                        ViewBag.mobi_child_str1 = nl.mobi_child_str1(classid);
                    }
                }
                return View();
            }

        }

        public ActionResult search(string k = "", int pageid = 1)
        {
            if (pageid <= 0)
            {
                pageid = 1;
            }

            string pagurl = Request.Path + "?k=" + k;
            string pagestr = "&k=" + k;
            int rows = 1;
            createPageHtml cph = new createPageHtml();
            cph.pageUrl = pagurl;
            ViewBag.pageUrl = pagurl;
            Expression<Func<info, bool>> wherelba = PredicateExtensionses.True<info>();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                pageNewlist p = new pageNewlist();

                p.emptyStr = "<div class=\"nr\"><img src=\"images/noinfo.png\"style=\"margin:0 20%;width:60%\"></div>";
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                do_class_view dcv = new do_class_view();
                InfoTableList itl = new InfoTableList(s);
                siteSeo seo = new siteSeo(s, "cn");
                DalSiteSeo dss = new DalSiteSeo(s);
                var ljwm = dmc.find(n => n.Caenname == "ljwm");
                ViewBag.ljwm_url = nl.getUrlLink(ljwm);
                var ss = dss.FindList(n => n.lang == "cn", 1, null).SingleOrDefault();
                if (ss != null)
                {
                    ViewBag.tel = ss.tel;
                }
                int topnum = 10;
                List<info> ilist = new List<info>();
                //List<int> arrid = new List<int>();
                //switch (t)
                //{
                //    case "pro":
                //        arrid = dcv.showallclassid("alcp");
                //        break;
                //    case "news":
                //        arrid = dcv.showallclassid("aldt");
                //        break;
                //}
                //wherelba = wherelba.And(n => arrid.Contains(n.classid.Value));
                wherelba = wherelba.And(n => n.title.Contains(k));
                bool isPage = true;
                p.seostr = seo.reSeo("index");
                ViewBag.leftmenu = nl.leftMenu();
                ViewBag.seostr = seo.reSeo("i");
                topnum = 10;
                var obje = di.FindListPage(pageid, topnum, out rows, wherelba, itl.getInfoOrder()).Select(itl.get_info_coloum(true)).ToList();
                if (string.IsNullOrEmpty(k))
                {
                    p.listHtml = "<div class=\"nr\"><img src=\"images/noinfo.png\"style=\"margin:0 20%;width:60%\"></div>";
                    isPage = false;
                }
                else
                {
                    if (obje.Count() > 0)
                    {
                        p.listHtml = cph.news_list(obje);
                        p.listHtml_sj = cph.sj_news_list(obje);
                        isPage = true;
                    }
                    else
                    {
                        p.listHtml = "<div class=\"nr\"><img src=\"images/noinfo.png\"style=\"margin:0 20%;width:60%\"></div>";
                        isPage = false;
                    }
                }
                if (isPage)
                {
                    p.PageFootHtml = cph.PageFoot(rows, topnum, pageid.ToString(), Request.Path, pagestr, true, true);
                }
                return View(p);
            }
        }

        public ActionResult classpage(int cid = 0)
        {
            common.common.ischangeMobi(Request.Url.PathAndQuery);
            createPageHtml cph = new createPageHtml();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                pageNewlist p = new pageNewlist();
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                do_class_view dcv = new do_class_view();
                InfoTableList itl = new InfoTableList(s);
                siteSeo seo = new siteSeo(s);
                var classObj = dmc.FindList(n => n.Catalogid == cid, 1, nl.getOrderList()).FirstOrDefault();
                var classlist = dmc.FindList(n => n.ParentID == cid, 0, nl.getOrderList()).ToList();
                if (classObj == null)
                {
                    classObj = new menuClass();
                }
                ViewBag.banner = nl.getLmBanner(classObj);
                ViewBag.leftmenu = nl.leftMenu(cid);
                ViewBag.cid = cid;
                p.seostr = seo.reSeo("c");
                p.path = nl.classPath(cid);
                if (classlist.Count > 0)
                {
                    //p.listHtml = cph.infoclass_img_list(classlist);
                }
                else
                {
                    p.listHtml = "<div class=\"nr\"><img src=\"images/noinfo.png\"style=\"margin:0 20%;width:60%\"></div>";
                }
                return View(p);
            }
        }
        public ActionResult yzm()
        {
            //首先实例化验证码的类  
            ValidateCode validateCode = new ValidateCode();
            //生成验证码指定的长度  
            string code = validateCode.GetRandomString(4);

            Session["yljcheode"] = code;

            //创建验证码的图片  
            byte[] bytes = validateCode.CreateImage(code);

            //最后将验证码返回  
            return File(bytes, @"image/jpeg");
        }


    }
}