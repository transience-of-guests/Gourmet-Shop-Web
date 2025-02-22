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
    public class SuppliersController : Controller
    {
        private readonly SupplierRepository _supplierRepository;

        public SuppliersController(SupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _supplierRepository.GetAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }
        //Show form if we are going this route
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                await _supplierRepository.AddAsync(supplier);
                return RedirectToAction(nameof(Index));
            }

            return View(supplier);
        }
        //Show form if we are going this route
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = _supplierRepository.GetAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Supplier supplier)
        {
            if(id != supplier.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _supplierRepository.UpdateAsync(supplier);
                return RedirectToAction(nameof(Index));
            }

            return View(supplier);
        }

        //Show confimation of deletion
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _supplierRepository.GetAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _supplierRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
