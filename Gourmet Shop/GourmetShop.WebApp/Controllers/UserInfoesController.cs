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
        private readonly UserManager<Authentication> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IUserInfoRepository _userInfoRepository;
        private readonly GourmetShopDbContext _context;

        public UserInfoesController(IUserInfoRepository userInfoRepository, UserManager<Authentication> userManager, RoleManager<IdentityRole> roleManager, GourmetShopDbContext context)
        {
            _userInfoRepository = userInfoRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
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

        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //public async Task<IActionResult> Create()
        //{
        //    string? authId = null;

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    }

        //    if (authId == null)
        //    {
        //        return RedirectToAction("Login", "Account"); // Redirect to login if not authenticated
        //        //return Redirect("/Identity/Account/Login");
        //    }

        //    // Check if the user already has a UserInfo record
        //    var existingUserInfo = await _userInfoRepository.GetByAuthenticationIdAsync(authId);
        //    if (existingUserInfo != null)
        //    {
        //        return RedirectToAction("Index", "Home"); // Redirect if user info already exists
        //    }

        //    // Pass the authId to the view
        //    var userInfo = new UserInfo { AuthenticationId = authId };
        //    return View(userInfo);


        //    // return View();
        //}
        //--------------------------------------------------------------------------
        ////[Authorize(Roles = "Admin")]
        /// <summary>
        /// 
        /// 

        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        /// 

        //[HttpGet]
        //public async Task<IActionResult> Create()
        //{
        //    // Get the authentication ID of the currently logged-in user
        //    string authId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (string.IsNullOrEmpty(authId))
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    // Check if the user already has a UserInfo record
        //    var existingUserInfo = await _userInfoRepository.GetByAuthenticationIdAsync(authId);
        //    if (existingUserInfo != null)
        //    {
        //        return RedirectToAction("Index", "Home"); // Or to any other page if UserInfo already exists
        //    }

        //    // Pass the authentication ID to the view (use it to populate the hidden field)
        //    return View(new UserInfo { AuthenticationId = authId });
        // GET: Create UserInfo



        //[HttpPost]
        //public async Task<IActionResult> Create(UserInfo userInfo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Assuming you have access to the current logged-in user
        //        var user = await _userManager.GetUserAsync(User);
        //        userInfo.AuthenticationId = user.Id;  // Assign the AuthenticationId to the logged-in user

        //        // Save to the database
        //        _context.UserInfo.Add(userInfo);
        //        await _context.SaveChangesAsync();

        //        return RedirectToAction("Index", "Home");  // Or wherever you want to redirect after saving
        //    }

        //    return View(userInfo);
        //}



        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create()
        {
            // Get the authentication ID of the currently logged-in user
            string? authId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (authId == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if no authentication ID found
            }

            // Check if the user already has a UserInfo record
            var existingUserInfo = await _userInfoRepository.GetByAuthenticationIdAsync(authId);
            if (existingUserInfo != null)
            {
                // Redirect to the home page or another page if UserInfo already exists
                return RedirectToAction("Index", "Home");
            }

            // The user does not have a UserInfo record, so create one
            // It's because in case if the user doesn't actually fill out their information, we can still have a record of them
            await _userInfoRepository.AddAsync(new UserInfo { AuthenticationId = authId, FirstName="", LastName="", City="", Country="" });
            UserInfo user = await _userInfoRepository.GetByAuthenticationIdAsync(authId);

            // Pass the AuthenticationId to the view so the user can fill out their info
            ViewBag.AuthUserId = authId;
            return View(user); // Pre-populate the AuthenticationId field
        }

        // POST: Create UserInfo

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(UserInfo userInfo)
        {
            // Model state will always be invalid since we're actually not using a model
            /*if (!ModelState.IsValid)
            {
                // Log validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine("Validation Errors: " + string.Join(", ", errors));
                return View(userInfo);
            }*/

            // Ensure AuthenticationId is populated from the authenticated user if it's not already set
            if (string.IsNullOrEmpty(userInfo.AuthenticationId) && User.Identity.IsAuthenticated)
            {
                userInfo.AuthenticationId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the authenticated user ID
            }

            if (string.IsNullOrEmpty(userInfo.AuthenticationId))
            {
                return RedirectToAction("Login", "Account"); // Redirect if no AuthenticationId is found
            }

            try
            {
                // Verify if the AuthenticationId exists in the Users table and assign it
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.AuthenticationId == userInfo.AuthenticationId);
                if (existingUser == null)
                    throw new Exception("User not found.");

                userInfo.AuthenticationId = existingUser.AuthenticationId; // Set AuthenticationId to match the user ID

                // Add UserInfo to the database
                // await _userInfoRepository.AddAsync(userInfo);
                // CHECKME: Was getting an error of DbUpdateConcurrencyException: The database operation was expected to affect 1 row(s), but actually affected 0 row(s); data may have been modified or deleted since entities were loaded. See https://go.microsoft.com/fwlink/?LinkId=527962 for information on understanding and handling optimistic concurrency exceptions.
                
                existingUser.FirstName = userInfo.FirstName;
                existingUser.FirstName = userInfo.LastName;
                existingUser.City = userInfo.City;
                existingUser.Country = userInfo.Country;

                await _userInfoRepository.UpdateAsync(existingUser);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(userInfo);
            }
            // Redirect after successful creation
            return RedirectToAction("AvailableProducts", "Products");
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(UserInfo userInfo)
        //{
        //    //userInfo.AuthenticationId = _context.Users.FirstOrDefault(s => s.Id == userInfo.AuthenticationId);
        //    if (!ModelState.IsValid)
        //    {
        //        // If the model is invalid, log validation errors and return the same view with errors
        //        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        //        Console.WriteLine("Validation Errors: " + string.Join(", ", errors));
        //        return View(userInfo);
        //    }

        //    // Ensure AuthenticationId is populated from the authenticated user if it's not already set
        //    if (string.IsNullOrEmpty(userInfo.AuthenticationId) && User.Identity.IsAuthenticated)
        //    {
        //        userInfo.AuthenticationId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Set the AuthenticationId from the logged-in user
        //    }

        //    if (string.IsNullOrEmpty(userInfo.AuthenticationId))
        //    {
        //        return RedirectToAction("Login", "Account"); // Redirect to login if no AuthenticationId is found
        //    }


        //    await _userInfoRepository.AddAsync(userInfo);

        //    // Redirect to the next page after successful creation
        //    return RedirectToAction("AvailableProducts", "Products");
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(UserInfo userInfo)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(userInfo); // Return to the same view with validation errors
        //    }

        //    // If the AuthenticationId is missing, set it (it should already be in the model from the hidden input field)
        //    if (string.IsNullOrEmpty(userInfo.AuthenticationId))
        //    {
        //        userInfo.AuthenticationId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    }

        //    // Add the user info to the database
        //    await _userInfoRepository.AddAsync(userInfo);

        //    // Redirect to another page after successful creation
        //    return RedirectToAction("AvailableProducts", "Products");
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(UserInfo userInfo)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        //        Console.WriteLine("Validation Errors: " + string.Join(", ", errors)); // Log validation errors
        //        return View(userInfo); // Return the same view with validation errors
        //    }

        //    if (string.IsNullOrEmpty(userInfo.AuthenticationId) && User.Identity.IsAuthenticated)
        //    {
        //        userInfo.AuthenticationId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    }

        //    if (string.IsNullOrEmpty(userInfo.AuthenticationId))
        //    {
        //        // If authentication ID is not found, redirect to login
        //        return RedirectToAction("Login", "Account");
        //    }

        //    await _userInfoRepository.AddAsync(userInfo);
        //    return RedirectToAction("AvailableProducts", "Products");
        //}


        //[HttpPost]
        //public async Task<IActionResult> Create(UserInfo userInfo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Ensure the user is authenticated and fetch their Authentication ID
        //        if (string.IsNullOrEmpty(userInfo.AuthenticationId) && User.Identity.IsAuthenticated)
        //        {
        //            userInfo.AuthenticationId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        }

        //        if (string.IsNullOrEmpty(userInfo.AuthenticationId))
        //        {
        //            // return RedirectToAction("Login", "Account"); // Redirect to login if no valid authentication ID
        //            return Redirect("/Identity/Account/Login");
        //        }

        //        // Save the user info to the database
        //        await _userInfoRepository.AddAsync(userInfo);

        //        // Redirect the user after successfully creating the UserInfo record
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View(userInfo); // Return view with validation errors if form submission is invalid
        //}

        //public async Task<IActionResult> Create(UserInfo userInfo)
        //{
        //    //await _userInfoRepository.AddAsync(userInfo);
        //    //return RedirectToAction(nameof(Index));




        //    var user = await _userManager.GetUserAsync(User); // Get the current logged-in user

        //    if (user == null)
        //    {
        //        return RedirectToAction("Index", "Home"); // Redirect if user is not authenticated
        //        //Needs to be dredurected to the login.
        //    }

        //    ViewBag.AuthUserId = user.Id; // Assign user ID to ViewBag

        //    return View();




        //    // FIXME: Validation will never work because ModelState is always invalid, need to use ViewModels?
        //    /*if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await _userInfoRepository.AddAsync(user);
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", ex.Message);
        //        }
        //    }

        //    ViewBag.SubcategoryId = new SelectList(await _userInfoRepository.GetSelectableSubcategories(), "Id", "Name");
        //    ViewBag.SupplierId = new SelectList(await _userInfoRepository.GetSelectableSuppliers(), "Id", "CompanyName");

        //    return View(user);*/
        //}

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
