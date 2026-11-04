using Microsoft.EntityFrameworkCore;
using Stockly.Domain.Entities;
using Stockly.Domain.Interfaces;
using Stockly.Infrastructure.Persistence.Data;

namespace Stockly.Infrastructure.Persistence.Repositories;

public class WarehouseRepository(AppDbContext context) : IWarehouseRepository
{
    public async Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken ct)
        => await context.Warehouses.FirstOrDefaultAsync(w => w.Id == id, ct);

    public async Task<bool> IsCodeTakenAsync(string code, Guid? excludeId = null, CancellationToken ct = default)
        => await context.Warehouses.AnyAsync(w => w.Code == code && (excludeId == null || w.Id != excludeId), ct);

    public async Task<int> CountAsync(string? keyword, bool includeInactive, CancellationToken ct = default)
    {
        var query = context.Warehouses.AsQueryable();

        if (!includeInactive)
            query = query.Where(w => w.IsActive);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(w => w.Name.Contains(keyword) || w.Code.Contains(keyword));

        return await query.CountAsync(ct);
    }

    public async Task<IEnumerable<Warehouse>> GetPagedAsync(string? keyword, bool includeInactive, int page, int pageSize, CancellationToken ct = default)
    {
        var query = context.Warehouses.AsNoTracking().AsQueryable();

        if (!includeInactive)
            query = query.Where(w => w.IsActive);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(w => w.Name.Contains(keyword) || w.Code.Contains(keyword));

        return await query
            .OrderBy(w => w.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public void Add(Warehouse entity) => context.Warehouses.Add(entity);
    public void Update(Warehouse entity) => context.Warehouses.Update(entity);
    public void Remove(Warehouse entity) => context.Warehouses.Remove(entity);
}
