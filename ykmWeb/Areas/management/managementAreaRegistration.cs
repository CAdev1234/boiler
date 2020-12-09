using System.Web.Mvc;

namespace ykmWeb.Areas.management
{
    public class managementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "management";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "management_default",
                "management/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}