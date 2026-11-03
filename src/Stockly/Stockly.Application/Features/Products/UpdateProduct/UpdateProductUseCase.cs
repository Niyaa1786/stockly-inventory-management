using FluentValidation;
using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Products.UpdateProduct;

public class UpdateProductUseCase(IUnitOfWork unitOfWork, IValidator<UpdateProductRequest> validator)
    : IUseCase<UpdateProductRequest, UpdateProductResponse>
{
    public async Task<UpdateProductResponse> ExecuteAsync(UpdateProductRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var product = await unitOfWork.Products.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        var sku = request.Sku.Trim();
        if (await unitOfWork.Products.IsSkuTakenAsync(sku, request.Id, ct))
            throw new AppValidationException(nameof(request.Sku), "SKU already exists.");

        if (request.CategoryId != product.CategoryId)
        {
            var category = await unitOfWork.Categories.GetByIdAsync(request.CategoryId, ct)
                ?? throw new NotFoundException(nameof(Category), request.CategoryId);

            if (!category.CanBeAssignedToProduct)
                throw new AppValidationException(nameof(request.CategoryId), "Category is deactivated.");

            product.ChangeCategory(category);
        }

        product.UpdateInfo(sku, request.Name, request.Description, request.Unit, request.MinStockLevel);
        await unitOfWork.SaveChangesAsync(ct);

        return new UpdateProductResponse
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
