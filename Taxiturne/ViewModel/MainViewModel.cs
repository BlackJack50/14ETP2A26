using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Taxiturne.Model;
using Taxiturne.Model.Interfaces;
using Taxiturne.Utils;
using Taxiturne.Utils.Commands;
using Taxiturne.Utils.Services;
using Taxiturne.Utils.Services.Interfaces;

namespace Taxiturne.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IUserSessionService _userSessionService;

        public INavigationService NavigationService
        {
            get => _navigationService;
        }

        public IUserSessionService UserSessionService
        {
            get => _userSessionService;
        }

        public ICommand NavigateToConnectUserViewCommand { get; set; }
        public ICommand NavigateToHomeViewCommand { get; set; }

        public ICommand DisconnectCommand { get; }

        private void Disconnect()
        {
            _userSessionService.ConnectedUser = null;
            OnPropertyChanged(nameof(UserSessionService.IsUserConnected));
            _navigationService.NavigateTo<ConnectUserViewModel>();
        }

        public MainViewModel(INavigationService navigationService, IUserSessionService userSessionService)
        {
            _navigationService = navigationService;
            _userSessionService = userSessionService;

            NavigateToConnectUserViewCommand = new RelayCommand(() => NavigationService.NavigateTo<ConnectUserViewModel>());
            NavigateToHomeViewCommand = new RelayCommand(() => NavigationService.NavigateTo<HomeViewModel>());
            DisconnectCommand = new RelayCommand(Disconnect, () => UserSessionService.IsUserConnected);

            NavigationService.NavigateTo<HomeViewModel>();
        }
    }
}
