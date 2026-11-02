namespace Stockly.Application.Common.DTOs;

public class TokenResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiration { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}
