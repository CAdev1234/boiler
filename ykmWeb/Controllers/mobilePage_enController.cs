using ykmWeb.Bll;
using ykmWeb.Dal;
using ykmWeb.Models;
using ykmWeb.sysHtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using ykmWeb.Dal.Serv;

namespace ykmWeb.Controllers
{
    public class mobilePage_enController : Controller
    {
        // GET: mobilePage
        public ActionResult Index()
        {
            using (ykmWebDbContext s=new ykmWebDbContext())
            {
                pageMobiIndex p = new pageMobiIndex();
                DalMenuClass dl = new DalMenuClass(s);
                DalGgw dg = new DalGgw(s);
                DalInfo di = new DalInfo(s);
                do_class_view dcv = new do_class_view();
                siteSeo seo = new siteSeo(s, "en");
                InfoTableList it = new InfoTableList(s);
                NavLIst na = new NavLIst(s);

                ViewBag.cid = 0;

                p.seostr = seo.reSeo("index");
                p.htmlStr = new Dictionary<string, string>();
                p.htmlStr["banner"] = mobiHtml.getBanner(dg.FindList(g => g.ggwposition == "sjbanner_en", 6, new OrderModelField[] { new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList());
                //产品中心
                p.htmlStr["procenter"] = mobiHtml.getProCenter_en();
                //走进亨恒
                p.htmlStr["aboutddhh"] = mobiHtml.getAboutHH_en();
                //公司动态
                p.htmlStr["comnews"] = mobiHtml.getComNews_en();
                return View(p);
            }
        }

        public ActionResult list(int cid = 0, int pageid = 1)
        {
            if (pageid <= 0)
            {
                pageid = 1;
            }

            string pagurl = Request.Path + "?cid=" + cid;
            //string pagestr = "&cid=" + cid;
            int rows = 1;
            createPageHtml cph = new createPageHtml();
            cph.pageUrl = pagurl;
            //Expression<Func<info, bool>> wherelba = PredicateExtensionses.True<info>();
            //子页搜索功能
            Expression<Func<info, bool>> wherelba = PredicateExtensionses.True<info>();
            string pagestr = "&cid=" + cid;
            //子页搜索功能
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                pageMobi p = new pageMobi();
                p.strHtml = new Dictionary<string, string>(); //未将对象引用到实例错误
                //p.emptyStr = "<div class=\"money\"><div class=\"nohave\">信息正在更新中...</div></div>";
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                do_class_view dcv = new do_class_view();
                InfoTableList itl = new InfoTableList(s);
                siteSeo seo = new siteSeo(s, "en");
                var classObj = dmc.FindList(n => n.Catalogid == cid, 1, nl.getOrderList()).FirstOrDefault();
                ViewBag.cid = cid;
                if (classObj == null)
                {
                    classObj = new menuClass();
                }
                int topnum = 10;
                List<info> ilist = new List<info>();
                p.classList = nl.getMenuClass(classObj);
                List<int> arrid = dcv.showallclassid(p.classList.Caenname);
                wherelba = wherelba.And(n => arrid.Contains(n.classid.Value));

                p.seostr = seo.reSeo("c");

                //p.path = nl.h5classPath(cid);
                ViewBag.classtitle = classObj.Catalogname;
                ViewBag.classfbt = classObj.subtitle;

                bool isPage = false;

                p.strHtml["submenu"] = nl.h5_subMenu_en(classObj.Catalogid.Value);

                var obje = di.FindListPage(pageid, topnum, out rows, wherelba, itl.getInfoOrder()).Select(itl.get_info_coloum(true)).ToList();
                if (obje.Count > 0)
                {
                    switch (classObj.pclisttype)
                    {
                        case "pro-list"://产品列表
                            p.strHtml["pagelist"] = mobiHtml.pro_list_en(obje);
                            isPage = true;
                            break;
                        case "news-list"://新闻列表
                            p.strHtml["pagelist"] = mobiHtml.news_list_en(obje);
                            isPage = true;
                            break;
                        //case "msg"://留言板
                        //    p.strHtml["pagelist"] = mobiHtml.message_borad();
                        //    isPage = false;
                        //    break;
                        case "cont"://内容
                            p.strHtml["pagelist"] = mobiHtml.info_content(obje);
                            break;
                    }
                }
                else if (classObj.pclisttype == "msg")
                {
                    p.strHtml["pagelist"] = mobiHtml.message_borad_en();
                }
                else
                {
                    p.strHtml["pagelist"] = "<div class=\"jianjie\"><div class=\"neirong infocontent\">Information is being updated...</div></div>";
                }

                if (isPage)
                {
                    //p.PageFootHtml = cph.PageFootH5(rows, topnum, pageid.ToString(), Request.Path, pagestr, false, false);
                    //p.smallPageFootHtml = cph.PageFootNext(rows, topnum, pageid.ToString(), Request.Path, pagestr);
                    var pn = (rows % topnum > 0) ? rows % topnum + 1 : rows % topnum;
                    p.PageFootHtml = "<input id=\"fy\" value=\"1\" type=\"hidden\" /><script src=\"/web_js/fanye.js\"></script><div class=\"page\"><a class=\"more\" href=\"javascript:;\" onclick=\"fanye("+ cid + ","+ pn + ")\">Load more</a></div>";
                }
                return View(p);
            }
        }

        public ActionResult cont(int id = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalInfo di = new DalInfo(s);
                DalMenuClass dmc = new DalMenuClass(s);
                InfoTableList itl = new InfoTableList(s);
                NavLIst nl = new NavLIst(s);
                pageMobi p = new pageMobi();
                p.strHtml = new Dictionary<string, string>();
                siteSeo seo = new siteSeo(s, "en");
                var o = di.FindList(n => n.id == id, 1, null).SingleOrDefault();
                ViewBag.cid = o.classid;
                if (o != null)
                {
                    int cid = o.classid.Value;
                    ViewBag.cid = o.classid;
                    var classObj = dmc.FindList(n => n.Catalogid == cid, 1, nl.getOrderList()).FirstOrDefault();
                    if (classObj == null)
                    {
                        classObj = new menuClass();
                    }
                    //p.path = nl.h5classPath(cid);
                    ViewBag.classtitle = classObj.Catalogname;
                    ViewBag.classfbt = classObj.subtitle;
                    p.strHtml["submenu"] = nl.h5_subMenu_en(classObj.Catalogid.Value);
                    p.seostr = seo.reSeo("i");
                    p.classList = nl.getMenuClass(classObj);
                    p.strHtml["cont"] = mobiHtml.get_cont_page_en(o);
                }
                else
                {
                    p.classList = new viewMenuClass();
                    p.strHtml["cont"] = "";

                }
                return View(p);
            }

        }

