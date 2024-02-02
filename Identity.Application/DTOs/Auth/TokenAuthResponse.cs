namespace Identity.Application.DTOs.Auth;

public class TokenAuthResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
