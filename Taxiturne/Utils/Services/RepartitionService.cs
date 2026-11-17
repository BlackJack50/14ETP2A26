using Microsoft.EntityFrameworkCore;
using Taxiturne.Model;

namespace Taxiturne.Services
{
    public class RepartitionService : IRepartitionService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public RepartitionService(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IReadOnlyList<string> ValiderConformite(
            Course course,
            Vehicule vehicule)
        {
            var erreurs = new List<string>();

            if (course.SiegeEnfantRequis &&
                !vehicule.PossedeSiegeEnfant)
            {
                erreurs.Add(
                    "Le véhicule ne possède pas de siège pour enfants.");
            }

            if (course.NombrePassagers >
                vehicule.Categorie.PassagersMax)
            {
                erreurs.Add(
                    "Le nombre de passagers dépasse la capacité du véhicule.");
            }

            if (course.AnimalABord &&
                !vehicule.AccepteAnimaux)
            {
                erreurs.Add(
                    "Le véhicule n'accepte pas les animaux de compagnie.");
            }

            if (course.NombreBagages >
                vehicule.Categorie.BagagesMax)
            {
                erreurs.Add(
                    "Le nombre de bagages dépasse la capacité du véhicule.");
            }

            if (course.CategorieDemandeeId != vehicule.CategorieId)
            {
                erreurs.Add(
                    "Le véhicule n'est pas de la bonne catégorie.");
            }

            if (course.FauteuilRoulant &&
                !vehicule.Categorie.PossedeRampe)
            {
                erreurs.Add(
                    "Le véhicule n'est pas adapté aux fauteuils roulants.");
            }

            return erreurs;
        }

        public IReadOnlyList<string> AttribuerCourse(
            Course courseSelectionnee,
            Chauffeur chauffeurSelectionne)
        {
            using var context = _contextFactory.CreateDbContext();

            var course = context.Courses
                .Include(c => c.CategorieDemandee)
                .FirstOrDefault(c => c.Id == courseSelectionnee.Id);

            var chauffeur = context.Chauffeurs
                .Include(c => c.Vehicule)
                .ThenInclude(v => v.Categorie)
                .FirstOrDefault(c => c.Id == chauffeurSelectionne.Id);

            var erreurs = new List<string>();

            if (course == null)
            {
                erreurs.Add("La course n'existe plus.");
            }

            if (chauffeur == null)
            {
                erreurs.Add("Le chauffeur n'existe plus.");
            }

            if (erreurs.Count > 0)
            {
                return erreurs;
            }

            // Les deux entités existent à ce stade.
            if (course!.Statut != StatutCourse.Demandee)
            {
                erreurs.Add("La course est déjà affectée.");
            }

            if (!chauffeur!.EstDisponible)
            {
                erreurs.Add("Le chauffeur n'est pas disponible.");
            }

            erreurs.AddRange(
                ValiderConformite(course, chauffeur.Vehicule));

            if (erreurs.Count > 0)
            {
                return erreurs;
            }

            course.ChauffeurId = chauffeur.Id;
            course.Statut = StatutCourse.Affectee;
            chauffeur.EstDisponible = false;

            context.SaveChanges();

            return erreurs;
        }
    }
}