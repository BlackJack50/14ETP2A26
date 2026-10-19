using System.Windows;
using Taxiturne.Utils;
using Taxiturne.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Taxiturne.Model.Interfaces;
using Taxiturne.Model.DAL;
using Taxiturne.Utils.Services.Interfaces;
using Taxiturne.Utils.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Taxiturne
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public App()
        {
            // Note à moi-même, mieux séparer en fonctions ici. 
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());
            IConfiguration configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<ConnectUserViewModel>();

            services.AddSingleton<IUserDAL, UserDAL>();
            services.AddSingleton<ICourseDAL, CourseDAL>();
            services.AddSingleton<IChauffeurDAL, ChauffeurDAL>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IUserSessionService, Service>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider =>
            {
                BaseViewModel ViewModelFactory(Type viewModelType)
                {
                    return (BaseViewModel)serviceProvider.GetRequiredService(viewModelType);
                }
                return ViewModelFactory;
            });

            services.AddDbContext<ApplicationDbContext>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (dbContext.Database.EnsureCreated())
                {
                    dbContext.SeedData();
                }
            }

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
