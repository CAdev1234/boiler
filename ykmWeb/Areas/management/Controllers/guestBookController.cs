using ykmWeb.Areas.management.Controllers;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ykmWeb.Models;
using System.Linq.Expressions;

namespace ykmWeb.Areas.management.Controllers
{
    public class guestBookController : Controller
    {
        // GET: management/guestBook
        [webAuthorzize]
        public ActionResult Index(int page = 1)
        {
            string key = Request.QueryString["key"];
            string key2 = Request.QueryString["key2"];
            string sdata = Request.QueryString["sdata"];
            string edata = Request.QueryString["edata"];
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                if (page <= 1)
                {
                    page = 1;
                }
                int row = 0;


                Expression<Func<guestbook, bool>> wherelba = PredicateExtensionses.True<guestbook>();
                //    key = "新闻";
                if (string.IsNullOrEmpty(key) == false)
                {
                    wherelba = wherelba.And(u => u.cont.Contains(key));
                }
                //if (string.IsNullOrEmpty(key2) == false)
                //{
                //    wherelba = wherelba.And(u => u.username.Contains(key2));
                //}

                if (string.IsNullOrEmpty(sdata) == false)
                {
                    DateTime _sdate = DateTime.Parse(sdata);
                    wherelba = wherelba.And(u => u.insertdate >= _sdate);
                }

                if (string.IsNullOrEmpty(edata) == false)
                {
                    DateTime _edate = DateTime.Parse(edata);
                    wherelba = wherelba.And(u => u.insertdate <= _edate);
                }

                string pagestr = "key=" + key +"&key2="+ key2 + "&sdate="+sdata +"&edate="+ edata;
                DalGuestBook di = new DalGuestBook(s);
                var ilist = di.FindListPage(page, 20, out row, wherelba, new OrderModelField[] { new OrderModelField { propertyName = "insertdate", IsDESC = true },new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                if (ilist.Count() > 0)
                {
                    ViewBag.footpage = common.common.PageFoot_bootstrap(row, 20, page.ToString(), System.Web.HttpContext.Current.Request.Path, pagestr, "cn", true, true);
                }
                ViewBag.infocount = row;
                return View(ilist);
            }
        }

        [webAuthorzize]
        public ActionResult del(int id = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalGuestBook di = new DalGuestBook(s);
                di.del_all(n => n.id == id);
                return RedirectToAction("index");
            }
        }

        [webAuthorzize]
        public ActionResult view(int id = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalGuestBook drf = new DalGuestBook(s);
                var i = drf.find(n => n.id == id);
                if (i != null)
                {
                    drf.edit(n => n.id == i.id, g => new guestbook { state = 100 });
                    return View(i);
                }
                return View();
            }
        }
    }
}