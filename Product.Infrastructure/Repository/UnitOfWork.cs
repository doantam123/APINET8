using Product.Core.Interface;
using Product.Infrastructure.Data;

namespace Product.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepository(_context);
        }
    }
}
