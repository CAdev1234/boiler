using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ykmWeb.Areas.management.Controllers
{
    public class topmenuController : Controller
    {
        // GET: yljmanager/topmenu

        [ChildActionOnly]
        public ActionResult showtopmenu()
        {
       //     Session["qxtype"] = "100";
            string id = RouteData.Values["id"].ToString();
            string cid = RouteData.Values["cid"].ToString();
            ykmWeb.Bll.ht_menu ht = new ykmWeb.Bll.ht_menu();
            string qxstr = "";
            if (Session["qxtype"].ToString() != "100")
            {
                qxstr = Session["qxlv"].ToString();
            }
            //string s = ht.list_top_child(id, cid, qxstr);
            //ViewBag.topmenuhtml = s;
            return View();
        }
    }
}