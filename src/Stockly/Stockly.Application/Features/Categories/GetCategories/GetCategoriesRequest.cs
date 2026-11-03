namespace Stockly.Application.Features.Categories.GetCategories;

public class GetCategoriesRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Keyword { get; set; }
    public bool IncludeInactive { get; set; }
}
