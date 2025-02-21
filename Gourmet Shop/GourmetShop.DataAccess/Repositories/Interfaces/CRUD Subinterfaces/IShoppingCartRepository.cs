using GourmetShop.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GourmetShop.DataAccess.Repositories.Interfaces.CRUD_Subinterfaces
{
    public interface IShoppingCartRepository : IGourmetShopRepository<ShoppingCart>
    {
        Task AddToCartAsync(int customerId, int productId, int quantity);
        Task UpdateCartItemQuantity(int cartId, int productId, int newQuantity);
        Task RemoveFromCart(int cartId, int productId);
        Task ClearCart(int cartId);
        Task<List<ShoppingCartDetail>> ViewCartASync(int cartId);
        Task<bool> PlaceOrderAync(int customerId);

        Task<int> GetCartIdForCustomerAsync(int customerId);
          

    }
}
