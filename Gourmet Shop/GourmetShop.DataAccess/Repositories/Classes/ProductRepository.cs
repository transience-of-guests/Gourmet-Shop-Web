using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GourmetShop.DataAccess.Models;
using System.Data.Common;
using System.Xml.Linq;
using GourmetShop.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace GourmetShop.DataAccess.Repositories
{
    public class ProductRepository : GourmetShopRepository, IProductRepository
    {
        public ProductRepository(string connectionString) : base(connectionString)
        {
        }

        public ProductRepository(GourmetShopDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Subcategory) // Make sure to eagerly load the subcategories and suppliers
                .Include(p => p.Supplier)
                .ToListAsync();
        }

        public async Task<Product> GetAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        //CHECKME ADDED

        public List<Product> GetAvailableProductsForAdmin()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("GetAvailableProducts", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"].ToString(),
                            SupplierId =Convert.ToInt32(reader["SupplierID"]),
                            UnitPrice = Convert.ToDecimal(reader["Price"])
                        });
                    }
                }
            }
            return products;
        }

        public List<Product> GetAvailableProductsForCust()
        {
            List <Product> products = new List<Product>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("GetAvailableProducts", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"].ToString(),
                            SupplierId=Convert.ToInt32(reader["SupplierID"]),
                            UnitPrice = Convert.ToDecimal(reader["Price"])
                        });
                    }
                }
            }

            return products;
        }

        public List<Product>GetSupplierByName(string supplierName)
        {
            List<Product> products = new List<Product>();

            using(SqlConnection conn = new SqlConnection(_connectionString))
            using(SqlCommand cmd = new SqlCommand("GetProductsBySupplierName", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierName", supplierName);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            UnitPrice = Convert.ToDecimal(reader["Price"])
                        });
                    }
                }
            }
            return products;
        }

    }
}
