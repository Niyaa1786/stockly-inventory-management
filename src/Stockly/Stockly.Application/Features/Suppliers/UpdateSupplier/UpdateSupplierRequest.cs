using System.Text.Json.Serialization;

namespace Stockly.Application.Features.Suppliers.UpdateSupplier;

public class UpdateSupplierRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}
