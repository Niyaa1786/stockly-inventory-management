using FluentValidation;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Categories.CreateCategory;

public class CreateCategoryUseCase(IUnitOfWork unitOfWork, IValidator<CreateCategoryRequest> validator)
    : IUseCase<CreateCategoryRequest, CreateCategoryResponse>
{
    public async Task<CreateCategoryResponse> ExecuteAsync(CreateCategoryRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var category = new Category(request.Name, request.Description);

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
