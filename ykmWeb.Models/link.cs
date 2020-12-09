using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Models
{
    public class link
    {
        public int? id { get; set; }
        public int? classid { get; set; }
        public string classtype { get; set; }
        public string linkname { get; set; }
        public string linkurl { get; set; }
        public string linkimg { get; set; }
        public string uploadfiles { get; set; }
        public int? infosoft { get; set; }
        public int? state { get; set; }
        public string sqr { get; set; }
        public string tel { get; set; }
    }
}
