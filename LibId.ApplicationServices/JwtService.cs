using System.Text;

using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

using LibIdentity.Domain.UserAgg;
using LibIdentity.DomainContracts.Auth;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using AppFramework.Infrastructure;

namespace LibIdentity.ApplicationServices;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJWTAuthetication(UserIdentity user)
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
    //public string GenerateJwtToken(UserIdentity user)
    //{

    //        var subject = new ClaimsIdentity(new[]
    //          {
    //            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
    //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //            new Claim(ClaimTypes.NameIdentifier, user.UserName),
    //            new Claim(JwtRegisteredClaimNames.Email, user.Email)
    //          });


    //roles.ForEach(role =>
    //{
    //    claims.Add(new Claim(ClaimTypes.Role, role));
    //});

    //var tokenDescriptor = new SecurityTokenDescriptor
    //{
    //    Subject = subject,
    //    Expires = expires,
    //    Issuer = _configuration["Jwt:Issuer"],
    //    Audience = _configuration["Jwt:Audience"],
    //    SigningCredentials = signingCredentials
    //};

    //var tokenHandler = new JwtSecurityTokenHandler();
    //var token = tokenHandler.CreateToken(tokenDescriptor);
    //var jwtToken = tokenHandler.WriteToken(token);
    //return jwtToken;
    //}
}
