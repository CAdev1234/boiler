using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Models
{
    public class siteseo
    {
        public int? id { get; set; }
        //SEO
        public string sitename { get; set; }
        public string sitetitle { get; set; }
        public string sitekeyword { get; set; }
        public string sitedesc { get; set; }
        //WEB
        public string botinfo { get; set; }
        public string copyinfo { get; set; }
        public string icpinfo { get; set; }
        public string icpurl { get; set; }
        //tel
        public string tel { get; set; }
        public string lang { get; set; }
        public int? year { get; set; }
    }
}
