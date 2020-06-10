using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace shradhabookstores
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Index of User
            routes.MapRoute(
                name: "user",
                url: "user",
                defaults: new { controller = "Users", action = "Index" }
            );


            // products/index
            routes.MapRoute(
              name: "category",
              url: "category",
              defaults: new { controller = "Categories", action = "Index" },
              namespaces: new string[] { "shradhabookstores.Controller.Backend" }
            );



            // --------------------------------------------------------------
            // Default frontend
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Product", id = UrlParameter.Optional }
            );
        }
    }
}
