using Identity.Application.DTOs.User;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.UserRole;

public record AssignUserToRoleCommand(AssignRoleDto Dto) : IRequest<string>;

public sealed class AssignUserToRoleCommandHandler : IRequestHandler<AssignUserToRoleCommand, string>
{
    private readonly IUserService _userService;

    public AssignUserToRoleCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(AssignUserToRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.AssignRoleAsync(request.Dto.UserId, request.Dto.RoleId);
        return result;
    }
}

