using System.Text;

using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

using LibIdentity.Domain.UserAgg;
using LibIdentity.DomainContracts.Auth;
using Microsoft.Extensions.Configuration;


namespace LibIdentity.ApplicationServices;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService()
    {
    }

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(UserIdentity user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_configuration.GetValue<string>("Jwt:Secret"))), SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>("Jwt:Issuer"),

            audience: _configuration.GetValue<string>("Jwt:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public bool ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Secret"))),
            ValidAudience = _configuration.GetValue<string>("Jwt:Audience"),
            ValidIssuer = _configuration.GetValue<string>("Jwt:Issuer"),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
        }
        catch (SecurityTokenException)
        {
            return false;
        }

        return true;
    }


}
