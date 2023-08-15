namespace LibIdentity.DomainContracts.UserContracts;

public class UserWithRolesViewModel : UserDto
{
    public int Id { get; set; }
    public List<string> RoleName { get; set; }
}
