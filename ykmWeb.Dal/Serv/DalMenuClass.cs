using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Dal.Serv
{
    public class DalMenuClass : BaseRepository<Models.menuClass>
    {
        public DalMenuClass(ykmWebDbContext ykmWebDbContext) : base(ykmWebDbContext)
        {
            
        }
        public string getCatalogName(int catalogid)
        {
            return FindList(n => n.Catalogid == catalogid, 0, null).Select(g => g.Catalogname).FirstOrDefault();
        }
        public int getCatalogId(string caenname)
        {
            return FindList(n => n.Caenname == caenname, 0, null).Select(g => g.Catalogid.Value).FirstOrDefault();
        }
        public int findTopidByCid(int cid)
        {
            int pid = FindList(n => n.Catalogid == cid, 1, new OrderModelField[] { new OrderModelField { propertyName = "RootID", IsDESC = false }, new OrderModelField { propertyName = "Orders", IsDESC = false } }).Select(g=>g.ParentID).SingleOrDefault().Value;// get_object("parentid", "", "catalogid=" + cid).ParentID.ToString();
            if (pid == 0)
            {
                return cid;
            }
            else
            {
                return findTopidByCid(pid);
            }
        }

        public Models.menuClass getParentInfo(Models.menuClass c)
        {
            Models.menuClass ParentInfo = new Models.menuClass();
            if (c != null)
            {
                ParentInfo = find(n => n.Catalogid == c.ParentID);
            }
            return ParentInfo;
        }
    }
}
