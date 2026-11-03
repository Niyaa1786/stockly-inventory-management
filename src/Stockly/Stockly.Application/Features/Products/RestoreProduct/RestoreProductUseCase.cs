using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Products.RestoreProduct;

public class RestoreProductUseCase(IUnitOfWork unitOfWork)
    : IUseCase<RestoreProductRequest, RestoreProductResponse>
{
    public async Task<RestoreProductResponse> ExecuteAsync(RestoreProductRequest request, CancellationToken ct = default)
    {
        var product = await unitOfWork.Products.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        product.Restore();
        await unitOfWork.SaveChangesAsync(ct);

        return new RestoreProductResponse();
    }
}
