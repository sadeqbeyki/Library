﻿using AutoMapper;
using Identity.Application.Common.Exceptions;
using Identity.Application.DTOs.Role;
using Identity.Application.Interfaces;
using Identity.Domain.Entities.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    private readonly IMapper _mapper;

    public RoleService(RoleManager<ApplicationRole> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }
    #region Get
    public async Task<RoleDto> GetRoleByIdAsync(Guid id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        var roleMap = _mapper.Map<RoleDto>(role);
        return roleMap;
    }
    #endregion

    #region GetAll
    public async Task<List<RoleDto>> GetRolesAsync()
    {
        List<ApplicationRole> roles = await _roleManager.Roles.ToListAsync();
        var rolesMap =  _mapper.Map<List<RoleDto>>(roles);
        return rolesMap;
    }
    #endregion

    #region Create
    public async Task<IdentityResult> CreateRoleAsync(RoleDto model)
    {
        var roleMap = _mapper.Map<ApplicationRole>(model);
        var result = await _roleManager.CreateAsync(roleMap);
        if (!result.Succeeded)
        {
            //var errors = result.Errors.Select(e => e.Description);
            //throw new ValidationException(result.Errors);
            throw new Exception("The chosen role has already been registered on the application");
        }
        return result;
    }
    #endregion

    #region Update
    public async Task<IdentityResult> UpdateRoleAsync(RoleDto dto)

    {
        if (dto.Name != null)
        {
            var role = await _roleManager.FindByIdAsync(dto.Id.ToString());
            role.Name = dto.Name;

            var result = await _roleManager.UpdateAsync(role);
            return result;
        }
        throw new Exception("unsuccessfull");

    }
    #endregion

    #region Delete
    public async Task<IdentityResult> DeleteRoleAsync(Guid id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString())
            ?? throw new NotFoundException("Role not found");

        if (role.Name == "Admin")
        {
            throw new BadRequestException("You can not delete Administrator Role");
        }
        var roleMap = _mapper.Map<ApplicationRole>(role);
        var result = await _roleManager.DeleteAsync(roleMap);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            //throw new ValidationException(string.Join("\n", errors));
            throw new ValidationException();
        }
        return result;

    }

    public async Task<RoleDto> GetRoleByNameAsync(string roleName)
    {
        ApplicationRole role = await _roleManager.FindByNameAsync(roleName)
            ?? throw new NotFoundException("not found role");
        return _mapper.Map<RoleDto>(role);
    }
    #endregion
}
