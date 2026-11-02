using Stockly.Application.Common.DTOs;
using Stockly.Domain.Enums;

namespace Stockly.Application.Features.Users.GetUserById;

public class GetUserByIdResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public Role Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}