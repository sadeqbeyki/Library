using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities.Auth;

public class Token : IdentityUserToken<string>
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public Token(string id, string userName, string accessToken, string? refreshToken, DateTime refreshTokenExpiryTime)
    {
        UserId = id;
        Name = userName;
        Value = accessToken;
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
    }
}
