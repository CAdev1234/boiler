using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ykmWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("index", "", new { controller = "Home", action = "Index" }); //引导页

            //routes.MapRoute("index", "index", new { controller = "Home", action = "Index"}, new { t = @"\d*" }); //首页

            routes.MapRoute("list", "list", new { controller = "Home", action = "list", cid = UrlParameter.Optional }, new { cid = @"\d*" }); //制度
            routes.MapRoute("search", "search", new { controller = "Home", action = "search", cid = UrlParameter.Optional }, new { t = "",k="" }); //制度
            routes.MapRoute("cont", "cont", new { controller = "Home", action = "cont", id = UrlParameter.Optional }, new { id = @"\d*" }); //制度
            routes.MapRoute("classpage", "classpage", new { controller = "Home", action = "classpage", cid = 9 }, new { cid = @"\d*" }); //制度


            routes.MapRoute("h5index", "h5", new { controller = "mobilePage", action = "Index" }); //首页
            routes.MapRoute("h5list", "h5/list", new { controller = "mobilePage", action = "list", cid = UrlParameter.Optional }, new { cid = @"\d*" }); //制度
            routes.MapRoute("h5cont", "h5/cont", new { controller = "mobilePage", action = "cont", id = UrlParameter.Optional }, new { id = @"\d*" }); //制度
            routes.MapRoute("h5classpage", "h5/classpage", new { controller = "mobilePage", action = "classpage", cid = 9 }, new { cid = @"\d*" }); //制度
            routes.MapRoute("h5search", "h5/search", new { controller = "mobilePage", action = "search", cid = UrlParameter.Optional }, new { t = "", k = "" }); //制度

            //英文版
            routes.MapRoute("index1", "en/index", new { controller = "Home_en", action = "Index" }, new { t = @"\d*" }); //首页

            routes.MapRoute("list1", "en/list", new { controller = "Home_en", action = "list", cid = UrlParameter.Optional }, new { cid = @"\d*" }); //制度
            routes.MapRoute("search1", "en/search", new { controller = "Home_en", action = "search", cid = UrlParameter.Optional }, new { t = "", k = "" }); //制度
            routes.MapRoute("cont1", "en/cont", new { controller = "Home_en", action = "cont", id = UrlParameter.Optional }, new { id = @"\d*" }); //制度
            routes.MapRoute("classpage1", "en/classpage", new { controller = "Home_en", action = "classpage", cid = 9 }, new { cid = @"\d*" }); //制度

            routes.MapRoute("h5index1", "en/h5", new { controller = "mobilePage_en", action = "Index" }); //首页
            routes.MapRoute("h5list1", "en/h5/list", new { controller = "mobilePage_en", action = "list", cid = UrlParameter.Optional }, new { cid = @"\d*" }); //制度
            routes.MapRoute("h5cont1", "en/h5/cont", new { controller = "mobilePage_en", action = "cont", cid = UrlParameter.Optional }, new { cid = @"\d*" }); //制度





            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
