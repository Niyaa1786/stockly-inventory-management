using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Warehouses.GetWarehouseById;

public class GetWarehouseByIdUseCase(IUnitOfWork unitOfWork)
    : IUseCase<GetWarehouseByIdRequest, GetWarehouseByIdResponse>
{
    public async Task<GetWarehouseByIdResponse> ExecuteAsync(GetWarehouseByIdRequest request, CancellationToken ct = default)
    {
        var warehouse = await unitOfWork.Warehouses.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Warehouse), request.Id);

        return new GetWarehouseByIdResponse
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Code = warehouse.Code,
            Address = warehouse.Address,
            IsActive = warehouse.IsActive,
            CreatedAt = warehouse.CreatedAt
        };
    }
}
