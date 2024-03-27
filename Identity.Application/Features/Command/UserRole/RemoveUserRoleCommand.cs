using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.UserRole;

public class RemoveUserRoleCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}

public sealed class RemoveUserRoleCommandHandler : IRequestHandler<RemoveUserRoleCommand, bool>
{
    private readonly IUserService _userService;

    public RemoveUserRoleCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == null || request.RoleId == null)
            throw new Exception("cant find user or role");
        var result = await _userService.RemoveUserRole(request.UserId, request.RoleId);
        return result;
    }
}
