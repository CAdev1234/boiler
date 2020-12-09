using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ykmWeb.Dal.Serv;
using ykmWeb.Dal;
using ykmWeb.Models;
using ykmWeb.Areas.management.Controllers;
using ykmWeb.common;

namespace yljmanager.Controllers
{
    public class getuploadfilesController : Controller
    {
        // GET: getuploadfiles
        //[webAuthorzize]
        //[HttpPost]
        //public JsonResult updata()
        //{
        //    //ykmWeb.Bll.user_login ul = new ykmWeb.Bll.user_login();
        //    //var r = ul.checkuserlogin(usertoken);
        //    //if (!r)
        //    //{
        //    //    return Json(e.return_state(212, ""));
        //    //}

        //    if (Request.Files.Count > 0)
        //    {
        //        sanduploads u = new sanduploads();
        //        u.imgW = 750;
        //        u.imgH = 750;
        //        u.UpMaxSize = 20480;
        //        string filename = u.uploadfiles_mvc(Request.Files[0], "/uploads/");
        //        string[] f = filename.Split('|');
        //        if (f[0] == "ok")
        //        {
        //            //ap.datafilename = f[1];
        //            //ap.datafileurl = f[1];
        //            //List<api_file_load> afl = new List<api_file_load>();
        //            //afl.Add(ap);
        //            //s.data = afl;
        //            //s.message = "上传成功";
        //            //s.Rowscount = 1;
        //            //s.state = 100;
        //        }
        //        else
        //        {
        //            //s.data = null;
        //            //s.message = f[1];
        //            //s.Rowscount = 0;
        //            //s.state = 400;
        //        }
        //    }
        //    else
        //    {
        //       // sand_wx_app.tools_class.WriteLog("无文件");
        //    }
        //    //return Json(s);
        //}

        //[HttpPost]
        //public JsonResult delfiles(string usertoke, string files)
        //{
        //    error_code e = new error_code();
        //    ykmWeb.Bll.user_login ul = new ykmWeb.Bll.user_login();
        //    var r = ul.checkuserlogin(usertoke);
        //    if (!r)
        //    {
        //        return Json(e.return_state(212, ""));
        //    }

        //    common.delfiles(files);
        //    return Json(e.return_state(100, ""));
        //}


        //[userAuthorzize]
        [HttpPost]
        public string uploads()
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