using Identity.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.Query.Email;

public record ConfirmEmailQuery(string token, string email) : IRequest<IdentityResult>;
public sealed class ConfirmEmailQueryHandler : IRequestHandler<ConfirmEmailQuery, IdentityResult>
{
    private readonly IUserService _userService;

    public ConfirmEmailQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IdentityResult> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
    {
        var emailConfirmToken = await _userService.ConfirmEmail(request.token, request.email);
        return emailConfirmToken;
    }
}

