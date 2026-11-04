using Stockly.Domain.Exceptions;

namespace Stockly.Domain.Entities;

public class Supplier
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public string? Address { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Supplier() { }

    public Supplier(string name, string? phone, string? email, string? address)
    {
        ValidateRules(name);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Phone = phone?.Trim();
        Email = email?.Trim();
        Address = address?.Trim();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    // Checks
    public bool CanBeUpdated => IsActive;
    public bool CanBeDeactivated => IsActive;
    public bool CanBeRestored => !IsActive;
    public bool CanBeUsedInStockMovement => IsActive;

    // Behaviours
    public void UpdateInfo(string name, string? phone, string? email, string? address)
    {
        EnsureCanBeUpdated();
        ValidateRules(name);

        Name = name.Trim();
        Phone = phone?.Trim();
        Email = email?.Trim();
        Address = address?.Trim();
    }

    public void SoftDelete()
    {
        if (!CanBeDeactivated)
            throw new DomainException("Supplier is already deactivated.");

        IsActive = false;
    }

    public void Restore()
    {
        if (!CanBeRestored)
            throw new DomainException("Supplier is already active.");

        IsActive = true;
    }

    // Guards / rules
    private void EnsureCanBeUpdated()
    {
        if (!CanBeUpdated)
            throw new DomainException("Cannot update a deactivated supplier.");
    }

    private static void ValidateRules(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Supplier name is required.");
    }
}