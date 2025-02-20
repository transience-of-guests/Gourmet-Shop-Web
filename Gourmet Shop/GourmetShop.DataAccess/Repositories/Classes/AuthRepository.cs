using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GourmetShop.DataAccess.Models;
using System.Security.Policy;

namespace GourmetShop.DataAccess.Repositories
{
    public class AuthRepository : GourmetShopRepository, IAuthRepository
    {
        private readonly UserRepository userRepo;
        public AuthRepository(string connectionString) : base(connectionString)
        {
            userRepo = new UserRepository(connectionString);
        }

        public int Register(User user, Authentication authentication)
        {
            try
            {
                
                using (var conn = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("Register", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@RoleId", user.RoleId));
                    command.Parameters.Add(new SqlParameter("@FirstName", user.FirstName));
                    command.Parameters.Add(new SqlParameter("@LastName", user.LastName));
                    command.Parameters.Add(new SqlParameter("@City", user.City));
                    command.Parameters.Add(new SqlParameter("@Country", user.Country));
                    command.Parameters.Add(new SqlParameter("@Phone", user.Phone));

                    command.Parameters.Add(new SqlParameter("@Username", authentication.Username));
                    command.Parameters.Add(new SqlParameter("@Password", authentication.Password));
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int userId = 0;

                        while (reader.Read())
                        {
                            userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                        }

                        return userId;
                    }
                }
            }
            // Because we have RAISERROR in the stored procedure, we need to catch the SqlException
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetPassword(string username)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("GetPassword", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@Username", username));

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        string password = "";

                        while (reader.Read())
                        {
                            password = reader["Password"].ToString();
                        }

                        return password;
                    }
                }
            }
            // Because we have RAISERROR in the stored procedure, we need to catch the SqlException
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int GetUserId(string username)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("GetUserIdLogin", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@Username", username));

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int userId = 0;

                        while (reader.Read())
                        {
                            userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                        }

                        return userId;
                    }
                }
            }
            // Because we have RAISERROR in the stored procedure, we need to catch the SqlException
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        























    }








}
