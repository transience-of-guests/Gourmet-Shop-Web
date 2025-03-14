﻿using GourmetShop.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Repositories
{
    public interface ISubCategoryRepository : IGourmetShopRepository<Subcategory>, ICrudOperations<Subcategory>
    {
        Task<IEnumerable<Product>> GetProductsBySubcategoryAsync(int subcategoryId);
    }
}
