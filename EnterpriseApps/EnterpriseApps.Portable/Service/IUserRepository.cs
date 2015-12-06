using EnterpriseApps.Portable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseApps.Portable.Service
{
    public interface IUserRepository
    {
        Task<ObservableCollection<User>> GetUsersAsync();

        void SearchUsers(string query);

        void SetSelectedUser(User user);

        event EventHandler<User> SelectedUserChanged;
    }
}
