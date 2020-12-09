using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Dal.Serv
{
    public class DalSiteSeo : BaseRepository<Models.siteseo>
    {
        public DalSiteSeo(ykmWebDbContext ykmWebDbContext) : base(ykmWebDbContext)
        {

        }
    }
}
