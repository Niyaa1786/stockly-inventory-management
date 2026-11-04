using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Suppliers.DeactivateSupplier;

public class DeactivateSupplierUseCase(IUnitOfWork unitOfWork)
    : IUseCase<DeactivateSupplierRequest, DeactivateSupplierResponse>
{
    public async Task<DeactivateSupplierResponse> ExecuteAsync(DeactivateSupplierRequest request, CancellationToken ct = default)
    {
        var supplier = await unitOfWork.Suppliers.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Supplier), request.Id);

        supplier.SoftDelete();
        await unitOfWork.SaveChangesAsync(ct);

        return new DeactivateSupplierResponse();
    }
}
