using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ykmWeb.Models
{
    public class pageMode:pageMain
    {

        /// <summary>
        /// 栏目名称
        /// </summary>
        public viewMenuClass classList { get; set; } 
        /// <summary>
        /// 分页html
        /// </summary>
        public string PageFootHtml { get; set; }
        /// <summary>
        /// 分页html_sj
        /// </summary>
        public string PageFootHtml_sj { get; set; }
        /// <summary>
        /// 小分页
        /// </summary>
        public string smallPageFootHtml { get; set; }
        /// <summary>
        /// 如果主内容为空,显示的内容
        /// </summary>
        public string emptyStr { get; set; }

        /// <summary>
        /// 页面路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 顶部导航
        /// </summary>
        public string mobi_child_str { get; set; }

        /// <summary>
        /// 顶部导航1
        /// </summary>
        public string mobi_child_str1 { get; set; }


    }
}