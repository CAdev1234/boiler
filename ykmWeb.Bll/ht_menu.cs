
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using System.Web;
using Newtonsoft.Json;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;
using ykmWeb.Models;

namespace ykmWeb.Bll
{
    public class ht_menu
    {

        // string str = "[{\"id\":\"1\",\"mname\":\"信息管理\",\"url\":\"/yljmanager/webmaster/defaultmain\",\"ico\":\"glyphicon-list\",\"child\":[{\"id\":\"1_3\",\"mname\":\"信息管理\",\"url\":\"/yljmanager/webmaster/defaultmain\",\"ico\":\"\"},{\"id\":\"1_1\",\"mname\":\"信息列表\",\"url\":\"/yljmanager/infolist/index\",\"ico\":\"\"},{\"id\":\"1_2\",\"mname\":\"栏目管理管理\",\"url\":\"/yljmanager/infoclass/index?t=infoclass\",\"ico\":\"\"}]},{\"id\":\"7\",\"mname\":\"会员管理\",\"url\":\"/yljmanager/userinfoper/index\",\"ico\":\"glyphicon-user\",\"child\":[{\"id\":\"7_1\",\"mname\":\"会员管理\",\"url\":\"/yljmanager/userinfoper/index\",\"ico\":\"\"},{\"id\":\"7_2\",\"mname\":\"会员等级管理\",\"url\":\"/yljmanager/userlevallist/index\",\"ico\":\"\"}]},{\"id\":\"2\",\"mname\":\"商城管理\",\"url\":\"/yljmanager/shop/splist\",\"ico\":\"glyphicon-shopping-cart\",\"child\":[{\"id\":\"2_1\",\"mname\":\"商品分类管理\",\"url\":\"/yljmanager/shopclass/index\",\"ico\":\"\"},{\"id\":\"2_12\",\"mname\":\"品牌管理\",\"url\":\"/yljmanager/brand/index\",\"ico\":\"\"},{\"id\":\"2_5\",\"mname\":\"商品管理\",\"url\":\"/yljmanager/shop/splist\",\"ico\":\"\"},{\"id\":\"2_6\",\"mname\":\"运费模版\",\"url\":\"/yljmanager/Pro_yf_template/index\",\"ico\":\"\"},{\"id\":\"2_7\",\"mname\":\"商城订单管理\",\"url\":\"/yljmanager/orderlist/index\",\"ico\":\"\"},{\"id\":\"2_13\",\"mname\":\"售后服务单\",\"url\":\"/yljmanager/orderservice/index\",\"ico\":\"\"},{\"id\":\"2_8\",\"mname\":\"优惠设置\",\"url\":\"/yljmanager/pro_yh/list\",\"ico\":\"\"},{\"id\":\"2_9\",\"mname\":\"商品评价\",\"url\":\"/yljmanager/evaluate/index\",\"ico\":\"\"},{\"id\":\"2_10\",\"mname\":\"商品单位\",\"url\":\"/yljmanager/dw/\",\"ico\":\"\"},{\"id\":\"2_15\",\"mname\":\"活动专区管理\",\"url\":\"/yljmanager/pro_zq_list/index\",\"ico\":\"\"}]},{\"id\":\"3\",\"mname\":\"广告位管理\",\"url\":\"/yljmanager/ggwlist/index\",\"ico\":\"glyphicon-bullhorn\",\"child\":[{\"id\":\"3_1\",\"mname\":\"广告位管理\",\"url\":\"/yljmanager/ggwlist/index\",\"ico\":\"\"}]},{\"id\":\"4\",\"mname\":\"应用设置\",\"url\":\"/yljmanager/siteconfig/index\",\"ico\":\"glyphicon-asterisk\",\"child\":[{\"id\":\"4_1\",\"mname\":\"联系方式\",\"url\":\"/yljmanager/siteconfig/index\",\"ico\":\"\"},{\"id\":\"4_2\",\"mname\":\"热门搜索设置\",\"url\":\"/yljmanager/searchHotList/index\",\"ico\":\"\"},{\"id\":\"4_7\",\"mname\":\"快递公司设置\",\"url\":\"/yljmanager/siteconfig/kd_code_list_100\",\"ico\":\"\"}]},{\"id\":\"5\",\"mname\":\"管理员管理\",\"url\":\"/yljmanager/webmaster/index\",\"ico\":\"glyphicon-lock\",\"child\":[{\"id\":\"5_1\",\"mname\":\"管理员列表\",\"url\":\"/yljmanager/webmaster/index\",\"ico\":\"\"},{\"id\":\"5_2\",\"mname\":\"修改密码\",\"url\":\"/yljmanager/webmaster/pass\",\"ico\":\"\"},{\"id\":\"5_3\",\"mname\":\"退出登录\",\"url\":\"/yljmanager/webmaster/layout\",\"ico\":\"\"}]},{\"id\":\"6\",\"mname\":\"统计\",\"url\":\"/yljmanager/Statistics/orderlist\",\"ico\":\"glyphicon-list-alt\",\"child\":[{\"id\":\"6_2\",\"mname\":\"订单统计\",\"url\":\"/yljmanager/Statistics/orderlist\",\"ico\":\"\"},{\"id\":\"6_3\",\"mname\":\"商品销量\",\"url\":\"/yljmanager/Statistics/cplist\",\"ico\":\"\"}]},{\"id\":\"13\",\"mname\":\"活动管理\",\"url\":\"/yljmanager/company_coupon/index\",\"ico\":\"glyphicon-yen\",\"child\":[{\"id\":\"13_5\",\"mname\":\"优惠券管理\",\"url\":\"/yljmanager/company_coupon/index\",\"ico\":\"\"},{\"id\":\"13_6\",\"mname\":\"会员优惠券领用记录\",\"url\":\"/yljmanager/company_coupon/user_company_coupon\",\"ico\":\"\"},{\"id\":\"13_1\",\"mname\":\"团购管理\",\"url\":\"/yljmanager/groupbuy/index\",\"ico\":\"\"},{\"id\":\"13_2\",\"mname\":\"秒杀管理\",\"url\":\"/yljmanager/mkillbuy/index\",\"ico\":\"\"}]}]";
        string str = "";
        public ht_menu()
        {
            str = File.ReadAllText(HttpContext.Current.Server.MapPath("/config/menu.json"),Encoding.UTF8);
        }

