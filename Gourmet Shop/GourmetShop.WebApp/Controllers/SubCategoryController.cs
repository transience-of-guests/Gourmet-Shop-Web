using GourmetShop.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GourmetShop.WebApp.Controllers
{
    public class SubCategoryController : Controller
    {

        private readonly ISubCategoryRepository _subcategoryRepository;
        private readonly IProductRepository _productRepository;

        public SubCategoryController(ISubCategoryRepository subcategoryRepository, IProductRepository productRepository)
        {
            _subcategoryRepository = subcategoryRepository;
            _productRepository = productRepository;
        }

        // GET: SubCategory
        //public async Task<IActionResult> Index()
        //{
        //    var subcategories = await _subcategoryRepository.GetAllAsync();
        //    return View(subcategories); // Pass subcategories to the view
        //}
        public async Task<IActionResult> Index()
        {
            var subcategories = await _subcategoryRepository.GetAllAsync();
            ViewData["Subcategories"] = subcategories; // Pass subcategories using ViewData
            return View();
            //var products = await _productRepository.GetAvailableProductsForCust();
            //return View(products); // Main view still gets the products
        }


        [HttpGet]
        public async Task<IActionResult> GetProductsBySubcategory(int subcategoryId)
        {
            var products = await _subcategoryRepository.GetProductsBySubcategoryAsync(subcategoryId);

            // Return the partial view with the filtered products
            return Ok(products);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetProductsBySubcategory(int subcategoryId)
        //{
        //    var products = await _subcategoryRepository.GetProductsBySubcategoryAsync(subcategoryId);

        //    // Pass the filtered products to the view
        //    return View("Products", products); // Change "ProductView" to the name of your existing product view
        //}


        //    //public IActionResult Index()
        //    //{
        //    //    return View();
        //    //}

        //    private readonly ISubCategoryRepository _subcategoryRepository;

        //    public SubCategoryController(ISubCategoryRepository subcategoryRepository)
        //    {
        //        _subcategoryRepository = subcategoryRepository;
        //    }

        //    [HttpGet]
        //    public async Task<IActionResult> GetAllSubcategories()
        //    {
        //        var subcategories = await _subcategoryRepository.GetAllAsync();
        //        return Ok(subcategories);
        //    }

        //    [HttpGet]
        //    public async Task<IActionResult> GetProductsBySubcategory(int subcategoryId)
        //    {
        //        var products = await _subcategoryRepository.GetProductsBySubcategoryAsync(subcategoryId);
        //        return Ok(products);
        //    }
    }
}
