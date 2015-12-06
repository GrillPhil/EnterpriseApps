using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseApps.Portable.Model;
using EnterpriseApps.Portable.Service;

namespace EnterpriseApps.Portable.ViewModel
{
    public class UserViewModel : AsyncViewModelBase
    {
        private IUserRepository _userRepository;
        private Model.User _user;

        public User User
        {
            get
            {
                return _user;
            }

            set
            {
                _user = value;
                RaisePropertyChanged();
            }
        }

        public UserViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userRepository.SelectedUserChanged += (__, user) => User = user;
        }
    }
}
