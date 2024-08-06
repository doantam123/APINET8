using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.Core.Entities;
using Product.Core.Interface;
using Product.Infrastructure.Data;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProductController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("get-all-products")]
        public async Task<ActionResult> Get()
        {
            var res = await _uow.ProductRepository.GetAllAsync();
            return Ok(res);
        }

        [HttpGet("get-product-by-id/{id}")]
        public async Task<ActionResult> get(int id)
        {

            var res = await _uow.ProductRepository.GetByidAsync(id, x => x.Category);
            if (res is null)
                return NotFound();
            var result = _mapper.Map<ProductDto>(res);
            return Ok(result);
        }

        [HttpPost("add-new-product")]
        public async Task<ActionResult> Post(CreateProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.ProductRepository.AddAsync(productDto);
                    return Ok(productDto);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
