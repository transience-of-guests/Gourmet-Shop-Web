using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GourmetShop.DataAccess.Models;

namespace GourmetShop.DataAccess.Repositories
{
    public class SupplierRepository : GourmetShopRepository, ISupplierRepository_CRUD
    {
        // Call the stored procedures here
        public SupplierRepository(string connectionString) : base(connectionString)
        {
        }

        public void Add(Supplier entity)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("AddSupplier", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@CompanyName", entity.CompanyName));
                    command.Parameters.Add(new SqlParameter("@ContactName", entity.ContactName));
                    command.Parameters.Add(new SqlParameter("@ContactTitle", entity.ContactTitle));
                    command.Parameters.Add(new SqlParameter("@City", entity.City));
                    command.Parameters.Add(new SqlParameter("@Country", entity.Country));
                    command.Parameters.Add(new SqlParameter("@Phone", entity.Phone));
                    command.Parameters.Add(new SqlParameter("@Fax", entity.Fax));

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
                using (var command = new SqlCommand("DeleteSupplier", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@SupplierId", id));

                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<Supplier> GetAll()
        {
            List<Supplier> suppliers = new List<Supplier>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("GetSuppliers", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Supplier supplier = new Supplier();

                            supplier.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            supplier.CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName"));
                            supplier.ContactName = reader.IsDBNull(reader.GetOrdinal("ContactName")) ? null : reader.GetString(reader.GetOrdinal("ContactName"));
                            supplier.ContactTitle =reader.IsDBNull(reader.GetOrdinal("ContactTitle")) ? null :  reader.GetString(reader.GetOrdinal("ContactTitle"));
                            supplier.City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City"));
                            supplier.Country = reader.IsDBNull(reader.GetOrdinal("Country")) ? null : reader.GetString(reader.GetOrdinal("Country"));
                            supplier.Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone"));
                            supplier.Fax = reader.IsDBNull(reader.GetOrdinal("Fax")) ? null : reader.GetString(reader.GetOrdinal("Fax"));

                            // https://stackoverflow.com/questions/8370927/how-do-i-loop-through-rows-with-a-data-reader-in-c
                            suppliers.Add(supplier);
                        }
                    }
                }

                return suppliers;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // TO-DO: 
        public Supplier GetById(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("GetSupplierById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@SupplierId", id));

                    Supplier supplier = new Supplier();

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            supplier.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            supplier.CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName"));
                            supplier.ContactName = reader.IsDBNull(reader.GetOrdinal("ContactName")) ? null : reader.GetString(reader.GetOrdinal("ContactName"));
                            supplier.ContactTitle = reader.IsDBNull(reader.GetOrdinal("ContactTitle")) ? null : reader.GetString(reader.GetOrdinal("ContactTitle"));
                            supplier.City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City"));
                            supplier.Country = reader.IsDBNull(reader.GetOrdinal("Country")) ? null : reader.GetString(reader.GetOrdinal("Country"));
                            supplier.Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone"));
                            supplier.Fax = reader.IsDBNull(reader.GetOrdinal("Fax")) ? null : reader.GetString(reader.GetOrdinal("Fax"));
                        }

                        return supplier;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Update(Supplier entity)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("UpdateSupplier", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@SupplierId", entity.Id));
                    command.Parameters.Add(new SqlParameter("@CompanyName", entity.CompanyName));
                    command.Parameters.Add(new SqlParameter("@ContactName", entity.ContactName));
                    command.Parameters.Add(new SqlParameter("@ContactTitle", entity.ContactTitle));
                    command.Parameters.Add(new SqlParameter("@City", entity.City));
                    command.Parameters.Add(new SqlParameter("@Country", entity.Country));
                    command.Parameters.Add(new SqlParameter("@Phone", entity.Phone));
                    command.Parameters.Add(new SqlParameter("@Fax", entity.Fax));

                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
