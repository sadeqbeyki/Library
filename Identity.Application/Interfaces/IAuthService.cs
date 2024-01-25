
using Identity.Application.DTOs.User;
using Identity.Domain.Entities.User;

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
}
