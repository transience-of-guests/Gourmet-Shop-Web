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
    public class UserRepository: GourmetShopRepository, IUserRepository<User>
    {
        public UserRepository(string connectionString) : base(connectionString)
        {
        }

        public bool UserExists(string firstName, string lastName, string phone)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("CheckUserExists", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Phone", phone);

                        int count = (int)cmd.ExecuteScalar();

                        return count > 0;
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

        public void GetAllCustomers()
        {
        }

        public User GetByUserId(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("GetUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        User user = new User();

                        if (reader.Read())
                        {
                            user.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            user.RoleId = reader.GetInt32(reader.GetOrdinal("RoleId"));
                            user.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            user.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                            user.City = reader.GetString(reader.GetOrdinal("City"));
                            user.Country = reader.GetString(reader.GetOrdinal("Country"));
                            user.Phone = reader.GetString(reader.GetOrdinal("Phone"));
                        }

                        return user;
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
