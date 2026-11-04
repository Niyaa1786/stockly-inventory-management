using FluentValidation;
using Stockly.Application.Common.Interfaces;

namespace Stockly.Application.Features.Warehouses.GetWarehouses;

public class GetWarehousesUseCase(IUnitOfWork unitOfWork, IValidator<GetWarehousesRequest> validator)
    : IUseCase<GetWarehousesRequest, GetWarehousesResponse>
{
    public async Task<GetWarehousesResponse> ExecuteAsync(GetWarehousesRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var items = await unitOfWork.Warehouses.GetPagedAsync(request.Keyword, request.IncludeInactive, request.Page, request.PageSize, ct);
        var total = await unitOfWork.Warehouses.CountAsync(request.Keyword, request.IncludeInactive, ct);

        return new GetWarehousesResponse
        {
            Items = items.Select(e => new WarehouseDto
            {
                Id = e.Id,
                Name = e.Name,
                Code = e.Code,
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
