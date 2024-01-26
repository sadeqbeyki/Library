namespace Identity.Application.DTOs.User;

public class UserRolesDto : CreateUserDto
{ 
    public string Id { get; set; }
    public List<string> RoleName { get; set; }
}
