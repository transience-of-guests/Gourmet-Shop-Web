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

namespace GourmetShop.WebApp.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ShoppingCartsController> _logger;

        public ShoppingCartsController(IShoppingCartRepository shoppingCartRepository, IHttpContextAccessor httpContextAccessor, ILogger<ShoppingCartsController> logger)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        private int? GetCustomerIdFromSession()
        {
            //return _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
            var customerIdString = _httpContextAccessor.HttpContext?.Session.GetString("UserId");

            if (int.TryParse(customerIdString, out int customerId))
            {
                return customerId;
            }

            return null;
        }

        [HttpGet("cart/get-cart-id")]
        public async Task<IActionResult> GetCartId()
        {
            var customerId = GetCustomerIdFromSession();
            if (customerId == null)
                return Unauthorized(new { Message = "User not logged in." });

            int cartId = await _shoppingCartRepository.GetCartIdForCustomerAsync(customerId.Value);
            return Ok(new { CartId = cartId });
        }

        //[HttpPost("cart/add")]
        //public async Task<IActionResult> AddToCart(int productId, int quantity)
        //{
        //    var customerId = GetCustomerIdFromSession();
        //    if (customerId == null)
        //        return Unauthorized(new { Message = "User not logged in." });

        //    await _shoppingCartRepository.AddToCartAsync(customerId.Value, productId, quantity);
        //   // return Ok(new { Message = "Product added to cart" });
        //    // Redirect to the cart view
        //    return RedirectToAction("ViewCart");
        //}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1) // Default quantity to 1
        {
            var customerId = GetCustomerIdFromSession();
            
            // Don't need this since we have Authorize
                if (customerId == null)
                {
                // Redirect to login page if user is not logged in
                    // return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

            await _shoppingCartRepository.AddToCartAsync(customerId.Value, productId, quantity);
            return RedirectToAction("ViewCart", "ShoppingCart");
        }

        [HttpPost("cart/update")]
        [Authorize]
        public async Task<IActionResult> UpdateCartItemQuantity(int cartId, int productId, int newQuantity)
        {
            var customerId = GetCustomerIdFromSession();
            if (customerId == null)
                return Unauthorized(new { Message = "User not logged in." });

            await _shoppingCartRepository.UpdateCartItemQuantity(cartId, productId, newQuantity);
            return Ok(new { Message = "Cart updated" });
        }

        [HttpPost("cart/remove")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int cartId, int productId)
        {
            var customerId = GetCustomerIdFromSession();
            if (customerId == null)
                return Unauthorized(new { Message = "User not logged in." });

            await _shoppingCartRepository.RemoveFromCart(cartId, productId);
            return Ok(new { Message = "Item removed from cart" });
        }

        [HttpPost("cart/clear")]
        [Authorize]
        public async Task<IActionResult> ClearCart(int cartId)
        {
            var customerId = GetCustomerIdFromSession();
            if (customerId == null)
                return Unauthorized(new { Message = "User not logged in." });

            await _shoppingCartRepository.ClearCart(cartId);
            return Ok(new { Message = "Cart has been cleared." });
        }

        [HttpGet("cart/view")]
        //public async Task<IActionResult> ViewCart()
        //{
        //    var customerId = GetCustomerIdFromSession();
        //    if (customerId == null)
        //        return Unauthorized(new { Message = "User not logged in." });

        //    int cartId = await _shoppingCartRepository.GetCartIdForCustomerAsync(customerId.Value);
        //    var cartItems = await _shoppingCartRepository.ViewCartASync(cartId);
        //    // Pass the cart items to the view
        //    return View("ShoppingCart", cartItems);
        //    //return Ok(cartItems);
        //}
        public async Task<IActionResult> ViewCart()
        {
            var customerId = GetCustomerIdFromSession();
            if (customerId == null)
            {
                _logger.LogInformation("Session does not contain UserId.");
                return Unauthorized(new { Message = "User not logged in." });
            }

            _logger.LogInformation("UserId from session: " + customerId);

            int cartId = await _shoppingCartRepository.GetCartIdForCustomerAsync(customerId.Value);
            var cartItems = await _shoppingCartRepository.ViewCartASync(cartId);
            return View("ShoppingCart", cartItems);
        }


        [HttpPost("cart/place-order")]
        public async Task<IActionResult> PlaceOrder()
        {
            var customerId = GetCustomerIdFromSession();
            if (customerId == null)
                return Unauthorized(new { Message = "User not logged in." });

            bool success = await _shoppingCartRepository.PlaceOrderAync(customerId.Value);
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
        //    var customerId = GetCustomerIdFromSession();
        //    if (customerId == null)
        //        return Unauthorized(new { Message = "User not logged in." });

        //    bool success = await _shoppingCartRepository.PlaceOrderAync(customerId.Value);
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
        //    var cartItems = await _shoppingCartRepository.ViewCartASync(cartId);
        //    return Ok(cartItems);
        //}

        //[HttpPost("cart/place-order")]
        //public async Task<IActionResult> PlaceOrder(int customerId)
        //{
        //    bool success = await _shoppingCartRepository.PlaceOrderAync(customerId);
        //    return success
        //        ? Ok(new { Message = "Order placed." })
        //        : BadRequest(new { Message = "Failed to place order." });
        //}


    }
}