        class main
        {
            public string id { get; set; }
            public string mname { get; set; }
            public string url { get; set; }
            public string ico { get; set; }
            public List<main> child { get; set; }
            public string target { get; set; }
        }


        public string list_left_menu(string bqx, string mqx)
        {
            //<li>
            //    <a href="#">
            //        <i class="fa fa-home"></i>
            //        <span class="nav-label">主页</span>
            //        <span class="fa arrow"></span>
            //    </a>
            //    <ul class="nav nav-second-level">
            //        <li>
            //            <a class="J_menuItem" href="index_v1.html" data-index="0">主页示例一</a>
            //        </li>
            //        <li>
            //            <a class="J_menuItem" href="index_v2.html">主页示例二</a>
            //        </li>
            //        <li>
            //            <a class="J_menuItem" href="index_v3.html">主页示例三</a>
            //        </li>
            //        <li>
            //            <a class="J_menuItem" href="index_v4.html">主页示例四</a>
            //        </li>
            //        <li>
            //            <a href="index_v5.html" target="_blank">主页示例五</a>
            //        </li>
            //    </ul>
            //</li>
            StringBuilder sb = new StringBuilder();
            List<main> l = JsonConvert.DeserializeObject<List<main>>(str);
            if (string.IsNullOrEmpty(bqx) == false)
            {
                l = l.Where(n => bqx.Contains("," + n.id + ",")).ToList();
            }
            sb.Append("<li>");
            sb.Append("    <a class=\"J_menuItem\" href=\"/management/statistics/index\" data-index=\"0\">");
            sb.Append("        <i class=\"fa fa-home\"></i>");
            sb.Append("        <span class=\"nav-label\">主页</span>");
            sb.Append("    </a>");
            sb.Append("</li>");
            if (l.Count > 0)
            {
                string css = "", target = "";
                foreach (main m in l)
                {
                    if (m.ico == "fa-cubes") //文章分类
                    {
                        using (ykmWebDbContext s = new ykmWebDbContext())
                        {
                            DalMenuClass dmc = new DalMenuClass(s);
                            sb.Append(menu_class_list(dmc.FindList(n => true, 0, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList()));
                        }
                    }
                    else
                    {
                        css = (m.child == null) ? " class=\"J_menuItem\"" : "";
                        target = (m.target != null) ? " target=\"" + m.target + "\"" : "";
                        css = (m.target == null) ? css : "";
                        sb.Append("<li>");
                        sb.Append("    <a" + css + " href=\"" + m.url + "\"" + target + ">");
                        sb.Append("        <i class=\"fa " + m.ico + "\"></i>");
                        sb.Append("        <span class=\"nav-label\">" + m.mname + "</span>");
                        if (m.child != null)
                        {
                            sb.Append("        <span class=\"fa arrow\"></span>");
                        }
                        sb.Append("    </a>");
                        if (m.child != null)
                        {
                            List<main> l2 = m.child;
                            if (string.IsNullOrEmpty(mqx) == false)
                            {
                                l2 = l2.Where(n => mqx.Contains("," + n.id + ",")).ToList();
                            }
                            sb.Append("    <ul class=\"nav nav-second-level\">");
                            foreach (var i in l2)
                            {
                                target = (i.target != null) ? " target=\"" + i.target + "\"" : "";
                                css = (i.target == null) ? " class=\"J_menuItem\"" : "";
                                sb.Append("        <li>");
                                sb.Append("            <a" + css + " href=\"" + i.url + "\" data-index=\"0\"" + target + ">" + i.mname + "</a>");
                                sb.Append("        </li>");
                            }
                            sb.Append("    </ul>");
                        }
                        sb.Append("</li>");
                    }
                }
            }
            return sb.ToString();
        }


        //public string list_top_child(string id, string cid, string mqx)
        //{
        //    if (id == "0" || cid == "0")
        //    {
        //        return "";
        //    }

        //    StringBuilder sb = new StringBuilder();
        //    List<main> l = JsonConvert.DeserializeObject<List<main>>(str);
        //    main m= l.Where(a => a.id == id).First();
        //    if (m != null)
        //    {
        //        List<main> l2 = m.child;
        //        if (string.IsNullOrEmpty(mqx) == false)
        //        {
        //            l2 = l2.Where(n => mqx.Contains("," + n.id + ",")).ToList();
        //        }
        //        if (l2.Count > 0)
        //        {
        //            sb.Append("<ul>");
        //            foreach (main m2 in l2)
        //            {
        //                string classstr = "";
        //                if ((m.id+"_"+cid) == m2.id)
        //                {
        //                    classstr = "class=\"bg\"";
        //                }
        //                sb.Append("<li><a href=\"" + m2.url + "\" "+classstr+" >" + m2.mname + "</a></li>");
        //            }
        //            sb.Append("</ul>");
        //        }
        //    }
        //    return sb.ToString();
        //}

        public string list_checkbox_qx(string bigqx,string smqx)
        {
            StringBuilder sb = new StringBuilder();
            List<main> l = JsonConvert.DeserializeObject<List<main>>(str);
            if(l.Count > 0)
            {
                foreach(main m in l)
                {
                    string checkedb = "";
                    if (bigqx.Contains(","+m.id+","))
                    {
                        checkedb = "checked";
                    }
                    sb.Append("<div class=\"checklist\">");
                    sb.Append("<div class=\"hicke\"><input type=\"checkbox\"  class=\"p_"+m.id+"\" name=\"bqx\" id=\"bqx"+m.id+"\" value=\"" + m.id + "\" "+checkedb+" >" + m.mname + "</div>");
                    if (m.child != null)
                    {
                        List<main> l2 = m.child;
                        if (l2.Count > 0)
                        {
                            sb.Append("<div class=\"childhi\">");
                            foreach (main m2 in l2)
                            {
                                string checkedm = "";
                                if (smqx.Contains("," + m2.id + ","))
                                {
                                    checkedm = "checked";
                                }
                                sb.Append("<input type=\"checkbox\" name=\"mqx\" class=\"p_" + m.id + "\" id=\"mqx" + m2.id + "\" value=\"" + m2.id + "\" " + checkedm + " >" + m2.mname + "");
                            }
                            sb.Append("</div>");
                        }
                    }
                    sb.Append("</div>");
                }
            }
            return sb.ToString();
        }







        public string list_left_menu(string bqx, string mqx, string lang)
        {
            //<li>
            //    <a href="#">
            //        <i class="fa fa-home"></i>
            //        <span class="nav-label">主页</span>
            //        <span class="fa arrow"></span>
            //    </a>
            //    <ul class="nav nav-second-level">
            //        <li>
            //            <a class="J_menuItem" href="index_v1.html" data-index="0">主页示例一</a>
            //        </li>
            //        <li>
            //            <a class="J_menuItem" href="index_v2.html">主页示例二</a>
            //        </li>
            //        <li>
            //            <a class="J_menuItem" href="index_v3.html">主页示例三</a>
            //        </li>
            //        <li>
            //            <a class="J_menuItem" href="index_v4.html">主页示例四</a>
            //        </li>
            //        <li>
            //            <a href="index_v5.html" target="_blank">主页示例五</a>
            //        </li>
            //    </ul>
            //</li>
            StringBuilder sb = new StringBuilder();
            List<main> l = JsonConvert.DeserializeObject<List<main>>(str);
            if (string.IsNullOrEmpty(bqx) == false)
            {
                l = l.Where(n => bqx.Contains("," + n.id + ",")).ToList();
            }
            sb.Append("<li>");
            sb.Append("    <a class=\"J_menuItem\" href=\"/management/statistics/index\" data-index=\"0\">");
            sb.Append("        <i class=\"fa fa-home\"></i>");
            sb.Append("        <span class=\"nav-label\">主页</span>");
            sb.Append("    </a>");
            sb.Append("</li>");
            if (l.Count > 0)
            {
                string css = "", target = "";
                foreach (main m in l)
                {
                    if (m.ico == "fa-cubes") //文章分类
                    {
                        using (ykmWebDbContext s = new ykmWebDbContext())
                        {
                            DalMenuClass dmc = new DalMenuClass(s);
                            sb.Append(menu_class_list(dmc.FindList(n => n.language == lang, 0, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList()));
                        }
                    }
                    else
                    {
                        css = (m.child == null) ? " class=\"J_menuItem\"" : "";
                        target = (m.target != null) ? " target=\"" + m.target + "\"" : "";
                        css = (m.target == null) ? css : "";
                        sb.Append("<li>");
                        sb.Append("    <a" + css + " href=\"" + m.url + "?lang="+ lang + "\"" + target + ">");
                        sb.Append("        <i class=\"fa " + m.ico + "\"></i>");
                        sb.Append("        <span class=\"nav-label\">" + m.mname + "</span>");
                        if (m.child != null)
                        {
                            sb.Append("        <span class=\"fa arrow\"></span>");
                        }
                        sb.Append("    </a>");
                        if (m.child != null)
                        {
                            List<main> l2 = m.child;
                            if (string.IsNullOrEmpty(mqx) == false)
                            {
                                l2 = l2.Where(n => mqx.Contains("," + n.id + ",")).ToList();
                            }
                            sb.Append("    <ul class=\"nav nav-second-level\">");
                            foreach (var i in l2)
                            {
                                target = (i.target != null) ? " target=\"" + i.target + "\"" : "";
                                css = (i.target == null) ? " class=\"J_menuItem\"" : "";
                                sb.Append("        <li>");
                                sb.Append("            <a" + css + " href=\"" + i.url + "\" data-index=\"0\"" + target + ">" + i.mname + "</a>");
                                sb.Append("        </li>");
                            }
                            sb.Append("    </ul>");
                        }
                        sb.Append("</li>");
                    }
                }
            }
            return sb.ToString();
        }


        public string sanji_list(int classid)
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                StringBuilder sb = new StringBuilder();
                DalMenuClass dmc = new DalMenuClass(s);
                var l = dmc.FindList(n => n.ParentID == classid, 0, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList();
                if (l.Count != 0)
                {
                    sb.Append("<ul class=\"nav nav-third-level\">");
                    foreach (var m in l)
                    {
                        sb.Append("<li><a class=\"J_menuItem\" href=\"" + getHtUrl(m) + "\">" + m.Catalogname + "</a></li>");
                    }
                    sb.Append("</ul>");
                }
                return sb.ToString();
            }
        }


        public string menu_class_list(List<menuClass> l)
        {
            StringBuilder sb = new StringBuilder();
            string css = "";
            if (l.Count > 0)
            {
                List<menuClass> l1 = l.Where(n => n.ParentID == 0).ToList();
                if (l1.Count > 0)
                {
                    foreach (var m in l1)
                    {
                        css = (m.Child == 0) ? " class=\"J_menuItem\"" : "";
                        sb.Append("<li>");
                        sb.Append("    <a" + css + " href=\"" + getHtUrl(m) + "\">");
                        sb.Append("        <i class=\"fa fa-angle-double-right\"></i>");
                        sb.Append("        <span class=\"nav-label\">" + m.Catalogname + "</span>");
                        if (m.Child > 0)
                        {
                            sb.Append("        <span class=\"fa arrow\"></span>");
                        }
                        sb.Append("    </a>");
                        if (m.Child > 0)
                        {
                            List<menuClass> l2 = l.Where(n => n.ParentID == m.Catalogid).ToList();
                            sb.Append("    <ul class=\"nav nav-second-level\">");
                            foreach (var m2 in l2)
                            {
                                css = (m2.Child == 0) ? " class=\"J_menuItem\"" : "";
                                sb.Append("        <li>");
                                sb.Append("            <a" + css + " href=\"" + getHtUrl(m2) + "\" data-index=\"0\">" + m2.Catalogname + "</a>");
                                sb.Append(sanji_list(m2.Catalogid.Value));
                                sb.Append("        </li>");
                            }
                            sb.Append("    </ul>");
                        }
                        sb.Append("</li>");
                    }
                }
            }
            return sb.ToString();
        }





        public string getHtUrl(menuClass i)
        {
            string url = "";
            if (i.pclisttype.IndexOf("list") != -1)
            {
                if (i.pclisttype.IndexOf("pro") != -1)
                {
                    url = string.Format("/management/infolist/prolist/{0}", i.Catalogid);
                }
                else if (i.pclisttype.IndexOf("news") != -1)
                {
                    url = string.Format("/management/infolist/newslist/{0}", i.Catalogid);
                }
            }
            else if (i.pclisttype.IndexOf("cont") != -1)
            {
                url = string.Format("/management/infolist/cont/{0}", i.Catalogid);
            }
            else if(i.pclisttype.IndexOf("msg") != -1)
            {
                //url = string.Format("/management/infolist/msg/{0}", i.Catalogid);
                url = "/management/guestBook/index";
            }
            return url;
        }
    }
}
