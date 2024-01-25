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
using System.Security.Policy;

namespace Identity.Services.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IAuthService _authService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;


    public AuthService(IConfiguration configuration, IAuthService authService,
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _configuration = configuration;
        _authService = authService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public string GenerateJWTAuthetication(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtHeaderParameterNames.Jku, user.UserName),
            new Claim(JwtHeaderParameterNames.Kid, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
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

    //--

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

    public async Task<string> Login(LoginUserDto model)
    {
        ApplicationUser user = await _userManager.FindByNameAsync(model.Username)
            ?? await _userManager.FindByEmailAsync(model.Username)
            ?? throw new Exception("Fill in the blank fields!");


        if (user == null && !await _userManager.CheckPasswordAsync(user, model.Password))
            throw new BadRequestException("نام کاربری یا پسورد اشتباه است");

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new BadRequestException("Login failed");

        var jwtToken = _authService.GenerateJWTAuthetication(user);
        var validateToken = _authService.ValidateToken(jwtToken);

        var response = new UnauthorizedResult();
        return jwtToken ?? response.ToString();
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
        var toRemoveClaims = new List<Claim>();
        var allJtiClaims = allClaims.Where(claim => claim.Type.Equals(JwtRegisteredClaimNames.Jti)).ToList();
        if (allJtiClaims.Count > 4)
        {
            toRemoveClaims = allJtiClaims.SkipLast(4).ToList();
            allJtiClaims = allJtiClaims.TakeLast(4).ToList();
        }

        var secretKey = _configuration["JwtIssuerOptions:SecretKey"];

        SigningCredentials credentials = new(new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256);

        DateTime tokenExpireOn = DateTime.Now.AddHours(3);
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?
            .Equals("Development", StringComparison.InvariantCultureIgnoreCase) == true)
        {
            // If its development then set 3 years as token expiry for testing purpose
            tokenExpireOn = DateTime.Now.AddYears(3);
        }

        string roles = string.Join("; ", await _userManager.GetRolesAsync(user));

        // Obtain Role of User
        IList<string> rolesOfUser = await _userManager.GetRolesAsync(user);

        // Add new claims
        string userName = user.UserName ?? throw new NotFoundException("cant find user");
        List<Claim> tokenClaims = new()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new(JwtRegisteredClaimNames.Sub, user.UserName),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Role, rolesOfUser.FirstOrDefault() ?? "Member"),
                new(ClaimTypes.Name, user.PhoneNumber)

                //new Claim("user_role", "admin")
            };


        //adedd-------------------------------------------------------------------------

        //var resultB = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, resultB.Principal);
        //bool resultA =ClaimsPrincipal.Current.Identity.IsAuthenticated;
        //var claim = new Claim(user.UserName, user.PasswordHash);
        //var identity = new ClaimsIdentity(new[] { claim }, "BasicAuthentication"); // this uses basic auth
        //var principal = new ClaimsPrincipal(identity);
        //bool resultB = ClaimsPrincipal.Current.Identity.IsAuthenticated;
        //adedd-------------------------------------------------------------------------


        // Make JWT token
        JwtSecurityToken token = new(
            issuer: _configuration["JwtIssuerOptions:Issuer"],
            audience: _configuration["JwtIssuerOptions:Audience"],
            claims: tokenClaims.Union(allJtiClaims),
            expires: tokenExpireOn,
            signingCredentials: credentials
        );

        // Set current user details for busines & common library
        string userEmail = user.Email ?? throw new NotFoundException("The email value cannot be empty");
        var currentUser = await _userManager.FindByEmailAsync(user.Email) ?? throw new NotFoundException("No user found with this email");

        // Update claim details
        await _userManager.RemoveClaimsAsync(currentUser, toRemoveClaims);
        /*var claims =*/
        await _userManager.AddClaimsAsync(currentUser, tokenClaims);

        // Return it
        JwtTokenDto generatedToken = new()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpireOn = tokenExpireOn,
            User = currentUser
        };

        return generatedToken;
    }

}
