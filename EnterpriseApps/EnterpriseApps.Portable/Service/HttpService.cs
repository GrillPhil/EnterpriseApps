using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseApps.Portable.DTO;
using System.Net.Http;
using Newtonsoft.Json;

namespace EnterpriseApps.Portable.Service
{
    public class HttpService : IHttpService
    {
        private readonly string _baseUrl;
        private const string _getUsersCall = "?results={0}"; // {0} = desired result count

        public HttpService(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<IEnumerable<Result>> GetUsersAsync(int count)
        {
            using (var httpClient = new HttpClient())
            {
                var requestUrl = _baseUrl + string.Format(_getUsersCall, count);
                var response = await httpClient.GetAsync(requestUrl);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(responseContent))
                {
                    var result = await Task.Run(() =>
                    {
                        return JsonConvert.DeserializeObject<RootObject>(responseContent);
                    });
                    return result.Results;
                }
                else
                {
                    throw new HttpServiceException();
                }
            }
        }
    }
}
