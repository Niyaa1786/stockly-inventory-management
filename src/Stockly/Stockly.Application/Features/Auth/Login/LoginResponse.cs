using Stockly.Domain.Enums;

namespace Stockly.Application.Features.Auth.Login;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiration { get; set; }
    public UserDto User { get; set; } = new();
    public string Message { get; set; } = "Login successful.";
}
public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public Role Role { get; set; }
    public bool IsActive { get; set; }
}
