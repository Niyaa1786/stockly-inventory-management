using Stockly.Domain.Entities;

namespace Stockly.Domain.Interfaces
{
    public interface ISupplierRepository : IBaseRepository<Supplier, Guid>
    {
        Task<int> CountAsync(string? keyword, bool includeInactive, CancellationToken ct = default);
        Task<IEnumerable<Supplier>> GetPagedAsync(string? keyword, bool includeInactive, int page, int pageSize, CancellationToken ct = default);
    }
}
