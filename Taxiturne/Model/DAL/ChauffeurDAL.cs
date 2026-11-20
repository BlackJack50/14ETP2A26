using Microsoft.EntityFrameworkCore;
using Taxiturne.Model.Interfaces;

namespace Taxiturne.Model.DAL
{
    public class ChauffeurDAL : IChauffeurDAL
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public ChauffeurDAL(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IReadOnlyList<Chauffeur> ObtenirChauffeursDisponibles()
        {
            using var context = _contextFactory.CreateDbContext();

            return context.Chauffeurs
                .AsNoTracking()
                .Include(chauffeur => chauffeur.Vehicule)
                .ThenInclude(vehicule => vehicule.Categorie)
                .Where(chauffeur => chauffeur.EstDisponible)
                .OrderBy(chauffeur => chauffeur.Nom)
                .ToList();
        }
    }
}
