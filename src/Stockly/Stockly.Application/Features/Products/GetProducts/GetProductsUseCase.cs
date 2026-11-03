using FluentValidation;
using Stockly.Application.Common.Interfaces;

namespace Stockly.Application.Features.Products.GetProducts;

public class GetProductsUseCase(IUnitOfWork unitOfWork, IValidator<GetProductsRequest> validator)
    : IUseCase<GetProductsRequest, GetProductsResponse>
{
    public async Task<GetProductsResponse> ExecuteAsync(GetProductsRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var items = await unitOfWork.Products.GetPagedWithDetailsAsync(request.Keyword, request.CategoryId, request.IncludeInactive, request.Page, request.PageSize, ct);
        var total = await unitOfWork.Products.CountAsync(request.Keyword, request.CategoryId, request.IncludeInactive, ct);

        return new GetProductsResponse
        {
            Items = items.Select(p => new ProductDto
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? string.Empty,
                Sku = p.Sku,
                Name = p.Name,
                Description = p.Description,
                Unit = p.Unit,
                MinStockLevel = p.MinStockLevel,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList(),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
