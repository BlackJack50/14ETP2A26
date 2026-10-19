namespace Taxiturne.Model
{
    public class CategorieVehicule
    {
        public int Id { get; set; }

        public string Nom { get; set; } = string.Empty;

        public int PassagersMax { get; set; }

        public int BagagesMax { get; set; }

        public bool PossedeRampe { get; set; }
    }
}
