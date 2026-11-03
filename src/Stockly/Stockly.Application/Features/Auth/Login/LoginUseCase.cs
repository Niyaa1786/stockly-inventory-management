using FluentValidation;
using Stockly.Application.Common.DTOs;
using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;

namespace Stockly.Application.Features.Auth.Login;

public class LoginUseCase(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ITokenGenerator tokenGenerator,
    IValidator<LoginRequest> validator) : IUseCase<LoginRequest, LoginResponse>
{
    public async Task<LoginResponse> ExecuteAsync(LoginRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var user = await unitOfWork.Users.GetByEmailAsync(request.Email.Trim(), ct);

        if (user is null || !passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.CanLogin)
            throw new ForbiddenException("This account has been deactivated.");

        var token = tokenGenerator.GenerateToken(user);

        return new LoginResponse
        {
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken,
            User = new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                IsActive = user.IsActive
            }
        };
    }
}
