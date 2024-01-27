using Identity.Application.DTOs.User;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Query.User;

public record GetUserWithRolesQuery : IRequest<List<UserRolesDto>>;

public sealed class GetUserWithRolesQueryHandler : IRequestHandler<GetUserWithRolesQuery, List<UserRolesDto>>
{
    private readonly IUserService _userService;

    public GetUserWithRolesQueryHandler(IUserService iUserService)
    {
        _userService = iUserService;
    }

    public async Task<List<UserRolesDto>> Handle(GetUserWithRolesQuery request, CancellationToken cancellationToken)
    {
        List<UserRolesDto> users = await _userService.GetUserWithRoles();

        return users.Where(x => x.Roles.Count > 1).ToList();
    }
}