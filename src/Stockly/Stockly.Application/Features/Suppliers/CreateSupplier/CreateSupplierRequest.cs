namespace Stockly.Application.Features.Suppliers.CreateSupplier;

public class CreateSupplierRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}
