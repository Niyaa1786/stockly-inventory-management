using FluentValidation;
using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;

namespace Stockly.Application.Features.Auth.ChangePassword;

public class ChangePasswordUseCase(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IValidator<ChangePasswordRequest> validator) : IUseCase<ChangePasswordRequest, ChangePasswordResponse>
{
    public async Task<ChangePasswordResponse> ExecuteAsync(ChangePasswordRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var user = await unitOfWork.Users.GetByIdAsync(request.UserId, ct)
            ?? throw new NotFoundException("User not found.");

        if (!passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            throw new AppValidationException(nameof(request.CurrentPassword), "Current password is incorrect.");

        user.ChangePassword(passwordHasher.HashPassword(request.NewPassword));
        await unitOfWork.SaveChangesAsync(ct);

        return new ChangePasswordResponse();
    }
}
