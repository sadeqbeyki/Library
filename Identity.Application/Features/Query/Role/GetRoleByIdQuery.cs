using Identity.Application.DTOs;
using Identity.Application.DTOs.Role;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Query.Role;

public record GetRoleByIdQuery(string Id) : IRequest<RoleDto>;
public sealed class GetRoleQueryByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
{
    private readonly IRoleService _roleService;

    public GetRoleQueryByIdHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleService.GetRoleByIdAsync(request.Id);
        return role;
    }
}

