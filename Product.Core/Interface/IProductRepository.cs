using Product.Core.Entities;


namespace Product.Core.Interface
{
    public interface IProductRepository : IGenergicRepository<Products>
    {
        //Task<bool> AddAsync(CreateProductDto dto);
    }
}
