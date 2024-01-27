namespace Identity.Application.DTOs.User;

public class AssignRoleDto
{
    public string UserId { get; set; }
    public string RoleId { get; set; }

    //public string RoleName { get; set; }
    //public string UserName { get; set; }
    //public IList<string> Roles { get; set; }
}