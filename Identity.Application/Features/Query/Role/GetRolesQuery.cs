using Identity.Application.DTOs;
using Identity.Application.DTOs.Role;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Query.Role;

public record GetRolesQuery : IRequest<List<RoleDto>>;

public sealed class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<RoleDto>>
{
    private readonly IRoleService _roleService;

    public GetRolesQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<List<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        List<RoleDto> roles = await _roleService.GetRolesAsync();
        return roles;
    }
}