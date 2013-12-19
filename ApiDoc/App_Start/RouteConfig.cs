using System.Web.Mvc;
using System.Web.Routing;

namespace ApiDoc.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "Base",
                url: "",
                defaults: new { controller = "ApiDescription", action = "List" }
            );

            routes.MapRoute(
               name: "Modules",
               url: "{apiName}",
               defaults: new { controller = "Modules", action = "List" }
            );

            routes.MapRoute(
               name: "Methods",
               url: "{apiName}/{moduleName}",
               defaults: new { controller = "Methods", action = "List" }
            );

            routes.MapRoute(
               name: "SingleMethod",
               url: "{apiName}/{moduleName}/{methodName}",
               defaults: new { controller = "Methods", action = "View" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ApiDescription", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}