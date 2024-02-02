using Identity.Application.DTOs.Auth;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.Auth;

public record RefreshTokenCommand(TokenAuthRequest dto) : IRequest<TokenAuthResponse>;

internal sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenAuthResponse>
{
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<TokenAuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.Refresh(request.dto);

        return result;
    }
}
