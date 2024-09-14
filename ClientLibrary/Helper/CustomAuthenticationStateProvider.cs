using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Helper
{
    public class CustomAuthenticationStateProvider(LocalStorageServices localStorageServices) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new(new ClaimsPrincipal());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var stringToken = await localStorageServices.GetToken();
            if (string.IsNullOrEmpty(stringToken)) return await Task.FromResult(new AuthenticationState(anonymous));
            var deserializeToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
            if(deserializeToken == null ) return await Task.FromResult(new AuthenticationState(anonymous));
            var getUserClaims = DecryptToken(deserializeToken.Token!);
            if(getUserClaims == null) return await Task.FromResult(new AuthenticationState(anonymous));
            var claimPrincipal = setClaimPrincipal(getUserClaims);
            return await Task.FromResult(new AuthenticationState(claimPrincipal));
        }
        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if(userSession.Token!= null || userSession.RefreshToken != null)
            {
                var serializeSession = Serializations.SerializzeObj(userSession);
                await localStorageServices.SetToken(serializeSession);
                var getUserClaims = DecryptToken(userSession.Token!);
                claimsPrincipal = setClaimPrincipal(getUserClaims);
            }
            else
            {
                await localStorageServices.RemoveToken();
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
        public static ClaimsPrincipal setClaimPrincipal(CustomUserClaims claims)
        {
            if (claims.Email is null) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier,claims.Id!),
                    new(ClaimTypes.Name,claims.name!),
                    new(ClaimTypes.Email,claims.Email!),
                    new(ClaimTypes.Role,claims.Role!),
                },"JwtAuth"));

        }
        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            if(string.IsNullOrEmpty(jwtToken)) return new CustomUserClaims();
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userId = token.Claims.FirstOrDefault(_=>_.Type == ClaimTypes.NameIdentifier);
            var name = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name);
            var email = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Email);
            var role = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Role);
            return new CustomUserClaims(userId!.Value!,name!.Value,email!.Value,role!.Value);

        }
    }
}
