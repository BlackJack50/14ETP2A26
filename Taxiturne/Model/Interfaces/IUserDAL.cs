using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxiturne.Model.Interfaces
{
    public interface IUserDAL
    {
        User? FindByUsernameAndPassword(string username, string password);
    }
}
