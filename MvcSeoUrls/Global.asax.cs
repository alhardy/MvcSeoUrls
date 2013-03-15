using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using MvcSeoUrls.Core;
using MvcSeoUrls.ModeBinder;


namespace MvcSeoUrls
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ExtensibleActionInvoker>().As<IActionInvoker>();
            containerBuilder.RegisterControllers(Assembly.GetExecutingAssembly())
                .PropertiesAutowired()
                .InjectActionInvoker();
            containerBuilder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            containerBuilder.RegisterModelBinderProvider();            
            var container = containerBuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            ModelBinders.Binders.DefaultBinder = new ExtendedDefaultModelBinder();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
}