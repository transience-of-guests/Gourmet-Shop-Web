using System;
using System.Collections.Generic;

namespace GourmetShop.DataAccess.Repositories
{
    // NOTE:
    // The justification is that the ProductRepository and SupplierRepository are subclasses and uses one connection string
    public class GourmetShopRepository
    {
        protected readonly string _connectionString;

        public GourmetShopRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
