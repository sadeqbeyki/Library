using Microsoft.AspNetCore.Identity;

namespace LibIdentity.Domain.UserAgg;

public class User : IdentityUser<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    //public DateTime? BirthDate { get; set; }
}
