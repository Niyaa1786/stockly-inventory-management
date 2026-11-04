using Stockly.Domain.Exceptions;

namespace Stockly.Domain.Entities;

public class Warehouse
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Warehouse() { }

    public Warehouse(string name, string code, string? address)
    {
        ValidateRules(name, code);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Code = code.Trim();
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
    public void UpdateInfo(string name, string code, string? address)
    {
        EnsureCanBeUpdated();
        ValidateRules(name, code);

        Name = name.Trim();
        Code = code.Trim();
        Address = address?.Trim();
    }

    public void SoftDelete()
    {
        if (!CanBeDeactivated)
            throw new DomainException("Warehouse is already deactivated.");

        IsActive = false;
    }

    public void Restore()
    {
        if (!CanBeRestored)
            throw new DomainException("Warehouse is already active.");

        IsActive = true;
    }

    // Guards / rules
    private void EnsureCanBeUpdated()
    {
        if (!CanBeUpdated)
            throw new DomainException("Cannot update a deactivated warehouse.");
    }

    private static void ValidateRules(string name, string code)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Warehouse name is required.");

        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Warehouse code is required.");
    }
}