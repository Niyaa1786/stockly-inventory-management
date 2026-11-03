namespace Stockly.Application.Features.Products.GetProducts;

public class GetProductsRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Keyword { get; set; }
    public Guid? CategoryId { get; set; }
    public bool IncludeInactive { get; set; }
}
