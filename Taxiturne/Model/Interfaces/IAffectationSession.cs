namespace Taxiturne.Model.Interfaces
{
    public interface IAffectationSession : IDisposable
    {
        Course? ObtenirCourse(int id);

        Chauffeur? ObtenirChauffeur(int id);

        void Sauvegarder();
    }

    public interface IAffectationSessionFactory
    {
        IAffectationSession Ouvrir();
    }
}
