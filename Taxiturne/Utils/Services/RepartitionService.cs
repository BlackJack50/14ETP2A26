
using Taxiturne.Model;
using Taxiturne.Model.Interfaces;

namespace Taxiturne.Services
{
    public class RepartitionService : IRepartitionService
    {
        private readonly IAffectationSessionFactory _sessionFactory;

        public RepartitionService(
            IAffectationSessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
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

            if (course.CategorieDemandeeId !=
                vehicule.CategorieId)
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
            using var session = _sessionFactory.Ouvrir();

            var course = session.ObtenirCourse(
                courseSelectionnee.Id);

            var chauffeur = session.ObtenirChauffeur(
                chauffeurSelectionne.Id);

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

            session.Sauvegarder();

            return erreurs;
        }
    }
}
