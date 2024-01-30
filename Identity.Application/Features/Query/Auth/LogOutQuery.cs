using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Query.Auth;

public record LogOutQuery(string returnUrl) : IRequest<string>;
public sealed class LogOutQueryHandler : IRequestHandler<LogOutQuery, string>
{
    private readonly IAuthService _authService;

    public LogOutQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<string> Handle(LogOutQuery request, CancellationToken cancellationToken)
    {
        var role = await _authService.LogOutAsync(request.returnUrl);
        return role;
    }
}
