using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Products.DeactivateProduct;

public class DeactivateProductUseCase(IUnitOfWork unitOfWork)
    : IUseCase<DeactivateProductRequest, DeactivateProductResponse>
{
    public async Task<DeactivateProductResponse> ExecuteAsync(DeactivateProductRequest request, CancellationToken ct = default)
    {
        var product = await unitOfWork.Products.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        product.SoftDelete();
        await unitOfWork.SaveChangesAsync(ct);

        return new DeactivateProductResponse();
    }
}
