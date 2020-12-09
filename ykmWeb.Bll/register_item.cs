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
    public class register_item
    {
        List<items> l = new List<items>();
        public register_item()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath("/config/register.json"));
            l= JsonConvert.DeserializeObject<List<items>>(json);
        }
        
        class items
        {
            public int id { get; set; }
            public string dataValue { get; set; }
            public string titleText { get; set; }
            public string type { get; set; }
        }

        /// <summary>
        /// 返回选择列表
        /// </summary>
        /// <param name="selectValue"></param>
        /// <param name="listType"></param>
        /// <returns></returns>
        public string getOption(string selectValue, string listType = "")
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count() > 0)
            {
                var ilist = l.Where(g => g.type == listType);
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
                        sb.Append("<option value=\"" + i.dataValue + "\" "+ selected + " >" + i.titleText + "</option>");
                    }
                }
            }
            return sb.ToString();
        }


    }
    
}
