using Identity.Application.DTOs.Auth;
using Identity.Application.DTOs.User;
using System.Security.Claims;

namespace Identity.Application.Interfaces;

public interface IAuthService
{
    Task<TokenAuthResponse> LoginAsync(LoginUserDto model);
    bool IsAuthenticated(ClaimsPrincipal user);
    string CurrentUserRole(ClaimsPrincipal user);
    Task<string> LogOutAsync(string returnUrl);
    Task<bool> Revoke(string Username);
    Task<TokenAuthResponse> Refresh(TokenAuthRequest model);
}
