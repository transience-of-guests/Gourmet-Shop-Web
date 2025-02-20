using GourmetShop.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Repositories
{
    internal interface ISupplierRepository_CRUD: IGourmetShopRepository<Supplier>
    {
        IEnumerable<Supplier> GetAll();
        Supplier GetById(int id);
        void Add(Supplier entity);
        void Update(Supplier entity);
        void Delete(int id);
    }
}
