using GourmetShop.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Repositories
{
    internal interface IProductRepository_CRUD : IGourmetShopRepository<Product>
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Add(Product entity);
        void Update(Product entity);
        void Delete(int id);
    }
}
