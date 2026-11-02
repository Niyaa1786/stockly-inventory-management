using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Interfaces;
using Stockly.Infrastructure.Persistence.Data;

namespace Stockly.Infrastructure.Persistence.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IUserRepository? _users;

    public IUserRepository Users => _users ??= new UserRepository(context);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => context.SaveChangesAsync(ct);
}
