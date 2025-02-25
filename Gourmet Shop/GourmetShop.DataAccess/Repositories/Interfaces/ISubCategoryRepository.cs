using GourmetShop.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Repositories.Interfaces
{
    public interface ISubCategoryRepository : IGourmetShopRepository<Subcategory>
    {

        Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync();
        Task<IEnumerable<Product>> GetProductsBySubcategoryAsync(int subcategoryId);

    }
}
