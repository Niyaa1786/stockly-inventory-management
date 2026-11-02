namespace Stockly.Domain.Interfaces
{
    public interface IBaseRepository<T, TId> where T : class
    {
        Task<T?> GetByIdAsync(TId id, CancellationToken ct);

        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
