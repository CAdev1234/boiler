using System;
using ykmWeb.Models;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using ykmWeb.common;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;

namespace ykmWeb.Bll
{
    public class do_class_view
    {
  
        public do_class_view()
        {
     
        }
        private string showpathimg(int i, int e)
        {
            string restr = "";
            if ((i + 1) == e)
            {
                restr = " background:url(images/2.gif);background-repeat:no-repeat; background-position:left center; padding-left:22px;";
            }
            else
            {
                restr = " background:url(images/zj.gif);background-repeat:no-repeat; background-position:left center; padding-left:22px;";
            }
            return restr;
        }

        /// <summary>
        /// 显示一级分了
        /// </summary>
        /// <param name="seturl">设置链接url</param>
        /// <returns></returns>
        public string do_view(string type,string p,string lang)
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dalMenu = new DalMenuClass(s);
                int _p = int.Parse(p);
                Expression<Func<menuClass, bool>> wherelba = PredicateExtensionses.True<menuClass>();
                wherelba = wherelba.And(n => n.ParentID == _p);
                if (_p == 0)
                {
                    wherelba = wherelba.And(n => n.language == lang);
                }
                List<menuClass> l = dalMenu.FindList(wherelba, 0,null).OrderBy(g=>g.RootID).ThenBy(n=>n.Orders).ToList();
                if (l.Count > 0)
                {
                    if (type == "1")
                    {
                        sb.Append(" <input name=\"tablename\" type=\"hidden\" id=\"tablename\" value=\"menuClass\" />");
                    }
                    //else if (type == "n")
                    //{
                    //    sb.Append(" <input name=\"tablename\" type=\"hidden\" id=\"tablename\" value=\"menuClass\" />");
                    //}
                    int i = 1;
                    foreach (menuClass n in l)
                    {
                        int cid = n.Catalogid.Value;
                        int pid = n.ParentID.Value;
                        string classisc = "";
                        if (n.Child > 0)
                        {
                            classisc = "isclick";
                        }
                        sb.Append("<div class=\" heig40 pidbox nclass \" data-pid=\"" + pid + "\" data-cid=\"" + cid + "\">");
                        sb.Append("    <div class=\"col-sm-4  heig40 bor2-b2 fst14 titleclass " + classisc + "\">");
                        if (n.Child == 0)
                        {

                        }
                        else
                        {
                            //sb.Append("        <span class=\"glyphicon glyphicon-plus\" id=\"ico" + cid + "\">");
                            //sb.Append("        </span>");
                            sb.Append("<i class=\"fa fa-chevron-down\" aria-hidden=\"true\"></i>");
                        }


                        sb.Append("        <span id=\"title_" + cid + "\">");
                        sb.Append(n.Catalogname);
                        sb.Append("        </span>");
                        sb.Append("    </div>");
                        sb.Append("    <div class=\"col-sm-8 tr heig40 bor2-b2 fst14 ctrlclass\">");
                        sb.Append("        <button class=\"btn btn-default btn-sm addc green\">");
                        sb.Append("            添加子类");
                        sb.Append("        </button>");
                        sb.Append("        <button class=\"btn btn-default btn-sm setc green\">");
                        sb.Append("            修改");
                        sb.Append("        </button>");
                        if (i > 1)
                        {
                            sb.Append("        <button class=\"btn btn-default btn-sm movet green\">");
                            sb.Append("            上移");
                            sb.Append("        </button>");

                        }
                        if (i < l.Count)
                        {
                            sb.Append("        <button class=\"btn btn-default btn-sm moved green\">");
                            sb.Append("            下移");
                            sb.Append("        </button>");
                        }
                        sb.Append("        <button class=\"btn btn-default btn-sm linkc green\">");
                        sb.Append("            设置链接");
                        sb.Append("        </button>");

                        sb.Append("        <button class=\"btn btn-default btn-sm linkh green\">");
                        sb.Append("            设置链接(手机)");
                        sb.Append("        </button>");

                        sb.Append("        <button class=\"btn btn-default btn-sm delc green\">");
                        sb.Append("            删除");
                        sb.Append("        </button>");
                        sb.Append("    </div>");
                        sb.Append("    <div id=\"p" + cid + "\" class=\"padleft childclass\">");
                        sb.Append("    </div>");
                        sb.Append("</div>");
                        i++;
                    }

                }
                else
                {
                    if (p == "0")
                    {
                        sb.Append(" <input name=\"tablename\" type=\"hidden\" id=\"tablename\" value=\"menuClass\" />");
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 显示一级分了
        /// </summary>
        /// <param name="seturl">设置链接url</param>
        /// <returns></returns>
        public string user_do_view(string type, string p, string lang)
        {
            StringBuilder sb = new StringBuilder();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dalMenu = new DalMenuClass(s);
                int _p = int.Parse(p);
                Expression<Func<menuClass, bool>> wherelba = PredicateExtensionses.True<menuClass>();
                wherelba = wherelba.And(n => n.ParentID == _p);
                if (_p == 0)
                {
                    wherelba = wherelba.And(n => n.language == lang);
                }
                List<menuClass> l = dalMenu.FindList(wherelba, 0, null).OrderBy(g => g.RootID).ThenBy(n => n.Orders).ToList();
                if (l.Count > 0)
                {
                    if (type == "1")
                    {
                        sb.Append(" <input name=\"tablename\" type=\"hidden\" id=\"tablename\" value=\"menuClass\" />");
                    }
                    //else if (type == "n")
                    //{
                    //    sb.Append(" <input name=\"tablename\" type=\"hidden\" id=\"tablename\" value=\"menuClass\" />");
                    //}
                    int i = 1;
                    foreach (menuClass n in l)
                    {
                        int cid = n.Catalogid.Value;
                        int pid = n.ParentID.Value;
                        string classisc = "";
                        if (n.Child > 0)
                        {
                            classisc = "isclick";
                        }
                        sb.Append("<div class=\" heig40 pidbox nclass \" data-pid=\"" + pid + "\" data-cid=\"" + cid + "\">");
                        sb.Append("    <div class=\"col-sm-4  heig40 bor2-b2 fst14 titleclass " + classisc + "\">");
                        if (n.Child == 0)
                        {

                        }
                        else
                        {
                            //sb.Append("        <span class=\"glyphicon glyphicon-plus\" id=\"ico" + cid + "\">");
                            //sb.Append("        </span>");
                            sb.Append("<i class=\"fa fa-chevron-down\" aria-hidden=\"true\"></i>");
                        }


                        sb.Append("        <span id=\"title_" + cid + "\">");
                        sb.Append(n.Catalogname);
                        sb.Append("        </span>");
                        sb.Append("    </div>");
                        sb.Append("    <div class=\"col-sm-8 tr heig40 bor2-b2 fst14 ctrlclass\">");
                        sb.Append("        <button class=\"btn btn-default btn-sm addc green\">");
                        sb.Append("            添加子类");
                        sb.Append("        </button>");
                        sb.Append("        <button class=\"btn btn-default btn-sm setc green\">");
                        sb.Append("            修改");
                        sb.Append("        </button>");
                        if (i > 1)
                        {
                            sb.Append("        <button class=\"btn btn-default btn-sm movet green\">");
                            sb.Append("            上移");
                            sb.Append("        </button>");

                        }
                        if (i < l.Count)
                        {
                            sb.Append("        <button class=\"btn btn-default btn-sm moved green\">");
                            sb.Append("            下移");
                            sb.Append("        </button>");
                        }
                        sb.Append("        <button class=\"btn btn-default btn-sm linkc green\">");
                        sb.Append("            设置链接");
                        sb.Append("        </button>");

                        sb.Append("        <button class=\"btn btn-default btn-sm linkh green\">");
                        sb.Append("            设置链接(手机)");
                        sb.Append("        </button>");

                        //sb.Append("        <button class=\"btn btn-default btn-sm delc\">");
                        //sb.Append("            删除");
                        //sb.Append("        </button>");
                        sb.Append("    </div>");
                        sb.Append("    <div id=\"p" + cid + "\" class=\"padleft childclass\">");
                        sb.Append("    </div>");
                        sb.Append("</div>");
                        i++;
                    }

                }
                else
                {
                    if (p == "0")
                    {
                        sb.Append(" <input name=\"tablename\" type=\"hidden\" id=\"tablename\" value=\"menuClass\" />");
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 显示当前标记名称下所有子类的id集合
        /// </summary>
        /// <param name="enClassName">标记名称</param>
        /// <returns></returns>
        public List<int> showallclassid(string enClassName)
        {
            string parentstr = "", childstr = "";
            int seldepth = 0;
            List<int> reCatalogif = new List<int>();
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                var obj = dmc.FindList(n => n.Caenname == enClassName, 1, null).Select(g => new { g.Catalogid, g.ParentStr, g.Depth }).FirstOrDefault();
                if (obj != null)
                {
                    parentstr = obj.ParentStr + "," + obj.Catalogid;
                    childstr = obj.ParentStr + "," + obj.Catalogid + ",";
                    seldepth = obj.Depth.Value + 1;
                    reCatalogif.Add(obj.Catalogid.Value);
                    var ilist = dmc.FindList(n => (n.Depth == seldepth && n.ParentStr == parentstr) || (n.Depth > seldepth && n.ParentStr.Contains(childstr)), 0, new OrderModelField[] { new OrderModelField { propertyName = "Catalogid", IsDESC = false } }).Select(g => g.Catalogid).ToList();
                    if (ilist.Count > 0)
                    {
                        foreach (var o in ilist)
                        {
                            reCatalogif.Add(o.Value);
                        }
                    }
                }
            }
            return reCatalogif;
        }

        //判断产品分类不是从Parentid=0开始
        public int getTopCidForSameType(int depth = 1, string type = "")
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalMenuClass dmc = new DalMenuClass(s);
                if (!string.IsNullOrEmpty(type))
                {
                    var l = dmc.FindList(n => n.Depth == depth && n.tabletype == type, 0, null).ToList();
                    if (l.Count > 0)
                    {
                        return l[0].ParentID.Value;
                    }
                    else
                    {
                        return getTopCidForSameType(depth + 1, type);
                    }
                }
                return 0;
            }
        }

        public string list_choose_div(string type, string p,string language="",string infochoose="")
        {
            StringBuilder sb = new StringBuilder();
           using(ykmWebDbContext s=new ykmWebDbContext())
            {
                DalMenuClass dl = new DalMenuClass(s);
                int _pid = int.Parse(p);
                ////判断产品分类不是从Parentid=0开始,parentid等于0时执行此段代码，因为每次点击栏目都将重新执行此代码
                //if (_pid == 0)
                //{
                //    var o = dl.find(n => n.ParentID == _pid);
                //    _pid = getTopCidForSameType(o.Depth.Value, infochoose);
                //}
                ////判断产品分类不是从Parentid=0开始
                Expression<Func<menuClass, bool>> wherelba = PredicateExtensionses.True<menuClass>();
                wherelba = wherelba.And(n => n.ParentID == _pid);
                if (!string.IsNullOrEmpty(infochoose))
                {
                    wherelba = wherelba.And(n => n.tabletype == infochoose);
                }
                if (!string.IsNullOrEmpty(language))
                {
                    wherelba = wherelba.And(n => n.language == language);
                }
                var l = dl.FindList(wherelba, 0, new OrderModelField[] { new OrderModelField { propertyName = "ParentID", IsDESC = false }, new OrderModelField { propertyName = "Catalogid", IsDESC = false } }).Select(g=>new { g.Catalogid,g.Child,g.Catalogname,g.ParentID}).ToList();
                if (l.Count > 0)
                {
                    if (type == "1")
                    {
                        sb.Append(" <input name=\"tablename\" type=\"hidden\" id=\"tablename\" value=\"menuClass\" />");
                    }
                    int i = 1;
                    foreach (var n in l)
                    {
                        int cid = n.Catalogid.Value;
                        int pid = n.ParentID.Value;
                        string classisc = "";
                        if (n.Child > 0)
                        {
                            classisc = "isclick";
                        }
                        sb.Append("<div class=\" heig40 pidbox nclass \" data-pid=\"" + pid + "\" data-cid=\"" + cid + "\" >");
                        sb.Append("    <div class=\"left  heig40 bor2-b2 fst14 titleclass " + classisc + "\" style=\"width:70%\">");
                        if (n.Child == 0)
                        {

                        }
                        else
                        {
                            sb.Append("        <span class=\"glyphicon glyphicon-plus\" id=\"ico" + cid + "\">");
                            sb.Append("        </span>");
                        }


                        sb.Append("        <span id=\"title_" + cid + "\">");
                        sb.Append(n.Catalogname);
                        sb.Append("        </span>");
                        sb.Append("    </div>");
                        sb.Append("    <div class=\"right tr heig40 bor2-b2 fst14 ctrlclass\" style=\"width:30%\">");
                        sb.Append("        <button class=\"btn btn-default btn-sm choose right\" data-catalogname=\""+n.Catalogname+"\">");
                        sb.Append("            选择");
                        sb.Append("        </button>");
                        sb.Append("    </div>");
                        sb.Append("    <div id=\"p" + cid + "\" class=\"padleft childclass\">");
                        sb.Append("    </div>");
                        sb.Append("</div>");
                        i++;
                    }
                }
                else
                {
                    //  sb.Append("<div class=\"classmenu\" style=\"background:none\"><div class=\"classxg\"  onmouseover=\"this.className='classxgx'\" onmouseout=\"this.className='classxg'\">暂无子类</div></div>");
                    if (p == "0")
                    {
                        sb.Append(" <input name=\"tablename\" type=\"hidden\" id=\"tablename\" value=\"menuClass\" />");
                    }

                }
            }
           
            return sb.ToString();
        }



    }
}
