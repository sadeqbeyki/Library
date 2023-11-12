using AutoMapper;
using LibIdentity.Domain.UserAgg;

namespace LibIdentity.DomainContracts.Auth;

public interface IJwtService
{
    string GenerateJWTAuthetication(UserIdentity user);
    string ValidateToken(string token);
    string GenerateJwtToken(UserIdentity user);
}
