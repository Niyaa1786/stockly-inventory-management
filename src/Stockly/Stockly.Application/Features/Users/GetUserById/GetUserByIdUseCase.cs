using Stockly.Application.Common.DTOs;
using Stockly.Application.Common.Exceptions;
using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Entities;

namespace Stockly.Application.Features.Users.GetUserById;

public class GetUserByIdUseCase(IUnitOfWork unitOfWork)
    : IUseCase<GetUserByIdRequest, GetUserByIdResponse>
{
    public async Task<GetUserByIdResponse> ExecuteAsync(GetUserByIdRequest request, CancellationToken ct = default)
    {
        var user = await unitOfWork.Users.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(User), request.Id);

        return new GetUserByIdResponse
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
