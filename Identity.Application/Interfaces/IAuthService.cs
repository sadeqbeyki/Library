using Identity.Application.DTOs.Auth;
using Identity.Application.DTOs.User;
using Identity.Domain.Entities.User;
using System.Security.Claims;

namespace Identity.Application.Interfaces;

public interface IAuthService
{
    Task<AuthenticatedResponse> LoginAsync(LoginUserDto model);
    bool IsAuthenticated(ClaimsPrincipal user);
    string CurrentUserRole(ClaimsPrincipal user);
    Task<string> LogOutAsync(string returnUrl);
    Task<bool> Revoke(string Username);
    Task<AuthenticatedResponse> Refresh(TokenDto model);
}
