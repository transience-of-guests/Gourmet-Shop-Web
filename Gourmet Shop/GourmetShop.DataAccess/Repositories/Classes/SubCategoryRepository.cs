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
        public async Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync()
        {
            return await _context.Subcategories.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsBySubcategoryAsync(int subcategoryId)
        {
            return await _context.Products
                .Where(p => p.SubcategoryId == subcategoryId && !p.IsDiscontinued)
                .ToListAsync();
        }
    }
}
