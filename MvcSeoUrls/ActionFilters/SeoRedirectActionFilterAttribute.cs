using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSeoUrls.Extensions;

namespace MvcSeoUrls.ActionFilters
{
    public class SeoRedirectActionFilterAttribute : ActionFilterAttribute
    {
        #region fields

        private readonly Type _routeDataType;

        #endregion

        #region ctor
    
        public SeoRedirectActionFilterAttribute(Type routeDataType)
        {
            _routeDataType = routeDataType;
        }

        #endregion

        #region ActionFilterAttribute overrides

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var routeData = filterContext.ActionParameters.Values.FirstOrDefault(x => x.GetType() == _routeDataType);

            if (routeData == null || filterContext.RouteData == null) return;

            var doRedirect = routeData.EnsureRouteDataValueMatches(filterContext.RouteData.Values);

            if (doRedirect)
            {
                filterContext.HttpContext.Response.StatusCode = 301;
                var urlHelper = new UrlHelper(new RequestContext(filterContext.HttpContext, new RouteData()));
                var url = urlHelper.RouteUrl(filterContext.RouteData.Values);

                //TODO: Do something here to avoid infinite redirect
                if (filterContext.HttpContext.Request.Url != null)
                    filterContext.Result = new RedirectResult(url + filterContext.HttpContext.Request.Url.Query);
            }

            base.OnActionExecuting(filterContext);
        }

        #endregion
    }
}