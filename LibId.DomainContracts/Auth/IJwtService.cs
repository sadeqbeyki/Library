using LibIdentity.Domain.UserAgg;

namespace LibIdentity.DomainContracts.Auth;

public interface IJwtService
{
    string GenerateJwtToken(UserIdentity user);
    bool ValidateJwtToken(string token);
}
