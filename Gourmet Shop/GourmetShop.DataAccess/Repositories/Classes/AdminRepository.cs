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

    //CHECKME
    public class AdminRepository : GourmetShopRepository, IAdmin
    {

        public AdminRepository(string connectionString) : base(connectionString)
        {
        }

        public Admin GetByUserId(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("GetAdminByUserId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Admin admin = new Admin();

                        if (reader.Read())
                        {
                            admin.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            admin.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
                            admin.Email = reader.GetString(reader.GetOrdinal("Email"));
                        }

                        return admin;
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

        public (int TotalUnitsSold, decimal TotalSalesAmount) GetProductSales(int productId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("GetProductSales", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductID", productId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int totalUnitsSold = reader.GetInt32(reader.GetOrdinal("TotalUnitsSold"));
                            decimal totalSalesAmount = reader.GetDecimal(reader.GetOrdinal("TotalSalesAmount"));
                            return (totalUnitsSold, totalSalesAmount);
                        }
                    }
                }
            }
            catch (SqlException ex) // Catches SQL-specific errors
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex) // Catches any other unexpected errors
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return (0, 0); // Return default values in case of failure
        }

    }
}
