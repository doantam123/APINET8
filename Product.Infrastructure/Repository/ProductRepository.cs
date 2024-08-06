using Product.Core.Entities;
using Product.Core.Interface;
using Product.Infrastructure.Data;

namespace Product.Infrastructure.Repository
{
    public class ProductRepository : GenergicRepository<Products>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
