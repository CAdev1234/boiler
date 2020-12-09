using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using ykmWeb.Dal;
using System.Linq.Expressions;

namespace ykmWeb.sysHtml
{
    public class InfoTableList
    {
        private ykmWebDbContext s;
        public InfoTableList(ykmWebDbContext _s)
        {
            s = _s;
        }
        public OrderModelField[] getInfoOrder()
        {
            OrderModelField[] or = new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "sorts", IsDESC = true }, new OrderModelField { propertyName = "insertdate", IsDESC = false }, new OrderModelField { propertyName = "id", IsDESC = true } };
            return or;
        }
        public OrderModelField[] getInfoOrderasc()
        {
            OrderModelField[] or = new OrderModelField[] { new OrderModelField { propertyName = "istop", IsDESC = true }, new OrderModelField { propertyName = "infosort", IsDESC = false }, new OrderModelField { propertyName = "infodate", IsDESC = false }, new OrderModelField { propertyName = "id", IsDESC = false } };
            return or;
        }
        public Expression<Func<Models.info, Models.view_info>> get_info_coloum(bool isCont=false)
        {
            Expression<Func<Models.info, Models.view_info>> nr;
            if (isCont)
            {
                nr = g => new Models.view_info { defaultpic = g.defaultpic, id = g.id, title = g.title, insertdate = g.insertdate, price = g.price, intro = g.intro, classid = g.classid, issame = g.issame, cont = g.cont, h5cont = g.h5cont };
            }
            else
            {
                nr = g => new Models.view_info { defaultpic = g.defaultpic, id = g.id, title = g.title, insertdate = g.insertdate, price = g.price, intro = g.intro, classid = g.classid };
            }
        
            return nr;
        }

        public List<Models.info> setFristCont(List<Models.info> list)
        {
            if(list.Count() > 0)
            {
                int id = list[0].id.Value;
                list[0].intro = s.db_info.Where(m => m.id == id).Select(g => g.intro).SingleOrDefault();
            }
            return list;
        }
        //public Expression<Func<Models.ggw, Models.app_view_ggw>> get_app_ggw_coloum()
        //{
        //    Expression<Func<Models.ggw, Models.app_view_ggw>> nr;
        //    nr = g => new Models.app_view_ggw { id = g.id, ggwlink = g.ggwlink, sorts = g.sorts, ggwposition = g.ggwposition, imgurl = g.imgurl, title = g.title };
        //    return nr;
        //}


        #region 上一页&下一页
        //public string listProvOrNext(int id, int classid)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    var o = s.db_viewInfoSort.Where(n => n.id == id).SingleOrDefault();
        //    var orid = o.orid;
        //    var prov = s.db_viewInfoSort.Where(n => n.orid < orid && n.classid == classid).OrderByDescending(g => g.orid).Take(1).SingleOrDefault();
        //    var next = s.db_viewInfoSort.Where(n => n.orid > orid && n.classid == classid).OrderBy(g => g.orid).Take(1).SingleOrDefault();
        //    sb.Append("<div class=\"listpage\">");
        //    sb.Append("<ul>");
        //    if (prov != null)
        //    {
        //        sb.Append("<li class=\"first\"><a href=\"/cont?id=" + prov.id + "\"><span>上一篇：</span>" + prov.title + "</a></li>");
        //    }
        //    else
        //    {
        //        sb.Append("<li class=\"first\"><a href=\"javascript:void(null)\"><span>上一篇：</span>没有了</a></li>");
        //    }
        //    sb.Append("<li><a href=\"javascript:history.back(-1)\">返回</a></li>");
        //    if (next != null)
        //    {
        //        sb.Append("<li class=\"last\"><a href=\"/cont?id=" + next.id + "\"><span>下一篇：</span>" + next.title + "</a></li>");
        //    }
        //    else
        //    {
        //        sb.Append("<li class=\"last\"><a href=\"javascript:void(null)\"><span>下一篇：</span>没有了</a></li>");
        //    }
        //    sb.Append("</ul></div>");
        //    return sb.ToString();
        //}
        #endregion

    }
}