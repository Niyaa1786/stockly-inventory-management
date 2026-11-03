using FluentValidation;
using Stockly.Application.Common.DTOs;
using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Users.UpdateUser;

public class UpdateUserUseCase(IUnitOfWork unitOfWork, IValidator<UpdateUserRequest> validator)
    : IUseCase<UpdateUserRequest, UpdateUserResponse>
{
    public async Task<UpdateUserResponse> ExecuteAsync(UpdateUserRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var user = await unitOfWork.Users.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(User), request.Id);

        if (await unitOfWork.Users.IsEmailTakenAsync(request.Email, request.Id, ct))
            throw new AppValidationException(nameof(request.Email), "Email already exists.");

        user.UpdateProfile(request.Email, request.FullName);
        user.AssignRole(request.Role);
        await unitOfWork.SaveChangesAsync(ct);

        return new UpdateUserResponse
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            IsActive = user.IsActive
        };
    }
}
