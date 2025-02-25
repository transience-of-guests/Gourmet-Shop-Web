using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GourmetShop.DataAccess.Models;
using GourmetShop.DataAccess.Repositories;
using GourmetShop.DataAccess.Repositories.Interfaces;
using GourmetShop.DataAccess.Repositories.Classes;

// TODO: Modify to use the repository, not the model
namespace GourmetShop.WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ISubCategoryRepository _subcategoryRepository;


        public ProductsController(IProductRepository productRepository, ISubCategoryRepository subcategoryRepository)
        {
            _productRepository = productRepository;
            _subcategoryRepository = subcategoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _productRepository.GetAllAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Modify to use the repository, not the model, need to load the potential subcategories and suppliers
            ViewBag.SubcategoryId = new SelectList(await _productRepository.GetSelectableSubcategories(), "Id", "Name");
            ViewBag.SupplierId = new SelectList(await _productRepository.GetSelectableSuppliers(), "Id", "CompanyName");

            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create(Product product)
        {
            await _productRepository.AddAsync(product);
            return RedirectToAction(nameof(Index));

            // FIXME: Validation will never work because ModelState is always invalid, need to use ViewModels?
            /*if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.AddAsync(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.SubcategoryId = new SelectList(await _productRepository.GetSelectableSubcategories(), "Id", "Name");
            ViewBag.SupplierId = new SelectList(await _productRepository.GetSelectableSuppliers(), "Id", "CompanyName");

            return View(product);*/
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.SubcategoryId = new SelectList(await _productRepository.GetSelectableSubcategories(), "Id", "Name");
            ViewBag.SupplierId = new SelectList(await _productRepository.GetSelectableSuppliers(), "Id", "CompanyName");

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            await _productRepository.UpdateAsync(product);
            return RedirectToAction(nameof(Index));

            // FIXME: Validation will never work because ModelState is always invalid, need to use ViewModels?
            /*if (ModelState.IsValid)
            {
                await _productRepository.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SubcategoryId = new SelectList(await _productRepository.GetSelectableSubcategories(), "Id", "Name");
            ViewBag.SupplierId = new SelectList(await _productRepository.GetSelectableSuppliers(), "Id", "CompanyName");
            return View(product);*/
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        //Added Check Me
        //public async Task<IActionResult> AvailableProducts()
        //{
        //    var products = await _productRepository.GetAvailableProductsForCust();
        //    return View(products);
        //}

        
        public async Task<IActionResult> AvailableProducts(int? subcategoryId)
        {

            // Hardcoded products for testing purposes



            var subcategories = await _subcategoryRepository.GetAllSubcategoriesAsync();
            ViewData["Subcategories"] = subcategories;

            // Get available products, filtered by subcategory if provided
            var products = subcategoryId.HasValue
                ? await _subcategoryRepository.GetProductsBySubcategoryAsync(subcategoryId.Value)
                : await _productRepository.GetAvailableProductsForCust();  // Or get all available products if no filter is applied

            return View("Products", products);
        }

        //    public async Task<IActionResult> AvailableProducts(int? subcategoryId)
        //    {
        //        // Hardcoded products for testing purposes
        //        var hardcodedProducts = new List<Product>
        //{
        //    new Product { Id = 1, ProductName = "Product 1", UnitPrice = 10.99m, IsDiscontinued = false, Subcategory = new Subcategory { Name = "Subcategory 1" } },
        //    new Product { Id = 2, ProductName = "Product 2", UnitPrice = 12.99m, IsDiscontinued = false, Subcategory = new Subcategory { Name = "Subcategory 2" } },
        //    new Product { Id = 3, ProductName = "Product 3", UnitPrice = 14.99m, IsDiscontinued = false, Subcategory = new Subcategory { Name = "Subcategory 3" } }
        //};

        //        var hardcodedSubcategories = new List<Subcategory>
        //{
        //    new Subcategory { Id = 1, Name = "Subcategory 1"},
        //    new Subcategory { Id = 2, Name = "Subcategory 2" },
        //    new Subcategory { Id = 3, Name = "Subcategory 3"  }
        //};

        //        // Get all subcategories for filtering (hardcoded data)
        //        ViewData["Subcategories"] = hardcodedSubcategories;

        //        // Get all subcategories for filtering
        //        var subcategories = await _subcategoryRepository.GetAllSubcategoriesAsync();
        //        //ViewData["Subcategories"] = subcategories;

        //        // Set the hardcoded products in ViewData
        //        ViewData["Products"] = hardcodedProducts;

        //        return View("Products");
        //    }

    }
}
