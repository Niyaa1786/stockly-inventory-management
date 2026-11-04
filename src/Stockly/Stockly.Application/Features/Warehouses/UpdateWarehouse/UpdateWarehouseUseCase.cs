using FluentValidation;
using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Warehouses.UpdateWarehouse;

public class UpdateWarehouseUseCase(IUnitOfWork unitOfWork, IValidator<UpdateWarehouseRequest> validator)
    : IUseCase<UpdateWarehouseRequest, UpdateWarehouseResponse>
{
    public async Task<UpdateWarehouseResponse> ExecuteAsync(UpdateWarehouseRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var warehouse = await unitOfWork.Warehouses.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Warehouse), request.Id);

        var code = request.Code.Trim();
        if (await unitOfWork.Warehouses.IsCodeTakenAsync(code, request.Id, ct))
            throw new AppValidationException(nameof(request.Code), "Warehouse code already exists.");

        warehouse.UpdateInfo(request.Name, code, request.Address);
        await unitOfWork.SaveChangesAsync(ct);

        return new UpdateWarehouseResponse
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Code = warehouse.Code,
            Address = warehouse.Address,
            IsActive = warehouse.IsActive
        };
    }
}
