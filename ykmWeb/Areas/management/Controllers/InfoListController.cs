using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;
using ykmWeb.Models;
using ykmWeb.common;
using System.Linq.Expressions;
using ykmWeb.Bll;
//using sand_common;

namespace ykmWeb.Areas.management.Controllers
{

    public class InfoListController : Controller
    {
  
        [webAuthorzize]
        public ActionResult Index()
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalInfo di = new DalInfo(s);
                string p = ykmWeb.common.common.IsNumeric(Request.QueryString["Page"]);
                string key = Request.QueryString["key"];
                string un = Request.QueryString["un"];
                string sta = ykmWeb.common.common.IsNumeric(Request.QueryString["sta"]);
                string sdate = Request.QueryString["sdate"];
                string edate = Request.QueryString["edate"];
                if (p == "0")
                {
                    p = "1";
                }
                string pagestr = "&key=" + Server.UrlEncode(key) + "&un=" + un + "&sta=" + sta + "&sdate=" + sdate + "&edate=" + edate;
                int row = 0;
                Expression<Func<info, bool>> wherelba = PredicateExtensionses.True<info>();
                //    key = "新闻";
                if (string.IsNullOrEmpty(key) == false)
                {
                    wherelba = wherelba.And(u => u.title.Contains(key));
                }
                //if (string.IsNullOrEmpty(un) == false)
                //{
                //    wherelba = wherelba.And(u => u.authorname.Contains(un));
                //}
                //if (sta != "0")
                //{
                //    int _sta = int.Parse(sta);
                //    wherelba = wherelba.And(u => u.opustypes == _sta);
                //}
                if (string.IsNullOrEmpty(sdate) == false)
                {
                    DateTime _sdate = DateTime.Parse(sdate);
                    wherelba = wherelba.And(u => u.insertdate >= _sdate);
                }

                if (string.IsNullOrEmpty(edate) == false)
                {
                    DateTime _edate = DateTime.Parse(edate);
                    wherelba = wherelba.And(u => u.insertdate <= _edate);
                }
                //   var ilist=di.FindListPage_by_storedProcedure(out row, "id,title,infodate,defaultpic,istop,tj,null as Vendor", "","infodate desc,id desc","10",p);

