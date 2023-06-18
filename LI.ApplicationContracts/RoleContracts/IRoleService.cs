using Microsoft.AspNetCore.Identity;

namespace LI.ApplicationContracts.RoleContracts
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRole(RoleDto model);
        Task<IdentityResult> DeleteRole(RoleDto dto);
        Task<RoleDto> GetRole(string id);
        Task<List<RoleDto>> GetRoles();
        Task<IdentityResult> UpdateRole(RoleDto user);
    }
}