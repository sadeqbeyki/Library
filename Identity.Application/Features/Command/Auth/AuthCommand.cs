﻿using Identity.Application.Common.Exceptions;
using Identity.Application.DTOs.Auth;
using Identity.Application.DTOs.User;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Features.Command.Auth;

public record AuthCommand(LoginUserDto dto) : IRequest<TokenAuthResponse>;

internal sealed class AuthCommandHandler : IRequestHandler<AuthCommand, TokenAuthResponse>
{
    private readonly IAuthService _authService;

    public AuthCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<TokenAuthResponse> Handle(AuthCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request.dto);
        return result ?? throw new BadRequestException("Invalid username or password");
    }
}
