using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.UserRole;

public record IsInRoleCommand(string userId, string role) : IRequest<bool>;

public class IsInRoleCommandHandler : IRequestHandler<IsInRoleCommand, bool>
{
    private readonly IUserService _userService;

    public IsInRoleCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<bool> Handle(IsInRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.IsInRoleAsync(request.userId, request.role);
        return result;
    }
}

