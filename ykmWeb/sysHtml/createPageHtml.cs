using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ykmWeb.Bll;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;
using ykmWeb.Models;

namespace ykmWeb.sysHtml
{
    public class createPageHtml
    {

        /// <summary>
        /// 传入页面URL
        /// </summary>
        public string pageUrl { get; set; }
        /// <summary>
        /// 生成的年份
        /// </summary>
        /// <param name="defYear"></param>
        /// <returns></returns>
        public string SearchhtmlYear(int defYear)
        {
            StringBuilder sb = new StringBuilder();
            int yearData = DateTime.Now.Year;
            int startYear = 2015;
            while (yearData >= startYear)
            {
                string defCss = "";
                if (yearData == defYear)
                {
                    if (startYear == yearData)
                    {
                        defCss = " class=\"on last\"";
                    }
                    else
                    {
                        defCss = " class=\"on\"";
                    }
                }
                else
                {
                    if (startYear == yearData)
                    {
                        defCss = " class=\"last\"";
                    }
                }
                sb.Append(" <li " + defCss + " ><a href=\"" + common.common.JoinUrl(pageUrl, "y=" + yearData) + "\">" + yearData + "</a></li>");
                yearData--;
            }
            return sb.ToString();
        }

        #region 显示页码
        /// <summary>
        /// 显示页码
        /// </summary>
        /// <param name="totle">总记录数</param>
        /// <param name="pagesize">每页显示多少条</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pagename">页面名称</param>
        /// <param name="pageLinkText">查询参数</param>
        /// <param name="isshownum">是否显示数字页码</param>
        /// <param name="showte">是否显示首页和尾页</param>
        /// <returns></returns>
        public string PageFoot(int totle, int pagesize, string pageIndex, string pagename, string pageLinkText, bool isshownum, bool showte)
        {
                //<div class="page">
                //            <ul>
                //                <li class="ye"><a href="#">首页</a></li>
                //                <li class="ye"><a href="#">上一页</a></li>
                //                <li class="on"><a href="#">1</a></li>
                //                <li><a href="#">2</a></li>
                //                <li><a href="#">3</a></li>
                //                <li class="ye"><a href="#">下一页</a></li>
                //                <li class="ye"><a href="#">尾页</a></li>
                //            </ul>
                //        </div>
            StringBuilder sb = new StringBuilder();
            if (totle > 0)
            {
                sb.Append("<div class=\"page\"><ul>");
                string activeStyle = "";
                int CurPage;
                int pagecont = 0;
                if (totle % pagesize == 0)
                {
                    pagecont = totle / pagesize;
                }
                else
                {
                    pagecont = totle / pagesize + 1;
                }
                if (common.common.IsNumeric(pageIndex) == "0")
                {
                    pageIndex = "1";
                }
                if (common.common.IsNumeric(pageIndex) != "0")
                {
                    CurPage = Convert.ToInt32(pageIndex);
                }
                else
                {
                    CurPage = 1;
                }
                if (CurPage > 1)
                {
                    if (showte == true)
                    {
                        sb.Append("<li class=\"ye\"><a href=\"" + pagename + "?pageid=1" + pageLinkText + "\">首页</a></li>");
                    }
                    sb.Append("<li class=\"ye\"><a href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage - 1) + pageLinkText + "\" >上一页</a></li>");
                }
                else
                {
                    if (showte == true)
                    {
                        sb.Append("<li class=\"ye\"><a href=\"javascript:void(null)\">首页</a></li>");
                    }

                    sb.Append("<li class=\"ye\"><a href=\"javascript:void(null)\" >上一页</a></li>");
                }
                if (isshownum == true)
                {
                    if (pagecont <= 6)//如果页数小于等于6
                    {
                        for (int i = 1; i <= pagecont; i++)
                        {
                            if (i == CurPage)
                            {
                                activeStyle = "class=\"on\"";
                            }
                            else
                            {
                                activeStyle = "";
                            }
                            sb.Append("<li " + activeStyle + " ><a href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\">" + i.ToString() + "</a></li>");
                        }
                    }
                    else
                    {
                        int csid, jsid;
                        csid = CurPage - 3;
                        if (csid <= 0)
                        {
                            csid = 1;
                        }
                        jsid = csid + 6;

                        if (jsid > pagecont)
                        {
                            jsid = pagecont;
                        }
                        for (int i = csid; i <= jsid; i++)
                        {
                            if (CurPage == i)
                            {

                                activeStyle = "class=\"on\"";

                                sb.Append("<li " + activeStyle + " ><a href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\"  >" + i.ToString() + "</a></li>");
                            }
                            else
                            {
                                sb.Append("<li><a href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\">" + i.ToString() + "</a></li>");
                            }
                        }

                        if (pagecont >= 18 && CurPage < (pagecont - 7))
                        {

                            for (int i = pagecont - 2; i <= pagecont; i++)
                            {
                                if (i == CurPage)
                                {
                                    activeStyle = "class=\"on\"";
                                }
                                else
                                {
                                    activeStyle = "";
                                }
                                sb.Append("<li " + activeStyle + " ><a href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\" " + activeStyle + " >" + i.ToString() + "</a></li>");
                            }
                        }
                    }
                }
                if (CurPage < pagecont)
                {
                    sb.Append("<li class=\"ye\"><a href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage + 1) + pageLinkText + "\">下一页</a></li>");
                    if (showte == true)
                    {
                        sb.Append("<li class=\"ye\"><a href=\"" + pagename + "?pageid=" + pagecont.ToString() + pageLinkText + "\">尾页</a></li>");
                    }
                }
                else
                {
                    sb.Append("<li class=\"ye\"><a href=\"javascript:void(null)\">下一页</a></li>");
                    if (showte == true)
                    {
                        sb.Append("<li class=\"ye\"><a href=\"javascript:void(null)\">尾页</a></li>");
                    }
                }
                sb.Append("</ul>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }

        public string PageFoot_en(int totle, int pagesize, string pageIndex, string pagename, string pageLinkText, bool isshownum, bool showte)
        {
            //<div class="page">
            //    <ul>
            //        <li class="ye"><a href="#">首页</a></li>
            //        <li class="ye"><a href="#">上一页</a></li>
            //        <li><a href="#">1</a></li>
            //        <li><a href="#">2</a></li>
            //        <li><a href="#">3</a></li>
            //        <li><a href="#">4</a></li>
            //        <li class="mei"><a href="#">.....</a></li>
            //        <li><a href="#">15</a></li>
            //        <li class="ye"><a href="#">下一页</a></li>
            //        <li class="ye"><a href="#">尾页</a></li>
            //    </ul>
            //    <div class="tz">
            //        <div class="wenzi">跳转至第</div>
            //        <input type="number">
            //        <div class="wenzi">页 </div>
            //        <div class="btn"><a href="#">GO</a></div>
            //    </div>
            //</div>
            StringBuilder sb = new StringBuilder();
            if (totle > 0)
            {
                sb.Append("<div class=\"page\"><ul>");
                string activeStyle = "";
                int CurPage;
                int pagecont = 0;
                if (totle % pagesize == 0)
                {
                    pagecont = totle / pagesize;
                }
                else
                {
                    pagecont = totle / pagesize + 1;
                }
                if (common.common.IsNumeric(pageIndex) == "0")
                {
                    pageIndex = "1";
                }
                if (common.common.IsNumeric(pageIndex) != "0")
                {
                    CurPage = Convert.ToInt32(pageIndex);
                }
                else
                {
                    CurPage = 1;
                }
                if (CurPage > 1)
                {
                    if (showte == true)
                    {
                        sb.Append("<li class=\"ye\"><a href=\"" + pagename + "?pageid=1" + pageLinkText + "\">home</a></li>");
                    }
                    sb.Append("<li class=\"ye\"><a href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage - 1) + pageLinkText + "\" >previous</a></li>");
                }
                else
                {
                    if (showte == true)
                    {
                        sb.Append("<li class=\"ye\"><a href=\"javascript:void(null)\">home</a></li>");
                    }

                    sb.Append("<li class=\"ye\"><a href=\"javascript:void(null)\" >previous</a></li>");
                }
                if (isshownum == true)
                {
                    if (pagecont <= 6)//如果页数小于等于6
                    {
                        for (int i = 1; i <= pagecont; i++)
                        {
                            if (i == CurPage)
                            {
                                activeStyle = "class=\"on\"";
                            }
                            else
                            {
                                activeStyle = "";
                            }
                            sb.Append("<li " + activeStyle + " ><a href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\">" + i.ToString() + "</a></li>");
                        }
                    }
                    else
                    {
                        int csid, jsid;
                        csid = CurPage - 3;
                        if (csid <= 0)
                        {
                            csid = 1;
                        }
                        jsid = csid + 6;

                        if (jsid > pagecont)
                        {
                            jsid = pagecont;
                        }
                        for (int i = csid; i <= jsid; i++)
                        {
                            if (CurPage == i)
                            {

                                activeStyle = "class=\"on\"";

                                sb.Append("<li " + activeStyle + " ><a href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\"  >" + i.ToString() + "</a></li>");
                            }
                            else
                            {
                                sb.Append("<li><a href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\">" + i.ToString() + "</a></li>");
                            }
                        }

                        if (pagecont >= 18 && CurPage < (pagecont - 7))
                        {

                            for (int i = pagecont - 2; i <= pagecont; i++)
                            {
                                if (i == CurPage)
                                {
                                    activeStyle = "class=\"on\"";
                                }
                                else
                                {
                                    activeStyle = "";
                                }
                                sb.Append("<li " + activeStyle + " ><a href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\" " + activeStyle + " >" + i.ToString() + "</a></li>");
                            }
                        }
                    }
                }
                if (CurPage < pagecont)
                {
                    sb.Append("<li class=\"ye\"><a href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage + 1) + pageLinkText + "\">next</a></li>");
                    if (showte == true)
                    {
                        sb.Append("<li class=\"ye\"><a href=\"" + pagename + "?pageid=" + pagecont.ToString() + pageLinkText + "\">last</a></li>");
                    }
                }
                else
                {
                    sb.Append("<li class=\"ye\"><a href=\"javascript:void(null)\">next</a></li>");
                    if (showte == true)
                    {
                        sb.Append("<li class=\"ye\"><a href=\"javascript:void(null)\">last</a></li>");
                    }
                }
                sb.Append("</ul>");
                sb.Append("    <div class=\"tz\">");
                sb.Append("        <div class=\"wenzi\">Jump to</div>");
                sb.Append("        <input id=\"pageid\" name=\"pageid\" value=\"1\" type=\"number\" min=\"1\" max=\"" + pagecont + "\">");
                sb.Append("        <div class=\"wenzi\">page </div>");
                sb.Append("        <div class=\"btn\"><a href=\"javascript:;\" onclick=\"jump()\">GO</a></div>");
                sb.Append("    </div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }

        public string PageFootAjax(int totle, int pagesize, string pageIndex, string pagename, string pageLinkText, bool isshownum, bool showte)
        {
            //<div class="page">
            //    <ul>
            //        <li><a href="jvascript:;"><</a></li>
            //        <li class="on"><a href="jvascript:;">1</a></li>
            //        <li><a href="jvascript:;">2</a></li>
            //        <li><a href="jvascript:;">3</a></li>
            //        <li><a href="jvascript:;">4</a></li>
            //        <li><a href="jvascript:;">5</a></li>
            //        <li><a href="jvascript:;">6</a></li>
            //        <li><a href="jvascript:;">7</a></li>
            //        <li><a href="jvascript:;">...</a></li>
            //        <li><a href="jvascript:;">99</a></li>
            //        <li><a href="jvascript:;">100</a></li>
            //        <li><a href="jvascript:;">></a></li>
            //    </ul>
            //</div>
            StringBuilder sb = new StringBuilder();
            //sb.Append("<div class=\"page\"><ul>");
            if (totle > 0)
            {
                string activeStyle = "";
                int CurPage;
                int pagecont = 0;
                if (totle % pagesize == 0)
                {
                    pagecont = totle / pagesize;
                }
                else
                {
                    pagecont = totle / pagesize + 1;
                }
                if (common.common.IsNumeric(pageIndex) == "0")
                {
                    pageIndex = "1";
                }
                if (common.common.IsNumeric(pageIndex) != "0")
                {
                    CurPage = Convert.ToInt32(pageIndex);
                }
                else
                {
                    CurPage = 1;
                }
                if (CurPage > 1)
                {
                    if (showte == true)
                    {
                        sb.Append("<li class=\"sy_ye\"><a href=\"" + pagename + "?pageid=1" + pageLinkText + "\">首页</a></li>");
                    }
                    // href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage - 1) + pageLinkText + "\"
                    sb.Append("<li><a onclick=\"turnPage("+ Convert.ToString(CurPage - 1) + ",'"+ pageLinkText + "')\"><</a></li>");
                }
                else
                {
                    if (showte == true)
                    {
                        sb.Append("<li class=\"sy_ye\"><a href=\"javascript:void(null)\">首页</a></li>");
                    }

                    sb.Append("<li><a href=\"javascript:void(null)\" ><</a></li>");
                }
                if (isshownum == true)
                {
                    if (pagecont <= 6)//如果页数小于等于6
                    {
                        for (int i = 1; i <= pagecont; i++)
                        {
                            if (i == CurPage)
                            {
                                activeStyle = "class=\"on\"";
                            }
                            else
                            {
                                activeStyle = "";
                            }
                            // href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\"
                            sb.Append("<li " + activeStyle + " ><a onclick=\"turnPage(" + i.ToString() + ",'" + pageLinkText + "')\">" + i.ToString() + "</a></li>");
                        }
                    }
                    else
                    {
                        int csid, jsid;
                        csid = CurPage - 3;
                        if (csid <= 0)
                        {
                            csid = 1;
                        }
                        jsid = csid + 6;

                        if (jsid > pagecont)
                        {
                            jsid = pagecont;
                        }
                        for (int i = csid; i <= jsid; i++)
                        {
                            if (CurPage == i)
                            {
                                activeStyle = "class=\"on\"";
                                // href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\"
                                sb.Append("<li " + activeStyle + " ><a onclick=\"turnPage(" + i.ToString() + ",'" + pageLinkText + "')\">" + i.ToString() + "</a></li>");
                            }
                            else
                            {
                                // href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\"
                                sb.Append("<li><a onclick=\"turnPage(" + i.ToString() + ",'" + pageLinkText + "')\">" + i.ToString() + "</a></li>");
                            }
                        }

                        if (pagecont >= 18 && CurPage < (pagecont - 7))
                        {

                            for (int i = pagecont - 2; i <= pagecont; i++)
                            {
                                if (i == CurPage)
                                {
                                    activeStyle = "class=\"on\"";
                                }
                                else
                                {
                                    activeStyle = "";
                                }
                                // href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\"
                                sb.Append("<li " + activeStyle + " ><a onclick=\"turnPage(" + i.ToString() + ",'" + pageLinkText + "')\">" + i.ToString() + "</a></li>");
                            }
                        }
                    }
                }
                if (CurPage < pagecont)
                {
                    // href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage + 1) + pageLinkText + "\"
                    sb.Append("<li><a onclick=\"turnPage(" + Convert.ToString(CurPage + 1) + ",'" + pageLinkText + "')\">></a></li>");
                    if (showte == true)
                    {
                        sb.Append("<li class=\"sy_ye\"><a href=\"" + pagename + "?pageid=" + pagecont.ToString() + pageLinkText + "\">尾页</a></li>");
                    }
                }
                else
                {
                    sb.Append("<li><a href=\"javascript:void(null)\">></a></li>");
                    if (showte == true)
                    {
                        sb.Append("<li class=\"sy_ye\"><a href=\"javascript:void(null)\">尾页</a></li>");
                    }
                }
            }
            //sb.Append("</ul></div>");
            return sb.ToString();
        }
        public string PageFootH5(int totle, int pagesize, string pageIndex, string pagename, string pageLinkText, bool isshownum, bool showte)
        {
            //<div class="page">
            //    <ul>
            //        <li><a href="#"><</a></li>
            //        <li class="on"><a href="#">1</a></li>
            //        <li><a href="#">2</a></li>
            //        <li><a href="#">...</a></li>
            //        <li><a href="#">98</a></li>
            //        <li><a href="#">99</a></li>
            //        <li><a href="#">></a></li>
            //    </ul>
            //</div>
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"page\">");
            if (totle > 0)
            {
                sb.Append("<ul>");
                string activeStyle = "";
                int CurPage;
                int pagecont = 0;
                if (totle % pagesize == 0)
                {
                    pagecont = totle / pagesize;
                }
                else
                {
                    pagecont = totle / pagesize + 1;
                }
                if (common.common.IsNumeric(pageIndex) == "0")
                {
                    pageIndex = "1";
                }
                if (common.common.IsNumeric(pageIndex) != "0")
                {
                    CurPage = Convert.ToInt32(pageIndex);
                }
                else
                {
                    CurPage = 1;
                }
                if (CurPage > 1)
                {
                    if (showte == true)
                    {
                        sb.Append("<li><a href=\"" + pagename + "?pageid=1" + pageLinkText + "\">首页</a></li>");
                    }
                    sb.Append("<li><a href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage - 1) + pageLinkText + "\" ><</a></li>");
                }
                else
                {
                    if (showte == true)
                    {
                        sb.Append("<li><a href=\"javascript:void(null)\">首页</a></li>");
                    }
                    sb.Append("<li><a href=\"javascript:void(null)\" ><</a></li>");
                }
                if (isshownum == true)
                {
                    if (pagecont <= 3)//如果页数小于等于6
                    {
                        for (int i = 1; i <= pagecont; i++)
                        {
                            if (i == CurPage)
                            {
                                activeStyle = "class=\"on\"";
                            }
                            else
                            {
                                activeStyle = "";
                            }
                            sb.Append("<li " + activeStyle + " ><a href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\"  >" + i.ToString() + "</a></li>");
                        }
                    }
                    else
                    {
                        int csid, jsid;
                        csid = CurPage - 2;
                        if (csid <= 0)
                        {
                            csid = 1;
                        }
                        jsid = csid + 3;

                        if (jsid > pagecont)
                        {
                            jsid = pagecont;
                        }
                        for (int i = csid; i <= jsid; i++)
                        {
                            if (CurPage == i)
                            {

                                activeStyle = "class=\"on\"";

                                sb.Append("<li " + activeStyle + " ><a href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\"  >" + i.ToString() + "</a></li>");
                            }
                            else
                            {
                                sb.Append("<li><a href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\">" + i.ToString() + "</a></li>");
                            }
                        }

                        if (pagecont >= 18 && CurPage < (pagecont - 7))
                        {

                            for (int i = pagecont - 2; i <= pagecont; i++)
                            {
                                if (i == CurPage)
                                {
                                    activeStyle = "class=\"on\"";
                                }
                                else
                                {
                                    activeStyle = "";
                                }
                                sb.Append("<li " + activeStyle + " ><a href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\" " + activeStyle + " >" + i.ToString() + "</a></li>");
                            }
                        }
                    }
                }
                else
                {
                    sb.Append("<li class=\"sy_bt\">|</li>");
                }
                if (CurPage < pagecont)
                {
                    sb.Append("<li><a href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage + 1) + pageLinkText + "\">></a></li>");
                    if (showte == true)
                    {
                        sb.Append("<li><a href=\"" + pagename + "?pageid=" + pagecont.ToString() + pageLinkText + "\">尾页</a></li>");
                    }
                }
                else
                {
                    sb.Append("<li><a href=\"javascript:void(null)\" >></a></li>");
                    if (showte == true)
                    {
                        sb.Append("<li><a href=\"javascript:void(null)\">尾页</a></li>");
                    }
                }
                sb.Append("</ul>");
            }
            sb.Append("</div>");
            return sb.ToString();
        }
        public string PageFootAjaxH5(int totle, int pagesize, string pageIndex, string pagename, string pageLinkText, bool isshownum, bool showte)
        {
            //<div class="page">
            //    <ul>
            //        <li><a href="#"><</a></li>
            //        <li class="on"><a href="#">1</a></li>
            //        <li><a href="#">2</a></li>
            //        <li><a href="#">...</a></li>
            //        <li><a href="#">98</a></li>
            //        <li><a href="#">99</a></li>
            //        <li><a href="#">></a></li>
            //    </ul>
            //</div>
            StringBuilder sb = new StringBuilder();
            //sb.Append("<div class=\"page\"><ul>");
            if (totle > 0)
            {
                string activeStyle = "";
                int CurPage;
                int pagecont = 0;
                if (totle % pagesize == 0)
                {
                    pagecont = totle / pagesize;
                }
                else
                {
                    pagecont = totle / pagesize + 1;
                }
                if (common.common.IsNumeric(pageIndex) == "0")
                {
                    pageIndex = "1";
                }
                if (common.common.IsNumeric(pageIndex) != "0")
                {
                    CurPage = Convert.ToInt32(pageIndex);
                }
                else
                {
                    CurPage = 1;
                }
                if (CurPage > 1)
                {
                    if (showte == true)
                    {
                        sb.Append("<li class=\"sy_ye\"><a href=\"" + pagename + "?pageid=1" + pageLinkText + "\">首页</a></li>");
                    }
                    // href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage - 1) + pageLinkText + "\"
                    sb.Append("<li><a onclick=\"turnPageH5(" + Convert.ToString(CurPage - 1) + ",'" + pageLinkText + "')\"><</a></li>");
                }
                else
                {
                    if (showte == true)
                    {
                        sb.Append("<li class=\"sy_ye\"><a href=\"javascript:void(null)\">首页</a></li>");
                    }

                    sb.Append("<li><a href=\"javascript:void(null)\" ><</a></li>");
                }
                if (isshownum == true)
                {
                    if (pagecont <= 3)//如果页数小于等于6
                    {
                        for (int i = 1; i <= pagecont; i++)
                        {
                            if (i == CurPage)
                            {
                                activeStyle = "class=\"on\"";
                            }
                            else
                            {
                                activeStyle = "";
                            }
                            // href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\"
                            sb.Append("<li " + activeStyle + " ><a onclick=\"turnPageH5(" + i.ToString() + ",'" + pageLinkText + "')\">" + i.ToString() + "</a></li>");
                        }
                    }
                    else
                    {
                        int csid, jsid;
                        csid = CurPage - 2;
                        if (csid <= 0)
                        {
                            csid = 1;
                        }
                        jsid = csid + 3;

                        if (jsid > pagecont)
                        {
                            jsid = pagecont;
                        }
                        for (int i = csid; i <= jsid; i++)
                        {
                            if (CurPage == i)
                            {
                                activeStyle = "class=\"on\"";
                                // href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\"
                                sb.Append("<li " + activeStyle + " ><a onclick=\"turnPageH5(" + i.ToString() + ",'" + pageLinkText + "')\">" + i.ToString() + "</a></li>");
                            }
                            else
                            {
                                // href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\"
                                sb.Append("<li><a onclick=\"turnPageH5(" + i.ToString() + ",'" + pageLinkText + "')\">" + i.ToString() + "</a></li>");
                            }
                        }

                        if (pagecont >= 18 && CurPage < (pagecont - 7))
                        {

                            for (int i = pagecont - 2; i <= pagecont; i++)
                            {
                                if (i == CurPage)
                                {
                                    activeStyle = "class=\"on\"";
                                }
                                else
                                {
                                    activeStyle = "";
                                }
                                // href=\"" + pagename + "?pageid=" + i.ToString() + pageLinkText + "\"
                                sb.Append("<li " + activeStyle + " ><a onclick=\"turnPageH5(" + i.ToString() + ",'" + pageLinkText + "')\">" + i.ToString() + "</a></li>");
                            }
                        }
                    }
                }
                if (CurPage < pagecont)
                {
                    // href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage + 1) + pageLinkText + "\"
                    sb.Append("<li><a onclick=\"turnPageH5(" + Convert.ToString(CurPage + 1) + ",'" + pageLinkText + "')\">></a></li>");
                    if (showte == true)
                    {
                        sb.Append("<li class=\"sy_ye\"><a href=\"" + pagename + "?pageid=" + pagecont.ToString() + pageLinkText + "\">尾页</a></li>");
                    }
                }
                else
                {
                    sb.Append("<li><a href=\"javascript:void(null)\">></a></li>");
                    if (showte == true)
                    {
                        sb.Append("<li class=\"sy_ye\"><a href=\"javascript:void(null)\">尾页</a></li>");
                    }
                }
            }
            //sb.Append("</ul></div>");
            return sb.ToString();
        }
        public string LoadMore(int pn,string t,string uid) {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\" src=\"/web_js/fanye.js\"></script>");
            sb.Append("<div class=\"more\"><a href=\"javascript:fanye(" + pn + ", '" + t + "','" + uid + "');\">点击加载更多</a></div>");
            return sb.ToString();
        }
        #endregion

        #region 显示页码2
        /// <summary>
        /// 显示页码
        /// </summary>
        /// <param name="totle">总记录数</param>
        /// <param name="pagesize">每页显示多少条</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pagename">页面名称</param>
        /// <param name="pageLinkText">查询参数</param>
        /// <returns></returns>
        public string PageFootNext(int totle, int pagesize, string pageIndex, string pagename, string pageLinkText)
        {
            StringBuilder sb = new StringBuilder();
            if (totle > 0)
            {
                int CurPage;
                int pagecont = 0;
                if (totle % pagesize == 0)
                {
                    pagecont = totle / pagesize;
                }
                else
                {
                    pagecont = totle / pagesize + 1;
                }
                if (common.common.IsNumeric(pageIndex) == "0")
                {
                    pageIndex = "1";
                }
                if (common.common.IsNumeric(pageIndex) != "0")
                {
                    CurPage = Convert.ToInt32(pageIndex);
                }
                else
                {
                    CurPage = 1;
                }
                if (CurPage > 1)
                {
                    sb.Append("<li  class=\"prev\"><a href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage - 1) + pageLinkText + "\" >上一页</a></li>");
                }
                else
                {
                    sb.Append("<li  class=\"prev\"><a href=\"javascript:void(null)\" >上一页</a></li>");
                }

                if (CurPage < pagecont)
                {
                    sb.Append("<li class=\"next\"><a href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage + 1) + pageLinkText + "\">下一页</a></li>");
                }
                else
                {
                    sb.Append("<li class=\"next\" ><a href=\"javascript:void(null)\" >下一页</a></li>");
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 显示页码手机
        /// <summary>
        /// 显示页码
        /// </summary>
        /// <param name="totle">总记录数</param>
        /// <param name="pagesize">每页显示多少条</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pagename">页面名称</param>
        /// <param name="pageLinkText">查询参数</param>
        /// <returns></returns>
        public string PageMobiFootNext(int totle, int pagesize, string pageIndex, string pagename, string pageLinkText)
        {
            StringBuilder sb = new StringBuilder();
            if (totle > 0)
            {
                int CurPage;
                int pagecont = 0;
                if (totle % pagesize == 0)
                {
                    pagecont = totle / pagesize;
                }
                else
                {
                    pagecont = totle / pagesize + 1;
                }
                if (common.common.IsNumeric(pageIndex) == "0")
                {
                    pageIndex = "1";
                }
                if (common.common.IsNumeric(pageIndex) != "0")
                {
                    CurPage = Convert.ToInt32(pageIndex);
                }
                else
                {
                    CurPage = 1;
                }
                if (CurPage > 1)
                {
                    sb.Append("<a href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage - 1) + pageLinkText + "\" >上一页</a>");
                }
                else
                {
                    sb.Append("<a href=\"javascript:void(null)\" >上一页</a>");
                }

                if (CurPage < pagecont)
                {
                    sb.Append("<a href=\"" + pagename + "?pageid=" + Convert.ToString(CurPage + 1) + pageLinkText + "\">下一页</a>");
                }
                else
                {
                    sb.Append("<a href=\"javascript:void(null)\" >下一页</a>");
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 列表显示形式
        public string list(List<ykmWeb.Models.info> l)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count() > 0)
            {
                int i = 1;
                sb.Append("<div class=\"money\"><ul>");
                foreach (var o in l)
                {
                    //sb.Append("<li><a href=\"/cont?id=" + o.id + "\"><h3>" + o.title + "</h3><div class=\"date\">" + o.infodate.Value.Month + "-" + o.infodate.Value.Day + "</div></a></li>");
                    if (i % 5 == 0)
                    {
                        sb.Append("</ul>");
                        sb.Append("<ul>");
                    }
                    i++;
                }
                sb.Append("</ul></div>");
            }
            return sb.ToString();
        }
        #endregion

        #region 图文形式
        public string listImgCont(List<Models.info> l)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count() > 0)
            {
                int i = 1;
                sb.Append("<div class=\"monopoly\"><ul>");
                foreach (var o in l)
                {
                    sb.Append("<li><a href=\"/cont?id=" + o.id + "\"class=\"left\"><img src=\"" + o.defaultpic + "\"alt=\"\"/></a><a href=\"/cont?id=" + o.id + "\"class=\"wenzi\"><h2>" + o.title + "</h2><div class=\"txt\"></div><div class=\"date\"></div></a></li>");
                    i++;
                }
                sb.Append("</ul></div>");
            }
            return sb.ToString();
        }
        #endregion

        #region 图片列表
        public string imageList(List<Models.info> l)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count() > 0)
            {
                sb.Append("<div class=\"one\">");
                sb.Append("<ul>");
                foreach (var o in l)
                {
                    sb.Append("<li><a href=\"/cont?id=" + o.id + "\"><img src=\"" + o.defaultpic + "\"alt=\"\"/><h3>" + o.title + "</h3></a></li>");
                }
                sb.Append("</ul></div>");
            }
            return sb.ToString();
        }
        #endregion

        #region 详情
        public string listContent(Models.info i)
        {
            StringBuilder sb = new StringBuilder();
            if (i != null)
            {
                sb.Append("<div class=\"conwenzi\">");
                //    sb.Append("<h3>中共丹东市风光无限网站专卖局机关委员会党员积分评分表</h3>");
                //      sb.Append("<div class=\"date\">发布时间：2019-3-28<span class=\"ren\">发布人：管理员</span></div>");

                //sb.Append("<div class=\"txt\">" + i.jj + "</div>");


                sb.Append("</div>");
                if (i.classid == 14)
                {
                    sb.Append("<iframe src=\"/map.html\" width=\"100%\" height=\"600\" frameborder=\"0\"></iframe>");
                }

            }
            return sb.ToString();
        }
        #endregion


        public string createLInk(Dictionary<string, string> dr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var o in dr)
            {
                sb.Append("&" + o.Key + "=" + o.Value);
            }
            return sb.ToString();
        }

        private string setLinkVal(Dictionary<string, string> dr, string key, string val)
        {
            dr[key] = val;
            return createLInk(dr);
        }

        private string getSearCode(string conStr)
        {
            switch (conStr)
            {
                default:
                    return "m";
                case "mjstr":
                    return "m";
                case "ftstr":
                    return "f";
                case "hxstr":
                    return "h";
                case "qystr":
                    return "q";
            }
        }

        public string createSearch(Dictionary<string, string> dr, string conStr, string val)
        {
            StringBuilder sb = new StringBuilder();
            ykmWeb.Bll.user_config_type us = new Bll.user_config_type(conStr);
            var list = us.getData(true);
            if (list.Count() > 0)
            {
                int i = 1;
                foreach (var o in list)
                {
                    string css1 = "";

                    if (i == 1)
                    {
                        css1 = "first ";
                    }

                    if (val == o.Key)
                    {
                        css1 = css1 + " on";
                    }

                    if (i == list.Count())
                    {
                        css1 = css1 + " last";
                    }

                    sb.Append("<li  class=\"" + css1 + "\" ><a href=\"/an?pageid=1" + setLinkVal(dr, getSearCode(conStr), o.Key) + "\">" + o.Value + "</a></li>");
                    i++;
                }
            }
            dr[getSearCode(conStr)] = val;//恢复
            return sb.ToString();
        }


        public string createSearchMobi(Dictionary<string, string> dr, string conStr, string val, string key)
        {
            StringBuilder sb = new StringBuilder();
            ykmWeb.Bll.user_config_type us = new Bll.user_config_type(conStr);
            var list = us.getData(true);
            if (list.Count() > 0)
            {
                sb.Append("<select name=\"" + key + "\" id=\"" + key + "\" onchange=\"changepage('" + createLInk(dr) + "',this,'" + key + "')\"> ");
                int i = 1;
                foreach (var o in list)
                {
                    string selected = "";

                    if (o.Key == val)
                    {
                        selected = "selected";
                    }
                    sb.Append("<option  value=\"" + o.Key + "\" " + selected + "  >" + o.Value + "</option>");
                    i++;
                }
                sb.Append("</select>");
            }
            dr[getSearCode(conStr)] = val;//恢复
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="sortkey">r，正常排序，c 热点排序</param>
        /// <param name="desc">1倒叙，0正序</param>
        /// <returns></returns>
        public string createSortImg(Dictionary<string, string> dr, string sortkey, string desc)
        {
            StringBuilder sb = new StringBuilder();
            string imgup = "/images/shangh.jpg";
            string imgdown = "/images/xiah.jpg";

            string imgup1 = "/images/shangh.jpg";
            string imgdown2 = "/images/xiah.jpg";

            if (sortkey == "r")
            {
                if (desc == "1")
                {
                    imgdown = "/images/xia.jpg";
                }
                else
                {
                    imgup = "/images/shang.jpg";
                }
            }
            else
            {
                if (desc == "1")
                {
                    imgdown2 = "/images/xia.jpg";
                }
                else
                {
                    imgup1 = "/images/shang.jpg";
                }
            }
            dr["ss"] = "r";
            dr["sd"] = "0";
            var link1 = "/an?pageid=1" + createLInk(dr);
            dr["ss"] = "r";
            dr["sd"] = "1";
            var link2 = "/an?pageid=1" + createLInk(dr);
            dr["ss"] = "c";
            dr["sd"] = "0";
            var link3 = "/an?pageid=1" + createLInk(dr);
            dr["ss"] = "c";
            dr["sd"] = "1";
            var link4 = "/an?pageid=1" + createLInk(dr);

            sb.Append("<div class=\"paixu\">最新排序<a href=\"" + link1 + "\"><img src=\"" + imgup + "\"alt=\"\"class=\"shang\"/></a><a href=\"" + link2 + "\"><img src=\"" + imgdown + "\"alt=\"\"class=\"xia\"/></a></div><div class=\"paixu\">人气排序<a href=\"" + link3 + "\"><img src=\"" + imgup1 + "\"alt=\"\"class=\"shang\"/></a><a href=\"" + link4 + "\"><img src=\"" + imgdown2 + "\"alt=\"\"class=\"xia\"/></a></div>");
            dr["ss"] = sortkey;
            dr["sd"] = desc;
            return sb.ToString();
        }

        public string ggwLink(ggw g)
        {
            StringBuilder sb = new StringBuilder();
            if (g != null)
            {
                if (!string.IsNullOrEmpty(g.ggwlink))
                {
                    sb.Append("<a href=\"" + g.ggwlink + "\"><img  src=\"" + g.imgurl + "\" alt=\"" + g.title + "\" /></a>");
                }
                else
                {
                    sb.Append("<img  src=\"" + g.imgurl + "\" alt=\"" + g.title + "\" />");
                }
            }
            return sb.ToString();
        }




        //上海奥龙
        #region 首页产品栏目滚动
        public string index_lanmu()
        {
        //    <div class="pro_lanmu">
        //    <div class="pc_box">
        //        <div class="picScroll-left1">
        //            <div class="hd">
        //                <a class="next"></a>
        //                <ul></ul>
        //                <a class="prev"></a>
        //                <span class="pageState"></span>
        //            </div>
        //            <div class="bd">
        //                <ul class="picList">
        //                    <li>
        //                        <a href="#">
        //                            <div class="left"><img src="/web_images/cpfl_slt_12.jpg" class="enlarge-img"></div>
        //                            <div class="right">
        //                                <div class="tit">硬度检测设备</div>
        //                                <div class="more"><img src="/web_images/more_19.jpg"></div>
        //                            </div>
        //                        </a>
        //                    </li>
        //                </ul>
        //            </div>
        //        </div>
        //        <script type="text/javascript">
        //            jQuery(".picScroll-left1").slide({ titCell: ".hd ul", mainCell: ".bd ul", autoPage: true, effect: "leftLoop", autoPlay: true, vis: 4, delayTime: 700 });
        //        </script>
        //    </div>
        //</div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                var c = dmc.find(n => n.Caenname == "cpzs");
                var lc = dmc.FindList(n => n.ParentID == c.Catalogid, 0, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList();
                sb.Append("<div class=\"pro_lanmu\">");
                sb.Append("<div class=\"pc_box\">");
                sb.Append("<div class=\"picScroll-left1\">");
                sb.Append("<div class=\"hd\">");
                sb.Append("<a class=\"next\"></a>");
                sb.Append("<ul></ul>");
                sb.Append("<a class=\"prev\"></a>");
                sb.Append("<span class=\"pageState\"></span>");
                sb.Append("</div>");
                sb.Append("<div class=\"bd\">");
                sb.Append("<ul class=\"picList\">");
                if (lc.Count > 0)
                {
                    foreach (var o in lc)
                    {
                        sb.Append("<li>");
                        sb.Append("<a href=\""+nl.getUrlLink(o)+"\">");
                        sb.Append("<div class=\"left\"><img src=\""+o.downloadfiles+ "\" class=\"enlarge-img\"></div>");
                        sb.Append("<div class=\"right\">");
                        sb.Append("<div class=\"tit\">"+o.Catalogname+"</div>");
                        sb.Append("<div class=\"more\"><img src=\"/web_images/more_19.jpg\"></div>");
                        sb.Append("</div>");
                        sb.Append("</a>");
                        sb.Append("</li>");
                    }
                }
                sb.Append("</ul>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("<script type=\"text/javascript\">");
                sb.Append("jQuery(\".picScroll-left1\").slide({ titCell: \".hd ul\", mainCell: \".bd ul\", autoPage: true, effect: \"leftLoop\", autoPlay: true, vis: 4, delayTime: 700 });");
                sb.Append("</script>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion
        #region 首页产品中心
        public string index_product()
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                do_class_view dcv = new do_class_view();
                List<int> arrid = new List<int>();
                var c = dmc.find(n => n.Caenname == "cpzs");
                arrid = dcv.showallclassid(c.Caenname);
                sb.Append("<div class=\"slideBox\">");
                sb.Append("<a class=\"sPrev\" href=\"javascript:void(0)\"><img src=\"/web_images/back-arrow-red-btn.png\"></a>");
                if (arrid != null)
                {
                    sb.Append("<ul>");
                    var li = di.FindList(n => arrid.Contains(n.classid.Value), 0, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    if (li.Count > 0)
                    {
                        string href = "";
                        foreach (var i in li)
                        {
                            href = nl.getContLink(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id;
                            sb.Append("<li>");
                            sb.Append("<a href=\""+href+"\">");
                            sb.Append("<div class=\"title\">"+i.title+"</div>");
                            sb.Append("<div class=\"lanmu\">" + dmc.getCatalogName(i.classid.Value) + "</div>");
                            sb.Append("<img src=\""+i.defaultpic+"\" class=\"enlarge-img\">");
                            sb.Append("<div class=\"more\">点击了解</div>");
                            sb.Append("</a>");
                            sb.Append("</li>");
                        }
                    }
                    sb.Append("</ul>");
                }
                sb.Append("<a class=\"sNext\" href=\"javascript:void(0)\"><img src=\"/web_images/next-arrow-red-btn.png\"></a>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion
        #region 首页产品中心
        public string ydjcsb()
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                do_class_view dcv = new do_class_view();
                List<int> arrid = new List<int>();
                var c = dmc.find(n => n.Caenname == "swzgl");
                arrid = dcv.showallclassid(c.Caenname);
                sb.Append("<div class=\"slideBox\">");
                sb.Append("<div class=\"product\">");
                sb.Append("<a class=\"sPrev\" href=\"javascript:void(0)\"><img src=\"/web_images/left_06.jpg\"></a>");
                sb.Append("<div class=\"pc_box\">");
                if (arrid != null)
                {
                    sb.Append("<ul>");
                    var li = di.FindList(n => arrid.Contains(n.classid.Value), 0, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    if (li.Count > 0)
                    {
                        string href = "";
                        foreach (var i in li)
                        {
                            href = nl.getContLink(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id;
                            sb.Append("<li>");
                            sb.Append("<a href=\"" + href + "\">");
                            sb.Append("<div class=\"title\">" + i.title + "</div>");
                            sb.Append("<div class=\"lanmu\">" + dmc.getCatalogName(i.classid.Value) + "</div>");
                            sb.Append("<img src=\"" + i.defaultpic + "\" class=\"enlarge-img\">");
                            sb.Append("<div class=\"more\">点击了解</div>");
                            sb.Append("</a>");
                            sb.Append("</li>");
                        }
                    }
                    sb.Append("</ul>");
                }
                sb.Append("</div>");
                sb.Append("<a class=\"sNext\" href=\"javascript:void(0)\"><img src=\"/web_images/right_08.jpg\"></a>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion
        #region 首页产品中心
        public string dzzxydj()
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                do_class_view dcv = new do_class_view();
                List<int> arrid = new List<int>();
                var c = dmc.find(n => n.Caenname == "rmgl");
                arrid = dcv.showallclassid(c.Caenname);
                sb.Append("<div class=\"slideBox\">");
                sb.Append("<div class=\"product\">");
                sb.Append("<a class=\"sPrev\" href=\"javascript:void(0)\"><img src=\"/web_images/left_06.jpg\"></a>");
                sb.Append("<div class=\"pc_box\">");
                if (arrid != null)
                {
                    sb.Append("<ul>");
                    var li = di.FindList(n => arrid.Contains(n.classid.Value), 0, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    if (li.Count > 0)
                    {
                        string href = "";
                        foreach (var i in li)
                        {
                            href = nl.getContLink(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id;
                            sb.Append("<li>");
                            sb.Append("<a href=\"" + href + "\">");
                            sb.Append("<div class=\"title\">" + i.title + "</div>");
                            sb.Append("<div class=\"lanmu\">" + dmc.getCatalogName(i.classid.Value) + "</div>");
                            sb.Append("<img src=\"" + i.defaultpic + "\" class=\"enlarge-img\">");
                            sb.Append("<div class=\"more\">点击了解</div>");
                            sb.Append("</a>");
                            sb.Append("</li>");
                        }
                    }
                    sb.Append("</ul>");
                }
                sb.Append("</div>");
                sb.Append("<a class=\"sNext\" href=\"javascript:void(0)\"><img src=\"/web_images/right_08.jpg\"></a>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion
        #region 首页产品中心
        public string jxzysb()
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                do_class_view dcv = new do_class_view();
                List<int> arrid = new List<int>();
                var c = dmc.find(n => n.Caenname == "ryrqgl");
                arrid = dcv.showallclassid(c.Caenname);
                sb.Append("<div class=\"slideBox\">");
                sb.Append("<div class=\"product\">");
                sb.Append("<a class=\"sPrev\" href=\"javascript:void(0)\"><img src=\"/web_images/left_06.jpg\"></a>");
                sb.Append("<div class=\"pc_box\">");
                if (arrid != null)
                {
                    sb.Append("<ul>");
                    var li = di.FindList(n => arrid.Contains(n.classid.Value), 0, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    if (li.Count > 0)
                    {
                        string href = "";
                        foreach (var i in li)
                        {
                            href = nl.getContLink(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id;
                            sb.Append("<li>");
                            sb.Append("<a href=\"" + href + "\">");
                            sb.Append("<div class=\"title\">" + i.title + "</div>");
                            sb.Append("<div class=\"lanmu\">" + dmc.getCatalogName(i.classid.Value) + "</div>");
                            sb.Append("<img src=\"" + i.defaultpic + "\" class=\"enlarge-img\">");
                            sb.Append("<div class=\"more\">点击了解</div>");
                            sb.Append("</a>");
                            sb.Append("</li>");
                        }
                    }
                    sb.Append("</ul>");
                }
                sb.Append("</div>");
                sb.Append("<a class=\"sNext\" href=\"javascript:void(0)\"><img src=\"/web_images/right_08.jpg\"></a>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion
        #region 首页产品中心
        public string clsysb()
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                do_class_view dcv = new do_class_view();
                List<int> arrid = new List<int>();
                var c = dmc.find(n => n.Caenname == "rfgl");
                arrid = dcv.showallclassid(c.Caenname);
                sb.Append("<div class=\"slideBox\">");
                sb.Append("<div class=\"product\">");
                sb.Append("<a class=\"sPrev\" href=\"javascript:void(0)\"><img src=\"/web_images/left_06.jpg\"></a>");
                sb.Append("<div class=\"pc_box\">");
                if (arrid != null)
                {
                    sb.Append("<ul>");
                    var li = di.FindList(n => arrid.Contains(n.classid.Value), 0, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    if (li.Count > 0)
                    {
                        string href = "";
                        foreach (var i in li)
                        {
                            href = nl.getContLink(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id;
                            sb.Append("<li>");
                            sb.Append("<a href=\"" + href + "\">");
                            sb.Append("<div class=\"title\">" + i.title + "</div>");
                            sb.Append("<div class=\"lanmu\">" + dmc.getCatalogName(i.classid.Value) + "</div>");
                            sb.Append("<img src=\"" + i.defaultpic + "\" class=\"enlarge-img\">");
                            sb.Append("<div class=\"more\">点击了解</div>");
                            sb.Append("</a>");
                            sb.Append("</li>");
                        }
                    }
                    sb.Append("</ul>");
                }
                sb.Append("</div>");
                sb.Append("<a class=\"sNext\" href=\"javascript:void(0)\"><img src=\"/web_images/right_08.jpg\"></a>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion

        #region 首页走进奥龙
        public string index_about_aolong()
        {
             //<div class="right">
             //       <a href="#">
             //           <div class="title">
             //               <div class="tit">走进奥龙</div>
             //               <div class="ftit">
             //                   专注硬度检测62年<br />
             //                   全国知名注册品牌
             //               </div>
             //           </div>
             //           <div class="txt">
             //               上海奥龙星迪检测设备有限公司前身为上海材料试验机厂，成立于1956年，是我国较早定点研发、生产材料试验设备的企业之一。
             //               公司已通过ISO9001国际质量体系认证，产品均获得欧盟CE认证。 公司目前设有硬度检测设备、金相制样设备、材料试验设备及无损检测设备四大产品线，
             //               产品具有生产工艺先进，品质控制严格，质量稳定、可靠性好、品种齐全的特点，长期远销美洲、欧洲、非洲、东南亚等40多个国家和地区，
             //               深受国内外用户的一致好评。
             //           </div>
             //           <div class="gsjj">公司简介</div>
             //       </a>
             //   </div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalSiteSeo dss = new DalSiteSeo(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                DalGgw dg = new DalGgw(s);
                var ss = dss.FindList(n => n.lang == "cn", 1, null).SingleOrDefault();
                var c = dmc.find(n => n.Caenname == "zjal");
                if (c != null)
                {
                    sb.Append("<div class=\"right\">");
                    var cc1 = dmc.find(n => n.Caenname == "gsjj");
                    if (cc1 != null)
                    {
                        sb.Append("<a href=\""+nl.getUrlLink(cc1)+"\">");
                        sb.Append("<div class=\"title\">");
                        sb.Append("<div class=\"tit\">" + c.Catalogname + "</div>");
                        sb.Append("<div class=\"ftit\">");
                        sb.Append("专注硬度检测" + ss.year + "年<br />");
                        sb.Append("全国知名注册品牌");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        string cc1href = nl.getUrlLink(cc1);
                        var cc1ggw = dg.find(n => n.ggwposition == "gsjj");
                        var cc1info = di.find(n => n.classid == cc1.Catalogid);
                        if (cc1info != null)
                        {
                            sb.Append("<div class=\"txt\">");
                            sb.Append(""+ cc1info.intro+"");
                            sb.Append("</div>");
                        }

                        sb.Append("<div class=\"gsjj\">"+cc1.Catalogname+"</div>");
                        sb.Append("</a>");
                    }
                    sb.Append("</div>");
                }
            }
            return sb.ToString();
        }
        #endregion
        #region 首页新闻中心
        public string index_news()
        {
        //         <div class="news">
        //    <div class="pc_box">
        //        <div class="title">新闻中心</div>
        //        <div class="list">
        //            <ul>
        //                <li class="teshu">
        //                    <a href="#">
        //                        <div class="left"><img src="/web_images/news_18.jpg" class="enlarge-img"></div>
        //                        <div class="right">
        //                            <div class="tit">2019年中国热处理行业厂长经理大会</div>
        //                            <div class="txt">在新中国成立70周年前夕，由中国热处理行业协会主办，无锡市热处理协会协办的2019年中国热处理行业厂长经理大会暨绿色发展高峰论坛9月17-21日</div>
        //                            <div class="date">2019-09-25</div>
        //                        </div>
        //                    </a>
        //                </li>
        //                <li>
        //                    <a href="#">
        //                        <div class="tit">2019年中国热处理行业厂长经理大会</div>
        //                        <div class="date">2019-09-25</div>
        //                    </a>
        //                </li>
        //            </ul>
        //        </div>
        //    </div>
        //</div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                do_class_view dcv = new do_class_view();
                List<int> arrid = new List<int>();
                var c = dmc.find(n => n.Caenname == "xwzx");
                arrid = dcv.showallclassid("c");
                sb.Append("<div class=\"news\">");
                sb.Append("<div class=\"pc_box\">");
                sb.Append("<div class=\"title\">"+c.Catalogname+"</div>");
                if (arrid != null)
                {
                    sb.Append("<div class=\"list\">");
                    sb.Append("<ul>");
                    var li = di.FindList(n => arrid.Contains(n.classid.Value), 6, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    if (li.Count > 0)
                    {
                        int num = 0;
                        string css = "", href = "";
                        foreach (var i in li)
                        {
                            href = nl.getContLink(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id;
                            if (num == 0|| num == 1)
                            {
                                sb.Append("<li class=\"teshu\">");
                                sb.Append("<a href=\"" + href + "\">");
                                sb.Append("<div class=\"left\">");
                                if (i.defaultpic != null)
                                {
                                    sb.Append("<img src=\"" + i.defaultpic + "\" class=\"enlarge-img\">");
                                }
                                else {
                                    sb.Append("<img src=\"/web_images/news_18.jpg\" class=\"enlarge-img\">");
                                }
                                sb.Append("</div>");
                                sb.Append("<div class=\"right\">");
                                sb.Append("<div class=\"tit\">"+i.title+"</div>");
                                sb.Append("<div class=\"txt\">"+ common.common.DelHTML(i.intro)+"</div>");
                                sb.Append("<div class=\"date\">"+i.insertdate.Value.Year+"-"+i.insertdate.Value.Month +"-"+i.insertdate.Value.Day+"</div>");
                                sb.Append("</div>");
                                sb.Append("</a>");
                                sb.Append("</li>");
                            }
                            else
                            {
                                sb.Append("<li>");
                                sb.Append("<a href=\"" + href + "\">");
                                sb.Append("<div class=\"tit\">"+i.title+"</div>");
                                sb.Append("<div class=\"date\">" + i.insertdate.Value.Year + "-" + i.insertdate.Value.Month + "-" + i.insertdate.Value.Day + "</div>");
                                sb.Append("</a>");
                                sb.Append("</li>");
                            }
                            num++;
                        }
                    }
                    sb.Append("</ul>");
                    sb.Append("</div>");
                }
                sb.Append("</div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion
        #region 首页联系我们
        public string index_lxwm()
        {
            //<div class="right">
            //       <a href="#">
            //           <div class="title">
            //               <div class="tit">走进奥龙</div>
            //               <div class="ftit">
            //                   专注硬度检测62年<br />
            //                   全国知名注册品牌
            //               </div>
            //           </div>
            //           <div class="txt">
            //               上海奥龙星迪检测设备有限公司前身为上海材料试验机厂，成立于1956年，是我国较早定点研发、生产材料试验设备的企业之一。
            //               公司已通过ISO9001国际质量体系认证，产品均获得欧盟CE认证。 公司目前设有硬度检测设备、金相制样设备、材料试验设备及无损检测设备四大产品线，
            //               产品具有生产工艺先进，品质控制严格，质量稳定、可靠性好、品种齐全的特点，长期远销美洲、欧洲、非洲、东南亚等40多个国家和地区，
            //               深受国内外用户的一致好评。
            //           </div>
            //           <div class="gsjj">公司简介</div>
            //       </a>
            //   </div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalSiteSeo dss = new DalSiteSeo(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                DalGgw dg = new DalGgw(s);
                var ss = dss.FindList(n => n.lang == "cn", 1, null).SingleOrDefault();
                var c = dmc.find(n => n.Caenname == "ljwm");
                string cc1href = nl.getUrlLink(c);
                var cc1info = di.find(n => n.classid == c.Catalogid);
                if (c != null)
                {
                    sb.Append("<div class=\"lxwm\">");
                    sb.Append("<a href=\""+ cc1href + "\">");
                    sb.Append("<div class=\"tit\">24小时全国统一热线</div>");
                    sb.Append("<div class=\"dianhua\">" + ss.tel + "</div>");
                    sb.Append("<div class=\"btn\"><img src=\"/web_images/sj_dh_08.jpg\" class=\"enlarge-img\"></div>");
                    sb.Append("<div class=\"txt\">");
                    if (cc1info != null)
                    {
                        sb.Append("" + cc1info.intro + "");
                    }
                    sb.Append("</div>");
                    sb.Append("<div class=\"bohao\"><span>联系我们</span></div>");
                    sb.Append("</a>");
                    sb.Append("</div>");
                }
            }
            return sb.ToString();
        }
        #endregion
        #region 首页友情链接
        public string index_link()
        {
               //<div class="txt"><a href="#">上海奥龙星迪检测设备有限公司 </a>/</div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalLink dlk = new DalLink(s);
                var li = dlk.FindList(n => true, 0, null ).ToList();
                if (li.Count > 0) {
                    int num = 0;
                    sb.Append("<div class=\"txt\">");
                    foreach (var a in li) {
                        if (num == li.Count - 1)
                        {
                            sb.Append("<a href=\"" + a.linkurl + "\">" + a.linkname + "</a>");
                        }
                        else { 
                        sb.Append("<a href=\"" + a.linkurl + "\">" + a.linkname + "</a>&nbsp;/&nbsp;");
                        }
                        num++;
                    }
                    sb.Append("</div>");
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 首页手机产品栏目滚动
        public string index_sj_lanmu()
        {
            //  <div class="picScroll-left">
            //    <div class="hd">
            //        <a class="next"></a>
            //        <ul></ul>
            //        <a class="prev"></a>
            //        <span class="pageState"></span>
            //    </div>
            //    <div class="bd">
            //        <ul class="picList">
            //            <li>
            //                <a href="#">
            //                    <div class="left"><img src="/web_images/cpfl_slt_12.jpg"></div>
            //                    <div class="right">
            //                        <div class="tit">硬度检测设备</div>
            //                        <div class="more"><img src="/web_images/more_19.jpg"></div>
            //                    </div>
            //                </a>
            //            </li>
            //        </ul>
            //    </div>
            //</div>
            //<script type="text/javascript">
            //    jQuery(".picScroll-left").slide({ titCell: ".hd ul", mainCell: ".bd ul", autoPage: true, effect: "leftLoop", autoPlay: true, vis: 2, delayTime: 700 });
            //</script>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                var c = dmc.find(n => n.Caenname == "cpzs");
                var lc = dmc.FindList(n => n.ParentID == c.Catalogid, 0, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList();
                sb.Append("<div class=\"picScroll-left\">");
                sb.Append("<div class=\"hd\">");
                sb.Append("<a class=\"next\"></a>");
                sb.Append("<ul></ul>");
                sb.Append("<a class=\"prev\"></a>");
                sb.Append("<span class=\"pageState\"></span>");
                sb.Append("</div>");
                sb.Append("<div class=\"bd\">");
                sb.Append("<ul class=\"picList\">");
                if (lc.Count > 0)
                {
                    foreach (var o in lc)
                    {
                          sb.Append("<li>");
                        sb.Append("<a href=\"" + nl.getUrlLink(o) + "\">");
                        //sb.Append("<div class=\"left\"><img src=\"" + o.downloadfiles + "\"></div>");
                        sb.Append("<div class=\"left\"><img src=\"" + "/web_images/product1.png" + "\"></div>");
                        sb.Append("<div class=\"right\">");
                        sb.Append("<div class=\"tit\">" + o.Catalogname + "</div>");
                        //sb.Append("<div class=\"more\"><img src=\"/web_images/more_19.jpg\"></div>");
                        sb.Append("<div class=\"more\">操作简单、触摸屏智能控制、温度压力自动控制、一键启停机操作、火力大小任意控制、电</div>\n");
                        sb.Append("</div>");
                        sb.Append("</a>");
                        sb.Append("</li>");
                    }
                }
                sb.Append("</ul>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("<script type=\"text/javascript\">");
                sb.Append("jQuery(\".picScroll-left\").slide({ titCell: \".hd ul\", mainCell: \".bd ul\", autoPage: true, effect: \"leftLoop\", autoPlay: false, vis: 2, delayTime: 700 });");
                sb.Append("</script>");
            }
            return sb.ToString();
        }
        #endregion
        #region 首页手机产品展示
        public string index_sj_pro()
        {
            //  <div class="pro_list">
            //    <div class="top_tit">
            //        <div class="title"><a href="#">产品展示</a></div>
            //        <div class="more"><a href="#">全部产品</a></div>
            //    </div>
            //    <div class="list">
            //        <ul>
            //            <li>
            //                <a href="#">
            //                    <div class="slt"><img src="/web_images/pro_39.jpg"></div>
            //                    <div class="title">HB-3000E电子布氏硬度计</div>
            //                    <div class="lanmu">布氏硬度计系列</div>
            //                </a>
            //            </li>
            //        </ul>
            //    </div>
            //</div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                do_class_view dcv = new do_class_view();
                List<int> arrid = new List<int>();
                var c = dmc.find(n => n.Caenname == "cpzs");
                arrid = dcv.showallclassid(c.Caenname);
                //sb.Append("<div class=\"pro_list\">");
                //sb.Append("<div class=\"c-red txt-center fst24\">Product</div>");
                //sb.Append("<div class=\"top_tit\">");
                //sb.Append("<div class=\"title\"><a href=\"" + nl.getUrlLink(c) + "\">" + c.Catalogname + "</a></div>");
                //sb.Append("</div>");
                //if (arrid.Count != 0)
                //{
                //    sb.Append("<div class=\"list\">");
                //    sb.Append("<ul>");
                //    var li = di.FindList(n => arrid.Contains(n.classid.Value), 6, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                //    if (li.Count > 0)
                //    {
                //        string href = "";
                //        foreach (var i in li)
                //        {
                //            href = nl.getContLink(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id;
                //            sb.Append("<li>");
                //            sb.Append("<a href=\"" + href + "\">");
                //            sb.Append("<div class=\"slt\"><img src=\"" + i.defaultpic + "\"></div>");
                //            sb.Append("<div class=\"title\">" + i.title + "</div>");
                //            sb.Append("<div class=\"lanmu\">" + dmc.getCatalogName(i.classid.Value) + "</div>");
                //            sb.Append("</a>");
                //            sb.Append("</li>");
                //        }
                //    }
                //    sb.Append("</ul>");
                //    sb.Append("</div>");
                //}
                //sb.Append("</div>");


                sb.Append("<div class=\"picScroll-left\">");
                sb.Append("<div class=\"hd\">");
                sb.Append("<a class=\"next\"></a>");
                sb.Append("<ul></ul>");
                sb.Append("<a class=\"prev\"></a>");
                sb.Append("<span class=\"pageState\"></span>");
                sb.Append("</div>");
                sb.Append("<div class=\"bd\">");
                sb.Append("<ul class=\"picList\">");
                if (arrid.Count != 0)
                {
                    var li = di.FindList(n => arrid.Contains(n.classid.Value), 6, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    foreach (var i in li)
                    {
                        sb.Append("<li>");
                        sb.Append("<a href=\"" + dmc.getCatalogName(i.classid.Value) + "\">");
                        sb.Append("<div class=\"left\"><img src=\"" + "/web_images/product1.png" + "\"></div>");
                        sb.Append("<div class=\"right\">");
                        sb.Append("<div class=\"tit\">" + i.title + "</div>");
                        //sb.Append("<div class=\"more\"><img src=\"/web_images/more_19.jpg\"></div>");
                        sb.Append("<div class=\"more\">操作简单、触摸屏智能控制、温度压力自动控制、一键启停机操作、火力大小任意控制、电</div>\n");
                        sb.Append("</div>");
                        sb.Append("</a>");
                        sb.Append("</li>");
                    }
                }
                sb.Append("</ul>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("<script type=\"text/javascript\">");
                sb.Append("jQuery(\".picScroll-left\").slide({ titCell: \".hd ul\", mainCell: \".bd ul\", autoPage: true, effect: \"leftLoop\", autoPlay: false, vis: 2, delayTime: 700 });");
                sb.Append("</script>");
            }
            return sb.ToString();
        }
        #endregion
        #region 首页手机走进奥龙
        public string index_sj_aolong()
        {
            //<div class="right">
            //       <a href="#">
            //           <div class="title">
            //               <div class="tit">走进奥龙</div>
            //               <div class="ftit">
            //                   专注硬度检测62年<br />
            //                   全国知名注册品牌
            //               </div>
            //           </div>
            //           <div class="txt">
            //               上海奥龙星迪检测设备有限公司前身为上海材料试验机厂，成立于1956年，是我国较早定点研发、生产材料试验设备的企业之一。
            //               公司已通过ISO9001国际质量体系认证，产品均获得欧盟CE认证。 公司目前设有硬度检测设备、金相制样设备、材料试验设备及无损检测设备四大产品线，
            //               产品具有生产工艺先进，品质控制严格，质量稳定、可靠性好、品种齐全的特点，长期远销美洲、欧洲、非洲、东南亚等40多个国家和地区，
            //               深受国内外用户的一致好评。
            //           </div>
            //           <div class="gsjj">公司简介</div>
            //       </a>
            //   </div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalSiteSeo dss = new DalSiteSeo(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                DalGgw dg = new DalGgw(s);
                var ss = dss.FindList(n => n.lang == "cn", 1, null).SingleOrDefault();
                var c = dmc.find(n => n.Caenname == "zjal");
                if (c != null)
                {
                    var cc1 = dmc.find(n => n.Caenname == "gsjj");
                    if (cc1 != null)
                    {
                        var cc1ggw = dg.find(n => n.ggwposition == "gsjj");
                        var cc1info = di.find(n => n.classid == cc1.Catalogid);
                        
                        sb.Append("<div class=\"zjal\">");
                        sb.Append("<div class=\"zjal_nr\">");
                        sb.Append("<div class=\"title\">" + c.Catalogname + "</div>");
                        if (cc1ggw != null)
                        {
                            sb.Append("<div class=\"slt\">");
                            sb.Append("<embed class=\"edui-faked-video\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" src=\"http://1258165379.vod2.myqcloud.com/1701b371vodcq1258165379/3787c26c5285890806852192054/8hqEjnhdQvYA.mp4\"wmode=\"transparent\" play=\"true\" loop=\"false\" menu=\"false\" allowscriptaccess=\"never\" allowfullscreen=\"true\" />");
                                sb.Append("</div>");
                        }
                        sb.Append("<div class=\"txt\">");
                        if (cc1info != null)
                        {
                            sb.Append("" + cc1info.intro + "");
                        }
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("<div class=\"more\"><a href=\"" + nl.getUrlLink(cc1) + "\">了解详情</a></div>");
                        sb.Append("</div>");
                    }
                }
            }
            return sb.ToString();
        }
        #endregion
        #region 首页新闻中心
        public string sj_index_news()
        {
            //  <div class="news">
            //    <div class="top_tit">新闻中心</div>
            //    <div class="list">
            //        <ul>
            //            <li>
            //                <a href="#">
            //                    <div class="left"><img src="/web_images/news_18.jpg"></div>
            //                    <div class="right">
            //                        <div class="title">2019年中国热处理行业厂长经...</div>
            //                        <div class="txt">在新中国成立70周年前夕，由中国热处理行业协会主办，无锡市热处理协会协办...</div>
            //                        <div class="date">2019-09-25</div>
            //                    </div>
            //                </a>
            //            </li>
            //        </ul>
            //    </div>
            //    <div class="more"><a href="#">查看更多</a></div>
            //</div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                do_class_view dcv = new do_class_view();
                List<int> arrid = new List<int>();
                var c = dmc.find(n => n.Caenname == "xwzx");
                arrid = dcv.showallclassid(c.Caenname);
                sb.Append("<div class=\"news\">");
                sb.Append("<div class=\"top_tit\">" + c.Catalogname + "</div>");
                if (arrid != null)
                {
                    sb.Append("<div class=\"list\">");
                    sb.Append("<ul>");
                    var li = di.FindList(n => arrid.Contains(n.classid.Value), 3, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    if (li.Count > 0)
                    {
                        string href = "";
                        foreach (var i in li)
                        {
                            href = nl.getContLink(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id;
                            sb.Append("<li>");
                            sb.Append("<a href=\""+href+"\">");
                            sb.Append("<div class=\"left\"><img src=\""+i.defaultpic+"\"></div>");
                            sb.Append("<div class=\"right\">");
                            sb.Append("<div class=\"title\">"+i.title+"</div>");
                            sb.Append("<div class=\"txt\">"+i.intro+"</div>");
                            sb.Append("<div class=\"date\">"+i.insertdate.Value.Year+"-"+i.insertdate.Value.Month+"-"+i.insertdate.Value.Day+"</div>");
                            sb.Append("</div>");
                            sb.Append("</a>");
                            sb.Append("</li>");
                        }
                    }
                    sb.Append("</ul>");
                    sb.Append("</div>");
                }
                sb.Append("<div class=\"more\"><a href=\"" + nl.getUrlLink(c) + "\">查看更多</a></div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion
        #region 首页手机联系我们
        public string sj_lxwm()
        {
            //<div class="yijian_tel">
            //    <div class="nr">
            //        <div class="tit">24小时全国统一热线</div>
            //        <div class="dianhua"><a href="#">400-805-3385</a></div>
            //        <div class="btn"><a href="#"><img src="/web_images/sj_dh_08.jpg"></a></div>
            //        <div class="txt">
            //            厂 址：上海市松江区玉阳路288弄E1号<br />
            //            电子信箱：sale@hardnesstestersh.com<br />
            //            电话：400-805-3385 021-63770518<br />
            //            售后服务：021-58122328
            //        </div>
            //        <div class="bohao"><a href="#">一键拨号</a></div>

            //    </div>
            //</div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalSiteSeo dss = new DalSiteSeo(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                DalGgw dg = new DalGgw(s);
                var ss = dss.FindList(n => n.lang == "cn", 1, null).SingleOrDefault();
                var c = dmc.find(n => n.Caenname == "ljwm");
                string cc1href = nl.getUrlLink(c);
                var cc1info = di.find(n => n.classid == c.Catalogid);
                if (c != null)
                {
                    sb.Append("<div class=\"yijian_tel\">");
                    sb.Append("<div class=\"nr\">");
                    sb.Append("<div class=\"tit\">24小时全国统一热线</div>");
                    sb.Append("<div class=\"dianhua\"><a href=\"tel:" + ss.tel + "\">" + ss.tel + "</a></div>");
                    sb.Append("<div class=\"btn\"><a href=\"tel:" + ss.tel + "\"><img src=\"/web_images/sj_dh_08.jpg\"></a></div>");
               
                        sb.Append("<div class=\"txt\">");
                        if (cc1info != null)
                        {
                            sb.Append("" + cc1info.intro + "");
                        }
                        sb.Append("</div>");
                    sb.Append("<div class=\"bohao\"><a href=\"tel:" + ss.tel + "\">一键拨号</a></div>");

                    sb.Append("</div>");
                    sb.Append("</div>");
                }
            }
            return sb.ToString();
        }
        #endregion


        #region pc分类内容页
        public string info_content(List<view_info> l)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count > 0)
            {
                sb.Append("<div class=\"nr\">");
                sb.Append(l[0].cont);
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion
        #region pc_产品列表
        public string pro_list(List<view_info> l)
        {
                //<div class="pro_list">
                //            <ul>
                //                <li>
                //                    <a href="#">
                //                        <div class="slt"><img src="web_images/pro_39.jpg" class="enlarge-img"></div>
                //                        <div class="tit">MHRS-15045触摸屏数显双洛氏硬度计</div>
                //                    </a>
                //                </li>
                //            </ul>
                //        </div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    sb.Append("<div class=\"pro_list\"><ul>");
                    foreach (var o in l)
                    {
                        href = nl.getContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("<li>");
                        sb.Append("<a href=\""+ href + "\">");
                        sb.Append("<div class=\"slt\"><img src=\""+o.defaultpic+"\" class=\"enlarge-img\"></div>");
                        sb.Append("<div class=\"tit\">" + o.title + "</div>");
                        sb.Append("</a>");
                        sb.Append("</li>");
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        #endregion
        #region pc_环境列表
        public string huanjing_list(List<view_info> l)
        {
          //<div class="huanjing_list">
          //                  <ul>
          //                      <li>
          //                          <a href="#">
          //                              <div class="slt"><img src="web_images/slt_13.jpg" class="enlarge-img"></div>
          //                              <div class="tit">布氏硬度计装配车间</div>
          //                          </a>
          //                      </li>
          //                  </ul>
          //              </div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    sb.Append("<div class=\"huanjing_list\"><ul>");
                    foreach (var o in l)
                    {
                        href = nl.getContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("<li>");
                        sb.Append("<a href=\"" + href + "\">");
                        sb.Append("<div class=\"slt\"><img src=\"" + o.defaultpic + "\" class=\"enlarge-img\"></div>");
                        sb.Append("<div class=\"tit\">" + o.title + "</div>");
                        sb.Append("</a>");
                        sb.Append("</li>");
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        #endregion
        #region pc_新闻列表
        public string news_list(List<view_info> l)
        {
                   //<div class="news_list">
                   //         <ul>
                   //             <li>
                   //                 <a href="#">
                   //                     <div class="title">列表样式标题显示位置列表样式标题显示位置列表样式标题显示位置列表样式标题显示位置</div>
                   //                     <div class="date">2020-08-04</div>
                   //                 </a>
                   //             </li>
                   //             <li class="last">
                   //                 <a href="#">
                   //                     <div class="title">列表样式标题显示位置列表样式标题显示位置列表样式标题显示位置列表样式标题显示位置</div>
                   //                     <div class="date">2020-08-04</div>
                   //                 </a>
                   //             </li>
                   //         </ul>
                   //     </div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    int num = 0;
                    string href = "";
                    sb.Append("<div class=\"news_list\"><ul>");
                    string css = "";
                    foreach (var o in l)
                    {
                        href = nl.getContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        if (num == l.Count - 1) {
                            css = " class=\"last\"";
                        }
                        sb.Append("<li "+ css + ">");
                        sb.Append("<a href=\""+ href + "\">");
                        sb.Append("<div class=\"title\">"+o.title+"</div>");
                        sb.Append("<div class=\"date\">"+o.insertdate.Value.Year+"-"+o.insertdate.Value.Month+"-"+o.insertdate.Value.Day+"</div>");
                        sb.Append("</a>");
                        sb.Append("</li>");
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        #endregion
        #region pc_视频中心
        public string shipin_list(List<view_info> l)
        {
                //<div class="shipin_list">
                //            <ul>
                //                <li>
                //                    <a href="#">
                //                        <div class="left"><img src="web_images/slt_13.jpg" class="enlarge-img"></div>
                //                        <div class="right">
                //                            <div class="tit">列表样式标题显示位置列表样式</div>
                //                            <div class="txt">
                //                                简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短
                //                                介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍简短介绍
                //                            </div>
                //                            <div class="date">2020-08-04</div>
                //                        </div>
                //                    </a>
                //                </li>
                //            </ul>
                //        </div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    sb.Append("<div class=\"shipin_list\"><ul>");
                    string css = "";
                    foreach (var o in l)
                    {
                        href = nl.getContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("<li>");
                        sb.Append("<a href=\""+href+"\">");
                        sb.Append("<div class=\"left\"><img src=\""+o.defaultpic+"\" class=\"enlarge-img\"></div>");
                        sb.Append("<div class=\"right\">");
                        sb.Append("<div class=\"tit\">"+o.title+"</div>");
                        sb.Append("<div class=\"txt\">"+o.intro+"</div>");
                        sb.Append("<div class=\"date\">"+o.insertdate.Value.Year+"-"+o.insertdate.Value.Month+"-"+o.insertdate.Value.Day+"</div>");
                        sb.Append("</div>");
                        sb.Append("</a>");
                        sb.Append("</li>");
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        #endregion
        #region sj_产品列表
        public string sj_pro_list(List<view_info> l)
        {
               //<div class="pro_list">
               //     <ul>
               //         <li>
               //             <a href="#">
               //                 <div class="slt"><img src="web_images/pro_39.jpg"></div>
               //                 <div class="title">HB-3000E电子布氏硬度计</div>
               //                 <div class="lanmu">布氏硬度计系列</div>
               //             </a>
               //         </li>
               //     </ul>
               // </div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    sb.Append("<div class=\"pro_list\"><ul>");
                    foreach (var o in l)
                    {
                        href = nl.getContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("<li>");
                        sb.Append("<a href=\"" + href + "\">");
                        sb.Append("<div class=\"slt\"><img src=\"" + o.defaultpic + "\" class=\"enlarge-img\"></div>");
                        sb.Append("<div class=\"title\">" + o.title + "</div>");
                        sb.Append("<div class=\"lanmu\">" + dmc.getCatalogName(o.classid.Value) + "</div>");
                        sb.Append("</a>");
                        sb.Append("</li>");
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        #endregion
        #region sj_环境列表
        public string sj_huanjing_list(List<view_info> l)
        {
            //<div class="huanjing_list">
            //        <ul>
            //            <li>
            //                <a href="#">
            //                    <div class="slt"><img src="web_images/huanjing_03.jpg"></div>
            //                    <div class="tit">布氏硬度计装配车间</div>
            //                </a>
            //            </li>
            //        </ul>
            //    </div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    sb.Append("<div class=\"huanjing_list\"><ul>");
                    foreach (var o in l)
                    {
                        href = nl.getContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("<li>");
                        sb.Append("<a href=\"" + href + "\">");
                        sb.Append("<div class=\"slt\"><img src=\"" + o.defaultpic + "\"></div>");
                        sb.Append("<div class=\"tit\">" + o.title + "</div>");
                        sb.Append("</a>");
                        sb.Append("</li>");
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        #endregion
        #region sj_新闻列表
        public string sj_news_list(List<view_info> l)
        {
                // <div class="news_list">
                //    <ul>
                //        <li>
                //            <a href="#">
                //                <div class="tit">列表样式标题显示位置列表样式标题显示位置列表样式标题显示位置列表样式标题显示位置</div>
                //                <div class="date">2020/08/04</div>
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
                    sb.Append("<div class=\"news_list\"><ul>");
                    foreach (var o in l)
                    {
                        href = nl.getContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("<li>");
                        sb.Append("<a href=\"" + href + "\">");
                        sb.Append("<div class=\"tit\">" + o.title + "</div>");
                        sb.Append("<div class=\"date\">" + o.insertdate.Value.Year + "-" + o.insertdate.Value.Month + "-" + o.insertdate.Value.Day + "</div>");
                        sb.Append("</a>");
                        sb.Append("</li>");
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        #endregion
        #region sj_视频中心
        public string sj_shipin_list(List<view_info> l)
        {
               //<div class="viedo_list">
               //     <ul>
               //         <li>
               //             <a href="#">
               //                 <div class="left"><img src="web_images/slt_13.jpg"></div>
               //                 <div class="right">
               //                     <div class="tit">列表样式标题显示位置列表样式标位置列表样式标题显示位置</div>
               //                     <div class="date">2020/08/04</div>
               //                 </div>
               //             </a>
               //         </li>
               //     </ul>
               // </div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                if (l.Count() > 0)
                {
                    string href = "";
                    sb.Append("<div class=\"viedo_list\"><ul>");
                    foreach (var o in l)
                    {
                        href = nl.getContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("<li>");
                        sb.Append("<a href=\"" + href + "\">");
                        sb.Append("<div class=\"left\"><img src=\"" + o.defaultpic + "\" class=\"enlarge-img\"></div>");
                        sb.Append("<div class=\"right\">");
                        sb.Append("<div class=\"tit\">" + o.title + "</div>");
                        sb.Append("<div class=\"date\">" + o.insertdate.Value.Year + "/" + o.insertdate.Value.Month + "/" + o.insertdate.Value.Day + "</div>");
                        sb.Append("</div>");
                        sb.Append("</a>");
                        sb.Append("</li>");
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        #endregion





        #region 列表显示形式-产品
        //public string cplist(List<viewinfo> l)
        //{
        //    //<div class="cxtravel">
        //    //    <ul>
        //    //        <li class="last">
        //    //            <div class="left"><a href="javascript:;"><img src="/web_images/travel.jpg" alt="" /></a></div>
        //    //            <div class="wenzi">
        //    //                <h2>丹东到朝鲜新义州半日游登岸游</h2>
        //    //                <div class="txt">体验别样的中朝界河鸭绿江沿岸风光，享受热情好客的朝鲜人民为您准备的丰富的朝鲜特色美食，观看民俗娱乐表演。归国之前还可以进入国际免税品商店，选择您心仪的国际知名品牌的免税商品。</div>
        //    //                <div class="bot">
        //    //                    <div class="price">￥<b>400</b>/人</div>
        //    //                    <a href="javascript:;"><span>查看具体行程</span></a>
        //    //                </div>
        //    //            </div>
        //    //        </li>
        //    //    </ul>
        //    //</div>
        //    StringBuilder sb = new StringBuilder();
        //    NavLIst nl = new NavLIst(new ykmWebDbContext());
        //    DalMenuClass dmc = new DalMenuClass(new ykmWebDbContext());
        //    if (l.Count() > 0)
        //    {
        //        string css = "";
        //        int num = 0;
        //        sb.Append("<div class=\"cxtravel\"><ul>");
        //        foreach (var o in l)
        //        {
        //            css = (num == l.Count - 1) ? " class=\"last\"" : "";
        //            sb.Append("        <li" + css + ">");
        //            sb.Append("            <div class=\"left\"><a href=\"" + nl.getContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id + "\"><img src=\"" + o.defaultpic + "\" alt=\"" + o.title + "\" /></a></div>");
        //            sb.Append("            <div class=\"wenzi\">");
        //            sb.Append("                <h2>" + o.title + "</h2>");
        //            sb.Append("                <div class=\"txt\">" + o.jj + "</div>");
        //            sb.Append("                <div class=\"bot\">");
        //            //sb.Append("                    <div class=\"price\">￥<b>" + o.price + "</b>/人</div>");
        //            sb.Append("                    <a href=\"" + nl.getContLink(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id + "\"><span>查看具体行程</span></a>");
        //            sb.Append("                </div>");
        //            sb.Append("            </div>");
        //            sb.Append("        </li>");
        //            num++;
        //        }
        //        sb.Append("</ul></div>");
        //    }
        //    return sb.ToString();
        //}
        #endregion






        #region 首页产品中心
        public string index_product_center()
        {
            //<div class="hd">
            //    <div class="sub-menu">
            //        <a id="ykm-right" class="ykm-btn" href="javascript:;"></a>
            //        <a id="ykm-left" class="ykm-btn" href="javascript:;"></a>
            //        <div class="c-menu">
            //            <ul id="c-ul">
            //                <li class="c-item"><a href="report1?year=2020">大宗商品</a></li>
            //                <span>|</span>
            //                <li class="c-item "><a href="report1?year=2019">电子配件</a></li>
            //            </ul>
            //        </div>
            //    </div>
            //    <div class="more">
            //        <a href="#">查看全部1</a>
            //    </div>
            //</div>
            //<div class="bd">
            //    <ul>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li class="bt">
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li class="bt">
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //    </ul>
            //</div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                var c = dmc.find(n => n.Caenname == "cpzx");
                if (c != null)
                {
                    sb.Append("<div class=\"title\">" + c.Catalogname + "</div>");
                    sb.Append("<div class=\"entit\">" + c.subtitle + "</div>");
                    if (c.Child > 0)
                    {
                        sb.Append("<div class=\"hd\">");
                        sb.Append("    <div class=\"sub-menu\">");
                        sb.Append("        <div class=\"fake-right\"><a id=\"ykm-right\" class=\"ykm-btn\" href=\"javascript:;\"></a></div>");
                        sb.Append("        <div class=\"fake-left\"><a id=\"ykm-left\" class=\"ykm-btn\" href=\"javascript:;\"></a></div>");
                        sb.Append("        <div class=\"c-menu\">");
                        sb.Append("            <ul id=\"c-ul\">");
                        var lc = dmc.FindList(n => n.ParentID == c.Catalogid, 0, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList();
                        if (lc.Count > 0)
                        {
                            int n = 0;
                            foreach (var o in lc)
                            {
                                if (n != 0)
                                {
                                    sb.Append("<span>|</span>");
                                }
                                sb.Append("<li class=\"c-item\"><a href=\"javascript:;\">" + o.Catalogname + "</a></li>");
                                n++;
                            }
                        }
                        sb.Append("            </ul>");
                        sb.Append("        </div>");
                        sb.Append("    </div>");
                        sb.Append("    <div class=\"more\">");
                        if (lc.Count > 0)
                        {
                            foreach (var o in lc)
                            {
                                sb.Append("<a href=\"" + nl.getUrlLink(o) + "\">查看全部</a>");
                            }
                        }
                        sb.Append("    </div>");
                        sb.Append("</div>");
                        sb.Append("<div class=\"bd\">");

                        if (lc.Count > 0)
                        {
                            foreach (var o in lc)
                            {
                                sb.Append("    <ul>");
                                var li = di.FindList(n => n.classid == o.Catalogid, 8, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = false }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                                if (li.Count > 0)
                                {
                                    int m = 0;
                                    string css = "";
                                    foreach (var i in li)
                                    {
                                        css = (m % 4 == 3) ? " class=\"bt\"" : "";
                                        sb.Append("        <li" + css + ">");
                                        sb.Append("            <a href=\"" + nl.getContLink(o) + "?id=" + i.id + "\">");
                                        sb.Append("                <div class=\"slt\"><img src=\"" + i.defaultpic + "\"></div>");
                                        sb.Append("                <div class=\"tit_box\">");
                                        sb.Append("                    <div class=\"tit\">" + i.title + "</div>");
                                        sb.Append("                    <div class=\"price\">￥" + common.common.getMoneyType(double.Parse(common.common.IsNumeric_n(i.price))) + "</div>");
                                        sb.Append("                </div>");
                                        sb.Append("            </a>");
                                        sb.Append("        </li>");
                                        m++;
                                    }
                                }
                                sb.Append("    </ul>");
                            }
                        }
                        sb.Append("</div>");
                    }
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 英文版首页产品中心
        public string index_product_center_en()
        {
            //<div class="hd">
            //    <div class="sub-menu">
            //        <a id="ykm-right" class="ykm-btn" href="javascript:;"></a>
            //        <a id="ykm-left" class="ykm-btn" href="javascript:;"></a>
            //        <div class="c-menu">
            //            <ul id="c-ul">
            //                <li class="c-item"><a href="report1?year=2020">大宗商品</a></li>
            //                <span>|</span>
            //                <li class="c-item "><a href="report1?year=2019">电子配件</a></li>
            //            </ul>
            //        </div>
            //    </div>
            //    <div class="more">
            //        <a href="#">查看全部1</a>
            //    </div>
            //</div>
            //<div class="bd">
            //    <ul>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li class="bt">
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //        <li class="bt">
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
            //                </div>
            //            </a>
            //        </li>
            //    </ul>
            //</div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                var c = dmc.find(n => n.Caenname == "products");
                if (c != null)
                {
                    sb.Append("<div class=\"title\">" + c.Catalogname + "</div>");
                    sb.Append("<div class=\"entit\">" + c.subtitle + "</div>");
                    if (c.Child > 0)
                    {
                        sb.Append("<div class=\"hd\">");
                        sb.Append("    <div class=\"sub-menu\">");
                        sb.Append("        <div class=\"fake-right\"><a id=\"ykm-right\" class=\"ykm-btn\" href=\"javascript:;\"></a></div>");
                        sb.Append("        <div class=\"fake-left\"><a id=\"ykm-left\" class=\"ykm-btn\" href=\"javascript:;\"></a></div>");
                        sb.Append("        <div class=\"c-menu\">");
                        sb.Append("            <ul id=\"c-ul\">");
                        var lc = dmc.FindList(n => n.ParentID == c.Catalogid, 0, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).ToList();
                        if (lc.Count > 0)
                        {
                            int n = 0;
                            foreach (var o in lc)
                            {
                                if (n != 0)
                                {
                                    sb.Append("<span>|</span>");
                                }
                                sb.Append("<li class=\"c-item\"><a href=\"javascript:;\">" + o.Catalogname + "</a></li>");
                                n++;
                            }
                        }
                        sb.Append("            </ul>");
                        sb.Append("        </div>");
                        sb.Append("    </div>");
                        sb.Append("    <div class=\"more\">");
                        if (lc.Count > 0)
                        {
                            foreach (var o in lc)
                            {
                                sb.Append("<a href=\"" + nl.getUrlLink_en(o) + "\">View all</a>");
                            }
                        }
                        sb.Append("    </div>");
                        sb.Append("</div>");
                        sb.Append("<div class=\"bd\">");

                        if (lc.Count > 0)
                        {
                            foreach (var o in lc)
                            {
                                sb.Append("    <ul>");
                                var li = di.FindList(n => n.classid == o.Catalogid, 8, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = false }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                                if (li.Count > 0)
                                {
                                    int m = 0;
                                    string css = "";
                                    foreach (var i in li)
                                    {
                                        css = (m % 4 == 3) ? " class=\"bt\"" : "";
                                        sb.Append("        <li" + css + ">");
                                        sb.Append("            <a href=\"" + nl.getContLink_en(o) + "?id=" + i.id + "\">");
                                        sb.Append("                <div class=\"slt\"><img src=\"" + i.defaultpic + "\"></div>");
                                        sb.Append("                <div class=\"tit_box\">");
                                        sb.Append("                    <div class=\"tit\">" + i.title + "</div>");
                                        sb.Append("                    <div class=\"price\">￥" + common.common.getMoneyType(double.Parse(common.common.IsNumeric_n(i.price))) + "</div>");
                                        sb.Append("                </div>");
                                        sb.Append("            </a>");
                                        sb.Append("        </li>");
                                        m++;
                                    }
                                }
                                sb.Append("    </ul>");
                            }
                        }
                        sb.Append("</div>");
                    }
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 首页公司动态
        public string index_company_news()
        {
            //<div class="title">公司动态</div>
            //<div class="entit">Company news</div>
            //<div class="first">
            //    <a href="#">
            //        <div class="slt"><img src="/web_images/dongtai_13.jpg"></div>
            //        <div class="wenzi">
            //            <div class="tit">亨恒贸易公司捐赠紧缺物资口罩4600个</div>
            //            <div class="txt">
            //                2月26日，亨恒贸易公司总经理尹永利在得知元宝区防控物资紧缺的现实情况后，捐赠4600个口罩，价值4.3万元，用于疫情防控一线工作。
            //                尹永利总经理表示：疫情当前，谁都不能置身事外，我希望通过自己的微薄之力，把正能量传递出去。
            //            </div>
            //            <div class="date">
            //                <div class="yueri">03/12</div>
            //                <div class="year">2020</div>
            //            </div>
            //        </div>
            //    </a>
            //</div>
            //<div class="list">
            //    <ul>
            //        <li>
            //            <a href="#">
            //                <div class="list_date">
            //                    <div class="yueri">03/12</div>
            //                    <div class="year">2020</div>
            //                </div>
            //                <div class="txt_box">
            //                    <div class="list_tit">丹东亨恒贸易有限公司位于秀美的鸭绿江畔</div>
            //                    <div class="list_txt">
            //                        丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。我公司是自主经营，独立核算，自负盈亏
            //                        具有独立法人资格的经济实体。公司目前共划分为办公室，财务部，业务部...
            //                    </div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="list_date">
            //                    <div class="yueri">03/12</div>
            //                    <div class="year">2020</div>
            //                </div>
            //                <div class="txt_box">
            //                    <div class="list_tit">丹东亨恒贸易有限公司位于秀美的鸭绿江畔</div>
            //                    <div class="list_txt">
            //                        丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。我公司是自主经营，独立核算，自负盈亏
            //                        具有独立法人资格的经济实体。公司目前共划分为办公室，财务部，业务部...
            //                    </div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="list_date">
            //                    <div class="yueri">03/12</div>
            //                    <div class="year">2020</div>
            //                </div>
            //                <div class="txt_box">
            //                    <div class="list_tit">丹东亨恒贸易有限公司位于秀美的鸭绿江畔</div>
            //                    <div class="list_txt">
            //                        丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。我公司是自主经营，独立核算，自负盈亏
            //                        具有独立法人资格的经济实体。公司目前共划分为办公室，财务部，业务部...
            //                    </div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="list_date">
            //                    <div class="yueri">03/12</div>
            //                    <div class="year">2020</div>
            //                </div>
            //                <div class="txt_box">
            //                    <div class="list_tit">丹东亨恒贸易有限公司位于秀美的鸭绿江畔</div>
            //                    <div class="list_txt">
            //                        丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。我公司是自主经营，独立核算，自负盈亏
            //                        具有独立法人资格的经济实体。公司目前共划分为办公室，财务部，业务部...
            //                    </div>
            //                </div>
            //            </a>
            //        </li>
            //    </ul>
            //</div>
            //<div class="more"><a href="#">查看全部</a></div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                var c = dmc.find(n => n.Caenname == "gsdt");
                if (c != null)
                {
                    sb.Append("<div class=\"title\">" + c.Catalogname + "</div>");
                    sb.Append("<div class=\"entit\">" + c.subtitle + "</div>");
                    var li = di.FindList(n => n.classid == c.Catalogid, 5, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    if (li.Count > 0)
                    {
                        int num = 0;
                        string css = "", href = "";
                        foreach (var i in li)
                        {
                            href = nl.getContLink(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id;
                            if (num == 0)
                            {
                                sb.Append("<div class=\"first\">");
                                sb.Append("    <a href=\"" + href + "\">");
                                sb.Append("        <div class=\"slt\"><img src=\"" + i.defaultpic + "\"></div>");
                                sb.Append("        <div class=\"wenzi\">");
                                sb.Append("            <div class=\"tit\">" + i.title + "</div>");
                                sb.Append("            <div class=\"txt\">" + common.common.DelHTML(i.intro) + "</div>");
                                sb.Append("            <div class=\"date\">");
                                sb.Append("                <div class=\"yueri\">" + common.common.get_format_for_nums(i.insertdate.Value.Month) + "/" + common.common.get_format_for_nums(i.insertdate.Value.Day) + "</div>");
                                sb.Append("                <div class=\"year\">" + i.insertdate.Value.Year + "</div>");
                                sb.Append("            </div>");
                                sb.Append("        </div>");
                                sb.Append("    </a>");
                                sb.Append("</div>");
                            }
                            else
                            {
                                if (num == 1)
                                {
                                    sb.Append("<div class=\"list\">");
                                    sb.Append("    <ul>");
                                }
                                sb.Append("        <li>");
                                sb.Append("            <a href=\"" + href + "\">");
                                sb.Append("                <div class=\"list_date\">");
                                sb.Append("                    <div class=\"yueri\">" + common.common.get_format_for_nums(i.insertdate.Value.Month) + "/" + common.common.get_format_for_nums(i.insertdate.Value.Day) + "</div>");
                                sb.Append("                    <div class=\"year\">" + i.insertdate.Value.Year + "</div>");
                                sb.Append("                </div>");
                                sb.Append("                <div class=\"txt_box\">");
                                sb.Append("                    <div class=\"list_tit\">" + i.title + "</div>");
                                sb.Append("                    <div class=\"list_txt\">" + common.common.DelHTML(i.intro) + "</div>");
                                sb.Append("                </div>");
                                sb.Append("            </a>");
                                sb.Append("        </li>");
                                if (num == li.Count - 1)
                                {
                                    sb.Append("    </ul>");
                                    sb.Append("</div>");
                                }
                            }
                            num++;
                        }
                    }
                    sb.Append("<div class=\"more\"><a href=\"" + nl.getUrlLink(c) + "\">查看全部</a></div>");
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 英文版首页公司动态
        public string index_company_news_en()
        {
            //<div class="title">公司动态</div>
            //<div class="entit">Company news</div>
            //<div class="first">
            //    <a href="#">
            //        <div class="slt"><img src="/web_images/dongtai_13.jpg"></div>
            //        <div class="wenzi">
            //            <div class="tit">亨恒贸易公司捐赠紧缺物资口罩4600个</div>
            //            <div class="txt">
            //                2月26日，亨恒贸易公司总经理尹永利在得知元宝区防控物资紧缺的现实情况后，捐赠4600个口罩，价值4.3万元，用于疫情防控一线工作。
            //                尹永利总经理表示：疫情当前，谁都不能置身事外，我希望通过自己的微薄之力，把正能量传递出去。
            //            </div>
            //            <div class="date">
            //                <div class="yueri">03/12</div>
            //                <div class="year">2020</div>
            //            </div>
            //        </div>
            //    </a>
            //</div>
            //<div class="list">
            //    <ul>
            //        <li>
            //            <a href="#">
            //                <div class="list_date">
            //                    <div class="yueri">03/12</div>
            //                    <div class="year">2020</div>
            //                </div>
            //                <div class="txt_box">
            //                    <div class="list_tit">丹东亨恒贸易有限公司位于秀美的鸭绿江畔</div>
            //                    <div class="list_txt">
            //                        丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。我公司是自主经营，独立核算，自负盈亏
            //                        具有独立法人资格的经济实体。公司目前共划分为办公室，财务部，业务部...
            //                    </div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="list_date">
            //                    <div class="yueri">03/12</div>
            //                    <div class="year">2020</div>
            //                </div>
            //                <div class="txt_box">
            //                    <div class="list_tit">丹东亨恒贸易有限公司位于秀美的鸭绿江畔</div>
            //                    <div class="list_txt">
            //                        丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。我公司是自主经营，独立核算，自负盈亏
            //                        具有独立法人资格的经济实体。公司目前共划分为办公室，财务部，业务部...
            //                    </div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="list_date">
            //                    <div class="yueri">03/12</div>
            //                    <div class="year">2020</div>
            //                </div>
            //                <div class="txt_box">
            //                    <div class="list_tit">丹东亨恒贸易有限公司位于秀美的鸭绿江畔</div>
            //                    <div class="list_txt">
            //                        丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。我公司是自主经营，独立核算，自负盈亏
            //                        具有独立法人资格的经济实体。公司目前共划分为办公室，财务部，业务部...
            //                    </div>
            //                </div>
            //            </a>
            //        </li>
            //        <li>
            //            <a href="#">
            //                <div class="list_date">
            //                    <div class="yueri">03/12</div>
            //                    <div class="year">2020</div>
            //                </div>
            //                <div class="txt_box">
            //                    <div class="list_tit">丹东亨恒贸易有限公司位于秀美的鸭绿江畔</div>
            //                    <div class="list_txt">
            //                        丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。我公司是自主经营，独立核算，自负盈亏
            //                        具有独立法人资格的经济实体。公司目前共划分为办公室，财务部，业务部...
            //                    </div>
            //                </div>
            //            </a>
            //        </li>
            //    </ul>
            //</div>
            //<div class="more"><a href="#">查看全部</a></div>
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                NavLIst nl = new NavLIst(s);
                var c = dmc.find(n => n.Caenname == "companydynamics");
                if (c != null)
                {
                    sb.Append("<div class=\"title\">" + c.Catalogname + "</div>");
                    sb.Append("<div class=\"entit\">" + c.subtitle + "</div>");
                    var li = di.FindList(n => n.classid == c.Catalogid, 5, new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = true }, new OrderModelField { propertyName = "id", IsDESC = true } }).ToList();
                    if (li.Count > 0)
                    {
                        int num = 0;
                        string css = "", href = "";
                        foreach (var i in li)
                        {
                            href = nl.getContLink_en(dmc.find(n => n.Catalogid == i.classid)) + "?id=" + i.id;
                            if (num == 0)
                            {
                                sb.Append("<div class=\"first\">");
                                sb.Append("    <a href=\"" + href + "\">");
                                sb.Append("        <div class=\"slt\"><img src=\"" + i.defaultpic + "\"></div>");
                                sb.Append("        <div class=\"wenzi\">");
                                sb.Append("            <div class=\"tit\">" + i.title + "</div>");
                                sb.Append("            <div class=\"txt\">" + common.common.DelHTML(i.intro) + "</div>");
                                sb.Append("            <div class=\"date\">");
                                sb.Append("                <div class=\"yueri\">" + common.common.get_format_for_nums(i.insertdate.Value.Month) + "/" + common.common.get_format_for_nums(i.insertdate.Value.Day) + "</div>");
                                sb.Append("                <div class=\"year\">" + i.insertdate.Value.Year + "</div>");
                                sb.Append("            </div>");
                                sb.Append("        </div>");
                                sb.Append("    </a>");
                                sb.Append("</div>");
                            }
                            else
                            {
                                if (num == 1)
                                {
                                    sb.Append("<div class=\"list\">");
                                    sb.Append("    <ul>");
                                }
                                sb.Append("        <li>");
                                sb.Append("            <a href=\"" + href + "\">");
                                sb.Append("                <div class=\"list_date\">");
                                sb.Append("                    <div class=\"yueri\">" + common.common.get_format_for_nums(i.insertdate.Value.Month) + "/" + common.common.get_format_for_nums(i.insertdate.Value.Day) + "</div>");
                                sb.Append("                    <div class=\"year\">" + i.insertdate.Value.Year + "</div>");
                                sb.Append("                </div>");
                                sb.Append("                <div class=\"txt_box\">");
                                sb.Append("                    <div class=\"list_tit\">" + i.title + "</div>");
                                sb.Append("                    <div class=\"list_txt\">" + common.common.DelHTML(i.intro) + "</div>");
                                sb.Append("                </div>");
                                sb.Append("            </a>");
                                sb.Append("        </li>");
                                if (num == li.Count - 1)
                                {
                                    sb.Append("    </ul>");
                                    sb.Append("</div>");
                                }
                            }
                            num++;
                        }
                    }
                    sb.Append("<div class=\"more\"><a href=\"" + nl.getUrlLink_en(c) + "\">View all</a></div>");
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 首页走进亨恒
        public string index_about_ddhh()
        {
            //<div class="title">走进亨恒</div>
            //<div class="entit">about ddhh</div>
            //<div class="top">
            //    <div class="slt"><img src="/web_images/gsjj_14.jpg"></div>
            //    <div class="wenzi">
            //        <div class="tit">公司简介</div>
            //        <div class="txt">
            //            丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。我公司是自主经营，独立核算，
            //            自负盈亏具有独立法人资格的经济实体。公司目前共划分为办公室，财务部，业务部，仓储物流部，四个部门。公司在尹永利总经理的带领下致力于
            //            中朝两国贸易已有十余年时间，公司业务覆盖粮食，化肥，纯碱，电子配件，钟表，玩具，摆件，化妆品，纸类，五金工具，轴承，轮胎的对朝出口
            //            业务以及朝鲜优质矿产品的进口与转口业务。
            //        </div>
            //        <div class="more"><a href="#">了解更多></a></div>
            //    </div>
            //</div>
            //<div class="bom">
            //    <div class="wenzi">
            //        <div class="tit">企业文化</div>
            //        <div class="txt">
            //            丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。我公司是自主经营，独立核算，
            //            自负盈亏具有独立法人资格的经济实体。公司目前共划分为办公室，财务部，业务部，仓储物流部，四个部门。公司在尹永利总经理的带领下致力于
            //            中朝两国贸易已有十余年时间，公司业务覆盖粮食，化肥，纯碱，电子配件，钟表，玩具，摆件，化妆品，纸类，五金工具，轴承，轮胎的对朝出口
            //            业务以及朝鲜优质矿产品的进口与转口业务。
            //        </div>
            //        <div class="more"><a href="#">了解更多></a></div>
            //    </div>
            //    <div class="slt"><img src="/web_images/qywh_17.jpg"></div>
            //</div>
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
                    sb.Append("<div class=\"title\">" + c.Catalogname + "</div>");
                    sb.Append("<div class=\"entit\">" + c.subtitle + "</div>");
                    var cc1 = dmc.find(n => n.Caenname == "gsjj");
                    var cc2 = dmc.find(n => n.Caenname == "qywh");
                    if(cc1 != null)
                    {
                        string cc1href = nl.getUrlLink(cc1);
                        var cc1ggw = dg.find(n => n.ggwposition == "gsjj");
                        var cc1info = di.find(n => n.classid == cc1.Catalogid);
                        if(cc1info != null)
                        {
                            sb.Append("<div class=\"top\">");
                            if (cc1ggw != null)
                            {
                                sb.Append("    <div class=\"slt\"><img src=\"" + cc1ggw.imgurl + "\"></div>");
                            }
                            sb.Append("    <div class=\"wenzi\">");
                            sb.Append("        <div class=\"tit\">" + cc1.Catalogname + "</div>");
                            sb.Append("        <div class=\"txt\">" + common.common.DelHTML(cc1info.intro) + "</div>");
                            sb.Append("        <div class=\"more\"><a href=\"" + cc1href + "\">了解更多></a></div>");
                            sb.Append("    </div>");
                            sb.Append("</div>");
                        }
                    }
                    if(cc2 != null)
                    {
                        string cc2href = nl.getUrlLink(cc2);
                        var cc2ggw = dg.find(n => n.ggwposition == "qywh");
                        var cc2info = di.find(n => n.classid == cc2.Catalogid);
                        if(cc2info != null)
                        {
                            sb.Append("<div class=\"bom\">");
                            sb.Append("    <div class=\"wenzi\">");
                            sb.Append("        <div class=\"tit\">" + cc2.Catalogname + "</div>");
                            sb.Append("        <div class=\"txt\">" + common.common.DelHTML(cc2info.intro) + "</div>");
                            sb.Append("        <div class=\"more\"><a href=\""+ cc2href + "\">了解更多></a></div>");
                            sb.Append("    </div>");
                            if (cc2ggw != null)
                            {
                                sb.Append("    <div class=\"slt\"><img src=\"" + cc2ggw.imgurl + "\"></div>");
                            }
                            sb.Append("</div>");
                        }
                    }
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 英文版首页走进亨恒
        public string index_about_ddhh_en()
        {
            //<div class="title">走进亨恒</div>
            //<div class="entit">about ddhh</div>
            //<div class="top">
            //    <div class="slt"><img src="/web_images/gsjj_14.jpg"></div>
            //    <div class="wenzi">
            //        <div class="tit">公司简介</div>
            //        <div class="txt">
            //            丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。我公司是自主经营，独立核算，
            //            自负盈亏具有独立法人资格的经济实体。公司目前共划分为办公室，财务部，业务部，仓储物流部，四个部门。公司在尹永利总经理的带领下致力于
            //            中朝两国贸易已有十余年时间，公司业务覆盖粮食，化肥，纯碱，电子配件，钟表，玩具，摆件，化妆品，纸类，五金工具，轴承，轮胎的对朝出口
            //            业务以及朝鲜优质矿产品的进口与转口业务。
            //        </div>
            //        <div class="more"><a href="#">了解更多></a></div>
            //    </div>
            //</div>
            //<div class="bom">
            //    <div class="wenzi">
            //        <div class="tit">企业文化</div>
            //        <div class="txt">
            //            丹东亨恒贸易有限公司位于秀美的鸭绿江畔，毗邻朝鲜民主主义人民共和国。我公司是自主经营，独立核算，
            //            自负盈亏具有独立法人资格的经济实体。公司目前共划分为办公室，财务部，业务部，仓储物流部，四个部门。公司在尹永利总经理的带领下致力于
            //            中朝两国贸易已有十余年时间，公司业务覆盖粮食，化肥，纯碱，电子配件，钟表，玩具，摆件，化妆品，纸类，五金工具，轴承，轮胎的对朝出口
            //            业务以及朝鲜优质矿产品的进口与转口业务。
            //        </div>
            //        <div class="more"><a href="#">了解更多></a></div>
            //    </div>
            //    <div class="slt"><img src="/web_images/qywh_17.jpg"></div>
            //</div>
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
                    sb.Append("<div class=\"title\">" + c.Catalogname + "</div>");
                    sb.Append("<div class=\"entit\">" + c.subtitle + "</div>");
                    var cc1 = dmc.find(n => n.Caenname == "companyprofile");
                    var cc2 = dmc.find(n => n.Caenname == "corporateculture");
                    if (cc1 != null)
                    {
                        string cc1href = nl.getUrlLink_en(cc1);
                        var cc1ggw = dg.find(n => n.ggwposition == "gsjj_en");
                        var cc1info = di.find(n => n.classid == cc1.Catalogid);
                        if (cc1info != null)
                        {
                            sb.Append("<div class=\"top\">");
                            if (cc1ggw != null)
                            {
                                sb.Append("    <div class=\"slt\"><img src=\"" + cc1ggw.imgurl + "\"></div>");
                            }
                            sb.Append("    <div class=\"wenzi\">");
                            sb.Append("        <div class=\"tit\">" + cc1.Catalogname + "</div>");
                            sb.Append("        <div class=\"txt\">" + common.common.DelHTML(cc1info.intro) + "</div>");
                            sb.Append("        <div class=\"more\"><a href=\"" + cc1href + "\">Learn more></a></div>");
                            sb.Append("    </div>");
                            sb.Append("</div>");
                        }
                    }
                    if (cc2 != null)
                    {
                        string cc2href = nl.getUrlLink_en(cc2);
                        var cc2ggw = dg.find(n => n.ggwposition == "qywh_en");
                        var cc2info = di.find(n => n.classid == cc2.Catalogid);
                        if (cc2info != null)
                        {
                            sb.Append("<div class=\"bom\">");
                            sb.Append("    <div class=\"wenzi\">");
                            sb.Append("        <div class=\"tit\">" + cc2.Catalogname + "</div>");
                            sb.Append("        <div class=\"txt\">" + common.common.DelHTML(cc2info.intro) + "</div>");
                            sb.Append("        <div class=\"more\"><a href=\"" + cc2href + "\">Learn more></a></div>");
                            sb.Append("    </div>");
                            if (cc2ggw != null)
                            {
                                sb.Append("    <div class=\"slt\"><img src=\"" + cc2ggw.imgurl + "\"></div>");
                            }
                            sb.Append("</div>");
                        }
                    }
                }
            }
            return sb.ToString();
        }
        #endregion


      


        #region 英文版产品列表
        public string pro_list_en(List<view_info> l)
        {
            //<div class="prolist">
            //    <ul>
            //        <li class="bt">
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/home_16.jpg"></div>
            //                <div class="tit_box">
            //                    <div class="tit">产品名称产品名称产..</div>
            //                    <div class="price">￥120.00</div>
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
                    string css = "", href = "";
                    int num = 0;
                    sb.Append("<div class=\"prolist\"><ul>");
                    foreach (var o in l)
                    {
                        href = nl.getContLink_en(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        css = (num % 3 == 2) ? " class=\"bt\"" : "";
                        sb.Append("        <li" + css + ">");
                        sb.Append("            <a href=\"" + href + "\">");
                        sb.Append("                <div class=\"slt\"><img src=\"" + o.defaultpic + "\"></div>");
                        sb.Append("                <div class=\"tit_box\">");
                        sb.Append("                    <div class=\"tit\">" + o.title + "</div>");
                        sb.Append("                    <div class=\"price\">￥" + common.common.getMoneyType(double.Parse(common.common.IsNumeric_n(o.price))) + "</div>");
                        sb.Append("                </div>");
                        sb.Append("            </a>");
                        sb.Append("        </li>");
                        num++;
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        #endregion

    

        #region 英文版新闻列表
        public string news_list_en(List<view_info> l)
        {
            //<div class="news_list">
            //    <ul>
            //        <li>
            //            <a href="#">
            //                <div class="slt"><img src="/web_images/dongtai_13.jpg"></div>
            //                <div class="wenzi">
            //                    <div class="tit">亨恒贸易公司捐赠紧缺物资口罩4600个</div>
            //                    <div class="txt">
            //                        2月26日，亨恒贸易公司总经理尹永利在得知元宝区防控物资紧缺的现实情况后，捐赠4600个口罩，价值4.3万元，用于疫情防控一线工作。
            //                        尹永利总经理表示：疫情当前，谁都不能置身事外，我希望通过自己的微薄之力，把正能量传递出去。
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
                    sb.Append("<div class=\"news_list\"><ul>");
                    foreach (var o in l)
                    {
                        href = nl.getContLink_en(dmc.find(n => n.Catalogid == o.classid)) + "?id=" + o.id;
                        sb.Append("        <li>");
                        sb.Append("            <a href=\"" + href + "\">");
                        sb.Append("                <div class=\"slt\"><img src=\"" + o.defaultpic + "\"></div>");
                        sb.Append("                <div class=\"wenzi\">");
                        sb.Append("                    <div class=\"tit\">" + o.title + "</div>");
                        sb.Append("                    <div class=\"txt\">" + common.common.DelHTML(o.intro) + "</div>");
                        sb.Append("                    <div class=\"date\">" + o.insertdate.Value.Year + "/" + common.common.get_format_for_nums(o.insertdate.Value.Month) + "/" + common.common.get_format_for_nums(o.insertdate.Value.Day) + "</div>");
                        sb.Append("                </div>");
                        sb.Append("            </a>");
                        sb.Append("        </li>");
                    }
                    sb.Append("</ul></div>");
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 留言板
        public string message_borad()
        {
            //<div class="Message">
            //                <div class="txt">
            //                    如果您对我们的产品及服务有任何意见或建议，<br />
            //                    可以通过留言的方式告诉我们。
            //                </div>
            //                <div class="tx_box">
            //                    <ul>
            //                        <li>
            //                            <div class="tit">称呼：</div>
            //                            <input type="text">
            //                        </li>
            //                        <li>
            //                            <div class="tit">电话：</div>
            //                            <input type="text">
            //                        </li>

            //                        <li>
            //                            <div class="tit">邮箱：</div>
            //                            <input type="text">
            //                        </li>
            //                        <li class="text">
            //                            <div class="tit">咨询内容：</div>
            //                            <textarea type="text"></textarea>
            //                        </li>
            //                        <li>
            //                            <div class="tit">验证码：</div>
            //                            <input type="text">
            //                            <img src="web_images/yzm_11.jpg">
            //                        </li>
            //                    </ul>
            //                </div>
            //                <div class="btn">
            //                    <ul>
            //                        <li class="red"><a href="#">提交留言</a></li>
            //                        <li class="grey"><a href="#">重置</a></li>
            //                    </ul>
            //                </div>
            //            </div>
            StringBuilder sb = new StringBuilder();
            sb.Append("<script src=\"/layer/layer.js\" type=\"text/javascript\"></script> \r\n");
            sb.Append("<script src=\"/web_js/savebook.js\" type=\"text/javascript\"></script> \r\n");
            sb.Append("<div class=\"Message\">\r\n");
            sb.Append(" <form name=\"form1\" id=\"form1\"> \r\n");
            sb.Append("<div class=\"txt\">\r\n");
            sb.Append("如果您对我们的产品及服务有任何意见或建议，<br />\r\n");
            sb.Append("可以通过留言的方式告诉我们。\r\n");
            sb.Append("</div>\r\n");
            sb.Append("<div class=\"tx_box\">\r\n");
            sb.Append("<ul>\r\n");
            sb.Append("<li>\r\n");
            sb.Append("<div class=\"tit\">称呼：</div>\r\n");
            sb.Append("<input type=\"text\" id=\"name\" name=\"name\" >\r\n");
            sb.Append("</li>\r\n");
            sb.Append("<li>\r\n");
            sb.Append("<div class=\"tit\">电话：</div>\r\n");
            sb.Append("<input type=\"text\" id=\"tel\" name=\"tel\" >\r\n");
            sb.Append("</li>\r\n");
            sb.Append("<li>\r\n");
            sb.Append("<div class=\"tit\">邮箱：</div>\r\n");
            sb.Append("<input type=\"text\" id=\"email\" name=\"email\" >\r\n");
            sb.Append("</li>\r\n");
            sb.Append("<li class=\"text\">\r\n");
            sb.Append("<div class=\"tit\">咨询内容：</div>\r\n");
            sb.Append("<textarea  id=\"cont\" name=\"cont\" type=\"text\"></textarea>\r\n");
            sb.Append("</li>\r\n");
            sb.Append("<li>\r\n");
            sb.Append("<div class=\"tit\">验证码：</div>\r\n");
            sb.Append("<input type=\"text\" id=\"checkcode\" name=\"checkcode\">\r\n");
            sb.Append("<img class=\"ckeckcode\"src=\"/Home/yzm\" style=\"cursor: pointer\" onclick=\"this.src ='/Home/yzm?t=' + Math.random()\" //>\r\n");
            //sb.Append("<img src=\"web_images/yzm_11.jpg\">\r\n");
            sb.Append("</li>\r\n");
            sb.Append("</ul>\r\n");
            sb.Append("</div>\r\n");
            sb.Append("<div class=\"btn\">\r\n");
            sb.Append("<ul>\r\n");
            sb.Append("<li class=\"red\"><a  href=\"javascript:;\" onclick=\"submit()\">提交留言</a></li>\r\n");
            sb.Append("<li class=\"grey\"><a href=\"javascript:;\" onclick=\"reset()\">重置</a></li>\r\n");
            sb.Append("</ul>\r\n");
            sb.Append("</div>\r\n");
            sb.Append(" </form> \r\n");
            sb.Append("</div>\r\n");
            return sb.ToString();
        }
        #endregion

        #region 英文版留言板
        public string message_borad_en()
        {
            //<div class="message">
            //<script src="/layer/layer.js" type="text/javascript"></script>
            //<script type="text/javascript"> 
            //    function submit() { 
            //        var data = $("#form1").serialize() 
            //        $.post("/apiguestbook/savebook", data, function (res) { 
            //            if (res.state == 100) { 
            //                layer.alert("提交成功！", function () { 
            //                    window.location.href = "/" 
            //                }); 
            //                //layer.open({ 
            //                //    content: '提交成功！', btn: '我知道了', yes: function () { window.location.href = "/h5/" } 
            //                //}); 
            //            }
            //            else {
            //                layer.alert(res.data);
            //                //layer.open({
            //                //    content: res.data, btn: '我知道了'
            //                //});
            //            }
            //        }); 
            //    } 
            //</script> 
            //    <form name="form1" id="form1">
            //    <div class="write">
            //        <h3><span>*</span>姓　　名：</h3>
            //        <input id="name" name="name" type="text" /></div>
            //    <div class="write">
            //        <h3><span>*</span>联系电话：</h3>
            //        <input id="tel" name="tel" type="text" /></div>
            //    <div class="write write1">
            //        <h3><span>*</span>留言内容：</h3>
            //        <textarea id="content" name="content"></textarea></div>
            //    <div class="btn"><a href="javascript:re_post_bar.submit('saveguest', 'form1');" class="submit">提交</a><a href="javascript:re_post_bar.reset();">重置</a></div>
            //        </form>
            //</div>
            StringBuilder sb = new StringBuilder();
            sb.Append("<script src=\"/layer/layer.js\" type=\"text/javascript\"></script> \r\n");
            sb.Append("<script src=\"/web_js/savebook_en.js\" type=\"text/javascript\"></script> \r\n");


            //sb.Append("    <div class=\"write code\"><h2>验证码</h2><input id=\"checkcode\" name=\"checkcode\" type=\"text\" /><img id=\"yzm\" src=\"/management/getCheckCode\" width=\"100\" height=\"40\" style=\"cursor:pointer\" onclick=\"this.src = '/management/getCheckCode?t=' + Math.random()\" /><a href=\"javascript:changeimg();\">看不清，换一张</a></div>");

            //System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath("/map.html"));
            //if (directory.Exists)//存在
            //{ 
            //sb.Append("<br /><iframe src=\"/map.html\" width=\"100%\" height=\"550\" style=\"overflow:hidden; border:0;\"></iframe> \r\n");
            //}


            sb.Append("<div class=\"Message\"> \r\n");
            sb.Append(" <form name=\"form1\" id=\"form1\"> \r\n");
            sb.Append("    <div class=\"liuyan_box\"> \r\n");
            sb.Append("        <div class=\"shuru_left\"> \r\n");
            sb.Append("            <div class=\"tit\">Your name：</div> \r\n");
            sb.Append("            <input id=\"name\" name=\"name\" type=\"text\"> \r\n");
            sb.Append("        </div> \r\n");
            sb.Append("        <div class=\"shuru\"> \r\n");
            sb.Append("            <div class=\"tit\">Message phone：</div> \r\n");
            sb.Append("            <input id=\"tel\" name=\"tel\" type=\"text\"> \r\n");
            sb.Append("        </div> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append("    <div class=\"liuyan_box\"> \r\n");
            sb.Append("        <div class=\"shuru_left\"> \r\n");
            sb.Append("            <div class=\"tit\">Your gender：</div> \r\n");
            sb.Append("            <input id=\"sex\" name=\"sex\" type=\"text\"> \r\n");
            sb.Append("        </div> \r\n");
            sb.Append("        <div class=\"shuru\"> \r\n");
            sb.Append("            <div class=\"tit\">Message mailbox：</div> \r\n");
            sb.Append("            <input id=\"email\" name=\"email\" type=\"text\"> \r\n");
            sb.Append("        </div> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append("    <div class=\"liuyan_box\"> \r\n");
            sb.Append("        <div class=\"lynr\"> \r\n");
            sb.Append("            <div class=\"tit\">Message content：</div> \r\n");
            sb.Append("            <textarea id=\"cont\" name=\"cont\" type=\"text\"></textarea> \r\n");
            sb.Append("        </div> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append("    <div class=\"btn_box\"> \r\n");
            sb.Append("        <div class=\"btn\"> \r\n");
            sb.Append("            <a href=\"javascript:;\" onclick=\"submit()\" class=\"btn_tj\">Submit message</a> \r\n");
            sb.Append("            <a href=\"javascript:;\" onclick=\"reset()\" class=\"btn_cz\">Message reset</a> \r\n");
            sb.Append("        </div> \r\n");
            sb.Append("    </div> \r\n");
            sb.Append(" </form> \r\n");
            sb.Append("</div> \r\n");

            return sb.ToString();
        }
        #endregion


       

        #region 产品内容页
        public string pro_content(List<view_info> l)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count > 0)
            {
                sb.Append("<div class=\"info\">");
                sb.Append("    <div class=\"toptit\">");
                sb.Append("        <div class=\"tit\">" + l[0].title + "</div>");
                sb.Append("        <div class=\"price\">￥：<span>"+ common.common.getMoneyType(double.Parse(common.common.IsNumeric_n(l[0].price))) + "</span></div>");
                sb.Append("    </div>");
                sb.Append("    <div class=\"txt\">");
                sb.Append(l[0].cont);
                sb.Append("    </div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion
        #region 新闻内容页
        public string news_content(List<view_info> l)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count > 0)
            {
                sb.Append("<div class=\"nr\">");
                sb.Append("        <div class=\"fbtit\">" + l[0].title + "</div>");
                sb.Append("        <div class=\"fb_date\">发表时间 ： " + common.common.get_date_format_for_nums(l[0].insertdate.Value) + "</div>");
                sb.Append(l[0].cont);
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion

        #region 英文版新闻内容页
        public string news_content_en(List<view_info> l)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count > 0)
            {
                sb.Append("<div class=\"info\">");
                sb.Append("    <div class=\"toptit\">");
                sb.Append("        <div class=\"tit\">" + l[0].title + "</div>");
                sb.Append("        <div class=\"date\">Time of publication ： " + common.common.get_date_format_for_nums(l[0].insertdate.Value) + "</div>");
                sb.Append("    </div>");
                sb.Append("    <div class=\"txt\">");
                sb.Append(l[0].cont);
                sb.Append("    </div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        #endregion


        #region 内容页
        public string get_cont_page(info o)
        {
            string Html = "";
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                if (o != null)
                {
                    var i = new view_info { classid = o.classid, cont = o.cont, defaultpic = o.defaultpic, h5cont = o.h5cont, id = o.id, insertdate = o.insertdate, intro = o.intro, issame = o.issame, price = o.price, title = o.title };
                    var c = dmc.find(n => n.Catalogid == o.classid);
                    if(c != null)
                    {
                        switch (c.pclisttype)
                        {
                            case "pro-list"://产品列表
                                Html = news_content(new List<view_info> { i });
                                break;
                            case "news-list"://新闻列表
                                Html = news_content(new List<view_info> { i });
                                break;
                            case "news-huanjing-list"://产品列表
                                Html = news_content(new List<view_info> { i });
                                break;
                            case "news-shipin-list"://新闻列表
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
        public string get_cont_page_en(info o)
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