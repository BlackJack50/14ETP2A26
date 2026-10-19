using Microsoft.EntityFrameworkCore;
using Taxiturne.Model.Interfaces;

namespace Taxiturne.Model.DAL
{
    public class ChauffeurDAL : IChauffeurDAL
    {
        private readonly ApplicationDbContext _contexte;

        public ChauffeurDAL(ApplicationDbContext contexte)
        {
            _contexte = contexte;
        }

        public IReadOnlyList<Chauffeur> ObtenirChauffeursDisponibles()
        {
            return _contexte.Chauffeurs
                .Include(chauffeur => chauffeur.Vehicule)
                .ThenInclude(vehicule => vehicule.Categorie)
                .Where(chauffeur => chauffeur.EstDisponible)
                .OrderBy(chauffeur => chauffeur.Nom)
                .ToList();
        }
    }
}
