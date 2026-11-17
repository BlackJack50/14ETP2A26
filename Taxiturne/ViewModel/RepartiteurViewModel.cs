
using System.Collections.ObjectModel;
using System.Windows.Input;
using Taxiturne.Model;
using Taxiturne.Model.Interfaces;
using Taxiturne.Services;
using Taxiturne.Utils;
using Taxiturne.Utils.Commands;
using Taxiturne.Utils.Services.Interfaces;

namespace Taxiturne.ViewModel
{
    internal class RepartiteurViewModel : BaseViewModel
    {
        private readonly IRepartitionService _repartitionService;
        private readonly ICourseDAL _courseDAL;
        private readonly IChauffeurDAL _chauffeurDAL;
        private readonly IUserSessionService _userSessionService;

        public ObservableCollection<Chauffeur> Chauffeurs { get; }
            = new();

        public ObservableCollection<Course> Courses { get; }
            = new();

        private Chauffeur? _selectedChauffer;

        public Chauffeur? SelectedChauffer
        {
            get => _selectedChauffer;
            set
            {
                if (_selectedChauffer != value)
                {
                    _selectedChauffer = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private Course? _selectedCourse;

        public Course? SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                if (_selectedCourse != value)
                {
                    _selectedCourse = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private string _messageErreur = string.Empty;

        public string MessageErreur
        {
            get => _messageErreur;
            set
            {
                if (_messageErreur != value)
                {
                    _messageErreur = value;
                    OnPropertyChanged();
                }
            }
        }

        public User? ConnectedUser =>
            _userSessionService.ConnectedUser;

        public string WelcomeMessage =>
            ConnectedUser == null
                ? "Bienvenue chère personne inconnue!"
                : $"Bienvenue {ConnectedUser.Prenom} ({ConnectedUser.Role})!";

        public ICommand AttribuerCommand { get; }

        public RepartiteurViewModel(
            IRepartitionService repartitionService,
            ICourseDAL courseDAL,
            IChauffeurDAL chauffeurDAL,
            IUserSessionService userSessionService)
        {
            _repartitionService = repartitionService;
            _courseDAL = courseDAL;
            _chauffeurDAL = chauffeurDAL;
            _userSessionService = userSessionService;

            AttribuerCommand = new RelayCommand(
                Attribuer,
                CanAttribuer);

            RafraichirListes();
        }

        private void Attribuer()
        {
            if (SelectedCourse == null ||
                SelectedChauffer == null)
            {
                return;
            }

            var erreurs = _repartitionService.AttribuerCourse(
                SelectedCourse,
                SelectedChauffer);

            MessageErreur = string.Join("\n", erreurs);

            if (erreurs.Count == 0)
            {
                RafraichirListes();
            }
        }

        private bool CanAttribuer()
        {
            return SelectedCourse != null &&
                   SelectedChauffer != null;
        }

        private void RafraichirListes()
        {
            Chauffeurs.Clear();

            foreach (var chauffeur in
                _chauffeurDAL.ObtenirChauffeursDisponibles())
            {
                Chauffeurs.Add(chauffeur);
            }

            Courses.Clear();

            foreach (var course in
                _courseDAL.ObtenirCoursesDemandees())
            {
                Courses.Add(course);
            }

            SelectedChauffer = null;
            SelectedCourse = null;
        }
    }
}
