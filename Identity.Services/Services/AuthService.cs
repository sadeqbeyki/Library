using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using LibIdentity.DomainContracts.Auth;
using Microsoft.Extensions.Configuration;
using Identity.Domain.Entities.User;
using Identity.Application.Common.Exceptions;
using Identity.Application.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Identity.Services.Authorization.Const;
using Identity.Domain.Entities.Auth;

namespace Identity.Services.Services;

public class AuthService : ServiceBase<AuthService>, IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;


    public AuthService(IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<string> Login(LoginUserDto model)
    {
        ApplicationUser user = await _userManager.FindByNameAsync(model.Username)
            ?? await _userManager.FindByEmailAsync(model.Username)
            ?? throw new Exception($"No user found with this name: '{model.Username}'.");


        if (user == null && !await _userManager.CheckPasswordAsync(user, model.Password))
            throw new BadRequestException("نام کاربری یا پسورد اشتباه است");

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new BadRequestException("Login failed");


        var jwtToken = GenerateJWTAuthetication(user);
        var validateToken = ValidateToken(jwtToken);

        var response = new UnauthorizedResult();
        return jwtToken ?? response.ToString();
    }

    public async Task<string> LogOutAsync(string returnUrl)
    {
        await _signInManager.SignOutAsync();
        return returnUrl;
    }

    public string GenerateJWTAuthetication(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new (JwtHeaderParameterNames.Jku, user.UserName),
            new (JwtHeaderParameterNames.Kid, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Email, user.Email),
            //new ("AccessAllUser", user.AccessAllUser.ToString())
            //new (ClaimTypes.Role),
            //new (ClaimTypes.NameIdentifier, user.UserName),
        };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
        var expires = DateTime.Now.AddMinutes(2);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
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

    //-----------------------------------------------------

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


        var jwtToken = GenerateJWTAuth(model.RememberMe, user);
        var validateToken = ValidateToken(jwtToken);

        var response = new UnauthorizedResult();
        return jwtToken ?? response.ToString();
    }
    private string GenerateJWTAuth(bool isRememberMe, ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
        var claimsIdentity = new List<Claim>
            {
            new (ClaimTypes.NameIdentifier, user.Id),
            new (JwtHeaderParameterNames.Jku, user.UserName),
            new (JwtHeaderParameterNames.Kid, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Email, user.Email),
            };

        JwtSecurityToken jwtToken = new(
        
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claimsIdentity,
            expires: GetExpireDateTime(isRememberMe),
            signingCredentials: signingCredentials
        );
        var token =  tokenHandler.WriteToken(jwtToken);
        user.Tokens = new List<Token>()
        {
            new Token
            {
                UserId = user.Id,
                Name = user.UserName,
                Value = token
            }
        };
        //user.PasswordHash = null;
        //AuthenticationProperties authProperties = new() { ExpiresUtc = GetExpireDateTime(isRememberMe) };
        //_httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
        //    new ClaimsPrincipal(claimsIdentity)
        //    ,authProperties);
        return token;

    }

    private static DateTime GetExpireDateTime(bool rememberMe)
    {
        return rememberMe
            ? DateTime.UtcNow.AddDays(30)
            : DateTime.UtcNow.AddDays(1);
    }

    private static ClaimsIdentity GetClaimsIdentity(ApplicationUser user)
    {
        List<Claim> claims = new()
        {
            new (JwtHeaderParameterNames.Jku, user.UserName),
            new (JwtHeaderParameterNames.Kid, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Email, user.Email),
            new (AuthorizePermissionConsts.User.UserAccess, "1,2,3,4,5")
        };
        return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    }

}
