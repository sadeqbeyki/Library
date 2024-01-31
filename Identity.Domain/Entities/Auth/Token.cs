using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities.Auth;

public class Token : IdentityUserToken<string>
{
    public int Id { get; set; }
    
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public Token(string userId, string userName, string accessToken, string? refreshToken, DateTime refreshTokenExpiryTime)
    {
        UserId = userId;
        UserName = userName;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
    }
}
