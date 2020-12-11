using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;
using ykmWeb.Models;
using ykmWeb.common;
using System.Linq.Expressions;
using ykmWeb.Bll;
using ykmWeb.sysHtml;
using System.Text;
using System.Web.Script.Serialization;
using HtmlAgilityPack;

namespace ykmWeb.Controllers
{
    public class siteParentController : Controller
    {

        // GET: siteParent
        [ChildActionOnly]
        public ActionResult top(int cid = 0, string t = "")
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalSiteSeo dss = new DalSiteSeo(s);
                var ss = dss.FindList(n => n.lang == "cn", 1,null).SingleOrDefault();
                if (ss != null)
                {
                    ViewBag.year = ss.year;
                    ViewBag.tel = ss.tel;
                }
                ViewBag.topmenu = nl.LIstMainNav(cid);
                if (cid == 0 && t == "index")
                {
                    ViewBag.banner = nl.getIndexBanner();
                }
                else
                {
                    ViewBag.banner = nl.getLmBanner(cid);
                }
                return View();
            }
        }

        [ChildActionOnly]
        public ActionResult top_en(int cid = 0, string t = "")
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                ViewBag.topmenu = nl.LIstMainNav_en(cid);
                if (cid == 0 && t == "index")
                {
                    ViewBag.banner = nl.getIndexBanner_en();
                }
                else
                {
                    ViewBag.banner = nl.getLmBanner(cid);
                }
                return View();
            }
        }

        [ChildActionOnly]
        public ActionResult boot()
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                DalSiteSeo dss = new DalSiteSeo(s);
                var c = dmc.find(item => item.Caenname == "ljwm");
                var contactInfo = (di.find(item => item.classid == c.Catalogid).cont).ToString();
                StringBuilder sb1 = new StringBuilder();
                pageContact contactModel = new pageContact();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(contactInfo);
                var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//p");
                foreach (var node in htmlNodes)
                {
                    if (node.InnerText.IndexOf("联系人") != -1)
                    {
                        contactModel.name = node.InnerText.Substring(4);
                        contactModel.name = contactModel.name.Replace(" ", "");
                        continue;
                    }
                    if (node.InnerText.IndexOf("联系电话") != -1)
                    {
                        contactModel.mobile = node.InnerText.Substring(5);
                        contactModel.mobile = contactModel.mobile.Replace(" ", "");
                        continue;
                    }
                    if (node.InnerText.IndexOf("座机号码") != -1)
                    {
                        contactModel.telephone = node.InnerText.Substring(5);
                        contactModel.telephone = contactModel.telephone.Replace(" ", "");
                        continue;
                    }
                    if (node.InnerText.IndexOf("联系地址") != -1)
                    {
                        contactModel.address = node.InnerText.Substring(5);
                        contactModel.address = contactModel.address.Replace(" ", "");
                        continue;
                    }
                }

                var ss = dss.FindList(n => n.lang == "cn", 1, null).SingleOrDefault();
                if (ss != null)
                {
                    ViewBag.COPYINFO = ss.copyinfo;
                    ViewBag.ICPINFO = ss.icpinfo;
                }
            }
            return View();
        }

        [ChildActionOnly]
        public ActionResult boot_en()
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalSiteSeo dss = new DalSiteSeo(s);
                DalMenuClass dmc = new DalMenuClass(s);
                do_visitor dv = new do_visitor();
                var msg = dmc.FindList(n => n.pclisttype == "msg", 1, null).SingleOrDefault();
                if (msg != null)
                {
                    ViewBag.msgUrl = nl.getUrlLink_en(msg);
                }
                ViewBag.botmenu = nl.LIstbootNav_en();
                var ss = dss.FindList(n => n.lang=="en", 1, null).SingleOrDefault();
                if (ss != null)
                {
                    ViewBag.botinfo = ss.botinfo.Replace("\r\n", "<br>");
                    ViewBag.copyinfo = ss.copyinfo;
                    ViewBag.icpurl = ss.icpurl;
                    ViewBag.icpinfo = ss.icpinfo;
                }
                ViewBag.ewmimg = nl.getImgList("botewm_en");
                dv.addVisitor();
            }
            return View();
        }











        [ChildActionOnly]
        public ActionResult h5_top(int cid = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                string a = "";
                if (cid == 0)
                {
                    a = "首页";
                }
                else
                {
                    DalMenuClass dmc = new DalMenuClass(s);
                    var aid = dmc.find(n => n.Catalogid == cid);
                    a = aid.Catalogname;
                }

                ViewData["title"] = a;
                NavLIst nl = new NavLIst(s);
                ViewData["mobi_nav"] = nl.mobi_nav(a);
                return View();
            }
        }
        [ChildActionOnly]
        public ActionResult h5_top_list (int cid = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                var c = dmc.find(n => n.Catalogid == cid);
                if (c!=null) { 
                    ViewBag.classTitle = c.Catalogname;
                }
                string a = "";
                if (cid == 0)
                {
                    a = "首页";
                }
                else
                {
                    var aid = dmc.find(n => n.Catalogid == cid);
                    a = aid.Catalogname;
                }

                ViewData["title"] = a;
                NavLIst nl = new NavLIst(s);
                ViewData["mobi_nav"] = nl.mobi_nav(a);
            }
            return View();
        }

        [ChildActionOnly]
        public ActionResult h5_copyright()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult h5_bot()
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalSiteSeo dss = new DalSiteSeo(s);
                do_visitor dv = new do_visitor();
                ViewBag.botmenu = nl.LIstbootNavMobi();
                var ss = dss.FindList(n => n.lang == "cn", 1, null).SingleOrDefault();
                if (ss != null)
                {
                    ViewBag.botinfo = ss.botinfo;
                    ViewBag.copyinfo = ss.copyinfo;
                    ViewBag.icpurl = ss.icpurl;
                    ViewBag.icpinfo = ss.icpinfo;
                    ViewBag.tel = ss.tel;
                }
            }
            return View();
        }

        [ChildActionOnly]
        public ActionResult h5_bot_en()
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalSiteSeo dss = new DalSiteSeo(s);
                do_visitor dv = new do_visitor();
                ViewBag.botmenu = nl.LIstbootNavMobi_en();
                var ss = dss.FindList(n => n.lang=="en", 1, null).SingleOrDefault();
                if (ss != null)
                {
                    ViewBag.botinfo = ss.botinfo;
                    ViewBag.copyinfo = ss.copyinfo;
                    ViewBag.icpurl = ss.icpurl;
                    ViewBag.icpinfo = ss.icpinfo;
                    ViewBag.tel = ss.tel;
                }
                dv.addVisitor();
            }
            return View();
        }






        [ChildActionOnly]
        public ActionResult pageTop(int cid = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalGgw dgg = new DalGgw(s);
                var banner = dgg.FindList(n => n.ggwposition == "i1", 1, null).FirstOrDefault();
                if (banner != null)
                {
                    string link = string.IsNullOrEmpty(banner.ggwlink) ? "javascript:void(null)" : banner.ggwlink;
                    ViewData["banner"] = "<a href=\"" + link + "\" target=\"_blank\"><img src=\"" + banner.imgurl + "\"alt=\"" + banner.title + "\"/></a>";
                }
                int _cid = int.Parse(common.common.IsNumeric(Request.QueryString["cid"]));
                if (_cid == 0)
                {
                    _cid = cid;
                }

                sysHtml.NavLIst nav = new sysHtml.NavLIst(s);
                ViewData["nav"] = nav.LIstMainNav(_cid);
            }
            return View();
        }

        [ChildActionOnly]
        public ActionResult leftPart(int cid = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                //pageLeftModel p = new pageLeftModel();
                int _cid = int.Parse(common.common.IsNumeric(Request.QueryString["cid"]));
                if (_cid == 0)
                {
                    _cid = cid;
                }
                NavLIst nl = new NavLIst(s);
                DalInfo di = new DalInfo(s);
                InfoTableList it = new InfoTableList(s);
                //p.leftMenuHtml = nl.leftMenu(_cid);
                //p.zxzx = di.FindList(null, 10, null).OrderByDescending(g => g.infodate).ThenByDescending(g => g.id).Select(it.get_info_coloum()).ToList();//最新资讯
               // p.lbt = di.FindList(n => !string.IsNullOrEmpty(n.defaultpic), 5, null).OrderByDescending(g => g.infodate).ThenByDescending(g => g.id).Select(it.get_info_coloum(true)).ToList();//轮播
                return View();
            }
        }

        [ChildActionOnly]
        public ActionResult Pageboot()
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nav = new NavLIst(s);
                DalGgw dg = new DalGgw(s);
                htmlggw_ht hgh = new htmlggw_ht();
                ViewData["nav"] = nav.LIstbootNav(0);
                ViewData["ewm"] = hgh.bot_ewm(dg.FindList(n => n.ggwposition == "ewm_bot", 2, new OrderModelField[] { new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList());
            }
            return View();
        }

        [ChildActionOnly]
        public ActionResult mobileTop()
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalSiteSeo dss = new DalSiteSeo(s);
                DalMenuClass dmc = new DalMenuClass(s);
                NavLIst nl = new NavLIst(s);
                var ss = dss.find(n => true);
                ViewBag.topmenu = nl.mobi_top_nav();
            }
            return View();
        }
        [ChildActionOnly]
        public ActionResult mobileBot(int dqcid = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalSiteSeo dss = new DalSiteSeo(s);
                DalMenuClass dmc = new DalMenuClass(s);
                NavLIst nl = new NavLIst(s);

                //产品-栏目链接
                var pro = dmc.find(n => n.leftnavshow == 1 && n.tabletype == "pro" && n.Depth == 2);
                ViewBag.pro_nav = nl.getMobiUrlLink(pro);

            }
            return View();
        }
        [ChildActionOnly]
        public ActionResult mobileSubMenu(int dqcid = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                ViewBag.menulist = nl.LIstbootNavMobi(dqcid);
            }
            return View();
        }



        [ChildActionOnly]
        public string mobileNav()
        {
            using (ykmWeb.Dal.ykmWebDbContext s = new ykmWebDbContext())
            {
                Dal.Serv.DalMenuClass dl = new Dal.Serv.DalMenuClass(s);
                NavLIst nl = new NavLIst(s);
                return nl.mobi_nav();
            }
        }

        [ChildActionOnly]
        public ActionResult mobileBotMenu()
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                sysHtml.NavLIst nav = new sysHtml.NavLIst(s);
                ViewData["nav"] = nav.LIstbootNavMobi(0);
                //   DalCountNum da = new DalCountNum(s);
                //   ViewData["num"]=da.pageAddCount();
            }
            return View();
        }


    }
}