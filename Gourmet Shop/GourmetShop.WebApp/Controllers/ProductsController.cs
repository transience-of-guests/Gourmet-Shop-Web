using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GourmetShop.DataAccess.Models;
using GourmetShop.DataAccess.Repositories;

// TODO: Modify to use the repository, not the model
namespace GourmetShop.WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
    }
}
