using FluentValidation;
using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Categories.UpdateCategory;

public class UpdateCategoryUseCase(IUnitOfWork unitOfWork, IValidator<UpdateCategoryRequest> validator)
    : IUseCase<UpdateCategoryRequest, UpdateCategoryResponse>
{
    public async Task<UpdateCategoryResponse> ExecuteAsync(UpdateCategoryRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        if (await unitOfWork.Categories.IsNameTakenAsync(request.Name.Trim(), request.Id, ct))
            throw new AppValidationException(nameof(request.Name), "Category name already exists.");

        var category = await unitOfWork.Categories.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Category), request.Id);

        category.UpdateInfo(request.Name, request.Description);
        await unitOfWork.SaveChangesAsync(ct);

        return new UpdateCategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive
        };
    }
}
