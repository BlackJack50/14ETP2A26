namespace Taxiturne.Model
{
    public class Chauffeur
    {
        public int Id { get; set; }

        public string Prenom { get; set; } = string.Empty;

        public string Nom { get; set; } = string.Empty;

        public int VehiculeId { get; set; }

        public Vehicule Vehicule { get; set; } = null!;

        public bool EstDisponible { get; set; }
    }
}
