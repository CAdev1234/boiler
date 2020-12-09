using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Bll
{
   public class siteLanguage
    {
        private string ftstr = "[{lang:\"中文\",code:\"cn\"},{lang:\"英文\",code:\"en\"}]";
        private List<languageCode> ilist = new List<languageCode>();
        class languageCode {
            public string lang { get; set; }
            public string code { get; set; }
        }
        public siteLanguage()
        {
            ilist = JsonConvert.DeserializeObject<List<languageCode>>(ftstr);
        }

        public string getListText()
        {
            StringBuilder sb = new StringBuilder();
            if(ilist.Count() > 0)
            {
                int i = 0;
                foreach(var o in ilist)
                {
                    string act = "";
                    if (i == 0)
                    {
                        act = "active";
                    }
                    sb.Append(" <li class=\""+act+"\"><a href=\"javascript:void(null)\" data-toggle=\"tab\" data-val=\""+o.code+"\"> "+o.lang+"</a></li>");
                    i++;
                }
            }
            return sb.ToString();
        }

        public string getListText(string code)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(code)==false)
            {
                ilist = ilist.Where(n => n.code == code).ToList();
            }
            if (ilist.Count() > 0)
            {
                int i = 0;
                foreach (var o in ilist)
                {
                    string act = "";
                    if (i == 0)
                    {
                        act = "active";
                    }
                    sb.Append(" <li class=\"" + act + "\"><a href=\"javascript:void(null)\" data-toggle=\"tab\" data-val=\"" + o.code + "\"> " + o.lang + "</a></li>");
                    i++;
                }
            }
            return sb.ToString();
        }

        public string return_option(string v)
        {
            StringBuilder sb = new StringBuilder(); 
            if (ilist.Count > 0)
            {
                foreach (var m in ilist)
                {
                    string sv = "";
                    if (v == m.code.ToString())
                    {
                        sv = "selected";
                    }
                    sb.Append("<option value=\"" + m.code + "\" " + sv + " >" + m.lang + "</option>");
                }
            }
            return sb.ToString();
        }

        public string getFristOneInput()
        {
            return "<input type=\"hidden\" name=\"lang\" id=\"lang\" value=\""+ilist[0].code+"\" />";
        }
        public string getFristOneInput(string lang="")
        {
            if(string.IsNullOrEmpty(lang) ==false)
            { 
                return "<input type=\"hidden\" name=\"lang\" id=\"lang\" value=\"" + lang + "\" />";
            }
            else
            {
                return "<input type=\"hidden\" name=\"lang\" id=\"lang\" value=\"" + ilist[0].code + "\" />";
            }
        }
    }
}
