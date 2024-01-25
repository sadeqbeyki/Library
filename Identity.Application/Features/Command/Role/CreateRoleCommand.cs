using Identity.Application.DTOs.Role;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.Role;

public record CreateRoleCommand(RoleDto dto) : IRequest<int>;


public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, int>
{
    private readonly IRoleService _userService;

    public CreateRoleCommandHandler(IRoleService userService)
    {
        _userService = userService;
    }

    public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.CreateRoleAsync(request.dto);
        return result.Succeeded ? 1 : 0;    
    }
}