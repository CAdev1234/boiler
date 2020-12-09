using ykmWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;
using ykmWeb.Bll;

namespace ykmWeb.sysHtml
{
    public class mobiHtml
    {
        /// <summary>
        /// 手机轮播图广告位_可通用
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static string getBanner(List<ggw> l)
        {
            StringBuilder sb = new StringBuilder();
            if(l.Count() > 0)
            {
                sb.Append("<div id=\"focus\"class=\"focus\"><div class=\"hd\"><ul></ul></div><div class=\"bd\"><ul>");  
                foreach (var o in l)
                {
                    string link = "javascript:void(null)";
                    if (!string.IsNullOrEmpty(o.ggwlink))
                    {
                        link = o.ggwlink;
                    }
                    sb.Append("<li><a href=\""+ link + "\"><img src=\""+o.imgurl+"\" /></a></li>");
                }
                sb.Append("</ul></div></div>");
            }

            sb.Append("<script>\r\n");
            sb.Append("    TouchSlide({\r\n");
            sb.Append("        slideCell: \"#focus\",\r\n");
            sb.Append("        titCell: \".hd ul\",\r\n");
            sb.Append("        //开启自动分页 autoPage:true ，此时设置 titCell 为导航元素包裹层\r\n");
            sb.Append("        mainCell: \".bd ul\",\r\n");
            sb.Append("        effect: \"left\",\r\n");
            sb.Append("        autoPlay: true,\r\n");
            sb.Append("        //自动播放\r\n");
            sb.Append("        autoPage: true,\r\n");
            sb.Append("        //自动分页\r\n");
            sb.Append("        switchLoad: \"_src\" //切换加载，真实图片路径为\"_src\"\r\n");
            sb.Append("    });\r\n");
            sb.Append("</script>\r\n");
            return sb.ToString();
        }

        /// <summary>
        /// 单个输出广告位图片和连接 可通用
        /// </summary>
        /// <param name="o">广告位类型</param>
        /// <returns></returns>
        public static string getGGwImage(ggw o)
        {
            StringBuilder sb = new StringBuilder();
            if (o != null)
            {
                string link = "javascript:void(null)";
                if (!string.IsNullOrEmpty(o.ggwlink))
                {
                    link = o.ggwlink;
                }
                sb.Append("<a href=\"" + link + "\"><img src=\"" + o.imgurl + "\" /></a>");
            }
            return sb.ToString();
        }

        public static string fghtml(Dictionary<string,string> l)
        {
            StringBuilder sb = new StringBuilder();
            if(l.Count() > 0)
            {
                foreach(var o in l)
                {
                    sb.Append("<li ><a href=\"/h5/an?f="+o.Key+"\">"+o.Value+"</a></li>");
                }
            }
            return sb.ToString();
        }

        private static string getStateStr(string fg,string val) {
            ykmWeb.Bll.user_config_type uct = new Bll.user_config_type(fg);
            return uct.get_text(val);
        }

        public static string listGGW(List<ggw> l)
        {
            StringBuilder sb = new StringBuilder();
            if(l.Count() > 0)
            {
                sb.Append("<ul>");
                foreach( var o in l)
                {
                    string link = "javascript:void(null)";
                    if (!string.IsNullOrEmpty(o.ggwlink))
                    {
                        link = o.ggwlink;
                    }
                    sb.Append("<li><a href=\""+ link + "\"><img src=\""+o.imgurl+"\" alt=\""+o.title+"\"></a></li>");
                }
          
                sb.Append("</ul>");
            }
            return sb.ToString();
        }


