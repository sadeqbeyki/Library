using Microsoft.AspNetCore.Identity;
namespace LibIdentity.Domain.UserAgg;

public class UserIdentity : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? BirthDate { get; set; }

}
