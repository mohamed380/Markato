using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Markato
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Markato", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "login",
                url: "{controller}/{action}/{email}",
                defaults: new { controller = "Markato", action = "Signin"}
            );
            routes.MapRoute(
            name: "su",
            url: "{controller}/{action}/{email}",
            defaults: new { controller = "MainDirectors", action = "EmployeeProfile" }
        );




        }
    }
}
