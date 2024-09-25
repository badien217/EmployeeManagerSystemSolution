﻿using BaseLibrary.DTOs;
using BaseLibrary.Reponses;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Services.Contracts
{
    public interface IUserAccountServices
    {
        Task<GeneralResponse> CreateAsync(Register user);
        Task<LoginResponse> SignInAsync(Login user);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken user);
        Task<WeatherForecast[]> GetWeatherForecasts();
    }
}
