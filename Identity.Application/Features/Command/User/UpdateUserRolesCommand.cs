using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.User;

public class UpdateUserRolesCommand : IRequest<int>
{
    public string Username { get; set; }
    public IList<string> Roles { get; set; }
}

public sealed class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, int>
{
    private readonly IUserService _userService;

    public UpdateUserRolesCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<int> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateUsersRole(request.Username, request.Roles);
        return result ? 1 : 0;
    }
}