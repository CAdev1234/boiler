using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Dal.Serv
{
    public class DalVisitor : BaseRepository<Models.visitor>
    {
        public DalVisitor(ykmWebDbContext ykmWebDbContext) : base(ykmWebDbContext)
        {

        }
    }
}
