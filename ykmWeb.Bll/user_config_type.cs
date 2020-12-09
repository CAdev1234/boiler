using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace ykmWeb.Bll
{
    public class user_config_type
    {
        private string chk_state = "[{id:\"0\",na:\"未查看\"},{id:\"100\",na:\"已查看\"}]";
        private string activity_state = "[{id:\"0\",na:\"未审核\"},{id:\"100\",na:\"已通过\"},{id:\"200\",na:\"未通过\"}]";
        private string user_type = "[{id:\"1\",na:\"志愿者账号\"},{id:\"2\",na:\"团体账号\"}]";
        //运费使用
        private string jsfs = "[{id:\"100\",na:\"按重量\"},{id:\"200\",na:\"按件数\"},{id:\"300\",na:\"按体积\"}]";
        private string yjlx = "[{id:\"100\",na:\"快递\"}]";
        private string isby = "[{id:\"100\",na:\"包邮\"},{id:\"0\",na:\"不包邮\"}]";
        //订单使用
        private string paytype = "[{id:\"1\",na:\"微信支付\"},{id:\"2\",na:\"支付宝支付\"}]";//,{id:\"2\",na:\"货到付款\"}
        private string psfstype = "[{id:\"1\",na:\"快递配送\"}]";//,{id:\"2\",na:\"送货上门\"}
        private string orderstate = "[{id:\"0\",na:\"等待支付\"},{id:\"10\",na:\"已提交\"},{id:\"100\",na:\"已支付\"},{id:\"200\",na:\"支付失败\"},{id:\"300\",na:\"已发货\"},{id:\"400\",na:\"已收货\"},{id:\"401\",na:\"订单完毕\"},{id:\"450\",na:\"已退款\"},{id:\"500\",na:\"订单已取消\"},{id:\"1000\",na:\"客户已经删除\"}]";
        private string orderpaystate = "[{id:\"0\",na:\"等待支付\"},{id:\"100\",na:\"已支付\"},{id:\"200\",na:\"支付失败\"},{id:\"450\",na:\"已退款\"},{id:\"500\",na:\"订单已取消\"},{id:\"1000\",na:\"客户已经删除\"}]";
        private string userorderlist = "[{id:\"0\",na:\"待付款\"},{id:\"100\",na:\"代发货\"},{id:\"300\",na:\"待收货\"},{id:\"401\",na:\"已完成\"}]";
        //评价使用
        private string pj_state = "[{id:\"1\",na:\"差评\"},{id:\"2\",na:\"中评\"},{id:\"3\",na:\"好评\"}]";

        private string table_state = "[{id:\"0\",na:\"空桌\"},{id:\"1\",na:\"已占用\"}]";

        private string master_type = "[{id:\"10\",na:\"管理员\"},{id:\"100\",na:\"超级管理员\"}]";
        //上下架状态
        private string state_sxj_txt = "[{id:\"0\",na:\"未上架\"},{id:\"100\",na:\"上架\"},{id:\"200\",na:\"下架\"}]";
        private string state_sxj_btn = "[{id:\"0\",na:\"上架\"},{id:\"100\",na:\"下架\"},{id:\"200\",na:\"上架\"}]";

        //火戈属性
        private string opus_type = "[{id:\"0\",na:\"普通作品\"},{id:\"1\",na:\"光合道推荐\"}]";
        private string bg_type = "[{id:\"0\",na:\"白色\"},{id:\"1\",na:\"黑色\"},{id:\"2\",na:\"灰色\"}]";
        private string user_level = "[{id:\"0\",na:\"普通用户\"},{id:\"1\",na:\"英雄榜用户\"}]";
        private string user_sex = "[{id:\"0\",na:\"女\"},{id:\"1\",na:\"男\"}]";
        private string obj_str = "";
        
        public user_config_type(string t) {
            switch (t)
            {
                case "chk_s":
                    obj_str = chk_state;
                    break;
                case "act_s":
                    obj_str = activity_state;
                    break;
                case "user_t":
                    obj_str = user_type;
                    break;
                //运费使用
                case "jsfs":
                    obj_str = jsfs;
                    break;
                case "yjlx":
                    obj_str = yjlx;
                    break;
                case "isby":
                    obj_str = isby;
                    break;
                //订单使用
                case "orstate":
                    obj_str = orderstate;
                    break;
                case "psfstype":
                    obj_str = psfstype;
                    break;
                case "paytype":
                    obj_str = paytype;
                    break;
                //评价使用
                case "pj_state":
                    obj_str = pj_state;
                    break;
                case "table_state":
                    obj_str = table_state;
                    break;
                //管理员
                case "master_type":
                    obj_str = master_type;
                    break;
                //上下架
                case "txt_sxj":
                    obj_str = state_sxj_txt;
                    break;
                case "btn_sxj":
                    obj_str = state_sxj_btn;
                    break;
                //火戈
                case "opus_type":
                    obj_str = opus_type;
                    break;
                case "bg_type":
                    obj_str = bg_type;
                    break;
                case "user_level":
                    obj_str = user_level;
                    break;
                case "user_sex":
                    obj_str = user_sex;
                    break;
            }
        }

        class main
        {
            public string id { get; set; }
            public string na { get; set; }
        }

        public string return_option(string v)
        {
            StringBuilder sb = new StringBuilder();
            List<main> l = JsonConvert.DeserializeObject<List<main>>(obj_str);
            if (l.Count > 0)
            {
                foreach (main m in l)
                {
                    string sv = "";
                    if (v == m.id.ToString())
                    {
                        sv = "selected";
                    }
                    sb.Append("<option value=\"" + m.id + "\" " + sv + " >" + m.na + "</option>");
                }
            }
            return sb.ToString();
        }

        public string get_text(string v)
        {
            if (string.IsNullOrEmpty(v))
            {
                return "";
            }
            List<main> l = JsonConvert.DeserializeObject<List<main>>(obj_str);
            main ma = l.Where(m => m.id == v).First();
            if (ma != null)
            {
                return ma.na;
            }
            else
            {
                return "";
            }
        }

        public string return_checkbox(string v,string checkboxname)
        {
            StringBuilder sb = new StringBuilder();
            List<main> l = JsonConvert.DeserializeObject<List<main>>(obj_str);
            if (l.Count > 0)
            {
                int i = 1;
                foreach (main m in l)
                {
                    string sv = "";
                    if (v!=null && v.Contains(m.id.ToString()))
                    {
                        sv = "checked";
                    }
                    sb.Append("<input type=\"checkbox\" name=\""+ checkboxname + "\" id=\""+ checkboxname + "_"+i+"\"  value=\"" + m.id + "\" " + sv + " />" + m.na + "");
                    i++;
                }
            }
            return sb.ToString();
        }

        public string get_text_by_arr(string v)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(v))
            {
                return "";
            }
            List<main> l = JsonConvert.DeserializeObject<List<main>>(obj_str);
            l = l.Where(n => v.Contains(n.id)).ToList();
            foreach (main m in l)
            {
                sb.Append(m.na + ",");
            }
            return sb.ToString();
        }

        public Dictionary<string, string> getData(bool isarr = false)
        {
            Dictionary<string, string> dr = new Dictionary<string, string>();
            if (isarr)
            {
                dr.Add("0", "全部");
            }
            List<main> l = JsonConvert.DeserializeObject<List<main>>(obj_str);
            if (l.Count > 0)
            {
                foreach(var o in l)
                {
                    dr.Add(o.id, o.na);
                }
            }
            return dr;
        }

        public string list_ul_li(string url)
        {
            StringBuilder sb = new StringBuilder();
            List<main> l = JsonConvert.DeserializeObject<List<main>>(obj_str);
            if (l.Count > 0)
            {
                sb.Append("<ul class=\"dropdown-menu\" role=\"menu\">");
                foreach (main m in l)
                {
                    sb.Append("<li><a href=\"" + url + m.id + "\">" + m.na + "</a></li>");
                }
                sb.Append("</ul>");
            }
            return sb.ToString();
        }

    }
}
