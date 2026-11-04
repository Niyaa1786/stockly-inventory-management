using FluentValidation;
using Stockly.Application.Common.Interfaces;

namespace Stockly.Application.Features.Suppliers.GetSuppliers;

public class GetSuppliersUseCase(IUnitOfWork unitOfWork, IValidator<GetSuppliersRequest> validator)
    : IUseCase<GetSuppliersRequest, GetSuppliersResponse>
{
    public async Task<GetSuppliersResponse> ExecuteAsync(GetSuppliersRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var items = await unitOfWork.Suppliers.GetPagedAsync(request.Keyword, request.IncludeInactive, request.Page, request.PageSize, ct);
        var total = await unitOfWork.Suppliers.CountAsync(request.Keyword, request.IncludeInactive, ct);

        return new GetSuppliersResponse
        {
            Items = items.Select(e => new SupplierDto
            {
                Id = e.Id,
                Name = e.Name,
                Phone = e.Phone,
                Email = e.Email,
                Address = e.Address,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            }).ToList(),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
