using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ykmWeb.Bll
{
    public class formOption
    {
        List<form_option> l = new List<form_option>();
        public formOption()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath("/config/options.json"));
            l= JsonConvert.DeserializeObject<List<form_option>>(json);
        }
        
        public class form_option
        {
            public string type { get; set; }
            public string key { get; set; }
            public string value { get; set; }
        }

        /// <summary>
        /// 返回选择列表
        /// </summary>
        /// <param name="selectValue"></param>
        /// <param name="listType"></param>
        /// <returns></returns>
        public string getOption(string type,string dqval)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count() > 0)
            {
                string css = "";
                var ilist = l.Where(g => g.type == type);
                if (ilist.Count() > 0)
                {
                    sb.Append("<option value=\"\" date-placeholder=\"\">请选择</option>");
                    foreach (var i in ilist)
                    {
                        css = (i.value == dqval) ? "selected" : "";
                        sb.Append("<option value=\"" + i.value + "\" "+ css + ">" + i.key + "</option>");
                    }
                }
            }
            return sb.ToString();
        }

        public string getCheckbox(string type, string dqval)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count() > 0)
            {
                string css = "";
                var ilist = l.Where(g => g.type == type);
                if (ilist.Count() > 0)
                {
                    sb.Append("<option value=\"\" date-placeholder=\"\">请选择</option>");
                    foreach (var i in ilist)
                    {
                        css = (i.value == dqval) ? "selected" : "";
                        sb.Append("<option value=\"" + i.value + "\" " + css + ">" + i.key + "</option>");
                    }
                }
            }
            return sb.ToString();
        }

        //ask_1,ask_2,ask_3,edu,
        public IEnumerable<form_option> getOptionList(string type)
        {
            var ilist = l.Where(g =>g.type == type);
            return ilist;
        }

    }
    
}
