using Taxiturne.Model;

namespace Taxiturne.Data
{
    public static class DonneesDeDepart
    {
        public static void Ajouter(ApplicationDbContext context)
        {
            var berline = new CategorieVehicule { Nom = "Berline", PassagersMax = 4, BagagesMax = 4 };
            var familiale = new CategorieVehicule { Nom = "Familiale", PassagersMax = 6, BagagesMax = 8 };
            var adapte = new CategorieVehicule { Nom = "Adapté", PassagersMax = 4, BagagesMax = 4, PossedeRampe = true };
            var ecologique = new CategorieVehicule { Nom = "Écologique", PassagersMax = 4, BagagesMax = 3 };
            var luxe = new CategorieVehicule { Nom = "Luxe", PassagersMax = 4, BagagesMax = 5 };

            var julie = NouveauChauffeur("Julie", "Gagnon", "T-1001", berline, siegeEnfant: true, animaux: false);
            var samuel = NouveauChauffeur("Samuel", "Roy", "T-1002", berline, siegeEnfant: false, animaux: true);
            var nadia = NouveauChauffeur("Nadia", "Côté", "T-2001", familiale, siegeEnfant: true, animaux: true);
            var olivier = NouveauChauffeur("Olivier", "Bouchard", "T-3001", adapte, siegeEnfant: false, animaux: true);
            var karine = NouveauChauffeur("Karine", "Morin", "T-4001", ecologique, siegeEnfant: false, animaux: false);
            var philippe = NouveauChauffeur("Philippe", "Lavoie", "T-5001", luxe, siegeEnfant: false, animaux: false);
            var isabelle = NouveauChauffeur("Isabelle", "Fortin", "T-1003", berline, siegeEnfant: true, animaux: true);
            isabelle.EstDisponible = false;

            var sophie = new Client { Prenom = "Sophie", Nom = "Caron", Telephone = "418 555-0101" };
            var louise = new Client { Prenom = "Louise", Nom = "Bergeron", Telephone = "418 555-0102" };
            var martin = new Client { Prenom = "Martin", Nom = "Pelletier", Telephone = "418 555-0103" };
            var antoine = new Client { Prenom = "Antoine", Nom = "Girard", Telephone = "418 555-0104" };
            var emilie = new Client { Prenom = "Émilie", Nom = "Ouellet", Telephone = "418 555-0105" };

            var aujourdhui = DateTime.Today;

            context.Chauffeurs.AddRange(julie, samuel, nadia, olivier, karine, philippe, isabelle);
            context.Courses.AddRange(
                new Course
                {
                    Client = sophie, AdresseDepart = "Château Frontenac", AdresseDestination = "Aéroport Jean-Lesage",
                    DateHeurePriseEnCharge = aujourdhui.AddHours(9), NombrePassagers = 2, NombreBagages = 2,
                    CategorieDemandee = berline
                },
                new Course
                {
                    Client = louise, AdresseDepart = "1050, chemin Sainte-Foy", AdresseDestination = "Hôpital de l'Enfant-Jésus",
                    DateHeurePriseEnCharge = aujourdhui.AddHours(10), NombrePassagers = 1, NombreBagages = 0,
                    CategorieDemandee = adapte, FauteuilRoulant = true
                },
                new Course
                {
                    Client = martin, AdresseDepart = "Gare du Palais", AdresseDestination = "Galeries de la Capitale",
                    DateHeurePriseEnCharge = aujourdhui.AddHours(11), NombrePassagers = 6, NombreBagages = 5,
                    CategorieDemandee = familiale
                },
                new Course
                {
                    Client = antoine, AdresseDepart = "Place Laurier", AdresseDestination = "3e Avenue, Limoilou",
                    DateHeurePriseEnCharge = aujourdhui.AddHours(13), NombrePassagers = 1, NombreBagages = 1,
                    CategorieDemandee = berline, AnimalABord = true
                },
                new Course
                {
                    Client = emilie, AdresseDepart = "Marché du Vieux-Port", AdresseDestination = "Chute Montmorency",
                    DateHeurePriseEnCharge = aujourdhui.AddHours(14), NombrePassagers = 3, NombreBagages = 2,
                    CategorieDemandee = berline, SiegeEnfantRequis = true
                },
                new Course
                {
                    Client = sophie, AdresseDepart = "Aéroport Jean-Lesage", AdresseDestination = "Château Frontenac",
                    DateHeurePriseEnCharge = aujourdhui.AddHours(8), NombrePassagers = 2, NombreBagages = 2,
                    CategorieDemandee = berline, Statut = StatutCourse.Affectee, Chauffeur = isabelle
                });
            context.SaveChanges();
        }

        private static Chauffeur NouveauChauffeur(string prenom, string nom, string immatriculation,
                                                  CategorieVehicule categorie, bool siegeEnfant, bool animaux)
        {
            var vehicule = new Vehicule
            {
                Immatriculation = immatriculation,
                Categorie = categorie,
                PossedeSiegeEnfant = siegeEnfant,
                AccepteAnimaux = animaux
            };
            return new Chauffeur { Prenom = prenom, Nom = nom, Vehicule = vehicule, EstDisponible = true };
        }
    }
}
