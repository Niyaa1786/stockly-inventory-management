using Stockly.Domain.Exceptions;

namespace Stockly.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Category() { }

    public Category(string name, string? description)
    {
        ValidateRules(name);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description?.Trim();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    // Checks
    public bool CanBeUpdated => IsActive;
    public bool CanBeDeactivated => IsActive;
    public bool CanBeRestored => !IsActive;
    public bool CanBeAssignedToProduct => IsActive;

    // Behaviour
    public void UpdateInfo(string name, string? description)
    {
        EnsureCanBeUpdated();
        ValidateRules(name);

        Name = name.Trim();
        Description = description;
    }

    public void SoftDelete()
    {
        if (!CanBeDeactivated)
            throw new DomainException("Category is already deactivated.");

        IsActive = false;
    }

    public void Restore()
    {
        if (!CanBeRestored)
            throw new DomainException("Category is already active.");

        IsActive = true;
    }

    // Guards / rules
    private void EnsureCanBeUpdated()
    {
        if (!CanBeUpdated)
            throw new DomainException("Cannot update a deactivated category.");
    }

    private static void ValidateRules(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name is required.");
    }
}
