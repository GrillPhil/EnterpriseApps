using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseApps.Portable.ViewModel;
using EnterpriseApps.Portable.Service.Fakes;
using GalaSoft.MvvmLight.Views.Fakes;
using EnterpriseApps.Portable.Service;

namespace EnterpriseApps.Portable.Tests
{
    [TestClass]
    public class UserViewModelIntegrationTests
    {
        [TestCategory("Integration Tests")]
        [Timeout(5000)]
        [TestMethod]
        public async Task TestLoadUsersAsync()
        {
            // Arrange
            var baseUrl = "http://api.randomuser.me/";
            var vm = CreateUsersViewModel(baseUrl);
            var po = new PrivateObject(vm);

            // Act
            await (Task)po.Invoke("LoadUsersAsync");

            // Assert
            Assert.IsNotNull(vm.Users);
        }

        private UsersViewModel CreateUsersViewModel(string baseUrl)
        {
            var httpService = new HttpService(baseUrl);
            var mappingService = new MappingService();
            var userRepository = new UserRepository(httpService, mappingService);

            var stubResourceService = new StubIResourceService();
            stubResourceService.GetStringString = (_) =>
            {
                return string.Empty;
            };

            var stubDialogService = new StubIDialogService();

            return new UsersViewModel(userRepository, stubResourceService, stubDialogService);
        }
    }
}
