using Identity.Application.DTOs.User;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.User.Queries;

public record GetUserDetailsQuery(string UserId) : IRequest<UserDetailsDto>;

public sealed class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsDto>
{
    private readonly IUserService _iUserService;

    public GetUserDetailsQueryHandler(IUserService iUserService)
    {
        _iUserService = iUserService;
    }

    public async Task<UserDetailsDto> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await _iUserService.GetUserByIdAsync(request.UserId, cancellationToken);
        return user;
    }
}