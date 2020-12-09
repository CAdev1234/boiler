using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ykmWeb.common;

namespace ykmWeb.Areas.management.Controllers
{
    public class uploadfilesController : Controller
    {
        // GET: management/uploadfiles
        [HttpPost]
        [webAuthorzize]
        public string Index()
        {
            if (Request.Files.Count > 0)
            {
                sanduploads u = new sanduploads();
                u.imgW = 2000;
                u.imgH = 2000;
                u.UpMaxSize = 20480000;
                string filename = u.uploadfiles_mvc(Request.Files[0], "/uploads/");
                return ("{\"result\":\"" + filename + "\"}");
            }
            else
            {
                return "";
            }
        }
    }
}