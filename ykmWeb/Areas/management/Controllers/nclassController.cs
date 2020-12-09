using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ykmWeb.Bll;
using ykmWeb.Dal;

namespace ykmWeb.Areas.management.Controllers
{
    public class nclassController : Controller
    {
        // GET: yljmanager/nclass
        [webAuthorzize]
        public ActionResult showchild()
        {
            string tn = Request.QueryString["tn"];
            string pid = Request.QueryString["pid"];
            string choose = Request.QueryString["choo"];
            string lang= Request.QueryString["lang"];
            string tableType = Request.QueryString["tableType"];

            string t = tn;
            do_class_view dc = new do_class_view();
            if (choose == "1")
            {
                Response.Write(dc.list_choose_div("n", pid, lang,tableType));
            }
            else
            {
                Response.Write(dc.do_view("n", pid,lang));
            }

            return null;
        }

        [webAuthorzize]
        public ActionResult user_showchild()
        {
            string tn = Request.QueryString["tn"];
            string pid = Request.QueryString["pid"];
            string choose = Request.QueryString["choo"];
            string lang = Request.QueryString["lang"];
            string tableType = Request.QueryString["tableType"];

            string t = tn;
            do_class_view dc = new do_class_view();
            if (choose == "1")
            {
                Response.Write(dc.list_choose_div("n", pid, lang, tableType));
            }
            else
            {
                Response.Write(dc.user_do_view("n", pid, lang));
            }

            return null;
        }

        [webAuthorzize]
        public string del()
        {
            string pid = Request.QueryString["pid"];
            string cid = Request.QueryString["cid"];
            string tn = Request.QueryString["tn"];

            nclassdo nc = null;
            if (pid == "0")
            {
                nc = new nclass_do_del_root(tn, cid);

            }
            else
            {
                nc = new nclass_do_del(tn, cid);
            }
            int state = nc.do_action();
            if (state == 1)
            {
                return "1";
            }
            else
            {
                return "2";
            }
        }

        [webAuthorzize]
        [HttpPost]
        public string Dosort(string tn, string pid, string cid, string s)
        {
            nclassdo nc = null;
            if (pid == "0")
            {
                if (s == "t")
                {
                    nc = new nclass_oneup(tn, cid, "1");
                }
                else if (s == "d")
                {
                    nc = new nclass_onedown(tn, "1", cid);

                }
            }
            else
            {
                if (s == "t")
                {
                    nc = new nclass_nup(tn, "1", cid);
                }
                else if (s == "d")
                {
                    nc = new nclass_ndown(tn, "1", cid);

                }
            }
            return nc.do_action().ToString();
        }



    }
}