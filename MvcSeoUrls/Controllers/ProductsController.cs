using AttributeRouting;
using AttributeRouting.Web.Mvc;
using MvcSeoUrls.ActionFilters;
using MvcSeoUrls.Core;
using System.Web.Mvc;

namespace MvcSeoUrls.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : Controller
    {
        [SeoRedirectActionFilter(typeof(ProductSearchCriteriaRouteData))]
        [GET("{Category}/{Manufacturer}")]
        public ActionResult Search(ProductSearchCriteriaRouteData searchCriteriaRouteData)
        {
            // map route data to domain object and perform search
            ViewBag.Message = string.Format("categoryId: {0}, manufacturerId: {1}, keywords {2}", searchCriteriaRouteData.CategoryId, searchCriteriaRouteData.ManufactureId, searchCriteriaRouteData.Keywords);

            return View("Index");
        }
    }
}
