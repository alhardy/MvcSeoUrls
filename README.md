 
Recently I was tasked to generate some SEO friendly URLs for an asp.net MVC project. My solution below is available at git hub In the past I've always taken the route in achieving this with URL re-writing rather than using routing. The annoyance I find with URL re-writing is a few things,

It becomes messy quite quickly in managing an XML configuration file with a whole heap of regular expressions
It's can becomes difficult to determine what rules match for a given URL
Routes and re-write rules will end up conflicting
Regular expressions can start to get complicated
So after doing a bit of research on how to achieve SEO friending URLs using MVC routing, I came across AttributeRouting. Attribute Routing is an open source product with allows you to define routes in a much cleaner way than the out of box asp.net MVC way. I wont go into detail, I recommend you go check it out.
 
Although this is an awesome framework for managing routes I still needed a way to dynamically generate links for SEO friendly URLs, provide an alias route parameters and bind comma delimited strings into a List or Collection of objects. So here's the approach I've taken.
 
Using AttributeRouting, I've defined my routes on an action. I have also created a custom action filter which will perform a 301 redirect in the case of the Category and Manufacturer Names not matching the given their correct values (given the id for each of these)
        
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
 
 In the about example the List<int> will automatically be converted a comma delimited string. asp.net MVC by default will not automatically ind a comma delimited string back to any type of IEnumerable, to achieve this I've set my default model binder to a custom model binder which also will take into account any RouteAlias attributes. You'll also notice an attribute "MatchRouteDataValue" on the manufacturer and category properties, this basically tells the SeoRedirectActionFilter to perform a 301 redirect to a URL with the correct parameters substituted if the route data values with keys of each property name are incorrect.


 
