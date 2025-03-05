using GourmetShop.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Admin.WebApp.Controllers
{
    public class Suppliers : Controller
    {
        private readonly ISupplierRepository _supplierRepository;

        public Suppliers(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            return View(suppliers);
        }
    }
}
