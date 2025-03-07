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
using GourmetShop.DataAccess.Repositories.Interfaces.CRUD_Subinterfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GourmetShop.WebApp.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ShoppingCartsController> _logger;
        private readonly IUserInfoRepository _userInfoRepository;

        public ShoppingCartsController(IShoppingCartRepository shoppingCartRepository, IHttpContextAccessor httpContextAccessor, ILogger<ShoppingCartsController> logger, IUserInfoRepository userInfoRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _userInfoRepository = userInfoRepository;
        }

        private async Task<int?> GetCustomerIdFromSession()
        {
            int? customerId = null;

            // Reference Login.cshtml.cs ln.130 to see where this is set
            string authId = _httpContextAccessor.HttpContext?.Session.GetString("UserId");

            if (authId == null && User.Identity.IsAuthenticated)
            {
                authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            if (authId != null)
            {
                // FIXME
                // Need to create a user info when you register the user, otherwise there will be a bug where you can't get that
                UserInfo? userInfo = await _userInfoRepository.GetByAuthenticationIdAsync(authId);

                if (userInfo != null)
                {
                    customerId = userInfo.Id;
                }
            }

            return customerId;
        }

        [HttpGet("cart/get-cart-id")]
        [Authorize]
        public async Task<IActionResult> GetCartId()
        {
            int customerId = (int) await GetCustomerIdFromSession();

            if (customerId == null)
                return Unauthorized(new { Message = "User not logged in." });

            int cartId = await _shoppingCartRepository.GetCartIdForCustomerAsync(customerId);
            return Ok(new { CartId = cartId });
        }

        //[HttpPost("cart/add")]
        //public async Task<IActionResult> AddToCart(int productId, int quantity)
        //{
        //    int customerId = (int) await GetCustomerIdFromSession();
        //    if (customerId == null)
        //        return Unauthorized(new { Message = "User not logged in." });

        //    await _shoppingCartRepository.AddToCartAsync(customerId, productId, quantity);
        //   // return Ok(new { Message = "Product added to cart" });
        //    // Redirect to the cart view
        //    return RedirectToAction("ViewCart");
        //}

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1) // Default quantity to 1
        {
            int customerId = (int) await GetCustomerIdFromSession();

            await _shoppingCartRepository.AddToCartAsync(customerId, productId, quantity);
            return RedirectToAction("ViewCart", "ShoppingCarts");
        }

        [HttpPost("cart/update")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateCartItemQuantity(int cartId, int productId, int newQuantity)
        {
            int customerId = (int) await GetCustomerIdFromSession();
          
            await _shoppingCartRepository.UpdateCartItemQuantity(cartId, productId, newQuantity);
            return Ok(new { Message = "Cart updated" });
        }

        [HttpPost("cart/remove")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemoveFromCart(int cartId, int productId)
        {
            int customerId = (int) await GetCustomerIdFromSession();
          
            await _shoppingCartRepository.RemoveFromCart(cartId, productId);
            return Ok(new { Message = "Item removed from cart" });
        }

        [HttpPost("cart/clear")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ClearCart(int cartId)
        {
            int customerId = (int) await GetCustomerIdFromSession();
            
            await _shoppingCartRepository.ClearCart(cartId);
            return Ok(new { Message = "Cart has been cleared." });
        }

        [HttpGet("cart/view")]
        [Authorize(Roles = "Customer")]
        //public async Task<IActionResult> ViewCart()
        //{
        //    int customerId = (int) await GetCustomerIdFromSession();
        //    if (customerId == null)
        //        return Unauthorized(new { Message = "User not logged in." });

        //    int cartId = await _shoppingCartRepository.GetCartIdForCustomerAsync(customerId);
        //    var cartItems = await _shoppingCartRepository.ViewCartAsync(cartId);
        //    // Pass the cart items to the view
        //    return View("ShoppingCart", cartItems);
        //    //return Ok(cartItems);
        //}
        public async Task<IActionResult> ViewCart()
        {
            int customerId = (int) await GetCustomerIdFromSession();
  
            int cartId = await _shoppingCartRepository.GetCartIdForCustomerAsync(customerId);
            var cartItems = await _shoppingCartRepository.ViewCartAsync(cartId);
            return View("ShoppingCart", cartItems);
        }


        [HttpPost("cart/place-order")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PlaceOrder()
        {
            int customerId = (int) await GetCustomerIdFromSession();

            bool success = await _shoppingCartRepository.PlaceOrderAsync(customerId);
            if (success)
            {
                // Redirect to the "OrderPlaced" view after order is successfully placed
                return View("OrderPlaced");
            }
            else
            {
                return BadRequest(new { Message = "Failed to place order." });
            }
        }

        //[HttpPost("cart/place-order")]
        //public async Task<IActionResult> PlaceOrder()
        //{
        //    int customerId = (int) await GetCustomerIdFromSession();
        //    if (customerId == null)
        //        return Unauthorized(new { Message = "User not logged in." });

        //    bool success = await _shoppingCartRepository.PlaceOrderAsync(customerId);
        //    return success
        //        ? Ok(new { Message = "Order placed." })
        //        : BadRequest(new { Message = "Failed to place order." });
        //}

        //[HttpGet("cart/get-cart-id/{customerId}")]
        //public async Task<IActionResult> GetCartId(int customerId)
        //{
        //    int cartId = await _shoppingCartRepository.GetCartIdForCustomerAsync(customerId);
        //    return Ok(new { CartId = cartId });
        //}

        //[HttpPost("cart/add")]
        //public async Task<IActionResult> AddToCart(int customerId, int productId, int quantity)
        //{
        //    await _shoppingCartRepository.AddToCartAsync(customerId, productId, quantity);
        //    return Ok(new { Message = "Product added to cart" });
        //}

        //[HttpPost("cart/update")]
        //public async Task<IActionResult> UpdateCartItemQuantity(int cartId, int productId, int newQuantity)
        //{
        //    await _shoppingCartRepository.UpdateCartItemQuantity(cartId, productId, newQuantity);
        //    return Ok(new { Message = "Cart updated" });

        //}

        //[HttpPost("cart/remove")]
        //public async Task<IActionResult> RemoveFromCart(int cartId, int productId)
        //{
        //    await _shoppingCartRepository.RemoveFromCart(cartId, productId);
        //    return Ok(new { Message = "Item removed from cart" });
        //}

        //[HttpPost("cart/clear")]
        //public async Task<IActionResult> ClearCart(int cartId)
        //{
        //    await _shoppingCartRepository.ClearCart(cartId);
        //    return Ok(new { Message = "Cart has been cleared." });
        //}

        //[HttpGet("cart/view/{cartId}")]
        //public async Task<IActionResult> ViewCart(int cartId)
        //{
        //    var cartItems = await _shoppingCartRepository.ViewCartAsync(cartId);
        //    return Ok(cartItems);
        //}

        //[HttpPost("cart/place-order")]
        //public async Task<IActionResult> PlaceOrder(int customerId)
        //{
        //    bool success = await _shoppingCartRepository.PlaceOrderAsync(customerId);
        //    return success
        //        ? Ok(new { Message = "Order placed." })
        //        : BadRequest(new { Message = "Failed to place order." });
        //}


    }
}
