using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.Core.Entities;
using Product.Core.Interface;
using Product.Infrastructure.Data;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork iOW, IMapper mapper)
        {
            _unitOfWork = iOW;
            _mapper = mapper;
        }

        [HttpGet("get-all-category")]
        public async Task<ActionResult> Get()
        {
            var allCategory = await _unitOfWork.CategoryRepository.GetAllAsync();
            if (allCategory != null)
            {
                var res = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<ListCategoryDTO>>(allCategory);
                return Ok(res);
            }
            return BadRequest();
        }

        [HttpGet("get-category-by-id/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var categoryById = await _unitOfWork.CategoryRepository.GetAsync(id);
            if (categoryById != null)
            {
                return Ok(_mapper.Map<Category, ListCategoryDTO>(categoryById));
            }
            return BadRequest();
        }

        [HttpPost("add-new-category")]
        public async Task<ActionResult> Post(CategoryDTO categoryDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = _mapper.Map<Category>(categoryDTO);
                    await _unitOfWork.CategoryRepository.AddAsync(res);
                    return Ok(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-existing-category-by-id/{id}")]
        public async Task<ActionResult> Put(int id, CategoryDTO categoryDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingCategory = await _unitOfWork.CategoryRepository.GetAsync(id);
                    if (existingCategory != null)
                    {
                        _mapper.Map(categoryDTO, existingCategory);

                    }
                    await _unitOfWork.CategoryRepository.UpdateAsync(id, existingCategory);
                    return Ok(existingCategory);
                }
                return BadRequest($"Category id [{id}] Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-category-by-id/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var exiting_category = await _unitOfWork.CategoryRepository.GetAsync(id);
                if (exiting_category != null)
                {
                    await _unitOfWork.CategoryRepository.DeleteAsync(id);
                    return Ok($"This Category [{exiting_category.Name}] Is deleted ");
                }
                else
                {
                    return BadRequest($"This Category [{exiting_category.Id}] Not Found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
