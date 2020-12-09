using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Bll
{
    public class siteSeo
    {
        public Models.menuClass mclass { get; set; }
        public Models.info ic { get; set; }
        private string title;
        private string keyword;
        private string desc;
        private string sitename;
        private string lang;
        private Dal.ykmWebDbContext s;
        public siteSeo(Dal.ykmWebDbContext _s, string language = "")
        {
            s = _s;
            Dal.Serv.DalSiteSeo dss = new Dal.Serv.DalSiteSeo(s);
            var obj = dss.FindList(n => n.lang == language, 1, null).OrderByDescending(g => g.id).SingleOrDefault();
            if (obj != null)
            {
                sitename = obj.sitename;
                title = string.IsNullOrEmpty(obj.sitetitle) ? obj.sitename : obj.sitetitle;
                keyword = obj.sitekeyword;
                desc = obj.sitedesc;
                lang = obj.lang;
            }
        }

        public string reSeo(string seoType)
        {
            switch (seoType)
            {
                case "index":
                    break;
                case "c":
                    classpage();
                    break;
                case "i":
                    infopage();
                    break;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<title>" + title + "</title>");
            sb.Append("<meta name=\"keywords\" content=\"" + keyword + "\">");
            sb.Append("<meta name=\"description\" content=\"" + desc + "\">");
            return sb.ToString();
        }

        private void classpage()
        {

            if (mclass != null)
            {

                if (!string.IsNullOrEmpty(mclass.sitename))
                {
                    title = mclass.sitename;
                }
                else
                {
                    title = mclass.Catalogname + "_" + sitename;
                }

                if (!string.IsNullOrEmpty(mclass.keyword))
                {
                    keyword = mclass.keyword;
                }

                if (!string.IsNullOrEmpty(mclass.keycont))
                {
                    desc = mclass.keycont;
                }
            }
        }

        private void infopage()
        {
            if (ic != null)
            {
                if (!string.IsNullOrEmpty(ic.sitetitle))
                {
                    title = ic.sitetitle;
                }
                else
                {
                    title = ic.title + "_" + sitename;
                }

                if (!string.IsNullOrEmpty(ic.keywords))
                {
                    keyword = ic.keywords;
                }

                if (!string.IsNullOrEmpty(ic.description))
                {
                    desc = ic.description;
                }
            }
        }

    }
}
