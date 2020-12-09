using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ykmWeb.Bll;
using ykmWeb.common;

namespace ykmWeb.Areas.management.Controllers
{
    public class leftpartController : Controller
    {
        // GET: yljmanager/leftpart
        [ChildActionOnly]
        public ActionResult listleftMenu(string lang="cn")
        {
            user_config_type uct = new user_config_type("master_type");
            ViewBag.uname = Session["master"].ToString();
            ViewBag.utype = uct.get_text(Session["qxtype"].ToString());

            string qxstr = "",smstr="";
        
            if (HttpContext.Session["qxtype"].ToString()!= "100")
            {
                qxstr = (string.IsNullOrEmpty(HttpContext.Session["qx"].ToString())) ? "0" : HttpContext.Session["qx"].ToString();
                smstr = (string.IsNullOrEmpty(HttpContext.Session["qxlv"].ToString())) ? "0" : HttpContext.Session["qxlv"].ToString();
            }   

            ht_menu ht = new ht_menu();
            ViewBag.menustr = ht.list_left_menu(qxstr, smstr, lang);
            return View();
        }
    }
}