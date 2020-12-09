using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Dal.Serv
{
    public class DalLink : BaseRepository<Models.link>
    {
        public DalLink(ykmWebDbContext ykmWebDbContext) : base(ykmWebDbContext)
        {
        }
    }
}
