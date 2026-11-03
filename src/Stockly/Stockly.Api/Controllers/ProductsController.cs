using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stockly.Api.Responses;
using Stockly.Application.Features.Products.CreateProduct;
using Stockly.Application.Features.Products.DeactivateProduct;
using Stockly.Application.Features.Products.GetProducts;
using Stockly.Application.Features.Products.GetProductById;
using Stockly.Application.Features.Products.RestoreProduct;
using Stockly.Application.Features.Products.UpdateProduct;
using Stockly.Domain.Enums;

namespace Stockly.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductsController(
    GetProductsUseCase getProductsUseCase,
    GetProductByIdUseCase getProductByIdUseCase,
    CreateProductUseCase createProductUseCase,
    UpdateProductUseCase updateProductUseCase,
    DeactivateProductUseCase deactivateProductUseCase,
    RestoreProductUseCase restoreProductUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<GetProductsResponse>>> GetProducts([FromQuery] GetProductsRequest request, CancellationToken ct)
    {
        var result = await getProductsUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<GetProductsResponse>.Success(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<GetProductByIdResponse>>> GetProductById(Guid id, CancellationToken ct)
    {
        var result = await getProductByIdUseCase.ExecuteAsync(new GetProductByIdRequest { Id = id }, ct);
        return Ok(ApiResponse<GetProductByIdResponse>.Success(result));
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<CreateProductResponse>>> CreateProduct(CreateProductRequest request, CancellationToken ct)
    {
        var result = await createProductUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<CreateProductResponse>.Success(result, "Product created successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<UpdateProductResponse>>> UpdateProduct(Guid id, UpdateProductRequest request, CancellationToken ct)
    {
        request.Id = id;
        var result = await updateProductUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<UpdateProductResponse>.Success(result, "Product updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<DeactivateProductResponse>>> DeactivateProduct(Guid id, CancellationToken ct)
    {
        var result = await deactivateProductUseCase.ExecuteAsync(new DeactivateProductRequest { Id = id }, ct);
        return Ok(ApiResponse<DeactivateProductResponse>.Success(result, "Product deactivated successfully."));
    }

    [HttpPost("{id:guid}/restore")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<RestoreProductResponse>>> RestoreProduct(Guid id, CancellationToken ct)
    {
        var result = await restoreProductUseCase.ExecuteAsync(new RestoreProductRequest { Id = id }, ct);
        return Ok(ApiResponse<RestoreProductResponse>.Success(result, "Product restored successfully."));
    }
}
