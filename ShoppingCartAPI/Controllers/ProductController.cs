using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.DTO;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Repository;

namespace ShoppingCartAPI.Repositories
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductsController(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            var response = new List<ProductDto>();
            foreach (var product in products)
            {
                response.Add(new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    FeaturedImageUrl = product.FeaturedImageUrl,
                    Categories = product.Categories.Select(x => new ProductCategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name

                    }).ToList()
                });
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var response = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                Price = product.Price,
                FeaturedImageUrl = product.FeaturedImageUrl,
                Categories = product.Categories.Select(x => new ProductCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name

                }).ToList()
            };

            return Ok(response);
        }



        [HttpGet("searchByCategory/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(category);
            if (!products.Any())
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                ShortDescription = productDto.ShortDescription,
                Price = productDto.Price,
                FeaturedImageUrl = productDto.FeaturedImageUrl,
                Categories = new List<ProductCategory>()
            };

            foreach (var item in productDto.Categories)
            {
                var existingCategory = await _productCategoryRepository.GetByIdAsync(item);
                if (existingCategory != null)
                {
                    product.Categories.Add(existingCategory);
                }
            }

            product = await _productRepository.AddAsync(product);

            var response = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                Price = product.Price,
                FeaturedImageUrl = product.FeaturedImageUrl,
                Categories = product.Categories.Select(x => new ProductCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name

                }).ToList()
            };

            return Ok(response);

        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto productDto)
        {
            var product = new Product
            {
                Id = id,
                Name = productDto.Name,
                ShortDescription = productDto.ShortDescription,
                Price = productDto.Price,
                FeaturedImageUrl = productDto.FeaturedImageUrl,
                Categories = new List<ProductCategory>()
            };

            foreach (var item in productDto.Categories)
            {
                var existingCategory = await _productCategoryRepository.GetByIdAsync(item);
                if (existingCategory != null)
                {
                    product.Categories.Add(existingCategory);
                }
            }

            var updatedProduct = await _productRepository.UpdateAsync(product);

            if (updatedProduct == null)
            {
                return NotFound();
            }

            var response = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                Price = product.Price,
                FeaturedImageUrl = product.FeaturedImageUrl,
                Categories = product.Categories.Select(x => new ProductCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name

                }).ToList()
            };

            return Ok(response);


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleteProduct = await _productRepository.DeleteAsync(id);
            if (deleteProduct == null)
            {
                return NotFound();
            }

            var response = new ProductDto
            {
                Id = deleteProduct.Id,
                Name = deleteProduct.Name,
                ShortDescription = deleteProduct.ShortDescription,
                Price = deleteProduct.Price,
                FeaturedImageUrl = deleteProduct.FeaturedImageUrl,
            };

            return Ok(response);
        }
    }

}
