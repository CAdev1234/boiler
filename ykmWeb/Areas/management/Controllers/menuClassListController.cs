using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ykmWeb.Bll;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;

namespace ykmWeb.Areas.management.Controllers
{
    public class menuClassListController : Controller
    {
        // GET: management/menuClassList
        [webAuthorzize]
        public ActionResult Index(string lang="")
        {
            string t = "menuClass";
            siteLanguage s = new siteLanguage();
            //ViewData["input"] = s.getFristOneInput();
            //ViewData["html"] = s.getListText();
            ViewData["input"] = s.getFristOneInput(lang);
            ViewData["html"] = s.getListText(lang);
            return View();
        
        }

        [webAuthorzize]
        public ActionResult saveone()
        {
            return null;
        }

        [webAuthorzize]
        public ActionResult addchild()
        {
            using(ykmWebDbContext s=new ykmWebDbContext())
            {
                int pid = int.Parse(common.common.IsNumeric(Request.QueryString["pid"]));
                int cid = int.Parse(common.common.IsNumeric(Request.QueryString["cid"]));
                string lang = Request.QueryString["lang"];
                menuRelation m = new menuRelation();
                pageLink pl = new pageLink();
                string a = Request.QueryString["a"];
                Models.menuClass i = new Models.menuClass();
                DalMenuClass di = new DalMenuClass(s);
                if (a == "e")
                {
                    i = di.find(b => b.Catalogid.Value == cid);
                    ViewData["pclisttype"]= m.getOption(i.pclisttype, i.pagename);
                    ViewData["pagename"] = pl.getOption(i.pagename,i.tabletype);
                    ViewBag.savetype = "up";
                }
                else
                {
                    i.ParentID = pid;
                    i.mainnavshow = 1;
                    i.leftnavshow = 1;
                    i.linktype = 0;
                    i.linkurl = "";
                    i.linkCid = 0;
                    i.h5linktype = 0;
                    i.h5linkurl = "";
                    i.h5linkCid = 0;
                    i.language = lang;
                    ViewData["pclisttype"]=m.getOption("", "info");
                    ViewData["pagename"] = pl.getOption("","info");
                    ViewBag.savetype = "save";
                }
                return View(i);
            }
       
        }

        [webAuthorzize]
        [HttpPost]
        public string save(Models.menuClass i)
        {
            string savetype = Request.Form["savetype"];
            if (savetype == "save")
            {
                nclassdo n = new nclass_do_save("menuClass", new Models.nclass
                {
                    ParentID = i.ParentID,
                    Catalogname = i.Catalogname
                });
                int cid = n.do_action();
                if (cid != 0)
                {
                    i.Catalogid = cid;
                    i.linktype = 0;
                    i.h5linktype = 0;
                }
            }
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dm = new DalMenuClass(s);
                dm.edit(c => c.Catalogid == i.Catalogid, g => new Models.menuClass { Catalogname = i.Catalogname, tabletype = i.tabletype, pagename = i.pagename, i_pagename = i.i_pagename, mainnavshow = i.mainnavshow, sitename = i.sitename, keyword = i.keyword, keycont = i.keycont, pclisttype = i.pclisttype, mblisttype = i.mblisttype, defaultpic = i.defaultpic, linktype=i.linktype, linkurl=i.linkurl,  leftnavshow=i.leftnavshow, linkCid=i.linkCid, language=i.language, subtitle = i.subtitle, h5linkCid=i.h5linkCid, h5linktype=i.h5linktype, h5linkurl=i.h5linkurl ,downloadfiles=i.downloadfiles});
            }
            return "<script>parent.updateall(" + i.ParentID.ToString() + ")</script>";
        }

        [webAuthorzize]
        public ActionResult nclass_view_list()
        {
            siteLanguage s = new siteLanguage();
            string ptype = Request.QueryString["ptype"];
            string tt = Request.QueryString["tt"];
            ViewData["tt"] = tt;
            ViewData["ptype"] = ptype;
            ViewData["input"] = s.getFristOneInput();
            ViewData["html"] = s.getListText();
            return View();
        }

        [webAuthorzize]
        public string listPageContent(string code)
        {
            menuRelation m = new menuRelation();
            return m.getOption("", code);
        }

        [webAuthorzize]
        public string PageLInkContent(string code)
        {
            pageLink pl = new pageLink();
            return pl.getOption("", code);
        }

        [webAuthorzize]
        public ActionResult setLink(int  cid=0)
        {
            siteLanguage s = new siteLanguage();
            string ptype = "setlink";
            string tt = Request.QueryString["tt"];
            ViewData["tt"] = tt;
            ViewData["ptype"] = ptype;
            ViewData["input"] = s.getFristOneInput();
            ViewData["html"] = s.getListText();

            using (ykmWebDbContext s2=new ykmWebDbContext())
            {
                DalMenuClass c = new DalMenuClass(s2);
                var o = c.FindList(n => n.Catalogid == cid, 1, null).FirstOrDefault();
                return View(o);
            }
        }

