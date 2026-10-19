using Taxiturne.Model.Interfaces;
using Taxiturne.Utils;
using Taxiturne.Utils.Commands;
using Taxiturne.Utils.Services.Interfaces;
using Taxiturne.Utils.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Taxiturne.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IUserDAL _userDAL;
        private readonly INavigationService _navigationService;
        private readonly IUserSessionService _userSessionService;

        public User? ConnectedUser
        {
            get => _userSessionService.ConnectedUser;
        }

        public string WelcomeMessage
        {
            get => ConnectedUser == null ? "Bienvenue chère personne inconnue!" : $"Bienvenue {ConnectedUser.Prenom} ({ConnectedUser.Role})!";
        }

        public HomeViewModel(IUserDAL userDAL, INavigationService navigationService, IUserSessionService userSessionService)
        {
            _userDAL = userDAL;
            _navigationService = navigationService;
            _userSessionService = userSessionService;
            LogoutCommand = new RelayCommand(Logout, CanLogout);
        }

        // Commande pour la déconnexion
        public ICommand LogoutCommand { get; set; }

        // Méthode pour gérer la déconnexion de l'utilisateur
        private void Logout()
        {
            _userSessionService.ConnectedUser = null;
            _navigationService.NavigateTo<ConnectUserViewModel>();
        }

        // Vérifie si la commande de déconnexion peut être exécutée
        private bool CanLogout()
        {
            return _userSessionService.IsUserConnected;
        }
    }
}
