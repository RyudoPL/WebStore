using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ProductDetails",
                url: "product-{id}.html",
                defaults: new { controller = "Category", action = "Details" }
                );

            routes.MapRoute(
            name: "ProductList",
                url: "Category/{CategoryName}",
                defaults: new { controller = "Category", action = "List" }
            );

            routes.MapRoute(
                name: "Static Websites",
                url: "{name}.html",
                defaults: new { controller = "Home", action = "StaticWebsites" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
