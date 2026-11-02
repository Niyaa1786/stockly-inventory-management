using Stockly.Domain.Interfaces;

namespace Stockly.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
