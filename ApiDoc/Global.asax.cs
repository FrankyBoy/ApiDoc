using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ApiDoc.App_Start;
using ApiDoc.DataAccess.Proxies;
using ApiDoc.Provider;
using Autofac;
using Autofac.Integration.Mvc;

namespace ApiDoc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RegisterComponents();
        }

        private static void RegisterComponents()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.Register(p => new ApiDocDbProxy())
                   .As<IApiDocDbProxy>()
                   .SingleInstance();

            builder.Register(c => new NodeProvider(c.Resolve<IApiDocDbProxy>()))
                   .As<INodeProvider>()
                   .SingleInstance();
            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}