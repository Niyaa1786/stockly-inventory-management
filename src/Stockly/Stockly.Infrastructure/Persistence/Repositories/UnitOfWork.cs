using Stockly.Application.Common.Interfaces;
using Stockly.Domain.Interfaces;
using Stockly.Infrastructure.Persistence.Data;

namespace Stockly.Infrastructure.Persistence.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IUserRepository? _users;
    private ICategoryRepository? _categories;
    private IProductRepository? _products;
    private IWarehouseRepository? _warehouses;
    private ISupplierRepository? _suppliers;

    public IUserRepository Users => _users ??= new UserRepository(context);
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(context);
    public IProductRepository Products => _products ??= new ProductRepository(context);
    public IWarehouseRepository Warehouses => _warehouses ??= new WarehouseRepository(context);
    public ISupplierRepository Suppliers => _suppliers ??= new SupplierRepository(context);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => context.SaveChangesAsync(ct);
}
