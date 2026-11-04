namespace Stockly.Application.Features.Warehouses.GetWarehouses;

public class GetWarehousesRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Keyword { get; set; }
    public bool IncludeInactive { get; set; }
}
