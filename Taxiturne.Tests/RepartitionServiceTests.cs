
using Taxiturne.Model;
using Taxiturne.Model.Interfaces;
using Taxiturne.Services;

namespace Taxiturne.Tests
{
    public class RepartitionServiceTests
    {
        private readonly Mock<IAffectationSessionFactory> _factory;
        private readonly Mock<IAffectationSession> _session;
        private readonly RepartitionService _service;

        public RepartitionServiceTests()
        {
            _factory = new Mock<IAffectationSessionFactory>(
                MockBehavior.Strict);

            _session = new Mock<IAffectationSession>(
                MockBehavior.Strict);

            _factory.Setup(f => f.Ouvrir())
                .Returns(_session.Object);

            _session.Setup(s => s.Dispose());

            _service = new RepartitionService(_factory.Object);
        }

        private static (Course course, Chauffeur chauffeur)
            CreerDonneesValides()
        {
            var categorie = new CategorieVehicule
            {
                Id = 1,
                Nom = "Standard",
                PassagersMax = 4,
                BagagesMax = 2,
                PossedeRampe = true
            };

            var vehicule = new Vehicule
            {
                Id = 1,
                CategorieId = categorie.Id,
                Categorie = categorie,
                PossedeSiegeEnfant = true,
                AccepteAnimaux = true
            };

            var chauffeur = new Chauffeur
            {
                Id = 10,
                VehiculeId = vehicule.Id,
                Vehicule = vehicule,
                EstDisponible = true
            };

            var course = new Course
            {
                Id = 20,
                NombrePassagers = 2,
                NombreBagages = 1,
                CategorieDemandeeId = categorie.Id,
                AnimalABord = false,
                SiegeEnfantRequis = false,
                FauteuilRoulant = false,
                Statut = StatutCourse.Demandee
            };

            return (course, chauffeur);
        }

        private void ConfigurerLecture(
            Course course,
            Chauffeur chauffeur)
        {
            _session.Setup(s => s.ObtenirCourse(course.Id))
                .Returns(course);

            _session.Setup(s => s.ObtenirChauffeur(chauffeur.Id))
                .Returns(chauffeur);
        }

        private static void VerifierErreur(
            IReadOnlyList<string> erreurs,
            string message,
            bool erreurAttendue)
        {
            if (erreurAttendue)
            {
                Assert.Contains(message, erreurs);
            }
            else
            {
                Assert.DoesNotContain(message, erreurs);
            }
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(true, false, true)]
        [InlineData(true, true, false)]
        public void SiegeEnfant_ValideSelonBesoin(
            bool requis,
            bool disponible,
            bool erreurAttendue)
        {
            var (course, chauffeur) = CreerDonneesValides();

            course.SiegeEnfantRequis = requis;
            chauffeur.Vehicule.PossedeSiegeEnfant = disponible;

            var erreurs = _service.ValiderConformite(
                course, chauffeur.Vehicule);

            VerifierErreur(
                erreurs,
                "Le véhicule ne possède pas de siège pour enfants.",
                erreurAttendue);
        }

