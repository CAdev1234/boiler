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
    public  class menuRelation
    {
        List<relationData> l = new List<relationData>();
        public menuRelation()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath("/config/classlist.json"));
            l= JsonConvert.DeserializeObject<List<relationData>>(json);
        }
        
        class relationData
        {
            public string code { get; set; }
            public string dataValue { get; set; }
            public string title { get; set; }
            public string demoUrl { get; set; }
        }

        /// <summary>
        /// 返回选择列表
        /// </summary>
        /// <param name="selectValue"></param>
        /// <param name="listType"></param>
        /// <returns></returns>
        public string getOption(string selectValue, string listType = "news")
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count() > 0)
            {
                var ilist = l.Where(g => g.code == listType);
                if (ilist.Count() > 0)
                {
                    sb.Append("<option value=\"\">请选择</option>");
                    foreach (var i in ilist)
                    {
                        string selected = "";
                        if (i.dataValue == selectValue)
                        {
                            selected = "selected";
                        }
                        sb.Append("<option value=\"" + i.dataValue + "\" "+ selected + " >" + i.title + "</option>");
                    }
                }
            }
            return sb.ToString();
        }


    }
    
}
