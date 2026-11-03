using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stockly.Api.Responses;
using Stockly.Application.Features.Auth.ChangePassword;
using Stockly.Application.Features.Auth.Login;
using Stockly.Application.Features.Auth.Logout;
using Stockly.Application.Features.Auth.RefreshAccessToken;

namespace Stockly.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthController(
    LoginUseCase loginUseCase,
    RefreshAccessTokenUseCase refreshAccessTokenUseCase,
    LogoutUseCase logoutUseCase,
    ChangePasswordUseCase changePasswordUseCase) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await loginUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<LoginResponse>.Success(result, "Login successful."));
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<RefreshAccessTokenResponse>>> Refresh(RefreshAccessTokenRequest request, CancellationToken ct)
    {
        var result = await refreshAccessTokenUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<RefreshAccessTokenResponse>.Success(result, "Token refreshed."));
    }

    [HttpPost("logout")]
    public async Task<ActionResult<ApiResponse<LogoutResponse>>> Logout(CancellationToken ct)
    {
        var result = await logoutUseCase.ExecuteAsync(new LogoutRequest { UserId = GetUserId() }, ct);
        return Ok(ApiResponse<LogoutResponse>.Success(result, "Logout successful."));
    }

    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResponse<ChangePasswordResponse>>> ChangePassword(ChangePasswordRequest request, CancellationToken ct)
    {
        request.UserId = GetUserId();
        var result = await changePasswordUseCase.ExecuteAsync(request, ct);
        return Ok(ApiResponse<ChangePasswordResponse>.Success(result, "Password changed successfully."));
    }

    private Guid GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User ID claim not found");
        return Guid.Parse(userId);
    }
}
