using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;

namespace Stockly.Application.Features.Auth.Logout;

public class LogoutUseCase(IUnitOfWork unitOfWork) : IUseCase<LogoutRequest, LogoutResponse>
{
    public async Task<LogoutResponse> ExecuteAsync(LogoutRequest request, CancellationToken ct = default)
    {
        var user = await unitOfWork.Users.GetByIdAsync(request.UserId, ct)
            ?? throw new NotFoundException("User not found.");

        user.RevokeSessions();
        await unitOfWork.SaveChangesAsync(ct);

        return new LogoutResponse();
    }
}
