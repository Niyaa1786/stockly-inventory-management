using Microsoft.EntityFrameworkCore;
using Stockly.Domain.Entities;
using Stockly.Domain.Interfaces;
using Stockly.Infrastructure.Persistence.Data;

namespace Stockly.Infrastructure.Persistence.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct)
        => await context.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<int> CountAsync(string? keyword, bool includeInactive, CancellationToken ct = default)
    {
        var query = context.Categories.AsQueryable();

        if (!includeInactive)
            query = query.Where(c => c.IsActive);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(c => c.Name.Contains(keyword));

        return await query.CountAsync(ct);
    }

    public async Task<IEnumerable<Category>> GetPagedAsync(string? keyword, bool includeInactive, int page, int pageSize, CancellationToken ct = default)
    {
        var query = context.Categories.AsNoTracking().AsQueryable();

        if (!includeInactive)
            query = query.Where(c => c.IsActive);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(c => c.Name.Contains(keyword));

        return await query
            .OrderBy(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public void Add(Category entity) => context.Categories.Add(entity);
    public void Update(Category entity) => context.Categories.Update(entity);
    public void Remove(Category entity) => context.Categories.Remove(entity);
}
