using EnterpriseApps.Portable.Service.Fakes;
using EnterpriseApps.Portable.ViewModel;
using GalaSoft.MvvmLight.Views.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EnterpriseApps.Portable.Tests
{
    [TestClass]
    public class UsersViewModelTests
    {
        [TestMethod]
        public async Task TestLoadUsersAsync()
        {
            // Arrange
            var vm = CreateUsersViewModel();
            var po = new PrivateObject(vm);

            // Act
            await (Task)po.Invoke("LoadUsersAsync");

            // Assert
            Assert.IsNotNull(vm.Users);
        }

        private UsersViewModel CreateUsersViewModel()
        {
            var stubUserRepository = new StubIUserRepository();
            stubUserRepository.GetUsersAsync = () => 
            {
                var users = new ObservableCollection<Model.User>();
                return Task.FromResult(users);
            };

            var stubResourceService = new StubIResourceService();
            stubResourceService.GetStringString = (_) =>
            {
                return string.Empty;
            };

            var stubDialogService = new StubIDialogService();

            return new UsersViewModel(stubUserRepository, stubResourceService, stubDialogService);
        }
    }
}
