 
        
		[SeoRedirectActionFilter(typeof(ProductSearchCriteriaRouteData))]
        [GET("{Category}/{Manufacturer}")]        
        public ActionResult Search(ProductSearchCriteriaRouteData searchCriteriaRouteData)
        {
            // map route data to domain object and perform search
            ViewBag.Message = string.Format("categoryId: {0}, manufacturerId: {1}, keywords {2}", searchCriteriaRouteData.CategoryId, searchCriteriaRouteData.ManufactureId, searchCriteriaRouteData.Keywords);

            return View("Index");
        } 
In my views for example to generate a URL I simply use a UrlHelper extension method

	<a href="@(Url.GenerateUrl(new ProductSearchCriteriaRouteData{CategoryId = 1, ManufactureId = 2, Keywords = "aklsdfj", Colours = new List<int>{1, 2, 3}}))">Product Search</a>

Url.GenerateUrl is a UrlHelper extension method which dynamically generates route data from an object also taking into account any alias I need and then simply uses RouteUrl passing name and route data to generate an SEO url.



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



 
