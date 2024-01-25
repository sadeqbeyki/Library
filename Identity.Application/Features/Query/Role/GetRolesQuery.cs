using Identity.Application.DTOs;
using Identity.Application.DTOs.Role;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Query.Role;

public record GetRolesQuery : IRequest<IEnumerable<RoleDto>>;

public sealed class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IRoleService _iroleService;

    public GetRolesQueryHandler(IRoleService iroleService)
    {
        _iroleService = iroleService;
    }

    public async Task<IEnumerable<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _iroleService.GetRolesAsync();
        return roles;
    }
}