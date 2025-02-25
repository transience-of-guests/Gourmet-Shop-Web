using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GourmetShop.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using GourmetShop.DataAccess.Data;

namespace GourmetShop.DataAccess.Repositories
{
    public class UserInfoRepository: GourmetShopRepository, IUserInfoRepository
    {
        public UserInfoRepository(string connectionString) : base(connectionString)
        {
        }

        public UserInfoRepository(GourmetShopDbContext context) : base(context)
        {
        }

        public async Task<bool> UserExistsAsync(string firstName, string lastName)
        {
            return await _context.Users
       .AnyAsync(u => u.FirstName == firstName && u.LastName == lastName);
        }

        public Task<IEnumerable<UserInfo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserInfo>> GetAllByRoleAsync(List<string?> authIds)
        {
            return await _context.Users
                .Where(item => authIds.Contains(item.AuthenticationId))
                .ToListAsync();
        }

        public async Task<UserInfo> GetAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddAsync(UserInfo item)
        {
            _context.Users.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserInfo item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Users.FindAsync(id);
            if (item != null)
            {
                _context.Users.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(int TotalUnitsSold, decimal TotalSalesAmount)> GetProductSalesAsync(int productId)
        {
            var result = await _context.OrderItems
            .Where(od => od.ProductId == productId)
            .GroupBy(od => od.ProductId)
            .Select(g => new
            {
                TotalUnitsSold = g.Sum(od => od.Quantity),
                TotalSalesAmount = g.Sum(od => od.Quantity * od.UnitPrice)
            })
            .FirstOrDefaultAsync();

            return result != null ? (result.TotalUnitsSold, result.TotalSalesAmount) : (0, 0);
        }
    }
}
