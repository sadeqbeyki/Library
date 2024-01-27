using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.UserRole;

public class UpdateUsersRoleCommand : IRequest<bool>
{
    public string Username { get; set; }
    public IList<string> Roles { get; set; }
}


public sealed class UpdateUsersRoleCommandHandler : IRequestHandler<UpdateUsersRoleCommand, bool>
{
    private readonly IUserService _userService;

    public UpdateUsersRoleCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(UpdateUsersRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateUsersRole(request.Username, request.Roles);
        return result;
    }
}

