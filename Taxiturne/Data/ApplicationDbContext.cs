
using Microsoft.EntityFrameworkCore;
using Taxiturne.Data;
using Taxiturne.Model;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<CategorieVehicule> Categories { get; set; }
    public DbSet<Vehicule> Vehicules { get; set; }
    public DbSet<Chauffeur> Chauffeurs { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Course> Courses { get; set; }

    public void SeedData()
    {
        if (!Users.Any())
        {
            Users.AddRange(
                new User
                {
                    Prenom = "Hélène",
                    Nom = "Sarrazin",
                    Username = "hsarrazin",
                    Password = "password123",
                    Role = "Administrateur"
                },
                new User
                {
                    Prenom = "Marc",
                    Nom = "Tremblay",
                    Username = "mtremblay",
                    Password = "password123",
                    Role = "Repartiteur"
                }
            );

            SaveChanges();
        }

        if (!Courses.Any())
        {
            DonneesDeDepart.Ajouter(this);
        }
    }
}
