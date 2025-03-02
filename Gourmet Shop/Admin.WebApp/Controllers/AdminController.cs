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
        public IActionResult AdminLogin()
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        [HttpGet]
        public IActionResult AdminRegister()
        {
            return RedirectToPage("/Account/Register", new { area = "Identity" });
        }

        [HttpPost]
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
                    return RedirectToAction("Dashboard", "Admin");
                }
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }
    }
}

