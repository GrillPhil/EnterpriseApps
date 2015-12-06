using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using EnterpriseApps.Portable.Service;
using EnterpriseApps.Portable.Service.Fakes;
using EnterpriseApps.Portable.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace EnterpriseApps.Portable.Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        [TestMethod]
        public async Task TestGetUsersAsync()
        {
            // Arrange
            var repository = CreateUserRepository();

            // Act
            var result = await repository.GetUsersAsync();

            // Assert
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void TestSearchUsers()
        {
            // Arrange
            var query = "sTEn";
            var allUsers = new List<Model.User>()
            {
                new Model.User() { FirstName = "Petra" },
                new Model.User() { FirstName = "Carsten" },
                new Model.User() { FirstName = "Philipp" }
            };
            var currentUsers = new ObservableCollection<Model.User>(allUsers);

            var repository = CreateUserRepository();
            var po = new PrivateObject(repository);
            po.SetField("_allUsers", allUsers);
            po.SetField("_currentUsers", currentUsers);

            // Act
            po.Invoke("SearchUsers", query);
            var result = (ObservableCollection<Model.User>)po.GetField("_currentUsers");

            // Assert
            Assert.IsTrue(result.Count() == 1);
        }

        private UserRepository CreateUserRepository()
        {
            var stubHttpService = new StubIHttpService();
            stubHttpService.GetUsersAsyncInt32 = (count) =>
            {
                var results = new List<Result>();
                for (int i = 0; i < count; i++)
                {
                    results.Add(new Result()
                    {
                        User = new User()
                    });
                }
                return Task.FromResult<IEnumerable<Result>>(results);
            };
            var mappingService = new MappingService();
            return new UserRepository(stubHttpService, mappingService);
        }
    }
}
