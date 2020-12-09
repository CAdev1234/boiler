using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ykmWeb.Models;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;
using ykmWeb.common;
using System.Linq.Expressions;

namespace ykmWeb.Areas.management.Controllers
{
    public class webmasterController : Controller
    {
        // GET: yljmanager/webmaster
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="w"></param>
        [webAuthorzize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public void updata(webmanager w)
        {
            int uid = int.Parse(Session["Muid"].ToString());
            string uname = Session["master"].ToString();
            int utype = int.Parse(Session["qxtype"].ToString());
            string savetype = Request.Form["savetype"];
            if (ModelState.IsValid)
            {
                using (ykmWebDbContext s = new ykmWebDbContext())
                {
                    DalWebmanager di = new DalWebmanager(s);
                    if (savetype == "save")
                    {
                        if (di.exist(n => n.adminname == w.adminname))
                        {
                            Response.Write(ykmWeb.common.common.ErrorInfo("用户名重复"));
                            Response.End();
                        }
                        else
                        {
                            di.add(new webmanager { adminname = w.adminname, adminpass = ykmWeb.common.common.Sha256(w.adminpass), bigqx = "", smqx = "", qxtype = w.qxtype, infodate=DateTime.Now, create_userid = uid, create_username= uname, create_usertype = utype, shop_id = uid });
                            Response.Write(ykmWeb.common.common.divalert(Url.Action("index"), "提交成功", true));
                        }
                    }
                    else if (savetype == "up")
                    {
                        di.edit(n=>n.id==w.id,g=> new webmanager { adminpass = ykmWeb.common.common.Sha256(w.adminpass),qxtype = w.qxtype });
                        Response.Write(ykmWeb.common.common.divalert(Url.Action("index"), "提交成功", true));
                    }
                }
            }
        }
        [webAuthorzize]
        public string checkuname()
        {
            string u = Request.QueryString["adminname"];
            return "{\"valid\":\"true\"}";
        }

        public string checkcode()
        {  
            string u = Request.QueryString["checkcode"];
            if (Session["yljcheode"] == null)
            {
                return "{\"valid\":\"false\"}";
            }      
            if(u.ToLower() != Session["yljcheode"].ToString().ToLower())
            {
                return "{\"valid\":\"false\"}";
            }
            return "{\"valid\":\"true\"}";
        }
        [webAuthorzize]
        public ActionResult edit(int id = 0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalWebmanager di = new DalWebmanager(s);
                webmanager w = di.find(n => n.id == id);
                if (w == null)
                {
                    ViewBag.savetype = "save";
                    w = new webmanager();
                }
                else
                {
                    ViewBag.savetype = "up";
                }
                return View(w);
            }
        }
        [webAuthorzize]
        public ActionResult Index()
        {
            //   Response.Write(ykmWeb.common.common.Sha256("123456"));
            // Response.Write(common.checkstr("mobile", "19804159352"));
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalWebmanager di = new DalWebmanager(s);
                string p = ykmWeb.common.common.IsNumeric(Request.QueryString["Page"]);
                if (p == "0")
                {
                    p = "1";
                }
                string pagestr = "";
                int row = 0;
                var ilist = di.FindListPage(int.Parse(p), 10, out row, null, new OrderModelField[] { new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                if (ilist.Count() > 0)
                {
                    ViewBag.footpage = ykmWeb.common.common.PageFoot_bootstrap(row, 10, p, System.Web.HttpContext.Current.Request.Path, pagestr, "cn", true, true);
                }

                return View(ilist);
            }
        }

        [webAuthorzize(qxstr ="100")]
        public ActionResult del(int? id)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalWebmanager di = new DalWebmanager(s);
                di.del_all(n => n.id == id);
                return RedirectToAction("index");
            }
        }

        public ActionResult maindefault()
        {
            //Response.Write(ykmWeb.common.common.checkstr("mobile", "18342569876"));
            //  ctrl_app_mobile_code cam = new ctrl_app_mobile_code();
        //    Response.Write(ykmWeb.common.common.Encode("536fe0e6f9021d6cc0c363dd50075402", "veD9ef65"));
            // string result = cam.send("15641537198", "SMS_144915968", "{\"code\":\"111111\"}");
            ViewData["bgimg"] =common.common.reRandRoundNum(1, 7) + ".jpg";
            Bll.master_login ml = new Bll.master_login();
            if (ml.checkonlien())
            {
                return Redirect(Url.Action("defaultmain"));
            }
            else
            {
                LoginViewModel l = new LoginViewModel();
                HttpCookie cokpass = Request.Cookies["yljmanage_User_pass"];
                if (cokpass != null)
                {
                    l.UserName = ykmWeb.common.common.Decode(cokpass["uname"]);
                    l.Password = ykmWeb.common.common.Decode(cokpass["pass"]);
                    l.RememberMe = true;
                }
                else
                {
                    l.RememberMe = false;
                }
                return View(l);
            }
          
           
        }

