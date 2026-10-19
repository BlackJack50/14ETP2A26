using System.ComponentModel;

namespace Taxiturne.Utils.Services.Interfaces
{
    public interface IUserSessionService : INotifyPropertyChanged
    {
        User ConnectedUser { get; set; }

        bool IsUserConnected { get; }
    }
}
