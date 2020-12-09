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
    public class do_visitor
    {
        public do_visitor()
        {
        }

        /// <summary>
        /// 增加访问者
        /// </summary>
        public void addVisitor()
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalVisitor dv = new DalVisitor(s);
                dv.add(new visitor { insertdate = DateTime.Now, ip = HttpContext.Current.Request.UserHostAddress });
            }
        }

        public int? getVisitorTotal()
        {
            using (ykmWebDbContext s = new ykmWebDbContext())
            {
                DalVisitor dv = new DalVisitor(s);
                return dv.count(n => true);
            }
        }
    }
}
