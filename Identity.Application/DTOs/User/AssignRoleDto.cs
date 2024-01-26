namespace Identity.Application.DTOs.User;

public class AssignRoleDto
{
    public string Username { get; set; }
    public IList<string> Roles { get; set; }
}