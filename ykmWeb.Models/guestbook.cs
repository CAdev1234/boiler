using System;

namespace ykmWeb.Models
{
    public class guestbook
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string tel { get; set; }
        public string sex { get; set; }
        public string email { get; set; }
        public string cont { get; set; }
        public DateTime? insertdate { get; set; }
        public int? state { get; set; }
        public string repeatCont { get; set; }
        public DateTime? repeatdate { get; set; }
    }


}
