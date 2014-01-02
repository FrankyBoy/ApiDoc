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
            /*routes.MapRoute("Apis",         "",             new { controller = "ApiDescription", action = "List" });
            routes.MapRoute("ApiCreate",    "__create",     new { controller = "ApiDescription", action = "Create" });
            routes.MapRoute("ApiEdit",      "__edit",       new { controller = "ApiDescription", action = "Edit" });
            routes.MapRoute("ApiDelete",    "__delete",     new { controller = "ApiDescription", action = "Delete" });
            routes.MapRoute("ApiRevisions", "__revisions", new { controller = "ApiDescription", action = "Revisions" });
            */
            routes.MapRoute("CreateNode",   "CreateNode/{parentId}",    new { controller = "Wiki", action = "CreateNode" });
            routes.MapRoute("CreateMethod", "CreateMethod/{parentId}",  new { controller = "Wiki", action = "CreateMethod" });
            routes.MapRoute("Create",       "Create/{*path}",           new { controller = "Wiki", action = "Create" });

            routes.MapRoute("EditNode",     "EditNode/{id}",    new { controller = "Wiki", action = "EditNode" });
            routes.MapRoute("EditMethod",   "EditMethod/{id}",  new { controller = "Wiki", action = "EditMethod" });
            routes.MapRoute("Edit",         "Edit/{*path}",     new { controller = "Wiki", action = "Edit" });
            
            routes.MapRoute("History",  "History/{*path}",  new { controller = "Wiki", action = "History" });
            routes.MapRoute("Delete",   "Delete/{*path}",   new { controller = "Wiki", action = "Delete" });
            routes.MapRoute("Display",  "{*path}",          new { controller = "Wiki", action = "Display" });
            #endregion
            
        }
    }
}