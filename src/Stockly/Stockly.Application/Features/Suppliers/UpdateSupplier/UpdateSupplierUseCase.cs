using FluentValidation;
using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Suppliers.UpdateSupplier;

public class UpdateSupplierUseCase(IUnitOfWork unitOfWork, IValidator<UpdateSupplierRequest> validator)
    : IUseCase<UpdateSupplierRequest, UpdateSupplierResponse>
{
    public async Task<UpdateSupplierResponse> ExecuteAsync(UpdateSupplierRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var supplier = await unitOfWork.Suppliers.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Supplier), request.Id);

        supplier.UpdateInfo(request.Name, request.Phone, request.Email, request.Address);
        await unitOfWork.SaveChangesAsync(ct);

        return new UpdateSupplierResponse
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
