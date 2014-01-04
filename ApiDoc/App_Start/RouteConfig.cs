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
            routes.MapRoute("CreateBranch", "CreateBranch/{*path}", new { controller = "Wiki", action = "CreateBranch" });
            routes.MapRoute("CreateMethod", "CreateMethod/{*path}", new { controller = "Wiki", action = "CreateMethod" });
            routes.MapRoute("Create",       "Create/{*path}",       new { controller = "Wiki", action = "Create" });

            routes.MapRoute("EditBranch",   "EditBranch/{*path}",   new { controller = "Wiki", action = "EditBranch" });
            routes.MapRoute("EditMethod",   "EditMethod/{*path}",   new { controller = "Wiki", action = "EditMethod" });
            routes.MapRoute("Edit",         "Edit/{*path}",         new { controller = "Wiki", action = "Edit" });
            
            routes.MapRoute("History",  "History/{*path}",  new { controller = "Wiki", action = "History" });
            routes.MapRoute("Delete",   "Delete/{*path}",   new { controller = "Wiki", action = "Delete" });
            routes.MapRoute("Display",  "{*path}",          new { controller = "Wiki", action = "Display" });
            #endregion
            
        }
    }
}