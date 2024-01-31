using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Identity.Domain.Entities.User;
using Identity.Application.Common.Exceptions;
using Identity.Application.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Identity.Domain.Entities.Auth;
using Identity.Application.Interfaces;
using System.Security.Cryptography;
using Identity.Application.DTOs.Auth;
using Azure.Core;
using Identity.Persistance;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services.Services;

public class AuthService : ServiceBase<AuthService>, IAuthService
{
    private readonly IConfiguration _configuration;
    private new readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    protected readonly AppIdentityDbContext _appIdentityDbContext;



    public AuthService(IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IServiceProvider serviceProvider,
        AppIdentityDbContext appIdentityDbContext) : base(serviceProvider)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
        _appIdentityDbContext = appIdentityDbContext;
    }
    public async Task<AuthenticatedResponse> LoginAsync(LoginUserDto model)
    {
        ApplicationUser user = await _userManager.FindByNameAsync(model.Username)
            ?? await _userManager.FindByEmailAsync(model.Username)
            ?? throw new Exception($"No user found with this name: '{model.Username}'.");

        if (user == null && !await _userManager.CheckPasswordAsync(user, model.Password))
            throw new BadRequestException("Check password failed!");

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new BadRequestException("Login failed");


        var claims = await GetClaimsIdentity(user);
        AuthenticatedResponse response = new()
        {
            AccessToken = await GenerateAccessToken(model.RememberMe, claims),
            RefreshToken = GenerateRefreshToken()
        };

        SaveToken(user.Id, user.UserName, response.AccessToken, response.RefreshToken);

        return response;
    }

    public bool IsAuthenticated(ClaimsPrincipal user)
    {
        return _signInManager.IsSignedIn(user);
        //    var claims = _httpContextAccessor.HttpContext?.User?.Claims.ToList();
        //    return claims.Count > 0; 
    }

    public string CurrentUserRole(ClaimsPrincipal user)
    {
        if (IsAuthenticated(user))
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        return null;
    }

    public async Task<string> LogOutAsync(string returnUrl)
    {
        await _signInManager.SignOutAsync();
        return returnUrl;
    }

    #region Token Section
    //access token
    private async Task<string> GenerateAccessToken(bool isRememberMe, IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken jwtToken = new(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: GetExpireDateTime(isRememberMe),
            signingCredentials: signingCredentials
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        var validateToken = ValidateToken(accessToken);

        return accessToken;
    }

    private static DateTime GetExpireDateTime(bool rememberMe)
    {
        return rememberMe
            ? DateTime.UtcNow.AddDays(30)
            : DateTime.UtcNow.AddDays(1);
    }

    private async Task<IEnumerable<Claim>> GetClaimsIdentity(ApplicationUser user)
    {
        IList<string> rolesOfUser = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>();
        foreach (var role in rolesOfUser)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        }
        claims.Add(new(ClaimTypes.NameIdentifier, user.Id));
        claims.Add(new(JwtHeaderParameterNames.Jku, user.UserName));
        claims.Add(new(JwtHeaderParameterNames.Kid, Guid.NewGuid().ToString()));
        claims.Add(new(JwtRegisteredClaimNames.Email, user.Email));
        //claims.Add(new(AuthorizePermissionConsts.User.UserAccess, "1,2,3,4,5"));

        return claims;
        //return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private void SaveToken(string id, string userName, string accessToken, string refreshToken)
    {
        Token token = new(id, userName, accessToken, refreshToken, DateTime.Now.AddDays(7));
        _appIdentityDbContext.Set<Token>().AddAsync(token);
        _appIdentityDbContext.SaveChanges();
    }

    //refresh token
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public async Task<AuthenticatedResponse> Refresh(TokenDto model)
    {
        bool isRmemberMe = true;
        var principal = GetPrincipalFromExpiredToken(model.AccessToken);
        var username = principal.Identity.Name; //this is mapped to the Name claim by default
        var userToken = _appIdentityDbContext.UserTokens.SingleOrDefault(u => u.Name == username);
        if (userToken is null || userToken.RefreshToken != model.RefreshToken || userToken.RefreshTokenExpiryTime <= DateTime.Now)
            throw new BadRequestException("Invalid client request");

        var newAccessToken = await GenerateAccessToken(isRmemberMe, principal.Claims);
        var newRefreshToken = GenerateRefreshToken();
        SaveToken(userToken.UserId, username, newAccessToken, newRefreshToken);
        AuthenticatedResponse response = new()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
        };
        return response;
    }

    public async Task<bool> Revoke(string Username)
    {
        var userToken = await _appIdentityDbContext.UserTokens.SingleOrDefaultAsync(u => u.Name == Username)
            ?? throw new Exception("cant find user");
        userToken.RefreshToken = null;
        _appIdentityDbContext.SaveChanges();
        return true;
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
            ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    //validate token
    private string ValidateToken(string token)
    {
        if (token == null)
            return "Invalid token: Token is null.";

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")));

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero

            }, out SecurityToken validatedToken);

            // Corrected access to the validatedToken
            var jwtToken = (JwtSecurityToken)validatedToken;
            var jku = jwtToken.Claims.First(claim => claim.Type == "jku").Value;
            var kid = jwtToken.Claims.First(claim => claim.Type == "kid").Value;

            return kid;
        }
        catch (Exception ex)
        {
            return $"Invalid token: {ex.Message}";
        }
    }

    #endregion

}
