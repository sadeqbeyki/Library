using Identity.Application.DTOs.User;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.UserRole;

public record AssignUserToRoleCommand(AssignRoleDto Dto) : IRequest<bool>;

public sealed class AssignUserToRoleCommandHandler : IRequestHandler<AssignUserToRoleCommand, bool>
{
    private readonly IUserService _userService;

    public AssignUserToRoleCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(AssignUserToRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.AssignUserToRole(request.Dto.Username, request.Dto.Roles);
        return result;
    }
}

