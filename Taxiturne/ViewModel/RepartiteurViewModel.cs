using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Taxiturne.Model;
using Taxiturne.Model.Interfaces;
using Taxiturne.Utils;
using Taxiturne.Utils.Commands;
using Taxiturne.Utils.Services.Interfaces;

namespace Taxiturne.ViewModel
{
    internal class RepartiteurViewModel : BaseViewModel
    {
        private readonly IUserDAL _userDAL;
        private readonly INavigationService _navigationService;
        private readonly IUserSessionService _userSessionService;
        private readonly ICourseDAL _courseDAL;
        private readonly IChauffeurDAL _chauffeurDAL;

        public ObservableCollection<Chauffeur> Chauffeurs
        {
            get => new ObservableCollection<Chauffeur>(_chauffeurDAL.ObtenirChauffeursDisponibles());
        }

        public ObservableCollection<Course> Courses
        {
            get => new ObservableCollection<Course>(_courseDAL.ObtenirCoursesDemandees());
        }
        private Chauffeur _selectedChauffer;
        public Chauffeur SelectedChauffer
        {
            get => _selectedChauffer;
            set { _selectedChauffer = value; OnPropertyChanged(nameof(SelectedChauffer)); }
        }
        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set { _selectedCourse = value; OnPropertyChanged(nameof(SelectedCourse)); }
        }

        private string _msgErreur;
        public string MessageErreur
        {
            get => _msgErreur;
            set { _msgErreur = value; OnPropertyChanged(nameof(MessageErreur)); }
        }

        public User? ConnectedUser
        {
            get => _userSessionService.ConnectedUser;
        }

        public string WelcomeMessage
        {
            get => ConnectedUser == null ? "Bienvenue chère personne inconnue!" : $"Bienvenue {ConnectedUser.Prenom} ({ConnectedUser.Role})!";
        }

        public RepartiteurViewModel(IUserDAL userDAL, INavigationService navigationService, IUserSessionService userSessionService, ICourseDAL courseDAL, IChauffeurDAL chauffeurDAL)
        {
            _userDAL = userDAL;
            _navigationService = navigationService;
            _userSessionService = userSessionService;
            _courseDAL = courseDAL;
            LogoutCommand = new RelayCommand(Logout, CanLogout);
            AttribuerCommand = new RelayCommand(Attribuer, CanAttribuer);
            _chauffeurDAL = chauffeurDAL;
        }

        public ICommand AttribuerCommand { get; set; }

        // Commande pour la déconnexion
        public ICommand LogoutCommand { get; set; }

        // Méthode pour gérer la déconnexion de l'utilisateur
        private void Logout()
        {
            _userSessionService.ConnectedUser = null;
            _navigationService.NavigateTo<ConnectUserViewModel>();
        }

        private void Attribuer()
        {
            if (SelectedChauffer != null && SelectedCourse != null)
            {
                Vehicule vehicule = SelectedChauffer.Vehicule;
                Course course = SelectedCourse;

                List<string> erreurs = ValiderComformite(course, vehicule);

                if (erreurs.Count > 0)
                {
                    MessageErreur += string.Join("\n", erreurs);
                    return;
                }

                MessageErreur = null;

                course.ChauffeurId = SelectedChauffer.Id;
                course.Statut = StatutCourse.Affectee;
                SelectedChauffer.EstDisponible = false;
                _courseDAL.MettreAJour(course);
                // Rafraîchir les collections pour refléter les changements
                OnPropertyChanged(nameof(Chauffeur));
                OnPropertyChanged(nameof(Course));
            }
        }
        
        private List<string> ValiderComformite(Course course, Vehicule vehicule)
        {
            List<string> erreurs = new List<string>();


            if (course.SiegeEnfantRequis && !vehicule.PossedeSiegeEnfant)
            {
                erreurs.Add("Le véhicule ne possède pas de siège pour enfants.");
            }

            if (course.NombrePassagers > vehicule.Categorie.PassagersMax)
            {
                erreurs.Add("Le nombre de passagers dépasse la capacité du véhicule.");
            }

            if (course.AnimalABord && !vehicule.AccepteAnimaux)
            {
                erreurs.Add("Le véhicule ne accepte pas les animaux de compagnie.");
            }

            if (course.NombreBagages > vehicule.Categorie.BagagesMax)
            {
                erreurs.Add("Le nombre de bagages dépasse la capacité du véhicule.");
            }

            if (course.CategorieDemandee != vehicule.Categorie)
                erreurs.Add("La véhicule n'est pas la bonne catégorie.");
            return erreurs;
        }

        private bool CanAttribuer()
        {
            return SelectedChauffer != null && SelectedCourse != null;
        }

        // Vérifie si la commande de déconnexion peut être exécutée
        private bool CanLogout()
        {
            return _userSessionService.IsUserConnected;
        }
    }
}
