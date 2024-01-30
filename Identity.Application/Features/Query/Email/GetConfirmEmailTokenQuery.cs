using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Query.Email;

public record GetConfirmEmailTokenQuery(string Id) : IRequest<string>;
public sealed class GetConfirmEmailTokenQueryHandler : IRequestHandler<GetConfirmEmailTokenQuery, string>
{
    private readonly IUserService _userService;

    public GetConfirmEmailTokenQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(GetConfirmEmailTokenQuery request, CancellationToken cancellationToken)
    {
        var emailConfirmToken = await _userService.GetConfirmEmailToken(request.Id);
        return emailConfirmToken;
    }
}

