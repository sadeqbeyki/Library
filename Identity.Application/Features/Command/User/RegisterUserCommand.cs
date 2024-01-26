using Identity.Application.DTOs.User;
using Identity.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.Command.User;

public record RegisterUserCommand(CreateUserDto dto) : IRequest<IdentityResult>;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
{
    private readonly IUserService _userService;
    public RegisterUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.Register(request.dto);
        return result;
    }
}