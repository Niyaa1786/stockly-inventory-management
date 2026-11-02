using Stockly.Domain.Enums;
using Stockly.Domain.Exceptions;

namespace Stockly.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public Role Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private User() { }

    public User(string email, string passwordHash, string fullName, Role role)
    {
        ValidateProfile(email, fullName);
        ValidateRole(role);

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash is required.");

        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        FullName = fullName;
        Role = role;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public bool CanLogin => IsActive;
    public bool CanBeUpdated => IsActive;

    public void UpdateProfile(string email, string fullName)
    {
        EnsureCanBeUpdated();
        ValidateProfile(email, fullName);

        Email = email;
        FullName = fullName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignRole(Role role)
    {
        EnsureCanBeUpdated();
        ValidateRole(role);

        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string passwordHash)
    {
        EnsureCanBeUpdated();

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash is required.");

        PasswordHash = passwordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        if (!IsActive)
            throw new DomainException("User is already deactivated.");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    // Guards / rules
    private void EnsureCanBeUpdated()
    {
        if (!CanBeUpdated)
            throw new DomainException("Cannot update a deactivated user.");
    }

    private static void ValidateProfile(string email, string fullName)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required.");

        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name is required.");
    }

    private static void ValidateRole(Role role)
    {
        if (!Enum.IsDefined(role))
            throw new DomainException("Role is invalid.");
    }
}
