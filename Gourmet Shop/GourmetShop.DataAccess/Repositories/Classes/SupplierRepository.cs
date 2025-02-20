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
    public class SupplierRepository : GourmetShopRepository, ISupplierRepository
    {
        // Call the stored procedures here
        public SupplierRepository(string connectionString) : base(connectionString)
        {
        }

        public SupplierRepository(GourmetShopDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier> GetAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            _context.Entry(supplier).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
