using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Products.GetProductById;

public class GetProductByIdUseCase(IUnitOfWork unitOfWork)
    : IUseCase<GetProductByIdRequest, GetProductByIdResponse>
{
    public async Task<GetProductByIdResponse> ExecuteAsync(GetProductByIdRequest request, CancellationToken ct = default)
    {
        var product = await unitOfWork.Products.GetByIdWithDetailsAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        return new GetProductByIdResponse
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
            Sku = product.Sku,
            Name = product.Name,
            Description = product.Description,
            Unit = product.Unit,
            MinStockLevel = product.MinStockLevel,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}
