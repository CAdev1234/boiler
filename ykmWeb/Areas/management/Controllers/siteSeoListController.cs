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

namespace ykmWeb.Areas.management.Controllers
{
    public class siteSeoListController : Controller
    {
        // GET: management/siteSeoList
        [webAuthorzize]
        public ActionResult Index(string lang="cn")
        {
            using(ykmWebDbContext s=new ykmWebDbContext())
            {
                DalSiteSeo dss = new DalSiteSeo(s);
                var o = dss.FindList(n=>n.lang== lang, 1, new OrderModelField[] { new OrderModelField { propertyName = "id", IsDESC = true } }).FirstOrDefault();
                if (o == null)
                {
                    o = new siteseo();
                }
                ViewBag.lang = lang;
                return View(o);
            }   
        }

        [webAuthorzize]
        [HttpPost]
        public string update(siteseo se)
        {
            using(ykmWebDbContext s=new ykmWebDbContext())
            {
                DalSiteSeo dss = new DalSiteSeo(s);
                if (dss.count(n=>n.id == se.id) <= 0)
                {
                    dss.add(se);
                }
                else
                {
                    dss.edit(se);
                }
                return common.common.divalert(Url.Action("Index", new { lang = se.lang }), "提交成功", true);
            }
        }
    }
}