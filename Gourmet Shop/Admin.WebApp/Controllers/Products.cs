using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GourmetShop.DataAccess.Repositories;
using GourmetShop.DataAccess.Models;

namespace Admin.WebApp.Controllers
{

    public class Products : Controller
    {
        private readonly IProductRepository _productRepository;
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public Products(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }
    }

    
    
}
