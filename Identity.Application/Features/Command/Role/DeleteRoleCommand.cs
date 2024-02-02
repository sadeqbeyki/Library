using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.Role;

public record DeleteRoleCommand(Guid id) : IRequest<int>;


internal sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, int>
{
    private readonly IRoleService _roleService;

    public DeleteRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<int> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _roleService.DeleteRoleAsync(request.id);
        return result.Succeeded ? 1 : 0;
    }
}