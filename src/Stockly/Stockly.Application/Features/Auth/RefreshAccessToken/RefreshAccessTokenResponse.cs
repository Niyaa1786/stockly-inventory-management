namespace Stockly.Application.Features.Auth.RefreshAccessToken;

public class RefreshAccessTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
