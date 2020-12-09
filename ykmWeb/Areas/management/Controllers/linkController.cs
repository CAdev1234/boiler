using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;
using ykmWeb.Models;
using System.Linq.Expressions;
using ykmWeb.common;

namespace ykmWeb.Areas.management.Controllers
{
    public class linkController : Controller
    {
        // GET: yljmanager/link
        private DalLink di = new DalLink(new ykmWebDbContext());
        private link i = new link();

        //private shoplinkclass i = new shoplinkclass();
        //private do_shoplink di2 = new do_shoplink();
        //private shoplink i2 = new shoplink();

        //[webAuthorzize]
        //public ActionResult classlist()
        //{

        //    string p = common.IsNumeric(Request.QueryString["Page"]);
        //    if (p == "0")
        //    {
        //        p = "1";
        //    }
        //    string pagestr = "";
        //    int row = 0;
        //    Expression<Func<shoplinkclass, bool>> wherelba = PredicateExtensionses.True<shoplinkclass>(); ;
        //    var ilist = di.FindListPage(int.Parse(p), 20, out row, wherelba, new OrderModelField[] { new OrderModelField { propertyName = "id", IsDESC = true } });
        //    if (ilist.Count() > 0)
        //    {
        //        ViewBag.footpage = sand_common.common.PageFoot_bootstrap(row, 20, p, System.Web.HttpContext.Current.Request.Path, pagestr, "cn", true, true);
        //    }
        //    ViewBag.infocount = row;
        //    return View(ilist);
        //}

        //[webAuthorzize]
        //public ActionResult editclass(int id = 0)
        //{
        //    i = di.find(n => n.id == id);
        //    if (i == null)
        //    {
        //        ViewBag.savetype = "save";

        //        i = new shoplinkclass();

        //    }
        //    else
        //    {
        //        ViewBag.savetype = "up";
        //    }
        //    return View(i);
        //}

        //[HttpPost]
        //[webAuthorzize]
        //[ValidateAntiForgeryToken]
        //public void savelinkclass(shoplinkclass p)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string savetype = Request.Form["savetype"];
        //        if (savetype == "save")
        //        {
        //            di.add(p);
        //        }
        //        else
        //        {
        //            di.edit(p);
        //        }
        //        Response.Write(common.divalert(Url.Action("list"), "提交成功", true));
        //        Response.End();
        //    }
        //}

        //[webAuthorzize]
        //public void del(int id = 0)
        //{
        //    di.del_all(n => n.id == id);
        //    Response.Redirect(Request.UrlReferrer.ToString());
        //    Response.End();
        //}



        //////////////////////////////////////////////////////////////////////////////////////////////////


        [webAuthorzize]
        public ActionResult list()
        {
            string clssid= common.common.IsNumeric(Request.QueryString["key"]);
            string p = common.common.IsNumeric(Request.QueryString["Page"]);
            if (p == "0")
            {
                p = "1";
            }
            string pagestr = "";
            int row = 0;
            Expression<Func<link, bool>> wherelba = PredicateExtensionses.True<link>();
            if (clssid != "0")
            {
                int _classid = int.Parse(clssid);
                wherelba = wherelba.And(n => n.classid == _classid);
            }
            var ilist = di.FindListPage(int.Parse(p), 20, out row, wherelba, new OrderModelField[] { new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
            if (ilist.Count() > 0)
            {
                ViewBag.footpage = common.common.PageFoot_bootstrap(row, 20, p, System.Web.HttpContext.Current.Request.Path, pagestr, "cn", true, true);
            }
            ViewBag.infocount = row;
            return View(ilist);
        }

        [webAuthorzize]
        public ActionResult edit(int id = 0)
        {
            i = di.find(n => n.id == id);
            if (i == null)
            {
                ViewBag.savetype = "save";
                i = new link();
            }
            else
            {
                ViewBag.savetype = "up";
            }
            return View(i);
        }

        [HttpPost]
        [webAuthorzize]
        [ValidateAntiForgeryToken]
        public void save(link p)
        {
            if (ModelState.IsValid)
            {
                string savetype = Request.Form["savetype"];
                if (string.IsNullOrEmpty(p.linkimg) == false)
                {
                    p.classtype = "img";
                }
                else
                {
                    p.classtype = "info";
                }
                if (savetype == "save")
                {
                    di.add(p);
                }
                else
                {
                    di.edit(p);
                    //di.edit(p, new List<string> { "classid","classtype","linkname","linkurl","linkimg","uploadfiles","infosoft","state" });
                }
                Response.Write(common.common.divalert(Url.Action("list"), "提交成功", true));
                Response.End();
            }
        }

        [webAuthorzize]
        public void dellink(int id = 0)
        {
            i = di.find(n => n.id == id);
            if(i != null)
            { 
                //common.common.delImg(i.uploadfiles);
                di.del_all(n => n.id == id);
            }
            Response.Redirect(Request.UrlReferrer.ToString());
            Response.End();
        }
    }
}