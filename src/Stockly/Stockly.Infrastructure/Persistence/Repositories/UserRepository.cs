using Microsoft.EntityFrameworkCore;
using Stockly.Domain.Entities;
using Stockly.Domain.Interfaces;
using Stockly.Infrastructure.Persistence.Data;

namespace Stockly.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        => await context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
            => await context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct = default)
            => await context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, ct);

    public async Task<bool> IsEmailTakenAsync(string email, Guid? excludeId = null, CancellationToken ct = default)
        => await context.Users.AnyAsync(u => u.Email == email && (excludeId == null || u.Id != excludeId), ct);

    public async Task<int> CountAsync(string? keyword, bool includeInactive, CancellationToken ct = default)
    {
        var query = context.Users.AsQueryable();

        if (!includeInactive)
            query = query.Where(u => u.IsActive);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(u => u.Email.Contains(keyword) || u.FullName.Contains(keyword));

        return await query.CountAsync(ct);
    }

    public async Task<IEnumerable<User>> GetPagedAsync(string? keyword, bool includeInactive, int page, int pageSize, CancellationToken ct = default)
    {
        var query = context.Users.AsNoTracking().AsQueryable();

        if (!includeInactive)
            query = query.Where(u => u.IsActive);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(u => u.Email.Contains(keyword) || u.FullName.Contains(keyword));

        return await query
            .OrderBy(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public void Add(User entity) => context.Users.Add(entity);
    public void Update(User entity) => context.Users.Update(entity);
    public void Remove(User entity) => context.Users.Remove(entity);
}
