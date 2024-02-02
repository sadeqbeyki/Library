namespace Identity.Application.DTOs.Auth;

public class TokenAuthRequest
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
