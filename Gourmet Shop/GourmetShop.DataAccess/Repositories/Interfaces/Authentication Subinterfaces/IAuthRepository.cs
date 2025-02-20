using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Repositories
{
    public interface IAuthRepository
    {
        int GetUserId(string username);

        string GetPassword(string username);
    }
}
