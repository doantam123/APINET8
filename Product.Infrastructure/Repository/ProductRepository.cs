using AutoMapper;
using Microsoft.Extensions.FileProviders;
using Product.Core.Entities;
using Product.Core.Interface;
using Product.Infrastructure.Data;

namespace Product.Infrastructure.Repository
{
    public class ProductRepository : GenergicRepository<Products>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper) : base(context)
        {
            this._context = context;
            this._fileProvider = fileProvider;
            this._mapper = mapper;
        }

        public async Task<bool> AddAsync(CreateProductDto dto)
        {
            if (dto.Image is not null)
            {
                var root = Path.Combine("wwwroot", "images", "product");
                var productName = $"{Guid.NewGuid()}_{dto.Image.FileName}";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                var src = Path.Combine(root, productName);
                using (var fileStream = new FileStream(src, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(fileStream);
                }
                var res = _mapper.Map<Products>(dto);
                res.ProductPicture = src.Replace("wwwroot", string.Empty).Replace("\\", "/"); 
                await _context.Products.AddAsync(res);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
