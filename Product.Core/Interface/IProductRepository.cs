using Product.Core.Entities;
using Product.Core.Sharing;
using Product.Infrastructure.Data;


namespace Product.Core.Interface
{
    public interface IProductRepository : IGenergicRepository<Products>
    {
        Task<IEnumerable<ProductDto>> GetAllAsync(ProductParams productParams);
        Task<bool> AddAsync(CreateProductDto dto);
        Task<bool> UpdateAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
