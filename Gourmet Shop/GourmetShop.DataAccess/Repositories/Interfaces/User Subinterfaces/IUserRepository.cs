using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GourmetShop.DataAccess.Models;

namespace GourmetShop.DataAccess.Repositories
{
    public interface IUserRepository<T>
    {
        Task<T> GetByUserIdAsync(int userId);
    }
}
