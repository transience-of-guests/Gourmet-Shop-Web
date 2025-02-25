using GourmetShop.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GourmetShop.WebApp.Controllers
{
    public class SubCategoryController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly ISubCategoryRepository _subcategoryRepository;

        public SubCategoryController(ISubCategoryRepository subcategoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubcategories()
        {
            var subcategories = await _subcategoryRepository.GetAllSubcategoriesAsync();
            return Ok(subcategories);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsBySubcategory(int subcategoryId)
        {
            var products = await _subcategoryRepository.GetProductsBySubcategoryAsync(subcategoryId);
            return Ok(products);
        }
    }
}
