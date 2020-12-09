using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ykmWeb.Bll;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;

namespace ykmWeb.Areas.management.Controllers
{
    public class statisticsController : Controller
    {
        ykmWebDbContext s = new ykmWebDbContext();
        // GET: management/statistics
        public ActionResult Index(string t = "d")
        {
            do_visitor dv = new do_visitor();
            DalMenuClass dmc = new DalMenuClass(s);
            DalInfo di = new DalInfo(s);
            DalGuestBook dgb = new DalGuestBook(s);
            //产品中心
            var pro = dmc.FindList(n => n.pclisttype.IndexOf("pro") != -1, 0, null).Select(g => g.Catalogid).ToList();
            do_class_view dcv = new do_class_view();
            List<int> arrid = new List<int>();
            var c = dmc.find(n => n.Caenname == "cpzs");
            arrid = dcv.showallclassid(c.Caenname);
            var pro_sj = di.FindList(n => arrid.Contains(n.classid.Value), 1, new OrderModelField[] { new OrderModelField { propertyName = "insertdate", IsDESC = true } }).FirstOrDefault();

            ViewBag.Sum1 = di.count(n => pro.Contains(n.classid));
            if (pro_sj!=null) { 
            ViewBag.pro_sj = pro_sj.insertdate.Value.Year + "-" + pro_sj.insertdate.Value.Month + "-" + pro_sj.insertdate.Value.Day;
            }
            //公司动态
            //var news = dmc.FindList(n => n.pclisttype.IndexOf("news") != -1, 0, null).Select(g => g.Catalogid).ToList();
            List<int> arrid_news = new List<int>();
            var xinwen = dmc.find(n => n.Caenname == "xwzx");
            arrid_news = dcv.showallclassid(xinwen.Caenname);
            var news_sj = di.FindList(n => arrid_news.Contains(n.classid.Value), 1, new OrderModelField[] { new OrderModelField { propertyName = "insertdate", IsDESC = true } }).FirstOrDefault();

            var news = di.FindList(n => arrid_news.Contains(n.classid.Value), 0, new OrderModelField[] { new OrderModelField { propertyName = "insertdate", IsDESC = true } }).ToList();

            ViewBag.Sum2 = news.Count;
            if (news_sj != null)
            {
                ViewBag.news_sj = news_sj.insertdate.Value.Year + "-" + news_sj.insertdate.Value.Month + "-" + news_sj.insertdate.Value.Day;
            }
            //在线留言
            var liuyan = dgb.FindList(n => true, 1, new OrderModelField[] { new OrderModelField { propertyName = "insertdate", IsDESC = true } }).FirstOrDefault();

            ViewBag.Sum3 = dgb.count(n => true);
            if (liuyan != null)
            {
                ViewBag.liuyan_sj = liuyan.insertdate.Value.Year + "-" + liuyan.insertdate.Value.Month + "-" + liuyan.insertdate.Value.Day;
            }
            //网站总访问量
            ViewBag.Sum4 = dv.getVisitorTotal();


            ViewBag.t = t;

            DateTime dt = DateTime.Now;
            int year = dt.Year;
            int month = dt.Month;
            int day = dt.Day;

            switch (t)
            {
                case "y":
                    ViewBag.showNum = 1;
                    ViewBag.showType = "year";
                    ViewBag.Data1 = getDate1("y");
                    break;
                case "m":
                    ViewBag.showNum = 1;
                    ViewBag.showType = "month";
                    ViewBag.Data1 = getDate1("m");
                    break;
                case "d":
                    ViewBag.showNum = 3;
                    ViewBag.showType = "day";
                    ViewBag.Data1 = getDate1("d");
                    break;
                default:
                    ViewBag.showNum = 3;
                    ViewBag.showType = "day";
                    ViewBag.Data1 = getDate1();
                    break;
            }

            //dop.FindList()
            return View();
        }

