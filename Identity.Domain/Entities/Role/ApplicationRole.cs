using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities.Role;

public class ApplicationRole : IdentityRole<Guid>
{
    public string Name { get; set; }
}
