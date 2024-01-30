﻿
using Identity.Application.DTOs.User;
using Identity.Domain.Entities.User;
using System.Security.Claims;

namespace LibIdentity.DomainContracts.Auth;

public interface IAuthService
{
    string GenerateJWTAuthetication(ApplicationUser user);
    string ValidateToken(string token);

    //--
    Task<JwtTokenDto> SigninUserAsync(LoginUserDto request);
    Task<string> Login(LoginUserDto model);
    Task<string> LogOutAsync(string returnUrl);

    Task<JwtTokenDto> GetJwtSecurityTokenAsync(ApplicationUser user);
    Task<string> LoginAsync(LoginUserDto model);
    bool IsAuthenticated(ClaimsPrincipal user);
}
