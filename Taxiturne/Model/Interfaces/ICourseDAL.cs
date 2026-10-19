namespace Taxiturne.Model.Interfaces
{
    public interface ICourseDAL
    {
        IReadOnlyList<Course> ObtenirCoursesDemandees();

        void MettreAJour(Course course);
    }
}
