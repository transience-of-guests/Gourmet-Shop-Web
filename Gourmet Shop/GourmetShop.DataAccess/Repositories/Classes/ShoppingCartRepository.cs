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

namespace GourmetShop.DataAccess.Repositories
{
    //CHECKME
    public class ShoppingCartRepository : GourmetShopRepository, IShoppingCartRepository
    {
      
        public ShoppingCartRepository(string connectionString) : base(connectionString)
        {
        }
        public async Task AddToCartAsync(int customerId, int productId, int quantity)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AddToCart @CustomerId, @ProductId, @Quantity",
                new SqlParameter("@CustomerId", customerId),
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@Quantity", quantity)
                );
        }

        public async Task UpdateCartItemQuantity(int cartId, int productId, int newQuantity)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC UpdateCartItemQuanity @CartId, @ProductId, @NewQuantity",
                new SqlParameter("@CartId", cartId),
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@NewQuantity", newQuantity)

                );
        }

        public async Task RemoveFromCart(int cartId, int productId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC RemoveFromCart @CartId, @ProductId",
                new SqlParameter("@CartId", cartId),
                new SqlParameter("@ProductId", productId)
                );
        }

        public async Task ClearCart(int cartId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC ClearCart @CartId",
                new SqlParameter ("@CartId", cartId)
                );
        }

       public async Task <List<ShoppingCartDetail>> ViewCartASync(int cartId)
        {
            var cartItems = await _context.ShoppingCartDetails
             .FromSqlRaw("EXEC ViewCart @CartID", new SqlParameter("@CartID", cartId))
             .ToListAsync();

            return cartItems;
        }

        public async Task<bool> PlaceOrderAync(int customerId)
        {
            var result = await _context.Orders
                 .FromSqlRaw("EXEC PlaceOrder @CustomerId", new SqlParameter("@CustomerId", customerId))
            .ToListAsync();

            if (result == null || result.Count == 0)
            {
                throw new Exception("Order placement failed. No order was created.");
                
            }

            return true;
        }


        public async Task<int> GetCartIdForCustomerAsync(int customerId)
        {
            var cartId = await _context.ShoppingCarts
                .Where(c => c.Id == customerId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            return cartId;
        }

       




    }
}
