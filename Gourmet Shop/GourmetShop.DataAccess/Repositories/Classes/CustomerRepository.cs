using GourmetShop.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Repositories
{
    public class CustomerRepository : GourmetShopRepository, ICustomerRepository
    {
        public CustomerRepository(string connectionString) : base(connectionString)
        {
        }

        // CHECKME
        public Customer GetByUserId(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("GetCustomerByUserId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Customer customer = new Customer();

                        if (reader.Read())
                        {
                            customer.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            customer.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
                        }

                        return customer;
                    }
                }
            }
            catch (SqlException ex) // Catches SQL-specific errors
            {
                throw; // Rethrow the exception to the calling code
            }
            catch (Exception ex) // Catches any other unexpected errors
            {
                throw; // Rethrow the exception to the calling code
            }
        }
    }
}
