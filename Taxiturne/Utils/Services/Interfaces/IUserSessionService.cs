using System.ComponentModel;
using Taxiturne.Model;

namespace Taxiturne.Utils.Services.Interfaces
{
    public interface IUserSessionService : INotifyPropertyChanged
    {
        User ConnectedUser { get; set; }

        bool IsUserConnected { get; }
    }
}
