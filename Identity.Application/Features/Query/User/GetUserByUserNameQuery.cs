using Identity.Application.DTOs.User;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Query.User;

public record GetUserByUserNameQuery(string Username) : IRequest<UserDetailsDto>;

public sealed class GetUserByUserNameQueryHandler : IRequestHandler<GetUserByUserNameQuery, UserDetailsDto>
{
    private readonly IUserService _iUserService;

    public GetUserByUserNameQueryHandler(IUserService iUserService)
    {
        _iUserService = iUserService;
    }

    public async Task<UserDetailsDto> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
    {
        UserDetailsDto model = await _iUserService.GetUserByUserNameAsync(request.Username);
        return model;
    }
}