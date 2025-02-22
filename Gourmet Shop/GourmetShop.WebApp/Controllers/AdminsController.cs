using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GourmetShop.DataAccess.Data;
using GourmetShop.DataAccess.Models;
using GourmetShop.DataAccess.Repositories;

namespace GourmetShop.WebApp.Controllers
{
    public class AdminsController : Controller
    {
        private readonly AdminRepository _adminRepository;

        public AdminsController(AdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        // GET: Admins
        [HttpGet("admin/get/{userId}")]
        public async Task<IActionResult> GetAdminByUserId(int userId)
        {
            var admin = await _adminRepository.GetByUserIdAsync(userId);
            if (admin == null)
            {
                return NotFound("Admin not found");
            }
            return View(admin);
        }

        // GET: ProductSales
        [HttpGet("admin/product-sales/{productId}")]
        public async Task<IActionResult> GetProductSales(int productId)
        {
            var (totalUnitsSold, totalSalesAmount) = await _adminRepository.GetProductSalesAync(productId);

            var result = new
            {
                TotalUnits = totalUnitsSold,
                TotalSalesAmount = totalSalesAmount
            };

            //CHECKME: I'm not sure if this is the correct way to return this
                     //I assume this will actually be put into some sort of container on the view
            return Json(result);
        }

        
    }
}
