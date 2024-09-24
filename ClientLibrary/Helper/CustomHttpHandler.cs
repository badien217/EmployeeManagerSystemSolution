using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using ClientLibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Helper
{
    public class CustomHttpHandler(GetHttpClient getHttpClient,LocalStorageServices localStorageServices,IUserAccountServices accountServices) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool loginUrl = request.RequestUri!.AbsoluteUri.Contains("login");
            bool registerUrl = request.RequestUri!.AbsoluteUri.Contains("register");
            bool refreshTokenUrl = request.RequestUri!.AbsoluteUri.Contains(" RefreshToken");
            if (loginUrl || registerUrl || refreshTokenUrl) return await base.SendAsync(request, cancellationToken);
            var result = await base.SendAsync(request, cancellationToken);
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var stringToken = await localStorageServices.GetToken();
                if (stringToken == null) return result;
                string token = string.Empty;
                try { token = request.Headers.Authorization!.Parameter!; }
                catch { }
                var deserToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
                if (deserToken is null) return result;
                if (string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserToken.Token);
                    return await base.SendAsync(request, cancellationToken);
                }

                var newJwtToken = await GetReshToken(deserToken.RefreshToken);
                if (string.IsNullOrEmpty(newJwtToken)) return result;
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newJwtToken);
                return await base.SendAsync(request, cancellationToken);
            }
            return result;
        }

        private async Task<string> GetReshToken(string? refreshToken)
        {
            var result = await accountServices.RefreshTokenAsync(new RefreshToken() { Token = refreshToken });
            string serializeToken = Serializations.SerializzeObj(new UserSession()
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken
            });
            await localStorageServices.SetToken(serializeToken);
            return result.Token;
        }
    }
}
