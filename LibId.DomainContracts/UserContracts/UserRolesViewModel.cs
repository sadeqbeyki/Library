namespace LibIdentity.DomainContracts.UserContracts;

public class UserRolesViewModel : CreateUserViewModel
{
    public int Id { get; set; }
    public List<string> RoleName { get; set; }
}
