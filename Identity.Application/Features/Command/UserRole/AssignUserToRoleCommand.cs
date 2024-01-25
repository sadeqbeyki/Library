using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.UserRole;

public class AssignUserToRoleCommand : IRequest<bool>
{
    public string Username { get; set; }
    public IList<string> Roles { get; set; }
}

public sealed class AssignUserToRoleCommandHandler : IRequestHandler<AssignUserToRoleCommand, bool>
{
    private readonly IUserService _userService;

    public AssignUserToRoleCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(AssignUserToRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.AssignUserToRole(request.Username, request.Roles);
        return result;
    }
}

