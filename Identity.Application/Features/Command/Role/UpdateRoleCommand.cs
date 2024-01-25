using Identity.Application.DTOs.Role;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.Role;

public record UpdateRoleCommand(RoleDto dto) : IRequest<int>;

public sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, int>
{
    private readonly IRoleService _roleService;

    public UpdateRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<int> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _roleService.UpdateRoleAsync(request.dto);
        return result.Succeeded ? 1 : 0;
    }
}