        #region 首页产品中心
        public static string getProCenter()
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                do_class_view dcv = new do_class_view();
                var c = dmc.find(n => n.Caenname == "cpzx");
                if (c != null)
                {
                    sb.Append("<div class=\"pro\">");
                    sb.Append("    <div class=\"title\">" + c.Catalogname + "</div>");
                    sb.Append("    <div class=\"entitle\">" + c.subtitle + "</div>");
                    sb.Append("    <ul>");
                    var arr = dcv.showallclassid("cpzx");
                    var l = di.FindList(n => arr.Contains(n.classid.Value), 6, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    if (l.Count > 0)
                    {
                        foreach (var i in l)
                        {
                            sb.Append("        <li>");
                            sb.Append("            <a href=\"" + nl.getMobiContLink(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id + "\">");
                            sb.Append("                <div class=\"slt\"><img src=\"" + i.defaultpic + "\"></div>");
                            sb.Append("                <div class=\"tit\">" + i.title + "</div>");
                            sb.Append("                <div class=\"price\">￥" + common.common.getMoneyType(double.Parse(common.common.IsNumeric_n(i.price))) + "</div>");
                            sb.Append("            </a>");
                            sb.Append("        </li>");
                        }
                    }
                    sb.Append("    </ul>");
                    sb.Append("    <div class=\"more\"><a href=\"" + nl.getMobiUrlLink(c) + "\">查看全部</a></div>");
                    sb.Append("</div>");
                }
            }
            return sb.ToString();
        }
        #endregion
        #region 首页公司动态
        public static string getComNews()
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                var c = dmc.find(n => n.Caenname == "gsdt");
                if (c != null)
                {
                    sb.Append("<div class=\"gsdt\">");
                    sb.Append("    <div class=\"title\">" + c.Catalogname + "</div>");
                    sb.Append("    <div class=\"entitle\">" + c.subtitle + "</div>");
                    sb.Append("    <ul>");
                    var li = di.FindList(n => n.classid == c.Catalogid, 3, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    if (li.Count > 0)
                    {
                        string href = "";
                        foreach (var i in li)
                        {
                            href = nl.getMobiContLink(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id;
                            sb.Append("        <li>");
                            sb.Append("            <a href=\"" + href + "\">");
                            sb.Append("                <div class=\"nr_box\">");
                            sb.Append("                    <div class=\"slt\"><img src=\""+ i.defaultpic + "\"></div>");
                            sb.Append("                    <div class=\"wenzi\">");
                            sb.Append("                        <div class=\"tit\">" + i.title + "</div>");
                            sb.Append("                        <div class=\"txt\">" + common.common.DelHTML(i.intro) + "</div>");
                            sb.Append("                        <div class=\"date\">" + i.insertdate.Value.Year + "/" + common.common.get_format_for_nums(i.insertdate.Value.Month) + "/" + common.common.get_format_for_nums(i.insertdate.Value.Day) + "</div>");
                            sb.Append("                    </div>");
                            sb.Append("                </div>");
                            sb.Append("            </a>");
                            sb.Append("        </li>");
                        }
                    }
                    sb.Append("    </ul>");
                    sb.Append("    <div class=\"more\"><a href=\"" + nl.getMobiUrlLink(c) + "\">查看全部</a></div>");
                    sb.Append("</div>");
                }
            }
            return sb.ToString();
        }
        #endregion
        #region 首页走进亨恒
        public static string getAboutHH()
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                DalGgw dg = new DalGgw(s);
                var c = dmc.find(n => n.Caenname == "zjhh");
                if (c != null)
                {
                    sb.Append("<div class=\"zjhh_box\">");
                    sb.Append("    <div class=\"zjhh\">");
                    sb.Append("        <div class=\"title\">" + c.Catalogname + "</div>");
                    sb.Append("        <div class=\"entitle\">" + c.subtitle + "</div>");
                    sb.Append("        <div class=\"txt_box\">");
                    var ccggw = dg.find(n => n.ggwposition == "sjzjhh");
                    if(ccggw != null)
                    {
                        sb.Append("            <div class=\"slt\"><img src=\"" + ccggw.imgurl + "\"></div>");
                    }
                    sb.Append("            <div class=\"txt\">");
                    var cc = dmc.find(n => n.Caenname == "gsjj");
                    var ccinfo = di.find(n => n.classid == cc.Catalogid);
                    if(ccinfo != null)
                    {
                        sb.Append(common.common.DelHTML(ccinfo.intro));
                    }
                    sb.Append("            </div>");
                    sb.Append("            <div class=\"btn_box\">");
                    if (c.Child > 0)
                    {
                        var lc = dmc.FindList(n => n.ParentID == c.Catalogid && n.Caenname != "gsjj", 0, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList();
                        foreach(var ic in lc)
                        {
                            sb.Append("<a href=\"" + nl.getMobiUrlLink(ic) + "\">" + ic.Catalogname + "</a>");
                        }
                    }
                    sb.Append("            </div>");
                    sb.Append("        </div>");
                    sb.Append("    </div>");
                    sb.Append("</div>");
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 英文版首页产品中心
        public static string getProCenter_en()
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                do_class_view dcv = new do_class_view();
                var c = dmc.find(n => n.Caenname == "products");
                if (c != null)
                {
                    sb.Append("<div class=\"pro\">");
                    sb.Append("    <div class=\"title\">" + c.Catalogname + "</div>");
                    sb.Append("    <div class=\"entitle\">" + c.subtitle + "</div>");
                    sb.Append("    <ul>");
                    var arr = dcv.showallclassid("products");
                    var l = di.FindList(n => arr.Contains(n.classid.Value), 6, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    if (l.Count > 0)
                    {
                        foreach (var i in l)
                        {
                            sb.Append("        <li>");
                            sb.Append("            <a href=\"" + nl.getMobiContLink_en(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id + "\">");
                            sb.Append("                <div class=\"slt\"><img src=\"" + i.defaultpic + "\"></div>");
                            sb.Append("                <div class=\"tit\">" + i.title + "</div>");
                            sb.Append("                <div class=\"price\">￥" + common.common.getMoneyType(double.Parse(common.common.IsNumeric_n(i.price))) + "</div>");
                            sb.Append("            </a>");
                            sb.Append("        </li>");
                        }
                    }
                    sb.Append("    </ul>");
                    sb.Append("    <div class=\"more\"><a href=\"" + nl.getMobiUrlLink_en(c) + "\">View all</a></div>");
                    sb.Append("</div>");
                }
            }
            return sb.ToString();
        }
        #endregion
        #region 英文版首页公司动态
        public static string getComNews_en()
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                var c = dmc.find(n => n.Caenname == "companydynamics");
                if (c != null)
                {
                    sb.Append("<div class=\"gsdt\">");
                    sb.Append("    <div class=\"title\">" + c.Catalogname + "</div>");
                    sb.Append("    <div class=\"entitle\">" + c.subtitle + "</div>");
                    sb.Append("    <ul>");
                    var li = di.FindList(n => n.classid == c.Catalogid, 3, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    if (li.Count > 0)
                    {
                        string href = "";
                        foreach (var i in li)
                        {
                            href = nl.getMobiContLink_en(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id;
                            sb.Append("        <li>");
                            sb.Append("            <a href=\"" + href + "\">");
                            sb.Append("                <div class=\"nr_box\">");
                            sb.Append("                    <div class=\"slt\"><img src=\""+i.defaultpic+"\"></div>");
                            sb.Append("                    <div class=\"wenzi\">");
                            sb.Append("                        <div class=\"tit\">" + i.title + "</div>");
                            sb.Append("                        <div class=\"txt\">" + common.common.DelHTML(i.intro) + "</div>");
                            sb.Append("                        <div class=\"date\">" + i.insertdate.Value.Year + "/" + common.common.get_format_for_nums(i.insertdate.Value.Month) + "/" + common.common.get_format_for_nums(i.insertdate.Value.Day) + "</div>");
                            sb.Append("                    </div>");
                            sb.Append("                </div>");
                            sb.Append("            </a>");
                            sb.Append("        </li>");
                        }
                    }
                    sb.Append("    </ul>");
                    sb.Append("    <div class=\"more\"><a href=\"" + nl.getMobiUrlLink_en(c) + "\">View all</a></div>");
                    sb.Append("</div>");
                }
            }
            return sb.ToString();
        }
        #endregion
        #region 首页走进亨恒
        public static string getAboutHH_en()
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                DalGgw dg = new DalGgw(s);
                var c = dmc.find(n => n.Caenname == "aboutus");
                if (c != null)
                {
                    sb.Append("<div class=\"zjhh_box\">");
                    sb.Append("    <div class=\"zjhh\">");
                    sb.Append("        <div class=\"title\">" + c.Catalogname + "</div>");
                    sb.Append("        <div class=\"entitle\">" + c.subtitle + "</div>");
                    sb.Append("        <div class=\"txt_box\">");
                    var ccggw = dg.find(n => n.ggwposition == "sjzjhh_en");
                    if (ccggw != null)
                    {
                        sb.Append("            <div class=\"slt\"><img src=\"" + ccggw.imgurl + "\"></div>");
                    }
                    sb.Append("            <div class=\"txt\">");
                    var cc = dmc.find(n => n.Caenname == "companyprofile");
                    var ccinfo = di.find(n => n.classid == cc.Catalogid);
                    if (ccinfo != null)
                    {
                        sb.Append(common.common.DelHTML(ccinfo.intro));
                    }
                    sb.Append("            </div>");
                    sb.Append("            <div class=\"btn_box\">");
                    if (c.Child > 0)
                    {
                        var lc = dmc.FindList(n => n.ParentID == c.Catalogid && n.Caenname != "companyprofile", 0, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList();
                        foreach (var ic in lc)
                        {
                            sb.Append("<a href=\"" + nl.getMobiUrlLink_en(ic) + "\">" + ic.Catalogname + "</a>");
                        }
                    }
                    sb.Append("            </div>");
                    sb.Append("        </div>");
                    sb.Append("    </div>");
                    sb.Append("</div>");
                }
            }
            return sb.ToString();
        }
        #endregion



        #region 产品列表
        public static string pro_list(List<view_info> l)
        {
            //<div class="pro_list">
            //    <ul>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="images/pro_05.jpg"></div>
            //                <div class="tit">产品名称产品名称</div>
            //                <div class="price">￥120.00</div>
            //            </a>
            //        </li>
            //    </ul>
            //</div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    sb.Append("<div class=\"pro_list\"><ul id=\"addlist\">");
                    foreach (var o in l)
                    {
                        href = nl.getMobiContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("        <li>");
                        sb.Append("            <a href=\"" + href + "\">");
                        sb.Append("                <div class=\"slt\"><img src=\"" + o.defaultpic + "\"></div>");
                        sb.Append("                <div class=\"tit\">" + o.title + "</div>");
                        sb.Append("                <div class=\"price\">￥" + common.common.getMoneyType(double.Parse(common.common.IsNumeric_n(o.price))) + "</div>");
                        sb.Append("            </a>");
                        sb.Append("        </li>");
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        public static string add_pro_list(List<view_info> l)
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    foreach (var o in l)
                    {
                        href = nl.getMobiContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("        <li>");
                        sb.Append("            <a href=\"" + href + "\">");
                        sb.Append("                <div class=\"slt\"><img src=\"" + o.defaultpic + "\"></div>");
                        sb.Append("                <div class=\"tit\">" + o.title + "</div>");
                        sb.Append("                <div class=\"price\">￥" + common.common.getMoneyType(double.Parse(common.common.IsNumeric_n(o.price))) + "</div>");
                        sb.Append("            </a>");
                        sb.Append("        </li>");
                    }
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 英文产品列表
        public static string pro_list_en(List<view_info> l)
        {
            //<div class="pro_list">
            //    <ul>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="images/pro_05.jpg"></div>
            //                <div class="tit">产品名称产品名称</div>
            //                <div class="price">￥120.00</div>
            //            </a>
            //        </li>
            //    </ul>
            //</div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    sb.Append("<div class=\"pro_list\"><ul id=\"addlist\">");
                    foreach (var o in l)
                    {
                        href = nl.getMobiContLink_en(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("        <li>");
                        sb.Append("            <a href=\"" + href + "\">");
                        sb.Append("                <div class=\"slt\"><img src=\"" + o.defaultpic + "\"></div>");
                        sb.Append("                <div class=\"tit\">" + o.title + "</div>");
                        sb.Append("                <div class=\"price\">￥" + common.common.getMoneyType(double.Parse(common.common.IsNumeric_n(o.price))) + "</div>");
                        sb.Append("            </a>");
                        sb.Append("        </li>");
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        public static string add_pro_list_en(List<view_info> l)
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    foreach (var o in l)
                    {
                        href = nl.getMobiContLink_en(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("        <li>");
                        sb.Append("            <a href=\"" + href + "\">");
                        sb.Append("                <div class=\"slt\"><img src=\"" + o.defaultpic + "\"></div>");
                        sb.Append("                <div class=\"tit\">" + o.title + "</div>");
                        sb.Append("                <div class=\"price\">￥" + common.common.getMoneyType(double.Parse(common.common.IsNumeric_n(o.price))) + "</div>");
                        sb.Append("            </a>");
                        sb.Append("        </li>");
                    }
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 新闻列表
        public static string news_list(List<view_info> l)
        {
            //<div class="news">
            //    <ul>
            //        <li>
            //            <a href="#">
            //                <div class="nr_box">
            //                    <div class="slt"><img src="images/gsdt_35.jpg"></div>
            //                    <div class="wenzi">
            //                        <div class="tit">丹东亨恒贸易有限公司位于秀美的鸭绿</div>
            //                        <div class="txt">丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。</div>
            //                        <div class="date">2020/3/13</div>
            //                    </div>
            //                </div>
            //            </a>
            //        </li>
            //    </ul>
            //</div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    sb.Append("<div class=\"news\"><ul id=\"addlist\">");
                    foreach (var o in l)
                    {
                        href = nl.getMobiContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("        <li>");
                        sb.Append("            <a href=\"" + href + "\">");
                        sb.Append("                <div class=\"nr_box\">");
                        sb.Append("                    <div class=\"slt\"><img src=\"" + o.defaultpic + "\"></div>");
                        sb.Append("                    <div class=\"wenzi\">");
                        sb.Append("                        <div class=\"tit\">" + o.title + "</div>");
                        sb.Append("                        <div class=\"txt\">" + common.common.DelHTML(o.intro) + "</div>");
                        sb.Append("                        <div class=\"date\">" + o.insertdate.Value.Year + "/" + common.common.get_format_for_nums(o.insertdate.Value.Month) + "/" + common.common.get_format_for_nums(o.insertdate.Value.Day) + "</div>");
                        sb.Append("                    </div>");
                        sb.Append("                </div>");
                        sb.Append("            </a>");
                        sb.Append("        </li>");
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        public static string add_news_list(List<view_info> l)
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    foreach (var o in l)
                    {
                        href = nl.getMobiContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("        <li>");
                        sb.Append("            <a href=\"" + href + "\">");
                        sb.Append("                <div class=\"nr_box\">");
                        sb.Append("                    <div class=\"slt\"><img src=\"" + o.defaultpic + "\"></div>");
                        sb.Append("                    <div class=\"wenzi\">");
                        sb.Append("                        <div class=\"tit\">" + o.title + "</div>");
                        sb.Append("                        <div class=\"txt\">" + common.common.DelHTML(o.intro) + "</div>");
                        sb.Append("                        <div class=\"date\">" + o.insertdate.Value.Year + "/" + common.common.get_format_for_nums(o.insertdate.Value.Month) + "/" + common.common.get_format_for_nums(o.insertdate.Value.Day) + "</div>");
                        sb.Append("                    </div>");
                        sb.Append("                </div>");
                        sb.Append("            </a>");
                        sb.Append("        </li>");
                    }
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 英文新闻列表
        public static string news_list_en(List<view_info> l)
        {
            //<div class="news">
            //    <ul>
            //        <li>
            //            <a href="#">
            //                <div class="nr_box">
            //                    <div class="slt"><img src="images/gsdt_35.jpg"></div>
            //                    <div class="wenzi">
            //                        <div class="tit">丹东亨恒贸易有限公司位于秀美的鸭绿</div>
            //                        <div class="txt">丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。</div>
            //                        <div class="date">2020/3/13</div>
            //                    </div>
            //                </div>
            //            </a>
            //        </li>
            //    </ul>
            //</div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    sb.Append("<div class=\"news\"><ul id=\"addlist\">");
                    foreach (var o in l)
                    {
                        href = nl.getMobiContLink_en(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("        <li>");
                        sb.Append("            <a href=\"" + href + "\">");
                        sb.Append("                <div class=\"nr_box\">");
                        sb.Append("                    <div class=\"slt\"><img src=\"" + o.defaultpic + "\"></div>");
                        sb.Append("                    <div class=\"wenzi\">");
                        sb.Append("                        <div class=\"tit\">" + o.title + "</div>");
                        sb.Append("                        <div class=\"txt\">" + common.common.DelHTML(o.intro) + "</div>");
                        sb.Append("                        <div class=\"date\">" + o.insertdate.Value.Year + "/" + common.common.get_format_for_nums(o.insertdate.Value.Month) + "/" + common.common.get_format_for_nums(o.insertdate.Value.Day) + "</div>");
                        sb.Append("                    </div>");
                        sb.Append("                </div>");
                        sb.Append("            </a>");
                        sb.Append("        </li>");
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        public static string add_news_list_en(List<view_info> l)
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    foreach (var o in l)
                    {
                        href = nl.getMobiContLink_en(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("        <li>");
                        sb.Append("            <a href=\"" + href + "\">");
                        sb.Append("                <div class=\"nr_box\">");
                        sb.Append("                    <div class=\"slt\"><img src=\"" + o.defaultpic + "\"></div>");
                        sb.Append("                    <div class=\"wenzi\">");
                        sb.Append("                        <div class=\"tit\">" + o.title + "</div>");
                        sb.Append("                        <div class=\"txt\">" + common.common.DelHTML(o.intro) + "</div>");
                        sb.Append("                        <div class=\"date\">" + o.insertdate.Value.Year + "/" + common.common.get_format_for_nums(o.insertdate.Value.Month) + "/" + common.common.get_format_for_nums(o.insertdate.Value.Day) + "</div>");
                        sb.Append("                    </div>");
                        sb.Append("                </div>");
                        sb.Append("            </a>");
                        sb.Append("        </li>");
                    }
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 留言板
        public static string message_borad()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script src=\"/layer/layer.js\" type=\"text/javascript\"></script> \r\n");
            sb.Append("<script src=\"/web_js/savebook.js\" type=\"text/javascript\"></script> \r\n");
            sb.Append("<div class=\"zxly\"> \r\n");
            sb.Append(" <form name=\"form1\" id=\"form1\"> \r\n");
            sb.Append("    <div class=\"li_box\"> \r\n");
            sb.Append("        <div class=\"tit\">您的姓名：</div> \r\n");
            sb.Append("        <input id=\"name\" name=\"name\" type=\"text\"> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append("    <div class=\"li_box\"> \r\n");
            sb.Append("        <div class=\"tit\">您的性别：</div> \r\n");
            sb.Append("        <input id=\"sex\" name=\"sex\" type=\"text\"> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append("    <div class=\"li_box\"> \r\n");
            sb.Append("        <div class=\"tit\">联系电话：</div> \r\n");
            sb.Append("        <input id=\"tel\" name=\"tel\" type=\"text\"> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append("    <div class=\"li_box\"> \r\n");
            sb.Append("        <div class=\"tit\">电子邮箱：</div> \r\n");
            sb.Append("        <input id=\"email\" name=\"email\" type=\"text\"> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append("    <div class=\"li_box_big\"> \r\n");
            sb.Append("        <div class=\"tit\">留言内容：</div> \r\n");
            sb.Append("        <textarea id=\"cont\" name=\"cont\" type=\"text\"></textarea> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append("    <div class=\"btn\"> \r\n");
            sb.Append("        <a class=\"tj\" href=\"javascript:;\" onclick=\"submit()\">提交留言</a> \r\n");
            sb.Append("        <a class=\"qx\" href=\"javascript:;\" onclick=\"reset()\">信息重置</a> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append(" </form> \r\n");
            sb.Append("</div> \r\n");
            return sb.ToString();
        }
        #endregion


        #region 英文版留言板
        public static string message_borad_en()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script src=\"/layer/layer.js\" type=\"text/javascript\"></script> \r\n");
            sb.Append("<script src=\"/web_js/savebook_h5_en.js\" type=\"text/javascript\"></script> \r\n");
            sb.Append("<div class=\"zxly\"> \r\n");
            sb.Append(" <form name=\"form1\" id=\"form1\"> \r\n");
            sb.Append("    <div class=\"li_box\"> \r\n");
            sb.Append("        <div class=\"tit\">Your name：</div> \r\n");
            sb.Append("        <input id=\"name\" name=\"name\" type=\"text\"> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append("    <div class=\"li_box\"> \r\n");
            sb.Append("        <div class=\"tit\">Your gender：</div> \r\n");
            sb.Append("        <input id=\"sex\" name=\"sex\" type=\"text\"> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append("    <div class=\"li_box\"> \r\n");
            sb.Append("        <div class=\"tit\">Message phone：</div> \r\n");
            sb.Append("        <input id=\"tel\" name=\"tel\" type=\"text\"> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append("    <div class=\"li_box\"> \r\n");
            sb.Append("        <div class=\"tit\">Message mailbox：</div> \r\n");
            sb.Append("        <input id=\"email\" name=\"email\" type=\"text\"> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append("    <div class=\"li_box_big\"> \r\n");
            sb.Append("        <div class=\"tit\">Content：</div> \r\n");
            sb.Append("        <textarea id=\"cont\" name=\"cont\" type=\"text\"></textarea> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append("    <div class=\"btn\"> \r\n");
            sb.Append("        <a class=\"tj\" href=\"javascript:;\" onclick=\"submit()\">Submit message</a> \r\n");
            sb.Append("        <a class=\"qx\" href=\"javascript:;\" onclick=\"reset()\">Message reset</a> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append(" </form> \r\n");
            sb.Append("</div> \r\n");
            return sb.ToString();
        }
        #endregion

        #region 分类内容页
        public static string info_content(List<view_info> l)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count > 0)
            {
                sb.Append("<div class=\"jianjie\"><div class=\"neirong infocontent\">");
                if (l[0].issame == 0)
                {
                    sb.Append(l[0].h5cont);
                }
                else
                {
                    sb.Append(l[0].cont);
                }
                sb.Append("</div></div>");
            }
            return sb.ToString();
        }
        #endregion

        #region 产品内容页
        public static string pro_content(List<view_info> l)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count > 0)
            {
                sb.Append("<div class=\"jianjie\">");
                sb.Append("    <div class=\"title\">");
                sb.Append("        <div class=\"biaoti\">" + l[0].title + "</div>");
                sb.Append("        <div class=\"price\">￥：<span>" + common.common.getMoneyType(double.Parse(common.common.IsNumeric_n(l[0].price))) + "</span></div>");
                sb.Append("    </div>");
                sb.Append("    <div class=\"txt infocontent\">");
                if (l[0].issame == 0)
                {
                    sb.Append(l[0].h5cont);
                }
                else
                {
                    sb.Append(l[0].cont);
                }
                sb.Append("    </div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion
        #region 新闻内容页
        public static string news_content(List<view_info> l)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count > 0)
            {
                sb.Append("<div class=\"jianjie\">");
                sb.Append("    <div class=\"title\">");
                sb.Append("        <div class=\"biaoti\">" + l[0].title + "</div>");
                sb.Append("        <div class=\"date\">发表日期：" + common.common.get_date_format_for_nums(l[0].insertdate.Value) + "</div>");
                sb.Append("    </div>");
                sb.Append("    <div class=\"txt infocontent\">");
                if (l[0].issame == 0)
                {
                    sb.Append(l[0].h5cont);
                }
                else
                {
                    sb.Append(l[0].cont);
                }
                sb.Append("    </div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion
        #region 英文版新闻内容页
        public static string news_content_en(List<view_info> l)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count > 0)
            {
                sb.Append("<div class=\"jianjie\">");
                sb.Append("    <div class=\"title\">");
                sb.Append("        <div class=\"biaoti\">" + l[0].title + "</div>");
                sb.Append("        <div class=\"date\">Date of publication：" + common.common.get_date_format_for_nums(l[0].insertdate.Value) + "</div>");
                sb.Append("    </div>");
                sb.Append("    <div class=\"txt infocontent\">");
                if (l[0].issame == 0)
                {
                    sb.Append(l[0].h5cont);
                }
                else
                {
                    sb.Append(l[0].cont);
                }
                sb.Append("    </div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion


        #region 内容页
        public static string get_cont_page(info o)
        {
            string Html = "";
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                if (o != null)
                {
                    var i = new view_info { classid = o.classid, cont = o.cont, defaultpic = o.defaultpic, h5cont = o.h5cont, id = o.id, insertdate = o.insertdate, intro = o.intro, issame = o.issame, price = o.price, title = o.title };
                    var c = dmc.find(n => n.Catalogid == o.classid);
                    if (c != null)
                    {
                        switch (c.pclisttype)
                        {
                            case "pro-list"://产品列表
                                Html = pro_content(new List<view_info> { i });
                                break;
                            case "news-list"://新闻列表
                                Html = news_content(new List<view_info> { i });
                                break;
                            case "cont"://内容
                                Html = info_content(new List<view_info> { i });
                                break;
                        }
                    }
                }
            }
            return Html;
        }
        #endregion

        #region 英文版内容页
        public static string get_cont_page_en(info o)
        {
            string Html = "";
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                if (o != null)
                {
                    var i = new view_info { classid = o.classid, cont = o.cont, defaultpic = o.defaultpic, h5cont = o.h5cont, id = o.id, insertdate = o.insertdate, intro = o.intro, issame = o.issame, price = o.price, title = o.title };
                    var c = dmc.find(n => n.Catalogid == o.classid);
                    if (c != null)
                    {
                        switch (c.pclisttype)
                        {
                            case "pro-list"://产品列表
                                Html = pro_content(new List<view_info> { i });
                                break;
                            case "news-list"://新闻列表
                                Html = news_content_en(new List<view_info> { i });
                                break;
                            case "cont"://内容
                                Html = info_content(new List<view_info> { i });
                                break;
                        }
                    }
                }
            }
            return Html;
        }
        #endregion

    }
}