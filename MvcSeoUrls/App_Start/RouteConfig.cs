using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Mvc;

namespace MvcSeoUrls
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Clear();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapAttributeRoutes(config =>
            {
                config.ScanAssembly(Assembly.GetExecutingAssembly());
                config.AddRoutesFromAssembly(Assembly.GetExecutingAssembly());
                config.UseLowercaseRoutes = true;
                config.AppendTrailingSlash = false;
                config.AutoGenerateRouteNames = true;
            });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}