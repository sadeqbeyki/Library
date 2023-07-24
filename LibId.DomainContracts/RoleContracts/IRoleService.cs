using Microsoft.AspNetCore.Identity;

namespace LibIdentity.DomainContracts.RoleContracts
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRole(RoleDto model);
        Task<IdentityResult> DeleteRole(RoleDto dto);
        Task<RoleDto> GetRole(int id);
        Task<List<RoleDto>> GetRoles();
        Task<IdentityResult> UpdateRole(RoleDto user);
    }
}