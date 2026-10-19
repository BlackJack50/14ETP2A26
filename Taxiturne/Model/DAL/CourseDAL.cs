using Microsoft.EntityFrameworkCore;
using Taxiturne.Model.Interfaces;

namespace Taxiturne.Model.DAL
{
    public class CourseDAL : ICourseDAL
    {
        private readonly ApplicationDbContext _contexte;

        public CourseDAL(ApplicationDbContext contexte)
        {
            _contexte = contexte;
        }

        public IReadOnlyList<Course> ObtenirCoursesDemandees()
        {
            return _contexte.Courses
                .Include(course => course.Client)
                .Include(course => course.CategorieDemandee)
                .Where(course => course.Statut == StatutCourse.Demandee)
                .OrderBy(course => course.DateHeurePriseEnCharge)
                .ToList();
        }

        public void MettreAJour(Course course)
        {
            _contexte.Courses.Update(course);
            _contexte.SaveChanges();
        }
    }
}
