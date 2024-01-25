using Identity.Application.Common.Exceptions;
using Identity.Application.DTOs.User;
using LibIdentity.DomainContracts.Auth;
using MediatR;

namespace Identity.Application.Features.Command.Auth;

public record AuthCommand(LoginUserDto dto) : IRequest<JwtTokenDto>;

internal sealed class AuthCommandHandler : IRequestHandler<AuthCommand, JwtTokenDto>
{
    private readonly IAuthService _authService;

    public AuthCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<JwtTokenDto> Handle(AuthCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.SigninUserAsync(request.dto);

        return result == null ? throw new BadRequestException("Invalid username or password") : result;
    }
}
