using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ykmWeb.Models
{
    /// <summary>
    /// webmaster 的摘要说明
    /// </summary>
    public class webmanager
    {
        public int? id { get; set; }
        public string adminname { get; set; }
        public string adminpass { get; set; }
        public string bigqx { get; set; } //大权限
        public string smqx { get; set; } //小权限
        public int? qxtype { get; set; } //管理员类型 100超级管理员，10普通管理员
        public string usertoken { get; set; }//用户凭据
        //新建
        public DateTime? infodate { get; set; } //创建时间
        public int? create_userid { get; set; } //创建管理员id
        public string create_username { get; set; } //创建管理员账号
        public int? create_usertype { get; set; } //创建管理员权限
        public int? shop_id { get; set; } //所属店铺id-即为店铺总管理员id
    }

    public class LoginViewModel
    {

        public string UserName { get; set; }


        public string Password { get; set; }


        public bool RememberMe { get; set; }
    }
}

