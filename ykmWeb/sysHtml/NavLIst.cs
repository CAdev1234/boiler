using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;
using ykmWeb.Models;
using ykmWeb.common;
using System.Linq.Expressions;
using ykmWeb.Bll;
using HtmlAgilityPack;

namespace ykmWeb.sysHtml
{
    public class NavLIst
    {
        ykmWebDbContext s;
        OrderModelField[] mrorder;
        public NavLIst(ykmWebDbContext _s)
        {
            s = _s;
            mrorder = new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } };
        }

        public OrderModelField[] getOrderList()
        {
            return mrorder;
        }

        /// <summary>
        /// 一级导航
        /// </summary>
        /// <param name="l">导航合集</param>
        /// <param name="defCatalogid"></param>
        /// <returns></returns>
        public string LIstMainNav(int defCatalogid = 0)
        {
            //<li class="on"><a href="#">网站首页</a></li>
            //<li><a href="#">走进奥龙</a></li>
            //<li><a href="#">产品展示</a></li>
            //<li><a href="#">环境展示</a></li>
            //<li><a href="#">下载中心</a></li>
            //<li><a href="#">营销网络</a></li>
            //<li><a href="#">新闻中心</a></li>
            //<li><a href="#">联系我们</a></li>
            StringBuilder sb = new StringBuilder();
            DalMenuClass dl = new DalMenuClass(s);
            string parstr = "";
            var currObj = dl.FindList(n => n.Catalogid == defCatalogid, 0, null).SingleOrDefault();
            if (currObj != null)
            {
                parstr = currObj.ParentStr + "," + currObj.Catalogid.ToString();
            }
            else
            {
                parstr = "0";
            }
            List<menuClass> l = dl.FindList(n => n.mainnavshow == 1 && n.ParentID == 0&&n.language =="cn", 0, mrorder).ToList();
            if (l.Count() > 0)
            {
                if (defCatalogid == 0)
                {
                    sb.Append("<li class=\"on\"><a href=\"/\">网站首页</a></li>");
                }
                else
                {
                    sb.Append("<li><a href=\"/\">网站首页</a></li>");
                }
                foreach (var o in l)
                {
                    string defCss = "";
                    if (ishas(parstr, o.Catalogid.ToString()))
                    {
                        defCss = " on";
                    }
                    sb.Append("<li class=\"nLi"+ defCss + "\">");
                    sb.Append("    <a href=\"" + getUrlLink(o) + "\">" + o.Catalogname + "</a>");
                    //if (o.Child > 0)
                    //{
                    //    sb.Append(LIstMainSubNav(o.Catalogid.Value));
                    //}
                    sb.Append("</li>");
                }
            }
            return sb.ToString();
        }

        public string LIstMainNav_en(int defCatalogid = 0)
        {
            //<li class="on"><a href="#">网站首页</a></li>
            //<li><a href="#">走进亨恒</a></li>
            //<li><a href="#">产品中心</a></li>
            //<li><a href="#">人才招聘</a></li>
            //<li><a href="#">在线留言</a></li>
            //<li><a href="#">公司动态</a></li>
            //<li><a href="#">联系我们</a></li>
            StringBuilder sb = new StringBuilder();
            DalMenuClass dl = new DalMenuClass(s);
            string parstr = "";
            var currObj = dl.FindList(n => n.Catalogid == defCatalogid, 0, null).SingleOrDefault();
            if (currObj != null)
            {
                parstr = currObj.ParentStr + "," + currObj.Catalogid.ToString();
            }
            else
            {
                parstr = "0";
            }
            List<menuClass> l = dl.FindList(n => n.mainnavshow == 1 && n.ParentID == 0 && n.language == "en", 0, mrorder).ToList();
            if (l.Count() > 0)
            {
                if (defCatalogid == 0)
                {
                    sb.Append("<li class=\"on\"><a href=\"/en/index\">HOME</a></li>");
                }
                else
                {
                    sb.Append("<li><a href=\"/en/index\">HOME</a></li>");
                }
                foreach (var o in l)
                {
                    string defCss = "";
                    if (ishas(parstr, o.Catalogid.ToString()))
                    {
                        defCss = " on";
                    }
                    sb.Append("<li class=\"nLi" + defCss + "\">");
                    sb.Append("    <a href=\"" + getUrlLink_en(o) + "\">" + o.Catalogname + "</a>");
                    //if (o.Child > 0)
                    //{
                    //    sb.Append(LIstMainSubNav(o.Catalogid.Value));
                    //}
                    sb.Append("</li>");
                }
            }
            return sb.ToString();
        }

        public string LIstMainSubNav(int parentid = 0)
        {
            //<ul class="sub">
            //    <li><a href="javascript:;">企业介绍</a></li>
            //    <li><a href="javascript:;">董事长致辞</a></li>
            //    <li><a href="javascript:;">最新动态</a></li>
            //    <li class="buy"><a href="javascript:;">前往购买页面</a></li>
            //</ul>
            StringBuilder sb = new StringBuilder();
            DalMenuClass dl = new DalMenuClass(s);
            List<menuClass> l = dl.FindList(n => n.mainnavshow == 1 && n.ParentID == parentid, 0, mrorder).ToList();
            if (l.Count() > 0)
            {
                sb.Append("<ul class=\"sub\">");
                foreach (var o in l)
                {
                    sb.Append("    <li><a href=\"" + getUrlLink(o) + "\">" + o.Catalogname + "</a></li>");
                }
                var pro = dl.find(n => n.leftnavshow == 1 && n.tabletype == "pro" && n.Depth==2);
                sb.Append("    <li class=\"buy\"><a href=\""+ getUrlLink(pro) + "\">前往购买页面</a></li>");
                sb.Append("</ul>");
            }
            return sb.ToString();
        }

        public string mobi_nav(string title)
        {
            DalMenuClass dl = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            List<menuClass> l = dl.FindList(n => n.mainnavshow == 1 && n.ParentID == 0, 0, mrorder).ToList();
            if (l.Count() > 0)
            {
                sb.Append("<ul class=\"parents\">");
                sb.Append("<li class=\"nLi\"><a href=\"/\">网站首页</a></li>");
                foreach (var o in l)
                {
                    string link = "javascript:void(null)";
                    if (o.Child == 0)
                    {
                        link = getUrlLink(o);
                    }
                    sb.Append("<li class=\"nLi\"><a href=\"" + link + "\">" + o.Catalogname + "</a>");
                    if (o.Child > 0)
                    {
                        var pid = o.Catalogid.Value;
                        var vl = dl.FindList(n => n.ParentID == pid, 0, mrorder).ToList();
                        if (vl.Count > 0)
                        {
                            sb.Append("<ul class=\"sub\">");
                            foreach (var v in vl)
                            {
                                sb.Append("<li><a href=\"" + getUrlLink(v) + "\">" + v.Catalogname + "</a></li>");
                            }
                            sb.Append("</ul>");
                        }
                        sb.Append("</li>");
                        sb.Append("<div class=\"bg\"></div>");
                    }
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// 底部一级导航
        /// </summary>
        /// <param name="l">导航合集</param>
        /// <param name="defCatalogid"></param>
        /// <returns></returns>
        public string LIstbootNav(int defCatalogid = 0)
        {
            //<a href="#">网站首页</a>  /  <a href="#">走进亨恒 </a>  /  <a href="#">产品中心</a>  /  <a href="#">人才招聘 </a>  /  <a href="#">在线留言</a>  /  <a href="#">公司动态</a>/  <a href="#">联系我们</a>
            StringBuilder sb = new StringBuilder();
            DalMenuClass dl = new DalMenuClass(s);
            NavLIst nl = new NavLIst(s);
            List<menuClass> l = dl.FindList(n => n.mainnavshow == 1 && n.ParentID == 0&&n.language=="cn", 0, mrorder).ToList();
            if (l.Count() > 0)
            {
                sb.Append("<a href=\"/index\">网站首页</a>");
                foreach (var o in l)
                {
                    //sb.Append("<div class=\"daohang\">");
                    //sb.Append("    <h2>" + o.Catalogname + "</h2>");
                    //sb.Append(LIstSubbootNav(o.Catalogid.Value));
                    //sb.Append("</div>");
                    sb.Append("  /  ");
                    sb.Append("<a href=\"" + nl.getUrlLink(o) + "\">" + o.Catalogname + "</a>");
                }
            }
            return sb.ToString();
        }

        public string LIstbootNav_en(int defCatalogid = 0)
        {
            //<a href="#">网站首页</a>  /  <a href="#">走进亨恒 </a>  /  <a href="#">产品中心</a>  /  <a href="#">人才招聘 </a>  /  <a href="#">在线留言</a>  /  <a href="#">公司动态</a>/  <a href="#">联系我们</a>
            StringBuilder sb = new StringBuilder();
            DalMenuClass dl = new DalMenuClass(s);
            NavLIst nl = new NavLIst(s);
            List<menuClass> l = dl.FindList(n => n.mainnavshow == 1 && n.ParentID == 0 && n.language == "en", 0, mrorder).ToList();
            if (l.Count() > 0)
            {
                sb.Append("<a href=\"/en/index\">HOME</a>");
                foreach (var o in l)
                {
                    //sb.Append("<div class=\"daohang\">");
                    //sb.Append("    <h2>" + o.Catalogname + "</h2>");
                    //sb.Append(LIstSubbootNav(o.Catalogid.Value));
                    //sb.Append("</div>");
                    sb.Append("  /  ");
                    sb.Append("<a href=\"" + nl.getUrlLink_en(o) + "\">" + o.Catalogname + "</a>");
                }
            }
            return sb.ToString();
        }

        public string LIstSubbootNav(int parentid = 0)
        {
            //<ul>
            //    <li><a href="javascript:;">企业介绍</a></li>
            //    <li><a href="javascript:;">董事长致辞</a></li>
            //    <li><a href="javascript:;">最新动态</a></li>
            //</ul>
            StringBuilder sb = new StringBuilder();
            DalMenuClass dl = new DalMenuClass(s);

            List<menuClass> l = dl.FindList(n => n.mainnavshow == 1 && n.ParentID == parentid, 0, mrorder).ToList();
            if (l.Count() > 0)
            {
                sb.Append("<ul>");
                foreach (var o in l)
                {
                    sb.Append("<li><a href=\"" + getUrlLink(o) + "\">" + o.Catalogname + "</a></li>");
                }
                sb.Append("</ul>");
            }
            return sb.ToString();
        }


        /// <summary>
        /// 底部一级导航
        /// </summary>
        /// <param name="l">导航合集</param>
        /// <param name="defCatalogid"></param>
        /// <returns></returns>
        public string LIstbootNavMobi(int defCatalogid = 0)
        {
            StringBuilder sb = new StringBuilder();
            DalMenuClass dl = new DalMenuClass(s);
            NavLIst nl = new NavLIst(s);
            var l = dl.FindList(n=>n.ParentID == defCatalogid&&n.language=="cn",0,new OrderModelField[] { new OrderModelField { propertyName="RootID",IsDESC=false}, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList();
            if (l.Count > 0)
            {
                string href = "";
                foreach(var i in l)
                {
                    href = nl.getMobiUrlLink(i);
                    sb.Append("<a href=\"" + href + "\">" + i.Catalogname + "</a>");
                }
            }
            return sb.ToString();
        }

        public string LIstbootNavMobi_en(int defCatalogid = 0)
        {
            StringBuilder sb = new StringBuilder();
            DalMenuClass dl = new DalMenuClass(s);
            NavLIst nl = new NavLIst(s);
            var l = dl.FindList(n => n.ParentID == defCatalogid && n.language == "en", 0, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList();
            if (l.Count > 0)
            {
                string href = "";
                foreach (var i in l)
                {
                    href = nl.getMobiUrlLink_en(i);
                    sb.Append("<a href=\"" + href + "\">" + i.Catalogname + "</a>");
                }
            }
            return sb.ToString();
        }
        //public string LIstbootNavMobi(int defCatalogid = 0)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    DalMenuClass dl = new DalMenuClass(s);
        //    var topid = dl.findTopidByCid(defCatalogid);
        //    var top = dl.find(n => n.Catalogid == topid);
        //    sb.Append("<h3>" + top.Catalogname + "</h3>");
        //    sb.Append(LIstSubbootNavMobi(top.Catalogid.Value));
        //    return sb.ToString();
        //}
        public string LIstSubbootNavMobi(int parentid = 0)
        {
            //<ul>
            //    <li><a href="javascript:;">企业介绍</a></li>
            //    <li><a href="javascript:;">董事长致辞</a></li>
            //    <li><a href="javascript:;">最新动态</a></li>
            //</ul>
            StringBuilder sb = new StringBuilder();
            DalMenuClass dl = new DalMenuClass(s);

            List<menuClass> l = dl.FindList(n => n.mainnavshow == 1 && n.ParentID == parentid, 0, mrorder).ToList();
            if (l.Count() > 0)
            {
                sb.Append("<ul>");
                foreach (var o in l)
                {
                    sb.Append("<li><a href=\"" + getMobiUrlLink(o) + "\">" + o.Catalogname + "</a></li>");
                }
                var pro = dl.find(n => n.leftnavshow == 1 && n.tabletype == "pro" && n.Depth == 2);
                sb.Append("<li><a href=\"" + getMobiUrlLink(pro) + "\">前往购买界面</a></li>");

                sb.Append("</ul>");
            }
            return sb.ToString();
        }

        public string getUrlLink(menuClass m)
        {
            pageLink pl = new pageLink();
            if (m != null)
            {
                switch (m.linktype)
                {
                    default:
                        return pl.getUrl(m.pagename) + "?cid=" + m.Catalogid;
                    case 1:
                        DalMenuClass dmc = new DalMenuClass(s);
                        var o = dmc.FindList(n => n.Catalogid == m.linkCid, 1, null).Select(g => new { g.pagename, g.linkurl, g.linktype, g.Catalogid }).SingleOrDefault();
                        if (o != null)
                        {
                            if (o.linktype == 0)
                            {
                                return pl.getUrl(o.pagename) + "?cid=" + o.Catalogid;
                            }
                            else if (o.linktype == 2)
                            {
                                return o.linkurl;
                            }
                            else
                            {
                                return pl.getUrl(m.pagename) + "?cid=" + m.Catalogid;
                            }
                        }
                        else
                        {
                            return pl.getUrl(m.pagename) + "?cid=" + m.Catalogid;
                        }
                    case 2:
                        return m.linkurl;
                }
            }
            else
            {
                return "";
            }
        }
        public string getContLink(menuClass m)
        {
            pageLink pl = new pageLink();
            return pl.getContUrl(m.pagename);
        }

        public string getUrlLink_en(menuClass m)
        {
            pageLink pl = new pageLink();
            if (m != null)
            {
                switch (m.linktype)
                {
                    default:
                        return pl.getUrl_en(m.pagename) + "?cid=" + m.Catalogid;
                    case 1:
                        DalMenuClass dmc = new DalMenuClass(s);
                        var o = dmc.FindList(n => n.Catalogid == m.linkCid, 1, null).Select(g => new { g.pagename, g.linkurl, g.linktype, g.Catalogid }).SingleOrDefault();
                        if (o != null)
                        {
                            if (o.linktype == 0)
                            {
                                return pl.getUrl_en(o.pagename) + "?cid=" + o.Catalogid;
                            }
                            else if (o.linktype == 2)
                            {
                                return o.linkurl;
                            }
                            else
                            {
                                return pl.getUrl_en(m.pagename) + "?cid=" + m.Catalogid;
                            }
                        }
                        else
                        {
                            return pl.getUrl_en(m.pagename) + "?cid=" + m.Catalogid;
                        }
                    case 2:
                        return m.linkurl;
                }
            }
            else
            {
                return "";
            }
        }
        public string getContLink_en(menuClass m)
        {
            pageLink pl = new pageLink();
            return pl.getContUrl_en(m.pagename);
        }



        //public string getMobiUrlLink(menuClass m)
        //{
        //    pageLink pl = new pageLink();
        //    switch (m.linktype)
        //    {
        //        default:
        //            return pl.getH5Url(m.pagename) + "?cid=" + m.Catalogid;
        //        case 1:
        //            DalMenuClass dmc = new DalMenuClass(s);
        //            var o = dmc.FindList(n => n.Catalogid == m.linkCid, 1, null).Select(g => new { g.pagename, g.linkurl, g.linktype, g.Catalogid }).SingleOrDefault();
        //            if (o != null)
        //            {
        //                if (o.linktype == 0)
        //                {
        //                    return pl.getH5Url(o.pagename) + "?cid=" + o.Catalogid;
        //                }
        //                else if (o.linktype == 2)
        //                {
        //                    return o.linkurl;
        //                }
        //                else
        //                {
        //                    return pl.getH5Url(m.pagename) + "?cid=" + m.Catalogid;
        //                }
        //            }
        //            else
        //            {
        //                return pl.getH5Url(m.pagename) + "?cid=" + m.Catalogid;
        //            }
        //        case 2:
        //            return m.linkurl;
        //    }
        //}
        public string getMobiUrlLink(menuClass m)
        {
            pageLink pl = new pageLink();
            if (m != null)
            {
                switch (m.h5linktype)
                {
                    default:
                        return pl.getH5Url(m.pagename) + "?cid=" + m.Catalogid;
                    case 1:
                        DalMenuClass dmc = new DalMenuClass(s);
                        var o = dmc.FindList(n => n.Catalogid == m.h5linkCid, 1, null).Select(g => new { g.pagename, g.h5linkurl, g.h5linktype, g.Catalogid }).SingleOrDefault();
                        if (o != null)
                        {
                            if (o.h5linktype == 0)
                            {
                                return pl.getH5Url(o.pagename) + "?cid=" + o.Catalogid;
                            }
                            else if (o.h5linktype == 2)
                            {
                                return o.h5linkurl;
                            }
                            else
                            {
                                return pl.getH5Url(m.pagename) + "?cid=" + m.Catalogid;
                            }
                        }
                        else
                        {
                            return pl.getH5Url(m.pagename) + "?cid=" + m.Catalogid;
                        }
                    case 2:
                        return m.h5linkurl;
                }
            }
            else
            {
                return "";
            }
        }
        public string getMobiContLink(menuClass m)
        {
            pageLink pl = new pageLink();
            return pl.getH5ContUrl(m.pagename);
        }

        public string getMobiUrlLink_en(menuClass m)
        {
            pageLink pl = new pageLink();
            if (m != null)
            {
                switch (m.h5linktype)
                {
                    default:
                        return pl.getH5Url_en(m.pagename) + "?cid=" + m.Catalogid;
                    case 1:
                        DalMenuClass dmc = new DalMenuClass(s);
                        var o = dmc.FindList(n => n.Catalogid == m.h5linkCid, 1, null).Select(g => new { g.pagename, g.h5linkurl, g.h5linktype, g.Catalogid }).SingleOrDefault();
                        if (o != null)
                        {
                            if (o.h5linktype == 0)
                            {
                                return pl.getH5Url_en(o.pagename) + "?cid=" + o.Catalogid;
                            }
                            else if (o.h5linktype == 2)
                            {
                                return o.h5linkurl;
                            }
                            else
                            {
                                return pl.getH5Url_en(m.pagename) + "?cid=" + m.Catalogid;
                            }
                        }
                        else
                        {
                            return pl.getH5Url_en(m.pagename) + "?cid=" + m.Catalogid;
                        }
                    case 2:
                        return m.h5linkurl;
                }
            }
            else
            {
                return "";
            }
        }
        public string getMobiContLink_en(menuClass m)
        {
            pageLink pl = new pageLink();
            return pl.getH5ContUrl_en(m.pagename);
        }

        public viewMenuClass getMenuClass(menuClass m)
        {
            viewMenuClass v = new viewMenuClass();
            if (m != null)
            {
                v.Catalogid = m.Catalogid;
                v.Catalogname = m.Catalogname;
                v.Link = getUrlLink(m);
                v.Caenname = m.Caenname;
                v.defaultpic = m.defaultpic;
                v.fbt = m.subtitle;
            }        
            return v;
        }

        public viewMenuClass getMenuClass_en(menuClass m)
        {
            viewMenuClass v = new viewMenuClass();
            if (m != null)
            {
                v.Catalogid = m.Catalogid;
                v.Catalogname = m.Catalogname;
                v.Link = getUrlLink_en(m);
                v.Caenname = m.Caenname;
                v.defaultpic = m.defaultpic;
                v.fbt = m.subtitle;
            }
            return v;
        }

        public List<viewMenuClass> getMenuClassList(List<menuClass> l)
        {
            List<viewMenuClass> vl = new List<viewMenuClass>();
            if(l.Count() > 0)
            {
                foreach(var m in l)
                {
                    viewMenuClass v = new viewMenuClass();
                    v.Catalogid = m.Catalogid;
                    v.Catalogname = m.Catalogname;
                    v.Link = getUrlLink(m);
                    v.Caenname = m.Caenname;
                    v.fbt = m.subtitle;
                    vl.Add(v);
                }
            }
            return vl;
        }

        public viewMenuClass getMobiMenuClass(menuClass m)
        {
            viewMenuClass v = new viewMenuClass();
            if (m != null)
            {
                v.Catalogid = m.Catalogid;
                v.Catalogname = m.Catalogname;
                v.Link = getMobiUrlLink(m);
                v.Caenname = m.Caenname;
                v.defaultpic = m.defaultpic;
                v.fbt = m.subtitle;
            }
            return v;
        }
        public List<viewMenuClass> getMobiMenuClassList(List<menuClass> l)
        {
            List<viewMenuClass> vl = new List<viewMenuClass>();
            if (l.Count() > 0)
            {
                foreach (var m in l)
                {
                    viewMenuClass v = new viewMenuClass();
                    v.Catalogid = m.Catalogid;
                    v.Catalogname = m.Catalogname;
                    v.Link = getMobiUrlLink(m);
                    v.Caenname = m.Caenname;
                    v.fbt = m.subtitle;
                    vl.Add(v);
                }
            }
            return vl;
        }

        public string classPath(int cid)
        {
            //<a href="javascript:;">网站首页</a> - <a href="javascript:;">走进亨恒</a> - <a href="javascript:;">公司简介</a>
            StringBuilder sb = new StringBuilder();
            DalMenuClass dl = new DalMenuClass(s);
            var o = dl.FindList(n => n.Catalogid == cid, 0, mrorder).FirstOrDefault();
            if (o != null)
            {
                sb.Append("<a href=\"/\">网站首页</a>");
                o.ParentStr= o.ParentStr + "," + o.Catalogid.Value;
                string[] parstr = o.ParentStr.Split(',');
                var ilist = dl.FindList(g => parstr.Contains(g.Catalogid.ToString()), 0, mrorder).ToList();
                if(ilist.Count() > 0)
                {
                    foreach(var i in ilist)
                    {
                        sb.Append(" - <a href=\"" + getUrlLink(i)+"\">"+i.Catalogname+"</a>");
                    }                 
                }
            }
            return sb.ToString();

        }

        public string classPath_en(int cid)
        {
            //<a href="javascript:;">网站首页</a> - <a href="javascript:;">走进亨恒</a> - <a href="javascript:;">公司简介</a>
            StringBuilder sb = new StringBuilder();
            DalMenuClass dl = new DalMenuClass(s);
            var o = dl.FindList(n => n.Catalogid == cid, 0, mrorder).FirstOrDefault();
            if (o != null)
            {
                sb.Append("<a href=\"/en/index\">HOME</a>");
                o.ParentStr = o.ParentStr + "," + o.Catalogid.Value;
                string[] parstr = o.ParentStr.Split(',');
                var ilist = dl.FindList(g => parstr.Contains(g.Catalogid.ToString()), 0, mrorder).ToList();
                if (ilist.Count() > 0)
                {
                    foreach (var i in ilist)
                    {
                        sb.Append(" - <a href=\"" + getUrlLink_en(i) + "\">" + i.Catalogname + "</a>");
                    }
                }
            }
            return sb.ToString();

        }


        public string h5classPath(int cid)
        {
            //<a href="javascript:;">首页</a> － <a href="javascript:;">关于我们</a>
            StringBuilder sb = new StringBuilder();
            DalMenuClass dl = new DalMenuClass(s);
            var o = dl.FindList(n => n.Catalogid == cid, 0, mrorder).FirstOrDefault();
            if (o != null)
            {
                sb.Append("<a href=\"/h5/\">首页</a>");
                o.ParentStr = o.ParentStr + "," + o.Catalogid.Value;
                string[] parstr = o.ParentStr.Split(',');
                var ilist = dl.FindList(g => parstr.Contains(g.Catalogid.ToString()), 0, mrorder).ToList();
                if (ilist.Count() > 0)
                {
                    foreach (var i in ilist)
                    {
                        sb.Append(" － <a href=\"" + getMobiUrlLink(i) + "\">" + i.Catalogname + "</a>");
                    }
                }
            }
            return sb.ToString();

        }

        public string leftMenu_allnav_lv3(int cid=0)
        {
            //<li class="parentsli"><h3><a href="javascript:;">关于我们</a></h3></li>
            //<li class="on parentsli">
            //    <h3><a href="javascript:;">业务板块</a></h3>
            //    <ul class="child">
            //        <li class="childli">
            //            <a href="javascript:;">军工资质、咨询服务</a>
            //            <ul class="childchild">
            //                <li><a href="javascript:;">武器装备科研生产单位保密资格</a></li>
            //                <li><a href="javascript:;">武器装备质量管理体系认证</a></li>
            //                <li><a href="javascript:;">武器装备科研生产许可证</a></li>
            //                <li><a href="javascript:;">装备承制单位资格</a></li>
            //                <li class="childchildon"><a href="javascript:;">我要填写（申请军工资质信息登记）</a></li>
            //                <li><a href="javascript:;">资质问答</a></li>
            //                <li><a href="javascript:;">装备承制资格受理点</a></li>
            //                <li><a href="javascript:;">资料下载</a></li>
            //            </ul>
            //        </li>
            //        <li class="childli"><a href="javascript:;">民参军企技术推介</a></li>
            //        <li class="childli"><a href="javascript:;">军转民成果推广（转让）</a></li>
            //        <li class="childli"><a href="javascript:;">军民项目对接</a></li>
            //        <li class="childli"><a href="javascript:;">产品检测与维修</a></li>
            //        <li class="childli"><a href="javascript:;">金融支持</a></li>
            //        <li class="childli last"><a href="javascript:;">成功案例</a></li>
            //    </ul>
            //</li>
            //<li class="parentsli"><h3><a href="javascript:;">企业动态</a></h3></li>
            //<li class="parentsli"><h3><a href="javascript:;">服务承诺</a></h3></li>
            //<li class="parentsli"><h3><a href="javascript:;">联系我们</a></h3></li>
            DalMenuClass dm = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            var thisObj = dm.FindList(n => n.Catalogid == cid, 1, null).SingleOrDefault();
            if (thisObj != null)
            {
                int pid = thisObj.Catalogid.Value;
                int currCid = thisObj.Catalogid.Value;
                string pCatalogname = thisObj.Catalogname;
                string pEnglish = thisObj.subtitle;
                string checkPIdStr = thisObj.ParentStr + "," + thisObj.Catalogid.Value;
                string pCaenname = thisObj.Caenname;
                if (thisObj.ParentID> 0)
                {
                    int rootid = thisObj.RootID.Value;
                    var objP = dm.FindList(n => n.RootID == rootid && n.ParentID == 0, 1, getOrderList()).FirstOrDefault();
                    if (objP != null)
                    {
                        pid = objP.Catalogid.Value;
                        pCatalogname = objP.Catalogname;
                        pEnglish = objP.subtitle;
                        pCaenname = objP.Caenname;
                    }
                }
                //sb.Append("<div class=\"left title\">" + pCatalogname + "<img class=\"jt\" src=\"/web_images/box_left_jt.png\" /></div>");

                var toplist = dm.FindList(n => n.ParentID == 0, 0, getOrderList()).ToList();
                if(toplist.Count > 0)
                { 
                    foreach(var t in toplist)
                    {
                        string defstyle = "";
                        bool isact = ishas(checkPIdStr, t.Catalogid.ToString());
                        if (isact)
                        {
                            defstyle = "on";
                        }
                        sb.Append("<li class=\"parentsli " + defstyle + "\"><h3><a href=\"" + getUrlLink(t) + "\">" + t.Catalogname + "</a></h3>");
                        if (t.Child > 0 && isact)
                        {
                            var ilist = dm.FindList(n => n.ParentID == pid, 0, getOrderList()).ToList();
                            if (ilist.Count() > 0)
                            {
                                sb.Append("<ul class=\"child\">");
                                foreach(var o in ilist)
                                {
                                    string defstyle1 = "";
                                    bool isact1 = ishas(checkPIdStr, o.Catalogid.ToString());
                                    if (isact1 && o.Child==0)
                                    {
                                        defstyle1 = "childon";
                                    }
                                    sb.Append("<li class=\"childli " + defstyle1 + "\"><a href=\"" + getUrlLink(o) + "\">" + o.Catalogname + "</a>");
                                    if (o.Child > 0 && isact1)
                                    {
                                        int _pid = o.Catalogid.Value;
                                        var cList = dm.FindList(n => n.ParentID == _pid, 0, getOrderList()).ToList();
                                        if (cList.Count() > 0)
                                        {
                                            sb.Append("<ul class=\"childchild\">");
                                            foreach (var c in cList)
                                            {
                                                string Cdef = "";
                                                if (ishas(checkPIdStr, c.Catalogid.ToString()))
                                                {
                                                    Cdef = " class=\"childchildon\"";
                                                }
                                                sb.Append("<li " + Cdef + "><a href=\"" + getUrlLink(c) + "\">" + c.Catalogname + "</a></li>");
                                            }
                                            sb.Append("</ul>");
                                        }
                                    }
                                    sb.Append("</li>");
                                }
                                sb.Append("</ul>");
                            }
                            //else
                            //{
                            //    var clist = dm.FindList(n => n.Catalogid == cid, 0, getOrderList()).SingleOrDefault();
                            //    sb.Append("<ul>");
                            //    sb.Append("<li class=\"on last\"><a href=\"" + getUrlLink(clist) + "\" target=\"_blank\">" + clist.Catalogname + "</a></li>");
                            //    sb.Append("</ul>");
                            //}
                        }
                        sb.Append("</li>");
                    }
                }
            }
            return sb.ToString();
        }

        public string leftMenu()
        {
            DalMenuClass dm = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            var toplist = dm.FindList(n => n.ParentID == 0, 0, getOrderList()).ToList();
            if (toplist.Count > 0)
            {
                foreach (var t in toplist)
                {
                    sb.Append("<li class=\"parentsli\"><h3><a href=\"" + getUrlLink(t) + "\">" + t.Catalogname + "</a></h3></li>");
                }
            }
            return sb.ToString();
        }
        public string leftMenu(int cid = 0)
        {
            
            DalMenuClass dmc = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            var thisObj = dmc.FindList(n => n.Catalogid == cid, 1, null).SingleOrDefault();
            if (thisObj != null)
            {
                int pid = thisObj.Catalogid.Value;
                int currCid = thisObj.Catalogid.Value;
                string pCatalogname = thisObj.Catalogname;
                string pEnglish = thisObj.subtitle;
                string checkPIdStr = thisObj.ParentStr + "," + thisObj.Catalogid.Value;
                string pCaenname = thisObj.Caenname;
                if (thisObj.ParentID > 0)
                {
                    int rootid = thisObj.RootID.Value;
                    var objP = dmc.FindList(n => n.RootID == rootid && n.ParentID == 0 && n.language == "cn", 1, getOrderList()).FirstOrDefault();
                    if (objP != null)
                    {
                        pid = objP.Catalogid.Value;
                        pCatalogname = objP.Catalogname;
                        pEnglish = objP.subtitle;
                        pCaenname = objP.Caenname;
                    }
                }

                sb.Append("<div class=\"flex flex-column h100\" style=\"min-width:250px;\">\n");
                sb.Append("    <div>\n");
                sb.Append("        <div class=\"flex ai-center padleft16 padright16\" style=\"height:90px;background-color:#d40101\">\n");
                sb.Append("            <div class=\"c-fff\">\n");
                sb.Append("                <div class=\"fw-bold\" style=\"font-size:24px\">" + pCatalogname + "</div>\n");
                //sb.Append("                <div>ABOUT US</div>\n");
                sb.Append("            </div>\n");
                sb.Append("            <button class=\"flex jc-center ai-center\" style=\"border-radius: 50%; color: #d40101; padding: 5px; border: none; background: white; margin-left: auto;\">\n");
                sb.Append("                <i class=\"fa fa-arrow-right\"></i>\n");
                sb.Append("            </button>\n");
                sb.Append("        </div>\n");
                sb.Append("        <div class=\"padright16 padleft16 padtop16 padbot16 flex flex-column bg-fff\">\n");

                var ilist = dmc.FindList(n => n.leftnavshow == 1 && n.ParentID == pid, 0, getOrderList()).ToList();
                if (ilist.Count() > 0)
                {
                    foreach (var o in ilist)
                    {
                        bool isact1 = ishas(checkPIdStr, o.Catalogid.ToString());
                        if (isact1)
                        {
                            sb.Append("            <div class=\"flex ai-center martauto\" style=\"height:40px;cursor:pointer;\" onclick=\"{ window.location.href = " + "'" + getUrlLink(o) + "'" + " }\">\n");
                            sb.Append("                <div style=\"width: 5px;height: 5px;border-radius: 50%;background-color: #d40101;\"></div>\n");
                            sb.Append("                <div class=\"padleft10\" style=\"color: red\">" + o.Catalogname + "</div>\n");
                            sb.Append("                <div class=\"marlauto fst20\" style=\"color: red;\">></div>\n");
                            sb.Append("            </div>\n");
                        }else
                        {
                            sb.Append("            <div class=\"flex ai-center martauto\" style=\"height:40px;cursor:pointer;\" onclick=\"{ window.location.href = " + "'" + getUrlLink(o) + "'" + " }\">\n");
                            sb.Append("                <div style=\"width: 5px;height: 5px;border-radius: 50%;background-color: #d40101;\"></div>\n");
                            sb.Append("                <div class=\"padleft10\">" + o.Catalogname + "</div>\n");
                            sb.Append("                <div class=\"marlauto fst20\">></div>\n");
                            sb.Append("            </div>\n");
                        }
                    }
                }
                if (ilist.Count == 0)
                {
                    sb.Append("            <div class=\"flex ai-center martauto\" style=\"height:40px;cursor:pointer;\" onclick=\"{ window.location.href = '/list?cid=" + thisObj.Catalogid +"'}\">\n");
                    sb.Append("                <div style=\"width: 5px;height: 5px;border-radius: 50%;background-color: #d40101;\"></div>\n");
                    sb.Append("                <div class=\"padleft10\" style=\"color: red\">" + thisObj.Catalogname + "</div>\n");
                    sb.Append("                <div class=\"marlauto fst20\" style=\"color: red;\">></div>\n");
                    sb.Append("            </div>\n");
                }

                // Get contact info from database
                using (ykmWebDbContext s = new ykmWebDbContext())
                {
                    DalMenuClass dm = new DalMenuClass(s);
                    DalInfo di = new DalInfo(s);
                    var c = dm.find(item => item.Caenname == "ljwm");
                    var contactInfo = (di.find(item => item.classid == c.Catalogid).cont).ToString();
                    StringBuilder sb1 = new StringBuilder();

                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(contactInfo);
                    var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//p");
                    string contactname = "";
                    string mobile = "";
                    string address = "";
                    string telephone = "";
                    foreach (var node in htmlNodes)
                    {
                        if (node.InnerText.IndexOf("联系人") != -1)
                        {
                            contactname = node.InnerText.Substring(4);
                            contactname = contactname.Replace(" ", "");
                            continue;
                        }
                        if (node.InnerText.IndexOf("联系电话") != -1)
                        {
                            mobile = node.InnerText.Substring(5);
                            mobile = mobile.Replace(" ", "");
                            continue;
                        }
                        if (node.InnerText.IndexOf("座机号码") != -1)
                        {
                            telephone = node.InnerText.Substring(5);
                            telephone = telephone.Replace(" ", "");
                            continue;
                        }
                        if (node.InnerText.IndexOf("联系地址") != -1)
                        {
                            address = node.InnerText.Substring(5);
                            address = address.Replace(" ", "");
                            continue;
                        }
                    }
                    sb.Append("        </div>\n");
                    sb.Append("    </div>\n");
                    sb.Append("    <div class=\"martop30\">\n");
                    sb.Append("        <div class=\"flex ai-center padleft16 padright16\" style=\"height:90px;background-color:#444444\">\n");
                    sb.Append("            <div class=\"c-fff\">\n");
                    sb.Append("                <div class=\"fw-bold\" style=\"font-size:24px\">联系我们</div>\n");
                    sb.Append("                <div>CONTACT US</div>\n");
                    sb.Append("            </div>\n");
                    sb.Append("            <button onclick=\"{ window.location.href = '/contactus/index' }\"\n");
                    sb.Append("                style=\"border-radius: 50%; color: #444444; display: flex; justify-content: center; align-items: center; padding: 5px; border: none; background: white; margin-left: auto;\">\n");
                    sb.Append("               <i class=\"fa fa-arrow-right\"></i>\n");
                    sb.Append("            </button>\n");
                    sb.Append("        </div>\n");
                    sb.Append("        <div class=\"padleft16 padright16 padtop24 padbot24 flex flex-column bg-fff\">\n");
                    sb.Append("            <div>联系人： "+ contactname +"</div>\n");
                    sb.Append("            <div class=\"martop8\">联系电话： "+ mobile +"</div>\n");
                    sb.Append("            <div class=\"martop8\">联系地址： "+ address +"</div>\n");
                    sb.Append("            <div class=\"martop8\">座机号码： "+ telephone +"</div>\n");
                    sb.Append("        </div>\n");
                    sb.Append("    </div>\n");
                    sb.Append("</div>\n");
                }
                


                //sb.Append("    <div class=\"title\">" + pCatalogname + "</div>");
                ////var ilist = dm.FindList(n => n.leftnavshow == 1 && n.ParentID == pid, 0, getOrderList()).ToList();
                //sb.Append("<div class=\"yiji\">");
                //sb.Append("<ul>");
                //if (ilist.Count() > 0)
                //{
                //    foreach (var o in ilist)
                //    {
                //        string css = "";
                //        bool isact1 = ishas(checkPIdStr, o.Catalogid.ToString());
                //        if (isact1)
                //        {
                //            css = "on";
                //        }
                //        sb.Append("<li class=" + css + "><a href=\"" + getUrlLink(o) + "\"><div class=\"tit\">" + o.Catalogname + "</div></a>");
                //        if (o.Child > 0 && isact1)
                //        {
                //            var ilist1 = dm.FindList(n => n.ParentID == o.Catalogid, 0, getOrderList()).ToList();
                //            if (ilist1.Count() > 0)
                //            {
                //                sb.Append("<div class=\"erji\">");
                //                sb.Append("<ul>");
                //                foreach (var a in ilist1)
                //                {
                //                    string defstyle1 = "";
                //                    bool isact2 = ishas(checkPIdStr, a.Catalogid.ToString());
                //                    if (isact2 && a.Child == 0)
                //                    {
                //                        defstyle1 = "on";
                //                    }
                //                    sb.Append("<li class=" + defstyle1 + "><a href=\"" + getUrlLink(a) + "\"><div class=\"tit\">" + a.Catalogname + "</div></a></li>");
                //                }
                //                sb.Append("</ul>");
                //                sb.Append("</div>");
                //            }
                //        }
                //        sb.Append("</li>");
                //    }
                //}
                //else
                //{
                //    sb.Append("<li class=\"on\"><a href=\"" + getUrlLink(thisObj) + "\"><div class=\"tit\">" + thisObj.Catalogname + "</div></a></li>");
                //}
                //sb.Append("</ul>");
                //sb.Append("</div>");
            }
            return sb.ToString();
        }

        public string leftMenu_en(int cid = 0)
        {
            //<div class="aside">
            //    <div class="title">走进亨恒</div>
            //    <ul>
            //        <li class="on"><a href="#"><div class="tit">公司简介</div></a></li>
            //        <li><a href="#"><div class="tit">企业文化</div></a></li>
            //        <li><a href="#"><div class="tit">发展历程</div></a></li>
            //        <li><a href="#"><div class="tit">荣誉资质</div></a></li>
            //        <li><a href="#"><div class="tit">企业风采</div></a></li>
            //    </ul>
            //</div>
            DalMenuClass dm = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            var thisObj = dm.FindList(n => n.Catalogid == cid, 1, null).SingleOrDefault();
            if (thisObj != null)
            {
                int pid = thisObj.Catalogid.Value;
                int currCid = thisObj.Catalogid.Value;
                string pCatalogname = thisObj.Catalogname;
                string pEnglish = thisObj.subtitle;
                string checkPIdStr = thisObj.ParentStr + "," + thisObj.Catalogid.Value;
                string pCaenname = thisObj.Caenname;
                if (thisObj.ParentID > 0)
                {
                    int rootid = thisObj.RootID.Value;
                    var objP = dm.FindList(n => n.RootID == rootid && n.ParentID == 0 && n.language == "en", 1, getOrderList()).FirstOrDefault();
                    if (objP != null)
                    {
                        pid = objP.Catalogid.Value;
                        pCatalogname = objP.Catalogname;
                        pEnglish = objP.subtitle;
                        pCaenname = objP.Caenname;
                    }
                }
                sb.Append("<div class=\"aside\">");
                sb.Append("    <div class=\"title\">" + pCatalogname + "</div>");
                var ilist = dm.FindList(n => n.leftnavshow == 1 && n.ParentID == pid, 0, getOrderList()).ToList();
                sb.Append("<ul>");
                if (ilist.Count() > 0)
                {
                    int num = 0;
                    foreach (var o in ilist)
                    {
                        string css = "";
                        bool isact1 = ishas(checkPIdStr, o.Catalogid.ToString());
                        if (isact1)
                        {
                            css = " class=\"on\"";
                        }
                        sb.Append("<li" + css + "><a href=\"" + getUrlLink_en(o) + "\"><div class=\"tit\">" + o.Catalogname + "</div></a></li>");
                        num++;
                    }
                }
                else
                {
                    sb.Append("<li class=\"on\"><a href=\"" + getUrlLink_en(thisObj) + "\"><div class=\"tit\">" + thisObj.Catalogname + "</div></a></li>");
                }
                sb.Append("</ul>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }

        public string subMenu(int cid = 0)
        {
            //<div class="sy_clear">
            //    <div class="sy_btcptit">
            //        <ul>
            //            <li class="sy_on"><a href="#">糙米/精米</a></li>
            //            <li><a href="#">专享商品</a></li>
            //        </ul>
            //    </div>
            //</div>
            DalMenuClass dm = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            var thisObj = dm.FindList(n => n.Catalogid == cid, 1, null).SingleOrDefault();
            if (thisObj != null)
            {
                int pid = thisObj.Catalogid.Value;
                string checkPIdStr = thisObj.ParentStr + "," + thisObj.Catalogid.Value;
                if (thisObj.Depth >= 2)
                {
                    if (thisObj.Depth == 3)
                    {
                        pid = thisObj.ParentID.Value;
                    }
                    else if (thisObj.Depth == 4)
                    {
                        pid = dm.getParentInfo(thisObj).ParentID.Value;
                    }
                    int rootid = thisObj.RootID.Value;
                    var ilist = dm.FindList(n => n.leftnavshow == 1 && n.ParentID == pid && n.RootID == rootid && n.Depth == 3, 0, getOrderList()).ToList();
                    if (ilist.Count() > 0)
                    {
                        sb.Append("<div class=\"sy_clear\"><div class=\"sy_btcptit\"><ul>");
                        int num = 0;
                        foreach (var o in ilist)
                        {
                            string css = "";
                            bool isact1 = ishas(checkPIdStr, o.Catalogid.ToString());
                            if (isact1)// && o.Child == 0
                            {
                                css = " class=\"sy_on\"";
                            }
                            sb.Append("<li" + css + "><a href=\"" + getUrlLink(o) + "\">" + o.Catalogname + "</a></li>");
                            num++;
                        }
                        sb.Append("</ul></div></div>");
                    }
                }
            }
            return sb.ToString();
        }
        public string h5_subMenu(int cid = 0)
        {
            //<div class="navmenu">
            //    <div class="wrapper wrapper03" id="wrapper03">
            //        <div class="scroller">
            //            <ul class="clearfix">
            //                <li class="cur"><a href="javascript:void(0)">公司简介</a></li>
            //                <li><a href="javascript:void(0)">企业文化</a></li>
            //                <li><a href="javascript:void(0)">发展历程</a></li>
            //                <li class="last"><a href="javascript:void(0)">荣誉资质  </a></li>
            //            </ul>
            //        </div>
            //    </div>
            //    <script src="/web_js/navbarscroll.js"></script>
            //    <script src="/web_js/iscroll.js"></script>
            //    <script>$('.wrapper').navbarscroll({defaultSelect:0});</script>
            //</div>
            DalMenuClass dm = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            var thisObj = dm.FindList(n => n.Catalogid == cid, 1, null).SingleOrDefault();
            if (thisObj != null)
            {
                string checkPIdStr = thisObj.ParentStr + "," + thisObj.Catalogid.Value;
                if (thisObj.ParentID != 0)
                {
                    var ilist = dm.FindList(n => n.ParentID == thisObj.ParentID, 0, getOrderList()).ToList();
                    if (ilist.Count() > 0)
                    {
                        sb.Append("<div class=\"navmenu\">");
                        sb.Append("    <div class=\"wrapper wrapper03\" id=\"wrapper03\">");
                        sb.Append("        <div class=\"scroller\">");
                        sb.Append("            <ul class=\"clearfix\">");
                        int num = 0, select = 0;
                        foreach (var o in ilist)
                        {
                            string css = "",last="";
                            bool isact1 = ishas(checkPIdStr, o.Catalogid.ToString());
                            if (isact1)// && o.Child == 0
                            {
                                css = " cur";
                                select = num;
                            }
                            last = (num == ilist.Count - 1) ? "last" : "";
                            sb.Append("<li class=\""+ last + css + "\"><a href=\"" + getMobiUrlLink(o) + "\">" + o.Catalogname + "</a></li>");
                            num++;
                        }
                        sb.Append("            </ul>");
                        sb.Append("        </div>");
                        sb.Append("    </div>");
                        sb.Append("    <script src=\"/web_js/navbarscroll.js\"></script>");
                        sb.Append("    <script src=\"/web_js/iscroll.js\"></script>");
                        sb.Append("    <script>$('.wrapper').navbarscroll({defaultSelect:"+ select + "});</script>");
                        sb.Append("</div>");
                    }
                }
            }
            return sb.ToString();
        }

        public string h5_subMenu_en(int cid = 0)
        {
            //<div class="navmenu">
            //    <div class="wrapper wrapper03" id="wrapper03">
            //        <div class="scroller">
            //            <ul class="clearfix">
            //                <li class="cur"><a href="javascript:void(0)">公司简介</a></li>
            //                <li><a href="javascript:void(0)">企业文化</a></li>
            //                <li><a href="javascript:void(0)">发展历程</a></li>
            //                <li class="last"><a href="javascript:void(0)">荣誉资质  </a></li>
            //            </ul>
            //        </div>
            //    </div>
            //    <script src="/web_js/navbarscroll.js"></script>
            //    <script src="/web_js/iscroll.js"></script>
            //    <script>$('.wrapper').navbarscroll({defaultSelect:0});</script>
            //</div>
            DalMenuClass dm = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            var thisObj = dm.FindList(n => n.Catalogid == cid, 1, null).SingleOrDefault();
            if (thisObj != null)
            {
                string checkPIdStr = thisObj.ParentStr + "," + thisObj.Catalogid.Value;
                if (thisObj.ParentID != 0)
                {
                    var ilist = dm.FindList(n => n.ParentID == thisObj.ParentID, 0, getOrderList()).ToList();
                    if (ilist.Count() > 0)
                    {
                        sb.Append("<div class=\"navmenu\">");
                        sb.Append("    <div class=\"wrapper wrapper03\" id=\"wrapper03\">");
                        sb.Append("        <div class=\"scroller\">");
                        sb.Append("            <ul class=\"clearfix\">");
                        int num = 0, select = 0;
                        foreach (var o in ilist)
                        {
                            string css = "", last = "";
                            bool isact1 = ishas(checkPIdStr, o.Catalogid.ToString());
                            if (isact1)// && o.Child == 0
                            {
                                css = " cur";
                                select = num;
                            }
                            last = (num == ilist.Count - 1) ? "last" : "";
                            sb.Append("<li class=\"" + last + css + "\"><a href=\"" + getMobiUrlLink_en(o) + "\">" + o.Catalogname + "</a></li>");
                            num++;
                        }
                        sb.Append("            </ul>");
                        sb.Append("        </div>");
                        sb.Append("    </div>");
                        sb.Append("    <script src=\"/web_js/navbarscroll.js\"></script>");
                        sb.Append("    <script src=\"/web_js/iscroll.js\"></script>");
                        sb.Append("    <script>$('.wrapper').navbarscroll({defaultSelect:" + select + "});</script>");
                        sb.Append("</div>");
                    }
                }
            }
            return sb.ToString();
        }

        private bool ishas(string currParstr,string exCid)
        {
            string[] arrid = currParstr.Split(',');
            if (arrid.Contains(exCid))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// 手机端导航_可通用
        /// </summary>
        /// <returns></returns>
        public string mobi_nav()
        {
            DalMenuClass dl = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            List<menuClass> l = dl.FindList(n => n.mainnavshow == 1 && n.ParentID == 0, 0, mrorder).ToList();
            if(l.Count() > 0)
            {

                sb.Append("<div class=\"daoh\"><ul class=\"parents\">");
                sb.Append("<li class=\"nLi\"><a href=\"/h5\">首页</a></li>");
                foreach(var o in l)
                {
                    string link = "javascript:void(null)";
                    if(o.Child == 0)
                    {
                        link = getMobiUrlLink(o);
                    }

                    sb.Append("<li class=\"nLi\"><a href=\""+ link + "\">"+o.Catalogname+"</a>");
                    if(o.Child > 0)
                    {
                        var pid = o.Catalogid.Value;
                        var vl = dl.FindList(n => n.ParentID == pid, 0, mrorder).ToList();
                        if(vl.Count > 0)
                        {
                            sb.Append("<ul class=\"sub\">");
                            foreach(var v in vl)
                            {
                                sb.Append("<li><a href=\""+getMobiUrlLink(v)+"\">"+v.Catalogname+"</a></li>");
                            }
                            sb.Append("</ul>");
                        }
                    }
                    sb.Append("</li>");
                }
                sb.Append("</ul><div class=\"bg\"></div></div>");

                sb.Append("<script>\r\n");
                sb.Append("    $(\".header .menu\").click(function() {\r\n");
                sb.Append("        $(\".daoh\").toggle(\"slow\");\r\n");
                sb.Append("    });\r\n");
                sb.Append("$(\".daoh .nLi\").click(function() {\r\n");
                sb.Append("        $(this).addClass(\"on\").siblings().removeClass(\"on\");\r\n");
                sb.Append("        $(this).children(\".sub\").show().parents(\".nLi\").siblings().children(\".sub\").hide();\r\n");
                sb.Append("        $(this).siblings().children(\".sub\").hide();\r\n");
                sb.Append("        if ($(\".daoh .nLi .sub\").css(\"display\") === \"block\") {\r\n");
                sb.Append("            $(\".daoh .bg\").css(\"display\", \"block\");\r\n");
                sb.Append("        } else if ($(\".daoh .nLi .sub\").css(\"display\") === \"none\") {\r\n");
                sb.Append("            $(\".daoh .bg\").hide();\r\n");
                sb.Append("        }\r\n");
                sb.Append("    })\r\n");
                sb.Append("</script>\r\n");
            }
            return sb.ToString();
        }
        public string mobi_top_nav()
        {
            //<li class="nLi"><a href="javascript:;">首页</a></li>
            //<li class="nLi"><a href="javascript:;">我们的故事</a></li>
            //<li class="nLi"><a href="javascript:;">了解我们</a></li>
            //<li class="nLi"><a href="javascript:;">米饭秘籍</a></li>
            //<li class="nLi">
            //    <a href="javascript:;">专享服务</a>
            //    <ul class="sub">
            //        <li><a href="javascript:;">专享服务内容</a></li>
            //        <li>
            //            <a href="javascript:;">商品展示</a>
            //            <ul class="sub1">
            //                <li><a href="javascript:;">糙米/精米</a></li>
            //                <li><a href="javascript:;">专享商品</a></li>
            //            </ul>
            //        </li>
            //    </ul>
            //</li>
            //<li class="nLi"><a href="javascript:;">在线服务</a></li>
            DalMenuClass dl = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            List<menuClass> l = dl.FindList(n => n.mainnavshow == 1 && n.ParentID == 0, 8, mrorder).ToList();
            sb.Append("<li class=\"nLi\"><a href=\"/h5/\">首页</a></li>");
            if (l.Count() > 0)
            {
                foreach (var o in l)
                {
                    string link = "javascript:void(null)";
                    if (o.Child == 0)
                    {
                        link = getMobiUrlLink(o);
                    }
                    sb.Append("<li class=\"nLi\"><a href=\"" + link + "\">" + o.Catalogname + "</a>");
                    if (o.Child > 0)
                    {
                        var pid = o.Catalogid.Value;
                        var vl = dl.FindList(n => n.ParentID == pid, 0, mrorder).ToList();
                        if (vl.Count > 0)
                        {
                            sb.Append("<ul class=\"sub\">");
                            foreach (var v in vl)
                            {
                                string vlink = "javascript:void(null)";
                                if (v.Child == 0)
                                {
                                    vlink = getMobiUrlLink(v);
                                }
                                sb.Append("<li><a href=\"" + vlink + "\">" + v.Catalogname + "</a>");
                                if (v.Child > 0)
                                {
                                    var vpid = v.Catalogid.Value;
                                    var vvl = dl.FindList(n => n.ParentID == vpid, 0, mrorder).ToList();
                                    if (vvl.Count > 0)
                                    {
                                        sb.Append("<ul class=\"sub1\">");
                                        foreach (var vv in vvl)
                                        {
                                            sb.Append("<li><a href=\"" + getMobiUrlLink(vv) + "\">" + vv.Catalogname + "</a></li>");
                                        }
                                        sb.Append("</ul>");
                                    }
                                }
                                sb.Append("</li>");
                            }
                            sb.Append("</ul>");
                        }
                    }
                    sb.Append("</li>");
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// 手机端首页导航，
        /// </summary>
        /// <param name="Ceanneme">标记</param>
        /// <param name="img">传入图片，原样输出</param>
        /// <returns></returns>
        public string mobi_index_nav(string Ceanneme, string img= "/web_images/h5/nav1.jpg")
        {
            DalMenuClass dl = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            var o = dl.FindList(n => n.Caenname.Equals(Ceanneme), 1, mrorder).FirstOrDefault();
            if (o != null)
            {
                sb.Append("<li><a href=\"" + getMobiUrlLink(o) + "\"><img src=\""+ img + "\" alt=\""+o.Catalogname+"\" /><h2>"+o.Catalogname+"</h2></a></li>");
            }
            return sb.ToString();
        }
        public string mobi_index_nav2(string Ceanneme)
        {
            DalMenuClass dl = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            var o = dl.FindList(n => n.Caenname.Equals(Ceanneme), 1, mrorder).FirstOrDefault();
            if (o != null)
            {
                sb.Append("<div class=\"alltitle\"><h2>" + o.Catalogname + "</h2><div class=\"txt\">" + o.subtitle + "</div></div>");
            }
            return sb.ToString();
        }
        public string mobi_child_str(int cid)
        {
            DalMenuClass dm = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            var thisObj = dm.FindList(n => n.Catalogid == cid, 1, null).SingleOrDefault();
            if (thisObj != null)
            {
                int pid = thisObj.Catalogid.Value;
                int currCid = thisObj.Catalogid.Value;
                string pCatalogname = thisObj.Catalogname;
                string checkPIdStr = thisObj.ParentStr + "," + thisObj.Catalogid.Value;
                string pCaenname = thisObj.Caenname;
                if (thisObj.ParentID > 0)
                {
                    int rootid = thisObj.RootID.Value;
                    var objP = dm.FindList(n => n.RootID == rootid && n.ParentID == 0, 1, getOrderList()).FirstOrDefault();
                    if (objP != null)
                    {
                        pid = objP.Catalogid.Value;
                        pCatalogname = objP.Catalogname;
                        pCaenname = objP.Caenname;
                    }
                }
                var ilist = dm.FindList(n => n.ParentID == pid, 0, getOrderList()).ToList();
                if (ilist.Count() > 0)
                {
                    sb.Append("<div class=\"flex ai-center jc-center fw-bold\" style=\"font-size:30px; color: white; height: 60px; background-color:#d40101\">" + pCatalogname + "</div>");
                    sb.Append("<div class=\"navmenu\">");
                    sb.Append("<div class=\"wrapper wrapper02\" id=\"wrapper02\">");
                    sb.Append("<div class=\"scroller\">");
                    sb.Append("<ul class=\"clearfix\">");
                    int num = 0, cur = 0;
                    foreach (var o in ilist)
                    {
                        string defstyle1 = "";
                        if (o.Catalogid.Value == cid)
                        {
                            defstyle1 = " class=\"cur\"";
                            cur = num;
                        }
                        sb.Append("<li" + defstyle1 + "><a href=\"" + getUrlLink(o) + "\">" + o.Catalogname + "</a></li>");
                        num++;
                    }
                    sb.Append("</ul>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<script src=\"web_js/navbarscroll.js\"></script>");
                    sb.Append("<script src=\"web_js/iscroll.js\"></script>");
                    sb.Append("<script>$('#wrapper02').navbarscroll({ defaultSelect:" + cur + "});</script>");
                    sb.Append("</div>");
                }
                else
                {
                    sb.Append("<div class=\"list2\" style=\"width:100%;background:#e50017\">");
                    sb.Append("<div class=\"flex ai-center jc-center fw-bold\" style=\"font-size:30px; color: white; height: 60px; background-color:#d40101\">" + pCatalogname + "</div>");
                    sb.Append("</div>");
                }

            }
            return sb.ToString();
        }

        public string mobi_child_str1(int cid)
        {
            DalMenuClass dm = new DalMenuClass(s);
            StringBuilder sb = new StringBuilder();
            var thisObj = dm.FindList(n => n.Catalogid == cid, 1, null).SingleOrDefault();
            if (thisObj != null)
            {
                int pid = thisObj.Catalogid.Value;
                int currCid = thisObj.Catalogid.Value;
                string pCatalogname = thisObj.Catalogname;
                string checkPIdStr = thisObj.ParentStr + "," + thisObj.Catalogid.Value;
                string pCaenname = thisObj.Caenname;
                int pDepth = thisObj.Depth.Value;
                int pChild = thisObj.Child.Value;
                if (pDepth == 3 && pChild == 0)
                {
                    pid = thisObj.ParentID.Value;
                }
                var ilist = dm.FindList(n => n.ParentID == pid, 0, getOrderList()).ToList();
                if (ilist.Count() > 0)
                {
                    sb.Append("<div class=\"navmenu1\">");
                    sb.Append("<div class=\" wrapper03\" id=\"wrapper03\">");
                    sb.Append("<div class=\"scroller\">");
                    sb.Append("<ul class=\"clearfix\">");
                    int num = 0, cur1 = 0;
                    foreach (var o in ilist)
                    {
                        string defstyle1 = "";
                        if (o.Catalogid.Value == cid)
                        {
                            defstyle1 = " class=\"cur\"";
                            cur1 = num;
                        }
                        sb.Append("<li " + defstyle1 + "><a href=\"" + getUrlLink(o) + "\">" + o.Catalogname + "</a></li>");
                        num++;
                    }
                    sb.Append("</ul>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<script>$('#wrapper03').navbarscroll({  defaultSelect:" + cur1 + " });</script>");
                    sb.Append("</div>");
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 栏目banner图
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public string getLmBanner(menuClass i)
        {
            DalMenuClass dei = new DalMenuClass(s);
            string lmimg = "";
            if (i != null)
            {
                if (string.IsNullOrEmpty(i.defaultpic))
                {
                    var ilist = dei.getParentInfo(i);
                    lmimg = getLmBanner(ilist);
                }
                else
                {
                    lmimg = i.defaultpic;
                }
            }
            //return "<div class=\"banner\" style=\"background: url(" + lmimg + ") no-repeat; background-position:center top;\">&nbsp;</div>";
            return lmimg;
        }
        public string getClassTitle(int cid)
        {
            //<div class="title lvzx">
            //    <a href="javascript:;" class="on">旅游资讯</a>
            //    <a href="javascript:;">旅游攻略</a>
            //</div>
            //<div class="title">
            //    <h3>旅游资讯</h3>
            //</div>
            DalMenuClass dmc = new DalMenuClass(s);
            NavLIst nl = new NavLIst(s);
            StringBuilder sb = new StringBuilder();
            var o = dmc.find(n => n.Catalogid == cid);
            if (dmc.findTopidByCid(cid) == 4)
            {
                var l = dmc.FindList(n => n.ParentID == o.ParentID, 0, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList();

                sb.Append("<div class=\"title lvzx\">");
                if(l.Count()>0)
                {
                    string css = "";
                    foreach (var i in l)
                    {
                        css = ((i.ParentStr + "," + i.Catalogid.ToString()).IndexOf(cid.ToString()) != -1) ? " class=\"on\"" : "";
                        sb.Append("    <a href=\"" + nl.getMobiUrlLink(i) + "\"" + css + ">" + i.Catalogname + "</a>");
                    }
                }
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"title\">");
                sb.Append("    <h3>"+ o.Catalogname + "</h3>");
                sb.Append("</div>");
            }
            return sb.ToString(); ;
        }
        public string getClassTitleH5(int cid)
        {
            //<div class="title lvzx">
            //    <a href="javascript:;" class="on">旅游资讯</a>
            //    <a href="javascript:;">旅游攻略</a>
            //</div>
            //<div class="title">
            //    <h3>旅游资讯</h3>
            //</div>
            DalMenuClass dmc = new DalMenuClass(s);
            NavLIst nl = new NavLIst(s);
            StringBuilder sb = new StringBuilder();
            var o = dmc.find(n => n.Catalogid == cid);
            if (dmc.findTopidByCid(cid) == 4)
            {
                var l = dmc.FindList(n => n.ParentID == o.ParentID, 0, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList();

                sb.Append("<div class=\"title lvzx\">");
                if (l.Count() > 0)
                {
                    string css = "";
                    foreach (var i in l)
                    {
                        css = ((i.ParentStr + "," + i.Catalogid.ToString()).IndexOf(cid.ToString()) != -1) ? " class=\"on\"" : "";
                        sb.Append("    <a href=\"" + nl.getMobiUrlLink(i) + "\"" + css + ">" + i.Catalogname + "</a>");
                    }
                }
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"title\">");
                sb.Append("    <h3>" + o.Catalogname + "</h3>");
                sb.Append("</div>");
            }
            return sb.ToString(); ;
        }


        /// <summary>
        /// banner
        /// </summary>
        /// <param name="l">导航合集</param>
        /// <param name="defCatalogid"></param>
        /// <returns></returns>
        public string getIndexBanner()
        {
            //<div id="slideBox" class="slideBox">
            //    <div class="hd">
            //        <ul></ul>
            //    </div>
            //    <div class="bd">
            //        <ul>
            //            <li style="background: url(/web_images/banner_09.jpg) no-repeat 50%"><a href="#"></a></li>
            //            <li style="background: url(/web_images/banner_09.jpg) no-repeat 50%"><a href="#"></a></li>
            //            <li style="background: url(/web_images/banner_09.jpg) no-repeat 50%"><a href="#"></a></li>
            //        </ul>
            //    </div>
            //    <!-- 下面是前/后按钮代码，如果不需要删除即可 -->
            //    <a class="prev" href="javascript:void(0)"></a>
            //    <a class="next" href="javascript:void(0)"></a>
            //</div>
            //<script type="text/javascript">
            //    jQuery(".slideBox").slide({ mainCell: ".bd ul", autoPlay: true });
            //</script>


            DalGgw dg = new DalGgw(s);
            StringBuilder sb = new StringBuilder();
            var l = dg.FindList(n => n.ggwposition =="banner", 0, new OrderModelField[] { new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
            sb.Append("<div id=\"slideBox\" class=\"slideBox\">");
            sb.Append("<div class=\"hd\">");
            sb.Append("<ul>");
            if (l.Count > 0)
            {
                foreach (var i in l)
                {
                    sb.Append("<li></li>");
                }
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class=\"bd\">");
            sb.Append("<ul>");
            if (l.Count > 0)
            {
                foreach (var i in l)
                {
                    sb.Append("<li><a  href=\"" + i.ggwlink + "\"><img src=\"" + i.imgurl + "\" alt=\"\" /></a></li>");
                }
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<script type=\"text/javascript\">");
            sb.Append("    jQuery(\".slideBox\").slide({ mainCell: \".bd ul\", autoPlay: true });");
            sb.Append("</script>");
            return sb.ToString();
        }

        public string getIndexBanner_sj()
        {
            //  <div id="focus" class="focus">
            //    <div class="hd">
            //        <ul></ul>
            //    </div>
            //    <div class="bd">
            //        <ul>
            //            <li><a href="javascript:;"><img src="/web_images/sj_banner1_02.jpg" /></a></li>
            //            <li><a href="javascript:;"><img src="/web_images/sj_banner1_02.jpg" /></a></li>
            //            <li><a href="javascript:;"><img src="/web_images/sj_banner1_02.jpg" /></a></li>
            //        </ul>
            //    </div>
            //</div>
            //<script type="text/javascript">
            //    TouchSlide({
            //        slideCell: "#focus",
            //        titCell: ".hd ul", //开启自动分页 autoPage:true ，此时设置 titCell 为导航元素包裹层
            //        mainCell: ".bd ul",
            //        effect: "left",
            //        autoPlay: true,//自动播放
            //        autoPage: true, //自动分页
            //        switchLoad: "_src" //切换加载，真实图片路径为"_src"
            //    });
            //</script>


            DalGgw dg = new DalGgw(s);
            StringBuilder sb = new StringBuilder();
            var l = dg.FindList(n => n.ggwposition == "sjbanner", 0, new OrderModelField[] { new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
            sb.Append("<div id=\"focus\" class=\"focus\">");
            sb.Append("<div class=\"hd\">");
            sb.Append("<ul>");
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class=\"bd\">");
            sb.Append("<ul>");
            if (l.Count > 0)
            {
                foreach (var i in l)
                {
                    sb.Append("<li><a  href=\"" + i.ggwlink + "\"><img src=\"" + i.imgurl + "\" alt=\"\" /></a></li>");
                }
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<script type=\"text/javascript\">");
            sb.Append("TouchSlide({");
            sb.Append("slideCell: \"#focus\",");
            sb.Append("titCell: \".hd ul\","); //开启自动分页 autoPage:true ，此时设置 titCell 为导航元素包裹层
            sb.Append("mainCell: \".bd ul\",");
            sb.Append("effect: \"left\",");
            sb.Append("autoPlay: true,");//自动播放
            sb.Append("autoPage: true,"); //自动分页
            sb.Append("switchLoad:\"_src\" ");//切换加载，真实图片路径为"_src"");
            sb.Append("});");
            sb.Append("</script>");
            return sb.ToString();
        }

        public string getIndexBanner_en()
        {
            //<div id="slideBox" class="slideBox">
            //    <div class="hd">
            //        <ul></ul>
            //    </div>
            //    <div class="bd">
            //        <ul>
            //            <li style="background: url(/web_images/banner_09.jpg) no-repeat 50%"><a href="#"></a></li>
            //            <li style="background: url(/web_images/banner_09.jpg) no-repeat 50%"><a href="#"></a></li>
            //            <li style="background: url(/web_images/banner_09.jpg) no-repeat 50%"><a href="#"></a></li>
            //        </ul>
            //    </div>
            //    <!-- 下面是前/后按钮代码，如果不需要删除即可 -->
            //    <a class="prev" href="javascript:void(0)"></a>
            //    <a class="next" href="javascript:void(0)"></a>
            //</div>
            //<script type="text/javascript">
            //    jQuery(".slideBox").slide({ mainCell: ".bd ul", autoPlay: true });
            //</script>


            DalGgw dg = new DalGgw(s);
            StringBuilder sb = new StringBuilder();
            var l = dg.FindList(n => n.ggwposition == "banner_en", 0, new OrderModelField[] { new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
            sb.Append("<div id=\"slideBox\" class=\"slideBox\">");
            sb.Append("    <div class=\"hd\">");
            sb.Append("        <ul></ul>");
            sb.Append("    </div>");
            sb.Append("    <div class=\"bd\">");
            sb.Append("        <ul>");
            if (l.Count > 0)
            {
                foreach (var i in l)
                {
                    sb.Append("            <li style=\"background: url(" + i.imgurl + ") no-repeat 50%\"><a href=\"" + i.ggwlink + "\"></a></li>");
                }
            }
            sb.Append("        </ul>");
            sb.Append("    </div>");
            sb.Append("    <a class=\"prev\" href=\"javascript:void(0)\"></a>");
            sb.Append("    <a class=\"next\" href=\"javascript:void(0)\"></a>");
            sb.Append("</div>");
            sb.Append("<script type=\"text/javascript\">");
            sb.Append("    jQuery(\".slideBox\").slide({ mainCell: \".bd ul\", autoPlay: true });");
            sb.Append("</script>");
            return sb.ToString();
        }

        public string getLmBanner(int cid = 0)
        {
            DalMenuClass dmc = new DalMenuClass(s);
            string lmimg = "";
            var i = dmc.find(n => n.Catalogid == cid);
            if (i != null)
            {
                if (string.IsNullOrEmpty(i.defaultpic))
                {
                    var ilist = dmc.getParentInfo(i);
                    lmimg = getLmBanner(ilist);
                }
                else
                {
                    //lmimg = "background: url(" + i.defaultpic + ")";
                    lmimg = i.defaultpic;
                }
            }
            else
            {
                //lmimg = "background: url(/web_images/banner.jpg)";
                lmimg = "/web_images/banner.jpg";
                
            }

            return "<div class=\"zy_banner\"><img src=\"" + lmimg + "\"></div>";

            //return "<div class=\"banner\" style=\"background: url(" + lmimg + ") no-repeat; background-position:center top;\"></div>";
            //return lmimg;
        }
        

        public string getOneImg(string position = "")
        {
            DalGgw dg = new DalGgw(s);
            string img = "";
            var o = dg.FindList(n => n.ggwposition == position, 1, new OrderModelField[] { new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).SingleOrDefault();
            if(o != null)
            {
                if (string.IsNullOrEmpty(o.ggwlink))
                    img = "<img src=\"" + o.imgurl + "\" alt=\"" + o.title + "\" />";
                else
                    img = "<a href=\"" + o.ggwlink + "\" target=\"_blank\"><img src=\"" + o.imgurl + "\" alt=\"" + o.title + "\" /></a>";
            }
            return img;
        }

        public string getImgList(string position = "")
        {
            DalGgw dg = new DalGgw(s);
            StringBuilder sb = new StringBuilder();
            var l = dg.FindList(n => n.ggwposition == position, 0, new OrderModelField[] { new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
            if (l.Count>0)
            {
                foreach (var o in l)
                {
                    if (string.IsNullOrEmpty(o.ggwlink))
                        //sb.Append("<div class=\"img\"><img src=\"" + o.imgurl + "\" alt=\"" + o.title + "\" /><br>" + o.title + "</div>");
                    sb.Append("<li><img src=\"" + o.imgurl + "\"></li>");
                    else
                        sb.Append("<li><img src=\"" + o.imgurl + "\"></li>");
                    //sb.Append("<div class=\"img\"><a href=\"" + o.ggwlink + "\" target=\"_blank\"><img src=\"" + o.imgurl + "\" alt=\"" + o.title + "\" /><br>" + o.title + "</a></div>");
                }
            }
            return sb.ToString();
        }
    }
}