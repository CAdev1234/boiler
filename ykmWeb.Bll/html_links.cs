using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ykmWeb.Dal;
using ykmWeb.Dal.Serv;
using ykmWeb.Models;

namespace ykmWeb.Bll
{
    public class html_links
    {
        public html_links()
        {
        }
        
        public string links(List<link> l)
        {
            StringBuilder sb = new StringBuilder();
            if(l.Count>0)
            {
                foreach (var i in l)
                {
                    sb.Append("<li><a href=\"" + i.linkurl + "\" target=\"_blank\">" + i.linkname + "</a></li>");
                }
            }
            return sb.ToString();
        }

        public string h5links(List<link> l)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count > 0)
            {
                foreach (var i in l)
                {
                    sb.Append("<li><a href=\"" + i.linkurl + "\" target=\"_blank\">" + i.linkname + "</a></li>");
                }
            }
            return sb.ToString();
        }
    }
}
