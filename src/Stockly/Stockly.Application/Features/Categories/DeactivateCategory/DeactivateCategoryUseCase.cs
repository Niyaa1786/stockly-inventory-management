using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Categories.DeactivateCategory;

public class DeactivateCategoryUseCase(IUnitOfWork unitOfWork)
    : IUseCase<DeactivateCategoryRequest, DeactivateCategoryResponse>
{
    public async Task<DeactivateCategoryResponse> ExecuteAsync(DeactivateCategoryRequest request, CancellationToken ct = default)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Category), request.Id);

        category.SoftDelete();
        await unitOfWork.SaveChangesAsync(ct);

        return new DeactivateCategoryResponse();
    }
}
