using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Controllers;

public class AuthController : Controller
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }
    //private string CreateToken(UserIdentity user)
    //{
    //    List<Claim> claims = new() { new Claim(ClaimTypes.Name, user.UserName) };

    //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
    //        _configuration.GetSection("AppSettings:Token").Value!));

    //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

    //    var token = new JwtSecurityToken
    //        (
    //            claims: claims,
    //            expires: DateTime.Now.AddDays(1),
    //            signingCredentials: creds
    //        );

    //    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

    //    return jwt;
    //}
}
