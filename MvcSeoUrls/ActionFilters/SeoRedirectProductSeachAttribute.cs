using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSeoUrls.Core;
using MvcSeoUrls.Extensions;
using RouteData = System.Web.Routing.RouteData;

namespace MvcSeoUrls.ActionFilters
{
   public class SeoRedirectProductSeachAttribute : ActionFilterAttribute
    {
       public override void OnActionExecuting(ActionExecutingContext filterContext)
       {
           var doRedirect = false;

           var productSearchRouteData = filterContext.ActionParameters.Values.FirstOrDefault(x => x.GetType() == typeof(ProductSearchCriteriaRouteData)) as ProductSearchCriteriaRouteData;

           if (productSearchRouteData == null) return;

           if (filterContext.RouteData.Values["Category"] == null || 
               string.Compare(filterContext.RouteData.Values["Category"].ToString(), productSearchRouteData.Category.EscapeName(), StringComparison.InvariantCultureIgnoreCase) != 0)
           {
               filterContext.RouteData.Values["Category"] = productSearchRouteData.Category.EscapeName();
               doRedirect = true;
           }

           if (filterContext.RouteData.Values["Manufacturer"] == null || 
               string.Compare(filterContext.RouteData.Values["Manufacturer"].ToString(), productSearchRouteData.Manufacturer.EscapeName(), StringComparison.InvariantCultureIgnoreCase) != 0)
           {
               filterContext.RouteData.Values["Manufacturer"] = productSearchRouteData.Manufacturer.EscapeName();
               doRedirect = true;
           }

           if (doRedirect)
           {
               filterContext.HttpContext.Response.StatusCode = 301;
               var urlHelper = new UrlHelper(new RequestContext(filterContext.HttpContext, new RouteData()));
               var url = urlHelper.RouteUrl(filterContext.RouteData.Values);

               //probably should do something here to avoid infinte redirect
               if (filterContext.HttpContext.Request.Url != null)
                   filterContext.Result = new RedirectResult(url + filterContext.HttpContext.Request.Url.Query);
           }

           base.OnActionExecuting(filterContext);
       }
    }
}