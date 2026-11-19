using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Taxiturne.Model.DAL;
using Taxiturne.Model.Interfaces;
using Taxiturne.Services;
using Taxiturne.Utils;
using Taxiturne.Utils.Services;
using Taxiturne.Utils.Services.Interfaces;
using Taxiturne.ViewModel;

namespace Taxiturne
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            string dossier = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData),
                "Taxiturne");

            Directory.CreateDirectory(dossier);

            string dbPath = Path.Combine(dossier, "Taxiturne.db");

            services.AddDbContextFactory<ApplicationDbContext>(
                options => options.UseSqlite(
                    $"Data Source={dbPath}"));

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<ConnectUserViewModel>();
            services.AddSingleton<RepartiteurViewModel>();
            services.AddSingleton<IAffectationSessionFactory, AffectationSessionFactory>();
            services.AddSingleton<IAuthentificationService, AuthentificationService>();


            services.AddSingleton<IUserDAL, UserDAL>();
            services.AddSingleton<ICourseDAL, CourseDAL>();
            services.AddSingleton<IChauffeurDAL, ChauffeurDAL>();

            services.AddSingleton<IRepartitionService, RepartitionService>();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IUserSessionService, Service>();

            services.AddSingleton<Func<Type, BaseViewModel>>(provider =>
            {
                BaseViewModel ViewModelFactory(Type viewModelType)
                {
                    return (BaseViewModel)
                        provider.GetRequiredService(viewModelType);
                }

                return ViewModelFactory;
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var contextFactory = _serviceProvider
                .GetRequiredService<IDbContextFactory<ApplicationDbContext>>();

            using (var context = contextFactory.CreateDbContext())
            {
                context.Database.EnsureCreated();
                context.SeedData();
            }

            var mainWindow = _serviceProvider
                .GetRequiredService<MainWindow>();

            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider.Dispose();
            base.OnExit(e);
        }
    }
}
