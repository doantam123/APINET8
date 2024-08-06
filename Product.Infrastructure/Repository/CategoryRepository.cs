using Product.Core.Entities;
using Product.Core.Interface;
using Product.Infrastructure.Data;

namespace Product.Infrastructure.Repository
{
    public class CategoryRepository : GenergicRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
