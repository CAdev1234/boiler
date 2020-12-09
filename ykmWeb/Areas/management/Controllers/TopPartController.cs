using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ykmWeb.Areas.management.Controllers
{
    public class TopPartController : Controller
    {
        [ChildActionOnly]
        public ActionResult toppart()
        {
            return View();
        }
    }
}