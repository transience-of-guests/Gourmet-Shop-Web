using GourmetShop.DataAccess.Data;
using GourmetShop.DataAccess.Models;
using GourmetShop.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly GourmetShopDbContext _context;

        public SubCategoryRepository(GourmetShopDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Subcategory item)
        {
            _context.Subcategories.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var subcategory = await _context.Subcategories.FindAsync(id);
            if (subcategory != null)
            {
                _context.Subcategories.Remove(subcategory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Subcategory>> GetAllAsync()
        {
            return await _context.Subcategories.ToListAsync();
        }

        public async Task<Subcategory> GetAsync(int id)
        {
            return await _context.Subcategories.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProductsBySubcategoryAsync(int subcategoryId)
        {
            return await _context.Products
                .Where(p => p.SubcategoryId == subcategoryId && !p.IsDiscontinued)
                .ToListAsync();
        }

        public async Task UpdateAsync(Subcategory item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
