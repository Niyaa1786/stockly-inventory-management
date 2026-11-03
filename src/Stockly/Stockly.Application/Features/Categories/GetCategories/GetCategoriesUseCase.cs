using FluentValidation;
using Stockly.Application.Common.Interfaces;

namespace Stockly.Application.Features.Categories.GetCategories;

public class GetCategoriesUseCase(IUnitOfWork unitOfWork, IValidator<GetCategoriesRequest> validator)
    : IUseCase<GetCategoriesRequest, GetCategoriesResponse>
{
    public async Task<GetCategoriesResponse> ExecuteAsync(GetCategoriesRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var items = await unitOfWork.Categories.GetPagedAsync(request.Keyword, request.IncludeInactive, request.Page, request.PageSize, ct);
        var total = await unitOfWork.Categories.CountAsync(request.Keyword, request.IncludeInactive, ct);

        return new GetCategoriesResponse
        {
            Items = items.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt
            }).ToList(),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
