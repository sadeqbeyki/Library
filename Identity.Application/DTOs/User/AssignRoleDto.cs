namespace Identity.Application.DTOs.User;

public class AssignRoleDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    //public string RoleName { get; set; }
    //public string UserName { get; set; }
    //public IList<string> Roles { get; set; }
}