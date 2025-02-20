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

namespace GourmetShop.DataAccess.Repositories
{
    public class ProductRepository : GourmetShopRepository, IProductRepository_CRUD
    {
        public ProductRepository(string connectionString) : base(connectionString)
        {
        }

        public void Add(Product entity)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("AddProduct", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@ProductName", entity.ProductName));
                    command.Parameters.Add(new SqlParameter("@SupplierId", entity.SupplierId));
                    command.Parameters.Add(new SqlParameter("@UnitPrice", entity.UnitPrice));
                    command.Parameters.Add(new SqlParameter("@Package", entity.Package));
                    command.Parameters.Add(new SqlParameter("@IsDiscontinued", (entity.IsDiscontinued) ? 1 : 0));

                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // TO-BE COMPLETED
        public void Delete(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("DeleteProduct", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@ProductId", id));

                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<Product> GetAll()
        {
            List<Product> products = new List<Product>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("GetProducts", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product();

                            product.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            product.ProductName = reader.GetString(reader.GetOrdinal("ProductName"));
                            product.SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId"));
                            product.UnitPrice = reader.IsDBNull(reader.GetOrdinal("UnitPrice")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("UnitPrice"));
                            product.Package = reader.IsDBNull(reader.GetOrdinal("Package")) ? null : reader.GetString(reader.GetOrdinal("Package"));
                            product.IsDiscontinued = reader.GetBoolean(reader.GetOrdinal("IsDiscontinued"));

                            // https://stackoverflow.com/questions/8370927/how-do-i-loop-through-rows-with-a-data-reader-in-c
                            products.Add(product);
                        }
                    }
                }

                return products;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // TO-DO: 
        public Product GetById(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("GetProductById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@ProductId", id));

                    Product product = new Product();

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            product.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            product.ProductName = reader.GetString(reader.GetOrdinal("ProductName"));
                            product.SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId"));
                            product.UnitPrice = reader.IsDBNull(reader.GetOrdinal("UnitPrice")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("UnitPrice"));
                            product.Package = reader.IsDBNull(reader.GetOrdinal("Package")) ? null : reader.GetString(reader.GetOrdinal("Package"));
                            product.IsDiscontinued = reader.GetBoolean(reader.GetOrdinal("IsDiscontinued"));
                        }

                        return product;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Update(Product entity)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("UpdateProduct", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@ProductId", entity.Id));
                    command.Parameters.Add(new SqlParameter("@ProductName", entity.ProductName));
                    command.Parameters.Add(new SqlParameter("@SupplierId", entity.SupplierId));
                    command.Parameters.Add(new SqlParameter("@UnitPrice", entity.UnitPrice));
                    command.Parameters.Add(new SqlParameter("@Package", entity.Package));
                    command.Parameters.Add(new SqlParameter("@IsDiscontinued", (entity.IsDiscontinued) ? 1 : 0));

                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
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
