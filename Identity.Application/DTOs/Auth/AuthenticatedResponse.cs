namespace Identity.Application.DTOs.Auth;

public class AuthenticatedResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
