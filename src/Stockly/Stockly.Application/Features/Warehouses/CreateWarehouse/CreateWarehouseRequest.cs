namespace Stockly.Application.Features.Warehouses.CreateWarehouse;

public class CreateWarehouseRequest
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Address { get; set; }
}
