namespace Taxiturne.Model
{
    public class Course
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; } = null!;

        public string AdresseDepart { get; set; } = string.Empty;

        public string AdresseDestination { get; set; } = string.Empty;

        public DateTime DateHeurePriseEnCharge { get; set; }

        public int NombrePassagers { get; set; }

        public int NombreBagages { get; set; }

        public int CategorieDemandeeId { get; set; }

        public CategorieVehicule CategorieDemandee { get; set; } = null!;

        public bool AnimalABord { get; set; }

        public bool SiegeEnfantRequis { get; set; }

        public bool FauteuilRoulant { get; set; }

        public StatutCourse Statut { get; set; }

        public int? ChauffeurId { get; set; }

        public Chauffeur? Chauffeur { get; set; }
    }
}
