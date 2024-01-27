using Identity.Application.DTOs.Role;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Query.Role;

public record GetRoleIdByNameQuery(string name) : IRequest<RoleDto>;

public sealed class GetRoleIdByNameQueryHandler : IRequestHandler<GetRoleIdByNameQuery, RoleDto>
{
    private readonly IRoleService _roleService;

    public GetRoleIdByNameQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<RoleDto> Handle(GetRoleIdByNameQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleService.GetRoleByNameAsync(request.name);
        return role;
    }
}