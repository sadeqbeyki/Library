using Identity.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
namespace Identity.Domain.Entities.User;

public class ApplicationUser : IdentityUser<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? BirthDate { get; set; }

    [AllowNull]
    public List<Token> Tokens { get; set; }

    //[AllowNull]
    //public string[] Permissions { get; set; }
    //public string AccessAllUser { get; set; }

}
