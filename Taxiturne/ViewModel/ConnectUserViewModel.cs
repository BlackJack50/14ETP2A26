
using System.Windows.Controls;
using System.Windows.Input;
using Taxiturne.Services;
using Taxiturne.Utils;
using Taxiturne.Utils.Commands;
using Taxiturne.Utils.Services.Interfaces;

namespace Taxiturne.ViewModel
{
    class ConnectUserViewModel : BaseViewModel
    {
        private readonly IAuthentificationService _authentificationService;
        private readonly INavigationService _navigationService;
        private readonly IUserSessionService _userSessionService;

        private string _username = string.Empty;

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                    ValidateProperty(nameof(Username), value);
                }
            }
        }

        private string _password = string.Empty;

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                    ValidateProperty(nameof(Password), value);
                }
            }
        }

        public ICommand ConnectCommand { get; }

        public ConnectUserViewModel(
            IAuthentificationService authentificationService,
            INavigationService navigationService,
            IUserSessionService userSessionService)
        {
            _authentificationService = authentificationService;
            _navigationService = navigationService;
            _userSessionService = userSessionService;

            ConnectCommand = new RelayCommand(
                Connect,
                CanConnect);
        }

        private void Connect(object parameter)
        {
            if (parameter is not PasswordBox passwordBox)
            {
                AddError(
                    nameof(Password),
                    "Le mot de passe est requis.");

                OnPropertyChanged(nameof(ErrorMessages));
                return;
            }

            Password = passwordBox.Password;

            ValidateProperty(nameof(Username), Username);
            ValidateProperty(nameof(Password), Password);

            if (HasErrors)
            {
                return;
            }

            var user = _authentificationService.Authentifier(
                Username,
                Password);

            if (user == null)
            {
                AddError(
                    nameof(Password),
                    "Utilisateur ou mot de passe invalide.");

                OnPropertyChanged(nameof(ErrorMessages));
                return;
            }

            _userSessionService.ConnectedUser = user;

            if (user.Role == "Repartiteur")
            {
                _navigationService.NavigateTo<RepartiteurViewModel>();
            }
            else
            {
                _navigationService.NavigateTo<HomeViewModel>();
            }
        }

        private bool CanConnect()
        {
            return !string.IsNullOrWhiteSpace(Username);
        }

        private void ValidateProperty(
            string propertyName,
            string value)
        {
            ClearErrors(propertyName);

            switch (propertyName)
            {
                case nameof(Username):
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        AddError(
                            propertyName,
                            "Le nom d'utilisateur est requis.");
                    }
                    else if (value.Length < 2)
                    {
                        AddError(
                            propertyName,
                            "Le nom d'utilisateur doit contenir au moins 2 caractères.");
                    }
                    break;

                case nameof(Password):
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        AddError(
                            propertyName,
                            "Le mot de passe est requis.");
                    }
                    break;
            }

            OnPropertyChanged(nameof(ErrorMessages));
        }
    }
}
