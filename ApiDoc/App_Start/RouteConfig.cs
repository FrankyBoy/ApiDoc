using System.Web.Mvc;
using System.Web.Routing;

namespace ApiDoc.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Apis
            routes.MapRoute("Apis",         "",         new { controller = "ApiDescription", action = "List" });
            routes.MapRoute("ApiCreate",    "__create", new { controller = "ApiDescription", action = "Create" });
            routes.MapRoute("ApiEdit",      "__edit",   new { controller = "ApiDescription", action = "Edit" });
            routes.MapRoute("ApiDelete",    "__delete", new { controller = "ApiDescription", action = "Delete" });
            routes.MapRoute("ApiVersions",  "__history",new { controller = "ApiDescription", action = "History" });
            #endregion
            

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ApiDescription", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}