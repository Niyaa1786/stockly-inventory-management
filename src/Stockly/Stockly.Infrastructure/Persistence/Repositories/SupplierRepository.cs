using Microsoft.EntityFrameworkCore;
using Stockly.Domain.Entities;
using Stockly.Domain.Interfaces;
using Stockly.Infrastructure.Persistence.Data;

namespace Stockly.Infrastructure.Persistence.Repositories;

public class SupplierRepository(AppDbContext context) : ISupplierRepository
{
    public async Task<Supplier?> GetByIdAsync(Guid id, CancellationToken ct)
        => await context.Suppliers.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<int> CountAsync(string? keyword, bool includeInactive, CancellationToken ct = default)
    {
        var query = context.Suppliers.AsQueryable();

        if (!includeInactive)
            query = query.Where(s => s.IsActive);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(s => s.Name.Contains(keyword)
                || (s.Phone != null && s.Phone.Contains(keyword))
                || (s.Email != null && s.Email.Contains(keyword)));

        return await query.CountAsync(ct);
    }

    public async Task<IEnumerable<Supplier>> GetPagedAsync(string? keyword, bool includeInactive, int page, int pageSize, CancellationToken ct = default)
    {
        var query = context.Suppliers.AsNoTracking().AsQueryable();

        if (!includeInactive)
            query = query.Where(s => s.IsActive);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(s => s.Name.Contains(keyword)
                || (s.Phone != null && s.Phone.Contains(keyword))
                || (s.Email != null && s.Email.Contains(keyword)));

        return await query
            .OrderBy(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public void Add(Supplier entity) => context.Suppliers.Add(entity);
    public void Update(Supplier entity) => context.Suppliers.Update(entity);
    public void Remove(Supplier entity) => context.Suppliers.Remove(entity);
}
