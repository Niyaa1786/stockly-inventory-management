using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stockly.Api.Responses;
using Stockly.Application.Features.Suppliers.CreateSupplier;
using Stockly.Application.Features.Suppliers.DeactivateSupplier;
using Stockly.Application.Features.Suppliers.GetSupplierById;
using Stockly.Application.Features.Suppliers.GetSuppliers;
using Stockly.Application.Features.Suppliers.RestoreSupplier;
using Stockly.Application.Features.Suppliers.UpdateSupplier;
using Stockly.Domain.Enums;

namespace Stockly.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SuppliersController(
    GetSuppliersUseCase getSuppliersUseCase,
    GetSupplierByIdUseCase getSupplierByIdUseCase,
    CreateSupplierUseCase createSupplierUseCase,
    UpdateSupplierUseCase updateSupplierUseCase,
    DeactivateSupplierUseCase deactivateSupplierUseCase,
    RestoreSupplierUseCase restoreSupplierUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<GetSuppliersResponse>>> GetSuppliers([FromQuery] GetSuppliersRequest request, CancellationToken ct)
    {
        var result = await getSuppliersUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<GetSuppliersResponse>.Success(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<GetSupplierByIdResponse>>> GetSupplierById(Guid id, CancellationToken ct)
    {
        var result = await getSupplierByIdUseCase.ExecuteAsync(new GetSupplierByIdRequest { Id = id }, ct);
        return Ok(ApiResponse<GetSupplierByIdResponse>.Success(result));
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<CreateSupplierResponse>>> CreateSupplier(CreateSupplierRequest request, CancellationToken ct)
    {
        var result = await createSupplierUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<CreateSupplierResponse>.Success(result, "Supplier created successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<UpdateSupplierResponse>>> UpdateSupplier(Guid id, UpdateSupplierRequest request, CancellationToken ct)
    {
        request.Id = id;
        var result = await updateSupplierUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<UpdateSupplierResponse>.Success(result, "Supplier updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<DeactivateSupplierResponse>>> DeactivateSupplier(Guid id, CancellationToken ct)
    {
        var result = await deactivateSupplierUseCase.ExecuteAsync(new DeactivateSupplierRequest { Id = id }, ct);
        return Ok(ApiResponse<DeactivateSupplierResponse>.Success(result, "Supplier deactivated successfully."));
    }

    [HttpPost("{id:guid}/restore")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<RestoreSupplierResponse>>> RestoreSupplier(Guid id, CancellationToken ct)
    {
        var result = await restoreSupplierUseCase.ExecuteAsync(new RestoreSupplierRequest { Id = id }, ct);
        return Ok(ApiResponse<RestoreSupplierResponse>.Success(result, "Supplier restored successfully."));
    }
}