                var ilist = di.FindListPage(int.Parse(p), 10, out row, wherelba, new OrderModelField[] { new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).Select(n => new ht_list_info { id = n.id, title = n.title, insertdate = n.insertdate }).ToList();

                if (ilist.Count() > 0)
                {
                    ViewBag.footpage = ykmWeb.common.common.PageFoot_bootstrap(row, 10, p, System.Web.HttpContext.Current.Request.Path, pagestr, "cn", true, true);
                }
                ViewBag.infocount = row;
                return View(ilist);
            }
        }
        [webAuthorzize]
        public ActionResult edit(int id=0,int cid=0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {         
                info i;
                if (id == 0)
                {
                     i = new info(); i.id = 0; i.istop = 0; i.insertdate = DateTime.Now; i.sorts = 1; i.classid = 0;
                    //if (cid != 0)
                    //{
                    //    DalMenuClass dm = new DalMenuClass(s);
                    //    var o = dm.FindList(n => n.Catalogid == cid, 1, null).Select(g => new { g.Catalogid, g.Catalogname }).SingleOrDefault();
                    //    i.classid = o.Catalogid;
                    //    i.classname = o.Catalogname;
                    //}
                    ViewBag.savetype = "save";
                }
                else
                {
                    DalInfo di = new DalInfo(s);  
                    i = di.FindList(n => n.id == id,1,null).FirstOrDefault();
                    ViewBag.savetype = "up";           

                }
                return View(i);
            }
         
        }
        [webAuthorzize]
        public ActionResult view(int id = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                info i;
                if (id == 0)
                {
                    i = new info();
                    i.id = 0; i.insertdate = DateTime.Now; i.sorts = 0;
                    ViewBag.savetype = "save";
                }
                else
                {
                    DalInfo di = new DalInfo(s);
                    i = di.FindList(n => n.id == id, 1, null).FirstOrDefault();
                    ViewBag.savetype = "up";
                }
                return View(i);
            }

        }
        [webAuthorzize]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult updata(info o)
        {
            if (ModelState.IsValid)
            {
                string savetype = Request.Form["savetype"];
                using (ykmWebDbContext s = new ykmWebDbContext())
                {
                    DalInfo di = new DalInfo(s);
                    info i = new info();
                    //if (savetype == "save")
                    //{
                    //    i = new Opus { backgroundcolor = o.backgroundcolor, browsefake = o.browsefake, collectionfake = o.collectionfake, commentfake = o.commentfake, description = o.description, frontcover = o.frontcover, infodate = o.infodate, infosort = o.infosort, opustypes = o.opustypes, praisefake = o.praisefake, state = 100, title = o.title };
                    //    di.add(i);
                    //}
                    //else if (savetype == "up")
                    if (savetype == "up")
                    {
                        if (di.exist(a => a.id == o.id))
                        {
                            di.edit(n=>n.id==o.id,g=> new info {  sorts = o.sorts, title=o.title } );
                        }
                    }
                }
            }
            Response.Write(ykmWeb.common.common.divalert(Url.Action("Index"), "提交成功", true));
            Response.End();
           return null;
        }
        [webAuthorzize]
        public ActionResult del(int id)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalInfo di = new DalInfo(s);
                info i = di.find(a => a.id == id);
                if (!string.IsNullOrEmpty(i.defaultpic))
                {
                    ykmWeb.common.common.delfiles(i.defaultpic);
                }
                //List<Gallery> l = dg.FindList(n => n.infoid == id, 0, null).ToList();
                //if(l.Count>0)
                //{
                //    foreach(var o in l)
                //    {
                //        ykmWeb.common.common.delfiles(o.photourl);
                //        dg.del(o);
                //    }
                //}
                di.del(i);
                Response.Redirect(Request.UrlReferrer.ToString());
                Response.End();
                return null;
            } 
        }
        [webAuthorzize]
        [HttpPost]
        public void delall(string id="")
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                string[] arrid = id.Split(',');
                DalInfo di = new DalInfo(s);

                var l = di.FindList(n => arrid.Contains(n.id.ToString()), 0, null).Select(n => new { id=n.id, uploadfiles = n.uploadfiles }).ToList();
                if (l.Count() > 0)
                {
                    foreach (var v in l)
                    {
                        ykmWeb.common.common.delfiles(v.uploadfiles);
                        //List<Gallery> lg = dg.FindList(n => n.infoid == v.id, 0, null).ToList();
                        //if (lg.Count > 0)
                        //{
                        //    foreach (var o in lg)
                        //    {
                        //        ykmWeb.common.common.delfiles(o.photourl);
                        //        dg.del(o);
                        //    }
                        //}
                    }
                }
                di.del_all(n => arrid.Contains(n.id.ToString()));
            }
        }
        [webAuthorzize]
        [HttpPost]
        public void chageall(string id,string cid)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                string[] arrid = id.Split(',');
                int _cid = int.Parse(cid);
                DalMenuClass dic = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                var ic = dic.FindList(n => n.Catalogid == _cid, 0, null).Select(g=>new {g.Catalogid,g.Catalogname }).FirstOrDefault();
                if (ic != null)
                {
                    di.edit(n => arrid.Contains(n.id.ToString()), g => new info { classid = ic.Catalogid});
                }
            }
        }
        [webAuthorzize]
        public ActionResult dostate(int id)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalInfo di = new DalInfo(s);
                string type = Request.QueryString["t"];
                switch (type)
                {
                    case "1":
                        di.edit(n => n.id == id, g => new info { istop = 1 });
                        break;
                    case "2":
                        di.edit(n => n.id == id, g => new info { istop = 0 });
                        break;
                    case "3":
                        //di.edit(n => n.id == id, g => new info { tj = 1 });
                        break;
                    case "4":
                        //di.edit(n => n.id == id, g => new info { tj = 0});
                        break;
                    case "5":
                        //di.edit(n => n.id == id, g => new info { state = 100 });
                        break;
                    case "6":
                        //di.edit(n => n.id == id, g => new info { state = 200 });
                        break;
                }
                Response.Redirect(Request.UrlReferrer.ToString());
                Response.End();
                return null;
            }
        }






        //产品列表
        [webAuthorzize]
        public ActionResult prolist(int id = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalInfo di = new DalInfo(s);
                string p = ykmWeb.common.common.IsNumeric(Request.QueryString["Page"]);
                string key = Request.QueryString["key"];
                string sdate = Request.QueryString["sdate"];
                string edate = Request.QueryString["edate"];
                if (p == "0")
                {
                    p = "1";
                }
                string pagestr = "&id=" + id + "&key=" + Server.UrlEncode(key) + "&sdate=" + sdate + "&edate=" + edate;
                int row = 0;
                Expression<Func<info, bool>> wherelba = PredicateExtensionses.True<info>();
                if (id != 0)
                {
                    wherelba = wherelba.And(u => u.classid == id);
                }
                if (string.IsNullOrEmpty(key) == false)
                {
                    wherelba = wherelba.And(u => u.title.Contains(key));
                }
                if (string.IsNullOrEmpty(sdate) == false)
                {
                    DateTime _sdate = DateTime.Parse(sdate);
                    wherelba = wherelba.And(u => u.insertdate >= _sdate);
                }

                if (string.IsNullOrEmpty(edate) == false)
                {
                    DateTime _edate = DateTime.Parse(edate);
                    wherelba = wherelba.And(u => u.insertdate <= _edate);
                }

                DalMenuClass dmc = new DalMenuClass(s);
                var c = dmc.FindList(n => n.Catalogid == id, 1, null).Select(g => g.Catalogname).SingleOrDefault();
                ViewBag.pagetitle = c;
                ViewBag.pagecid = id;

                var ilist = di.FindListPage(int.Parse(p), 10, out row, wherelba, new OrderModelField[] { new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).Select(n => new ht_list_info { id = n.id, title = n.title, insertdate = n.insertdate, classid = n.classid, defaultpic = n.defaultpic, price = n.price }).ToList();

                if (ilist.Count() > 0)
                {
                    ViewBag.footpage = ykmWeb.common.common.PageFoot_bootstrap(row, 10, p, System.Web.HttpContext.Current.Request.Path, pagestr, "cn", true, true);
                }
                ViewBag.infocount = row;
                return View(ilist);
            }
        }
        [webAuthorzize]
        public ActionResult procont(int id = 0, int cid = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                info i;
                if (id == 0)
                {
                    i = new info();
                    i.id = 0;
                    i.istop = 0;
                    i.insertdate = DateTime.Now;
                    i.sorts = 1;
                    i.classid = cid;
                    i.issame = 1;
                    ViewBag.savetype = "save";
                }
                else
                {
                    DalInfo di = new DalInfo(s);
                    i = di.FindList(n => n.id == id, 1, null).FirstOrDefault();
                    ViewBag.savetype = "up";
                }
                return View(i);
            }

        }

        //新闻列表
        [webAuthorzize]
        public ActionResult newslist(int id = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalInfo di = new DalInfo(s);
                string p = ykmWeb.common.common.IsNumeric(Request.QueryString["Page"]);
                string key = Request.QueryString["key"];
                string sdate = Request.QueryString["sdate"];
                string edate = Request.QueryString["edate"];
                if (p == "0")
                {
                    p = "1";
                }
                string pagestr = "&key=" + Server.UrlEncode(key) + "&sdate=" + sdate + "&edate=" + edate;
                int row = 0;
                Expression<Func<info, bool>> wherelba = PredicateExtensionses.True<info>();
                if (id != 0)
                {
                    wherelba = wherelba.And(u => u.classid == id);
                }
                if (string.IsNullOrEmpty(key) == false)
                {
                    wherelba = wherelba.And(u => u.title.Contains(key));
                }
                if (string.IsNullOrEmpty(sdate) == false)
                {
                    DateTime _sdate = DateTime.Parse(sdate);
                    wherelba = wherelba.And(u => u.insertdate >= _sdate);
                }

                if (string.IsNullOrEmpty(edate) == false)
                {
                    DateTime _edate = DateTime.Parse(edate);
                    wherelba = wherelba.And(u => u.insertdate <= _edate);
                }
                DalMenuClass dmc = new DalMenuClass(s);
                var c = dmc.FindList(n => n.Catalogid == id, 1, null).Select(g => g.Catalogname).SingleOrDefault();
                ViewBag.pagetitle = c;
                ViewBag.pagecid = id;

                var ilist = di.FindListPage(int.Parse(p), 10, out row, wherelba, new OrderModelField[] { new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).Select(n => new ht_list_info { id = n.id, title = n.title, insertdate = n.insertdate, classid = n.classid, defaultpic = n.defaultpic, price = n.price }).ToList();

                if (ilist.Count() > 0)
                {
                    ViewBag.footpage = ykmWeb.common.common.PageFoot_bootstrap(row, 10, p, System.Web.HttpContext.Current.Request.Path, pagestr, "cn", true, true);
                }
                ViewBag.infocount = row;
                return View(ilist);
            }
        }
        [webAuthorzize]
        public ActionResult newscont(int id = 0, int cid = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                info i;
                if (id == 0)
                {
                    i = new info();
                    i.id = 0;
                    i.istop = 0;
                    i.insertdate = DateTime.Now;
                    i.sorts = 1;
                    i.classid = cid;
                    i.issame = 1;
                    ViewBag.savetype = "save";
                }
                else
                {
                    DalInfo di = new DalInfo(s);
                    i = di.FindList(n => n.id == id, 1, null).FirstOrDefault();
                    ViewBag.savetype = "up";
                }
                return View(i);
            }

        }

        /// <summary>
        /// 内容页
        /// </summary>
        /// <param name="id">这里id是catalogid</param>
        /// <returns></returns>
        [webAuthorzize]
        public ActionResult cont(int id = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                info i;
                var c = dmc.FindList(n => n.Catalogid == id, 0, null).SingleOrDefault();
                if (id > 0 && c != null)
                {
                    ViewBag.pagetitle = c.Catalogname;
                    i = di.FindList(n => n.classid == id, 1, null).SingleOrDefault();
                    if (i == null)
                    {
                        i = new info();
                        i.title = c.Catalogname;
                        i.classid = id;
                        i.issame = 1;
                        ViewBag.savetype = "save";
                    }
                    else
                    {
                        ViewBag.savetype = "up";
                    }
                    return View(i);
                }
                else
                {
                    return View("未知错误");
                }
            }

        }

        /// <summary>
        /// 添加/修改 通用版
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        [webAuthorzize]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult update(info o)
        {
            if (ModelState.IsValid)
            {
                string savetype = Request.Form["savetype"];
                string pagetype = Request.Form["pagetype"];
                using (ykmWebDbContext s = new ykmWebDbContext())
                {
                    DalInfo di = new DalInfo(s);
                    info i = new info();

                    switch(pagetype)
                    {
                        case "cont":
                            if (savetype == "save")
                            {
                                i = new info { title = o.title, classid = o.classid, cont = o.cont, h5cont = o.h5cont, issame = o.issame, insertdate = o.insertdate, istop = 0, sorts = 0,intro = o.intro };
                                var u = di.add(i);
                            }
                            else if (savetype == "up")
                            {
                                if (di.exist(a => a.id == o.id))
                                {
                                    di.edit(n => n.id == o.id, g => new info { title = o.title, classid = o.classid, cont = o.cont, h5cont = o.h5cont, issame = o.issame, intro = o.intro });
                                }
                            }
                            Response.Write(ykmWeb.common.common.divalert(Url.Action("cont", new { id = o.classid }), "提交成功", true));
                            Response.End();
                            return null;
                        case "procont":
                            if (savetype == "save")
                            {
                                i = new info { title = o.title, classid = o.classid, cont = o.cont, h5cont = o.h5cont, issame = o.issame, insertdate = o.insertdate, istop = 0, sorts = 0, defaultpic = o.defaultpic, uploadfiles = o.uploadfiles, price = o.price };
                                var u = di.add(i);
                            }
                            else if (savetype == "up")
                            {
                                if (di.exist(a => a.id == o.id))
                                {
                                    di.edit(n => n.id == o.id, g => new info { title = o.title, classid = o.classid, cont = o.cont, h5cont = o.h5cont, issame = o.issame, istop = o.istop, insertdate = o.insertdate, defaultpic = o.defaultpic, sorts = o.sorts, uploadfiles = o.uploadfiles });
                                }
                            }
                            Response.Write(ykmWeb.common.common.divalert(Url.Action("prolist", new { id = o.classid }), "提交成功", true));
                            Response.End();
                            return null;
                        case "newscont":
                            if (savetype == "save")
                            {
                                i = new info { title = o.title, classid = o.classid, cont = o.cont, h5cont = o.h5cont, issame = o.issame, insertdate = o.insertdate, istop = 0, sorts = 0, defaultpic = o.defaultpic, uploadfiles = o.uploadfiles, intro = o.intro };
                                var u = di.add(i);
                            }
                            else if (savetype == "up")
                            {
                                if (di.exist(a => a.id == o.id))
                                {
                                    di.edit(n => n.id == o.id, g => new info { title = o.title, classid = o.classid, cont = o.cont, h5cont = o.h5cont, issame = o.issame, istop = o.istop, insertdate = o.insertdate, defaultpic = o.defaultpic, sorts = o.sorts, uploadfiles = o.uploadfiles, intro = o.intro });
                                }
                            }
                            Response.Write(ykmWeb.common.common.divalert(Url.Action("newslist", new { id = o.classid }), "提交成功", true));
                            Response.End();
                            return null;
                        default:
                            break;
                    }
                }
            }
            Response.Write(ykmWeb.common.common.divalert(Url.Action("Index"), "提交成功", true));
            Response.End();
            return null;
        }
    }
}