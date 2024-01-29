using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities.Auth;

public class Token : IdentityUserToken<string>
{
    //public string TokenString { get; set; }
}
