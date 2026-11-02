using Stockly.Application.Common.DTOs;
using Stockly.Domain.Enums;

namespace Stockly.Application.Features.Users.CreateUser;

public class CreateUserResponse
{
    public UserDto User { get; set; } = new();
    public string Message { get; set; } = "User created successfully.";
}
public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public Role Role { get; set; }
    public bool IsActive { get; set; }
}