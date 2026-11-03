using Stockly.Application.Common.DTOs;
using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Users.RestoreUser;

public class RestoreUserUseCase(IUnitOfWork unitOfWork) : IUseCase<RestoreUserRequest, RestoreUserResponse>
{
    public async Task<RestoreUserResponse> ExecuteAsync(RestoreUserRequest request, CancellationToken ct = default)
    {
        var user = await unitOfWork.Users.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(User), request.Id);

        user.Restore();
        await unitOfWork.SaveChangesAsync(ct);

        return new RestoreUserResponse();
    }
}
