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
    }
}
