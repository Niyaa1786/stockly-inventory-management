using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stockly.Api.Responses;
using Stockly.Application.Features.Warehouses.CreateWarehouse;
using Stockly.Application.Features.Warehouses.DeactivateWarehouse;
using Stockly.Application.Features.Warehouses.GetWarehouseById;
using Stockly.Application.Features.Warehouses.GetWarehouses;
using Stockly.Application.Features.Warehouses.RestoreWarehouse;
using Stockly.Application.Features.Warehouses.UpdateWarehouse;
using Stockly.Domain.Enums;

namespace Stockly.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WarehousesController(
    GetWarehousesUseCase getWarehousesUseCase,
    GetWarehouseByIdUseCase getWarehouseByIdUseCase,
    CreateWarehouseUseCase createWarehouseUseCase,
    UpdateWarehouseUseCase updateWarehouseUseCase,
    DeactivateWarehouseUseCase deactivateWarehouseUseCase,
    RestoreWarehouseUseCase restoreWarehouseUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<GetWarehousesResponse>>> GetWarehouses([FromQuery] GetWarehousesRequest request, CancellationToken ct)
    {
        var result = await getWarehousesUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<GetWarehousesResponse>.Success(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<GetWarehouseByIdResponse>>> GetWarehouseById(Guid id, CancellationToken ct)
    {
        var result = await getWarehouseByIdUseCase.ExecuteAsync(new GetWarehouseByIdRequest { Id = id }, ct);
        return Ok(ApiResponse<GetWarehouseByIdResponse>.Success(result));
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<CreateWarehouseResponse>>> CreateWarehouse(CreateWarehouseRequest request, CancellationToken ct)
    {
        var result = await createWarehouseUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<CreateWarehouseResponse>.Success(result, "Warehouse created successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<UpdateWarehouseResponse>>> UpdateWarehouse(Guid id, UpdateWarehouseRequest request, CancellationToken ct)
    {
        request.Id = id;
        var result = await updateWarehouseUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<UpdateWarehouseResponse>.Success(result, "Warehouse updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<DeactivateWarehouseResponse>>> DeactivateWarehouse(Guid id, CancellationToken ct)
    {
        var result = await deactivateWarehouseUseCase.ExecuteAsync(new DeactivateWarehouseRequest { Id = id }, ct);
        return Ok(ApiResponse<DeactivateWarehouseResponse>.Success(result, "Warehouse deactivated successfully."));
    }

    [HttpPost("{id:guid}/restore")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<ActionResult<ApiResponse<RestoreWarehouseResponse>>> RestoreWarehouse(Guid id, CancellationToken ct)
    {
        var result = await restoreWarehouseUseCase.ExecuteAsync(new RestoreWarehouseRequest { Id = id }, ct);
        return Ok(ApiResponse<RestoreWarehouseResponse>.Success(result, "Warehouse restored successfully."));
    }
}
