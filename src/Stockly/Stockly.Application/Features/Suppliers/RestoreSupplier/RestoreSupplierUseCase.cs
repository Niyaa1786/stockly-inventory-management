using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Suppliers.RestoreSupplier;

public class RestoreSupplierUseCase(IUnitOfWork unitOfWork)
    : IUseCase<RestoreSupplierRequest, RestoreSupplierResponse>
{
    public async Task<RestoreSupplierResponse> ExecuteAsync(RestoreSupplierRequest request, CancellationToken ct = default)
    {
        var supplier = await unitOfWork.Suppliers.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Supplier), request.Id);

        supplier.Restore();
        await unitOfWork.SaveChangesAsync(ct);

        return new RestoreSupplierResponse();
    }
}
