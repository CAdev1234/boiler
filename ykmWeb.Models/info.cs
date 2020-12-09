using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Models
{
    public class info
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public int? classid { get; set; }
        public string defaultpic { get; set; }
        public string uploadfiles { get; set; }
        public string cont { get; set; }
        public int? sorts { get; set; }
        public int? istop { get; set; }
        public DateTime? insertdate { get; set; }
        public string sitetitle { get; set; }
        public string keywords { get; set; }
        public string description { get; set; }
        public int? issame { get; set; }
        public string h5cont { get; set; }
        public string intro { get; set; }
    }

    public class ht_list_info
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public int? classid { get; set; }
        public string defaultpic { get; set; }
        public DateTime? insertdate { get; set; }
    }

    public class view_info
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string defaultpic { get; set; }
        public DateTime? insertdate { get; set; }
        public int? classid { get; set; }
        public string intro { get; set; }
        public string cont { get; set; }
        public int? issame { get; set; }
        public string h5cont { get; set; }
    }

}
