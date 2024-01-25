using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.User;

public record DeleteUserCommand(string Id) : IRequest<int>;

public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, int>
{
    private readonly IUserService _userService;

    public DeleteUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.DeleteUserAsync(request.Id);
        return result ? 1 : 0;
    }
}
