using Stockly.Domain.Entities;

namespace Stockly.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User, Guid>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<bool> IsEmailTakenAsync(string email, Guid? excludeId = null, CancellationToken ct = default);
        Task<int> CountAsync(string? keyword, bool includeInactive, CancellationToken ct = default);
        Task<IEnumerable<User>> GetPagedAsync(string? keyword, bool includeInactive, int page, int pageSize, CancellationToken ct = default);
    }
}
