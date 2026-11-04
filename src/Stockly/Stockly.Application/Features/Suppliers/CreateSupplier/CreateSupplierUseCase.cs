using FluentValidation;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Suppliers.CreateSupplier;

public class CreateSupplierUseCase(IUnitOfWork unitOfWork, IValidator<CreateSupplierRequest> validator)
    : IUseCase<CreateSupplierRequest, CreateSupplierResponse>
{
    public async Task<CreateSupplierResponse> ExecuteAsync(CreateSupplierRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var supplier = new Supplier(request.Name, request.Phone, request.Email, request.Address);

        unitOfWork.Suppliers.Add(supplier);
        await unitOfWork.SaveChangesAsync(ct);

        return new CreateSupplierResponse
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Phone = supplier.Phone,
            Email = supplier.Email,
            Address = supplier.Address,
            IsActive = supplier.IsActive
        };
    }
}
