using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BAN_HANG
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);



            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}",
            //    defaults: new { controller = "Login", action = "Index" }
            //);



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "BAN_HANG.Areas.Client.Controllers" }
            ).DataTokens.Add("Area", "Client");

            routes.MapRoute(
                name: "Gioithieu",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Gioithieu", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "BAN_HANG.Areas.Client.Controllers" }
            ).DataTokens.Add("Area", "Client");

            routes.MapRoute(
                name: "DSDaiLy",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "DSDaiLy", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "BAN_HANG.Areas.Client.Controllers" }
            ).DataTokens.Add("Area", "Client");

            routes.MapRoute(
                name: "Product",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "BAN_HANG.Areas.Client.Controllers" }
            ).DataTokens.Add("Area", "Client");

            routes.MapRoute(
                name: "Lienhe",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Lienhe", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "BAN_HANG.Areas.Client.Controllers" }
            ).DataTokens.Add("Area", "Client");

            routes.MapRoute(
                name: "ChitietSP",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "ChitietSP", id = UrlParameter.Optional },
                namespaces: new string[] { "BAN_HANG.Areas.Client.Controllers" }
            ).DataTokens.Add("Area", "Client");

            routes.MapRoute(
                name: "Category",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "CategoryProduct", id = UrlParameter.Optional },
                namespaces: new string[] { "BAN_HANG.Areas.Client.Controllers" }
            ).DataTokens.Add("Area", "Client");

            routes.MapRoute(
                name: "Login",
                url: "{controller}/{action}",
                defaults: new { controller = "Login", action = "Index" }
            );
        }
    }
}
