using AutoMapper;
using LibIdentity.Domain.RoleAgg;
using LibIdentity.DomainContracts.RoleContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibIdentity.ApplicationServices;

public class RoleService : IRoleService
{
    private readonly RoleManager<RoleIdentity> _roleManager;

    private readonly IMapper _mapper;

    public RoleService(RoleManager<RoleIdentity> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }
    #region Get
    public async Task<RoleDto> GetRole(int id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        return _mapper.Map<RoleDto>(role);
    }
    #endregion

    #region GetAll
    public async Task<List<RoleDto>> GetRoles()
    {
        List<RoleIdentity> roles = await _roleManager.Roles.Take(50).ToListAsync();
        return _mapper.Map<List<RoleDto>>(roles);
    }
    #endregion

    #region Create
    public async Task<IdentityResult> CreateRole(RoleDto model)
    {
        var roleMap = _mapper.Map<RoleIdentity>(model);
        return await _roleManager.CreateAsync(roleMap);
    }
    #endregion

    #region Update
    public async Task<IdentityResult> UpdateRole(RoleDto role)
    {
        var result = await _roleManager.FindByIdAsync(role.Id.ToString());
        result.Name = role.Name;

        return await _roleManager.UpdateAsync(result);
    }
    #endregion

    #region Delete
    public async Task<IdentityResult> DeleteRole(RoleDto dto)
    {
        var role = await _roleManager.FindByIdAsync(dto.Id.ToString());
        var roleMap = _mapper.Map<RoleIdentity>(role);
        return await _roleManager.DeleteAsync(roleMap);
    }
    #endregion
}
