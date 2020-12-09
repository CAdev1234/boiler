using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smoke.Dal;
using smoke.Dal.Serv;
using smoke.Models;

namespace sysHtml
{
    interface NavLIst
    {
        string getList(List<menuClass> l, int defClassid);
    }
}
