using FluentValidation;
using Stockly.Application.Common.Interfaces;

namespace Stockly.Application.Features.Auth.RefreshAccessToken;

public class RefreshAccessTokenUseCase(
    IUnitOfWork unitOfWork,
    ITokenGenerator tokenGenerator,
    IValidator<RefreshAccessTokenRequest> validator) : IUseCase<RefreshAccessTokenRequest, RefreshAccessTokenResponse>
{
    public async Task<RefreshAccessTokenResponse> ExecuteAsync(RefreshAccessTokenRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var user = await unitOfWork.Users.GetByRefreshTokenAsync(request.RefreshToken, ct);

        if (user is null || !user.CanRefresh(DateTime.UtcNow))
            throw new UnauthorizedAccessException("Refresh token is invalid or expired.");

        var token = tokenGenerator.GenerateToken(user);
        user.SetRefreshToken(token.RefreshToken, token.RefreshTokenExpiration);
        await unitOfWork.SaveChangesAsync(ct);

        return new RefreshAccessTokenResponse
        {
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken,
        };
    }
}
