using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stockly.Api.Responses;
using Stockly.Application.Features.Users.CreateUser;
using Stockly.Application.Features.Users.DeactivateUser;
using Stockly.Application.Features.Users.GetUserById;
using Stockly.Application.Features.Users.GetUsers;
using Stockly.Application.Features.Users.RestoreUser;
using Stockly.Application.Features.Users.UpdateUser;
using Stockly.Domain.Enums;

namespace Stockly.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = nameof(Role.Admin))]
public class UsersController(
    GetUsersUseCase getUsersUseCase,
    GetUserByIdUseCase getUserByIdUseCase,
    CreateUserUseCase createUserUseCase,
    UpdateUserUseCase updateUserUseCase,
    DeactivateUserUseCase deactivateUserUseCase,
    RestoreUserUseCase restoreUserUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersRequest request, CancellationToken ct)
    {
        var result = await getUsersUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<GetUsersResponse>.Success(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken ct)
    {
        var result = await getUserByIdUseCase.ExecuteAsync(new GetUserByIdRequest { Id = id }, ct);
        return Ok(ApiResponse<GetUserByIdResponse>.Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request, CancellationToken ct)
    {
        var result = await createUserUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<CreateUserResponse>.Success(result, "User created successfully."));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserRequest request, CancellationToken ct)
    {
        request.Id = id;
        var result = await updateUserUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<UpdateUserResponse>.Success(result, "User updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeactivateUser(Guid id, CancellationToken ct)
    {
        var request = new DeactivateUserRequest { Id = id, CurrentUserId = GetUserId() };
        var result = await deactivateUserUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<DeactivateUserResponse>.Success(result, "User deactivated successfully."));
    }

    [HttpPost("{id:guid}/restore")]
    public async Task<IActionResult> RestoreUser(Guid id, CancellationToken ct)
    {
        var result = await restoreUserUseCase.ExecuteAsync(new RestoreUserRequest { Id = id }, ct);
        return Ok(ApiResponse<RestoreUserResponse>.Success(result, "User restored successfully."));
    }

    private Guid GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User ID claim not found");
        return Guid.Parse(userId);
    }
}