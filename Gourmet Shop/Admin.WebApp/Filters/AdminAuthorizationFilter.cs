using GourmetShop.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.WebApp.Filters
{
    public class AdminAuthorizationFilter : IAuthorizationFilter
    {
        private readonly SignInManager<Authentication> _signInManager;

        public AdminAuthorizationFilter(SignInManager<Authentication> signInManager)
        {
            _signInManager = signInManager;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated && !context.HttpContext.User.IsInRole("Admin"))
            {
                await _signInManager.SignOutAsync();
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
