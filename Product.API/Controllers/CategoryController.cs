using Microsoft.AspNetCore.Http;
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
        public CategoryController(IUnitOfWork iOW) 
        {
            _unitOfWork = iOW;
        }

        [HttpGet("get-all-category")]
        public async Task<ActionResult> Get() 
        {
            var allCategory = await _unitOfWork.CategoryRepository.GetAllAsync();
            if (allCategory != null) {
                return Ok(allCategory);
            }
            return BadRequest();
        }

        [HttpGet("get-category-by-id/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var categoryById = await _unitOfWork.CategoryRepository.GetAsync(id);
            if (categoryById != null)
            {
                return Ok(categoryById);
            }
            return BadRequest();
        }

        [HttpPost("add-new-category")]
        public async Task<ActionResult> Post(CategoryDTO categoryDTO)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var newCategory = new Category
                    {
                        Name = categoryDTO.Name,
                        Description = categoryDTO.Description,
                    };
                    await _unitOfWork.CategoryRepository.AddAsync(newCategory);
                    return Ok(newCategory);
                }
                return BadRequest();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-existing-categoru-by-id/{id}")]
        public async Task<ActionResult> Put(int id, CategoryDTO categoryDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingCategory = await _unitOfWork.CategoryRepository.GetAsync(id);
                    if (existingCategory != null)
                    {
                        existingCategory.Name = categoryDTO.Name;
                        existingCategory.Description = categoryDTO.Description; 
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
                return BadRequest($"This Category [{exiting_category.Id}] Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
