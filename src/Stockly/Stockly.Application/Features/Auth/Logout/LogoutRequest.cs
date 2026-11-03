using System.Text.Json.Serialization;

namespace Stockly.Application.Features.Auth.Logout;

public class LogoutRequest
{
    [JsonIgnore]
    public Guid UserId { get; set; }
}
