using LibIdentity.DomainContracts.RoleContracts;
using LibIdentity.DomainContracts.UserContracts;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class UserRoleViewModel
{
    public List<UserWithRolesViewModel> Users { get; set; }
    public List<RoleDto> Roles { get; set; }

    public int UserId { get; set; }
    public string UserName { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
}

