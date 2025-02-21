using GourmetShop.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Repositories
{
    //CHECKME
    public interface IAdmin : IGourmetShopRepository<Admin>, IUserRepository<Admin>
    {
        Task<(int TotalUnitsSold, decimal TotalSalesAmount)> GetProductSalesAync(int productId);
    }
}
