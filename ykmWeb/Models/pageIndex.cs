using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ykmWeb.Models
{
    public class pageIndex : pageMain
    {
        /// <summary>
        /// 列表字符串
        /// </summary>
        public string listHtml { get; set; }
        /// <summary>
        /// 页脚字符串
        /// </summary>
        public string footHtml { get; set; }
    }
}