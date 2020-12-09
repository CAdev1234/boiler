using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Dal.Serv
{
    public class DalWebmanager : BaseRepository<Models.webmanager>
    {
        public DalWebmanager(ykmWebDbContext ykmWebDbContext) : base(ykmWebDbContext)
        {

        }
    }
}
