using Identity.Application.Common.Exceptions;
using Identity.Application.DTOs.User;
using LibIdentity.DomainContracts.Auth;
using MediatR;

namespace Identity.Application.Features.Command.Auth;

public record LoginCommand(LoginUserDto dto) : IRequest<string>;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request.dto);

        return result == null ? 
            throw new BadRequestException("Invalid username or password") 
            : result;
    }
}
