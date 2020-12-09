using ykmWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ykmWeb.Areas.management.Controllers;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;
using ykmWeb.Bll;
using ykmWeb.common;
using ykmWeb.sysHtml;

namespace ykmWeb.Controllers
{
    public class ajaxController : Controller
    {
        // GET: ajax
        [HttpPost]
        public string savebook(FormCollection form)
        {
            guestbook g = new guestbook();
            var name = form["name"];
            var sex = form["sex"];
            var tel = form["tel"];
            var email = form["email"];
            var cont = form["cont"];
            var checkcode = form["checkcode"];
            if (string.IsNullOrEmpty(name))
            {
                return "0|姓名不能为空！";
            }
            if (string.IsNullOrEmpty(tel))
            {
                return "0|联系电话不能为空！";
            }
            if (string.IsNullOrEmpty(email))
            {
                return "0|电子邮箱不能为空！";
            }
            if (string.IsNullOrEmpty(cont))
            {
                return "0|留言内容不能为空！";
            }
            if (string.IsNullOrEmpty(checkcode))
            {
                return "0|验证码不能为空！";
            }
            else if (checkcode.ToLower() != Session["yljcheode"].ToString().ToLower())
            {
                return "0|验证码错误！";
            }

            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalGuestBook dg = new DalGuestBook(s);
                g.name = name;
                g.sex = sex;
                g.tel = tel;
                g.cont = cont;
                g.email = email;
                g.insertdate = DateTime.Now;
                g.state = 0;
                var d = dg.add(g);
                //common.common.SendEmail("yyl@ddhhtrading.com", "您的网站上有一条新的留言", "<b>留言内容</b><br>姓名：" + name + "<br>性别：" + sex + "<br>联系电话：" + tel + "<br>电子邮箱：" + email + "<br>留言内容：" + cont + "<br>");
                if (d != null)
                {
                    return "1|感谢您的留言，我们会尽快与您取得联系";
                }
                else
                {
                    return "0|留言失败";
                }
            }
        }

        [HttpPost]
        public string savebook_en(FormCollection form)
        {
            guestbook g = new guestbook();
            var name = form["name"];
            var sex = form["sex"];
            var tel = form["tel"];
            var email = form["email"];
            var cont = form["cont"];
            var checkcode = form["checkcode"];
            if (string.IsNullOrEmpty(name))
            {
                return "0|Name cannot be empty！";
            }
            if (string.IsNullOrEmpty(sex))
            {
                return "0|Gender cannot be empty！";
            }
            if (string.IsNullOrEmpty(tel))
            {
                return "0|Contact number cannot be empty！";
            }
            if (string.IsNullOrEmpty(email))
            {
                return "0|Email address cannot be empty！";
            }
            if (string.IsNullOrEmpty(cont))
            {
                return "0|Message content cannot be empty！";
            }
            //if (string.IsNullOrEmpty(checkcode))
            //{
            //    return "0|验证码不能为空！";
            //}
            //else if(checkcode.ToLower() != Session["yljcheode"].ToString().ToLower())
            //{
            //    return "0|验证码错误！";
            //}

            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalGuestBook dg = new DalGuestBook(s);
                g.name = name;
                g.sex = sex;
                g.tel = tel;
                g.cont = cont;
                g.email = email;
                g.insertdate = DateTime.Now;
                g.state = 0;
                var d = dg.add(g);
                common.common.SendEmail("yyl@ddhhtrading.com", "There is a new message on your website", "<b>Message content</b><br>Full name：" + name + "<br>Gender：" + sex + "<br>contact number：" + tel + "<br>mail box：" + email + "<br>Message content：" + cont + "<br>");
                if (d != null)
                {
                    return "1|Thank you for your message, we will contact you as soon as possible";
                }
                else
                {
                    return "0|Message failed";
                }
            }
        }

        //[HttpPost]
        //public string sendcheckcode(FormCollection form)
        //{
        //    var type = form["type"];
        //    var mobile = form["mobile"];
        //    var email = form["email"];
        //    var temp = form["temp"]; //使用模板
        //    bool result = false;
        //    string pattern = "";
        //    switch (type)
        //    {
        //        case "cbm":
        //            if (mobile == null || mobile == "")
        //            {
        //                return "0|手机号码不能为空";
        //            }
        //            pattern = @"0?(13|14|15|16|17|18|19)[0-9]{9}";
        //            result = Regex.IsMatch(mobile, pattern);
        //            if (!result)
        //            {
        //                return "0|请输入正确的手机号码";
        //            }
        //            break;
        //        case "cbe":
        //            pattern = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        //            result = Regex.IsMatch(email, pattern);
        //            if (email == null || email == "")
        //            {
        //                return "0|电子邮箱地址不能为空";
        //            }
        //            else if (!result)
        //            {
        //                return "0|请输入正确的电子邮箱地址";
        //            }
        //            break;
        //    }

        //    bool isSend = false;
        //    //首先实例化验证码的类
        //    ValidateCode validateCode = new ValidateCode();
        //    //生成验证码指定的长度
        //    string code = validateCode.GetRandomString(4);
        //    Session["yljcheode"] = code;
        //    if (type == "cbm")
        //    {
        //        string backstr = "";
        //        if (temp == "register")
        //        {
        //            backstr = ali_sms_send.send(mobile, "SMS_176420017", "{\"code\":\"" + code + "\"}");
        //        }
        //        else if(temp == "forget")
        //        {
        //            backstr = ali_sms_send.send(mobile, "SMS_176420016", "{\"code\":\"" + code + "\"}");
        //        }
        //        if (backstr == "OK")
        //        {
        //            isSend = true;
        //        }
        //    }
        //    else if(type == "cbm")
        //    {
        //        if (temp == "register")
        //        {
        //            isSend = common.common.SendEmail(email, "【双薪农科】短信验证码", "验证码" + code + "，您正在注册成为新用户，感谢您的支持！");
        //        }
        //        else if (temp == "forget")
        //        {
        //            isSend = common.common.SendEmail(email, "【双薪农科】短信验证码", "验证码" + code + "，您正在尝试修改登录密码，请妥善保管账户信息。");
        //        }
        //    }
        //    if (isSend)
        //    {
        //        return "1|发送成功";
        //    }
        //    else
        //    {
        //        return "0|发送失败";
        //    }
        //}



        [HttpPost]
        public string fanye(int cid = 0, int pageid = 1)
        {
            string res = "";
            if (pageid <= 0)
            {
                pageid = 1;
            }
            pageid = pageid + 1;
            int rows = 1;
            //子页搜索功能
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                NavLIst nl = new NavLIst(s);
                DalMenuClass dmc = new DalMenuClass(s);
                DalInfo di = new DalInfo(s);
                InfoTableList itl = new InfoTableList(s);
                var classObj = dmc.FindList(n => n.Catalogid == cid, 1, nl.getOrderList()).FirstOrDefault();
                int topnum = 10;
                var obje = di.FindListPage(pageid, topnum, out rows, n => n.classid == classObj.Catalogid, itl.getInfoOrder()).Select(itl.get_info_coloum(true)).ToList();
                if (obje.Count > 0)
                {
                    switch (classObj.pclisttype)
                    {
                        case "pro-list"://产品列表
                            res = mobiHtml.add_pro_list(obje);
                            break;
                        case "news-list"://新闻列表
                            res = mobiHtml.add_news_list(obje);
                            break;
                    }
                }
                return res;
            }
        }

    }
}