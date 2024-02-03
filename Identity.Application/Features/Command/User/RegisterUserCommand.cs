using Identity.Application.DTOs.User;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.User;

public record RegisterUserCommand(CreateUserDto dto) : IRequest<(Guid userId, string confirmEmailToken)>;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, (Guid userId, string confirmEmailToken)>
{
    private readonly IUserService _userService;
    public RegisterUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<(Guid userId, string confirmEmailToken)> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.Register(request.dto);
        return result;
    }
}