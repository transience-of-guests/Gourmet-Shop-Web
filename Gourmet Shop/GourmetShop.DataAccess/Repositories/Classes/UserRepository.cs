using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GourmetShop.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace GourmetShop.DataAccess.Repositories
{
    public class UserRepository: GourmetShopRepository, IUserRepository<User>
    {
        public UserRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<bool> UserExistsASync(string firstName, string lastName, string phone)
        {
            var result = await _context.Users
                .FromSqlRaw("EXEC CheckUserExists @FirstName, @LastName, @Phone",
                    new SqlParameter("@FirstName", firstName),
                    new SqlParameter("@LastName", lastName),
                    new SqlParameter("@Phone", phone))
                .FirstOrDefaultAsync();

            return true;
        }

        

        public void GetAllCustomers()
        {
        }

        public async Task<User> GetByUserIdAsync(int userId)
        {
            var user = await _context.Users
                .FromSqlRaw("EXEC GetUser @UserId",
                new SqlParameter("@UserId", userId)).FirstOrDefaultAsync();

            return user;
        }
    
    }
}
