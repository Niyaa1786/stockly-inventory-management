using Stockly.Application.Common.DTOs;
using Stockly.Domain.Enums;

namespace Stockly.Application.Features.Users.UpdateUser;

public class UpdateUserResponse
{
    public UserDto User { get; set; } = new();
    public string Message { get; set; } = "User updated successfully.";
}
public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public Role Role { get; set; }
    public bool IsActive { get; set; }
}