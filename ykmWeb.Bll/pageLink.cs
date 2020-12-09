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
    public  class pageLink
    {
        List<linkData> l = new List<linkData>();
        public pageLink()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath("/config/pageLink.json"), Encoding.UTF8);
            l= JsonConvert.DeserializeObject<List<linkData>>(json);
        }
        
        class linkData
        {
            public string code { get; set; }//编码与样式匹配
            public string tableType { get; set; }//表类型
            public string dataValue { get; set; }//pc页面链接
            public string dataContValue { get; set; }//pc详情链接
            public string dataMobiValue { get; set; }//手机页面链接
            public string dataMobiContValue { get; set; }//手机详情链接
            public string dataValue_en { get; set; }//英文版pc页面链接
            public string dataContValue_en { get; set; }//英文版pc详情链接
            public string dataMobiValue_en { get; set; }//英文版手机页面链接
            public string dataMobiContValue_en { get; set; }//英文版手机详情链接
            public string title { get; set; }//名称
        }

        /// <summary>
        /// 返回选择列表
        /// </summary>
        /// <param name="selectValue"></param>
        /// <param name="tabletype"></param>
        /// <returns></returns>
        public string getOption(string selectValue,string tabletype)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count() > 0)
            {
                if (!string.IsNullOrEmpty(tabletype)){
                    l = l.Where(g => g.tableType == tabletype).ToList();
                }

                sb.Append("<option value=\"\" >请选择</option>");
                foreach (var i in l)
                {
                    string selected = "";
                    if (i.code == selectValue)
                    {
                        selected = "selected";
                    }
                    sb.Append("<option value=\"" + i.code + "\" " + selected + " >" + i.title + "</option>");
                }
            }
            return sb.ToString();
        }

        public string getUrl(string code)
        {
            return l.Where(g => g.code == code).Select(g => g.dataValue).SingleOrDefault();
        }

        public string getContUrl(string code)
        {
            return l.Where(g => g.code == code).Select(g => g.dataContValue).SingleOrDefault();
        }


        public string getH5Url(string code)
        {
            return l.Where(g => g.code == code).Select(g => g.dataMobiValue).SingleOrDefault();
        }

        public string getH5ContUrl(string code)
        {
            return l.Where(g => g.code == code).Select(g => g.dataMobiContValue).SingleOrDefault();
        }

        public string getUrl_en(string code)
        {
            return l.Where(g => g.code == code).Select(g => g.dataValue_en).SingleOrDefault();
        }

        public string getContUrl_en(string code)
        {
            return l.Where(g => g.code == code).Select(g => g.dataContValue_en).SingleOrDefault();
        }


        public string getH5Url_en(string code)
        {
            return l.Where(g => g.code == code).Select(g => g.dataMobiValue_en).SingleOrDefault();
        }

        public string getH5ContUrl_en(string code)
        {
            return l.Where(g => g.code == code).Select(g => g.dataMobiContValue_en).SingleOrDefault();
        }
    }
    
}
