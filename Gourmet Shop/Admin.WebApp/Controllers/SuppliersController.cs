using GourmetShop.DataAccess.Data;
using GourmetShop.DataAccess.Models;
using GourmetShop.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Admin.WebApp.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly GourmetShopDbContext _context;

        public SuppliersController(ISupplierRepository supplierRepository, GourmetShopDbContext context)
        {
            _supplierRepository = supplierRepository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            return View(suppliers);
        }

        [HttpGet]
        public ActionResult AddSupplier()
        {
            
            return View(new Supplier());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSupplier(Supplier supplier)
        {
           
                await _supplierRepository.AddAsync(supplier);
                return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] Supplier supplier)
        {
            if (supplier == null) return BadRequest("Invalid supplier.");
            var existingSupplier = await _supplierRepository.GetAsync(supplier.Id);
            if (existingSupplier == null) return NotFound("Supplier not found.");
            
            if (!ModelState.IsValid)
            {
                return BadRequest($"Model state is invalid" + ModelState);
            }

            _context.Entry(existingSupplier).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            await _supplierRepository.UpdateAsync(supplier);
            return Ok(new { message = "Supplier updated" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var existingProduct = await _supplierRepository.GetAsync(id);
            if (existingProduct == null)
            {
                return NotFound("Supplier not found.");
            }


            await _supplierRepository.DeleteAsync(id);
            return Ok(new { message = "Supplier deleted" });
        }
    }
}
