using System.Text.Json.Serialization;

namespace Stockly.Application.Features.Categories.UpdateCategory;

public class UpdateCategoryRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
