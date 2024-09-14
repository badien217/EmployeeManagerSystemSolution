using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Reponses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using ServerLibrary.Reponsitories.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Reponsitories.Implementations
{
    public class UserAccountReponsitory(IOptions<JwtSection> config ,AppDbContext appDbContext) : IUserAccount
    {
        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            if (user is null)return new GeneralResponse(false, "model is empty");
            var checkUser = await FindUserByEmail(user.Email);
            if (checkUser != null) return new GeneralResponse(false, "User register Already");
            var applicationUser = await AddToDatabase(new ApplicationUser()
            {
                Name = user.Fullname,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            });
            var checkAdminRole = await appDbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Name!.Equals(Constrants.Admin));
            if(checkAdminRole is null)
            {
                var createAdminRole = await AddToDatabase(new SystemRole() { Name = Constrants.Admin });
                await AddToDatabase(new UserRole() { RoleId = createAdminRole.Id, UserId = applicationUser.Id });
                return new GeneralResponse(false, "Account created!");
            }
            var checkUserRole = await appDbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Name!.Equals(Constrants.User));
            SystemRole response = new();
            if (checkUserRole is null)
            {
                var createUserRole = await AddToDatabase(new SystemRole() { Name = Constrants.User });
                await AddToDatabase(new UserRole() { RoleId = createUserRole.Id, UserId = applicationUser.Id });

            }
            else
            {
                await AddToDatabase(new UserRole() { RoleId = checkUserRole.Id, UserId = applicationUser.Id });
            }
            return new GeneralResponse(false, "Account created!");
        }

        private async Task<T> AddToDatabase<T>(T model)
        {
            var result = appDbContext.Add(model);
            await appDbContext.SaveChangesAsync();
            return (T)result.Entity;
            
        }

        private async Task<ApplicationUser> FindUserByEmail(string? email) => await appDbContext.ApplicationUsers.FirstOrDefaultAsync(
            _=>_.Email!.ToLower()!.Equals(email.ToLower())
            );

        

        public async Task<LoginResponse> SignInAsync(Login user)
        {
            if(user is null) return new LoginResponse(false,"Model not empty");
            var applucationUser = await FindUserByEmail(user.Email);
            if (applucationUser is null) return new LoginResponse(false, "User not found");
            if (!BCrypt.Net.BCrypt.Verify(user.Password, applucationUser.Password)) return new LoginResponse(false, "Email/Password not valid");
            var getUserRole = await appDbContext.UserRoles.FirstOrDefaultAsync(_ => _.Id == applucationUser.Id);
            if (getUserRole is null) return new LoginResponse(false, "use role not found");
            var getRoleName = await appDbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Id == getUserRole.RoleId);
            if (getRoleName is null) return new LoginResponse(false, "use role not found");

            string jwtToken = GenerateToken(applucationUser, getRoleName!.Name);
            string refreshToken = GenerateRefreshToken();
            return new LoginResponse(true, "Login successfully", jwtToken, refreshToken);
        }
        private string GenerateToken(ApplicationUser user,string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!));
            var credentitals = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Email,user.Name),
                new Claim(ClaimTypes.Role,role)
            };
            var token = new JwtSecurityToken(
               issuer: config.Value.Issuer,
               audience: config.Value.Audience,
               claims: userClaims,
               expires: DateTime.Now.AddDays(1),
               signingCredentials: credentitals
               );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        private async Task<UserRole> FindUserRole(int userId) => await appDbContext.UserRoles.FirstOrDefaultAsync(_ => _.Id == userId);
        private async Task<SystemRole> FindRoleName(int roleId) => await appDbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Id == roleId);
        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
        {
            if (token is null) return new LoginResponse(false, "Model is empty");
            var findToken = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.Token!.Equals(token.Token));
            if (findToken is null) return new LoginResponse(false, "Refresh token is required");
            var user = await appDbContext.ApplicationUsers.FirstOrDefaultAsync(_ => _.Id == findToken.UserId);
            if (user is null) return new LoginResponse(false, "Refresh token could not generate because not found");
            var userRole = await FindUserRole(user.Id);
            var roleName = await FindRoleName(userRole.RoleId);
            string jwtToken = GenerateToken(user,roleName.Name);
            string refreshToken = GenerateRefreshToken();

            var updateRefreshToken = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.UserId == user.Id);
            if (updateRefreshToken is null) return new LoginResponse(false, "Refresh Token could not be generate because user has not sign in ");
            updateRefreshToken.Token = refreshToken;
            await appDbContext.SaveChangesAsync();
            return  new LoginResponse(true,"Token  refresh token successfully",jwtToken,refreshToken);



        }
    }
}
