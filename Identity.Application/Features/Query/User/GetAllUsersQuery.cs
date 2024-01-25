using Identity.Application.DTOs.User;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Query.User;

public record GetAllUsersQuery : IRequest<List<UserDetailsDto>>;

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDetailsDto>>
{
    private readonly IUserService _iUserService;

    public GetAllUsersQueryHandler(IUserService iUserService)
    {
        _iUserService = iUserService;
    }

    public async Task<List<UserDetailsDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _iUserService.GetAllUsersAsync();

        foreach (var user in users)
        {
            user.Roles = await _iUserService.GetUserRolesAsync(user.UserId);
        }
        return users;
    }
}