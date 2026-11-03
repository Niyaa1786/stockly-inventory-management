using FluentValidation;
using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Products.CreateProduct;

public class CreateProductUseCase(IUnitOfWork unitOfWork, IValidator<CreateProductRequest> validator)
    : IUseCase<CreateProductRequest, CreateProductResponse>
{
    public async Task<CreateProductResponse> ExecuteAsync(CreateProductRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var sku = request.Sku.Trim();
        if (await unitOfWork.Products.IsSkuTakenAsync(sku, null, ct))
            throw new AppValidationException(nameof(request.Sku), "SKU already exists.");

        var category = await unitOfWork.Categories.GetByIdAsync(request.CategoryId, ct)
            ?? throw new NotFoundException(nameof(Category), request.CategoryId);

        if (!category.CanBeAssignedToProduct)
            throw new AppValidationException(nameof(request.CategoryId), "Category is deactivated.");

        var product = new Product(category, sku, request.Name, request.Description, request.Unit, request.MinStockLevel);

        unitOfWork.Products.Add(product);
        await unitOfWork.SaveChangesAsync(ct);

        return new CreateProductResponse
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            Sku = product.Sku,
            Name = product.Name,
            Description = product.Description,
            Unit = product.Unit,
            MinStockLevel = product.MinStockLevel,
            IsActive = product.IsActive
        };
    }
}
