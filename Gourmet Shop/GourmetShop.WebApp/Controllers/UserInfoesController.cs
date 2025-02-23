using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GourmetShop.DataAccess.Data;
using GourmetShop.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using GourmetShop.DataAccess.Repositories;

namespace GourmetShop.WebApp.Controllers
{
    public class UserInfoesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IUserInfoRepository _userInfoRepository;

        public UserInfoesController(IUserInfoRepository userInfoRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userInfoRepository = userInfoRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // TODO: Authorize so that only admins can see this
        // GET: UsersController
        [Authorize(Roles = "Admin")]
        [HttpGet("customer/get/{userId}")]
        public async Task<IActionResult> Index()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Customer");

            // Get the IDs of users in the role as they are all strings
            List<string?> authIds = usersInRole.Select(u => u.Id).ToList();

            var users = await _userInfoRepository.GetAllByRoleAsync(authIds);
            return View(users);
        }

        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Details(int id)
        {
            // Separate user information
            var userInfo = await _userInfoRepository.GetAsync(id);
            if (userInfo == null || userInfo.AuthenticationId == null) // Might want to remove the second part of this check
            {
                return NotFound();
            }

            // Grabs ASP.NET Identity user
            var auth = await _userManager.FindByIdAsync(userInfo.AuthenticationId);

            if (auth == null)
            {
                return NotFound();
            }

            // Customer should only be able to see their own information
            if (User.IsInRole("Customer") && User.FindFirstValue(ClaimTypes.NameIdentifier) != userInfo.AuthenticationId)
            {
                return Forbid();
            }

            return View(userInfo);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create(UserInfo userInfo)
        {
            await _userInfoRepository.AddAsync(userInfo);
            return RedirectToAction(nameof(Index));

            // FIXME: Validation will never work because ModelState is always invalid, need to use ViewModels?
            /*if (ModelState.IsValid)
            {
                try
                {
                    await _userInfoRepository.AddAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.SubcategoryId = new SelectList(await _userInfoRepository.GetSelectableSubcategories(), "Id", "Name");
            ViewBag.SupplierId = new SelectList(await _userInfoRepository.GetSelectableSuppliers(), "Id", "CompanyName");

            return View(user);*/
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var userInfo = await _userInfoRepository.GetAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserInfo userInfo)
        {
            await _userInfoRepository.UpdateAsync(userInfo);
            return RedirectToAction(nameof(Index));

            // FIXME: Validation will never work because ModelState is always invalid, need to use ViewModels?
            /*if (ModelState.IsValid)
            {
                await _userInfoRepository.UpdateAsync(userInfo);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SubcategoryId = new SelectList(await _userInfoRepository.GetSelectableSubcategories(), "Id", "Name");
            ViewBag.SupplierId = new SelectList(await _userInfoRepository.GetSelectableSuppliers(), "Id", "CompanyName");
            return View(userInfo);*/
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var userInfo = await _userInfoRepository.GetAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }
            return View(userInfo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userInfoRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
