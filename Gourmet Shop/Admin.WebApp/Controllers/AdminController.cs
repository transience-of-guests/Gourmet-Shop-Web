using System.Security.Principal;
using Admin.WebApp.Models;
using GourmetShop.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Admin.WebApp.Models.LoginViewModel;

namespace Admin.WebApp.Controllers
{
    public class AdminController : Controller
    {

        private readonly UserManager<Authentication> _userManager;
        private readonly SignInManager<Authentication> _signInManager;

        public AdminController(UserManager<Authentication> userManager, SignInManager<Authentication> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminLogin()
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminRegister()
        {
            return RedirectToPage("/Account/Register", new { area = "Identity" });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    if (!_userManager.IsInRoleAsync(user, "Admin").Result)
                    {
                        await _signInManager.SignOutAsync();
                        Console.WriteLine("User is not an Admin");
                        return RedirectToPage("/Account/Login", new { area = "Identity" });
                    }

                    return RedirectToAction("Dashboard", "Admin");
                }
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); // Redirects to Home page after logout
        }
    }
}

