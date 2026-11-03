using Stockly.Domain.Exceptions;

namespace Stockly.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category? Category { get; private set; }
    public string Sku { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string Unit { get; private set; } = string.Empty;
    public int MinStockLevel { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Product() { }

    public Product(Category category, string sku, string name, string? description, string unit, int minStockLevel)
    {
        EnsureCategoryUsable(category);
        ValidateRules(sku, name, unit, minStockLevel);

        Id = Guid.NewGuid();
        CategoryId = category.Id;
        Sku = sku.Trim();
        Name = name.Trim();
        Description = description?.Trim();
        Unit = unit.Trim();
        MinStockLevel = minStockLevel;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    // Checks
    public bool CanBeUpdated => IsActive;
    public bool CanBeDeactivated => IsActive;
    public bool CanBeRestored => !IsActive;
    public bool CanBeUsedInStockMovement => IsActive;

    // Behaviour
    public void UpdateInfo(string sku, string name, string? description, string unit, int minStockLevel)
    {
        EnsureCanBeUpdated();
        ValidateRules(sku, name, unit, minStockLevel);

        Sku = sku.Trim();
        Name = name.Trim();
        Description = description?.Trim();
        Unit = unit.Trim();
        MinStockLevel = minStockLevel;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeCategory(Category category)
    {
        EnsureCanBeUpdated();
        EnsureCategoryUsable(category);

        CategoryId = category.Id;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        if (!CanBeDeactivated)
            throw new DomainException("Product is already deactivated.");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        if (!CanBeRestored)
            throw new DomainException("Product is already active.");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    // Guards / rules
    private void EnsureCanBeUpdated()
    {
        if (!CanBeUpdated)
            throw new DomainException("Cannot update a deactivated product.");
    }

    private static void EnsureCategoryUsable(Category category)
    {
        if (!category.CanBeAssignedToProduct)
            throw new DomainException("Cannot assign a deactivated category to a product.");
    }

    private static void ValidateRules(string sku, string name, string unit, int minStockLevel)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException("SKU is required.");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name is required.");

        if (string.IsNullOrWhiteSpace(unit))
            throw new DomainException("Unit is required.");

        if (minStockLevel < 0)
            throw new DomainException("Minimum stock level must be greater than or equal to 0.");
    }
}