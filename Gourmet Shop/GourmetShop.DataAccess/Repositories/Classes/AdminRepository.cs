using GourmetShop.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GourmetShop.DataAccess.Repositories
{

    //CHECKME
    public class AdminRepository : GourmetShopRepository, IAdmin
    {

        public AdminRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<Admin> GetByUserIdAsync(int userId)
        {
            
                return await _context.Admins
                .Where(a => a.UserId == userId)
                .FirstOrDefaultAsync();
           
        }

        public async Task<(int TotalUnitsSold, decimal TotalSalesAmount)> GetProductSalesAync(int productId)
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
