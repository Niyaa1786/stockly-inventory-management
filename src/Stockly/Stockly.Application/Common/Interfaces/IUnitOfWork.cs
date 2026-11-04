using Stockly.Domain.Interfaces;

namespace Stockly.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    ICategoryRepository Categories { get; }
    IProductRepository Products { get; }
    IWarehouseRepository Warehouses { get; }
    ISupplierRepository Suppliers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
