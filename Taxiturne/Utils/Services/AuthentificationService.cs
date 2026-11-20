
using Taxiturne.Model;
using Taxiturne.Model.Interfaces;

namespace Taxiturne.Services
{
    public class AuthentificationService : IAuthentificationService
    {
        private readonly IUserDAL _userDAL;

        public AuthentificationService(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }

        public User? Authentifier(
            string? username,
            string? password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            if (username.Length < 2)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            return _userDAL.FindByUsernameAndPassword(
                username,
                password);
        }
    }
}
