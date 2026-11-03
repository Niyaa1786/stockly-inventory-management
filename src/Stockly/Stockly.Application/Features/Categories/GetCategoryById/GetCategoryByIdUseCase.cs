using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Categories.GetCategoryById;

public class GetCategoryByIdUseCase(IUnitOfWork unitOfWork)
    : IUseCase<GetCategoryByIdRequest, GetCategoryByIdResponse>
{
    public async Task<GetCategoryByIdResponse> ExecuteAsync(GetCategoryByIdRequest request, CancellationToken ct = default)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Category), request.Id);

        return new GetCategoryByIdResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt
        };
    }
}
