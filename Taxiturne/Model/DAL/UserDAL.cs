using Microsoft.EntityFrameworkCore;
using Taxiturne.Model.Interfaces;

namespace Taxiturne.Model.DAL
{
    public class UserDAL : IUserDAL
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public UserDAL(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public User? FindByUsernameAndPassword(
            string username,
            string password)
        {
            using var context = _contextFactory.CreateDbContext();

            return context.Users
                .AsNoTracking()
                .FirstOrDefault(user =>
                    user.Username == username &&
                    user.Password == password);
        }
    }
}
