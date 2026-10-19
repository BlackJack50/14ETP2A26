using Microsoft.EntityFrameworkCore;
using Taxiturne.Utils.Services;
using Taxiturne.Data;
using Taxiturne.Model;
using System.IO;

public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(
       DbContextOptionsBuilder optionsBuilder)
    {
        // Définir le chemin absolu pour la base de données dans le répertoire AppData
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Taxiturne", "Taxiturne.db");
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
        var connectionString = $"Data Source={dbPath}";

        // Configurer le DbContext pour utiliser la chaîne de connexion
        optionsBuilder.UseSqlite(connectionString);
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
                new User { Prenom = "Hélène", Nom = "Sarrazin", Username = "hsarrazin", Password = "password123", Role = "Administrateur" },
                new User { Prenom = "Marc", Nom = "Tremblay", Username = "mtremblay", Password = "password123", Role = "Repartiteur" }
            );
            SaveChanges();
        }
        if (!Courses.Any())
        {
            DonneesDeDepart.Ajouter(this);
        }
    }
}
