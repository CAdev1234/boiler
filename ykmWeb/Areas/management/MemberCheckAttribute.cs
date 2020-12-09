
using System.Web;
using System.Web.Mvc;


namespace ykmWeb.Areas.management.Controllers
{
    public class webAuthorzize : AuthorizeAttribute
    { 
        public string qxstr { get; set; }
        private string url = "~/management/webmaster/maindefault";
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool istg = false;
            Bll.master_login c = new Bll.master_login();
            if (c.checkonlien())
            {
                istg = true;
            }
            return istg;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(url);
        }
    }
}