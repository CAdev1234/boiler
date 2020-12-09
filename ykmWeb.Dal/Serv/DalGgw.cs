using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Dal.Serv
{
    public class DalGgw : BaseRepository<Models.ggw>
    {
        public DalGgw(ykmWebDbContext ykmWebDbContext) : base(ykmWebDbContext)
        {

        }
    }
}
