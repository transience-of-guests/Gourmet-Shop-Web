using GourmetShop.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GourmetShop.DataAccess.Data;

namespace GourmetShop.DataAccess.Repositories
{
    public class CustomerRepository : GourmetShopRepository, ICustomerRepository
    {
        public CustomerRepository(string connectionString) : base(connectionString)
        {
        }

        public CustomerRepository(GourmetShopDbContext context) : base(context)
        {
        }
        public async Task<Customer> GetByUserIdAsync(int userId)
        {

            return await _context.Customers
            .Where(c => c.UserId == userId)
            .FirstOrDefaultAsync();

        }
      
    }
}
