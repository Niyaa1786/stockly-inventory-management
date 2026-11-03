using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stockly.Api.Responses;
using Stockly.Application.Features.Categories.CreateCategory;
using Stockly.Application.Features.Categories.DeactivateCategory;
using Stockly.Application.Features.Categories.GetCategories;
using Stockly.Application.Features.Categories.GetCategoryById;
using Stockly.Application.Features.Categories.RestoreCategory;
using Stockly.Application.Features.Categories.UpdateCategory;
using Stockly.Domain.Enums;

namespace Stockly.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController(
    GetCategoriesUseCase getCategoriesUseCase,
    GetCategoryByIdUseCase getCategoryByIdUseCase,
    CreateCategoryUseCase createCategoryUseCase,
    UpdateCategoryUseCase updateCategoryUseCase,
    DeactivateCategoryUseCase deactivateCategoryUseCase,
    RestoreCategoryUseCase restoreCategoryUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<GetCategoriesResponse>>> GetCategories([FromQuery] GetCategoriesRequest request, CancellationToken ct)
    {
        var result = await getCategoriesUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<GetCategoriesResponse>.Success(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<GetCategoryByIdResponse>>> GetCategoryById(Guid id, CancellationToken ct)
    {
        var result = await getCategoryByIdUseCase.ExecuteAsync(new GetCategoryByIdRequest { Id = id }, ct);
        return Ok(ApiResponse<GetCategoryByIdResponse>.Success(result));
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<CreateCategoryResponse>>> CreateCategory(CreateCategoryRequest request, CancellationToken ct)
    {
        var result = await createCategoryUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<CreateCategoryResponse>.Success(result, "Category created successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<UpdateCategoryResponse>>> UpdateCategory(Guid id, UpdateCategoryRequest request, CancellationToken ct)
    {
        request.Id = id;
        var result = await updateCategoryUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<UpdateCategoryResponse>.Success(result, "Category updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<DeactivateCategoryResponse>>> DeactivateCategory(Guid id, CancellationToken ct)
    {
        var result = await deactivateCategoryUseCase.ExecuteAsync(new DeactivateCategoryRequest { Id = id }, ct);
        return Ok(ApiResponse<DeactivateCategoryResponse>.Success(result, "Category deactivated successfully."));
    }

    [HttpPost("{id:guid}/restore")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<RestoreCategoryResponse>>> RestoreCategory(Guid id, CancellationToken ct)
    {
        var result = await restoreCategoryUseCase.ExecuteAsync(new RestoreCategoryRequest { Id = id }, ct);
        return Ok(ApiResponse<RestoreCategoryResponse>.Success(result, "Category restored successfully."));
    }
}
