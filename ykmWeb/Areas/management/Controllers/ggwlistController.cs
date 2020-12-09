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
    public class ggwlistController : Controller
    {
     
        // GET: yljmanager/ggwlist

        [webAuthorzize]
        public ActionResult Index()
        {
            string p = common.common.IsNumeric(Request.QueryString["Page"]);
            string key = Request.QueryString["key"];
            string posi = Request.QueryString["posi"];

            if (p == "0")
            {
                p = "1";
            }
            string pagestr = "&key="+Server.UrlEncode(key)+"&posi="+posi;
            using (ykmWebDbContext s=new ykmWebDbContext())
            {
                DalGgw di = new DalGgw(s);
                int row = 0;
                Expression<Func<ggw, bool>> wherelba = PredicateExtensionses.True<ggw>(); ;

                if (string.IsNullOrEmpty(key) == false)
                {
                    wherelba = wherelba.And(u => u.title.Contains(key));
                }
                if (string.IsNullOrEmpty(posi) == false)
                {
                    wherelba = wherelba.And(u => u.ggwposition == posi);
                }
                var ilist = di.FindListPage(int.Parse(p), 20, out row, wherelba, new OrderModelField[] { new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                if (ilist.Count() > 0)
                {
                    ViewBag.footpage = common.common.PageFoot_bootstrap(row, 20, p, System.Web.HttpContext.Current.Request.Path, pagestr, "cn", true, true);
                }
                ViewBag.infocount = row;
                return View(ilist);
            }
         
        }

        [webAuthorzize]
        public ActionResult edit(int id=0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalGgw di = new DalGgw(s);
                ggw g = di.find(n => n.id == id);
                if (g == null)
                {
                    ViewBag.savetype = "save";
                    g = new ggw();
                    g.sorts = 1;
                }
                else
                {
                    ViewBag.savetype = "up";
                }
                return View(g);
            }
        }

        [webAuthorzize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ggwupdata(ggw g)
        {
            //   var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                using (ykmWebDbContext s = new ykmWebDbContext())
                {
                    DalGgw di = new DalGgw(s);
                    ggw i = new ggw { ggwlink = g.ggwlink, ggwposition = g.ggwposition, imgurl = g.imgurl, title = g.title, sorts = g.sorts };
                    string savetype = Request.Form["savetype"];
                    if (savetype == "save")
                    {
                        i.insertdate = DateTime.Now;
                        di.add(i);
                    }
                    else if (savetype == "up")
                    {
                        i.id = g.id;
                        if (di.exist(a => a.id == g.id))
                        {
                            di.edit(i);
                        }

                    }
                }
                Response.Write(common.common.divalert(Url.Action("Index"), "提交成功", true));
                Response.End();          
            }
            return null;
        }

        [webAuthorzize]
        public void del(int id)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalGgw di = new DalGgw(s);
                ggw g = di.find(n => n.id == id);
                if (g != null)
                {
                    common.common.delfiles(g.imgurl);
                    di.del(g);
                }
                Response.Redirect(Request.UrlReferrer.ToString());
                Response.End();
            }
        }

        [webAuthorzize]
        [HttpPost]
        public void delall(string id = "")
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalGgw di = new DalGgw(s);
                string[] arrid = id.Split(',');
                var l = di.FindList(n => arrid.Contains(n.id.ToString()), 0, null);
                if (l.Count() > 0)
                {
                    foreach (ggw v in l)
                    {
                        common.common.delfiles(v.imgurl);
                    }
                }
                di.del_all(n => arrid.Contains(n.id.ToString()));
            }
        }
    }
}