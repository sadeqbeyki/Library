using Identity.Application.DTOs.Role;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRoleAsync(RoleDto model);
        Task<IdentityResult> DeleteRoleAsync(string id);
        Task<RoleDto> GetRoleByIdAsync(string id);
        Task<List<RoleDto>> GetRolesAsync();
        Task<IdentityResult> UpdateRoleAsync(RoleDto user);
    }
}
