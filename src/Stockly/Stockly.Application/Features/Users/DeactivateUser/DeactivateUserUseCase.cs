using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Users.DeactivateUser;

public class DeactivateUserUseCase(IUnitOfWork unitOfWork)
    : IUseCase<DeactivateUserRequest, DeactivateUserResponse>
{
    public async Task<DeactivateUserResponse> ExecuteAsync(DeactivateUserRequest request, CancellationToken ct = default)
    {
        // Prevents an admin from locking themselves out (and the system from losing its last admin this way).
        if (request.Id == request.CurrentUserId)
            throw new AppValidationException(nameof(request.Id), "You cannot deactivate your own account.");

        var user = await unitOfWork.Users.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(User), request.Id);

        user.SoftDelete();
        await unitOfWork.SaveChangesAsync(ct);

        return new DeactivateUserResponse();
    }
}
