using Identity.Application.DTOs.Role;

namespace Identity.Application.DTOs.User;

public class UserRolesDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}
