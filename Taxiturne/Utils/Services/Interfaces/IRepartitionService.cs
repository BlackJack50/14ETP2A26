using Taxiturne.Model;

namespace Taxiturne.Services
{
    public interface IRepartitionService
    {
        IReadOnlyList<string> ValiderConformite(
            Course course,
            Vehicule vehicule);

        IReadOnlyList<string> AttribuerCourse(
            Course course,
            Chauffeur chauffeur);
    }
}