        public ActionResult viewcode()
        {
            return null;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public void Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                using (ykmWebDbContext s = new ykmWebDbContext())
                {
                    DalWebmanager di = new DalWebmanager(s);
                    var _user = di.find(n => n.adminname == loginViewModel.UserName);
                    string savepass = Request.Form["savepass"];
                    if (_user == null) Response.Write(ykmWeb.common.common.divalert(Url.Action("maindefault", "webmaster"), "用户名不存在", true));
                    else if (_user.adminpass == ykmWeb.common.common.Sha256(loginViewModel.Password))
                    {
                        string usertoken = common.common.GenerateStringID();
                        _user.usertoken = usertoken;
                        di.edit(g => g.id == _user.id, n => new webmanager { usertoken = usertoken });
                        Bll.master_login ml = new Bll.master_login();
                        ml.insertSession(_user);
                        if (savepass == "true")
                        {
                            HttpCookie cokpass = Request.Cookies["yljmanage_User_pass"];
                            if (cokpass == null)
                            {
                                cokpass = new HttpCookie("yljmanage_User_pass");
                            }
                            cokpass.Expires = DateTime.Now.AddDays(15);
                            cokpass["uname"] = ykmWeb.common.common.Encode(loginViewModel.UserName);
                            cokpass["pass"] = ykmWeb.common.common.Encode(loginViewModel.Password);
                            Response.SetCookie(cokpass);
                        }
                        else
                        {
                            HttpCookie cok;
                            cok = Request.Cookies["yljmanage_User_pass"];
                            if (cok != null)
                            {
                                Response.Cookies["yljmanage_User_pass"].Expires = DateTime.Now.AddMilliseconds(-1);
                            }
                        }
                        //   Response.Write(ykmWeb.common.common.divalert(Url.Action("Index", "Infolist"), "登录成功", true));
                        //  Response.End();
                        Response.Redirect(Url.Action("defaultmain", "webmaster"));
                    }
                    else
                    {

                        Response.Write(ykmWeb.common.common.divalert(Url.Action("maindefault", "webmaster"), "密码错误", true));
                        Response.End();
                    }
                }
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        public void layout()
        {
            Bll.master_login ml = new Bll.master_login();
            ml.liveout();
            Response.Redirect(Url.Action("maindefault", "webmaster"));
        }

        /// <summary>
        /// 权限显示
        /// </summary>
        /// <returns></returns>
        [webAuthorzize(qxstr = "100")]
        public ActionResult listqx(int id=0)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalWebmanager di = new DalWebmanager(s);
                Bll.ht_menu hm = new Bll.ht_menu();
                webmanager w = di.FindList(n => n.id == id,1,null).FirstOrDefault();
                if (w == null)
                {
                    Response.End();
                }
                ViewData["qxstr"] = hm.list_checkbox_qx(w.bigqx, w.smqx);
                return View(w);
            }
        
        }
        [webAuthorzize(qxstr ="100")]
        [HttpPost]
        public void upqx(ykmWeb.Models.webmanager w)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalWebmanager di = new DalWebmanager(s);
                string bigqx = Request.Form["bqx"];
                string smqx = Request.Form["mqx"];
                w.bigqx = "," + bigqx + ",";
                w.smqx = "," + smqx + ",";
                di.edit(n=>n.id==w.id,g=>new webmanager { qxtype=w.qxtype,bigqx=w.bigqx,smqx=w.smqx});
                Response.Write(ykmWeb.common.common.divalert(Url.Action("index"), "提交成功", true));
            }
        }
        [webAuthorzize]
        public ActionResult listqxerror()
        {
            return View();
        }
        [webAuthorzize]
        public ActionResult pass()
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalWebmanager di = new DalWebmanager(s);
                int id = int.Parse(Session["Muid"].ToString());
                webmanager w = di.find(n => n.id == id);
                if (w == null)
                {
                    Response.End();
                }
                return View(w);
            }
        }

        [webAuthorzize]
        [HttpPost]
        public void uppass(webmanager w)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalWebmanager di = new DalWebmanager(s);
                w.adminpass = ykmWeb.common.common.Sha256(w.adminpass);
                di.edit(n=>n.id==w.id,g=>new webmanager { adminpass=w.adminpass });
                Response.Write(ykmWeb.common.common.divalert(Url.Action("pass"), "提交成功", true));
            }
        }

        [webAuthorzize]
        public ActionResult defaultmain(string lang = "cn")
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalInfo di = new DalInfo(s);
                
                DalMenuClass dic = new DalMenuClass(s);
                ViewData["infocount"] = di.count(null);
                ViewData["classcount"] = dic.count(null);
                ViewBag.lang = lang;
                //   return View(dic.FindList(null, 0, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList());
                return View();
            }
        }

        [ChildActionOnly]
        public string get_web_master_name(int adminid)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalWebmanager di = new DalWebmanager(s);
                var o = di.FindList(n => n.id == adminid, 0, null).Select(n => new { name = n.adminname }).SingleOrDefault();
                if (o != null)
                {
                    return o.name;
                }
                else
                {
                    return "";
                }
            }
        }

        public int get_shop_id(int uid)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalWebmanager di = new DalWebmanager(s);
                var u = di.find(n => n.id == uid);
                if (u.qxtype == 100)
                {
                    return u.id.Value;
                }
                else
                {
                    return get_shop_id(u.create_userid.Value);
                }
            }
        }

    }
}