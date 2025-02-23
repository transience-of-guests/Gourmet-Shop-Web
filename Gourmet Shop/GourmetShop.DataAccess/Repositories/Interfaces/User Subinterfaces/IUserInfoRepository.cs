using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GourmetShop.DataAccess.Models;

namespace GourmetShop.DataAccess.Repositories
{
    public interface IUserInfoRepository : IGourmetShopRepository<UserInfo>, ICrudOperations<UserInfo>
    {
        public Task<(int TotalUnitsSold, decimal TotalSalesAmount)> GetProductSalesAsync(int productId);

        public Task<IEnumerable<UserInfo>> GetAllByRoleAsync(List<string?> authIds);
    }
}
