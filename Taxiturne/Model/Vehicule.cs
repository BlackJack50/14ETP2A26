namespace Taxiturne.Model
{
    public class Vehicule
    {
        public int Id { get; set; }

        public string Immatriculation { get; set; } = string.Empty;

        public int CategorieId { get; set; }

        public CategorieVehicule Categorie { get; set; } = null!;

        public bool PossedeSiegeEnfant { get; set; }

        public bool AccepteAnimaux { get; set; }
    }
}
