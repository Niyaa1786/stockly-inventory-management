using FluentValidation;
using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Warehouses.CreateWarehouse;

public class CreateWarehouseUseCase(IUnitOfWork unitOfWork, IValidator<CreateWarehouseRequest> validator)
    : IUseCase<CreateWarehouseRequest, CreateWarehouseResponse>
{
    public async Task<CreateWarehouseResponse> ExecuteAsync(CreateWarehouseRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var code = request.Code.Trim();
        if (await unitOfWork.Warehouses.IsCodeTakenAsync(code, null, ct))
            throw new AppValidationException(nameof(request.Code), "Warehouse code already exists.");

        var warehouse = new Warehouse(request.Name, code, request.Address);

        unitOfWork.Warehouses.Add(warehouse);
        await unitOfWork.SaveChangesAsync(ct);

        return new CreateWarehouseResponse
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Code = warehouse.Code,
            Address = warehouse.Address,
            IsActive = warehouse.IsActive
        };
    }
}