        public ActionResult search(string k = "", string t = "", int pageid = 1)
        {
            if (pageid <= 0)
            {
                pageid = 1;
            }
            string pagurl = Request.Path + "?t=" + t + "&k=" + k;
            string pagestr = "&t=" + t + "&k=" + k;
            int rows = 1;
            createPageHtml cph = new createPageHtml();
            cph.pageUrl = pagurl;
            Expression<Func<info, bool>> wherelba = PredicateExtensionses.True<info>();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                pageMobi p = new pageMobi();
                p.strHtml = new Dictionary<string, string>(); //未将对象引用到实力错误
                p.emptyStr = "<div class=\"money\"><div class=\"nohave\">信息正在更新中...</div></div>";
                //DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                do_class_view dcv = new do_class_view();
                InfoTableList itl = new InfoTableList(s);
                siteSeo seo = new siteSeo(s, "en");
                int topnum = 20;
                List<info> ilist = new List<info>();
                wherelba = wherelba.And(n => n.title.Contains(k));
                bool isPage = false;
                p.seostr = seo.reSeo("index");
                var obje = di.FindListPage(pageid, topnum, out rows, wherelba, itl.getInfoOrder()).Select(itl.get_info_coloum(true)).ToList();
                if (string.IsNullOrEmpty(k))
                {
                    p.strHtml["pagelist"] = "<div class=\"search-list\"><div class=\"nohave\">关键字不能为空</div></div>";
                    isPage = false;
                }
                else
                {
                    if (obje.Count() > 0)
                    {
                        //p.strHtml["pagelist"] = mobiHtml.info_tit_list(obje);
                        isPage = true;
                    }
                    else
                    {
                        p.strHtml["pagelist"] = "<div class=\"search-list\"><div class=\"nohave\">没有查到信息</div></div>";
                        isPage = false;
                    }
                }
                if (isPage)
                {
                    p.PageFootHtml = cph.PageFootH5(rows, topnum, pageid.ToString(), Request.Path, pagestr, true, false);
                }
                return View(p);
            }
        }

        public ActionResult classpage(int cid = 0)
        {
            createPageHtml cph = new createPageHtml();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                pageMobi p = new pageMobi();
                p.strHtml = new Dictionary<string, string>(); //未将对象引用到实力错误
                DalMenuClass dmc = new DalMenuClass(s);
                InfoTableList itl = new InfoTableList(s);
                siteSeo seo = new siteSeo(s);
                var classObj = dmc.FindList(n => n.Catalogid == cid, 1, nl.getOrderList()).FirstOrDefault();
                var classlist = dmc.FindList(n => n.ParentID == cid, 0, nl.getOrderList()).ToList();
                if (classObj == null)
                {
                    classObj = new menuClass();
                }
                ViewBag.cid = cid;
                p.seostr = seo.reSeo("c");
                p.path = nl.classPath(cid);
                if (classlist.Count > 0)
                {
                    //p.strHtml["pagelist"] = mobiHtml.infoclass_img_list(classlist);
                }
                else
                {
                    p.strHtml["pagelist"] = "<div class=\"info-container\"><div class=\"nohave\">信息正在更新中...</div></div>";
                }
                return View(p);
            }
        }

    }
}