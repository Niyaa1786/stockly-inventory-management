using FluentValidation;
using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Categories.CreateCategory;

public class CreateCategoryUseCase(IUnitOfWork unitOfWork, IValidator<CreateCategoryRequest> validator)
    : IUseCase<CreateCategoryRequest, CreateCategoryResponse>
{
    public async Task<CreateCategoryResponse> ExecuteAsync(CreateCategoryRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var name = request.Name.Trim();
        if (await unitOfWork.Categories.IsNameTakenAsync(name, null, ct))
            throw new AppValidationException(nameof(request.Name), "Category name already exists.");

        var category = new Category(name, request.Description);

        unitOfWork.Categories.Add(category);
        await unitOfWork.SaveChangesAsync(ct);

        return new CreateCategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive
        };
    }
}
