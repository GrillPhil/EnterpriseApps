using EnterpriseApps.Portable.Model;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using EnterpriseApps.Portable.ExtensionMethods;

namespace EnterpriseApps.Portable.Service
{
    public class UserRepository : IUserRepository
    {
        private IHttpService _httpService;
        private IMappingService _mappingService;
        private IEnumerable<User> _allUsers;
        private ObservableCollection<User> _currentUsers;
        private User _selectedUser;

        public event EventHandler<Model.User> SelectedUserChanged;

        public UserRepository(IHttpService httpService, IMappingService mappingService)
        {
            _httpService = httpService;
            _mappingService = mappingService;
        }

        public async Task<ObservableCollection<User>> GetUsersAsync()
        {
            if (_allUsers == null)
            {
                var dtos = await _httpService.GetUsersAsync(100);
                _allUsers = await Task.Run(() => { return _mappingService.MapUsers(dtos).OrderBy(e => e.FirstName); });
                _currentUsers = new ObservableCollection<User>(_allUsers);
            }

            return _currentUsers;
        }

        public void SearchUsers(string query)
        {
            var searchResult = _allUsers.Where(e => (!string.IsNullOrEmpty(e.FirstName) && Regex.IsMatch(e.FirstName, query, RegexOptions.IgnoreCase)) || (!string.IsNullOrEmpty(e.LastName) && Regex.IsMatch(e.LastName, query, RegexOptions.IgnoreCase)));
            _currentUsers.Filter(searchResult);
        }

        public void SetSelectedUser(User user)
        {
            _selectedUser = user;
            if (SelectedUserChanged != null)
                SelectedUserChanged(this, user);
        }
    }
}
