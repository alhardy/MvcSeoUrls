using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSeoUrls.Core;

namespace MvcSeoUrls.Extensions
{
    public static class UrlExtensions
    {
        public static string GenerateUrl(this UrlHelper helper, ProductSearchCriteriaRouteData searchCriteriaRouteData)
        {
            return helper.RouteUrl("Products_Search", searchCriteriaRouteData.ToRouteData());           
        }

        public static string GenerateUrl(this UrlHelper helper, RouteValueDictionary rd)
        {
            return helper.RouteUrl("Products_Search", rd);
        }
    }

}