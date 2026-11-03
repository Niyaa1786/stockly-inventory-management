using Stockly.Domain.Entities;

namespace Stockly.Domain.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product, Guid>
    {
        Task<Product?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);
        Task<bool> IsSkuTakenAsync(string sku, Guid? excludeId = null, CancellationToken ct = default);
        Task<int> CountAsync(string? keyword, Guid? categoryId, bool includeInactive, CancellationToken ct = default);
        Task<IEnumerable<Product>> GetPagedWithDetailsAsync(string? keyword, Guid? categoryId, bool includeInactive, int page, int pageSize, CancellationToken ct = default);
    }
}
