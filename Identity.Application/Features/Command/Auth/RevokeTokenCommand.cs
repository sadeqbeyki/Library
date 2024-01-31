using Identity.Application.Common.Exceptions;
using Identity.Application.DTOs.User;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.Auth;

public record RevokeTokenCommand(string userName) : IRequest<bool>;

internal sealed class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, bool>
{
    private readonly IAuthService _authService;

    public RevokeTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var result =await _authService.Revoke(request.userName);

        return result;
    }
}
