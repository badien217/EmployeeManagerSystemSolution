using BaseLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Helper
{
    public class GetHttpClient(IHttpClientFactory httpClientFactory,LocalStorageServices localStorageServices)
    {
        private const string HeaderKey = "Authorization";
        public async Task<HttpClient> GetPrivateHttpClient()
        {
            var client = httpClientFactory.CreateClient("SystemsApiClient");
            var stringToken = await localStorageServices.GetToken();
            if (string.IsNullOrEmpty(stringToken)) return client;
            var deserializeToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
            if(deserializeToken == null ) return client;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",deserializeToken.Token);
            return client;
        }
        public HttpClient GetPublicHttpClient()
        {
            var client = httpClientFactory.CreateClient("SystemsApiClient");
            client.DefaultRequestHeaders.Remove(HeaderKey);
            return client;
        }
    }
}
