using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Reponses;
using ClientLibrary.Helper;
using ClientLibrary.Services.Contracts;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Services.Implementations
{
    public class UserAccountServices(GetHttpClient getHttpClient) : IUserAccountServices
    {
        public const string AuthUrl = "api/Authentication";

        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/Register", user);
            if (!result.IsSuccessStatusCode) return new GeneralResponse(false, "Error occured");
            return await result.Content.ReadFromJsonAsync<GeneralResponse>()!;
        }
        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/RefreshToken", user);
            if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error occured");
            return await result.Content.ReadFromJsonAsync<LoginResponse>()!;
        }

        public async Task<LoginResponse> SignInAsync(Login user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/Login", user);
            if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error occured");
            return await result.Content.ReadFromJsonAsync<LoginResponse>();
        }
        public async Task<WeatherForecast[]> GetWeatherForecasts()
        {
            var httpClient = await getHttpClient.GetPrivateHttpClient();
            var result = await httpClient.GetFromJsonAsync<WeatherForecast[]>("api/weatherforecast");
            return result!;
        }
    }
}
