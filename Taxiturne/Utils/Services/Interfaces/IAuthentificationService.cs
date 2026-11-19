using Taxiturne.Model;

namespace Taxiturne.Services
{
    public interface IAuthentificationService
    {
        User? Authentifier(
            string? username,
            string? password);
    }
}
