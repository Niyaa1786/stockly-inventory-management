using System.Text.Json.Serialization;

namespace Stockly.Application.Features.Auth.ChangePassword;

public class ChangePasswordRequest
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
