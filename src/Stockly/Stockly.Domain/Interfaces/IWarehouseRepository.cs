using Stockly.Domain.Entities;

namespace Stockly.Domain.Interfaces
{
    public interface IWarehouseRepository : IBaseRepository<Warehouse, Guid>
    {
        Task<bool> IsCodeTakenAsync(string code, Guid? excludeId = null, CancellationToken ct = default);
        Task<int> CountAsync(string? keyword, bool includeInactive, CancellationToken ct = default);
        Task<IEnumerable<Warehouse>> GetPagedAsync(string? keyword, bool includeInactive, int page, int pageSize, CancellationToken ct = default);
    }
}
