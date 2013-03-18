using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MvcSeoUrls.Attributes;

namespace MvcSeoUrls.Core
{
    public class ProductSearchCriteriaRouteData
    {
        private readonly ICollection<ProductCategory> _categories;
        private readonly ICollection<ProductManufacturer> _manufacturers;

        public ProductSearchCriteriaRouteData()
        {
            // Dynamic Lookup from cache
            _categories = new Collection<ProductCategory> { new ProductCategory { Id = 1, Name = "Category 1" }, new ProductCategory { Id = 2, Name = "Category 2" } };
            _manufacturers = new Collection<ProductManufacturer> { new ProductManufacturer { Id = 1, Name = "Manufacturer 1" }, new ProductManufacturer { Id = 2, Name = "Manufacturer 2" } };
        }   
  
        [RouteAlias("c")]
        public int CategoryId { get; set; }
        [RouteAlias("m")]
        public int ManufactureId { get; set; }
        [RouteAlias("k")]
        public string Keywords { get; set; }
        [RouteAlias("cr")]
        public List<int> Colours { get; set; }

        [MatchRouteDataValue]
        public string Manufacturer
        {
            get { return _manufacturers.Where(x => x.Id == ManufactureId).Select(x => x.Name).FirstOrDefault(); }
        }
        [MatchRouteDataValue]
        public string Category
        {
            get { return _categories.Where(x => x.Id == CategoryId).Select(x => x.Name).FirstOrDefault(); }
        }     
    }
}