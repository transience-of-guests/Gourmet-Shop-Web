using GourmetShop.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Repositories.Interfaces.CRUD_Subinterfaces
{
    public interface IShoppingCartRepository : IGourmetShopRepository<ShoppingCart>
    {
        void AddToCart(int customerId, int productId, int quantity);
        void UpdateCartItemQuantity(int cartId, int productId, int newQuantity);
        void RemoveFromCart(int cartId, int productId);
        void ClearCart(int cartId);
        
        void PlaceOrder(int customerId);

        DataTable ViewCart(int cartId);

    }
}