        [webAuthorzize]
        public ActionResult setH5Link(int cid = 0)
        {
            siteLanguage s = new siteLanguage();
            string ptype = "setH5Link";
            string tt = Request.QueryString["tt"];
            ViewData["tt"] = tt;
            ViewData["ptype"] = ptype;
            ViewData["input"] = s.getFristOneInput();
            ViewData["html"] = s.getListText();

            using (ykmWebDbContext s2 = new ykmWebDbContext())
            {
                DalMenuClass c = new DalMenuClass(s2);
                var o = c.FindList(n => n.Catalogid == cid, 1, null).FirstOrDefault();
                return View(o);
            }
        }

        [HttpPost]
        [webAuthorzize]
        public string updatelink(Models.menuClass m)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass c = new DalMenuClass(s);
                int? catalogid = m.Catalogid;
                c.edit(n => n.Catalogid == catalogid, g => new Models.menuClass { linkCid = m.linkCid, linktype = m.linktype, linkurl = m.linkurl });
                return "100";
            }       
        }

        [HttpPost]
        [webAuthorzize]
        public string updateh5link(Models.menuClass m)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass c = new DalMenuClass(s);
                int? catalogid = m.Catalogid;
                c.edit(n => n.Catalogid == catalogid, g => new Models.menuClass { h5linkCid = m.linkCid, h5linktype = m.linktype, h5linkurl = m.linkurl });
                return "100";
            }
        }



        /// <summary>
        /// 用户添加栏目
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        [webAuthorzize]
        public ActionResult menulist(string lang = "")
        {
            string t = "menuClass";
            siteLanguage a = new siteLanguage();
            //ViewData["input"] = s.getFristOneInput();
            //ViewData["html"] = s.getListText();
            ViewData["input"] = a.getFristOneInput(lang);
            ViewData["html"] = a.getListText(lang);
            int muid = int.Parse(System.Web.HttpContext.Current.Session["Muid"].ToString());
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalWebmanager duser = new DalWebmanager(s);
                var user = duser.find(n => n.id == muid);
                ViewBag.qxtype = user.qxtype;
            }
            return View();
        }


        [webAuthorzize]
        public ActionResult user_addchild()
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                int pid = int.Parse(common.common.IsNumeric(Request.QueryString["pid"]));
                int cid = int.Parse(common.common.IsNumeric(Request.QueryString["cid"]));
                string lang = Request.QueryString["lang"];
                menuRelation m = new menuRelation();
                pageLink pl = new pageLink();
                string a = Request.QueryString["a"];
                Models.menuClass i = new Models.menuClass();
                DalMenuClass di = new DalMenuClass(s);
                if (a == "e")
                {
                    i = di.find(b => b.Catalogid.Value == cid);
                    ViewData["pclisttype"] = m.getOption(i.pclisttype, i.pagename);
                    ViewData["pagename"] = pl.getOption(i.pagename, i.tabletype);
                    ViewBag.savetype = "up";
                }
                else
                {
                    i.ParentID = pid;
                    i.mainnavshow = 1;
                    i.leftnavshow = 1;
                    i.linktype = 0;
                    i.linkurl = "";
                    i.linkCid = 0;
                    i.h5linktype = 0;
                    i.h5linkurl = "";
                    i.h5linkCid = 0;
                    i.language = lang;
                    ViewData["pclisttype"] = m.getOption("", "info");
                    ViewData["pagename"] = pl.getOption("", "info");
                    ViewBag.savetype = "save";
                }
                return View(i);
            }

        }

        [webAuthorzize]
        [HttpPost]
        public string user_save(Models.menuClass i)
        {
            string savetype = Request.Form["savetype"];
            if (savetype == "save")
            {
                nclassdo n = new nclass_do_save("menuClass", new Models.nclass
                {
                    ParentID = i.ParentID,
                    Catalogname = i.Catalogname
                });
                int cid = n.do_action();
                if (cid != 0)
                {
                    i.Catalogid = cid;
                }
            }
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dm = new DalMenuClass(s);
                dm.edit(c => c.Catalogid == i.Catalogid, g => new Models.menuClass { Catalogname = i.Catalogname, tabletype = i.tabletype, pagename = i.pagename, i_pagename = i.i_pagename, mainnavshow = i.mainnavshow, sitename = i.sitename, keyword = i.keyword, keycont = i.keycont, pclisttype = i.pclisttype, mblisttype = i.mblisttype, defaultpic = i.defaultpic, linktype = i.linktype, linkurl = i.linkurl, leftnavshow = i.leftnavshow, linkCid = i.linkCid, language = i.language, subtitle = i.subtitle ,downloadfiles =i.downloadfiles});
            }
            return "<script>parent.updateall(" + i.ParentID.ToString() + ")</script>";
        }


    }
}