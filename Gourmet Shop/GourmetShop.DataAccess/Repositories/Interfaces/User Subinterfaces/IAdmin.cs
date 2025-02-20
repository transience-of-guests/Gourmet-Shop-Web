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
        (int TotalUnitsSold, decimal TotalSalesAmount) GetProductSales(int productId);
    }
}
