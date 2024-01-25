using Microsoft.AspNetCore.Identity;
namespace Identity.Domain.Entities.User;

public class ApplicationUser : IdentityUser<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? BirthDate { get; set; }

}
