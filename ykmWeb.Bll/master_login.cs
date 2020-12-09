using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ykmWeb.Dal;
using ykmWeb.Models;
namespace ykmWeb.Bll
{
    public class master_login
    {

        private string cookiesname = "yczeijifezkji", cookieskey = "yccccode", domain = "";

        #region 判断用户是否登陆有返回值
        public bool checkonlien()
        {
            //HttpContext.Current.Session["Muid"] = "8";
            //HttpContext.Current.Session["master"] = "janksongks";
            //HttpContext.Current.Session["qx"] = "1";
            //HttpContext.Current.Session["qxtype"] = "100";
            //HttpContext.Current.Session["qxlv"] = "admin";
            //return true;

            if (HttpContext.Current.Session["Muid"] == null || HttpContext.Current.Session["Muid"].ToString() == "")
            {
                string cookiesValue = getCookie(cookiesname);
                if (string.IsNullOrEmpty(cookiesValue))
                {
                    return false;
                }
                else
                {
                    string[] userCookies = cookiesValue.Split('|');
                    string token = userCookies[0];
                    using(ykmWebDbContext s=new ykmWebDbContext())
                    {
                        Dal.Serv.DalWebmanager dw = new Dal.Serv.DalWebmanager(s);
                        var o = dw.FindList(n => n.usertoken == token, 1, null).SingleOrDefault();
                        if (o == null)
                        {
                            return false;
                        }
                        else
                        {
                            insertSession(o);
                            return true;
                        }
                    }
                }
            }
            else
            {
                return true;
            }
        }
        #endregion
        #region 保存登陆状态
        /// <summary>
        /// 插入Session
        /// </summary>
        /// <param name="u"></param>
        public void insertSession(ykmWeb.Models.webmanager w)
        {
            HttpContext.Current.Session["Muid"] = w.id.ToString();
            HttpContext.Current.Session["master"] = w.adminname;
            HttpContext.Current.Session["qx"] = w.bigqx;
            HttpContext.Current.Session["qxtype"] = w.qxtype; 
            HttpContext.Current.Session["qxlv"] = w.smqx;
            //店铺id-即创建者id
            //HttpContext.Current.Session["spuid"] = w.shop_id.ToString();
            HttpContext.Current.Session["spuid"] = w.create_userid.ToString();

            savecookies(w.usertoken);
        }
        #endregion


        public  void  savecookies(string userToken)
        {                 
            HttpCookie cok;
            cok = HttpContext.Current.Request.Cookies[cookiesname];
            if (cok == null)
            {
                cok=  new HttpCookie(cookiesname);             
            }
            DateTime saveTime = DateTime.Now; ;
            string signToken = ykmWeb.common.common.Encode(userToken +"|"+ saveTime, cookieskey);
            cok["webtoken"] = signToken;
         //   cok.Expires = saveTime.AddDays(1);
            cok.Domain = domain;
            HttpContext.Current.Response.AppendCookie(cok);

        }
        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="cookieValue">cookies名称</param>
        /// <returns></returns>
        public string getCookie(string cookieValue)
        {
            if (!string.IsNullOrEmpty(cookieValue))
            {
                HttpCookie cok = HttpContext.Current.Request.Cookies[cookieValue];
                if (cok != null)
                {
                    string cokstr = ykmWeb.common.common.Decode(cok["webtoken"], cookieskey);
                    if (cokstr != null && cokstr!="0")
                    {
                        string[] cookieStrings = cokstr.Split('|');
                        DateTime exData = new DateTime();
                        DateTime.TryParse(cookieStrings[1],out  exData);
                        if (exData != null)
                        {
                            TimeSpan ts = DateTime.Now - exData;
                            if(ts.TotalDays > 1)
                            {
                                return "";
                            }
                            else
                            {
                                return cokstr;
                            }
                        }                  
                    }
                }           
            }
            return "";
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void liveout()
        {
            string wuid = HttpContext.Current.Session["Muid"].ToString();
            using(ykmWebDbContext s=new ykmWebDbContext())
            {
                Dal.Serv.DalWebmanager dv = new Dal.Serv.DalWebmanager(s);
                string re = common.common.GenerateStringID();
                int _wid = int.Parse(wuid);
                dv.edit(n => n.id == _wid, g => new webmanager { usertoken = re });
            }
            HttpContext.Current.Session.Abandon();
             HttpCookie cok;
            cok = HttpContext.Current.Request.Cookies[cookiesname];
            if (cok != null)
            {            
                cok.Expires = DateTime.Now.AddMilliseconds(-1);             
                HttpContext.Current.Response.AppendCookie(cok);
            }
        }
    }
}
