using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Suppliers.GetSupplierById;

public class GetSupplierByIdUseCase(IUnitOfWork unitOfWork)
    : IUseCase<GetSupplierByIdRequest, GetSupplierByIdResponse>
{
    public async Task<GetSupplierByIdResponse> ExecuteAsync(GetSupplierByIdRequest request, CancellationToken ct = default)
    {
        var supplier = await unitOfWork.Suppliers.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Supplier), request.Id);

        return new GetSupplierByIdResponse
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Phone = supplier.Phone,
            Email = supplier.Email,
            Address = supplier.Address,
            IsActive = supplier.IsActive,
            CreatedAt = supplier.CreatedAt
        };
    }
}
