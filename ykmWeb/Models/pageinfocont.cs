using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ykmWeb.Models
{
    public class pageinfocont:pageMode
    {
        public info infoContent { get; set; }
        public string infoFoot { get; set; }
        public List<info> tjinfolist { get; set; }
    }
}