using Identity.Application.DTOs.User;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.User;
public record UpdateUserCommand(UpdateUserDto dto) : IRequest<int>;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, int>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<int> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.UpdateUserAsync(request.dto);
        return user ? 1 : 0;
    }
}
