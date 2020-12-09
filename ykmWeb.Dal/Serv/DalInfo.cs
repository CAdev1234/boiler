using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Dal.Serv
{
    public class DalInfo : BaseRepository<Models.info>
    {
        public DalInfo(ykmWebDbContext ykmWebDbContext) : base(ykmWebDbContext)
        {

        }
    }
}
