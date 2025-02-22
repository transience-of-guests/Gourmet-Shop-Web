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
    public class ShoppingCartsController : Controller
    {
        private readonly ShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartsController(ShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet("cart/get-cart-id/{customerId}")]
        public async Task<IActionResult> GetCartId(int customerId)
        {
            int cartId = await _shoppingCartRepository.GetCartIdForCustomerAsync(customerId);
            return Ok(new { CartId = cartId });
        }

        [HttpPost("cart/add")]
        public async Task<IActionResult> AddToCart(int customerId, int productId, int quantity)
        {
            await _shoppingCartRepository.AddToCartAsync(customerId, productId, quantity);
            return Ok(new { Message = "Product added to cart" });
        }

        [HttpPost("cart/update")]
        public async Task<IActionResult> UpdateCartItemQuantity(int cartId, int productId, int newQuantity)
        {
            await _shoppingCartRepository.UpdateCartItemQuantity(cartId, productId, newQuantity);
            return Ok(new { Message = "Cart updated" });

        }

        [HttpPost("cart/remove")]
        public async Task<IActionResult> RemoveFromCart(int cartId, int productId)
        {
            await _shoppingCartRepository.RemoveFromCart(cartId, productId);
            return Ok(new { Message = "Item removed from cart" });
        }

        [HttpPost("cart/clear")]
        public async Task<IActionResult> ClearCart(int cartId)
        {
            await _shoppingCartRepository.ClearCart(cartId);
            return Ok(new { Message = "Cart has been cleared." });
        }

        [HttpGet("cart/view/{cartId}")]
        public async Task<IActionResult> ViewCart(int cartId)
        {
            var cartItems = await _shoppingCartRepository.ViewCartASync(cartId);
            return Ok(cartItems);
        }

        [HttpPost("cart/place-order")]
        public async Task<IActionResult> PlaceOrder(int customerId)
        {
            bool success = await _shoppingCartRepository.PlaceOrderAync(customerId);
            return success
                ? Ok(new { Message = "Order placed." })
                : BadRequest(new { Message = "Failed to place order." });
        }

        
    }
}