        [Theory]
        [InlineData(3, false)]
        [InlineData(4, false)]
        [InlineData(5, true)]
        public void Passagers_RespecteCapacite(
            int nombrePassagers,
            bool erreurAttendue)
        {
            var (course, chauffeur) = CreerDonneesValides();

            course.NombrePassagers = nombrePassagers;

            var erreurs = _service.ValiderConformite(
                course, chauffeur.Vehicule);

            VerifierErreur(
                erreurs,
                "Le nombre de passagers dépasse la capacité du véhicule.",
                erreurAttendue);
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(true, false, true)]
        [InlineData(true, true, false)]
        public void Animaux_ValideSelonAcceptation(
            bool animalABord,
            bool accepteAnimaux,
            bool erreurAttendue)
        {
            var (course, chauffeur) = CreerDonneesValides();

            course.AnimalABord = animalABord;
            chauffeur.Vehicule.AccepteAnimaux = accepteAnimaux;

            var erreurs = _service.ValiderConformite(
                course, chauffeur.Vehicule);

            VerifierErreur(
                erreurs,
                "Le véhicule n'accepte pas les animaux de compagnie.",
                erreurAttendue);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, true)]
        public void Bagages_RespecteCapacite(
            int nombreBagages,
            bool erreurAttendue)
        {
            var (course, chauffeur) = CreerDonneesValides();

            course.NombreBagages = nombreBagages;

            var erreurs = _service.ValiderConformite(
                course, chauffeur.Vehicule);

            VerifierErreur(
                erreurs,
                "Le nombre de bagages dépasse la capacité du véhicule.",
                erreurAttendue);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        public void Categorie_DoitCorrespondre(
            int categorieDemandee,
            bool erreurAttendue)
        {
            var (course, chauffeur) = CreerDonneesValides();

            course.CategorieDemandeeId = categorieDemandee;

            var erreurs = _service.ValiderConformite(
                course, chauffeur.Vehicule);

            VerifierErreur(
                erreurs,
                "Le véhicule n'est pas de la bonne catégorie.",
                erreurAttendue);
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(true, false, true)]
        [InlineData(true, true, false)]
        public void FauteuilRoulant_ValideSelonRampe(
            bool fauteuilRequis,
            bool rampeDisponible,
            bool erreurAttendue)
        {
            var (course, chauffeur) = CreerDonneesValides();

            course.FauteuilRoulant = fauteuilRequis;
            chauffeur.Vehicule.Categorie.PossedeRampe =
                rampeDisponible;

            var erreurs = _service.ValiderConformite(
                course, chauffeur.Vehicule);

            VerifierErreur(
                erreurs,
                "Le véhicule n'est pas adapté aux fauteuils roulants.",
                erreurAttendue);
        }

        [Fact]
        public void ValiderConformite_DonneesValides_AucuneErreur()
        {
            var (course, chauffeur) = CreerDonneesValides();

            var erreurs = _service.ValiderConformite(
                course, chauffeur.Vehicule);

            Assert.Empty(erreurs);

            _factory.Verify(f => f.Ouvrir(), Times.Never);
        }

        [Fact]
        public void ValiderConformite_PlusieursReglesViolees_RetourneToutesLesErreurs()
        {
            var (course, chauffeur) = CreerDonneesValides();

            course.SiegeEnfantRequis = true;
            chauffeur.Vehicule.PossedeSiegeEnfant = false;

            course.NombrePassagers = 5;

            course.AnimalABord = true;
            chauffeur.Vehicule.AccepteAnimaux = false;

            course.NombreBagages = 3;
            course.CategorieDemandeeId = 2;

            course.FauteuilRoulant = true;
            chauffeur.Vehicule.Categorie.PossedeRampe = false;

            var erreurs = _service.ValiderConformite(
                course, chauffeur.Vehicule);

            Assert.Equal(6, erreurs.Count);
        }

        [Fact]
        public void AttribuerCourse_ConditionsValides_SauvegardeUneFois()
        {
            var (course, chauffeur) = CreerDonneesValides();

            ConfigurerLecture(course, chauffeur);

            _session.Setup(s => s.Sauvegarder());

            var erreurs = _service.AttribuerCourse(
                course, chauffeur);

            Assert.Empty(erreurs);

            Assert.Equal(chauffeur.Id, course.ChauffeurId);
            Assert.Equal(StatutCourse.Affectee, course.Statut);
            Assert.False(chauffeur.EstDisponible);

            _session.Verify(
                s => s.Sauvegarder(),
                Times.Once);
        }

        [Theory]
        [InlineData(StatutCourse.Affectee)]
        [InlineData(StatutCourse.Annulee)]
        [InlineData(StatutCourse.Terminee)]
        public void AttribuerCourse_StatutInvalide_RefuseSansSauvegarder(
            StatutCourse statut)
        {
            var (course, chauffeur) = CreerDonneesValides();

            course.Statut = statut;

            ConfigurerLecture(course, chauffeur);

            var erreurs = _service.AttribuerCourse(
                course, chauffeur);

            Assert.Contains(
                "La course est déjà affectée.", erreurs);

            Assert.Equal(statut, course.Statut);
            Assert.Null(course.ChauffeurId);
            Assert.True(chauffeur.EstDisponible);

            _session.Verify(
                s => s.Sauvegarder(),
                Times.Never);
        }

        [Fact]
        public void AttribuerCourse_ChauffeurIndisponible_RefuseSansSauvegarder()
        {
            var (course, chauffeur) = CreerDonneesValides();

            chauffeur.EstDisponible = false;

            ConfigurerLecture(course, chauffeur);

            var erreurs = _service.AttribuerCourse(
                course, chauffeur);

            Assert.Contains(
                "Le chauffeur n'est pas disponible.",
                erreurs);

            Assert.Equal(StatutCourse.Demandee, course.Statut);
            Assert.Null(course.ChauffeurId);

            _session.Verify(
                s => s.Sauvegarder(),
                Times.Never);
        }

        [Fact]
        public void AttribuerCourse_VehiculeNonConforme_RefuseSansSauvegarder()
        {
            var (course, chauffeur) = CreerDonneesValides();

            course.NombrePassagers = 5;

            ConfigurerLecture(course, chauffeur);

            var erreurs = _service.AttribuerCourse(
                course, chauffeur);

            Assert.Contains(
                "Le nombre de passagers dépasse la capacité du véhicule.",
                erreurs);

            Assert.Equal(StatutCourse.Demandee, course.Statut);
            Assert.Null(course.ChauffeurId);
            Assert.True(chauffeur.EstDisponible);

            _session.Verify(
                s => s.Sauvegarder(),
                Times.Never);
        }


        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        public void AttribuerCourse_EntiteInexistante_RefuseSansSauvegarder(
            bool courseExiste,
            bool chauffeurExiste)
        {
            var (course, chauffeur) = CreerDonneesValides();

            _session.Setup(s => s.ObtenirCourse(course.Id))
                .Returns(courseExiste ? course : null);

            _session.Setup(s => s.ObtenirChauffeur(chauffeur.Id))
                .Returns(chauffeurExiste ? chauffeur : null);

            var erreurs = _service.AttribuerCourse(
                course, chauffeur);

            if (!courseExiste)
            {
                Assert.Contains(
                    "La course n'existe plus.", erreurs);
            }

            if (!chauffeurExiste)
            {
                Assert.Contains(
                    "Le chauffeur n'existe plus.", erreurs);
            }

            Assert.Null(course.ChauffeurId);
            Assert.True(chauffeur.EstDisponible);

            _session.Verify(
                s => s.Sauvegarder(),
                Times.Never);
        }
    }
}
