using Identity.Application.DTOs.User;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Query.User;

public record GetAllUsersQuery : IRequest<List<UserDetailsDto>>;

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDetailsDto>>
{
    private readonly IUserService _userService;

    public GetAllUsersQueryHandler(IUserService iUserService)
    {
        _userService = iUserService;
    }

    public async Task<List<UserDetailsDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        List<UserDetailsDto> users = await _userService.GetAllUsersAsync();

        foreach (var user in users)
        {
            user.Roles = await _userService.GetUserRolesAsync(user.Id);
        }
        return users;
    }
}