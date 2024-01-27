using Identity.Application.DTOs.Role;
using Identity.Application.DTOs.User;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class UserRoleViewModel
{
    public List<UserDetailsDto> Users { get; set; }
    public List<RoleDto> Roles { get; set; }
    public List<UserRolesDto> UserRoles { get; set; }

    public AssignRoleDto Assign { get; set; }

    public string UserId { get; set; }
    public string UserName { get; set; }
    public string RoleId { get; set; }
    public string RoleName { get; set; }
}

