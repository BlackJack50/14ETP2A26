namespace Taxiturne.Model.Interfaces
{
    public interface IChauffeurDAL
    {
        IReadOnlyList<Chauffeur> ObtenirChauffeursDisponibles();
    }
}
