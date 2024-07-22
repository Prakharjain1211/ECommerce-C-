using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.DTO;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Repository;

namespace ShoppingAPI.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoriesController(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            var productCategories = await _productCategoryRepository.GetAllAsync();
            return productCategories.Select(pc => new ProductCategoryDto
            {
                Id = pc.Id,
                Name = pc.Name
            }).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategoryDto>> GetProductCategory(int id)
        {
            var productCategory = await _productCategoryRepository.GetByIdAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return new ProductCategoryDto
            {
                Id = productCategory.Id,
                Name = productCategory.Name
            };
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<ProductCategoryDto>> CreateProductCategory(ProductCategoryDto productCategoryDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var productCategory = new ProductCategory
                    {
                        Name = productCategoryDto.Name
                    };

                    await _productCategoryRepository.AddAsync(productCategory);

                    return CreatedAtAction(nameof(GetProductCategory), new { id = productCategory.Id }, productCategoryDto);

                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException?.Message.Contains("UNIQUE constraint failed") ?? false)
                    {
                        ModelState.AddModelError("UniqueProperty", "The value must be unique.");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateProductCategory([FromRoute] int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = new ProductCategory
            {
                Id = id,
                Name = updateCategoryDto.Name,
            };

            await _productCategoryRepository.UpdateAsync(category);

            if (category == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new ProductCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            await _productCategoryRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
