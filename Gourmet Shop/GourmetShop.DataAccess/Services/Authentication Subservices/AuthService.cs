using GourmetShop.DataAccess.Models;
using GourmetShop.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Services
{
    public class AuthService
    {
        // FIXME: Pass in a connection string
        AuthRepository _authRepository;

        public AuthService(string connectionString)
        {
            _authRepository = new AuthRepository(connectionString);
        }

        // CHECKME: Do we need a separate email parameter? Because in the form you have username/email as a field, how does the email validator work?
        public int Register(User user, Authentication authentication)
        {
            authentication.Password = PasswordHasher.HashPassword(authentication.Password);

            return _authRepository.Register(user, authentication);
        }

        public int Login(string username, string password)
        {
            try
            {
                if (!PasswordHasher.VerifyPassword(password, _authRepository.GetPassword(username)))
                    throw new Exception("Invalid password");

                return _authRepository.GetUserId(username);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
