using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Labixa
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
         
            routes.MapRoute("Home", "Home", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("Blog", "Blog", new { controller = "Home", action = "Blog", id = UrlParameter.Optional });
            routes.MapRoute("faq", "faq", new { controller = "Home", action = "FAQ", id = UrlParameter.Optional });
            routes.MapRoute("Contact", "Contact", new { controller = "Contact", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("Shop", "Shop", new { controller = "Shop", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("Information", "Information", new { controller = "Shop", action = "Information", id = UrlParameter.Optional });
            routes.MapRoute("Deal", "Deal", new { controller = "Shop", action = "Deal", id = UrlParameter.Optional });
            routes.MapRoute("Service", "Service", new { controller = "Service", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("Index2", "Index2", new { controller = "Service", action = "Index2", id = UrlParameter.Optional });
            routes.MapRoute("Index3", "Index3", new { controller = "Service", action = "Index3", id = UrlParameter.Optional });
            routes.MapRoute("Index4", "Index4", new { controller = "Service", action = "Index4", id = UrlParameter.Optional });
            routes.MapRoute("Pages", "Pages", new { controller = "Pages", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("Signin", "Signin", new { controller = "TaiKhoan", action = "Login", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
