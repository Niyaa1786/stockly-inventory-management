using System.Text.Json.Serialization;

namespace Stockly.Application.Features.Users.DeactivateUser;

public class DeactivateUserRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }
    [JsonIgnore]
    public Guid CurrentUserId { get; set; }
}
