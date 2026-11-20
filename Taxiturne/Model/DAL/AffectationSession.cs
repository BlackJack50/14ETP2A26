
using Microsoft.EntityFrameworkCore;
using Taxiturne.Model.Interfaces;

namespace Taxiturne.Model.DAL
{
    public class AffectationSessionFactory : IAffectationSessionFactory
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public AffectationSessionFactory(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IAffectationSession Ouvrir()
        {
            return new AffectationSession(
                _contextFactory.CreateDbContext());
        }
    }

    public class AffectationSession : IAffectationSession
    {
        private readonly ApplicationDbContext _context;

        public AffectationSession(ApplicationDbContext context)
        {
            _context = context;
        }

        public Course? ObtenirCourse(int id)
        {
            return _context.Courses
                .Include(c => c.CategorieDemandee)
                .FirstOrDefault(c => c.Id == id);
        }

        public Chauffeur? ObtenirChauffeur(int id)
        {
            return _context.Chauffeurs
                .Include(c => c.Vehicule)
                .ThenInclude(v => v.Categorie)
                .FirstOrDefault(c => c.Id == id);
        }

        public void Sauvegarder()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
