using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stockly.Api.Responses;
using Stockly.Application.Features.Auth.Login;
using Stockly.Domain.Entities;

namespace Stockly.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(LoginUseCase loginUseCase) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await loginUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<LoginResponse>.Success(result, "Login successful."));
    }

    private Guid GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User ID claim not found");
        return Guid.Parse(userId);
    }
}