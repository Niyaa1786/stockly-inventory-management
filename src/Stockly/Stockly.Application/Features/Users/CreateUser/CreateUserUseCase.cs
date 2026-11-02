using FluentValidation;
using Stockly.Application.Common.DTOs;
using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Users.CreateUser;

public class CreateUserUseCase(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IValidator<CreateUserRequest> validator) : IUseCase<CreateUserRequest, CreateUserResponse>
{
    public async Task<CreateUserResponse> ExecuteAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        if (await unitOfWork.Users.IsEmailTakenAsync(request.Email, null, ct))
            throw new AppValidationException(nameof(request.Email), "Email already exists.");

        var user = new User(
            request.Email,
            passwordHasher.HashPassword(request.Password),
            request.FullName,
            request.Role);

        unitOfWork.Users.Add(user);
        await unitOfWork.SaveChangesAsync(ct);

        return new CreateUserResponse
        {
            User = new UserDto
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