        public string getDate1(string t = "d")
        {
            StringBuilder sb = new StringBuilder();
            int times = 0;//DateTime.DaysInMonth(Year, Month)
            DateTime dt = DateTime.Now;
            int year = dt.Year;
            int month = dt.Month;
            DalVisitor dop = new DalVisitor(s);
            switch (t)
            {
                case "y":
                    for (int i = (year - 9); i <= year; i++)
                    {
                        int num = dop.count(n => n.insertdate.Value.Year == i);
                        sb.Append(string.Format("[gd({0},1,1),{1}],", i, num));
                    }
                    break;
                case "m":
                    times = 12;
                    for (int i = 1; i <= times; i++)
                    {
                        int num = dop.count(n => n.insertdate.Value.Year == year && n.insertdate.Value.Month == i);
                        sb.Append(string.Format("[gd({0},{1},1),{2}],", year, i, num));
                    }
                    break;
                case "d":
                    times = DateTime.DaysInMonth(year, month);
                    for (var i = 1; i <= times; i++)
                    {
                        int num = dop.count(n => n.insertdate.Value.Year == year && n.insertdate.Value.Month == month && n.insertdate.Value.Day == i);
                        sb.Append(string.Format("[gd({0},{1},{2}),{3}],", year, month, i, num));
                    }
                    break;
            }
            return sb.ToString();
        }
        public string getDate2(string t = "d")
        {
            StringBuilder sb = new StringBuilder();
            int times = 0;//DateTime.DaysInMonth(Year, Month)
            DateTime dt = DateTime.Now;
            int year = dt.Year;
            int month = dt.Month;
            //DalOpus dop = new DalOpus(s);
            //switch (t)
            //{
            //    case "y":
            //        //var y = s.db_Opus.GroupBy(n => n.infodate.Value.Year).Select(g => new { year = g.Select(a => a.infodate.Value.Year).Distinct(), num = g.Count() }).ToList();
            //        for (int i = (year - 9); i <= year; i++)
            //        {
            //            int num = dop.count(n => n.infodate.Value.Year == i);
            //            sb.Append(string.Format("[gd({0},1,1),{1}],", i, num));
            //        }
            //        break;
            //    case "m":
            //        //var m = s.db_Opus.Where(n => n.infodate.Value.Year == dt.Year).GroupBy(n => n.infodate.Value.Month).Select(g => new { month = g.Select(a=>a.infodate.Value.Month).Distinct() ,num = g.Count() }).ToList();
            //        //res = JsonConvert.SerializeObject(q);
            //        times = 12;
            //        for (int i = 1; i <= times; i++)
            //        {
            //            int num = dop.count(n => n.infodate.Value.Year == year && n.infodate.Value.Month == i);
            //            sb.Append(string.Format("[gd({0},{1},1),{2}],", year, i, num));
            //        }
            //        break;
            //    case "d":
            //        //var d = s.db_Opus.Where(n => n.infodate.Value.Year == dt.Year && n.infodate.Value.Month == dt.Month).GroupBy(n => n.infodate.Value.Day).Select(g => new { month = g.Select(a => a.infodate.Value.Day).Distinct(), num = g.Count() }).ToList();
            //        times = DateTime.DaysInMonth(year, month);
            //        for (var i = 1; i <= times; i++)
            //        {
            //            int num = dop.count(n => n.infodate.Value.Year == year && n.infodate.Value.Month == month && n.infodate.Value.Day == i);
            //            sb.Append(string.Format("[gd({0},{1},{2}),{3}],", year, month, i, num));
            //        }
            //        break;
            //}
            return sb.ToString();
        }

        public int getLvNum(int today, int prev, out string zf)
        {
            int res = 0;
            zf = "";
            if (prev != 0)
            {
                var lv = ((today - prev) / prev) * 100;
                if (lv < 0)
                {
                    zf = "down";
                }
                else if (lv > 0)
                {
                    zf = "up";
                }
                res = lv;
            }
            return res;
        }
    }
}