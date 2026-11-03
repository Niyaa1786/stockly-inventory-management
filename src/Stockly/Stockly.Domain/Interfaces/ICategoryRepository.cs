using Stockly.Domain.Entities;

namespace Stockly.Domain.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category, Guid>
    {
        Task<bool> IsNameTakenAsync(string name, Guid? excludeId = null, CancellationToken ct = default);
        Task<int> CountAsync(string? keyword, bool includeInactive, CancellationToken ct = default);
        Task<IEnumerable<Category>> GetPagedAsync(string? keyword, bool includeInactive, int page, int pageSize, CancellationToken ct = default);
    }
}
