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

namespace Identity.Services.Services;

public class AuthService : ServiceBase<AuthService>, IAuthService
{
    private readonly IConfiguration _configuration;
    private new readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;


    public AuthService(IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }


    public async Task<JwtTokenDto> SigninUserAsync(LoginUserDto request)
    {
        var user = await _userManager.FindByNameAsync(request.Username) ?? throw new BadRequestException("User Doesn't exist!");

        if (user.UserName == request.Username)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, true, false);
            if (result.Succeeded)
            {
                JwtTokenDto token = await GetJwtSecurityTokenAsync(user);

                return token;
            }
            throw new NotFoundException("user is not allowed");
        }
        throw new BadRequestException("User Not Found!");
    }

    /// <summary>
    /// Generate JWT token for user
    /// </summary>
    /// <param name="user">User entity for which token will be generate</param>
    /// <returns>Generated token details including expire data</returns>
    public async Task<JwtTokenDto> GetJwtSecurityTokenAsync(ApplicationUser user)
    {
        // Obtain existing claims, Here we will obtain last 4 JTI claims only
        // As We only maintain login for 5 maximum sessions, So need to remove other from that
        var allClaims = await _userManager.GetClaimsAsync(user);
        List<Claim> toRemoveClaims = new();
        var allJtiClaims = allClaims.Where(claim => claim.Type.Equals(JwtRegisteredClaimNames.Jti)).ToList();
        if (allJtiClaims.Count > 4)
        {
            toRemoveClaims = allJtiClaims.SkipLast(4).ToList();
            allJtiClaims = allJtiClaims.TakeLast(4).ToList();
        }

        var secretKey = _configuration["JwtIssuerOptions:SecretKey"];

        SigningCredentials credentials = new(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256);

        DateTime tokenExpireOn = DateTime.Now.AddHours(3);
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?
            .Equals("Development", StringComparison.InvariantCultureIgnoreCase) == true)
        {
            // If its development then set 3 years as token expiry for testing purpose
            tokenExpireOn = DateTime.Now.AddYears(3);
        }

        //string roles = string.Join("; ", await _userManager.GetRolesAsync(user));

        // Obtain Role of User
        IList<string> rolesOfUser = await _userManager.GetRolesAsync(user);

        // Add new claims
        List<Claim> tokenClaims = new()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new(JwtRegisteredClaimNames.Sub, user.UserName),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Role, rolesOfUser.FirstOrDefault() ?? "Member"),
                new(ClaimTypes.Name, user.PhoneNumber)

                //new Claim("user_role", "Admin")
            };


        // Make JWT token
        JwtSecurityToken token = new(
            issuer: _configuration["JwtIssuerOptions:Issuer"],
            audience: _configuration["JwtIssuerOptions:Audience"],
            claims: tokenClaims.Union(allJtiClaims),
            expires: tokenExpireOn,
            signingCredentials: credentials
        );


        // Update claim details
        await _userManager.RemoveClaimsAsync(user, toRemoveClaims);
        await _userManager.AddClaimsAsync(user, tokenClaims);

        // Return it
        JwtTokenDto generatedToken = new()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpireOn = tokenExpireOn,
            User = user
        };

        return generatedToken;
    }

    //-----------------------------------------------------
    public async Task<string> LoginAsync(LoginUserDto model)
    {
        ApplicationUser user = await _userManager.FindByNameAsync(model.Username)
            ?? await _userManager.FindByEmailAsync(model.Username)
            ?? throw new Exception($"No user found with this name: '{model.Username}'.");

        if (user == null && !await _userManager.CheckPasswordAsync(user, model.Password))
            throw new BadRequestException("نام کاربری یا پسورد اشتباه است");


        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new BadRequestException("Login failed");


        var jwtToken = await GenerateJWTAuthetication(model.RememberMe, user);
        var validateToken = ValidateToken(jwtToken);

        var response = new UnauthorizedResult();
        return jwtToken ?? response.ToString();
    }

    public string ValidateToken(string token)
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


    private async Task<string> GenerateJWTAuthetication(bool isRememberMe, ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = await GetClaimsIdentity(user);
        JwtSecurityToken jwtToken = new(

            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: GetExpireDateTime(isRememberMe),
            signingCredentials: signingCredentials
        );
        var token = tokenHandler.WriteToken(jwtToken);

        SaveToken(user, token);

        return token;
    }

    private static DateTime GetExpireDateTime(bool rememberMe)
    {
        return rememberMe
            ? DateTime.UtcNow.AddDays(30)
            : DateTime.UtcNow.AddDays(1);
    }

    private async Task<List<Claim>> GetClaimsIdentity(ApplicationUser user)
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

    private void SaveToken(ApplicationUser user, string token)
    {
        user.Tokens = new List<Token>()
        {
            new()
            {
                UserId = user.Id,
                Name = user.UserName,
                Value = token
            }
        };
        _userManager.UpdateAsync(user);
    }
}
