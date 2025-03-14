using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GourmetShop.DataAccess.Repositories;
using GourmetShop.DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GourmetShop.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;

namespace Admin.WebApp.Controllers
{

    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly GourmetShopDbContext _context;
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public ProductsController(IProductRepository productRepository, GourmetShopDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AddProduct()
        {
            var suppliers = _context.Suppliers?.ToList() ?? new List<Supplier>();
            var subcategories = _context.Subcategories?.ToList() ?? new List<Subcategory>();
            ViewBag.SupplierId = new SelectList(_context.Suppliers, "Id", "CompanyName");
            ViewBag.SubcategoryId = new SelectList(_context.Subcategories, "Id", "Name");
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddProduct(Product product)
        {
            //bind the supplier object to the product
            product.Supplier = _context.Suppliers.FirstOrDefault(s => s.Id == product.SupplierId);
            if (product.Supplier == null)
            {
                ModelState.AddModelError("SupplierId", "Invalid supplier selected.");
            }
            else
            {
                await _productRepository.AddAsync(product);
                return RedirectToAction("Index");
            }


            ViewBag.SupplierId = new SelectList(_context.Suppliers, "Id","CompanyName", product.SupplierId);
            ViewBag.SubcategoryId = new SelectList(_context.Subcategories, "Id", "Name", product.SubcategoryId);
            return View(product);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromForm] Product product)
        {
            if (product == null) return BadRequest("Invalid product data.");

            if (!ModelState.IsValid)
            {
                return BadRequest($"Model state is invalid" + ModelState);
            }

            await _productRepository.UpdateAsync(product);
            return Ok(new { message = "Product updated" });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingProduct = await _productRepository.GetAsync(id);
            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }
            

            await _productRepository.DeleteAsync(id);
            return Ok(new { message = "Product deleted" });
        }

        

       
    }


    
    
}
