using Identity.Application.DTOs.User;
using Identity.Domain.Entities.User;
using System.Security.Claims;

namespace Identity.Application.Interfaces;

public interface IAuthService
{
    string ValidateToken(string token);

    //--
    Task<JwtTokenDto> SigninUserAsync(LoginUserDto request);
    Task<JwtTokenDto> GetJwtSecurityTokenAsync(ApplicationUser user);

    Task<string> LoginAsync(LoginUserDto model);
    bool IsAuthenticated(ClaimsPrincipal user);
    string CurrentUserRole(ClaimsPrincipal user);
    Task<string> LogOutAsync(string returnUrl);
}
