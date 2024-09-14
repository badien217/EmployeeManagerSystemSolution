using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Reponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Reponsitories.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAsync(Register user);
        Task<LoginResponse> SignInAsync(Login user);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
    }
}
