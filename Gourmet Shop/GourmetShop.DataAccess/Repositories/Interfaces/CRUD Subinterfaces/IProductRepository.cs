using GourmetShop.DataAccess.Models;
using GourmetShop.DataAccess.Repositories.Interfaces.CRUD_Subinterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Repositories
{
    public interface IProductRepository : IGourmetShopRepository<Product>, ICrudOperations<Product>
    {
        public Task<IEnumerable<Supplier>> GetSelectableSuppliers();

        public Task<IEnumerable<Subcategory>> GetSelectableSubcategories();

        //CHECK ME
       public Task<IEnumerable<Product>> GetAvailableProductsForCust();
     



    }
}
