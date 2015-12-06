using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using System.Net.Http.Fakes;
using EnterpriseApps.Portable.Service;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using EnterpriseApps.Portable.DTO;

namespace EnterpriseApps.Portable.Tests
{
    [TestClass]
    public class HttpServiceTests
    {
        [ExpectedException(typeof(Newtonsoft.Json.JsonReaderException))]
        [TestMethod]
        public async Task TestGetUsersAsyncWithInvalidResponseContent()
        {
            var baseUrl = "http://";
            var count = 100;
            var content = "Hello world";
            var message = CreateFakeHttpResponseMessage(HttpStatusCode.OK, content);

            // Act
            var result = await TestGetUsersAsyncWithShimmedHttpClient(baseUrl, count, message);
        }

        [ExpectedException(typeof(HttpServiceException))]
        [TestMethod]
        public async Task TestGetUsersAsyncWithEmptyResponseContent()
        {
            // Arrange
            var baseUrl = "http://";
            var count = 100;
            var content = string.Empty;
            var message = CreateFakeHttpResponseMessage(HttpStatusCode.OK, content);

            // Act
            var result = await TestGetUsersAsyncWithShimmedHttpClient(baseUrl, count, message);
        }

        [TestMethod]
        public async Task TestGetUsersAsyncWithValidResponseContent()
        {
            // Arrange
            var baseUrl = "http://";
            var count = 100;
            var content = await LoadStringFromFileAsync(@"SampleData\ValidSingleUserResponseContent.json");
            var message = CreateFakeHttpResponseMessage(HttpStatusCode.OK, content);

            // Act
            var result = await TestGetUsersAsyncWithShimmedHttpClient(baseUrl, count, message);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        private async Task<IEnumerable<Result>> TestGetUsersAsyncWithShimmedHttpClient(string baseUrl, int count, HttpResponseMessage message)
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.GetAsyncString = (__, ___) =>
                {
                    return Task.FromResult(message);
                };

                var service = new HttpService(baseUrl);
                var result = await service.GetUsersAsync(count);

                return result;
            }
        }

        private HttpResponseMessage CreateFakeHttpResponseMessage(HttpStatusCode status, string content)
        {
            var message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content ?? ""));
            message.Content = new StreamContent(stream);

            return message;
        }

        private async Task<string> LoadStringFromFileAsync(string url)
        {
            var tr = new StreamReader(url);
            return await tr.ReadToEndAsync();
        }
    }
}
