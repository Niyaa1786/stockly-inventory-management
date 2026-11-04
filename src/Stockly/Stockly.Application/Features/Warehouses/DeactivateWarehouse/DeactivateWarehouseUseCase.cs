using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Warehouses.DeactivateWarehouse;

public class DeactivateWarehouseUseCase(IUnitOfWork unitOfWork)
    : IUseCase<DeactivateWarehouseRequest, DeactivateWarehouseResponse>
{
    public async Task<DeactivateWarehouseResponse> ExecuteAsync(DeactivateWarehouseRequest request, CancellationToken ct = default)
    {
        var warehouse = await unitOfWork.Warehouses.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Warehouse), request.Id);

        warehouse.SoftDelete();
        await unitOfWork.SaveChangesAsync(ct);

        return new DeactivateWarehouseResponse();
    }
}
