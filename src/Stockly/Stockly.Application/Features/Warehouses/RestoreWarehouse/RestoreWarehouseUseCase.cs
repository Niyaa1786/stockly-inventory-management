using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Warehouses.RestoreWarehouse;

public class RestoreWarehouseUseCase(IUnitOfWork unitOfWork)
    : IUseCase<RestoreWarehouseRequest, RestoreWarehouseResponse>
{
    public async Task<RestoreWarehouseResponse> ExecuteAsync(RestoreWarehouseRequest request, CancellationToken ct = default)
    {
        var warehouse = await unitOfWork.Warehouses.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Warehouse), request.Id);

        warehouse.Restore();
        await unitOfWork.SaveChangesAsync(ct);

        return new RestoreWarehouseResponse();
    }
}
