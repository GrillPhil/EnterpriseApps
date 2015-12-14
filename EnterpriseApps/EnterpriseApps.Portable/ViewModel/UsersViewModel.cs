using EnterpriseApps.Portable.Model;
using EnterpriseApps.Portable.Service;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;

namespace EnterpriseApps.Portable.ViewModel
{
    public class UsersViewModel : AsyncViewModelBase
    {
        private IUserRepository _userRepository;
        private IResourceService _resourceService;
        private IDialogService _dialogService;
        private ObservableCollection<User> _users;
        private User _selectedUser;
        private string _query;

        public ObservableCollection<User> Users
        {
            get
            {
                return _users;
            }

            set
            {
                _users = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand InitCommand { get; private set; }
        public RelayCommand<User> SelectUserCommand { get; private set; }

        public User SelectedUser
        {
            get
            {
                return _selectedUser;
            }

            set
            {
                if (value != _selectedUser)
                {
                    _selectedUser = value;
                    SelectUser(value);
                    RaisePropertyChanged();
                }
            }
        }

        public string Query
        {
            get
            {
                return _query;
            }

            set
            {
                if (value != _query)
                {
                    _query = value;
                    RaisePropertyChanged();
                    SearchUsers(_query);
                }
            }
        }


		public UsersViewModel(IUserRepository userRepository, IResourceService resourceService, IDialogService dialogService)
        {
            _userRepository = userRepository;
            _resourceService = resourceService;
            _dialogService = dialogService;

            InitCommand = new RelayCommand(Init);
            SelectUserCommand = new RelayCommand<User>(SelectUser);

            if (IsInDesignMode)
                LoadMockUsers();
        }

        private async void Init()
        {
            await LoadUsersAsync();
        }

        private void SelectUser(User user)
        {
            _userRepository.SetSelectedUser(user);
        }

        private async Task LoadUsersAsync()
        {
            IsLoading = true;
            LoadingMessage = _resourceService.GetString("UsersLoadingMessage");

            try
            {
                Users = await _userRepository.GetUsersAsync();
                IsLoading = false;
                LoadingMessage = null;
            }
            catch (HttpRequestException)
            {
                ShowNetworkError();
            }
            catch (HttpServiceException)
            {
                ShowNetworkError();
            }
            catch (JsonReaderException)
            {
                ShowNetworkError();
            }
        }

        private void SearchUsers(string query)
        {
            _userRepository.SearchUsers(query);
        }

        private async void ShowNetworkError()
        {
            if (await _dialogService.ShowMessage(_resourceService.GetString("NetworkErrorMessage"), _resourceService.GetString("NetworkErrorTitle"), _resourceService.GetString("NetworkErrorButtonReload"), _resourceService.GetString("NetworkErrorButtonCancel"), null))
            {
                Init();
            }
            else
            {
                IsLoading = false;
                LoadingMessage = null;
            }
        }

        private void LoadMockUsers()
        {
            Users = new ObservableCollection<User>()
            {
                new User() { FirstName = "Samuel", LastName = "Kim", ThumbnailUrl = "https://randomuser.me/api/portraits/thumb/women/70.jpg" }
            };
        }
    }
}
