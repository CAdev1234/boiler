using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using System.Net;
using System.Drawing;
using System.Collections.Generic;
using System.Net.Mail;

namespace ykmWeb.common
{
    public class common
    {


        /// <summary>
        /// MD5加密程序去6-13位
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static string Md5(string password)
        {
            MD5CryptoServiceProvider hashmd5;
            hashmd5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(hashmd5.ComputeHash(Encoding.Default.GetBytes(password))).Replace("-", "").Substring(6, 13);
        }
        /// <summary>
        /// MD5加密程序
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static string Md5all(string password)
        {
            MD5CryptoServiceProvider hashmd5;
            hashmd5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(hashmd5.ComputeHash(Encoding.Default.GetBytes(password))).Replace("-", "");
        }
        /// <summary>
        /// 错误信息提示，并返回历史页面
        /// </summary>
        /// <param name="ErrMessage">错误信息提示</param>
        /// <returns></returns>
        public static string ErrorInfo(string ErrMessage)
        {
            string ErrorHTML = "";

            ErrorHTML = "<script language='javascript'>" +
                "alert('" + ErrMessage + "!');window.history.go(-1);</script>";
            return ErrorHTML;
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sStr">字符</param>
        /// <param name="Len">截取长度</param>
        /// <returns></returns>
        public static string GetSubString(string sStr, int Len)
        {
            if (string.IsNullOrEmpty(sStr))
            {
                return sStr;
            }

            if (sStr.Length > Len)
            {
                return sStr.Substring(0, Len) + "...";
            }
            else
            {
                return sStr;
            }
        }
        /// <summary>
        /// 成功提交后显示信息
        /// </summary>
        /// <param name="Message">提示信息</param>
        /// <param name="ToURL">跳转地址</param>
        /// <returns></returns>
        public static string SuccessInfo(string Message, string ToURL)
        {
            string ResultrHTML = "";
            ResultrHTML = "<script language='javascript'>" +
                    "alert('" + Message + "!');" + "window.location.href='" + ToURL + "';" +
                    "</script>";

            return ResultrHTML;
        }
        /// <summary>
        /// 成功提交后显示信息
        /// </summary>
        /// <param name="Message">提示信息</param>
        /// <param name="ToURL">跳转地址</param>
        /// <returns></returns>
        public static string SuccessInfo_parent(string ToURL)
        {
            string ResultrHTML = "";
            ResultrHTML = "<script language='javascript'>parent.location.href='" + ToURL + "';" +
                    "</script>";

            return ResultrHTML;
        }
        /// <summary>
        /// 错误信息后关闭页面
        /// </summary>
        /// <param name="ErrMessage">错误提示</param>
        /// <returns></returns>
        public static string ErrorCloseWindow(string ErrMessage)
        {
            string ErrorHTML = "";

            ErrorHTML = "<script language='javascript'>" +
                "alert('" + ErrMessage + "!');window.close();</script>";
            return ErrorHTML;
        }
        public static string IsNumeric(string strVal)
        {
            try
            {
                if (strVal.Length <= 9)
                {
                    try
                    {
                        int.Parse(strVal);
                        return strVal;
                    }
                    catch
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";
                }
            }
            catch
            {
                return "0";
            }
        }
        public static string IsNumeric_n(string strVal)
        {
            try
            {
                if (strVal.Length <= 9)
                {
                    try
                    {
                        decimal.Parse(strVal);
                        return strVal;
                    }
                    catch
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";
                }
            }
            catch
            {
                return "0";
            }
        }

        public static string reRandStr(int length)
        {
            byte[] random = new Byte[length / 2];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(random);
            string timestr = DateTime.Now.Ticks.ToString();
            StringBuilder sb = new StringBuilder(length);
            int i;
            for (i = 0; i < random.Length; i++)
            {
                sb.Append(String.Format("{0:X2}", random[i]));
            }
            sb.Append(timestr);
            return sb.ToString();
        }

        public static string reRand_abc(int length)
        {
            byte[] random = new Byte[length / 2];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(random);
            StringBuilder sb = new StringBuilder(length);
            int i;
            for (i = 0; i < random.Length; i++)
            {
                sb.Append(String.Format("{0:X2}", random[i]));
            }
            return sb.ToString();
        }

        public static int reRandRoundNum(int s,int e)
        {
            Random ra = new Random();
           return  ra.Next(s, e);
        }


        public static string Left(string sSource, int iLength)
        {
            return sSource.Substring(0, iLength > sSource.Length ? sSource.Length : iLength);
        }

        public static string Right(string sSource, int iLength)
        {
            return sSource.Substring(iLength > sSource.Length ? 0 : sSource.Length - iLength);
        }

        public static string Mid(string sSource, int iStart, int iLength)
        {
            int iStartPoint = iStart > sSource.Length ? sSource.Length : iStart;
            return sSource.Substring(iStartPoint, iStartPoint + iLength > sSource.Length ? sSource.Length - iStartPoint : iLength);
        }

        /// <summary>
        /// If don't input key , Use default key
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encode(string str)
        {
            return Encode(str, "qjgqjwjk");
        }
        /// <summary>
        /// Des 加密 GB2312 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encode(string str, string key)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(str);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                StringBuilder builder = new StringBuilder();
                foreach (byte num in stream.ToArray())
                {
                    builder.AppendFormat("{0:X2}", num);
                }
                stream.Close();
                return builder.ToString();
            }

        }
        /// <summary>
        /// if don't input key , Use default key
        /// </summary>
        /// <param name="str">Desc string </param>
        /// <returns></returns>
        public static string Decode(string str)
        {
            return Decode(str, "qjgqjwjk");
        }
        /// <summary>
        /// Des 解密 GB2312 
        /// </summary>
        /// <param name="str">Desc string</param>
        /// <param name="key">Key ,必须为8位 </param>
        /// <returns></returns>
        public static string Decode(string str, string key)
        {
            try
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                byte[] buffer = new byte[str.Length / 2];
                for (int i = 0; i < (str.Length / 2); i++)
                {
                    int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);
                    buffer[i] = (byte)num2;
                }
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                stream.Close();
                return Encoding.GetEncoding("GB2312").GetString(stream.ToArray());
            }
            catch
            {
                return "0";
            }
        }
        public static bool isId_inChild(string cid, string parentStr)
        {
            bool reState = false;
            if (parentStr.IndexOf(',') != -1)
            {
                string[] arrP = parentStr.Split(',');
                for (int i = 0; i < arrP.Length; i++)
                {
                    if (arrP[i] == cid)
                    {
                        reState = true;
                        break;
                    }
                }
            }
            else
            {
                if (cid == parentStr)
                {
                    reState = true;
                }
            }
            return reState;
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
        /// <param name="language">语言</param>
        /// <param name="isshownum">是否显示数字页码</param>
        /// <param name="showte">是否显示首页和尾页</param>
        /// <returns></returns>
        public static string PageFoot_zc(int totle, int pagesize, string pageIndex, string pagename, string pageLinkText, string language, bool isshownum, bool showte, bool jumppage)
        {
            StringBuilder sb = new StringBuilder();
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
                if (IsNumeric(pageIndex) == "0")
                {
                    pageIndex = "1";
                }
                if (IsNumeric(pageIndex) != "0")
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
                        sb.Append("<a href=\"" + pagename + "?Page=1" + pageLinkText + "\">首页</a>");
                    }
                    sb.Append("<a href=\"" + pagename + "?Page=" + Convert.ToString(CurPage - 1) + pageLinkText + "\" class=\"prev\"><span>〈</span> 上一页</a>");
                }
                else
                {
                    if (showte == true)
                    {
                        sb.Append("<a href=\"javascript:;\">首页</a>");
                    }

                    sb.Append("<a href=\"javascript:;\" class=\"prev\"><span>〈</span> 上一页</a>");
                }
                if (isshownum == true)
                {
                    if (pagecont <= 6)//如果页数小于等于6
                    {
                        for (int i = 1; i <= pagecont; i++)
                        {
                            if (i == CurPage)
                            {
                                activeStyle = "style=\"color:#ffffff;background-color:#88cc33;\"";
                            }
                            else
                            {
                                activeStyle = "";
                            }
                            sb.Append("<a href=\"" + pagename + "?Page=" + i.ToString() + pageLinkText + "\" " + activeStyle + " >" + i.ToString() + "</a>");
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

                                activeStyle = "style=\"color:#ffffff;background-color:#88cc33;\"";

                                sb.Append("<a href=\"" + pagename + "?Page=" + i.ToString() + pageLinkText + "\" " + activeStyle + " >" + i.ToString() + "</a>");
                            }
                            else
                            {
                                sb.Append("<a href=\"" + pagename + "?Page=" + i.ToString() + pageLinkText + "\">" + i.ToString() + "</a>");
                            }
                        }

                        if (pagecont >= 18 && CurPage < (pagecont - 7))
                        {
                            sb.Append("<a href=\"javascript:;\">....</a>");
                            for (int i = pagecont - 2; i <= pagecont; i++)
                            {
                                if (i == CurPage)
                                {
                                    activeStyle = "style=\"color:#ffffff;background-color:#88cc33;\"";
                                }
                                else
                                {
                                    activeStyle = "";
                                }
                                sb.Append("<a href=\"" + pagename + "?Page=" + i.ToString() + pageLinkText + "\" " + activeStyle + " >" + i.ToString() + "</a>");
                            }
                        }
                    }
                }
                if (CurPage < pagecont)
                {
                    sb.Append("<a href=\"" + pagename + "?Page=" + Convert.ToString(CurPage + 1) + pageLinkText + "\"  class=\"next\">下一页 <span>〉</span></a>");
                    if (showte == true)
                    {
                        sb.Append("<a href=\"" + pagename + "?Page=" + pagecont.ToString() + pageLinkText + "\">尾页</a>");
                    }
                }
                else
                {
                    sb.Append("<a href=\"javascript:;\"  class=\"next\">下一页 <span>〉</span></a>");
                    if (showte == true)
                    {
                        sb.Append("<a href=\"javascript:;\">尾页</a>");
                    }
                }
                if (jumppage)
                {
                    sb.Append("<span class=\"all\">共" + pagecont + "页</span><span>到第<input type=\"text\" id=\"pagenum\"  value=\"" + CurPage + "\"/>页</span><a href=\"javascript:;\" class=\"sure\" id=\"pagebtn\">确定</a>");
                }
            }
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
        /// <param name="language">语言</param>
        /// <param name="isshownum">是否显示数字页码</param>
        /// <param name="showte">是否显示首页和尾页</param>
        /// <returns></returns>
        public static string PageFoot_zc_2(int totle, int pagesize, string pageIndex, string pagename, string pageLinkText)
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
                if (IsNumeric(pageIndex) == "0")
                {
                    pageIndex = "1";
                }
                if (IsNumeric(pageIndex) != "0")
                {
                    CurPage = Convert.ToInt32(pageIndex);
                }
                else
                {
                    CurPage = 1;
                }

                sb.Append("<span><b>" + CurPage + "</b>/" + pagecont + "</span>");

                if (CurPage > 1)
                {
                    sb.Append("<a href=\"" + pagename + "?Page=" + Convert.ToString(CurPage - 1) + pageLinkText + "\" ><img src=\"/images/1/left.jpg\"  /></a>");
                }
                else
                {
                    sb.Append("<a href=\"javascript:;\"><img src=\"/images/1/left.jpg\"  /></a>");
                }

                if (CurPage < pagecont)
                {
                    sb.Append("<a href=\"" + pagename + "?Page=" + Convert.ToString(CurPage + 1) + pageLinkText + "\"  ><img src=\"/images/1/right.jpg\"  /></a>");

                }
                else
                {
                    sb.Append("<a href=\"javascript:;\" ><img src=\"/images/1/right.jpg\"  /></a>");
                }

            }
            return sb.ToString();
        }
        #endregion

        #region 显示页码AJAX
        /// <summary>
        /// 显示页码
        /// </summary>
        /// <param name="totle">总记录数</param>
        /// <param name="pagesize">每页显示多少条</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageLinkText">查询参数</param>
        /// <param name="language">语言</param>
        /// <param name="isshownum">是否显示数字页码</param>
        /// <param name="showte">是否显示首页和尾页</param>
        /// <returns></returns>
        public static string PageFoot_ajax(int totle, int pagesize, string pageIndex, string pageLinkText, string language, bool isshownum, bool showte)
        {
            StringBuilder sb = new StringBuilder();
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
                if (IsNumeric(pageIndex) == "0")
                {
                    pageIndex = "1";
                }
                if (IsNumeric(pageIndex) != "0")
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
                        sb.Append("<a href=\"javascript:void(null)\"  data-page=\"1\" data-cs=\"" + pageLinkText + "\"  >首页</a>");
                    }

                    sb.Append("<a href=\"javascript:void(null)\"   data-page=\"" + (CurPage - 1) + "\" data-cs=\"" + pageLinkText + "\"   >上一页</a>");
                }
                else
                {
                    if (showte == true)
                    {
                        sb.Append("<span>首页</span>");
                    }

                    sb.Append("<span>上一页</span>");
                }
                if (isshownum == true)
                {
                    if (pagecont <= 6)//如果页数小于等于6
                    {
                        for (int i = 1; i <= pagecont; i++)
                        {
                            if (i == CurPage)
                            {
                                activeStyle = "style=\"color:#ff0000\"";
                            }
                            else
                            {
                                activeStyle = "";
                            }
                            sb.Append("<a href=\"javascript:void(null)\" " + activeStyle + " data-page=\"" + i.ToString() + "\" data-cs=\"" + pageLinkText + "\"   >" + i.ToString() + "</a>");
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
                                activeStyle = "style=\"color:#ff0000\"";
                                sb.Append("<a href=\"javascript:void(null)\" " + activeStyle + "  data-page=\"" + i.ToString() + "\" data-cs=\"" + pageLinkText + "\"  >" + i.ToString() + "</a>");
                            }
                            else
                            {

                                sb.Append("<a href=\"javascript:void(null)\"  data-page=\"" + i.ToString() + "\" data-cs=\"" + pageLinkText + "\"  >" + i.ToString() + "</a>");
                            }
                        }

                        if (pagecont >= 18 && CurPage < (pagecont - 7))
                        {
                            sb.Append("<span>....</span>");
                            for (int i = pagecont - 2; i <= pagecont; i++)
                            {
                                if (i == CurPage)
                                {
                                    activeStyle = "style=\"color:#ff0000\"";
                                }
                                else
                                {
                                    activeStyle = "";
                                }
                                sb.Append("<a href=\"javascript:void(null)\" " + activeStyle + "  data-page=\"" + i.ToString() + "\" data-cs=\"" + pageLinkText + "\"  >" + i.ToString() + "</a>");
                            }
                        }
                    }
                }
                if (CurPage < pagecont)
                {
                    sb.Append("<a href=\"javascript:void(null)\"   data-page=\"" + (CurPage + 1) + "\" data-cs=\"" + pageLinkText + "\"      >下一页</a>");
                    if (showte == true)
                    {
                        sb.Append("<a href=\"javascript:void(null)\"   data-page=\"" + (pagecont) + "\" data-cs=\"" + pageLinkText + "\"      >尾页</a>");
                    }
                }
                else
                {
                    sb.Append("<span>下一页</span>");
                    if (showte == true)
                    {
                        sb.Append("<span>尾页</span>");
                    }
                }

            }
            return sb.ToString();
        }
        #endregion

        #region 显示页码mvc
        /// <summary>
        /// 显示页码
        /// </summary>
        /// <param name="totle">总记录数</param>
        /// <param name="pagesize">每页显示多少条</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pagename">页面名称</param>
        /// <param name="pageLinkText">查询参数</param>
        /// <param name="language">语言</param>
        /// <param name="isshownum">是否显示数字页码</param>
        /// <param name="showte">是否显示首页和尾页</param>
        /// <returns></returns>
        public static string PageFoot_MVC(int totle, int pagesize, string pageIndex, string pagename, string pageLinkText, string language, bool isshownum, bool showte)
        {
            StringBuilder sb = new StringBuilder();
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
                if (IsNumeric(pageIndex) == "0")
                {
                    pageIndex = "1";
                }
                if (IsNumeric(pageIndex) != "0")
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
                        sb.Append("<a href=\"" + pagename + pageLinkText + "/1\">首页</a>");
                    }

                    sb.Append("<a href=\"" + pagename + pageLinkText + "/" + Convert.ToString(CurPage - 1) + "\">上一页</a>");
                }
                else
                {
                    if (showte == true)
                    {
                        sb.Append("<span>首页</span>");
                    }

                    sb.Append("<span>上一页</span>");
                }
                if (isshownum == true)
                {
                    if (pagecont <= 6)//如果页数小于等于6
                    {
                        for (int i = 1; i <= pagecont; i++)
                        {
                            if (i == CurPage)
                            {
                                activeStyle = "style=\"color:#347de7\"";
                            }
                            else
                            {
                                activeStyle = "";
                            }
                            sb.Append("<a href=\"" + pagename + pageLinkText + "/" + i.ToString() + "\" " + activeStyle + " >" + i.ToString() + "</a>");
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

                                activeStyle = "style=\"color:#347de7\"";

                                sb.Append("<a href=\"" + pagename + pageLinkText + "/" + i.ToString() + "\" " + activeStyle + " >" + i.ToString() + "</a>");
                            }
                            else
                            {
                                sb.Append("<a href=\"" + pagename + pageLinkText + "/" + i.ToString() + "\">" + i.ToString() + "</a>");
                            }
                        }

                        if (pagecont >= 18 && CurPage < (pagecont - 7))
                        {
                            sb.Append("<span>....</span>");
                            for (int i = pagecont - 2; i <= pagecont; i++)
                            {
                                if (i == CurPage)
                                {
                                    activeStyle = "style=\"color:#347de7\"";
                                }
                                else
                                {
                                    activeStyle = "";
                                }
                                sb.Append("<a href=\"" + pagename + pageLinkText + "/" + i.ToString() + "\" " + activeStyle + " >" + i.ToString() + "</a>");
                            }
                        }
                    }
                }
                if (CurPage < pagecont)
                {
                    sb.Append("<a href=\"" + pagename + pageLinkText + "/" + Convert.ToString(CurPage + 1) + "\">下一页</a>");
                    if (showte == true)
                    {
                        sb.Append("<a href=\"" + pagename + pageLinkText + "/" + pagecont.ToString() + "\">尾页</a>");
                    }
                }
                else
                {
                    sb.Append("<span>下一页</span>");
                    if (showte == true)
                    {
                        sb.Append("<span>尾页</span>");
                    }
                }

            }
            return sb.ToString();
        }
        #endregion


        #region 显示页码
        /// <summary>
        /// 显示页码
        /// </summary>
        /// <param name="totle">总记录数</param>
        /// <param name="pagesize">每页显示多少条</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pagename">页面名称</param>
        /// <param name="pageLinkText">查询参数</param>
        /// <param name="language">语言</param>
        /// <param name="isshownum">是否显示数字页码</param>
        /// <param name="showte">是否显示首页和尾页</param>
        /// <returns></returns>
        public static string PageFoot_bootstrap(int totle, int pagesize, string pageIndex, string pagename, string pageLinkText, string language, bool isshownum, bool showte)
        {
            StringBuilder sb = new StringBuilder();
            if (totle > 0)
            {
                sb.Append("<ul class=\"pagination\">");
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
                if (IsNumeric(pageIndex) == "0")
                {
                    pageIndex = "1";
                }
                if (IsNumeric(pageIndex) != "0")
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
                        sb.Append("<li><a href=\"" + pagename + "?Page=1" + pageLinkText + "\">首页</a></li>");
                    }

                    sb.Append("<li><a href=\"" + pagename + "?Page=" + Convert.ToString(CurPage - 1) + pageLinkText + "\" class=\"glyphicon glyphicon-chevron-left\"></a></li>");
                }
                else
                {
                    if (showte == true)
                    {
                        sb.Append("<li class=\"disabled\"><a href=\"javascript:void(null)\">首页</a></li>");
                    }

                    sb.Append("<li  class=\"disabled\"><a href=\"javascript:void(null)\" class=\"glyphicon glyphicon-chevron-left\"></a></li>");
                }
                if (isshownum == true)
                {
                    if (pagecont <= 6)//如果页数小于等于6
                    {
                        for (int i = 1; i <= pagecont; i++)
                        {
                            if (i == CurPage)
                            {
                                activeStyle = "class=\"active\"";
                            }
                            else
                            {
                                activeStyle = "";
                            }
                            sb.Append("<li " + activeStyle + " ><a href=\"" + pagename + "?Page=" + i.ToString() + pageLinkText + "\"  >" + i.ToString() + "</a></li>");
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

                                activeStyle = "class=\"active\"";

                                sb.Append("<li " + activeStyle + " ><a href=\"" + pagename + "?Page=" + i.ToString() + pageLinkText + "\"  >" + i.ToString() + "</a></li>");
                            }
                            else
                            {
                                sb.Append("<li><a href=\"" + pagename + "?Page=" + i.ToString() + pageLinkText + "\">" + i.ToString() + "</a></li>");
                            }
                        }

                        if (pagecont >= 18 && CurPage < (pagecont - 7))
                        {

                            for (int i = pagecont - 2; i <= pagecont; i++)
                            {
                                if (i == CurPage)
                                {
                                    activeStyle = "class=\"active\"";
                                }
                                else
                                {
                                    activeStyle = "";
                                }
                                sb.Append("<li " + activeStyle + " ><a href=\"" + pagename + "?Page=" + i.ToString() + pageLinkText + "\" " + activeStyle + " >" + i.ToString() + "</a></li>");
                            }
                        }
                    }
                }
                if (CurPage < pagecont)
                {

                    sb.Append("<li><a href=\"" + pagename + "?Page=" + Convert.ToString(CurPage + 1) + pageLinkText + "\" class=\"glyphicon glyphicon-chevron-right\"></a></li>");
                    if (showte == true)
                    {
                        sb.Append("<li><a href=\"" + pagename + "?Page=" + pagecont.ToString() + pageLinkText + "\">尾页</a></li>");
                    }
                }
                else
                {
                    sb.Append("<li class=\"disabled\" ><a href=\"javascript:void(null)\" class=\"glyphicon glyphicon-chevron-right\"></a></li>");
                    if (showte == true)
                    {
                        sb.Append("<li class=\"disabled\" ><a href=\"javascript:void(null)\">尾页</a></li>");
                    }
                }
                sb.Append("</ul>");
            }
            return sb.ToString();
        }
        #endregion

        public static string divalert(string url, string content, bool isyy)
        {
            StringBuilder sb = new StringBuilder();
            if (isyy == true)
            {
                sb.Append("<script src=\"/Scripts/jquery-1.10.2.js\" type=\"text/javascript\"></script>");
                sb.Append("<script src=\"/layer/layer.js\" type=\"text/javascript\"></script>");

            }
            sb.Append("<script type=\"text/javascript\" language=\"javascript\" >window.history.forward(1); layer.alert(\'" + content + "\',{closeBtn:0},function(){window.location.href='" + url + "'});</script>");

            return sb.ToString();
        }
        public static string parent_divalert(string url, string content, bool isyy)
        {
            StringBuilder sb = new StringBuilder();
            if (isyy == true)
            {
                sb.Append("<script src=\"/Scripts/jquery-1.10.2.js\" type=\"text/javascript\"></script>");
                sb.Append("<script src=\"/layer/layer.js\" type=\"text/javascript\"></script>");
            }
            sb.Append("<script type=\"text/javascript\" language=\"javascript\" >var index = parent.layer.getFrameIndex(window.name); parent.closelayer('"+ content + "',index,'"+url+"');</script>");

            return sb.ToString();
        }

        public static string divalert_m(string url, string content, bool isyy)
        {
            StringBuilder sb = new StringBuilder();
            if (isyy == true)
            {
                sb.Append("<!DOCTYPE html><html><head lang=\"en\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
                sb.Append("<script src=\"/js/jquery-1.7.2.min.js\" type=\"text/javascript\"></script>");
                sb.Append("<script src=\"/layer/mobile/layer.js\" type=\"text/javascript\"></script></head>");

            }
            sb.Append("<body><script type=\"text/javascript\" language=\"javascript\" >javascript:window.history.forward(1); layer.open({ content: '" + content + "', btn: '我知道了', yes: function () { window.location.href ='" + url + "' }});</script></body></html>");

            return sb.ToString();
        }

        public static void delfiles(string upfiles)
        {
            try
            {
                if (string.IsNullOrEmpty(upfiles) && upfiles != "0")
                {

                }
                else
                {
                    string[] arrUpfile;
                    if (upfiles.IndexOf('|') != -1)
                    {
                        arrUpfile = upfiles.Split('|');
                        for (int i = 0; i < arrUpfile.Length; i++)
                        {
                            if (File.Exists(HttpContext.Current.Server.MapPath(arrUpfile[i])))
                            {
                                File.Delete(HttpContext.Current.Server.MapPath(arrUpfile[i]));
                            }
                        }
                    }
                    else
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath(upfiles)))
                        {
                            File.Delete(HttpContext.Current.Server.MapPath(upfiles));
                        }
                    }
                }
            }
            catch
            {

            }
        }
        public static MatchCollection get_marker(string t)
        {
            Regex re = new Regex(@"\[#--.*?--#\]");
            MatchCollection m = re.Matches(t);
            return m;
        }
        public static string get_marker_attribute(string t, string attr)
        {
            return Regex.Match(t, "(?<=" + attr + "=\")[a-z0-9_,]*(?=\")", RegexOptions.IgnoreCase).ToString();//类标签名称
        }

        public static string return_uploadfiles(string type)
        {
            switch (type)
            {
                default:
                    return "*.jpg;*.jpge;*.gif;*.png";
                case "jpg":
                    return "*.jpg;*.jpge;*.gif;*.png";
                case "txt":
                    return "*.doc;*.txt;*.docx;*.xls;*.xlsx";
                case "zip":
                    return "*.7z;*.rar;*.zip";
                case "vod":
                    return "*.flv";
            }
        }
        public static uint IPToInt(string ipAddress)
        {
            string disjunctiveStr = ".,: ";
            char[] delimiter = disjunctiveStr.ToCharArray();
            string[] startIP = null;
            for (int i = 1; i <= 5; i++)
            {
                startIP = ipAddress.Split(delimiter, i);
            }
            string a1 = startIP[0].ToString();
            string a2 = startIP[1].ToString();
            string a3 = startIP[2].ToString();
            string a4 = startIP[3].ToString();
            uint U1 = uint.Parse(a1);
            uint U2 = uint.Parse(a2);
            uint U3 = uint.Parse(a3);
            uint U4 = uint.Parse(a4);

            uint U = U1 << 24;
            U += U2 << 16;
            U += U3 << 8;
            U += U4;
            return U;
        }
        public static string IntToIP(uint ipAddress)
        {
            long ui1 = ipAddress & 0xFF000000;
            ui1 = ui1 >> 24;
            long ui2 = ipAddress & 0x00FF0000;
            ui2 = ui2 >> 16;
            long ui3 = ipAddress & 0x0000FF00;
            ui3 = ui3 >> 8;
            long ui4 = ipAddress & 0x000000FF;
            string IPstr = "";
            IPstr = System.Convert.ToString(ui1) + "."
            + System.Convert.ToString(ui2) + "."
            + System.Convert.ToString(ui3)
            + "." + System.Convert.ToString(ui4);
            return IPstr;
        }

        public static string return_page_name(string pagename, string cid)
        {
            string link = pagename == null ? "" : pagename;
            if (link.Contains("?") == false)
            {
                link += "?c=" + Encode(cid);
            }
            return link;
        }

        public static string request_form(object r)
        {
            if (r == null)
            {
                return "";
            }
            else
            {
                return r.ToString();
            }
        }

        /// <summary>
        /// 获取HTML中的图片
        /// </summary>
        /// <param name="Htmlstr">HTML代码</param>
        /// <returns></returns>
        public static string htmlimg(string Htmlstr)
        {
            string strTemp = "";
            MatchCollection m;
            m = Regex.Matches(Htmlstr, "(<img).*?>");
            for (int i = 0; i < m.Count; i++)
            {
                string ostr = m[i].ToString();
                //提取图片的地址
                MatchCollection m2;
                m2 = Regex.Matches(ostr, "(src=)['|\"].*?['\"]");
                for (int j = 0; j < m2.Count; j++)
                {
                    if (strTemp == "")
                    {
                        strTemp = m2[j].ToString();
                    }
                    else
                    {
                        strTemp = strTemp + "|" + m2[j].ToString();
                    }
                }
            }
            strTemp = strTemp.Replace("src=", "");
            strTemp = strTemp.Replace("\"", "");
            strTemp = strTemp.Replace("'", "");
            return strTemp;
        }

        /// <summary>
        /// 去掉HTML
        /// </summary>
        /// <param name="Htmlstring">传入参数</param>
        /// <returns>返回参数</returns>
        public static string DelHTML(string Htmlstring)//将HTML去除
        {
            #region
            //删除脚本
            if (string.IsNullOrEmpty(Htmlstring))
            {
                return "";
            }
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            //Htmlstring.Replace("", "");
            #endregion
            return Htmlstring;
        }
        public static string toHtml(string s)
        {
            if (string.IsNullOrEmpty(s) == false)
            {
                s = s.Replace("<", "&lt;");
                s = s.Replace(">", "&gt;");
                s = s.Replace("\r\n", "<br>");
                s = s.Replace(" ", "");
            }

            return s;
        }

        public static string showvod(string fileurl, string voidimg, string w, string h, string autostart)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=\"vod\"></div>");
            sb.Append(" <script type=\"text/javascript\" src=\"/Scripts/swfobject.js\"></script>");
            sb.Append(" <script type=\"text/javascript\">\r\n");
            sb.Append(" var so = new SWFObject('/flash/player.swf', 'ply', '" + w + "', '" + h + "', '9', '#000000');\r\n");
            sb.Append(" so.addParam('allowfullscreen', 'true');\r\n");
            sb.Append(" so.addParam('allowscriptaccess', 'always');\r\n");
            sb.Append(" so.addParam('wmode', 'opaque');\r\n");
            sb.Append(" so.addVariable('file', '" + fileurl + "');\r\n");
            sb.Append(" so.addVariable('autostart', '" + autostart + "');\r\n");
            sb.Append(" so.addVariable('icons', 'false');\r\n");
            sb.Append(" so.addVariable('repeat', 'false');\r\n");
            sb.Append(" so.addVariable('image','" + voidimg + "');\r\n");
            sb.Append(" so.write('vod');\r\n");
            sb.Append("</script>");
            return sb.ToString();
        }
        ///   <summary>   
        ///   得到2個日期的指定格式間隔   
        ///   </summary>   
        ///   <param   name="dt1">日期1</param>   
        ///   <param   name="dt2">日期2</param>   
        ///   <param   name="dateformat">間隔格式: y:年 M:月 d:天 h:小時 m:分鐘 s:秒 fff:毫秒 ffffff:微妙 fffffff:100毫微妙</param>   
        ///   <returns>間隔   long型</returns>   
        public static string GetIntervalOf2DateTime(DateTime dt1, DateTime dt2, string dateformat)
        {
            TimeSpan ts = dt1 - dt2;
            double t = ts.TotalDays;
            return (t / 365).ToString("0.00");
        }


        /// <summary>
        /// 判断字符窜是否有非法信息
        /// </summary>
        /// <param name="requestStr">字符串</param>
        /// <returns></returns>
        public static bool checkStr(string requestStr)
        {
            bool chkths = true;
            if (requestStr == "" || requestStr == null)
            {

            }
            else
            {
                string Sql_1 = "and|exec|insert|select|delete|update|count|chr|mid|master|truncate|char|declare|drop|drop+table|creat|creat+table";
                string[] sql_c = Sql_1.Split('|');
                foreach (string sl in sql_c)
                {
                    if (requestStr.ToLower().IndexOf(sl.Trim()) >= 0)
                    {
                        chkths = false;
                    }
                }
            }
            return chkths;
        }

        public static string re_search_by_xml(string type, string fexname, string val)
        {
            string path = HttpContext.Current.Server.MapPath("/systemxml/siteconfig.xml");
            StringBuilder sb = new StringBuilder();
            switch (type)
            {
                case "fc":
                    sb.Append("<input type=\"radio\" name=\"" + fexname + "\" value=\"\" checked  /><span>全部</span>");
                    XElement root = XElement.Load(path).Element("fc_search_text");
                    string rstr = root.Value;
                    string[] _r = rstr.Split('_');
                    foreach (string r in _r)
                    {
                        string[] _ar = r.Split('|');
                        string checkstr = "";
                        if (val == _ar[0])
                        {
                            checkstr = "checked";
                        }
                        sb.Append("<input type=\"radio\" name=\"" + fexname + "\" " + checkstr + " value=\"" + _ar[0] + "\" /><span>" + _ar[1] + "</span>");
                    }

                    break;
            }
            return sb.ToString();
        }

        public static string re_randnum(string v)
        {
            if (string.IsNullOrEmpty(v)) { return v; }
            if (v.IndexOf('.') != -1)
            {
                string[] arv = v.Split('.');
                if (arv[1] == "00")
                {
                    v = arv[0];
                }
            }
            return v;
        }

        public static int img_width(string url)
        {
            int w = 0;
            if (string.IsNullOrEmpty(url) == false)
            {
                if (url.Contains("baidu.com"))
                {
                    return w;
                }
                else if (url.Contains("http://"))
                {
                    try
                    {
                        WebRequest request = WebRequest.Create(url);
                        request.Credentials = CredentialCache.DefaultCredentials;
                        Stream s = request.GetResponse().GetResponseStream();
                        byte[] b = new byte[74373];
                        MemoryStream mes_keleyi_com = new MemoryStream(b);
                        s.Read(b, 0, 74373);
                        s.Close();
                        Image image = Image.FromStream(mes_keleyi_com);
                        w = image.Width;
                    }
                    catch
                    {

                    }
                }
                else
                {
                    string c = HttpContext.Current.Server.MapPath(url);
                    using (FileStream fs = new FileStream(c, FileMode.Open, FileAccess.Read))
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromStream(fs);
                        w = image.Width;
                    }
                }
            }
            return w;
        }

        public static void get_uploadimg_wh(string url, out int w, out int h)
        {
            string c = HttpContext.Current.Server.MapPath(url);
            using (FileStream fs = new FileStream(c, FileMode.Open, FileAccess.Read))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(fs);
                w = image.Width;
                h = image.Height;
            }
        }

        public static void delImg(string upfiles)
        {
            string[] arrUpfile;
            if (upfiles.IndexOf('|') != -1)
            {
                arrUpfile = upfiles.Split('|');
                for (int i = 0; i < arrUpfile.Length; i++)
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath(arrUpfile[i])))
                    {
                        File.Delete(HttpContext.Current.Server.MapPath(arrUpfile[i]));
                    }
                }
            }
            else
            {
                if (File.Exists(HttpContext.Current.Server.MapPath(upfiles)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(upfiles));
                }
            }
        }

        public static string get_username(string id)
        {
            if (id.Length >= 9)
            {
                return id;
            }
            string wxname = "";
            int name_leng = 9 - id.Length;
            wxname = "wx" + common.reRand_abc(name_leng) + id;
            return wxname;
        }

        public static string get_cs(System.Collections.Generic.IList<string> l, int n, bool tryint, string mrc)
        {
            if (l.Count > n)
            {
                if (tryint)
                {
                    int _n = 0;
                    int.TryParse(l[n], out _n);
                    return _n.ToString();
                }
                else
                {
                    return l[n];
                }
            }
            else
            {
                return mrc;
            }
        }

        public static bool IsMobile()
        {
            var context = HttpContext.Current;
            string u = context.Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino|UCWEB", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string Format_username(string uname)
        {
            if (string.IsNullOrEmpty(uname))
            {
                return "";
            }
            int l = uname.Length;
            if (l <= 1)
            {
                return uname;
            }
            else
            {
                int _l = (l / 2);
                int w = _l / 2;
                if (w <= 0)
                {
                    w = 1;
                }
                string x = "";
                for (int i = 0; i < _l; i++)
                {
                    x = x + "*";
                }
                uname = uname.Replace(uname.Substring(w, _l), x);
                return uname;
            }
        }
        /// <summary>
        /// 256位散列加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>密文</returns>
        public static string Sha256(string plainText)
        {
            SHA256Managed _sha256 = new SHA256Managed();
            byte[] _cipherText = _sha256.ComputeHash(Encoding.Default.GetBytes(plainText));
            return Convert.ToBase64String(_cipherText);
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
        /// <param name="language">语言</param>
        /// <param name="isshownum">是否显示数字页码</param>
        /// <param name="showte">是否显示首页和尾页</param>
        /// <returns></returns>
        public static string cms_PageFoot(int totle, int pagesize, string pageIndex, string pagename, string pageLinkText, string language, bool isshownum, bool showte)
        {
            StringBuilder sb = new StringBuilder();
            if (totle > 0)
            {
                string[] pn = pagename.Split('.');
                string p1 = pn[0] + "_";
                string p2 = "." + pn[1];
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
                if (IsNumeric(pageIndex) == "0")
                {
                    pageIndex = "1";
                }
                if (IsNumeric(pageIndex) != "0")
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
                        sb.Append("<a href=\"" + pagename + "\">首页</a>");
                    }

                    sb.Append("<a href=\"" + p1 + Convert.ToString(CurPage - 1) + p2 + "\">上一页</a>");
                }
                else
                {
                    if (showte == true)
                    {
                        sb.Append("<span>首页</span>");
                    }

                    sb.Append("<span>上一页</span>");
                }
                if (isshownum == true)
                {
                    if (pagecont <= 6)//如果页数小于等于6
                    {
                        for (int i = 1; i <= pagecont; i++)
                        {
                            if (i == CurPage)
                            {
                                activeStyle = "style=\"color:#347de7\"";
                            }
                            else
                            {
                                activeStyle = "";
                            }
                            if (i == 1)
                            {
                                sb.Append("<a href=\"" + pagename + "\" " + activeStyle + " >" + i.ToString() + "</a>");
                            }
                            else
                            {
                                sb.Append("<a href=\"" + p1 + i.ToString() + p2 + "\" " + activeStyle + " >" + i.ToString() + "</a>");
                            }

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

                                activeStyle = "style=\"color:#347de7\"";

                                sb.Append("<a href=\"" + p1 + i.ToString() + p2 + "\" " + activeStyle + " >" + i.ToString() + "</a>");
                            }
                            else
                            {
                                sb.Append("<a href=\"" + p1 + i.ToString() + p2 + "\">" + i.ToString() + "</a>");
                            }
                        }

                        if (pagecont >= 18 && CurPage < (pagecont - 7))
                        {
                            sb.Append("<span>....</span>");
                            for (int i = pagecont - 2; i <= pagecont; i++)
                            {
                                if (i == CurPage)
                                {
                                    activeStyle = "style=\"color:#347de7\"";
                                }
                                else
                                {
                                    activeStyle = "";
                                }
                                sb.Append("<a href=\"" + p1 + i.ToString() + p2 + "\" " + activeStyle + " >" + i.ToString() + "</a>");
                            }
                        }
                    }
                }
                if (CurPage < pagecont)
                {
                    sb.Append("<a href=\"" + p1 + Convert.ToString(CurPage + 1) + p2 + "\">下一页</a>");
                    if (showte == true)
                    {
                        sb.Append("<a href=\"" + p1 + pagecont.ToString() + p2 + "\">尾页</a>");
                    }
                }
                else
                {
                    sb.Append("<span>下一页</span>");
                    if (showte == true)
                    {
                        sb.Append("<span>尾页</span>");
                    }
                }

            }
            return sb.ToString();
        }
        #endregion


        /// <summary>  
        /// 笛卡尔积  
        /// </summary>  
        /// <param name="dimvalue">将每个维度的集合的元素视为List<string>,多个集合构成List<List<string>> dimvalue作为输入</param>  
        /// <param name="result">将多维笛卡尔乘积的结果放到List<string> result之中作为输出</param>  
        /// <param name="layer">int layer 只是两个中间过程的参数携带变量</param>  
        /// <param name="curstring"> string curstring只是两个中间过程的参数携带变量,传递""就行</param>  
        public static void DKR_run(List<List<string>> dimvalue, List<string> result, int layer = 0, string curstring = "")
        {
            if (layer < dimvalue.Count - 1)
            {
                if (dimvalue[layer].Count == 0)
                    DKR_run(dimvalue, result, layer + 1, curstring);
                else
                {
                    for (int i = 0; i < dimvalue[layer].Count; i++)
                    {
                        StringBuilder s1 = new StringBuilder();
                        s1.Append(curstring);

                        s1.Append(dimvalue[layer][i]);
                        s1.Append(",");
                        DKR_run(dimvalue, result, layer + 1, s1.ToString());
                    }
                }
            }
            else if (layer == dimvalue.Count - 1)
            {
                if (dimvalue[layer].Count == 0) result.Add(curstring);
                else
                {
                    for (int i = 0; i < dimvalue[layer].Count; i++)
                    {
                        result.Add(curstring + dimvalue[layer][i]);
                    }
                }
            }
        }

        public static string show_code(int max, string num)
        {
            int _max = max - num.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _max; i++)
            {
                sb.Append("0");
            }
            sb.Append(num);
            return sb.ToString();
        }

        public static string get_upload_url(string fileurl, string addurl, string reurl)
        {
            if (string.IsNullOrEmpty(fileurl) == false)
            {
                string _a = System.Configuration.ConfigurationManager.AppSettings[addurl];
                string _reurl = System.Configuration.ConfigurationManager.AppSettings[reurl];

                if (fileurl.IndexOf(_reurl) != -1)
                {
                    fileurl = fileurl.Replace(_reurl, _a);
                    return fileurl;
                }
                else
                {
                    return _a + fileurl;
                }
            }
            else
            {
                return fileurl;
            }
        }

        public static string replace_cont_img(string cont, string addurl, string reurl)
        {
            string re_str = "";
            string _a = System.Configuration.ConfigurationManager.AppSettings[addurl];
            string _reurl = System.Configuration.ConfigurationManager.AppSettings[reurl];
            if (string.IsNullOrEmpty(cont) == false)
            {
                re_str = cont.Replace(_reurl, _a).Replace("src=\"/uploads", "src=\"" + _a + "/uploads");

            }
            return re_str;
        }

        public static string reRandNum()
        {
            Random rd = new Random();
            int num = rd.Next(100000, 1000000);
            return num.ToString();
        }

        public static double tow(double price)
        {
            return Math.Round(price, 2, MidpointRounding.AwayFromZero);
        }
        public static double tow(double price, int n)
        {
            return Math.Round(price, n, MidpointRounding.AwayFromZero);
        }

        public static bool checkstr(string reg, string str)
        {
            string r = "";
            switch (reg)
            {
                case "u":
                    r = "";
                    break;
                case "n":
                    r = @"^[\u4e00-\u9fa5]{1,8}$";
                    break;
                case "s":
                    r = @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$|^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{4}$";
                    break;
                case "t":
                    r = @"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)";
                    break;
                case "m":
                    r = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                    break;
                case "mobile":
                    r = @"(1[3,4,5,6,7,8,9][0-9])\d{8}$";
                    break;
                default:
                    r = @"^[a-zA-Z0-9]{4,20}$";
                    break;
            }
            Regex g = new Regex(r);
            return g.IsMatch(str);
        }


        public static string GenerateStringID()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            string str = string.Format("{0:x}", i - DateTime.Now.Ticks);
            if (str.Length < 16)
            {
                str = str + reRand_abc(16 - str.Length);
            }
            return str;
        }

        public static string code_frmat(string str, char fh)
        {
            return Regex.Replace(str, @"(\w{4})", "$1-").Trim(fh);
        }

        public static bool is_wx()
        {
            string useragent = HttpContext.Current.Request.UserAgent;
            return useragent.ToLower().Contains("micromessenger");
        }

        /// <summary>
        /// 随机整数
        /// </summary>
        /// <param name="min">最小数</param>
        /// <param name="max">最大数</param>
        /// <returns>返回最小与最大之间的随机数有重复</returns>
        public static int GetRandomNumber(int min, int max)
        {
            int rtn = 0;
            Random r = new Random();
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int iSeed = BitConverter.ToInt32(buffer, 0);
            r = new Random(iSeed);
            rtn = r.Next(min, max + 1);
            return rtn;
        }


        public static string get_date_format(DateTime t)
        {
            if (t == null)
            {
                return "";
            }
            DateTime d = DateTime.Now;
            TimeSpan ts = d - t;
            double day = Math.Round(ts.TotalDays, MidpointRounding.AwayFromZero);
            if (day < 1)
            {
                return "今天" + t.ToShortTimeString();
            }
            else if (day == 1)
            {
                return "昨天" + t.ToShortTimeString();
            }
            else if (day == 2)
            {
                return "前天" + t.ToShortTimeString();
            }
            else
            {
                return t.ToShortDateString();
            }
        }

        public static string get_mr_photo(string photo, string mr)
        {
            return string.IsNullOrEmpty(photo) == true ? mr : photo;
        }

        public static bool SendEmail(string mailTo, string mailSubject, string mailContent)
        {
            //
            // 设置发送方的邮件信息,例如使用网易的smtp
            string smtpServer = "mail.ddhhtrading.com"; //SMTP服务器
            string mailFrom = "yyl@ddhhtrading.com"; //登陆用户名
            string userPassword = "Aa255601";//登陆密码

            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer; //指定SMTP服务器
            smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码

            // 发送邮件设置       
            MailMessage mailMessage = new MailMessage(mailFrom, mailTo); // 发送人和收件人
            mailMessage.Subject = mailSubject;//主题
            mailMessage.Body = mailContent;//内容
            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = MailPriority.Low;//优先级

            try
            {
                smtpClient.Send(mailMessage); // 发送邮件
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public static string Encrypt3DES(string a_strString, string a_strKey)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(a_strKey);
            DES.Mode = CipherMode.ECB;
            DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(a_strString);
            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        public static string Decrypt3DES(string a_strString, string a_strKey)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(a_strKey);
            DES.Mode = CipherMode.ECB;
            DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ICryptoTransform DESDecrypt = DES.CreateDecryptor();
            string result = "";
            try
            {
                byte[] Buffer = Convert.FromBase64String(a_strString);
                result = ASCIIEncoding.ASCII.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch
            {

            }
            return result;



        }

        public static string timestamp13()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
           return(Convert.ToInt64(ts.TotalMilliseconds).ToString());
        }


        public static void read_text(string pathFiles)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            bool isresult = true;
            if (File.Exists(pathFiles) == true)
            {
                StreamReader r = new StreamReader(pathFiles, true);
                r.BaseStream.Seek(0, SeekOrigin.Begin);
                while (r.Peek() > -1)
                {
                    if (check_uploadfiles(r.ReadLine()) == false)
                    {
                        isresult = false;
                        break;
                    }
                    i++;
                    if (i >= 10)
                    {
                        break;
                    }
                }
                r.Close();
                r.Dispose();
            }

            if (i == 0)
            {
                isresult = false;
            }
            if (isresult == false)
            {
                File.Delete(pathFiles);
            }
        }


        /// <summary>
        /// 判断字符窜是否有非法信息
        /// </summary>
        /// <param name="requestStr">字符串</param>
        /// <returns></returns>
        private static bool check_uploadfiles(string requestStr)
        {
            bool chkths = true;
            if (requestStr == "" || requestStr == null)
            {

            }
            else
            {
                string Sql_1 = "using|gif89au|webhandler|language|handler|system|public|class|void|httpcontext|string";
                string[] sql_c = Sql_1.Split('|');
                foreach (string sl in sql_c)
                {
                    if (requestStr.ToLower().IndexOf(sl.Trim()) >= 0)
                    {
                        chkths = false;
                        break;
                    }
                }
            }
            return chkths;
        }

        public static string dtitle(string title,string ftitle)
        {
            return string.IsNullOrEmpty(ftitle) ? title : ftitle;
        }

        public static string JoinUrl(string url,string paramStr)
        {
            if (url.IndexOf("?") != -1)
            {
                url = url + "&" + paramStr;
            }
            else
            {
                url = url + "?" + paramStr;
            }
            return url;
        }

        public static void ischangeMobi()
        {
            if (IsMobile())
            {
                HttpContext.Current.Response.Redirect("/h5/");
            }
        }

        public static void ischangeMobi(string pagename)
        {
            if (IsMobile())
            {
                HttpContext.Current.Response.Redirect("/h5/" + pagename);
            }
        }

        public static string getMoneyType(double val)
        {
            return String.Format("{0:N2}", val);
        }

        public static string get_date_format_for_nums(DateTime t)
        {
            if (t == null)
            {
                return "";
            }
            var year = t.Year.ToString();
            var month = (t.Month < 10) ? "0" + t.Month.ToString() : t.Month.ToString();
            var day = (t.Day < 10) ? "0" + t.Day.ToString() : t.Day.ToString();
            var hour = (t.Hour < 10) ? "0" + t.Hour.ToString() : t.Hour.ToString();
            var minute = (t.Minute < 10) ? "0" + t.Minute.ToString() : t.Minute.ToString();
            var second = (t.Second < 10) ? "0" + t.Second.ToString() : t.Second.ToString();

            return year + "/" + month + "/" + day + " " + hour + ":" + minute;
        }

        public static string MidStrEx_New(string sourse, string startstr, string endstr)
        {
            Regex rg = new Regex("(?<=(" + startstr + "))[.\\s\\S]*?(?=(" + endstr + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(sourse).Value;
        }

        public static string get_format_for_nums(int num)
        {
            return (num < 10) ? "0" + num.ToString() : num.ToString();
        }

}
}
