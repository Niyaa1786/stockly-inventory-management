using System.Text.Json.Serialization;
using Stockly.Domain.Enums;

namespace Stockly.Application.Features.Users.UpdateUser;

public class UpdateUserRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public Role Role { get; set; }
}
