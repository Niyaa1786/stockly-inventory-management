using FluentValidation;
using Stockly.Application.Common.DTOs;
using Stockly.Application.Common.Interfaces;

namespace Stockly.Application.Features.Users.GetUsers;

public class GetUsersUseCase(IUnitOfWork unitOfWork, IValidator<GetUsersRequest> validator)
    : IUseCase<GetUsersRequest, GetUsersResponse>
{
    public async Task<GetUsersResponse> ExecuteAsync(GetUsersRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

        var items = await unitOfWork.Users.GetPagedAsync(request.Keyword, request.IncludeInactive, request.Page, request.PageSize, ct);
        var total = await unitOfWork.Users.CountAsync(request.Keyword, request.IncludeInactive, ct);

        return new GetUsersResponse
        {
            Items = items.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Role = u.Role,
                IsActive = u.IsActive
            }).ToList(),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize,
        };
    }
}
