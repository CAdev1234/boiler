using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ykmWeb.Models;

namespace ykmWeb.Bll
{
    public class htmlggw_ht
    {
        private string str = "";
        public htmlggw_ht()
        {
            str = File.ReadAllText(HttpContext.Current.Server.MapPath("/config/ggwPosition.json"), Encoding.UTF8);
        }

        private List<bannerData> ggwpostr()
        {
            return JsonConvert.DeserializeObject<List<bannerData>>(str);
        }

        public string re_option(string val)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<bannerData> s = ggwpostr();  
            sb.Append("<option value=\"\">请选择</option>");
            foreach(var o in s)
            {
                string selected = "";
                if(val==o.position)
                {
                    selected = "selected";
                }
                sb.Append("<option value=\"" +o.position+ "\" "+ selected + " data-width=\""+o.width+ "\"   data-height=\"" + o.height + "\" >" + o.title+ "</option>");
            }
            return sb.ToString();
        }

        public string reggwstr(string va)
        {
            return ggwpostr().Where(g => g.position.Equals(va)).Select(g => g.title).SingleOrDefault();
        }
        

        class bannerData
        {
            public string position { get; set; }
            public string title { get; set; }
            public string width { get; set; }
            public string height { get; set; }
        }


        public string bot_ewm(List<ggw> l)
        {
            //<li><img src="/web_images/ewmhead.jpg" alt="" /><h3>二维码预留</h3></li>
            StringBuilder sb = new StringBuilder();
            if(l.Count>0)
            {
                foreach(var o in l)
                {
                    sb.Append("<li><img src=\"" + o.imgurl + "\" alt=\"" + o.title + "\" /><h3>" + o.title+"</h3></li>");
                }
            }
            return sb.ToString();
        }
    }
}
