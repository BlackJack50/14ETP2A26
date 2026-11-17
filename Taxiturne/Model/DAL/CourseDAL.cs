using Microsoft.EntityFrameworkCore;
using Taxiturne.Model.Interfaces;

namespace Taxiturne.Model.DAL
{
    public class CourseDAL : ICourseDAL
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public CourseDAL(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IReadOnlyList<Course> ObtenirCoursesDemandees()
        {
            using var context = _contextFactory.CreateDbContext();

            return context.Courses
                .AsNoTracking()
                .Include(course => course.Client)
                .Include(course => course.CategorieDemandee)
                .Where(course => course.Statut == StatutCourse.Demandee)
                .OrderBy(course => course.DateHeurePriseEnCharge)
                .ToList();
        }

        public void MettreAJour(Course course)
        {
            using var context = _contextFactory.CreateDbContext();

            context.Courses.Update(course);
            context.SaveChanges();
        }
    }
}
