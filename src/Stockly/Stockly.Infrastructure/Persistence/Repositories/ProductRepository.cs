using Microsoft.EntityFrameworkCore;
using Stockly.Domain.Entities;
using Stockly.Domain.Interfaces;
using Stockly.Infrastructure.Persistence.Data;

namespace Stockly.Infrastructure.Persistence.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct)
        => await context.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Product?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default)
        => await context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<bool> IsSkuTakenAsync(string sku, Guid? excludeId = null, CancellationToken ct = default)
        => await context.Products.AnyAsync(p => p.Sku == sku && (excludeId == null || p.Id != excludeId), ct);

    public async Task<int> CountAsync(string? keyword, Guid? categoryId, bool includeInactive, CancellationToken ct = default)
    {
        var query = context.Products.AsQueryable();

        if (!includeInactive)
            query = query.Where(p => p.IsActive);

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(p => p.Name.Contains(keyword) || p.Sku.Contains(keyword));

        return await query.CountAsync(ct);
    }

    public async Task<IEnumerable<Product>> GetPagedWithDetailsAsync(string? keyword, Guid? categoryId, bool includeInactive, int page, int pageSize, CancellationToken ct = default)
    {
        var query = context.Products.AsNoTracking().Include(p => p.Category).AsQueryable();

        if (!includeInactive)
            query = query.Where(p => p.IsActive);

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(p => p.Name.Contains(keyword) || p.Sku.Contains(keyword));

        return await query
            .OrderBy(p => p.Name)
            .ThenBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public void Add(Product entity) => context.Products.Add(entity);
    public void Update(Product entity) => context.Products.Update(entity);
    public void Remove(Product entity) => context.Products.Remove(entity);
}
