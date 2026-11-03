using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Categories.RestoreCategory;

public class RestoreCategoryUseCase(IUnitOfWork unitOfWork)
    : IUseCase<RestoreCategoryRequest, RestoreCategoryResponse>
{
    public async Task<RestoreCategoryResponse> ExecuteAsync(RestoreCategoryRequest request, CancellationToken ct = default)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Category), request.Id);

        category.Restore();
        await unitOfWork.SaveChangesAsync(ct);

        return new RestoreCategoryResponse();
    }
}
