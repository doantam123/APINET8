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
                var root = "/images/product/";
                var prodcutname = $"{Guid.NewGuid()}" + dto.Image.FileName;
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                var src = root + prodcutname;
                var pic_info = _fileProvider.GetFileInfo(src);
                var root_path = pic_info.PhysicalPath;
                using (var file_streem = new FileStream(root_path, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(file_streem);

                }
                var res = _mapper.Map<Products>(dto);
                res.ProductPicture = src;
                await _context.Products.AddAsync(res);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
