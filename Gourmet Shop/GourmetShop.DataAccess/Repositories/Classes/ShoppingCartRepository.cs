using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GourmetShop.DataAccess.Repositories.Interfaces.CRUD_Subinterfaces;
using GourmetShop.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using GourmetShop.DataAccess.Data;

namespace GourmetShop.DataAccess.Repositories
{
    //CHECKME
    public class ShoppingCartRepository : GourmetShopRepository, IShoppingCartRepository
    {
      
        public ShoppingCartRepository(string connectionString) : base(connectionString)
        {
        }

        public ShoppingCartRepository(GourmetShopDbContext context) : base(context)
        {
        }
        public async Task AddToCartAsync(int customerId, int productId, int quantity)
        {
            var cart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(c => c.UserId == customerId);

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = customerId,
                    CreatedDate = DateTime.Now
                };
                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = await _context.ShoppingCartDetails
                .FirstOrDefaultAsync(c => c.CartId == cart.Id && c.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                _context.ShoppingCartDetails.Update(cartItem);
            }
            else
            {
                cartItem = new ShoppingCartDetail
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = (decimal)await _context.Products
                        .Where(p => p.Id == productId)
                        .Select(p => p.UnitPrice)
                        .FirstOrDefaultAsync()
                        
                };
                _context.ShoppingCartDetails.Add(cartItem);
            }

            await _context.SaveChangesAsync();
        }
        public async Task UpdateCartItemQuantity(int cartId, int productId, int newQuantity)
        {
            var cartItem = _context.ShoppingCartDetails
                .FirstOrDefault(c => c.CartId == cartId && c.ProductId == productId);

            if(cartItem != null)
            {
                cartItem.Quantity = newQuantity;
                _context.Entry(cartItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromCart(int cartId, int productId)
        {

            var cartItem = await _context.ShoppingCartDetails
                .FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductId == productId);

            if (cartItem != null)
            {
                _context.ShoppingCartDetails.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCart(int cartId)
        {
            var cartItems = _context.ShoppingCartDetails
                .Where(c => c.CartId == cartId);

            if(await cartItems.AnyAsync())
            {
                _context.ShoppingCartDetails.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
               
        }

       public async Task <List<ShoppingCartDetail>> ViewCartAsync(int cartId)
        {
                return await _context.ShoppingCartDetails
                .Where(c => c.CartId == cartId)
                .Include(c =>c.Product).ToListAsync();
        }

        public async Task<bool> PlaceOrderAsync(int customerId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.ShoppingCartDetails)
                .FirstOrDefaultAsync(c => c.UserId == customerId);

            if (cart == null)
            {
                throw new Exception("Cart not found");
            }

            decimal totalPrice = cart.ShoppingCartDetails.Sum(c => c.Price * c.Quantity); // FIXED!

           
            var order = new Order
            {
                UserId = customerId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalPrice 
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderItems = cart.ShoppingCartDetails.Select(cartItem => new OrderItem
            {
                OrderId = order.Id,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.Price
            }).ToList();

            _context.OrderItems.AddRange(orderItems);
            await _context.SaveChangesAsync();

            _context.ShoppingCartDetails.RemoveRange(cart.ShoppingCartDetails);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<int> GetCartIdForCustomerAsync(int customerId)
        {
           int cartId = await _context.ShoppingCarts
                .Where(c => c.UserId == customerId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            return cartId;
        }

       




    }
}
